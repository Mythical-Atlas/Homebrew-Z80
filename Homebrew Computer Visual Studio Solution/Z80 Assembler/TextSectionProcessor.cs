using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Z80.Assembler.Assembler;
using static Z80.Assembler.Lexer;
using static Z80.InstructionInfo;

namespace Z80.Assembler {
	static class TextSectionProcessor {
		public static byte UnsignByte(sbyte value) {return(BitConverter.GetBytes(value)[0]);}

		public static void LoopThroughText(List<Statement> statements) {
			for(int s = 0; s < statements.Count; s++) {
				if(statements[s].section == SectionType.Text) {
					string mnemonic = "";
					string operand1 = "";
					string operand2 = "";
					int numOperands = 0;
					string op1Label = "";
					string op2Label = "";

					for(int t = 0; t < statements[s].tokens.Count; t++) {
						if(symbolNames.Contains(statements[s].tokens[t].value)) {
							Console.WriteLine("Converted reference to symbol \"" + statements[s].tokens[t].value + "\" to value: " + symbolValues[symbolNames.IndexOf(statements[s].tokens[t].value)].ToString());
							statements[s].tokens[t] = new Token(statements[s].tokens[t].type, symbolValues[symbolNames.IndexOf(statements[s].tokens[t].value)].ToString());
						}
						else {
							if(statements[s].tokens[t].value.StartsWith('(') && statements[s].tokens[t].value.EndsWith(')')) {
								string indirTok = statements[s].tokens[t].value.Substring(1, statements[s].tokens[t].value.Length - 2);

								if(symbolNames.Contains(indirTok)) {
									Console.WriteLine("Converted reference to symbol \"" + indirTok + "\" to value: " + symbolValues[symbolNames.IndexOf(indirTok)].ToString());
									statements[s].tokens[t] = new Token(statements[s].tokens[t].type, symbolValues[symbolNames.IndexOf(indirTok)].ToString());
								}
							}
						}
						if(labelNames.Contains(statements[s].tokens[t].value) && statements[s].tokens[t].type == TokenType.Operand) {
							Console.WriteLine("Found reference to label: " + statements[s].tokens[t].value);
							statements[s].tokens[t] = new Token(TokenType.LabelReference, statements[s].tokens[t].value);
						}
						else {
							if(statements[s].tokens[t].value.StartsWith('(') && statements[s].tokens[t].value.EndsWith(')')) {
								string indirectToken = statements[s].tokens[t].value.Substring(1, statements[s].tokens[t].value.Length - 2);

								if(labelNames.Contains(indirectToken) && statements[s].tokens[t].type == TokenType.Operand) {
									Console.WriteLine("Found reference to label: " + indirectToken);
									statements[s].tokens[t] = new Token(TokenType.IndirectLabelReference, indirectToken);
								}
							}
						}

						if(statements[s].tokens[t].type == TokenType.Instruction) {mnemonic = statements[s].tokens[t].value;}
						if(statements[s].tokens[t].type == TokenType.Operand) {
							if(operand1 == "") {
								operand1 = statements[s].tokens[t].value;
								numOperands++;
							}
							else {
								operand2 = statements[s].tokens[t].value;
								numOperands++;
							}
						}
						if(statements[s].tokens[t].type == TokenType.LabelReference) {
							if(operand1 == "") {
								operand1 = "0";
								numOperands++;
								op1Label = statements[s].tokens[t].value;
							}
							else {
								operand2 = "0";
								numOperands++;
								op2Label = statements[s].tokens[t].value;
							}
						}
						if(statements[s].tokens[t].type == TokenType.IndirectLabelReference) {
							if(operand1 == "") {
								operand1 = "(0)";
								numOperands++;
								op1Label = statements[s].tokens[t].value;
							}
							else {
								operand2 = "(0)";
								numOperands++;
								op2Label = statements[s].tokens[t].value;
							}
						}
						if(statements[s].tokens[t].type == TokenType.Label) {
							for(int l = 0; l < labelNames.Count; l++) {
								if(labelNames[l] == statements[s].tokens[t].value) {
									labelIndices[l] = pc;
									Console.WriteLine("Assigned address 0x" + pc.ToString("X4") + " to data label: " + statements[s].tokens[t].value);
								}
							}
						}
						if(statements[s].tokens[t].type == TokenType.Unknown) {
							Console.Error.WriteLine("Compiler error: Unknown token found \"" + statements[s].tokens[t].value + "\"");
							Console.Error.WriteLine("Statement contains " + statements[s].tokens.Count + " token(s)");
							Environment.Exit(1);
						}
					}

					if(mnemonic != "" && Mnemonics.Contains(mnemonic)) {
						int instruction = -1;
						OperandType opType1 = GetOperandType(operand1);
						OperandType opType2 = GetOperandType(operand2);

						for(int i = 0; i < 0x100; i++) {if(Mnemonics[i] == mnemonic && CheckOperandTypesMatch(FirstOperandTypes[i], opType1) && CheckOperandTypesMatch(SecondOperandTypes[i], opType2)) {instruction = i;}}

						if(instruction != -1) {
							Console.Write("Recognized instruction: " + mnemonic);
							if(opType1 != OperandType.Non) {Console.Write(" " + operand1);}
							if(opType2 != OperandType.Non) {Console.Write(" " + operand2);}
							Console.WriteLine();

							Console.WriteLine("Code at 0x" + pc.ToString("X4") + ":");
							Console.Write("0x" + instruction.ToString("X2"));
							if(opType1 != OperandType.Non || opType2 != OperandType.Non) {Console.Write(" ");}

							rom[pc++] = (byte)instruction;

							if(FirstOperandTypes[instruction] == OperandType.ImB) {
								rom[pc++] = (byte)Parse(operand1);

								Console.Write(rom[pc - 1].ToString("X2"));
								if(opType2 != OperandType.Non) {Console.Write(" ");}

								if(op1Label != "") {
									labelReferenceNames.Add(op1Label);
									labelReferenceIndices.Add(pc - 1);
									Console.WriteLine("\nFound label reference as operand: " + op1Label + " at 0x" + (pc - 1).ToString("X4"));
								}
							}
							if(FirstOperandTypes[instruction] == OperandType.ImS) {
								rom[pc++] = (byte)(Parse(operand1) & 0xFF);
								Console.Write(rom[pc - 1].ToString("X2") + " ");
								rom[pc++] = (byte)((Parse(operand1) & 0xFF00) >> 8);
								Console.Write(rom[pc - 1].ToString("X2"));

								if(op1Label != "") {
									labelReferenceNames.Add(op1Label);
									labelReferenceIndices.Add(pc - 2);
									Console.WriteLine("\nFound label reference as operand: " + op1Label + " at 0x" + (pc - 2).ToString("X4"));
								}
							}
							if(FirstOperandTypes[instruction] == OperandType.IdB || FirstOperandTypes[instruction] == OperandType.IdS) {
								rom[pc++] = (byte)(Parse(operand1.Substring(1, operand1.Length - 2)) & 0xFF);
								Console.Write(rom[pc - 1].ToString("X2") + " ");
								rom[pc++] = (byte)((Parse(operand1.Substring(1, operand1.Length - 2)) & 0xFF00) >> 8);
								Console.Write(rom[pc - 1].ToString("X2"));

								if(op1Label != "") {
									labelReferenceNames.Add(op1Label);
									labelReferenceIndices.Add(pc - 2);
									Console.WriteLine("\nFound dlabel reference as operand: " + op1Label + " at 0x" + (pc - 2).ToString("X4"));
								}
							}
							if(SecondOperandTypes[instruction] == OperandType.ImB) {
								rom[pc++] = (byte)Parse(operand2);
								Console.Write(rom[pc - 1].ToString("X2"));

								if(op2Label != "") {
									labelReferenceNames.Add(op2Label);
									labelReferenceIndices.Add(pc - 1);
									Console.WriteLine("\nFound label reference as operand: " + op2Label + " at 0x" + (pc - 1).ToString("X4"));
								}
							}
							if(SecondOperandTypes[instruction] == OperandType.ImS) {
								rom[pc++] = (byte)(Parse(operand2) & 0xFF);
								Console.Write(rom[pc - 1].ToString("X2") + " ");
								rom[pc++] = (byte)((Parse(operand2) & 0xFF00) >> 8);
								Console.Write(rom[pc - 1].ToString("X2"));

								if(op2Label != "") {
									labelReferenceNames.Add(op2Label);
									labelReferenceIndices.Add(pc - 2);
									Console.WriteLine("\nFound label reference as operand: " + op2Label + " at 0x" + (pc - 2).ToString("X4"));
								}
							}
							if(SecondOperandTypes[instruction] == OperandType.IdB || SecondOperandTypes[instruction] == OperandType.IdS) {
								rom[pc++] = (byte)(Parse(operand2.Substring(1, operand2.Length - 2)) & 0xFF);
								Console.Write(rom[pc - 1].ToString("X2") + " ");
								rom[pc++] = (byte)((Parse(operand2.Substring(1, operand2.Length - 2)) & 0xFF00) >> 8);
								Console.Write(rom[pc - 1].ToString("X2"));

								if(op2Label != "") {
									labelReferenceNames.Add(op2Label);
									labelReferenceIndices.Add(pc - 2);
									Console.WriteLine("\nFound dlabel reference as operand: " + op2Label + " at 0x" + (pc - 2).ToString("X4"));
								}
							}
						}
						else {
							Console.WriteLine("Unknown instruction: " + mnemonic);
							if(numOperands == 0) {Console.WriteLine("Instruction has no operands");}
							else {
								Console.WriteLine("Instruction has " + numOperands + " operand(s)");
								if(operand1 != "") {Console.WriteLine("Operand 1, \"" + operand1 + "\": " + opType1);}
								if(operand2 != "") {Console.WriteLine("Operand 2, \"" + operand2 + "\": " + opType2);}
							}
						}

						Console.WriteLine();
					}
				}

				Console.WriteLine();
			}
		}
	}
}
