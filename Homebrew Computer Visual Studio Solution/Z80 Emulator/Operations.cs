using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using static Z80.Emulator.CPU;
using static Z80.Emulator.MemoryAndRegisters;

namespace Z80.Emulator {
	public static class Operations {
		public static byte UnsignByte(sbyte value) {return(BitConverter.GetBytes(value)[0]);}
		public static sbyte SignByte(byte value) {return((sbyte)BitConverter.ToChar(new byte[]{value, 0}));}

		public static ushort UnsignShort(short value) {return(BitConverter.ToUInt16(new byte[]{(byte)(value & 0xFF), (byte)((value >> 8) & 0xFF)}));}
		public static short SignShort(ushort value) {return(BitConverter.ToInt16(new byte[]{(byte)(value & 0xFF), (byte)((value >> 8) & 0xFF)}));}

		public static void Push(ushort value) {
			SetUShort((ushort)(GetRegUShort(RegIndex.SP) - 2), value);
			SetRegUShort(RegIndex.SP, (ushort)(GetRegUShort(RegIndex.SP) - 2));
		}

		public static ushort Pop(ushort address) {
			ushort value = GetUShort(GetRegUShort(RegIndex.SP));
			SetRegUShort(RegIndex.SP, (ushort)(GetRegUShort(RegIndex.SP) + 2));
			return(value);
		}

		public static sbyte AddSBytes(sbyte in1, sbyte in2) {return(SignByte(AddBytes(UnsignByte(in1), UnsignByte(in2))));}
		public static byte AddBytes(byte in1, byte in2) {
			int intSum = in1 + in2;
			int intA = in1;
			int intB = in2;

			byte byteSum = (byte)(in1 + in2);

			SetFlag(FlagIndex.S, ((byte)intSum & 0x80) == 0x80);
			SetFlag(FlagIndex.Z, byteSum == 0);
			SetFlag(FlagIndex.H, (intA & 0x7) + (intB & 0x7) >= 0x8);
			SetFlag(FlagIndex.P, (intA & 0x80) == (intB & 0x80) && (intA & 0x80) != (intSum & 0x80));
			SetFlag(FlagIndex.N, false);
			SetFlag(FlagIndex.C, intA + intB >= 0x100);

			return((byte)intSum);
		}
		public static short AddShorts(short in1, short in2) {return(SignShort(AddUShorts(UnsignShort(in1), UnsignShort(in2))));}
		public static ushort AddUShorts(ushort in1, ushort in2) {
			int intSum = in1 + in2;
			int intA = in1;
			int intB = in2;

			SetFlag(FlagIndex.H, (intA & 0x7FF) + (intB & 0x7FF) >= 0x800);
			SetFlag(FlagIndex.N, false);
			SetFlag(FlagIndex.C, intA + intB >= 0x10000);

			return((ushort)intSum);
		}

		public static byte XORBytes(byte in1, byte in2) {
			byte sum = (byte)(in1 ^ in2);

			SetFlag(FlagIndex.S, (sum & 0x80) == 0x80);
			SetFlag(FlagIndex.Z, sum == 0);
			SetFlag(FlagIndex.H, false);
			SetFlag(FlagIndex.P, (in1 & 0x80) == (in2 & 0x80) && (in1 & 0x80) != (sum & 0x80));
			SetFlag(FlagIndex.N, false);
			SetFlag(FlagIndex.C, false);

			return(sum);
		}
	}
}
