using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Z80.InstructionInfo;

namespace Z80.Assembler {
	static class Lexer {
		// NASM
		// keywords are case-insensitive

		// directives
		const string DefineConstant = "equ"; // doesn't change the final ones and zeroes
		const string DefineByte = "db"; // actually saves this value in the final file
		const string DefineWord = "dw";
		const string ReserveByte = "resb";
		const string ReserveWord = "resw";
		const string SectionStart = "section";

		// sections
		const string TextSection = ".text";
		const string DataSection = ".data";

		// entry point label
		const string EntryPoint = "_start";

		readonly static string[] Directives = new string[]{
			DefineConstant,
			DefineByte,
			DefineWord,
			ReserveByte,
			ReserveWord
		};

		readonly static string[] Sections = new string[]{
			TextSection,
			DataSection
		};

		public enum SectionType {
			Preprocessor,
			Text,
			Data
		};

		public enum TokenType {
			Empty,
			Label,
			Directive,
			Instruction,
			Operand,
			EntryPoint,
			SectionStart,
			Unknown,
			LabelReference,
			IndirectLabelReference
		};

		public struct Statement {
			public Statement(SectionType section, List<Token> tokens) {
				this.section = section;
				this.tokens = tokens;
			}

			public SectionType section;
			public List<Token> tokens;
		}

		public struct Token {
			public Token(TokenType type, string value) {
				this.type = type;
				this.value = value;
			}

			public TokenType type;
			public string value;
		}

		public static List<Statement> statements;

		// things that cannot be used as labels:
			// opcodes
			// directives

		// notes:
			// the two sections can be in any order
			// labels can be anywhere except before the sections begin

