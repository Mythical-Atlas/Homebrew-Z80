using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Correll_EEPROM_Serial_Transfer {
	public partial class Form1 : Form {
		bool fileGood;

		public Form1() {
			InitializeComponent();

			CorrellSerial.Initialize();
			CorrellSerial.barTransfer = barTransfer;

			fileGood = false;
		}

		private void Form1_Shown(object sender, EventArgs e) {
			while(Application.OpenForms.Count > 0) {
				Application.DoEvents();

				UpdateConnectionStatus();
				UpdateTransferStatus();

				CorrellSerial.Update();
			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
			if(CorrellSerial.writing || CorrellSerial.reading) {
				DialogResult result = MessageBox.Show("Are you sure you want to exit? Exiting during a transfer will necessitate restarting the Arduino and it may corrupt data on the ROM.", "Transfer in Progress", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
				if(result == DialogResult.No) {e.Cancel = true;}
			}
		}

		private void btnFindArduino_Click(object sender, EventArgs e) {
			if(!CorrellSerial.port.IsOpen) {
				try {
					CorrellSerial.port.Open();
					CorrellSerial.port.Write("check CSTS handshake\n");

					if(CorrellSerial.port.ReadLine() != "CSTS handshake confirm") {
						CorrellSerial.port.Close();
						CorrellSerial.port.Dispose();

						MessageBox.Show("Arduino could not be found. Try reseting the Arduino.", "Serial Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						Console.WriteLine("Couldn't open a serial connection.");
					}
				}
				catch(Exception exception) {
					CorrellSerial.port.Close();
					CorrellSerial.port.Dispose();

					MessageBox.Show("Arduino could not be found. Try reseting the Arduino.", "Serial Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					Console.WriteLine("Couldn't open a serial connection.");
				}

				if(CorrellSerial.port.IsOpen) {
					btnFindArduino.Enabled = false;
					btnEndConnection.Enabled = true;
				}
			}

			//if(SerialThread.port.IsOpen) {lblConnectionStatus.Text = "Arduino Connection Status: Connected";}
			//else {lblConnectionStatus.Text = "Arduino Connection Status: No Connection";}
		}

		private void btnEndConnection_Click(object sender, EventArgs e) {
			if(CorrellSerial.port.IsOpen) {
				btnFindArduino.Enabled = true;
				btnEndConnection.Enabled = false;
				btnReadROM.Enabled = false;
				btnWriteROM.Enabled = false;
				lblConnectionStatus.Text = "No Connection";

				CorrellSerial.port.Close();
				CorrellSerial.port.Dispose();
			}
		}

		public void UpdateConnectionStatus() {
			if(CorrellSerial.port.IsOpen && lblConnectionStatus.Text == "No Connection") {lblConnectionStatus.Text = "Connected on COM7";}
			else if(!CorrellSerial.port.IsOpen && lblConnectionStatus.Text == "Connected on COM7") {
				btnFindArduino.Enabled = true;
				btnEndConnection.Enabled = false;
				btnReadROM.Enabled = false;
				btnWriteROM.Enabled = false;
				lblConnectionStatus.Text = "No Connection";

				MessageBox.Show("Arduino connection lost unexpectedly.", "Serial Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public void UpdateTransferStatus() {
			if(CorrellSerial.port.IsOpen) {
				if(CorrellSerial.reading) {
					lblTransferStatus.Text = "Reading from ROM to File";

					btnFindArduino.Enabled = false;
					btnEndConnection.Enabled = false;
					btnReadROM.Enabled = false;
					btnWriteROM.Enabled = false;

					this.UseWaitCursor = true;
				}
				else if(CorrellSerial.writing) {
					lblTransferStatus.Text = "Writing from File to ROM";

					btnFindArduino.Enabled = false;
					btnEndConnection.Enabled = false;
					btnReadROM.Enabled = false;
					btnWriteROM.Enabled = false;

					this.UseWaitCursor = true;
				}
				else {
					lblTransferStatus.Text = "No Transfer in Progress";

					this.UseWaitCursor = false;

					btnEndConnection.Enabled = true;
					btnReadROM.Enabled = true;
					btnWriteROM.Enabled = true;
				}
			}
		}

		private void btnReadROM_Click(object sender, EventArgs e) {
			if(CorrellSerial.port.IsOpen && !CorrellSerial.writing) {
				fileGood = false;
				dlgSaveROM.ShowDialog();

				if(fileGood) {
					if(dlgSaveROM.FileName != "") {
						CorrellSerial.savePath = dlgSaveROM.FileName;

						CorrellSerial.port.Write("read\n");
						CorrellSerial.reading = true;
					}
				}
			}
		}

		private void btnWriteROM_Click(object sender, EventArgs e) {
			if(CorrellSerial.port.IsOpen && !CorrellSerial.reading) {
				fileGood = false;
				dlgOpenROM.ShowDialog();

				if(fileGood) {
					try {CorrellSerial.data = new List<byte>(File.ReadAllBytes(dlgOpenROM.FileName));}
					catch(Exception exception) {}

					if(File.Exists(dlgOpenROM.FileName)) {
						if(CorrellSerial.data.Count == 8 * 1024) {
							CorrellSerial.port.Write("write\n");
							CorrellSerial.writing = true;
						}
						else {MessageBox.Show("ROM file must be exactly 8 kb in size.", "ROM File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);}
					}
				}
			}
		}

		private void dlgOpenROM_FileOk(object sender, CancelEventArgs e) {fileGood = true;}
		private void dlgSaveROM_FileOk(object sender, CancelEventArgs e) {fileGood = true;}
	}
}
