namespace Z80 {
	public static class Instructions_OLD {
		public enum Notation {
			a,
			r,
			HL,
			ixd,
			iyd,
			n,
			nn,
			d,
			b,
			e,
			cc,
			qq,
			ss,
			pp,
			rr,
			s,
			m,
			none
		}

		public struct Instruction {
			public Instruction(string mnemonic, int opcode, Notation operand1, Notation operand2, int length) {
				this.mnemonic = mnemonic;
				this.opcode = opcode;
				this.operand1 = operand1;
				this.operand2 = operand2;
				this.length = length;
			}
			public Instruction(string mnemonic, int opcode, Notation operand, int length) {
				this.mnemonic = mnemonic;
				this.opcode = opcode;
				this.operand1 = operand;
				this.operand2 = Notation.none;
				this.length = length;
			}

			public string mnemonic;
			public int opcode;
			public Notation operand1;
			public Notation operand2;
			public int length;
		}

		public enum Opcodes {
			addan = 0x80
		};

		public readonly static Instruction[] instructions = new Instruction[]{
			new Instruction("add", 0x80, Notation.a, Notation.r, 1),
			new Instruction("add", 0xC6, Notation.a, Notation.n, 2),
			new Instruction("ld", 0x21, Notation.r, Notation.nn, 3)
		};
	}
}