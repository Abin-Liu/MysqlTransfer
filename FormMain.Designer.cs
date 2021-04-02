
namespace MysqlTransfer
{
	partial class FormMain
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
			this.label1 = new System.Windows.Forms.Label();
			this.txtRemoteServer = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cmbRemoteDatabases = new System.Windows.Forms.ComboBox();
			this.btnRemoteListDatabase = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.txtRemotePassword = new System.Windows.Forms.TextBox();
			this.txtRemoteUserName = new System.Windows.Forms.TextBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.txtLocalPassword = new System.Windows.Forms.TextBox();
			this.txtLocalUserName = new System.Windows.Forms.TextBox();
			this.txtLocalBinFolder = new System.Windows.Forms.TextBox();
			this.btnStart = new System.Windows.Forms.Button();
			this.btnExit = new System.Windows.Forms.Button();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.radAll = new System.Windows.Forms.RadioButton();
			this.radStructureOnly = new System.Windows.Forms.RadioButton();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(17, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(79, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "服务器地址：";
			// 
			// txtRemoteServer
			// 
			this.txtRemoteServer.Location = new System.Drawing.Point(114, 23);
			this.txtRemoteServer.Name = "txtRemoteServer";
			this.txtRemoteServer.Size = new System.Drawing.Size(356, 20);
			this.txtRemoteServer.TabIndex = 1;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radStructureOnly);
			this.groupBox1.Controls.Add(this.radAll);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.cmbRemoteDatabases);
			this.groupBox1.Controls.Add(this.btnRemoteListDatabase);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.txtRemotePassword);
			this.groupBox1.Controls.Add(this.txtRemoteUserName);
			this.groupBox1.Controls.Add(this.txtRemoteServer);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(491, 167);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "源数据库";
			// 
			// cmbRemoteDatabases
			// 
			this.cmbRemoteDatabases.FormattingEnabled = true;
			this.cmbRemoteDatabases.Location = new System.Drawing.Point(114, 101);
			this.cmbRemoteDatabases.Name = "cmbRemoteDatabases";
			this.cmbRemoteDatabases.Size = new System.Drawing.Size(314, 21);
			this.cmbRemoteDatabases.TabIndex = 7;
			// 
			// btnRemoteListDatabase
			// 
			this.btnRemoteListDatabase.Image = global::MysqlTransfer.Properties.Resources.DatabaseList;
			this.btnRemoteListDatabase.Location = new System.Drawing.Point(434, 100);
			this.btnRemoteListDatabase.Name = "btnRemoteListDatabase";
			this.btnRemoteListDatabase.Size = new System.Drawing.Size(36, 24);
			this.btnRemoteListDatabase.TabIndex = 8;
			this.btnRemoteListDatabase.UseVisualStyleBackColor = true;
			this.btnRemoteListDatabase.Click += new System.EventHandler(this.btnRemoteListDatabase_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(17, 78);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(67, 13);
			this.label4.TabIndex = 4;
			this.label4.Text = "登录密码：";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(17, 104);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(79, 13);
			this.label5.TabIndex = 6;
			this.label5.Text = "目标数据库：";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(17, 52);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(79, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "登录用户名：";
			// 
			// txtRemotePassword
			// 
			this.txtRemotePassword.Location = new System.Drawing.Point(114, 75);
			this.txtRemotePassword.Name = "txtRemotePassword";
			this.txtRemotePassword.Size = new System.Drawing.Size(356, 20);
			this.txtRemotePassword.TabIndex = 5;
			this.txtRemotePassword.UseSystemPasswordChar = true;
			// 
			// txtRemoteUserName
			// 
			this.txtRemoteUserName.Location = new System.Drawing.Point(114, 49);
			this.txtRemoteUserName.Name = "txtRemoteUserName";
			this.txtRemoteUserName.Size = new System.Drawing.Size(356, 20);
			this.txtRemoteUserName.TabIndex = 3;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.btnBrowse);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.txtLocalPassword);
			this.groupBox2.Controls.Add(this.txtLocalUserName);
			this.groupBox2.Controls.Add(this.txtLocalBinFolder);
			this.groupBox2.Location = new System.Drawing.Point(12, 195);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(491, 114);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "本地数据库";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(17, 78);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(67, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "登录密码：";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(17, 52);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(79, 13);
			this.label7.TabIndex = 2;
			this.label7.Text = "登录用户名：";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(17, 26);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(94, 13);
			this.label8.TabIndex = 0;
			this.label8.Text = "Mysql程序位置：";
			// 
			// txtLocalPassword
			// 
			this.txtLocalPassword.Location = new System.Drawing.Point(114, 75);
			this.txtLocalPassword.Name = "txtLocalPassword";
			this.txtLocalPassword.Size = new System.Drawing.Size(356, 20);
			this.txtLocalPassword.TabIndex = 5;
			this.txtLocalPassword.UseSystemPasswordChar = true;
			// 
			// txtLocalUserName
			// 
			this.txtLocalUserName.Location = new System.Drawing.Point(114, 49);
			this.txtLocalUserName.Name = "txtLocalUserName";
			this.txtLocalUserName.Size = new System.Drawing.Size(356, 20);
			this.txtLocalUserName.TabIndex = 3;
			// 
			// txtLocalBinFolder
			// 
			this.txtLocalBinFolder.BackColor = System.Drawing.SystemColors.Window;
			this.txtLocalBinFolder.Location = new System.Drawing.Point(114, 23);
			this.txtLocalBinFolder.Name = "txtLocalBinFolder";
			this.txtLocalBinFolder.ReadOnly = true;
			this.txtLocalBinFolder.Size = new System.Drawing.Size(314, 20);
			this.txtLocalBinFolder.TabIndex = 1;
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(165, 323);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(75, 23);
			this.btnStart.TabIndex = 2;
			this.btnStart.Text = "Start";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// btnExit
			// 
			this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnExit.Location = new System.Drawing.Point(283, 323);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(75, 23);
			this.btnExit.TabIndex = 3;
			this.btnExit.Text = "Exit";
			this.btnExit.UseVisualStyleBackColor = true;
			this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
			// 
			// btnBrowse
			// 
			this.btnBrowse.Image = global::MysqlTransfer.Properties.Resources.Browse;
			this.btnBrowse.Location = new System.Drawing.Point(434, 21);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(36, 24);
			this.btnBrowse.TabIndex = 6;
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(17, 132);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(67, 13);
			this.label6.TabIndex = 9;
			this.label6.Text = "迁移内容：";
			// 
			// radAll
			// 
			this.radAll.AutoSize = true;
			this.radAll.Checked = true;
			this.radAll.Location = new System.Drawing.Point(114, 130);
			this.radAll.Name = "radAll";
			this.radAll.Size = new System.Drawing.Size(85, 17);
			this.radAll.TabIndex = 10;
			this.radAll.TabStop = true;
			this.radAll.Text = "结构和数据";
			this.radAll.UseVisualStyleBackColor = true;
			// 
			// radStructureOnly
			// 
			this.radStructureOnly.AutoSize = true;
			this.radStructureOnly.Location = new System.Drawing.Point(232, 132);
			this.radStructureOnly.Name = "radStructureOnly";
			this.radStructureOnly.Size = new System.Drawing.Size(61, 17);
			this.radStructureOnly.TabIndex = 10;
			this.radStructureOnly.Text = "仅结构";
			this.radStructureOnly.UseVisualStyleBackColor = true;
			// 
			// FormMain
			// 
			this.AcceptButton = this.btnStart;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnExit;
			this.ClientSize = new System.Drawing.Size(520, 360);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormMain";
			this.Text = "Mysql数据传输";
			this.Load += new System.EventHandler(this.FormMain_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtRemoteServer;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtRemotePassword;
		private System.Windows.Forms.TextBox txtRemoteUserName;
		private System.Windows.Forms.Button btnRemoteListDatabase;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtLocalPassword;
		private System.Windows.Forms.TextBox txtLocalUserName;
		private System.Windows.Forms.TextBox txtLocalBinFolder;
		private System.Windows.Forms.ComboBox cmbRemoteDatabases;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.RadioButton radStructureOnly;
		private System.Windows.Forms.RadioButton radAll;
		private System.Windows.Forms.Label label6;
	}
}

