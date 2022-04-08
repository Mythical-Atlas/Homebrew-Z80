using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Z80.C.Compiler.Data;

namespace Z80.C.Compiler {
	static class Lexer {
		/*
		 takes an unprocessed string containing C code

		 finds and defines statements
		 if a statement is found that contains a statement as an operand, the operand statement is defined first (recursively)

		 returns a list of statements with defined types and defined tokens
		 */
		public static List<Statement> Analyze(string input) {
			List<Statement> parStates = ParseString(input);
			List<Statement> finStates = new List<Statement>();

			for(int s = 0; s < parStates.Count; s++) {
				bool hasDefinition = false;
				bool hasIf = false;
				bool hasOperation = false;

				// checks if the statement contains more than 1 token so that it doesn't throw an error when checking tokens[1]
				if(parStates[s].tokens.Count >= 2) {
					if(parStates[s].tokens[0].type == TokenType.Type && parStates[s].tokens[1].type == TokenType.Symbol) {
						int newID = 0;
						if(finStates.Count > 0) {newID = finStates.Last().id + 1;}

						List<Token> tempTokens = new List<Token>(new Token[]{parStates[s].tokens[0], parStates[s].tokens[1]});
						finStates.Add(new Statement(newID, StatementType.Definition, tempTokens, parStates[s].level));

						hasDefinition = true;
					}
				}

				if(parStates[s].tokens[0].value == "if") {hasIf = true;}

				for(int t = 0; t < parStates[s].tokens.Count; t++) {if(Operators.Contains(parStates[s].tokens[t].value)) {hasOperation = true;}}

				if(hasOperation) {
					List<Token> operationTokens = new List<Token>();

					// changes what tokens are pulled for an operation statement depending on whether the statement also has a definition
					// this is because a definition statement has an extra token (the type) at the start of the statement
					if(hasDefinition) {operationTokens.AddRange(parStates[s].tokens.GetRange(1, parStates[s].tokens.Count - 1));}
					else if(hasIf) {operationTokens.AddRange(parStates[s].tokens.GetRange(2, parStates[s].tokens.Count - 4));}
					else {operationTokens.AddRange(parStates[s].tokens);}

					HandleOperation(operationTokens, parStates[s].level, finStates, out finStates);

					if(hasIf) {
						int newID = 0;
						if(finStates.Count > 0) {newID = finStates.Last().id + 1;}

						List<Token> finalTokens = new List<Token>();
						finalTokens.Add(new Token(TokenType.StatementID, "" + finStates.Last().id));
						finalTokens.Add(new Token(TokenType.Level, "" + (parStates[s].level + 1)));

						finStates.Add(new Statement(newID, StatementType.If, finalTokens, parStates[s].level));
					}
				}

				if(parStates[s].tokens[0].value == "}") {
					int newID = 0;
					if(finStates.Count > 0) {newID = finStates.Last().id + 1;}
					finStates.Add(new Statement(newID, StatementType.ClosingBracket, parStates[s].tokens, parStates[s].level));
				}
			}

			return(finStates);
		}

