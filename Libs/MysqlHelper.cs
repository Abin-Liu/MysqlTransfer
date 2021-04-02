//////////////////////////////////////////////////////////
// MysqlHelper
//
// DatabaseHelper派生类，封装了MySql.Data.MySqlClient类库中相关
// API，用以操作Mysql数据库。
//
// Abin
// 2018-3-12
//////////////////////////////////////////////////////////

using System;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace MFGLib
{
	/// <summary>
	/// DatabaseHelper派生类，封装了MySql.Data.MySqlClient类库中相关API，用以操作Mysql数据库。
	/// </summary>
	public class MysqlHelper : DatabaseHelper
	{
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="connectionString">连接字符串</param>
		public MysqlHelper(string connectionString) : base(connectionString)
		{
		}

		/// <summary>
		/// 重载NewConnection()
		/// </summary>
		/// <returns>DbConnection对象</returns>
		protected override DbConnection NewConnection()
		{
			return new MySqlConnection();
		}

		/// <summary>
		/// 重载NewCommand()
		/// </summary>
		/// <returns>DbCommand对象</returns>
		protected override DbCommand NewCommand()
		{
			return new MySqlCommand();
		}

		/// <summary>
		/// 重载NewParameter()
		/// </summary>
		/// <returns>DbParameter对象</returns>
		protected override DbParameter NewParameter()
		{
			return new MySqlParameter();
		}

		/// <summary>
		/// 重载NewDataAdapter()
		/// </summary>
		/// <returns>DbDataAdapter对象</returns>
		protected override DbDataAdapter NewDataAdapter()
		{
			return new MySqlDataAdapter();
		}

		/// <summary>
		/// 重载GetRecentIdentitySql()
		/// </summary>
		/// <returns>Mysql特有字符串</returns>
		protected override string GetRecentIdentitySql()
		{
			return "SELECT LAST_INSERT_ID()";
		}
	}
}
