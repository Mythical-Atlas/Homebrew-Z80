using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Z80.C.Compiler.Data;

namespace Z80.C.Compiler {
	static class Compiler {
		public static byte[] rom = new byte[8 * 1024];

		static List<Symbol> symbols = new List<Symbol>();

		static int pointerIndex = 0;
		static int pc = 0;

		static List<int[]> ifJumpReferences = new List<int[]>();
		static List<int[]> subroutineReferences = new List<int[]>();

		enum Subroutine {
			Compare,
			NegateHL,
			NegateBC,
			Multiply
		}

		public static byte[] Compile(List<Statement> statements) {
			for(int s = 0; s < statements.Count; s++) {
				if(statements[s].type == StatementType.Definition) {
					symbols.Add(new Symbol(statements[s].tokens[0].value, statements[s].tokens[1].value, pointerIndex));
					pointerIndex += 2;
				}
				if(statements[s].type == StatementType.If) {
					// load operand into hl
					StoreLoadCommandInROM(statements[s].tokens[0], false, RegIndex.HL);

					// check if operand is equal to 0
					// load l into a
					rom[pc++] = 0x7D;
					// load 0 into c
					rom[pc++] = 0x0E;
					rom[pc++] = 0x00;
					// xor a with c - zero mean equal, nonzero mean not equal
					rom[pc++] = 0xA9;

					// if zero, jump to next occurence of level
					// jump z to 0xFFFF (placeholder)
					rom[pc++] = 0xCA;
					rom[pc++] = 0xFF;
					rom[pc++] = 0xFF;

					// add address and level + 1 of jump operand to ifJumpReferences
					ifJumpReferences.Add(new int[]{pc - 2, statements[s].level + 1});
				}
				if(statements[s].type == StatementType.ClosingBracket) {
					// find previous reference to current level and replace it with pc
					for(int i = 0; i < ifJumpReferences.Count; i++) {
						if(ifJumpReferences[i][1] == statements[s].level) {
							rom[ifJumpReferences[i][0]] = (byte)(pc & 0xFF);
							rom[ifJumpReferences[i][0] + 1] = (byte)((pc >> 8) & 0xFF);

							ifJumpReferences.RemoveAt(i);
							break;
						}
					}
				}
				if(statements[s].type == StatementType.Assignment) {
					for(int i = 0; i < symbols.Count; i++) {
						if(symbols[i].identifier == statements[s].tokens[0].value) {
							// load operand into hl
							StoreLoadCommandInROM(statements[s].tokens[statements[s].tokens.Count - 1], CheckNegative(statements[s].tokens), RegIndex.HL);

							// store hl to (var pointer)
							rom[pc++] = 0x22;
							StoreShortInROM((short)(symbols[i].pointer + 0x2000));

							break;
						}
					}
				}
				if(statements[s].type == StatementType.Addition) {
					// load right operand into bc
					StoreLoadCommandInROM(statements[s].tokens[2], false, RegIndex.BC);
					// load left operand into hl
					StoreLoadCommandInROM(statements[s].tokens[0], false, RegIndex.HL);

					// add bc to hl
					rom[pc++] = 0x09;
					// push value in hl to stack
					rom[pc++] = 0xE5;
				}
				if(statements[s].type == StatementType.Subtraction) {
					// load right operand into bc
					StoreLoadCommandInROM(statements[s].tokens[2], false, RegIndex.BC);
					// load left operand into hl
					StoreLoadCommandInROM(statements[s].tokens[0], false, RegIndex.HL);

					// reset carry flag (must be set then negated)
					rom[pc++] = 0x37;
					rom[pc++] = 0x3F;
					// sbc bc from hl
					rom[pc++] = 0xED;
					rom[pc++] = 0x42;
					// push value in hl to stack
					rom[pc++] = 0xE5;
				}
				if(statements[s].type == StatementType.Multiplication) {
					// load right operand into bc
					StoreLoadCommandInROM(statements[s].tokens[2], false, RegIndex.BC);
					// load left operand into hl
					StoreLoadCommandInROM(statements[s].tokens[0], false, RegIndex.HL);

					// call subroutine
					rom[pc++] = 0xCD;
					rom[pc++] = 0xFF;
					rom[pc++] = 0xFF;
					subroutineReferences.Add(new int[]{pc - 2, (int)Subroutine.Multiply});

					// if equal, pushes 1 to stack
					// if not equal, pushes 0 to stack
				}
				if(statements[s].type == StatementType.Comparison) {
					// load right operand into bc
					StoreLoadCommandInROM(statements[s].tokens[2], false, RegIndex.BC);
					// load left operand into hl
					StoreLoadCommandInROM(statements[s].tokens[0], false, RegIndex.HL);

					// call comparison subroutine
					rom[pc++] = 0xCD;
					rom[pc++] = 0xFF; // placeholder
					rom[pc++] = 0xFF;
					subroutineReferences.Add(new int[]{pc - 2, (int)Subroutine.Compare});

					// if equal, pushes 1 to stack
					// if not equal, pushes 0 to stack
				}
			}

			// halt
			rom[pc++] = 0x76;

			AddSubroutinesToROM();

			return(rom);
		}