		/*
		 takes a list of tokens that is already confirmed to have one or more operations

		 can be recursed

		 returns a list of statement with ids
		 */
		static void HandleOperation(List<Token> tokensIn, int level, List<Statement> statesIn, out List<Statement> statesOut) {
			/*
			check if statement has more than one operation
			if yes:
				move down to the next most important
				pass that statement on to the next HandleOperation
				once statement(s) come back, use the last statement(s) in the list's ID(s) as operand(s)
			
			else:
				statement should only have 3 tokens
				process statement and give id
			 */

			// required to ignore negate tokens
			int tokenCount = 0;
			for(int i = 0; i < tokensIn.Count; i++) {
				if(tokensIn[i].type != TokenType.Negative) {tokenCount++;}
			}

			if(tokenCount > 3) { // recursion required
				// ironically, only the first index of the sequence is used
				int nextOp = SequenceOperations(tokensIn)[0];
				int recursionSide = 0;
				StatementType newType = CheckStatementType(tokensIn);

				if(nextOp > 1) { // left is recursive
					List<Token> operationTokens = new List<Token>();
					operationTokens.AddRange(tokensIn.GetRange(0, nextOp));
					HandleOperation(operationTokens, level, statesIn, out statesIn);

					recursionSide--;
				}
				if(nextOp < tokensIn.Count - 2) { // right is recursive
					List<Token> operationTokens = new List<Token>();
					operationTokens.AddRange(tokensIn.GetRange(nextOp + 1, tokensIn.Count - (nextOp + 1)));
					HandleOperation(operationTokens, level, statesIn, out statesIn);

					recursionSide++;
				}

				// depending on which side of operation is recursive, add tokens from old statement and ID of recursive statement(s)
				List<Token> finalTokens = new List<Token>();
				if(recursionSide < 0) {
					finalTokens.Add(new Token(TokenType.StatementID, "" + statesIn.Last().id));
					finalTokens.AddRange(tokensIn.GetRange(nextOp, 2));
				}
				if(recursionSide > 0) {
					finalTokens.AddRange(tokensIn.GetRange(nextOp - 1, 2));
					finalTokens.Add(new Token(TokenType.StatementID, "" + statesIn.Last().id));
				}
				if(recursionSide == 0) {
					finalTokens.Add(new Token(TokenType.StatementID, "" + statesIn[statesIn.Count - 2].id));
					finalTokens.Add(tokensIn[nextOp]);
					finalTokens.Add(new Token(TokenType.StatementID, "" + statesIn.Last().id));
				}

				int newID = 0;
				if(statesIn.Count > 0) {newID = statesIn.Last().id + 1;}

				statesIn.Add(new Statement(newID, newType, finalTokens, level));
			}
			else { // no recursion, operation only has 3 tokens
				int newID = 0;
				if(statesIn.Count > 0) {newID = statesIn.Last().id + 1;}

				StatementType newType = CheckStatementType(tokensIn);

				statesIn.Add(new Statement(newID, newType, tokensIn, level));
			}

			statesOut = statesIn;
		}

		static StatementType CheckStatementType(List<Token> tokens) {
			int nextOp = SequenceOperations(tokens)[0];

			if(tokens[nextOp].value == "=") {return(StatementType.Assignment);}
			if(tokens[nextOp].value == "==") {return(StatementType.Comparison);}
			if(tokens[nextOp].value == "+") {return(StatementType.Addition);}
			if(tokens[nextOp].value == "-") {return(StatementType.Subtraction);}
			if(tokens[nextOp].value == "*") {return(StatementType.Multiplication);}
			if(tokens[nextOp].value == "/") {return(StatementType.Division);}

			return(StatementType.Unknown);
		}

		/*
		 takes a list of tokens that is confirmed to have at least one operation
		 
		 returns the sequence in which the operations should be processed
		 */
		static List<int> SequenceOperations(List<Token> tokens) {
			List<int> order = new List<int>();

			List<int> equals = new List<int>();
			List<int> compares = new List<int>();
			List<int> math = new List<int>();

			for(int i = 0; i < tokens.Count; i++) {
				if(tokens[i].type == TokenType.Operator) {
					if(tokens[i].value == "=") {equals.Add(i);}
					if(tokens[i].value == "==") {compares.Add(i);}
					if(tokens[i].value == "*" || tokens[i].value == "/") {math.Add(i);}
					if(tokens[i].value == "+" || tokens[i].value == "-") {math.Add(i);}
				}
			}

			order.AddRange(equals);
			order.AddRange(compares);
			order.AddRange(math);

			return(order);
		}