		public static List<Statement> Analyze(string input) {
			int textSectionIndex = -1;
			int dataSectionIndex = -1;

			Console.WriteLine("Lexing input:\n----------");
			Console.WriteLine(input);
			Console.WriteLine("----------\n");

			bool textSectionExists = false;
			bool dataSectionExists = false;
			bool entryPointExists = false;

			// separate file into list of statements (separated by newlines and carriage returns)
			List<string> statementStrings = new List<string>(input.Split(new char[]{'\r', '\n'}));
			statements = new List<Statement>();
			for(int s = 0; s < statementStrings.Count; s++) {
				Console.WriteLine("Found statement: " + statementStrings[s]);

				// separate statement string into list of tokens (separated by spaces, tabs, and commas)
				// two lists of tokens: a normal one, and a lowercase one for finding case-insensitive keywords
				List<string> tokenStrings = new List<string>(statementStrings[s].Split(new char[]{' ', '\t', ','}));
				List<string> lowercaseTokenStrings = new List<string>(statementStrings[s].ToLower().Split(new char[]{' ', '\t', ','}));

				Console.WriteLine("Statement contains " + tokenStrings.Count + " tokens (unprocessed)");

				// check if this statement defines a section
				if(lowercaseTokenStrings.Contains(SectionStart)) {
					// throw an error if the wrong number of arguments
					if(tokenStrings.Count != 2) {ThrowError("Lexer error: Section definition statement has invalid number of tokens");}

					// check which section is being defined, then set the appropriate index
					if(lowercaseTokenStrings[lowercaseTokenStrings.IndexOf(SectionStart) + 1] == TextSection) {
						// throw an error if two text sections are found
						if(textSectionExists) {ThrowError("Lexer error: More than one text section found");}

						Console.WriteLine("\tFound text section definition");
						textSectionExists = true;
						textSectionIndex = s;
					}
					if(lowercaseTokenStrings[lowercaseTokenStrings.IndexOf(SectionStart) + 1] == DataSection) {
						// throw an error if two data sections are found
						if(dataSectionExists) {ThrowError("Lexer error: More than one data section found");}

						Console.WriteLine("\tFound data section definition");
						dataSectionExists = true;
						dataSectionIndex = s;
					}
				}

				// assign section to current statement
				SectionType sectionType = SectionType.Preprocessor;
				if(textSectionIndex > dataSectionIndex) {
					if(s >= textSectionIndex) {sectionType = SectionType.Text;}
					else if(s >= dataSectionIndex) {sectionType = SectionType.Data;}
				}
				if(textSectionIndex < dataSectionIndex) {
					if(s >= dataSectionIndex) {sectionType = SectionType.Data;}
					else if(s >= textSectionIndex) {sectionType = SectionType.Text;}
				}

				// add each token string to a list of tokens and define what type of token it is
				// order of token precedence:
					// empty strings (ignored)
					// comments (ignored)
					// keywords (section-related tokens are ignored, since they've already been processed)
					// opcodes
					// label if only 1 token in statement
				List<Token> tokens = new List<Token>();
				for(int t = 0; t < tokenStrings.Count; t++) {
					// ignore colons at the end of tokens
					if(tokenStrings[t].EndsWith(':')) {tokenStrings[t] = tokenStrings[t].Remove(tokenStrings[t].Length - 1);}
					if(lowercaseTokenStrings[t].EndsWith(':')) {lowercaseTokenStrings[t] = lowercaseTokenStrings[t].Remove(lowercaseTokenStrings[t].Length - 1);}

					if(tokenStrings[t] == "") {}
					else if(tokenStrings[t].Contains(';')) {
						Console.WriteLine("\tFound comment at index " + t);
						break;
					}
					else if(Directives.Contains(lowercaseTokenStrings[t])) {
						// throw an error if a directive is found in the text section
						if(sectionType == SectionType.Text) {ThrowError("Lexer error: Directive found in the text section");}
						// throw an error if a constant definition directive is not in the preprocessor section
						if(lowercaseTokenStrings[t] == DefineConstant && sectionType != SectionType.Preprocessor) {ThrowError("Lexer error: Constant definition directive in an invalid section");}
						// throw an error if a byte/word definition/reservation directive is not in the preprocessor section
						if((
							lowercaseTokenStrings[t] == DefineByte ||
							lowercaseTokenStrings[t] == DefineWord ||
							lowercaseTokenStrings[t] == ReserveByte ||
							lowercaseTokenStrings[t] == ReserveWord
						) && sectionType != SectionType.Data) {ThrowError("Lexer error: Byte/word definition/reservation directive in an invalid section");}

						Console.WriteLine("\tFound directive at index " + t);
						tokens.Add(new Token(TokenType.Directive, tokenStrings[t]));
					}
					else if(lowercaseTokenStrings[t] == SectionStart) {}
					else if(Sections.Contains(lowercaseTokenStrings[t])) {}
					else if(lowercaseTokenStrings[t] == EntryPoint) {
						// throw an error if a directive is found in the text section
						if(sectionType != SectionType.Text) {ThrowError("Lexer error: Entry point is in an invalid section");}
						// throw an error if two entry points are found
						if(entryPointExists) {ThrowError("Lexer error: More than one entry point found");}

						Console.WriteLine("\tFound entry point at index " + t);
						entryPointExists = true;
						tokens.Add(new Token(TokenType.EntryPoint, tokenStrings[t]));
					}
					else if(Mnemonics.Contains(lowercaseTokenStrings[t])) {
						Console.WriteLine("\tFound instruction at index " + t);
						tokens.Add(new Token(TokenType.Instruction, tokenStrings[t]));
					}
					else if(tokenStrings.Count == 1) {
						Console.WriteLine("\tSection only contains a label");
						tokens.Add(new Token(TokenType.Label, tokenStrings[t]));
					}
					else {tokens.Add(new Token(TokenType.Unknown, tokenStrings[t]));}
				}

				// loop through tokens again to define types for unkown lables
				// the second loop is so that it can use context to define token types
				for(int t = 0; t < tokens.Count; t++) {
					if(tokens[t].type == TokenType.Unknown) {
						if(t != tokens.Count - 1) {
							if(tokens[t + 1].type == TokenType.Directive) {
								Console.WriteLine("\tFound label (followed by directive) at index " + t);
								tokens[t] = new Token(TokenType.Label, tokens[t].value);
							}
							if(tokens[t + 1].type == TokenType.Instruction) {
								Console.WriteLine("\tFound label (followed by instruction) at index " + t);
								tokens[t] = new Token(TokenType.Label, tokens[t].value);
							}
						}
						else {
							Console.WriteLine("\tSection only contains a label");
							tokens[t] = new Token(TokenType.Label, tokens[t].value);
						}
						if(t > 0) {
							if(tokens[t - 1].type == TokenType.Directive) {
								Console.WriteLine("\tFound operand (preceded by directive) at index " + t);
								tokens[t] = new Token(TokenType.Operand, tokens[t].value);
							}
							if(tokens[t - 1].type == TokenType.Instruction) {
								Console.WriteLine("\tFound operand (preceded by instruction) at index " + t);
								tokens[t] = new Token(TokenType.Operand, tokens[t].value);
							}
						}
						if(t > 1) {
							if(tokens[t - 2].type == TokenType.Instruction) {
								Console.WriteLine("\tFound operand (preceded by instruction and operand) at index " + t);
								tokens[t] = new Token(TokenType.Operand, tokens[t].value);
							}
						}
					}
				}

				Console.WriteLine("Statement contains " + tokens.Count + " tokens (processed)");
				if(sectionType == SectionType.Preprocessor) {Console.WriteLine("Statement is in the preprocessor section");}
				if(sectionType == SectionType.Text) {Console.WriteLine("Statement is in the text section");}
				if(sectionType == SectionType.Data) {Console.WriteLine("Statement is in the data section");}

				if(tokens.Count > 0) {statements.Add(new Statement(sectionType, tokens));}

				Console.WriteLine();
			}

			// throw an error if an entry point is not found
			if(!entryPointExists) {ThrowError("Compiler error: Entry point not found");}
			// throw an error if a text section is not found
			if(!textSectionExists) {ThrowError("Compiler error: Text section not found");}
			// throw an error if a data section is not found
			if(!dataSectionExists) {ThrowError("Compiler error: Data section not found");}

			return(statements);

			// loop through list of statements, then tokens
				// search for directives
					// throw error if more than one directive is found in the statement
					// throw an error if more than one token is before the directive

					// process directive
						// throw an error if the wrong number of operands are found
				// search for opcodes
					// throw error if a directive is found in same statement as opcode, or if two opcodes are in the same statement
					// throw an error if more than one token is before the opcode

					// process opcode
						// throw an error if the wrong number of operands are found
				// if not a directive or opcode statement
					// throw an error if there are more than one tokens
		}

		static void ThrowError(string error) {
			Console.Error.WriteLine(error);
			Environment.Exit(1);
		}
	}
}
