using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Z80.Emulator.Operations;

namespace Z80.Emulator {
	public static class MemoryAndRegisters {
		public enum RegIndex {PC, SP, A, B, C, D, E, H, L, Flags, A2, B2, C2, D2, E2, H2, L2, F2, BC, DE, HL, IX, IY};
		public enum FlagIndex {S, Z, H, P, N, C}

		static byte[] ram = new byte[8 * 1024];
		static byte[] rom = new byte[8 * 1024];

		static ushort pc = 0;
		static ushort sp = 0;

		static byte regA = 0;
		static byte regB = 0;
		static byte regC = 0;
		static byte regD = 0;
		static byte regE = 0;
		static byte regH = 0;
		static byte regL = 0;
		static byte flags = 0;

		static byte regA2 = 0;
		static byte regB2 = 0;
		static byte regC2 = 0;
		static byte regD2 = 0;
		static byte regE2 = 0;
		static byte regH2 = 0;
		static byte regL2 = 0;
		static byte regF2 = 0;

		static ushort ix = 0;
		static ushort iy = 0;

		public static void Reset() {
			ram = new byte[8 * 1024];

			pc = 0;
			sp = 0;

			regA = 0;
			regB = 0;
			regC = 0;
			regD = 0;
			regE = 0;
			regH = 0;
			regL = 0;
			flags = 0;

			regA2 = 0;
			regB2 = 0;
			regC2 = 0;
			regD2 = 0;
			regE2 = 0;
			regH2 = 0;
			regL2 = 0;
			regF2 = 0;

			ix = 0;
			iy = 0;
		}

		public static void LoadROM(byte[] rom) {MemoryAndRegisters.rom = rom;}

		public static sbyte GetSByte(ushort address) {return(SignByte(GetByte(address)));}
		public static byte GetByte(ushort address) {
			if(MathF.Floor(address / (8 * 1024)) % 2 == 0) {return(rom[address % (8 * 1024)]);}
			else {return(ram[address % (8 * 1024)]);}
		}
		public static ushort GetUShort(ushort address) {return((ushort)(GetByte(address) + (GetByte((ushort)(address + 1)) << 8)));}
		public static void SetSByte(ushort address, sbyte value) {SetByte(address, UnsignByte(value));}
		public static void SetByte(ushort address, byte value) {
			if(MathF.Floor(address / (8 * 1024)) % 2 == 0) {}
			else {ram[address % (8 * 1024)] = value;}
		}
		public static void SetUShort(ushort address, ushort value) {
			SetByte(address, (byte)(value & 0xFF));
			SetByte((ushort)(address + 1), (byte)((value >> 8) & 0xFF));
		}

		public static sbyte GetRegSByte(RegIndex register) {return(SignByte(GetRegByte(register)));}
		public static byte GetRegByte(RegIndex register) {
			if(register == RegIndex.A) {return(regA);}
			if(register == RegIndex.B) {return(regB);}
			if(register == RegIndex.C) {return(regC);}
			if(register == RegIndex.D) {return(regD);}
			if(register == RegIndex.E) {return(regE);}
			if(register == RegIndex.H) {return(regH);}
			if(register == RegIndex.L) {return(regL);}
			if(register == RegIndex.Flags) {return(flags);}
			if(register == RegIndex.A2) {return(regA2);}
			if(register == RegIndex.B2) {return(regB2);}
			if(register == RegIndex.C2) {return(regC2);}
			if(register == RegIndex.D2) {return(regD2);}
			if(register == RegIndex.E2) {return(regE2);}
			if(register == RegIndex.H2) {return(regH2);}
			if(register == RegIndex.L2) {return(regL2);}
			if(register == RegIndex.F2) {return(regF2);}

			return(0);
		}
		public static void SetRegSByte(RegIndex register, sbyte value) {SetRegByte(register, UnsignByte(value));}
		public static void SetRegByte(RegIndex register, byte value) {
			if(register == RegIndex.A) {regA = value;}
			if(register == RegIndex.B) {regB = value;}
			if(register == RegIndex.C) {regC = value;}
			if(register == RegIndex.D) {regD = value;}
			if(register == RegIndex.E) {regE = value;}
			if(register == RegIndex.H) {regH = value;}
			if(register == RegIndex.L) {regL = value;}
			if(register == RegIndex.Flags) {flags = value;}
			if(register == RegIndex.A2) {regA2 = value;}
			if(register == RegIndex.B2) {regB2 = value;}
			if(register == RegIndex.C2) {regC2 = value;}
			if(register == RegIndex.D2) {regD2 = value;}
			if(register == RegIndex.E2) {regE2 = value;}
			if(register == RegIndex.H2) {regH2 = value;}
			if(register == RegIndex.L2) {regL2 = value;}
			if(register == RegIndex.F2) {regF2 = value;}
		}