		static void StoreLoadCommandInROM(Token token, bool negate, RegIndex register) {
			if(token.type == TokenType.Number) {
				// load immediate value into __
				if(register == RegIndex.HL) {rom[pc++] = 0x21;}
				if(register == RegIndex.BC) {rom[pc++] = 0x01;}

				if(!negate) {StoreShortInROM((short)ConvertToInt(token.value));}
				else {StoreShortInROM(FlipSign((short)ConvertToInt(token.value)));}
			}
			if(token.type == TokenType.StatementID) {
				// pop value from stack into __
				if(register == RegIndex.HL) {rom[pc++] = 0xE1;}
				if(register == RegIndex.BC) {rom[pc++] = 0xC1;}
			}
			if(token.type == TokenType.Symbol) {
				for(int x = 0; x < symbols.Count; x++) {
					if(symbols[x].identifier == token.value) {
						// load (symbol pointer) into __
						if(register == RegIndex.HL) {rom[pc++] = 0x2A;}
						if(register == RegIndex.BC) {rom[pc++] = 0xED; rom[pc++] = 0x4B;}

						StoreShortInROM((short)(symbols[x].pointer + 0x2000));

						if(negate) {
							if(register == RegIndex.HL) {
								rom[pc++] = 0xCD;
								rom[pc++] = 0xFF;
								rom[pc++] = 0xFF;
								subroutineReferences.Add(new int[]{pc - 2, (int)Subroutine.NegateHL});
							}
							if(register == RegIndex.BC) {
								rom[pc++] = 0xCD;
								rom[pc++] = 0xFF;
								rom[pc++] = 0xFF;
								subroutineReferences.Add(new int[]{pc - 2, (int)Subroutine.NegateBC});
							}
						}
					}
				}
			}
		}

