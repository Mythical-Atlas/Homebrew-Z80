using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Z80.Assembler.Lexer;
using static Z80.InstructionInfo;
using static Z80.Assembler.TextSectionProcessor;

namespace Z80.Assembler {
	static class Assembler {
		public struct Label {
			public Label(int _index, string _value) {
				index = _index;
				value = _value;
			}

			public int index;
			public string value;
		};
		public struct Symbol {
			public Symbol(string _name, int _value) {
				name = _name;
				value = _value;
			}

			public string name;
			public int value;
		};

		public enum DataLabelType {
			Byte,
			Short,
			NonData
		}

		public static byte[] rom = new byte[8 * 1024];
		public static int pc = 0;
		
		public static List<string> symbolNames = new List<string>();
		public static List<int> symbolValues = new List<int>();

		public static List<string> labelNames = new List<string>();
		public static List<int> labelIndices = new List<int>();
		public static List<int> labelValues = new List<int>();
		public static List<DataLabelType> labelTypes = new List<DataLabelType>();
		public static List<string> labelReferenceNames = new List<string>();
		public static List<int> labelReferenceIndices = new List<int>();

		public static byte[] Compile(List<Statement> statements) {
			Console.WriteLine("Compiling lexed input");
			Console.WriteLine("----------\n");

			FindSymbols(statements);
			FindLabels(statements);
			LoopThroughText(statements);
			ReplaceLabelReferences();

			return(rom);
		}

		static void FindSymbols(List<Statement> statements) {
			for(int s = 0; s < statements.Count; s++) {
				if(statements[s].section == SectionType.Preprocessor) {
					for(int t = 0; t < statements[s].tokens.Count; t++) {
						if(statements[s].tokens[t].type == TokenType.Directive) {
							if(statements[s].tokens[t].value == "equ") {
								if(t > 0 && t != statements[s].tokens.Count - 1) {
									symbolNames.Add(statements[s].tokens[t - 1].value);
									symbolValues.Add(Parse(statements[s].tokens[t + 1].value));
									Console.WriteLine("Recognized symbol: " + statements[s].tokens[t - 1].value + " " + statements[s].tokens[t + 1].value);
								}
							}
						}
					}
				}
			}
		}

		static void FindLabels(List<Statement> statements) {
			for(int s = 0; s < statements.Count; s++) {
				if(statements[s].section == SectionType.Text) {
					for(int t = 0; t < statements[s].tokens.Count; t++) {
						if(statements[s].tokens[t].type == TokenType.Label) {
								labelNames.Add(statements[s].tokens[t].value);
								labelIndices.Add(-1);
								labelValues.Add(-1);
								labelTypes.Add(DataLabelType.NonData);
								Console.WriteLine("Recognized valid label in text section: " + statements[s].tokens[t].value);
						}
					}
				}
				if(statements[s].section == SectionType.Data) {
					for(int t = 0; t < statements[s].tokens.Count; t++) {
						if(statements[s].tokens[t].type == TokenType.Directive) {
							if(statements[s].tokens[t].value == "db" || statements[s].tokens[t].value == "dw") {
								if(t > 0 && t != statements[s].tokens.Count - 1) {
									labelNames.Add(statements[s].tokens[t - 1].value);
									labelIndices.Add(-1);
									labelValues.Add(Parse(statements[s].tokens[t + 1].value));
									if(statements[s].tokens[t].value == "db") {labelTypes.Add(DataLabelType.Byte);}
									if(statements[s].tokens[t].value == "dw") {labelTypes.Add(DataLabelType.Short);}
									Console.WriteLine("Recognized valid label in data section: " + statements[s].tokens[t - 1].value);
								}
							}
						}
					}
				}
			}
		}

		static void ReplaceLabelReferences() {
			for(int l = 0; l < labelNames.Count; l++) {
				if(labelTypes[l] != DataLabelType.NonData) {
					labelIndices[l] = pc;

					Console.WriteLine("Assigned address 0x" + pc.ToString("X4") + " to data label: " + labelNames[l]);

					if(labelTypes[l] == DataLabelType.Byte) {rom[pc++] = (byte)labelValues[l];}
					if(labelTypes[l] == DataLabelType.Short) {
						rom[pc++] = (byte)(labelValues[l] & 0xFF);
						rom[pc++] = (byte)((labelValues[l] & 0xFF00) >> 8);
					}
				}
			}

			for(int r = 0; r < labelReferenceNames.Count; r++) {
				int l = labelIndices[labelNames.IndexOf(labelReferenceNames[r])];

				Console.WriteLine("Replaced operand at address 0x" + labelReferenceIndices[r].ToString("X4") + " to value: 0x" + l.ToString("X4"));

				rom[labelReferenceIndices[r]] = (byte)(l & 0xFF);
				rom[labelReferenceIndices[r] + 1] = (byte)((l & 0xFF00) >> 8);
			}
		}

		public static OperandType GetOperandType(string operand) {
			OperandType actualType = OperandType.Non;

			int value;
			if(TryParse(operand, out value)) {
				if(value > 0xFFFF) {}
				else if(value > 0xFF) {actualType = OperandType.ImS;}
				else {actualType = OperandType.ImB;}
			}
			else if(operand == "a") {actualType = OperandType.A;}
			else if(operand == "b") {actualType = OperandType.B;}
			else if(operand == "c") {actualType = OperandType.C;}
			else if(operand == "d") {actualType = OperandType.D;}
			else if(operand == "e") {actualType = OperandType.E;}
			else if(operand == "bc") {actualType = OperandType.BC;}
			else if(operand == "de") {actualType = OperandType.DE;}
			else if(operand == "hl") {actualType = OperandType.HL;}
			else if(operand == "sp") {actualType = OperandType.SP;}
			else if(operand == "nz") {actualType = OperandType.CNZ;}
			else if(operand == "z") {actualType = OperandType.CZ;}
			else if(operand == "nc") {actualType = OperandType.CNC;}
			else if(operand == "c") {actualType = OperandType.CC;}
			else if(operand == "po") {actualType = OperandType.CPO;}
			else if(operand == "pe") {actualType = OperandType.CPE;}
			else if(operand == "p") {actualType = OperandType.CP;}
			else if(operand == "m") {actualType = OperandType.CM;}
			else if(operand.StartsWith('(') && operand.EndsWith(')')) {if(TryParse(operand.Substring(1, operand.Length - 2), out value)) {actualType = OperandType.IdB;}}

			return(actualType);
		}

		public static bool CheckOperandTypesMatch(OperandType expected, OperandType actual) {
			if(expected == actual) {return(true);}
			else if(expected != actual) {
				if(expected == OperandType.ImS && actual == OperandType.ImB) {return(true);}
				if(expected == OperandType.IdS && actual == OperandType.IdB) {return(true);}
			}
			return(false);
		}

		public static bool TryParse(string input, out int output) {
			if(input.StartsWith("0x")) {
				try {
					int temp = Convert.ToInt32(input, 16);
					output = temp;
					return(true);
				}
				catch(Exception e) {
					output = 0;
					return(false);
				}
			}
			else {
				try {
					int temp = Convert.ToInt32(input);
					output = temp;
					return(true);
				}
				catch(Exception e) {
					output = 0;
					return(false);
				}
			}
		}
		public static int Parse(string input) {
			if(input.StartsWith("0x")) {
				try {
					int temp = Convert.ToInt32(input, 16);
					return(temp);
				}
				catch(Exception e) {return(0);}
			}
			else {
				try {
					int temp = Convert.ToInt32(input);
					return(temp);
				}
				catch(Exception e) {return(0);}
			}
		}
	}
}
