namespace Z80.C.Compiler {
	static class Program {
		static string LoadFile(string path) {
			string fileData = "";

			try {fileData = string.Join("\n", File.ReadAllLines(path));}
			catch(FileNotFoundException e) {
				Console.Error.WriteLine("File not found: " + path);
				Environment.Exit(1);
			}

			return(fileData);
		}

		public static void Main(String[] args) {
			if(args.Length != 1) {Console.Error.WriteLine("Invalid number of arguments");}

			File.WriteAllBytes("./TestROM.rom", Compiler.Compile(Lexer.Analyze(LoadFile(args[0]))));
		}
	}
}