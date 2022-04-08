
namespace Correll_EEPROM_Serial_Transfer
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.dlgOpenROM = new System.Windows.Forms.OpenFileDialog();
			this.btnWriteROM = new System.Windows.Forms.Button();
			this.btnFindArduino = new System.Windows.Forms.Button();
			this.btnReadROM = new System.Windows.Forms.Button();
			this.lblConnectionStatus = new System.Windows.Forms.Label();
			this.btnEndConnection = new System.Windows.Forms.Button();
			this.dlgSaveROM = new System.Windows.Forms.SaveFileDialog();
			this.lblTransferStatus = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.barTransfer = new System.Windows.Forms.ProgressBar();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// dlgOpenROM
			// 
			this.dlgOpenROM.DefaultExt = "rom";
			this.dlgOpenROM.Filter = "ROM files|*.rom|All files|*.*";
			this.dlgOpenROM.FileOk += new System.ComponentModel.CancelEventHandler(this.dlgOpenROM_FileOk);
			// 
			// btnWriteROM
			// 
			this.btnWriteROM.Enabled = false;
			this.btnWriteROM.Location = new System.Drawing.Point(3, 32);
			this.btnWriteROM.Name = "btnWriteROM";
			this.btnWriteROM.Size = new System.Drawing.Size(75, 23);
			this.btnWriteROM.TabIndex = 2;
			this.btnWriteROM.Text = "Write ROM";
			this.btnWriteROM.UseVisualStyleBackColor = true;
			this.btnWriteROM.Click += new System.EventHandler(this.btnWriteROM_Click);
			// 
			// btnFindArduino
			// 
			this.btnFindArduino.Location = new System.Drawing.Point(3, 3);
			this.btnFindArduino.Name = "btnFindArduino";
			this.btnFindArduino.Size = new System.Drawing.Size(92, 23);
			this.btnFindArduino.TabIndex = 3;
			this.btnFindArduino.Text = "Find Arduino";
			this.btnFindArduino.UseVisualStyleBackColor = true;
			this.btnFindArduino.Click += new System.EventHandler(this.btnFindArduino_Click);
			// 
			// btnReadROM
			// 
			this.btnReadROM.Enabled = false;
			this.btnReadROM.Location = new System.Drawing.Point(3, 3);
			this.btnReadROM.Name = "btnReadROM";
			this.btnReadROM.Size = new System.Drawing.Size(75, 23);
			this.btnReadROM.TabIndex = 5;
			this.btnReadROM.Text = "Read ROM";
			this.btnReadROM.UseVisualStyleBackColor = true;
			this.btnReadROM.Click += new System.EventHandler(this.btnReadROM_Click);
			// 
			// lblConnectionStatus
			// 
			this.lblConnectionStatus.AutoSize = true;
			this.lblConnectionStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblConnectionStatus.Location = new System.Drawing.Point(269, 22);
			this.lblConnectionStatus.Name = "lblConnectionStatus";
			this.lblConnectionStatus.Size = new System.Drawing.Size(78, 13);
			this.lblConnectionStatus.TabIndex = 6;
			this.lblConnectionStatus.Text = "No Connection";
			// 
			// btnEndConnection
			// 
			this.btnEndConnection.Enabled = false;
			this.btnEndConnection.Location = new System.Drawing.Point(3, 32);
			this.btnEndConnection.Name = "btnEndConnection";
			this.btnEndConnection.Size = new System.Drawing.Size(92, 23);
			this.btnEndConnection.TabIndex = 7;
			this.btnEndConnection.Text = "End Connection";
			this.btnEndConnection.UseVisualStyleBackColor = true;
			this.btnEndConnection.Click += new System.EventHandler(this.btnEndConnection_Click);
			// 
			// dlgSaveROM
			// 
			this.dlgSaveROM.DefaultExt = "rom";
			this.dlgSaveROM.Filter = "ROM files|*.rom|All files|*.*";
			this.dlgSaveROM.FileOk += new System.ComponentModel.CancelEventHandler(this.dlgSaveROM_FileOk);
			// 
			// lblTransferStatus
			// 
			this.lblTransferStatus.AutoSize = true;
			this.lblTransferStatus.Location = new System.Drawing.Point(218, 8);
			this.lblTransferStatus.Name = "lblTransferStatus";
			this.lblTransferStatus.Size = new System.Drawing.Size(118, 13);
			this.lblTransferStatus.TabIndex = 8;
			this.lblTransferStatus.Text = "No Transfer in Progress";
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.btnFindArduino);
			this.panel1.Controls.Add(this.lblConnectionStatus);
			this.panel1.Controls.Add(this.btnEndConnection);
			this.panel1.Location = new System.Drawing.Point(12, 12);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(392, 63);
			this.panel1.TabIndex = 9;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(101, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(162, 13);
			this.label1.TabIndex = 8;
			this.label1.Text = "Arduino Connection Status:";
			// 
			// panel2
			// 
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel2.Controls.Add(this.label2);
			this.panel2.Controls.Add(this.barTransfer);
			this.panel2.Controls.Add(this.lblTransferStatus);
			this.panel2.Controls.Add(this.btnReadROM);
			this.panel2.Controls.Add(this.btnWriteROM);
			this.panel2.Location = new System.Drawing.Point(12, 81);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(392, 65);
			this.panel2.TabIndex = 10;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(82, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(130, 13);
			this.label2.TabIndex = 10;
			this.label2.Text = "ROM Transfer Status:";
			// 
			// barTransfer
			// 
			this.barTransfer.Location = new System.Drawing.Point(85, 32);
			this.barTransfer.Name = "barTransfer";
			this.barTransfer.Size = new System.Drawing.Size(302, 23);
			this.barTransfer.TabIndex = 9;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 161);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(132, 13);
			this.label3.TabIndex = 11;
			this.label3.Text = "Copyright 2021 Ben Correll";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(344, 161);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(60, 13);
			this.label4.TabIndex = 12;
			this.label4.Text = "Version 1.0";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(416, 183);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Correll EEPROM Serial Transfer";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Shown += new System.EventHandler(this.Form1_Shown);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.OpenFileDialog dlgOpenROM;
		private System.Windows.Forms.Button btnWriteROM;
		private System.Windows.Forms.Button btnFindArduino;
		private System.Windows.Forms.Button btnReadROM;
		public System.Windows.Forms.Label lblConnectionStatus;
		private System.Windows.Forms.Button btnEndConnection;
		private System.Windows.Forms.SaveFileDialog dlgSaveROM;
		public System.Windows.Forms.Label lblTransferStatus;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.ProgressBar barTransfer;
		public System.Windows.Forms.Label label1;
		public System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
	}
}