		public static short GetRegShort(RegIndex register) {return(SignShort(GetRegUShort(register)));}
		public static ushort GetRegUShort(RegIndex register) {
			if(register == RegIndex.PC) {return(pc);}
			if(register == RegIndex.SP) {return(sp);}
			if(register == RegIndex.BC) {return((ushort)(regC + (regB << 8)));}
			if(register == RegIndex.DE) {return((ushort)(regE + (regD << 8)));}
			if(register == RegIndex.HL) {return((ushort)(regL + (regH << 8)));}
			if(register == RegIndex.IX) {return(ix);}
			if(register == RegIndex.IY) {return(iy);}

			return(0);
		}
		public static void SetRegShort(RegIndex register, short value) {SetRegUShort(register, UnsignShort(value));}
		public static void SetRegUShort(RegIndex register, ushort value) {
			if(register == RegIndex.PC) {pc = value;}
			if(register == RegIndex.SP) {sp = value;}
			if(register == RegIndex.BC) {
				regB = (byte)((value & 0xFF00) >> 8);
				regC = (byte)(value & 0xFF);
			}
			if(register == RegIndex.DE) {
				regD = (byte)((value & 0xFF00) >> 8);
				regE = (byte)(value & 0xFF);
			}
			if(register == RegIndex.HL) {
				regH = (byte)((value & 0xFF00) >> 8);
				regL = (byte)(value & 0xFF);
			}
			if(register == RegIndex.IX) {ix = value;}
			if(register == RegIndex.IY) {iy = value;}
		}

		public static byte GetFlagByte(FlagIndex flag) {
			if(flag == FlagIndex.S) {return((byte)((GetRegByte(RegIndex.Flags) >> 7) & 1));}
			if(flag == FlagIndex.Z) {return((byte)((GetRegByte(RegIndex.Flags) >> 6) & 1));}
			if(flag == FlagIndex.H) {return((byte)((GetRegByte(RegIndex.Flags) >> 4) & 1));}
			if(flag == FlagIndex.P) {return((byte)((GetRegByte(RegIndex.Flags) >> 2) & 1));}
			if(flag == FlagIndex.N) {return((byte)((GetRegByte(RegIndex.Flags) >> 1) & 1));}
			if(flag == FlagIndex.C) {return((byte)((GetRegByte(RegIndex.Flags) >> 0) & 1));}

			return(0);
		}
		public static bool GetFlagBool(FlagIndex flag) {return(GetFlagByte(flag) == 1);}
		public static void SetFlag(FlagIndex flag, byte value) {
			if(flag == FlagIndex.S) {SetRegByte(RegIndex.Flags, (byte)((GetRegByte(RegIndex.Flags) & 0b01111111) + ((value & 1) << 7)));}
			if(flag == FlagIndex.Z) {SetRegByte(RegIndex.Flags, (byte)((GetRegByte(RegIndex.Flags) & 0b10111111) + ((value & 1) << 6)));}
			if(flag == FlagIndex.H) {SetRegByte(RegIndex.Flags, (byte)((GetRegByte(RegIndex.Flags) & 0b11101111) + ((value & 1) << 4)));}
			if(flag == FlagIndex.P) {SetRegByte(RegIndex.Flags, (byte)((GetRegByte(RegIndex.Flags) & 0b11111011) + ((value & 1) << 2)));}
			if(flag == FlagIndex.N) {SetRegByte(RegIndex.Flags, (byte)((GetRegByte(RegIndex.Flags) & 0b11111101) + ((value & 1) << 1)));}
			if(flag == FlagIndex.C) {SetRegByte(RegIndex.Flags, (byte)((GetRegByte(RegIndex.Flags) & 0b11111110) + ((value & 1) << 0)));}
		}
		public static void SetFlag(FlagIndex flag, bool value) {
			if(value) {SetFlag(flag, 1);}
			if(!value) {SetFlag(flag, 0);}
		}

		public static void PushToStack(byte value) {
			SetRegUShort(RegIndex.SP, (ushort)(GetRegUShort(RegIndex.SP) - 1));
			SetByte(GetRegUShort(RegIndex.SP), value);
		}
		public static void PushToStack(ushort value) {
			PushToStack((byte)((value >> 8) & 0xFF));
			PushToStack((byte)(value & 0xFF));
		}
		public static byte PopByte() {
			byte output = GetByte(GetRegUShort(RegIndex.SP));
			SetRegUShort(RegIndex.SP, (ushort)(GetRegUShort(RegIndex.SP) + 1));
			return(output);
		}
		public static ushort PopUShort() {
			ushort output = PopByte();
			output = (ushort)((PopByte() << 8) + output);
			return(output);
		}
	}
}
