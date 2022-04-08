using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Z80.Emulator.CPU;
using static Z80.Emulator.MemoryAndRegisters;
using static Z80.Emulator.Operations;

namespace Z80.Emulator.Instructions {
	public static class AddInstructions {
		public static bool CheckAddInstructions() {
			byte pcByte0 = GetByte(GetRegUShort(RegIndex.PC));
			byte pcByte1 = GetByte((ushort)(GetRegUShort(RegIndex.PC) + 1));
			ushort pcUShort1 = GetUShort((ushort)(GetRegUShort(RegIndex.PC) + 1));

			switch(pcByte0) {
				case(0x03): { // increment bc
					SetRegUShort(RegIndex.BC, (ushort)(GetRegUShort(RegIndex.BC) + 1));
					return(true);
				}
				case(0x04): { // increment b
					SetRegSByte(RegIndex.B, AddSBytes(GetRegSByte(RegIndex.B), 1));
					return(true);
				}
				case(0x09): { // add bc to hl
					SetRegShort(RegIndex.HL, AddShorts(GetRegShort(RegIndex.HL), GetRegShort(RegIndex.BC)));
					return(true);
				}
				case(0x0C): { // increment c
					SetRegSByte(RegIndex.C, AddSBytes(GetRegSByte(RegIndex.C), 1));
					return(true);
				}
				case(0x13): { // increment de
					SetRegUShort(RegIndex.DE, (ushort)(GetRegUShort(RegIndex.DE) + 1));
					return(true);
				}
				case(0x14): { // increment d
					SetRegSByte(RegIndex.D, AddSBytes(GetRegSByte(RegIndex.D), 1));
					return(true);
				}
				case(0x19): { // add de to hl
					SetRegShort(RegIndex.HL, AddShorts(GetRegShort(RegIndex.HL), GetRegShort(RegIndex.DE)));
					return(true);
				}
				case(0x1C): { // increment e
					SetRegSByte(RegIndex.E, AddSBytes(GetRegSByte(RegIndex.E), 1));
					return(true);
				}
				case(0x23): { // increment hl
					SetRegUShort(RegIndex.HL, (ushort)(GetRegUShort(RegIndex.HL) + 1));
					return(true);
				}
				case(0x24): { // increment h
					SetRegSByte(RegIndex.H, AddSBytes(GetRegSByte(RegIndex.H), 1));
					return(true);
				}
				case(0x29): { // add hl to hl
					SetRegShort(RegIndex.HL, AddShorts(GetRegShort(RegIndex.HL), GetRegShort(RegIndex.HL)));
					return(true);
				}
				case(0x2C): { // increment l
					SetRegSByte(RegIndex.L, AddSBytes(GetRegSByte(RegIndex.L), 1));
					return(true);
				}
				case(0x33): { // increment sp
					SetRegUShort(RegIndex.SP, (ushort)(GetRegUShort(RegIndex.SP) + 1));
					return(true);
				}
				case(0x34): { // increment indirect byte (hl)
					SetSByte(GetRegUShort(RegIndex.HL), AddSBytes(GetSByte(GetRegUShort(RegIndex.HL)), 1));
					return(true);
				}
				case(0x39): { // add sp to hl
					SetRegShort(RegIndex.HL, AddShorts(GetRegShort(RegIndex.HL), GetRegShort(RegIndex.SP)));
					return(true);
				}
				case(0x3C): { // increment a
					SetRegSByte(RegIndex.A, AddSBytes(GetRegSByte(RegIndex.A), 1));
					return(true);
				}
				case(0x80): { // add b to a
					SetRegSByte(RegIndex.A, AddSBytes(GetRegSByte(RegIndex.A), GetRegSByte(RegIndex.B)));
					return(true);
				}
				case(0x81): { // add c to a
					SetRegSByte(RegIndex.A, AddSBytes(GetRegSByte(RegIndex.A), GetRegSByte(RegIndex.C)));
					return(true);
				}
				case(0x82): { // add d to a
					SetRegSByte(RegIndex.A, AddSBytes(GetRegSByte(RegIndex.A), GetRegSByte(RegIndex.D)));
					return(true);
				}
				case(0x83): { // add e to a
					SetRegSByte(RegIndex.A, AddSBytes(GetRegSByte(RegIndex.A), GetRegSByte(RegIndex.E)));
					return(true);
				}
				case(0x84): { // add h to a
					SetRegSByte(RegIndex.A, AddSBytes(GetRegSByte(RegIndex.A), GetRegSByte(RegIndex.H)));
					return(true);
				}
				case(0x85): { // add l to a
					SetRegSByte(RegIndex.A, AddSBytes(GetRegSByte(RegIndex.A), GetRegSByte(RegIndex.L)));
					return(true);
				}
				case(0x86): { // add indirect byte (hl) to a
					SetRegSByte(RegIndex.A, AddSBytes(GetRegSByte(RegIndex.A), GetSByte(GetRegUShort(RegIndex.HL))));
					return(true);
				}
				case(0x87): { // add a to a
					SetRegSByte(RegIndex.A, AddSBytes(GetRegSByte(RegIndex.A), GetRegSByte(RegIndex.A)));
					return(true);
				}
				case(0xC6): { // add immediate byte to a
					SetRegSByte(RegIndex.A, AddSBytes(GetRegSByte(RegIndex.A), SignByte(pcByte1)));
					return(true);
				}
			}

			return(false);
		}
	}
}