		static void AddSubroutinesToROM() {
			for(int i = 0; i < subroutineReferences.Count; i++) {
				if(subroutineReferences[i][1] == (int)Subroutine.Compare) {
					rom[subroutineReferences[i][0]] = (byte)(pc & 0xFF);
					rom[subroutineReferences[i][0] + 1] = (byte)((pc >> 8) & 0xFF);
				}
			}

			/*
			compare subroutine
			takes hl and bc as operands
			pushes result (0 or 1) to stack
			 */
			//   load h into a
			rom[pc++] = 0x7C;
			//   xor a with b - zero mean equal, nonzero mean not equal
			rom[pc++] = 0xA8;
			// 2 jr nz +6 - if nonzero (not equal), jump to loading 0 into de
			rom[pc++] = 0x20;
			rom[pc++] = 6 - 2;

			// 1 load l into a
			rom[pc++] = 0x7D;
			// 1 xor a with c - zero mean equal, nonzero mean not equal
			rom[pc++] = 0xA9;
			// 2 jr z +7 - if zero (equal), jump to loading 1 into de
			rom[pc++] = 0x28;
			rom[pc++] = 7 - 2;

			// 3 load 0 into de - not equal
			rom[pc++] = 0x11;
			rom[pc++] = 0x00;
			rom[pc++] = 0x00;
			// 2 jr +5 - to push de
			rom[pc++] = 0x18;
			rom[pc++] = 5 - 2;
			// 3 load 1 into de - equal
			rom[pc++] = 0x11;
			rom[pc++] = 0x01;
			rom[pc++] = 0x00;
			//   push de
			rom[pc++] = 0xD5;

			// return from subroutine call
			rom[pc++] = 0xC9;

			// if equal, pushes 1 to stack
			// if not equal, pushes 0 to stack

			for(int i = 0; i < subroutineReferences.Count; i++) {
				if(subroutineReferences[i][1] == (int)Subroutine.NegateHL) {
					rom[subroutineReferences[i][0]] = (byte)(pc & 0xFF);
					rom[subroutineReferences[i][0] + 1] = (byte)((pc >> 8) & 0xFF);
				}
			}

			/*
			negate hl subroutine
			takes hl as operand
			uses a
			returns -hl
			*/
				// xor h
			// load 0xFF into a
			rom[pc++] = 0x3E;
			rom[pc++] = 0xFF;
			// xor h with a
			rom[pc++] = 0xAC;
			// load a into h
			rom[pc++] = 0x67;

				// xor l
			// load 0xFF into a
			rom[pc++] = 0x3E;
			rom[pc++] = 0xFF;
			// xor l with a
			rom[pc++] = 0xAD;

				// add 1 to l
			// add 1 to a
			rom[pc++] = 0xC6;
			rom[pc++] = 0x01;
			// load a into l
			rom[pc++] = 0x6F;

				// add carry bit to h
			// load h into a
			rom[pc++] = 0x7C;
			// load 0 into h
			rom[pc++] = 0x26;
			rom[pc++] = 0x00;
			// adc h to a
			rom[pc++] = 0x8C;
			// load a into h
			rom[pc++] = 0x67;

			// return from subroutine call
			rom[pc++] = 0xC9;

			for(int i = 0; i < subroutineReferences.Count; i++) {
				if(subroutineReferences[i][1] == (int)Subroutine.NegateBC) {
					rom[subroutineReferences[i][0]] = (byte)(pc & 0xFF);
					rom[subroutineReferences[i][0] + 1] = (byte)((pc >> 8) & 0xFF);
				}
			}

			/*
			negate bc subroutine
			takes bc as operand
			uses a
			returns -bc
			*/
				// xor b
			// load 0xFF into a
			rom[pc++] = 0x3E;
			rom[pc++] = 0xFF;
			// xor b with a
			rom[pc++] = 0xA8;
			// load a into b
			rom[pc++] = 0x47;

				// xor c
			// load 0xFF into a
			rom[pc++] = 0x3E;
			rom[pc++] = 0xFF;
			// xor c with a
			rom[pc++] = 0xA9;

				// add 1 to c
			// add 1 to a
			rom[pc++] = 0xC6;
			rom[pc++] = 0x01;
			// load a into c
			rom[pc++] = 0x4F;

				// add carry bit to b
			// load b into a
			rom[pc++] = 0x75;
			// load 0 into b
			rom[pc++] = 0x06;
			rom[pc++] = 0x00;
			// adc b to a
			rom[pc++] = 0x88;
			// load a into b
			rom[pc++] = 0x47;

			// return from subroutine call
			rom[pc++] = 0xC9;

			for(int i = 0; i < subroutineReferences.Count; i++) {
				if(subroutineReferences[i][1] == (int)Subroutine.Multiply) {
					rom[subroutineReferences[i][0]] = (byte)(pc & 0xFF);
					rom[subroutineReferences[i][0] + 1] = (byte)((pc >> 8) & 0xFF);
				}
			}

			/*
			multiply c and l

			takes l and c as operands
			uses a, b, h, and de

			returns (de = l * c) pushed on to stack
			*/
			
			// load 0 into de
			rom[pc++] = 0x11;
			rom[pc++] = 0x00;
			rom[pc++] = 0x00;
			// load 0 into h
			rom[pc++] = 0x26;
			rom[pc++] = 0x00;
			// load 8 into b
			rom[pc++] = 0x06;
			rom[pc++] = 0x08;

			// rotate c to right by one bit, and copy old bit 0 to new bit 7 and carry //rrc c
			rom[pc++] = 0xCB;
			rom[pc++] = 0x09;
			// relative jump +8 if nc to (shift l left)
			rom[pc++] = 0x30;
			rom[pc++] = 0x06; // 8 - 2
			// load e into a
			rom[pc++] = 0x7D;
			// add l to a
			rom[pc++] = 0x85;
			// load a into e
			rom[pc++] = 0x5F;
			// load d into a
			rom[pc++] = 0x7A;
			// adc h to a
			rom[pc++] = 0x8C;
			// load a into d
			rom[pc++] = 0x87;

			// shift l to the left by one, copy old bit 7 into carry, and make new bit 0 a 0 // sla l
			rom[pc++] = 0xCB;
			rom[pc++] = 0x25;
			// shift h to the left by one, copy old bit 7 into carry, and make new bit 0 a 0 // sla h
			rom[pc++] = 0xCB;
			rom[pc++] = 0x24;
			// decrement b
			rom[pc++] = 0x05;
			// relative jump -15 if not zero to (rotate c)
			rom[pc++] = 0x20;
			rom[pc++] = 0xEF; // -15 - 2

			// return from subroutine call
			rom[pc++] = 0xC9;
		}

		static bool CheckNegative(List<Token> tokens) {
			bool output = false;
			foreach(Token t in tokens) {if(t.type == TokenType.Negative) {output = !output;}}
			return(output);
		}

		static int ConvertToInt(string input) {
			if(input.StartsWith("0x")) {
				try {
					int temp = Convert.ToInt32(input, 16);
					return(temp);
				}
				catch(Exception e) {return(0);}
			}
			else {
				try {
					int temp = Convert.ToInt32(input);
					return(temp);
				}
				catch(Exception e) {return(0);}
			}
		}
		static void StoreShortInROM(short value) {
			rom[pc++] = (byte)(value & 0xFF);
			rom[pc++] = (byte)((value >> 8) & 0xFF);
		}

		static short FlipSign(short input) {return((short)((input ^ 0xFFFF) + 1));}
	}
}
