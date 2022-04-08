using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Z80.Emulator.CPU;
using static Z80.Emulator.MemoryAndRegisters;
using static Z80.Emulator.Operations;

namespace Z80.Emulator.Instructions {
	static class BranchInstructions {
		public static bool CheckBranchInstructions() {
			byte pcByte0 = GetByte(GetRegUShort(RegIndex.PC));
			byte pcByte1 = GetByte((ushort)(GetRegUShort(RegIndex.PC) + 1));
			ushort pcUShort1 = GetUShort((ushort)(GetRegUShort(RegIndex.PC) + 1));

			switch(pcByte0) {
				case(0x18): { // relative jump
					SetRegShort(RegIndex.PC, (short)(GetRegShort(RegIndex.PC) + GetSByte((ushort)(GetRegUShort(RegIndex.PC) + 1))));
					return(true);
				}
				case(0x20): { // relative jump if nz
					if(!GetFlagBool(FlagIndex.Z)) {SetRegShort(RegIndex.PC, (short)(GetRegShort(RegIndex.PC) + GetSByte((ushort)(GetRegUShort(RegIndex.PC) + 1))));}
					return(true);
				}
				case(0x28): { // relative jump if z
					if(GetFlagBool(FlagIndex.Z)) {SetRegShort(RegIndex.PC, (short)(GetRegShort(RegIndex.PC) + GetSByte((ushort)(GetRegUShort(RegIndex.PC) + 1))));}
					return(true);
				}
				case(0xC2): { // jump if nz to immediate short
					if(!GetFlagBool(FlagIndex.Z)) {SetRegUShort(RegIndex.PC, (ushort)(pcUShort1 - 3));}
					return(true);
				}
				case(0xC9): { // return from subroutine
					SetRegUShort(RegIndex.PC, (ushort)(PopUShort() - 1));
					return(true);
				}
				case(0xCA): { // jump if z to immediate short
					if(GetFlagBool(FlagIndex.Z)) {SetRegUShort(RegIndex.PC, (ushort)(pcUShort1 - 3));}
					return(true);
				}
				case(0xCD): { // call subroutine at immediate short
					SetRegUShort(RegIndex.PC, (ushort)(GetRegUShort(RegIndex.PC) + 3));
					PushToStack(GetRegUShort(RegIndex.PC));
					SetRegUShort(RegIndex.PC, (ushort)(pcUShort1 - 3));
					return(true);
				}
			}

			return(false);
		}
	}
}
