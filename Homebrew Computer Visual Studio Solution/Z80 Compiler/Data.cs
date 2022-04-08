using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z80.C.Compiler {
	static class Data {
		/*public enum StatementType {
			Empty,
			Definition, // defines a variable or function (only gives identifier and type, but can be followed by an assignment in the same statement)
			Assignment, // assigns a value to an existing variable or function
			Operation, // performs a math or bitwise operation on two variables (includes comparisons, which returns a boolean)
			Branch, // takes a boolean and branches (can be if or loop)
		}*/

		public enum StatementType {
			Empty,
			Unknown,

			Definition,
			Assignment,
			
			Addition,
			Subtraction,
			Multiplication,
			Division,
			Comparison,

			If,
			ElseIf,
			Else,
			ClosingBracket,
			Loop
		}

		public enum TokenType {
			Empty,
			Unknown,

			Symbol,
			Number,
			Operator,
			Keyword,
			Type,
			StatementID,
			Level,
			Bracket,
			Negative
		}

		public enum RegIndex {PC, SP, A, B, C, D, E, H, L, Flags, A2, B2, C2, D2, E2, H2, L2, F2, BC, DE, HL, IX, IY};

		/*
		 a statement is a series of tokens representing *something* between statement separators
		 statement separators include: ';', '{', '}'
		 a statement may contain another statement as an operand, meaning statements are recursive
		 */
		public struct Statement {
			public Statement(int _id, StatementType _type, List<Token> _tokens, int _level) {id = _id; type = _type; tokens = _tokens; level = _level;}
		
			public int id;
			public StatementType type;
			public List<Token> tokens;
			public int level;
		}

		public struct Token {
			public Token(TokenType _type, string _value) {type = _type; value = _value;}

			public TokenType type;
			public string value;
		}

		public struct Symbol {
			public Symbol(string _type, string _identifier, int _pointer) {type = _type; identifier = _identifier; pointer = _pointer;}

			public string type;
			public string identifier;
			public int pointer;
		}

		public static string[] Types = new string[]{"int", "bool"};
		public static string[] Operators = new string[]{"=", "+", "-", "==", "*", "/"};
		public static string[] Keywords = new string[]{"if"};
		public static string[] Brackets = new string[]{"(", ")", "{", "}"};
	}
}
