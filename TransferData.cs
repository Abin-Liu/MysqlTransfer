using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using MFGLib;

namespace MysqlTransfer
{
	class TransferData
	{
		public string RemoteServer { get; set; }
		public string RemoteUserName { get; set; }
		public string RemotePassword { get; set; }
		public string RemoteDatabase { get; set; }
		public string RemoteConnectionString => string.Format("server={0};uid={1};pwd={2}", RemoteServer, RemoteUserName, RemotePassword);
		public bool StructureOnly { get; set; }

		public string LocalBinFolder { get; set; }
		public string LocalUserName { get; set; }
		public string LocalPassword { get; set; }
		public string LocalConnectionString => string.Format("server=localhost;uid={0};pwd={1}", LocalUserName, LocalPassword);

		const string ProductName = "MysqlTransfer";
		const string EncryptKey = "!Mysql.Tr@nsfer&__";

		public TransferData()
		{
			RegistryHelper reg = new RegistryHelper();
			reg.Open("Abin", ProductName, false);
			RemoteServer = reg.ReadString("RemoteServer");
			RemoteUserName = reg.ReadString("RemoteUserName");
			RemotePassword = CryptoHelper.Decrypt(reg.ReadString("RemotePassword"), EncryptKey);
			LocalBinFolder = reg.ReadString("LocalBinFolder");
			LocalUserName = reg.ReadString("LocalUserName");
			//LocalPassword = CryptoHelper.Decrypt(reg.ReadString("LocalPassword"), EncryptKey);
			reg.Close();
		}

		public void SaveRemoteConfig()
		{
			RegistryHelper reg = new RegistryHelper();
			reg.Open("Abin", ProductName, true);
			reg.WriteString("RemoteServer", RemoteServer);
			reg.WriteString("RemoteUserName", RemoteUserName);
			reg.WriteString("RemotePassword", CryptoHelper.Encrypt(RemotePassword, EncryptKey));
			reg.Close();
		}

		public void SaveLocalConfig()
		{
			RegistryHelper reg = new RegistryHelper();
			reg.Open("Abin", ProductName, true);
			reg.WriteString("LocalBinFolder", LocalBinFolder);
			reg.WriteString("LocalUserName", LocalUserName);
			//reg.WriteString("LocalPassword", CryptoHelper.Encrypt(LocalPassword, EncryptKey));
			reg.Close();
		}

		static readonly string[] BuiltinDatabases = { "mysql", "information_schema", "performance_schema" };

		public List<string> ListRemoteDatabases()
		{
			MysqlHelper db = new MysqlHelper(RemoteConnectionString);
			DataTable dt = db.QueryDataTable("show databases");
			List<string> names = new List<string>();
			foreach (DataRow dr in dt.Rows)
			{
				string name = SafeConvert.ToString(dr[0]);
				if (string.IsNullOrEmpty(name) || Array.IndexOf(BuiltinDatabases, name.ToLower()) != -1)
				{
					continue;
				}

				names.Add(SafeConvert.ToString(dr[0]));
			}
			return names;
		}

		public void VerifyLocalBinFolder()
		{
			if (!File.Exists(LocalBinFolder + "\\mysql.exe") || !File.Exists(LocalBinFolder + "\\mysqldump.exe"))
			{
				throw new Exception("指定目录未找到mysql.exe或mysqldump.exe");				
			}
		}

		static void ExecuteCommand(string command)
		{
			Process process = new Process();
			process.StartInfo.FileName = "cmd.exe";
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardInput = true;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;
			process.Start();

			process.StandardInput.WriteLine(command);
			process.StandardInput.WriteLine("exit");
			string error = process.StandardError.ReadToEnd();
			error = error.Replace("Warning: Using a password on the command line interface can be insecure.", "");
			error = error.Trim();

			if (error.ToLower().Contains("error"))
			{
				throw new Exception(error);
			}
		}		

		public void Process()
		{
			MysqlHelper db = new MysqlHelper(LocalConnectionString);
			db.Open();
			try
			{
				db.Execute("DROP DATABASE " + RemoteDatabase);
			}
			catch
			{
			}

			try
			{
				db.Execute("CREATE DATABASE " + RemoteDatabase);
			}
			finally
			{
				db.Close();
			}

			// 命令格式：
			// mysqldump --host=wuxengser05 -uroot -pJabil12345 -C --no-data --databases meoee |mysql --host=localhost -uroot -pJabil12345 meoee
			string command = string.Format("\"{0}\\mysqldump.exe\" --host={1} -u{2} -p{3} -C {4} --databases {5} | \"{0}\\mysql.exe\" --host=localhost -u{6} -p{7} {5}",
				LocalBinFolder, // 0
				RemoteServer, // 1
				RemoteUserName, // 2
				RemotePassword, // 3
				StructureOnly ? "--no-data " : "", // 4
				RemoteDatabase, // 5
				LocalUserName, // 6
				LocalPassword); // 7

			ExecuteCommand(command);
		}
	}
}
