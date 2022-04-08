using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Z80.Emulator.CPU;
using static Z80.Emulator.MemoryAndRegisters;
using static Z80.Emulator.Operations;

namespace Z80.Emulator.Instructions {
	public static class SubInstructions {
		public static bool CheckSubInstructions() {
			byte pcByte0 = GetByte(GetRegUShort(RegIndex.PC));
			byte pcByte1 = GetByte((ushort)(GetRegUShort(RegIndex.PC) + 1));
			ushort pcUShort1 = GetUShort((ushort)(GetRegUShort(RegIndex.PC) + 1));

			switch(pcByte0) {
				case(0x05): { // decrement b
					SetRegSByte(RegIndex.B, AddSBytes(GetRegSByte(RegIndex.B), -1));
					return(true);
				}
				case(0x0B): { // decrement bc
					SetRegShort(RegIndex.BC, (short)(GetRegShort(RegIndex.BC) - 1));
					return(true);
				}
				case(0x0D): { // decrement c
					SetRegSByte(RegIndex.C, AddSBytes(GetRegSByte(RegIndex.C), -1));
					return(true);
				}
				case(0x15): { // decrement d
					SetRegSByte(RegIndex.D, AddSBytes(GetRegSByte(RegIndex.D), -1));
					return(true);
				}
				case(0x1B): { // decrement de
					SetRegShort(RegIndex.DE, (short)(GetRegShort(RegIndex.DE) - 1));
					return(true);
				}
				case(0x1D): { // decrement e
					SetRegSByte(RegIndex.E, AddSBytes(GetRegSByte(RegIndex.E), -1));
					return(true);
				}
				case(0x25): { // decrement h
					SetRegSByte(RegIndex.H, AddSBytes(GetRegSByte(RegIndex.H), -1));
					return(true);
				}
				case(0x2B): { // decrement hl
					SetRegShort(RegIndex.HL, (short)(GetRegShort(RegIndex.HL) - 1));
					return(true);
				}
				case(0x2D): { // decrement l
					SetRegSByte(RegIndex.L, AddSBytes(GetRegSByte(RegIndex.L), -1));
					return(true);
				}
				case(0x35): { // decrement indirect byte (hl)
					SetSByte(GetRegUShort(RegIndex.HL), AddSBytes(GetSByte(GetRegUShort(RegIndex.HL)), -1));
					return(true);
				}
				case(0x3B): { // decrement sp
					SetRegShort(RegIndex.SP, (short)(GetRegShort(RegIndex.SP) - 1));
					return(true);
				}
				case(0x3D): { // decrement a
					SetRegSByte(RegIndex.A, AddSBytes(GetRegSByte(RegIndex.A), -1));
					return(true);
				}
				case(0x90): { // sub b from a
					SetRegSByte(RegIndex.A, AddSBytes(GetRegSByte(RegIndex.A), FlipSign(GetRegSByte(RegIndex.B))));
					return(true);
				}
				case(0x91): { // sub c from a
					SetRegSByte(RegIndex.A, AddSBytes(GetRegSByte(RegIndex.A), FlipSign(GetRegSByte(RegIndex.C))));
					return(true);
				}
				case(0xED): { // extended instructions
					switch(pcByte1) {
						case(0x42): { // sbc bc from hl
							SetRegShort(RegIndex.HL, AddShorts(GetRegShort(RegIndex.HL), FlipSign(GetRegShort(RegIndex.BC))));
							return(true);
						}
					}
					break;
				}
			}

			return(false);
		}

		static sbyte FlipSign(sbyte input) {return((sbyte)((input ^ 0xFF) + 1));}
		static short FlipSign(short input) {return((short)((input ^ 0xFFFF) + 1));}
	}
}
