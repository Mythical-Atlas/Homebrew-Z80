namespace Z80 {
	public static class InstructionInfo {
		public enum OperandType {Non, ImB, ImS, A, B, C, D, E, H, L, I, R, BC, DE, HL, SP, AF, AF2, IdB, IdS, IC, IBC, IDE, IHL, ISP, CNZ, CZ, CNC, CC, CPO, CPE, CP, CM, Zer, One, Two};

		public static int[] InstructionSizes = new int[]{
		// x0 x1 x2 x3 x4 x5 x6 x7 x8 x9 xA xB xC xD xE xF
			1, 3, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 2, 1, // 0x
			2, 3, 1, 1, 1, 1, 2, 1, 2, 1, 1, 1, 1, 1, 2, 1, // 1x
			2, 3, 3, 1, 1, 1, 2, 1, 2, 1, 3, 1, 1, 1, 2, 1, // 2x
			2, 3, 3, 1, 1, 1, 2, 1, 2, 1, 3, 1, 1, 1, 2, 1, // 3x
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, // 4x
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, // 5x
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, // 6x
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, // 7x
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, // 8x
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, // 9x
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, // Ax
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, // Bx
			1, 1, 3, 3, 3, 1, 2, 1, 1, 1, 3, 0, 3, 3, 2, 1, // Cx
			1, 1, 3, 2, 3, 1, 2, 1, 1, 1, 3, 2, 3, 0, 2, 1, // Dx
			1, 1, 3, 1, 3, 1, 2, 1, 1, 1, 3, 1, 3, 0, 2, 1, // Ex
			1, 1, 3, 1, 3, 1, 2, 1, 1, 1, 3, 1, 3, 0, 2, 1, // Fx
		};
		public static int[] ExtendedInstructionSizes = new int[]{
		// x0 x1 x2 x3 x4 x5 x6 x7 x8 x9 xA xB xC xD xE xF
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			2, 2, 2, 4, 2, 2, 2, 2, 2, 2, 2, 4, 0, 2, 0, 2, // 4x
			2, 2, 2, 4, 0, 2, 2, 2, 2, 2, 2, 4, 0, 2, 2, 2, // 5x
			2, 2, 2, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 2, 0, 2, // 6x
			2, 0, 2, 4, 0, 2, 2, 0, 2, 2, 2, 4, 0, 2, 2, 0, // 7x
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			2, 2, 2, 2, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, // Ax
			2, 2, 2, 2, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, // Bx
		};

		public static string[] Mnemonics = new string[]{
			 "nop",   "ld",   "ld",  "inc",  "inc",  "dec",   "ld", "rlca",   "ex",  "add",   "ld",  "dec",  "inc",  "dec",   "ld", "rrca",
			"djnz",   "ld",   "ld",  "inc",  "inc",  "dec",   "ld",  "rla",   "jr",  "add",   "ld",  "dec",  "inc",  "dec",   "ld",  "rra",
			  "jr",   "ld",   "ld",  "inc",  "inc",  "dec",   "ld",  "daa",   "jr",  "add",   "ld",  "dec",  "inc",  "dec",   "ld",  "cpl",
			  "jr",   "ld",   "ld",  "inc",  "inc",  "dec",   "ld",  "scf",   "jr",  "add",   "ld",  "dec",  "inc",  "dec",   "ld",  "ccf",
			  "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",
			  "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",
			  "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",
			  "ld",   "ld",   "ld",   "ld",   "ld",   "ld", "halt",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",   "ld",
			 "add",  "add",  "add",  "add",  "add",  "add",  "add",  "add",  "adc",  "adc",  "adc",  "adc",  "adc",  "adc",  "adc",  "adc",
			 "sub",  "sub",  "sub",  "sub",  "sub",  "sub",  "sub",  "sub",  "sbc",  "sbc",  "sbc",  "sbc",  "sbc",  "sbc",  "sbc",  "sbc",
			 "and",  "and",  "and",  "and",  "and",  "and",  "and",  "and",  "xor",  "xor",  "xor",  "xor",  "xor",  "xor",  "xor",  "xor",
			  "or",   "or",   "or",   "or",   "or",   "or",   "or",   "or",   "cp",   "cp",   "cp",   "cp",   "cp",   "cp",   "cp",   "cp",
			 "ret",  "pop",   "jp",   "jp", "call", "push",  "add",  "rst",  "ret",  "ret",   "jp",     "", "call", "call",  "adc",  "rst",
			 "ret",  "pop",   "jp",  "out", "call", "push",  "sub",  "rst",  "ret",  "exx",   "jp",   "in", "call",     "",  "sbc",  "rst",
			 "ret",  "pop",   "jp",   "ex", "call", "push",  "and",  "rst",  "ret",   "jp",   "jp",   "ex", "call",     "",  "xor",  "rst",
			 "ret",  "pop",   "jp",   "di", "call", "push",   "or",  "rst",  "ret",   "ld",   "jp",   "ei", "call",     "",   "cp",  "rst",
		};
		public static string[] ExtendedMnemonics = new string[]{
			    "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",
			    "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",
			    "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",
			    "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",
			  "in",  "out",  "sbc",   "ld",  "neg", "retn",   "im",   "ld",   "in",  "out",  "adc",   "ld",     "", "reti",     "",   "ld",
			  "in",  "out",  "sbc",   "ld",     "", "retn",   "im",   "ld",   "in",  "out",  "adc",   "ld",     "", "retn",   "im",   "ld",
			  "in",  "out",  "sbc",     "",     "", "retn",   "im",  "rrd",   "in",  "out",  "adc",     "",     "", "retn",     "",  "rld",
			    "",     "",  "sbc",   "ld",     "", "retn",   "im",     "",   "in",  "out",  "adc",   "ld",     "", "retn",   "im",     "",
			    "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",
			    "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",     "",
			 "ldi",  "cpi",  "ini", "outi",     "",     "",     "",     "",  "ldd",  "cpd",  "ind", "outd",     "",     "",     "",     "",
			"ldir", "cpir", "inir", "OperandTypeir",     "",     "",     "",     "", "lddr", "cpdr", "indr", "OperandTypedr",     "",     "",     "",     "",
		};

		public static OperandType[] FirstOperandTypes = new OperandType[]{
		//	x0		x1		x2		x3		x4		x5		x6		x7		x8		x9		xA		xB		xC		xD		xE		xF
			OperandType.Non,	OperandType.BC,	OperandType.IBC,	OperandType.BC,	OperandType.Non,	OperandType.B,	OperandType.B,	OperandType.B,	OperandType.Non,	OperandType.HL,	OperandType.A,	OperandType.BC,	OperandType.Non,	OperandType.C,	OperandType.C,	OperandType.Non,	// 0x
			OperandType.ImB,	OperandType.DE,	OperandType.IDE,	OperandType.DE,	OperandType.Non,	OperandType.D,	OperandType.D,	OperandType.D,	OperandType.Non,	OperandType.HL,	OperandType.A,	OperandType.DE,	OperandType.Non,	OperandType.E,	OperandType.E,	OperandType.Non,	// 1x
			OperandType.CNZ,	OperandType.HL,	OperandType.IdS,	OperandType.HL,	OperandType.Non,	OperandType.H,	OperandType.H,	OperandType.H,	OperandType.Non,	OperandType.HL,	OperandType.HL,	OperandType.HL,	OperandType.Non,	OperandType.L,	OperandType.L,	OperandType.Non,	// 2x
			OperandType.CNC,	OperandType.SP,	OperandType.IdS,	OperandType.SP,	OperandType.Non,	OperandType.IHL,	OperandType.IHL,	OperandType.IHL,	OperandType.Non,	OperandType.HL,	OperandType.A,	OperandType.SP,	OperandType.Non,	OperandType.A,	OperandType.A,	OperandType.Non,	// 3x
			OperandType.B,	OperandType.B,	OperandType.B,	OperandType.B,	OperandType.B,	OperandType.B,	OperandType.B,	OperandType.B,	OperandType.C,	OperandType.C,	OperandType.C,	OperandType.C,	OperandType.C,	OperandType.C,	OperandType.C,	OperandType.C,	// 4x
			OperandType.D,	OperandType.D,	OperandType.D,	OperandType.D,	OperandType.D,	OperandType.D,	OperandType.D,	OperandType.D,	OperandType.E,	OperandType.E,	OperandType.E,	OperandType.E,	OperandType.E,	OperandType.E,	OperandType.E,	OperandType.E,	// 5x
			OperandType.H,	OperandType.H,	OperandType.H,	OperandType.H,	OperandType.H,	OperandType.H,	OperandType.H,	OperandType.H,	OperandType.L,	OperandType.L,	OperandType.L,	OperandType.L,	OperandType.L,	OperandType.L,	OperandType.L,	OperandType.L,	// 6x
			OperandType.IHL,	OperandType.IHL,	OperandType.IHL,	OperandType.IHL,	OperandType.IHL,	OperandType.IHL,	OperandType.Non,	OperandType.IHL,	OperandType.A,	OperandType.A,	OperandType.A,	OperandType.A,	OperandType.A,	OperandType.A,	OperandType.A,	OperandType.A,	// 7x
			OperandType.A,	OperandType.A,	OperandType.A,	OperandType.A,	OperandType.A,	OperandType.A,	OperandType.A,	OperandType.A,	OperandType.A,	OperandType.A,	OperandType.A,	OperandType.A,	OperandType.A,	OperandType.A,	OperandType.A,	OperandType.A,	// 8x
			OperandType.B,	OperandType.C,	OperandType.D,	OperandType.E,	OperandType.H,	OperandType.L,	OperandType.IHL,	OperandType.A,	OperandType.A,	OperandType.A,	OperandType.A,	OperandType.A,	OperandType.A,	OperandType.A,	OperandType.A,	OperandType.A,	// 9x
			OperandType.B,	OperandType.C,	OperandType.D,	OperandType.E,	OperandType.H,	OperandType.L,	OperandType.IHL,	OperandType.A,	OperandType.B,	OperandType.C,	OperandType.D,	OperandType.E,	OperandType.H,	OperandType.L,	OperandType.IHL,	OperandType.A,	// Ax
			OperandType.B,	OperandType.C,	OperandType.D,	OperandType.E,	OperandType.H,	OperandType.L,	OperandType.IHL,	OperandType.A,	OperandType.B,	OperandType.C,	OperandType.D,	OperandType.E,	OperandType.H,	OperandType.L,	OperandType.IHL,	OperandType.A,	// Bx
			OperandType.CNZ,	OperandType.BC,	OperandType.CNZ,	OperandType.ImS,	OperandType.CNZ,	OperandType.BC,	OperandType.A,	OperandType.ImB,	OperandType.CZ,	OperandType.Non,	OperandType.CZ,	OperandType.Non,	OperandType.CZ,	OperandType.ImS,	OperandType.A,	OperandType.ImB,	// Cx
			OperandType.CNC,	OperandType.DE,	OperandType.CNC,	OperandType.IdB,	OperandType.CNC,	OperandType.DE,	OperandType.ImB,	OperandType.ImB,	OperandType.CC,	OperandType.Non,	OperandType.CC,	OperandType.A,	OperandType.CC,	OperandType.Non,	OperandType.A,	OperandType.ImB,	// Dx
			OperandType.CPO,	OperandType.HL,	OperandType.CPO,	OperandType.ISP,	OperandType.CPO,	OperandType.HL,	OperandType.ImB,	OperandType.ImB,	OperandType.CPE,	OperandType.IHL,	OperandType.CPE,	OperandType.DE,	OperandType.CPE,	OperandType.Non,	OperandType.ImB,	OperandType.ImB,	// Ex
			OperandType.CP,	OperandType.AF,	OperandType.CP,	OperandType.Non,	OperandType.CP,	OperandType.AF,	OperandType.ImB,	OperandType.ImB,	OperandType.CM,	OperandType.SP,	OperandType.CM,	OperandType.Non,	OperandType.CM,	OperandType.Non,	OperandType.ImB,	OperandType.ImB,	// Fx
		};
		public static OperandType[] SecondOperandTypes = new OperandType[]{
		//	x0		x1		x2		x3		x4		x5		x6		x7		x8		x9		xA		xB		xC		xD		xE		xF
			OperandType.Non,	OperandType.ImS,	OperandType.A,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.ImB,	OperandType.Non,	OperandType.AF2,	OperandType.BC,	OperandType.IBC,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.ImB,	OperandType.Non,	// 0x
			OperandType.Non,	OperandType.ImS,	OperandType.A,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.ImB,	OperandType.Non,	OperandType.ImB,	OperandType.DE,	OperandType.IDE,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.ImB,	OperandType.Non,	// 1x
			OperandType.Non,	OperandType.ImS,	OperandType.HL,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.ImB,	OperandType.Non,	OperandType.ImB,	OperandType.HL,	OperandType.IdS,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.ImB,	OperandType.Non,	// 2x
			OperandType.Non,	OperandType.ImS,	OperandType.A,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.ImB,	OperandType.Non,	OperandType.ImB,	OperandType.SP,	OperandType.IdS,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.ImB,	OperandType.Non,	// 3x
			OperandType.B,	OperandType.C,	OperandType.D,	OperandType.E,	OperandType.H,	OperandType.L,	OperandType.IHL,	OperandType.A,	OperandType.B,	OperandType.C,	OperandType.D,	OperandType.E,	OperandType.H,	OperandType.L,	OperandType.IHL,	OperandType.A,	// 4x
			OperandType.B,	OperandType.C,	OperandType.D,	OperandType.E,	OperandType.H,	OperandType.L,	OperandType.IHL,	OperandType.A,	OperandType.B,	OperandType.C,	OperandType.D,	OperandType.E,	OperandType.H,	OperandType.L,	OperandType.IHL,	OperandType.A,	// 5x
			OperandType.B,	OperandType.C,	OperandType.D,	OperandType.E,	OperandType.H,	OperandType.L,	OperandType.IHL,	OperandType.A,	OperandType.B,	OperandType.C,	OperandType.D,	OperandType.E,	OperandType.H,	OperandType.L,	OperandType.IHL,	OperandType.A,	// 6x
			OperandType.B,	OperandType.C,	OperandType.D,	OperandType.E,	OperandType.H,	OperandType.L,	OperandType.Non,	OperandType.A,	OperandType.B,	OperandType.C,	OperandType.D,	OperandType.E,	OperandType.H,	OperandType.L,	OperandType.IHL,	OperandType.A,	// 7x
			OperandType.B,	OperandType.C,	OperandType.D,	OperandType.E,	OperandType.H,	OperandType.L,	OperandType.IHL,	OperandType.A,	OperandType.B,	OperandType.C,	OperandType.D,	OperandType.E,	OperandType.H,	OperandType.L,	OperandType.IHL,	OperandType.A,	// 8x
			OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.B,	OperandType.C,	OperandType.D,	OperandType.E,	OperandType.H,	OperandType.L,	OperandType.IHL,	OperandType.A,	// 9x
			OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	// Ax
			OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	// Bx
			OperandType.Non,	OperandType.Non,	OperandType.ImS,	OperandType.Non,	OperandType.ImS,	OperandType.Non,	OperandType.ImB,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.ImS,	OperandType.Non,	OperandType.ImS,	OperandType.Non,	OperandType.ImB,	OperandType.Non,	// Cx
			OperandType.Non,	OperandType.Non,	OperandType.ImS,	OperandType.A,	OperandType.ImS,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.ImS,	OperandType.IdB,	OperandType.ImS,	OperandType.Non,	OperandType.ImB,	OperandType.Non,	// Dx
			OperandType.Non,	OperandType.Non,	OperandType.ImS,	OperandType.HL,	OperandType.ImS,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.ImS,	OperandType.HL,	OperandType.ImS,	OperandType.Non,	OperandType.Non,	OperandType.Non,	// Ex
			OperandType.Non,	OperandType.Non,	OperandType.ImS,	OperandType.Non,	OperandType.ImS,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.HL,	OperandType.ImS,	OperandType.Non,	OperandType.ImS,	OperandType.Non,	OperandType.Non,	OperandType.Non,	// Fx
		};

		public static OperandType[] ExtendedFirstOperandTypes = new OperandType[]{
		//	x0		x1		x2		x3		x4		x5		x6		x7		x8		x9		xA		xB		xC		xD		xE		xF
			OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,
			OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,
			OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,
			OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,
			OperandType.B,	OperandType.IC,	OperandType.HL,	OperandType.IdS,	OperandType.Non,	OperandType.Non,	OperandType.Zer,	OperandType.I,	OperandType.C,	OperandType.IC,	OperandType.HL,	OperandType.BC,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.R,	// 4x
			OperandType.D,	OperandType.IC,	OperandType.HL,	OperandType.IdS,	OperandType.Non,	OperandType.Non,	OperandType.One,	OperandType.A,	OperandType.E,	OperandType.IC,	OperandType.HL,	OperandType.DE,	OperandType.Non,	OperandType.Non,	OperandType.Two,	OperandType.A,	// 5x
			OperandType.H,	OperandType.IC,	OperandType.HL,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Zer,	OperandType.Non,	OperandType.L,	OperandType.IC,	OperandType.HL,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	// 6x
			OperandType.Non,	OperandType.Non,	OperandType.HL,	OperandType.IdS,	OperandType.Non,	OperandType.Non,	OperandType.One,	OperandType.Non,	OperandType.A,	OperandType.IC,	OperandType.HL,	OperandType.SP,	OperandType.Non,	OperandType.Non,	OperandType.Two,	OperandType.Non,	// 7x
			OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,
			OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,
			OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	// Ax
			OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	// Bx
		};
		public static OperandType[] ExtendedSecondOperandTypes = new OperandType[]{
		//	x0		x1		x2		x3		x4		x5		x6		x7		x8		x9		xA		xB		xC		xD		xE		xF
			OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,
			OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,
			OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,
			OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,
			OperandType.IC,	OperandType.B,	OperandType.BC,	OperandType.BC,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.A,	OperandType.IC,	OperandType.C,	OperandType.BC,	OperandType.IdS,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.A,	// 4x
			OperandType.IC,	OperandType.D,	OperandType.DE,	OperandType.DE,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.I,	OperandType.IC,	OperandType.E,	OperandType.DE,	OperandType.IdS,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.R,	// 5x
			OperandType.IC,	OperandType.H,	OperandType.HL,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.IC,	OperandType.L,	OperandType.HL,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	// 6x
			OperandType.Non,	OperandType.Non,	OperandType.SP,	OperandType.SP,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.IC,	OperandType.A,	OperandType.SP,	OperandType.IdS,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	// 7x
			OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,
			OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,
			OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	// Ax
			OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	OperandType.Non,	// Bx
		};
	}
}