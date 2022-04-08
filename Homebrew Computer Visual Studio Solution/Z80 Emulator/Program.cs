using Terminal.Gui;
using NStack;

namespace Z80.Emulator {
	public class Program {
		public static bool running = true;

		static int instructionCycle = 0;

		public static Label pcRegLabel;
		public static Label flagsBinLabel;
		public static Label flagsHexLabel;
		public static Label aRegLabel;
		public static Label bRegLabel;
		public static Label cRegLabel;
		public static Label dRegLabel;
		public static Label eRegLabel;
		public static Label hRegLabel;
		public static Label lRegLabel;
		public static Label bcRegLabel;
		public static Label deRegLabel;
		public static Label hlRegLabel;
		public static Label spRegLabel;

		public static Label cpuCyclesLabel;

		public static Label previousInstLabel;
		public static Label nextInstLabel;

		public static byte[] LoadFile(string path) {
			byte[] fileData = new byte[0];

			try {
				FileStream file = File.OpenRead(path);
				fileData = new byte[(int)file.Length];
				file.Read(fileData, 0, (int)file.Length);
				file.Close();
			}
			catch(FileNotFoundException e) {
				Console.Error.WriteLine("File not found: " + path);
				Environment.Exit(1);
			}

			if(fileData.Length != 8 * 1024) {Console.Error.WriteLine("File is incorrect size (must be exactly 8 KB)");}

			return(fileData);
		}

		public static void Main(string[] args) {
			if(args.Length != 1) {
				Console.Error.WriteLine("Invalid number of arguments");
				Environment.Exit(1);
			}

			MemoryAndRegisters.LoadROM(LoadFile(args[0]));

			Console.WriteLine("Successfully loaded ROM");
			Console.WriteLine("Beginning execution\n");

			Application.Init();
			var top = Application.Top;

			var cpuWin = new Window("CPU Status") {X = Pos.Left(top), Y = Pos.Top(top), Width = Dim.Percent(50), Height = Dim.Percent(20)};
			var regWin = new Window("Register Values") {X = Pos.Left(top), Y = Pos.Bottom(cpuWin), Width = Dim.Percent(50), Height = Dim.Percent(60)};
			var conWin = new Window("Controls") {X = Pos.Left(top), Y = Pos.Bottom(regWin), Width = Dim.Percent(50), Height = Dim.Fill()};
			var insWin = new Window("Instruction Info") {X = Pos.Right(cpuWin), Y = Pos.Top(top), Width = Dim.Fill(), Height = Dim.Fill()};

			top.Add(cpuWin);
			top.Add(regWin);
			top.Add(insWin);
			top.Add(conWin);

			cpuCyclesLabel = new Label("CPU Cycles (not actual cycles): 0") {X = 2, Y = 1};

			cpuWin.Add(cpuCyclesLabel);

			pcRegLabel = new Label("Program Counter: 0x0000") {X = 2, Y = 1};
			Label flagsIDLabel = new Label("         SZ H PNC") {X = 2, Y = 2};
			flagsBinLabel = new Label("Flags: 0b00000000") {X = 2, Y = 3};
			flagsHexLabel = new Label("Flags: 0x00") {X = 2, Y = 4};
			spRegLabel = new Label("Stack Pointer: 0x0000") {X = 2, Y = 5};
			aRegLabel = new Label("A: 0x00") {X = 2, Y = 6};
			bRegLabel = new Label("B: 0x00") {X = 2, Y = 7};
			cRegLabel = new Label("C: 0x00") {X = 2, Y = 8};
			dRegLabel = new Label("D: 0x00") {X = 2, Y = 9};
			eRegLabel = new Label("E: 0x00") {X = 2, Y = 10};
			bcRegLabel = new Label("BC: 0x0000") {X = 2, Y = 11};
			deRegLabel = new Label("DE: 0x0000") {X = 2, Y = 12};
			hlRegLabel = new Label("HL: 0x0000") {X = 2, Y = 13};

			regWin.Add(
				pcRegLabel,
				flagsIDLabel,
				flagsBinLabel,
				flagsHexLabel,
				spRegLabel,
				aRegLabel,
				bRegLabel,
				cRegLabel,
				dRegLabel,
				eRegLabel,
				bcRegLabel,
				deRegLabel,
				hlRegLabel
			);

			previousInstLabel = new Label("Previous Instruction: N/A") {X = 2, Y = 1};
			nextInstLabel = new Label("Next Instruction:") {X = 2, Y = 2};

			insWin.Add(previousInstLabel, nextInstLabel);

			Button stepButton = new Button("Step") {X = 2, Y = 1};
			stepButton.Clicked += () => {instructionCycle = 1;};

			Button resetButton = new Button("Reset") {X = 11, Y = 1};
			resetButton.Clicked += () => {
				MemoryAndRegisters.Reset();
				CPU.cycleCount = 0;
				CPU.UpdateDebug();
				instructionCycle = 0;
			};

			conWin.Add(stepButton, resetButton);

			Application.RunState appState = Application.Begin(top);
			while(running) {
				Application.RunLoop(appState, false);

				if(instructionCycle != 0) {
					CPU.Step();
					CPU.UpdateDebug();
					if(instructionCycle > 0) {instructionCycle--;}
				}
			}
		}
	}
}