		/*
		 takes the raw text data from the file

		 returns a list of statements and tokens
		 (the tokens have defined types, but the statements still need to be defined)
		 */
		static List<Statement> ParseString(string input) {
			// find operators/symbols
			List<int> indices = new List<int>();
			for(int i = 0; i < input.Length; i++) {
				if(
					input[i] == '!' ||
					input[i] == '&' ||
					input[i] == '*' ||
					input[i] == '(' ||
					input[i] == ')' ||
					input[i] == '-' ||
					input[i] == '+' ||
					input[i] == '=' ||
					input[i] == '{' ||
					input[i] == '}' ||
					input[i] == ';'
				) {indices.Add(i);}
			}

			// pad operators/symbols with spaces
			for(int i = indices.Count - 1; i >= 0; i--) {
				if(input[indices[i]] != '=') {
					input = input.Insert(indices[i] + 1, " ");
					input = input.Insert(indices[i] + 0, " ");
				}
				else {
					if(indices[i] < input.Length - 1) {if(input[indices[i] + 1] != '=') {input = input.Insert(indices[i] + 1, " ");}}
					if(indices[i] > 0) {if(input[indices[i] - 1] != '=') {input = input.Insert(indices[i] + 0, " ");}}
				}
			}

			// split the statements into tokens
			List<Statement> statements = new List<Statement>();
			List<string> semiStatementStrings = new List<string>(input.Split(';'));

			// can't use split for { and } because they need to be kept
			List<string> statementStrings = new List<string>();
			for(int s = 0; s < semiStatementStrings.Count; s++) {
				if(semiStatementStrings[s].Contains("{")) {
					// split into two different statements
					// one ends with {, the other starts right after
					for(int c = 0; c < semiStatementStrings[s].Length; c++) {
						if(semiStatementStrings[s][c] == '{') {
							statementStrings.Add(semiStatementStrings[s].Substring(0, c + 1));
							if(c < semiStatementStrings[s].Length) {statementStrings.Add(semiStatementStrings[s].Substring(c + 1, semiStatementStrings[s].Length - (c + 1)));}
							break;
						}
					}
				}
				else if(semiStatementStrings[s].Contains("}")) {
					// split into three different statements
					// one ends before }, one only contains }, the last starts right after
					for(int c = 0; c < semiStatementStrings[s].Length; c++) {
						if(semiStatementStrings[s][c] == '}') {
							statementStrings.Add(semiStatementStrings[s].Substring(0, c));
							statementStrings.Add("" + semiStatementStrings[s][c]);
							if(c < semiStatementStrings[s].Length) {statementStrings.Add(semiStatementStrings[s].Substring(c + 1, semiStatementStrings[s].Length - (c + 1)));}
							break;
						}
					}
				}
				else {statementStrings.Add(semiStatementStrings[s]);}
			}

			int currentLevel = 0;
			for(int s = 0; s < statementStrings.Count; s++) {
				List<Token> tokens = new List<Token>();
				List<string> tokenStrings = new List<string>(statementStrings[s].Split(new char[]{'\n', '\r', '\t', ' '}));

				// define the tokens' types
				for(int t = 0; t < tokenStrings.Count; t++) {
					if(tokenStrings[t] != "") {
						if(CheckIfNumber(tokenStrings[t])) {tokens.Add(new Token(TokenType.Number, tokenStrings[t]));}
						else if(Types.Contains(tokenStrings[t])) {tokens.Add(new Token(TokenType.Type, tokenStrings[t]));}
						else if(Operators.Contains(tokenStrings[t])) {tokens.Add(new Token(TokenType.Operator, tokenStrings[t]));}
						else if(Keywords.Contains(tokenStrings[t])) {tokens.Add(new Token(TokenType.Keyword, tokenStrings[t]));}
						else if(Brackets.Contains(tokenStrings[t])) {tokens.Add(new Token(TokenType.Bracket, tokenStrings[t]));}
						else if(ValidateSymbolName(tokenStrings[t])) {tokens.Add(new Token(TokenType.Symbol, tokenStrings[t]));}
					}
				}

				// loop through tokens again to find negative symbols
				for(int t = 0; t < tokens.Count; t++) {
					if(tokens[t].value == "-") {
						if(tokens[t + 1].type == TokenType.Number || tokens[t + 1].type == TokenType.Symbol) {
							if(tokens[t - 1].type == TokenType.Operator) {
								tokens[t] = new Token(TokenType.Negative, tokens[t].value);
							}
						}
					}
				}

				if(tokens.Count > 0) {statements.Add(new Statement(-1, StatementType.Unknown, tokens, currentLevel));}

				if(statementStrings[s].Contains("{")) {currentLevel++;}
				if(statementStrings[s].Contains("}")) {currentLevel--;}
			}

			return(statements);
		}

		public static bool CheckIfNumber(string input) {
			if(input.StartsWith("0x")) {
				try {
					int temp = Convert.ToInt32(input, 16);
					return(true);
				}
				catch(Exception e) {return(false);}
			}
			else {
				try {
					int temp = Convert.ToInt32(input);
					return(true);
				}
				catch(Exception e) {return(false);}
			}
		}

		public static bool ValidateSymbolName(string input) {return(true);}
	}
}
