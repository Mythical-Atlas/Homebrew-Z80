using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Z80.Emulator.Operations;
using static Z80.Emulator.MemoryAndRegisters;
using static Z80.Emulator.Instructions.LoadInstructions;
using static Z80.Emulator.Instructions.AddInstructions;
using static Z80.Emulator.Instructions.SubInstructions;
using static Z80.Emulator.Instructions.BranchInstructions;
using static Z80.InstructionInfo;

namespace Z80.Emulator {
	static class CPU {
		public static int cycleCount = 0;

		static string currentInst;

		public static void IncrementPC(int num) {SetRegUShort(RegIndex.PC, (ushort)(GetRegUShort(RegIndex.PC) + num));}

		public static void Step() {
			byte pcByte0 = GetByte(GetRegUShort(RegIndex.PC));
			byte pcByte1 = GetByte((ushort)(GetRegUShort(RegIndex.PC) + 1));
			byte pcByte2 = GetByte((ushort)(GetRegUShort(RegIndex.PC) + 2));
			byte pcByte3 = GetByte((ushort)(GetRegUShort(RegIndex.PC) + 3));
			ushort pcUShort1 = GetUShort(GetRegUShort(RegIndex.PC + 1));

			bool extended = false;
			if(pcByte0 == 0xED) {extended = true;}

			int instructionSize = InstructionSizes[pcByte0];
			if(extended) {instructionSize = ExtendedInstructionSizes[pcByte1];}

			bool instructionProcessed = CheckLoadInstructions();
			instructionProcessed |= CheckAddInstructions();
			instructionProcessed |= CheckSubInstructions();
			instructionProcessed |= CheckBranchInstructions();

			if(!instructionProcessed) {
				switch(pcByte0) {
					case(0x00): {break;} // no operation
					case(0x37): { // set carry flag
						SetFlag(FlagIndex.C, true);
						break;
					}
					case(0x3F): { // compliment (negate) carry flag
						SetFlag(FlagIndex.C, !GetFlagBool(FlagIndex.C));
						break;
					}
					case(0x76): { // halt
						IncrementPC(-1);
						break;
					}
					case(0xA0): { // AND a with b
						SetRegByte(RegIndex.A, (byte)(GetRegByte(RegIndex.A) & GetRegByte(RegIndex.B)));
						break;
					}
					case(0xA1): { // AND a with c
						SetRegByte(RegIndex.A, (byte)(GetRegByte(RegIndex.A) & GetRegByte(RegIndex.C)));
						break;
					}
					case(0xA8): { // XOR a with b
						SetRegByte(RegIndex.A, XORBytes(GetRegByte(RegIndex.A), GetRegByte(RegIndex.B)));
						break;
					}
					case(0xA9): { // XOR a with c
						SetRegByte(RegIndex.A, XORBytes(GetRegByte(RegIndex.A), GetRegByte(RegIndex.C)));
						break;
					}
					/*case(0xCB): { // bit instructions
						switch(pcByte1) {
							default: {break;}
						}

						break;
					}
					case(0xDD): { // ix instructions
						switch(pcByte1) {
							default: {break;}
						}

						break;
					}
					case(0xED): { // extended instructions
						switch(pcByte1) {
							default: {break;}
						}

						break;
					}
					case(0xFD): { // iy instructions
						switch(pcByte1) {
							default: {break;}
						}

						break;
					}*/
					default: { // unknown instructions
						Console.Error.WriteLine("Unknown instruction");
						Environment.Exit(1);

						break;
					}
				}
			}

			IncrementPC(instructionSize);
			cycleCount++;
		}

		public static void UpdateDebug() {
			byte pcByte0 = GetByte(GetRegUShort(RegIndex.PC));
			byte pcByte1 = GetByte((ushort)(GetRegUShort(RegIndex.PC) + 1));
			byte pcByte2 = GetByte((ushort)(GetRegUShort(RegIndex.PC) + 2));
			byte pcByte3 = GetByte((ushort)(GetRegUShort(RegIndex.PC) + 3));

			bool extended = false;
			if(pcByte0 == 0xED) {extended = true;}

			int instructionSize = InstructionSizes[pcByte0];
			if(extended) {instructionSize = ExtendedInstructionSizes[pcByte1];}

			Console.WriteLine("Cycle: " + cycleCount);
			Console.WriteLine("Program counter: 0x" + GetRegUShort(RegIndex.PC).ToString("X4"));
			Console.WriteLine("Next instruction: 0x" + pcByte0.ToString("X2"));
			if(instructionSize == 1) {Console.WriteLine("Instruction has no operands");}
			else {Console.WriteLine("Instruction has " + (instructionSize - 1) + " operands");}
			Console.WriteLine("Registers: ");
			Console.WriteLine("A: 0x" + GetRegByte(RegIndex.A).ToString("X2"));
			Console.WriteLine("BC: 0x" + GetRegUShort(RegIndex.BC).ToString("X4"));
			Console.WriteLine("DE: 0x" + GetRegUShort(RegIndex.DE).ToString("X4"));
			Console.WriteLine("HL: 0x" + GetRegUShort(RegIndex.HL).ToString("X4"));
			Console.WriteLine("SP: 0x" + GetRegUShort(RegIndex.SP).ToString("X4"));
			Console.WriteLine("Flags: 0b" + Convert.ToString(GetRegByte(RegIndex.Flags), 2).PadLeft(8, '0'));

			Program.cpuCyclesLabel.Text = "CPU Cycles (not actual cycles): " + cycleCount;

			Program.pcRegLabel.Text = "Program Counter: 0x" + GetRegUShort(RegIndex.PC).ToString("X4");
			Program.flagsBinLabel.Text = "Flags: 0b" + Convert.ToString(GetRegByte(RegIndex.Flags), 2).PadLeft(8, '0');
			Program.flagsHexLabel.Text = "Flags: 0x" + GetRegByte(RegIndex.Flags).ToString("X2");
			Program.spRegLabel.Text = "Stack Pointer: 0x" + GetRegUShort(RegIndex.SP).ToString("X4");
			Program.aRegLabel.Text = "A: 0x" + GetRegByte(RegIndex.A).ToString("X2");
			Program.bRegLabel.Text = "B: 0x" + GetRegByte(RegIndex.B).ToString("X2");
			Program.cRegLabel.Text = "C: 0x" + GetRegByte(RegIndex.C).ToString("X2");
			Program.dRegLabel.Text = "D: 0x" + GetRegByte(RegIndex.D).ToString("X2");
			Program.eRegLabel.Text = "E: 0x" + GetRegByte(RegIndex.E).ToString("X2");
			Program.bcRegLabel.Text = "BC: 0x" + GetRegUShort(RegIndex.BC).ToString("X4");
			Program.deRegLabel.Text = "DE: 0x" + GetRegUShort(RegIndex.DE).ToString("X4");
			Program.hlRegLabel.Text = "HL: 0x" + GetRegUShort(RegIndex.HL).ToString("X4");

			if(cycleCount == 0) {currentInst = "N/A";}

			Program.previousInstLabel.Text = "Previous Instruction: " + currentInst;

			currentInst = "0x" + pcByte0.ToString("X2");
			if(instructionSize > 1) {currentInst += " " + pcByte1.ToString("X2");}
			if(instructionSize > 2) {currentInst += " " + pcByte2.ToString("X2");}
			if(instructionSize > 3) {currentInst += " " + pcByte3.ToString("X2");}

			Program.nextInstLabel.Text = "Next Instruction: " + currentInst;
			Console.WriteLine();
		}
	}
}
