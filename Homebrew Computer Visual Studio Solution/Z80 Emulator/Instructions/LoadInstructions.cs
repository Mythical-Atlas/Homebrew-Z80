using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Z80.Emulator.CPU;
using static Z80.Emulator.MemoryAndRegisters;

namespace Z80.Emulator.Instructions {
	public static class LoadInstructions {
		public static bool CheckLoadInstructions() {
			byte pcByte0 = GetByte(GetRegUShort(RegIndex.PC));
			byte pcByte1 = GetByte((ushort)(GetRegUShort(RegIndex.PC) + 1));
			ushort pcUShort1 = GetUShort((ushort)(GetRegUShort(RegIndex.PC) + 1));
			ushort pcUShort2 = GetUShort((ushort)(GetRegUShort(RegIndex.PC) + 2));

			switch(pcByte0) {
				case(0x01): { // load immediate ushort into bc
					SetRegUShort(RegIndex.BC, pcUShort1);
					Console.WriteLine("DEBUG: 0x" + pcUShort1.ToString("X4"));
					return(true);
				}
				case(0x02): { // store a to indirect byte (bc)
					SetByte(GetRegUShort(RegIndex.BC), GetRegByte(RegIndex.A));
					return(true);
				}
				case(0x06): { // load immediate byte into b
					SetRegByte(RegIndex.B, pcByte1);
					return(true);
				}
				case(0x0A): { // load indirect byte (bc) into a
					SetRegByte(RegIndex.A, GetByte(GetRegUShort(RegIndex.BC)));
					return(true);
				}
				case(0x0E): { // load immediate byte into c
					SetRegByte(RegIndex.C, pcByte1);
					return(true);
				}
				case(0x11): { // load immediate ushort into de
					SetRegUShort(RegIndex.DE, pcUShort1);
					return(true);
				}
				case(0x12): { // store a to indirect byte (de)
					SetByte(GetRegUShort(RegIndex.DE), GetRegByte(RegIndex.A));
					return(true);
				}
				case(0x16): { // load immediate byte into d
					SetRegByte(RegIndex.D, pcByte1);
					return(true);
				}
				case(0x1A): { // load indirect byte (de) into a
					SetRegByte(RegIndex.A, GetByte(GetRegUShort(RegIndex.DE)));
					return(true);
				}
				case(0x1E): { // load immediate byte into e
					SetRegByte(RegIndex.E, pcByte1);
					return(true);
				}
				case(0x21): { // load immediate ushort into hl
					SetRegUShort(RegIndex.HL, pcUShort1);
					return(true);
				}
				case(0x22): { // store hl to indirect byte (immediate ushort)
					SetUShort(pcUShort1, GetRegUShort(RegIndex.HL));
					return(true);
				}
				case(0x26): { // load immediate byte into h
					SetRegByte(RegIndex.H, pcByte1);
					return(true);
				}
				case(0x2A): { // load indirect ushort (immediate ushort) into hl
					SetRegUShort(RegIndex.HL, GetUShort(pcUShort1));
					return(true);
				}
				case(0x2E): { // load immediate byte into l
					SetRegByte(RegIndex.L, pcByte1);
					return(true);
				}
				case(0x31): { // load immediate ushort into sp
					SetRegUShort(RegIndex.SP, pcUShort1);
					return(true);
				}
				case(0x32): { // store a to indirect byte (immediate ushort)
					SetByte(pcUShort1, GetRegByte(RegIndex.A));
					return(true);
				}
				case(0x36): { // store immediate byte to indirect byte (hl)
					SetByte(GetRegUShort(RegIndex.HL), pcByte1);
					return(true);
				}
				case(0x3A): { // load indirect byte (immediate ushort) into a
					SetRegByte(RegIndex.A, GetByte(pcUShort1));
					return(true);
				}
				case(0x3E): { // load immediate byte into a
					SetRegByte(RegIndex.A, pcByte1);
					return(true);
				}
				case(0x40): {return(true);} // load b into b
				case(0x41): { // load c into b
					SetRegByte(RegIndex.B, GetRegByte(RegIndex.C));
					return(true);
				}
				case(0x42): { // load d into b
					SetRegByte(RegIndex.B, GetRegByte(RegIndex.D));
					return(true);
				}
				case(0x43): { // load e into b
					SetRegByte(RegIndex.B, GetRegByte(RegIndex.E));
					return(true);
				}
				case(0x44): { // load h into b
					SetRegByte(RegIndex.B, GetRegByte(RegIndex.H));
					return(true);
				}
				case(0x45): { // load l into b
					SetRegByte(RegIndex.B, GetRegByte(RegIndex.L));
					return(true);
				}
				case(0x46): { // load indirect byte (hl) into b
					SetRegByte(RegIndex.B, GetByte(GetRegUShort(RegIndex.HL)));
					return(true);
				}
				case(0x47): { // load a into b
					SetRegByte(RegIndex.B, GetRegByte(RegIndex.A));
					return(true);
				}
				case(0x48): { // load b into c
					SetRegByte(RegIndex.C, GetRegByte(RegIndex.B));
					return(true);
				}
				case(0x49): {return(true);} // load c into c
				case(0x4A): { // load d into c
					SetRegByte(RegIndex.C, GetRegByte(RegIndex.D));
					return(true);
				}
				case(0x4B): { // load e into c
					SetRegByte(RegIndex.C, GetRegByte(RegIndex.E));
					return(true);
				}
				case(0x4C): { // load h into c
					SetRegByte(RegIndex.C, GetRegByte(RegIndex.H));
					return(true);
				}
				case(0x4D): { // load l into c
					SetRegByte(RegIndex.C, GetRegByte(RegIndex.L));
					return(true);
				}
				case(0x4E): { // load indirect byte (hl) into c
					SetRegByte(RegIndex.C, GetByte(GetRegUShort(RegIndex.HL)));
					return(true);
				}
				case(0x4F): { // load a into c
					SetRegByte(RegIndex.C, GetRegByte(RegIndex.A));
					return(true);
				}
				case(0x50): { // load b into d
					SetRegByte(RegIndex.D, GetRegByte(RegIndex.B));
					return(true);
				}
				case(0x51): { // load c into d
					SetRegByte(RegIndex.D, GetRegByte(RegIndex.C));
					return(true);
				}
				case(0x52): {return(true);} // load d into d
				case(0x53): { // load e into d
					SetRegByte(RegIndex.D, GetRegByte(RegIndex.E));
					return(true);
				}
				case(0x54): { // load h into d
					SetRegByte(RegIndex.D, GetRegByte(RegIndex.H));
					return(true);
				}
				case(0x55): { // load l into d
					SetRegByte(RegIndex.D, GetRegByte(RegIndex.L));
					return(true);
				}
				case(0x56): { // load indirect byte (hl) into d
					SetRegByte(RegIndex.D, GetByte(GetRegUShort(RegIndex.HL)));
					return(true);
				}
				case(0x57): { // load a into d
					SetRegByte(RegIndex.D, GetRegByte(RegIndex.A));
					return(true);
				}
				case(0x58): { // load b into e
					SetRegByte(RegIndex.E, GetRegByte(RegIndex.B));
					return(true);
				}
				case(0x59): { // load c into e
					SetRegByte(RegIndex.E, GetRegByte(RegIndex.C));
					return(true);
				}
				case(0x5A): { // load d into e
					SetRegByte(RegIndex.E, GetRegByte(RegIndex.D));
					return(true);
				}
				case(0x5B): {return(true);} // load e into e
				case(0x5C): { // load h into e
					SetRegByte(RegIndex.E, GetRegByte(RegIndex.H));
					return(true);
				}
				case(0x5D): { // load l into e
					SetRegByte(RegIndex.E, GetRegByte(RegIndex.L));
					return(true);
				}
				case(0x5E): { // load indirect byte (hl) into e
					SetRegByte(RegIndex.E, GetByte(GetRegUShort(RegIndex.HL)));
					return(true);
				}
				case(0x5F): { // load a into e
					SetRegByte(RegIndex.E, GetRegByte(RegIndex.A));
					return(true);
				}
				case(0x60): { // load b into h
					SetRegByte(RegIndex.H, GetRegByte(RegIndex.B));
					return(true);
				}
				case(0x61): { // load c into h
					SetRegByte(RegIndex.H, GetRegByte(RegIndex.C));
					return(true);
				}
				case(0x62): { // load d into h
					SetRegByte(RegIndex.H, GetRegByte(RegIndex.D));
					return(true);
				}
				case(0x63): { // load e into h
					SetRegByte(RegIndex.H, GetRegByte(RegIndex.E));
					return(true);
				}
				case(0x64): {return(true);} // load h into h
				case(0x65): { // load l into h
					SetRegByte(RegIndex.H, GetRegByte(RegIndex.L));
					return(true);
				}
				case(0x66): { // load indirect byte (hl) into h
					SetRegByte(RegIndex.H, GetByte(GetRegUShort(RegIndex.HL)));
					return(true);
				}
				case(0x67): { // load a into h
					SetRegByte(RegIndex.H, GetRegByte(RegIndex.A));
					return(true);
				}
				case(0x68): { // load b into l
					SetRegByte(RegIndex.L, GetRegByte(RegIndex.B));
					return(true);
				}
				case(0x69): { // load c into l
					SetRegByte(RegIndex.L, GetRegByte(RegIndex.C));
					return(true);
				}
				case(0x6A): { // load d into l
					SetRegByte(RegIndex.L, GetRegByte(RegIndex.D));
					return(true);
				}
				case(0x6B): { // load e into l
					SetRegByte(RegIndex.L, GetRegByte(RegIndex.E));
					return(true);
				}
				case(0x6C): { // load h into l
					SetRegByte(RegIndex.L, GetRegByte(RegIndex.H));
					return(true);
				}
				case(0x6D): {return(true);} // load l into l
				case(0x6E): { // load indirect byte (hl) into l
					SetRegByte(RegIndex.L, GetByte(GetRegUShort(RegIndex.HL)));
					return(true);
				}
				case(0x6F): { // load a into l
					SetRegByte(RegIndex.L, GetRegByte(RegIndex.A));
					return(true);
				}
				case(0x70): { // store b to indirect byte (hl)
					SetByte(GetRegUShort(RegIndex.HL), GetRegByte(RegIndex.B));
					return(true);
				}
				case(0x71): { // store c to indirect byte (hl)
					SetByte(GetRegUShort(RegIndex.HL), GetRegByte(RegIndex.C));
					return(true);
				}
				case(0x72): { // store d to indirect byte (hl)
					SetByte(GetRegUShort(RegIndex.HL), GetRegByte(RegIndex.D));
					return(true);
				}
				case(0x73): { // store e to indirect byte (hl)
					SetByte(GetRegUShort(RegIndex.HL), GetRegByte(RegIndex.E));
					return(true);
				}
				case(0x74): { // store h to indirect byte (hl)
					SetByte(GetRegUShort(RegIndex.HL), GetRegByte(RegIndex.H));
					return(true);
				}
				case(0x75): { // store l to indirect byte (hl)
					SetByte(GetRegUShort(RegIndex.HL), GetRegByte(RegIndex.L));
					return(true);
				}
				case(0x77): { // store a to indirect byte (hl)
					SetByte(GetRegUShort(RegIndex.HL), GetRegByte(RegIndex.A));
					return(true);
				}
				case(0x78): { // load b into a
					SetRegByte(RegIndex.A, GetRegByte(RegIndex.B));
					return(true);
				}
				case(0x79): { // load c into a
					SetRegByte(RegIndex.A, GetRegByte(RegIndex.C));
					return(true);
				}
				case(0x7A): { // load d into a
					SetRegByte(RegIndex.A, GetRegByte(RegIndex.D));
					return(true);
				}
				case(0x7B): { // load e into a
					SetRegByte(RegIndex.A, GetRegByte(RegIndex.E));
					return(true);
				}
				case(0x7C): { // load h into a
					SetRegByte(RegIndex.A, GetRegByte(RegIndex.H));
					return(true);
				}
				case(0x7D): { // load l into a
					SetRegByte(RegIndex.A, GetRegByte(RegIndex.L));
					return(true);
				}
				case(0x7E): { // load indirect byte (hl) into a
					SetRegByte(RegIndex.A, GetByte(GetRegUShort(RegIndex.HL)));
					return(true);
				}
				case(0x7F): {return(true);} // load a into a
				case(0xD5): { // push de
					PushToStack(GetRegUShort(RegIndex.DE));
					return(true);
				}
				case(0xE1): { // pop hl
					SetRegUShort(RegIndex.HL, PopUShort());
					return(true);
				}
				case(0xE5): { // push hl
					PushToStack(GetRegUShort(RegIndex.HL));
					return(true);
				}
				case(0xED): { // extended instructions
					switch(pcByte1) {
						case(0x4B): { // load indirect ushort (immediate ushort) into bc
							SetRegUShort(RegIndex.BC, GetUShort(pcUShort2));
							return(true);
						}
					}
					break;
				}
			}

			return(false);
		}
	}
}
