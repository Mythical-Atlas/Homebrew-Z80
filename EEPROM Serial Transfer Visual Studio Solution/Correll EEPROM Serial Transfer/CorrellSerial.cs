using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;
using System.Windows.Threading;
using System.IO;

namespace Correll_EEPROM_Serial_Transfer {
	class CorrellSerial {
		public static SerialPort port;

		public static bool reading;
		public static bool readInit;
		public static int sectionsRead;

		public static bool writing;
		public static bool writeInit;
		public static int pagesWritten;

		public static List<byte> data;

		public static string savePath;
		public static ProgressBar barTransfer;

		public static void Initialize() {
			port = new SerialPort("COM7", 9600);
			port.ReadTimeout = 1000;
			port.WriteTimeout = 1000;

			reading = false;
			readInit = false;
			sectionsRead = 0;

			writing = false;
			writeInit = false;
			pagesWritten = 0;

			data = new List<byte>();
		}

		public static void Update() {
			if(reading) {
				if(!readInit) {
					readInit = true;
					sectionsRead = 0;
					data = new List<byte>();

					barTransfer.Maximum = 1024 * 8;
				}
				else {
					byte[] tempBuf = new byte[1024];
					int numBytes = port.Read(tempBuf, 0, 1024);
					data.AddRange(tempBuf);
					data.RemoveRange(data.Count - (1024 - numBytes), (1024 - numBytes));

					barTransfer.Value = data.Count;

					if(data.Count == 1024 * 8) {
						reading = false;
						readInit = false;

						File.WriteAllBytes(savePath, data.ToArray());

						barTransfer.Value = 0;
						MessageBox.Show("ROM read succeeded.", "Transfer Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				}
			}
			if(writing) {
				if(!writeInit) {
					writeInit = true;
					pagesWritten = 0;

					barTransfer.Maximum = 1024 * 8 / 32;
				}
				else {
					if(port.ReadLine() == "ready") {
						byte[] tempBuf = new byte[32];
						for(int i = 0; i < 32; i++) {tempBuf[i] = data[i + pagesWritten * 32];}

						port.Write(tempBuf, 0, 32);

						//Console.WriteLine("wrote page " + pagesWritten);
						pagesWritten++;
						barTransfer.Value = pagesWritten;

						if(pagesWritten == 1024 * 8 / 32) {
							writing = false;
							writeInit = false;

							barTransfer.Value = 0;
							MessageBox.Show("ROM write succeeded.", "Transfer Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
					}
				}
			}
		}
	}
}
