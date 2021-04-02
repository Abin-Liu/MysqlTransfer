using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ToolkitForms;

namespace MysqlTransfer
{
	public partial class FormMain : Form
	{
		TransferData m_data;

		public FormMain()
		{
			m_data = new TransferData();
			InitializeComponent();
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
			txtRemoteServer.Text = m_data.RemoteServer;
			txtRemoteUserName.Text = m_data.RemoteUserName;
			txtRemotePassword.Text = m_data.RemotePassword;
			txtLocalBinFolder.Text = m_data.LocalBinFolder;
			txtLocalUserName.Text = m_data.LocalUserName;
			txtLocalPassword.Text = m_data.LocalPassword;
		}

		private void btnRemoteListDatabase_Click(object sender, EventArgs e)
		{
			this.Enabled = false;

			m_data.RemoteServer = txtRemoteServer.Text;
			m_data.RemoteUserName = txtRemoteUserName.Text;
			m_data.RemotePassword = txtRemotePassword.Text;

			List<string> databaseNames = null;
			try
			{
				databaseNames = m_data.ListRemoteDatabases();
				m_data.SaveRemoteConfig();
			}
			catch (Exception ex)
			{
				this.Enabled = true;
				MessageForm.Error(this, ex.Message);
				return;
			}			

			cmbRemoteDatabases.Items.Clear();
			cmbRemoteDatabases.Text = "";
			foreach (string name in databaseNames)
			{
				cmbRemoteDatabases.Items.Add(name);
			}

			if (databaseNames.Count > 0)
			{
				cmbRemoteDatabases.Text = databaseNames[0];
			}

			cmbRemoteDatabases.Focus();
			cmbRemoteDatabases.SelectAll();
			this.Enabled = true;			
		}		

		private void btnExit_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			m_data.RemoteDatabase = cmbRemoteDatabases.Text;
			m_data.LocalBinFolder = txtLocalBinFolder.Text;
			m_data.LocalUserName = txtLocalUserName.Text;
			m_data.LocalPassword = txtLocalPassword.Text;
			m_data.StructureOnly = radStructureOnly.Checked;

			if (m_data.RemoteDatabase == "")
			{
				MessageForm.Error(this, "目标数据库名称不能为空");
				cmbRemoteDatabases.Focus();
				cmbRemoteDatabases.SelectAll();
				return;
			}

			try
			{
				m_data.VerifyLocalBinFolder();
			}
			catch (Exception ex)
			{
				MessageForm.Error(this, ex.Message);
				txtLocalBinFolder.Focus();
				txtLocalBinFolder.SelectAll();
				return;
			}			

			if (m_data.LocalUserName == "")
			{
				MessageForm.Error(this, "本地数据库用户名不能为空");
				txtLocalUserName.Focus();
				txtLocalUserName.SelectAll();
				return;
			}			

			m_data.SaveLocalConfig();


			TaskForm form = new TaskForm();
			form.Title = "数据库迁移";
			form.Message = string.Format("正在传输: {0}/{1}", m_data.RemoteServer, m_data.RemoteDatabase);
			form.TaskProc = DoTransfer;
			form.AllowAbort = false;

			if (form.ShowDialog(this) != DialogResult.OK)
			{
				MessageForm.Error(this, form.Error);
				return;
			}			

			MessageForm.Info(this, "数据库迁移成功: " + m_data.RemoteDatabase);
		}

		private void DoTransfer()
		{
			m_data.Process();
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Title = "定位Mysql.exe";
			dlg.Filter = "Mysql主程序文件|mysql.exe;mysqldump.exe";
			dlg.CheckFileExists = true;
			dlg.InitialDirectory = txtLocalBinFolder.Text;
			dlg.FileName = "mysql.exe";

			if (dlg.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			txtLocalBinFolder.Text = Path.GetDirectoryName(dlg.FileName);
			txtLocalBinFolder.Focus();
		}
	}
}
