//////////////////////////////////////////////////////////
// DatabaseHelper
//
// 数据库访问抽象基类，派生类必须重载以下方法：
//
// DbConnection NewConnection() - 创建新DbConnection对象
// DbCommand NewCommand() - 创建新DbCommand对象
// DbParameter NewParameter() - 创建新DbParameter查询或执行参数对象
// DbDataAdapter NewDataAdapter() -- 创建新DbDataAdapter对象
//
// Abin
// 2018-3-12
//////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace MFGLib
{
	/// <summary>
	/// 数据库访问抽象基类
	/// </summary>
	public abstract class DatabaseHelper : IDisposable
	{
		#region 公开属性
		/// <summary> 
		/// 数据库连接字符串 
		/// </summary> 		
		public virtual string ConnectionString
		{
			get
			{
				return m_connection.ConnectionString;
			}

			protected set
			{				
				m_connection.ConnectionString = value;
			}
		}		
		
		/// <summary> 
		/// 检查数据库连接是否已打开 
		/// </summary> 	
		public virtual bool Opened
		{
			get
			{
				return m_connection.State == ConnectionState.Open;
			}
		}

		/// <summary>
		/// 检查数据库连接当前是否在事务中
		/// </summary>
		public bool OnTransaction
		{
			get
			{
				return m_transaction != null;
			}
		}
		#endregion

		#region 构造与析构
		/// <summary> 
		/// 默认构造函数
		/// </summary> 	
		public DatabaseHelper()
		{
			m_connection = NewConnection();
		}

		/// <summary> 
		/// 构造函数，以连接字符串为参数
		/// </summary> 	
		/// <param name="connectionString">连接字符串</param>
		public DatabaseHelper(string connectionString)
		{
			m_connection = NewConnection();
			ConnectionString = connectionString;
		}

		/// <summary>
		/// 析构函数
		/// </summary>
		~DatabaseHelper()
		{
			Close();
		}
		#endregion

		#region 数据库打开与关闭
		/// <summary> 
		/// 打开数据库连接
		/// </summary> 
		public virtual void Open()
		{
			if (!Opened)
			{
				m_connection.Open();
			}			
		}

		/// <summary> 
		/// 关闭数据库连接
		/// </summary> 
		public virtual void Close()
		{
			m_transaction = null;
			m_closeOnTrans = false;
			m_autoOpened = false;

			try
			{
				m_connection.Close();
			}
			catch
			{
			}			
		}

		/// <summary>
		/// 关闭数据库并销毁对象
		/// </summary>
		public virtual void Dispose()
		{
			Close();
			GC.SuppressFinalize(this);
		}
		#endregion

		#region 数据库事务
		/// <summary>
		/// 开始事务
		/// </summary>
		public virtual void BeginTransaction()
		{
			if (m_transaction != null)
			{
				throw new Exception("Only one transaction may activate at a time.");
			}

			// 如果此时connection未打开，则自动打开它，但需要记住在Commit或Rollback后关闭
			m_closeOnTrans = !Opened;
			if (m_closeOnTrans)
			{
				Open();
			}			

			m_transaction = m_connection.BeginTransaction();
		}

		/// <summary>
		/// Commit当前事务
		/// </summary>
		public virtual void CommitTransaction()
		{
			m_transaction.Commit();
			m_transaction = null;

			// 如果是在BeginTransaction时自动打开的则需要自动关闭
			if (m_closeOnTrans)
			{
				Close();
			}
		}

		/// <summary>
		/// Rollback当前事务
		/// </summary>
		public virtual void RollbackTransaction()
		{
			if (Opened)
			{
				m_transaction.Rollback();
			}
			
			m_transaction = null;

			// 如果是在BeginTransaction时自动打开的则需要自动关闭
			if (m_closeOnTrans)
			{
				Close();
			}
		}
		#endregion

		#region 数据库命令执行
		/// <summary> 
		/// 执行SQL语句，返回影响的记录数 
		/// </summary> 
		/// <param name="sql">SQL语句</param> 
		/// <param name="parameters">查询或执行参数</param> 
		/// <param name="identity">是否需要返回本次执行sql插入的最后一个自增列ID</param> 
		/// <returns>如果identity为true则返回本次执行sql插入的最后一个自增列ID，否则返回影响的记录数</returns> 
		public virtual int Execute(string sql, ProcedureParameter[] parameters = null, bool identity = false)
		{
			string identitySql = identity ? GetRecentIdentitySql() : null;
			if (identitySql != null)
			{
				sql += ";" + identitySql;
				return QueryInt(sql, parameters);
			}

			DbCommand command = PrepareCommand(sql, parameters);

			try
			{
				return command.ExecuteNonQuery();
			}
			catch
			{
				throw;
			}
			finally
			{
				CloseIfAutoOpened();
			}			
		}

		/// <summary> 
		/// 执行多条SQL语句，实现数据库事务。 
		/// </summary> 
		/// <param name="sqlList">事务语句集</param> 
		public virtual void ExecuteTransaction(IEnumerable<string> sqlList)
		{
			BeginTransaction();
			try
			{
				foreach (string sql in sqlList)
				{
					Execute(sql);
				}

				CommitTransaction();
			}
			catch
			{
				RollbackTransaction();
				throw;
			}
		}

		/// <summary> 
		/// 执行多条SQL语句，实现数据库事务。 
		/// </summary> 
		/// <param name="dataList">事务数据集</param> 
		public virtual void ExecuteTransaction(IEnumerable<TransactionData> dataList)
		{
			BeginTransaction();
			try
			{
				foreach (TransactionData data in dataList)
				{
					Execute(data.Sql, data.Parameters);								
				}

				CommitTransaction();		
			}
			catch
			{
				RollbackTransaction();
				throw;
			}			
		}
		#endregion

		#region 多数据集查询
		/// <summary> 
		/// 执行查询语句，返回DataSet对象
		/// </summary> 
		/// <param name="sqlList">多个查询语句集合</param>
		/// <param name="parameters">查询或执行参数</param> 
		/// <returns>DataSet对象</returns> 
		public virtual DataSet QueryDataSet(IEnumerable<string> sqlList, ProcedureParameter[] parameters = null)
		{
			string sql = string.Join(";", sqlList);
			return QueryDataSet(sql, parameters);
		}

		/// <summary> 
		/// 执行查询语句，返回DataSet对象
		/// </summary> 
		/// <param name="sql">查询语句</param>
		/// <param name="parameters">查询或执行参数</param> 
		/// <returns>DataSet对象</returns> 
		public virtual DataSet QueryDataSet(string sql, ProcedureParameter[] parameters = null)
		{
			DbCommand command = PrepareCommand(sql, parameters);

			try
			{
				DbDataAdapter adapter = NewDataAdapter();
				adapter.SelectCommand = command;
				DataSet ds = new DataSet();
				adapter.Fill(ds);
				return ds;
			}
			catch
			{
				throw;
			}
			finally
			{
				CloseIfAutoOpened();
			}
		}
		#endregion

		#region 多行查询
		/// <summary> 
		/// 执行查询语句，返回DataTable对象
		/// </summary> 
		/// <param name="sql">查询语句</param>
		/// <param name="parameters">查询或执行参数</param> 
		/// <returns>DataTable对象</returns>
		public virtual DataTable QueryDataTable(string sql, ProcedureParameter[] parameters = null)
		{
			DataSet ds = QueryDataSet(sql, parameters);
			if (ds.Tables.Count > 0)
			{
				return ds.Tables[0];
			}

			return null;			
		}		
		#endregion

		#region 单行查询
		/// <summary>
		/// 执行查询语句，返回一行数据库记录
		/// </summary>
		/// <param name="sql">查询语句</param>
		/// <param name="parameters">查询或执行参数</param>
		/// <returns>数据存在则返回DataRow对象，否则返回null</returns>
		public virtual DataRow QueryDataRow(string sql, ProcedureParameter[] parameters = null)
		{
			DataTable dt = QueryDataTable(sql, parameters);
			DataRow dr = null;
			if (dt.Rows.Count > 0)
			{
				dr = dt.Rows[0];
			}
			return dr;
		}		
		#endregion

		#region 单值查询
		/// <summary>
		/// 查询符合条件的记录是否存在
		/// </summary>
		/// <param name="sql">查询语句</param>
		/// <param name="parameters">查询或执行参数</param>
		/// <returns>如果记录存在返回true，否则返回false</returns>
		public virtual bool QueryExists(string sql, ProcedureParameter[] parameters = null)
		{
			return QueryObject(sql, parameters) != null;
		}

		/// <summary> 
		/// 执行查询语句，返回object对象
		/// </summary> 
		/// <param name="sql">查询语句</param>
		/// <param name="parameters">查询或执行参数</param> 
		/// <returns>数据存在则返回object对象，否则返回null</returns> 
		public virtual object QueryObject(string sql, ProcedureParameter[] parameters = null)
		{
			DbCommand command = PrepareCommand(sql, parameters);

			try
			{
				object obj = command.ExecuteScalar();
				if ((object.Equals(obj, null)) || (object.Equals(obj, DBNull.Value)))
				{
					obj = null;
				}

				return obj;
			}
			catch (Exception e)
			{
				throw e;
			}
			finally
			{
				CloseIfAutoOpened();
			}			
		}		

		/// <summary> 
		/// 执行查询语句，返回int值
		/// </summary> 
		/// <param name="sql">查询语句</param>
		/// <param name="parameters">查询或执行参数</param> 
		/// <returns>数据存在则返回结果值，否则返回0</returns> 
		public int QueryInt(string sql, ProcedureParameter[] parameters = null)
		{
			object obj = QueryObject(sql, parameters);
			if (obj == null)
			{
				return 0;
			}

			try
			{
				return Convert.ToInt32(obj);
			}
			catch
			{
				return 0;
			}
		}

		/// <summary> 
		/// 执行查询语句，返回double值
		/// </summary> 
		/// <param name="sql">查询语句</param>
		/// <param name="parameters">查询或执行参数</param>		/// 
		/// <returns>数据存在则返回结果值，否则返回0</returns> 
		public double QueryDouble(string sql, ProcedureParameter[] parameters = null)
		{
			object obj = QueryObject(sql, parameters);
			if (obj == null)
			{
				return 0;
			}

			try
			{
				return Convert.ToDouble(obj);
			}
			catch
			{
				return 0;
			}
		}

		/// <summary> 
		/// 执行查询语句，返回decimal值
		/// </summary> 
		/// <param name="sql">查询语句</param>
		/// <param name="parameters">查询或执行参数</param> 
		/// <returns>数据存在则返回结果值，否则返回0</returns> 
		public decimal QueryDecimal(string sql, ProcedureParameter[] parameters = null)
		{
			object obj = QueryObject(sql, parameters);
			if (obj == null)
			{
				return 0;
			}

			try
			{
				return Convert.ToDecimal(obj);
			}
			catch
			{
				return 0;
			}
		}

		/// <summary> 
		/// 执行查询语句，返回字符串
		/// </summary> 
		/// <param name="sql">查询语句</param>
		/// <param name="parameters">查询或执行参数</param> 
		/// <returns>数据存在则返回结果值，否则返回null</returns>
		public string QueryString(string sql, ProcedureParameter[] parameters = null)
		{
			object obj = QueryObject(sql, parameters);
			if (obj == null)
			{
				return null;
			}

			try
			{
				return Convert.ToString(obj);
			}
			catch
			{
				return null;
			}
		}

		/// <summary> 
		/// 执行查询语句，返回DateTime对象
		/// </summary> 
		/// <param name="sql">查询语句</param>
		/// <param name="parameters">查询或执行参数</param> 
		/// <returns>数据存在则返回结果值，否则返回DateTime.MinValue</returns>
		public DateTime QueryDateTime(string sql, ProcedureParameter[] parameters = null)
		{
			object obj = QueryObject(sql, parameters);
			if (obj == null)
			{
				return DateTime.MinValue;
			}

			try
			{
				return Convert.ToDateTime(obj);
			}
			catch
			{
				return DateTime.MinValue;
			}
		}

		/// <summary> 
		/// 执行查询语句，返回格式化的日期字符串，默认格式"yyyy-MM-dd"
		/// </summary> 
		/// <param name="sql">查询语句</param>		
		/// <param name="parameters">查询或执行参数</param> 
		/// <param name="format">日期格式</param>
		/// <returns>格式化的日期字符串</returns>
		public string QueryDateString(string sql, ProcedureParameter[] parameters = null, string format = "yyyy-MM-dd")
		{
			return QueryDateTimeString(sql, parameters, format);
		}

		/// <summary> 
		/// 执行查询语句，返回格式化的日期时间字符串，默认格式"yyyy-MM-dd HH:mm:ss"
		/// </summary> 
		/// <param name="sql">查询语句</param>		
		/// <param name="parameters">查询或执行参数</param> 
		/// <param name="format">日期时间格式</param>
		/// <returns>格式化的日期时间字符串</returns>
		public string QueryDateTimeString(string sql, ProcedureParameter[] parameters = null, string format = "yyyy-MM-dd HH:mm:ss")
		{
			DateTime dt = QueryDateTime(sql, parameters);
			if (dt > DateTime.MinValue)
			{
				return dt.ToString(format);
			}
			return null;
		}

		#endregion

		#region 静态方法
		// 一条sql语句如果是正常query一定带空格，而如果是存储过程名则一定不带空格
		private static CommandType GetCommandType(string sql)
		{
			if (string.IsNullOrEmpty(sql))
			{
				return CommandType.Text;
			}

			if (sql.Trim().IndexOf(' ') == -1)
			{
				return CommandType.StoredProcedure;
			}

			return CommandType.Text;
		}

		#endregion

		#region 抽象成员
		/// <summary>
		/// 要求继承类返回一个DbConnection对象
		/// </summary>
		/// <returns>DbConnection对象</returns>
		protected abstract DbConnection NewConnection();

		/// <summary>
		/// 要求继承类返回一个DbCommand对象
		/// </summary>
		/// <returns>DbCommand对象</returns>
		protected abstract DbCommand NewCommand();

		/// <summary>
		/// 要求继承类返回一个DbParameter对象
		/// </summary>
		/// <returns>DbParameter对象</returns>
		protected abstract DbParameter NewParameter();

		/// <summary>
		/// 要求继承类返回一个DbDataAdapter对象
		/// </summary>
		/// <returns>DbDataAdapter对象</returns>
		protected abstract DbDataAdapter NewDataAdapter();
		#endregion

		#region 辅助方法和属性
		private void AddParameters(DbCommand command, ProcedureParameter[] parameters)
		{
			if (parameters == null)
			{
				return;
			}
			
			foreach (ProcedureParameter param in parameters)
			{
				// 生成DbParameter对象并加入命令参数表
				DbParameter dbp = NewParameter();
				dbp.ParameterName = param.Name;
				dbp.Direction = ParameterDirection.Input;				

				if (param.Value == null)
				{
					dbp.Value = DBNull.Value;
				}
				else
				{
					dbp.Value = param.Value;
				}

				command.Parameters.Add(dbp);
			}
		}

		/// <summary> 
		/// 创建并初始化数据库命令对象
		/// </summary> 
		/// <param name="sql">SQL语句</param>	
		/// <param name="parameters">查询或执行参数</param> 
		/// <returns>DbCommand命令对象</returns>
		protected virtual DbCommand PrepareCommand(string sql, ProcedureParameter[] parameters)
		{
			TrackAutoOpened(); // 首先保存auto-opened状态
			Open();

			DbCommand command = NewCommand();
			command.CommandText = sql;
			command.Connection = m_connection;
			command.CommandType = GetCommandType(sql);

			if (m_transaction != null)
			{
				command.Transaction = m_transaction;
			}

			// 带参数调用，可能是存储过程，也可能是带参sql
			AddParameters(command, parameters);
			return command;
		}

		/// <summary>
		/// 获取本次连接域所创建的最后一个自增列ID的SQL语句
		/// </summary>
		/// <returns>如果存在则返回SQL语句，否则返回null</returns>
		protected virtual string GetRecentIdentitySql() { return null; }
		#endregion

		#region 私有成员
		// 用于追踪数据库连接是否由当前call打开的，如是，则当前call返回前必须关闭连接
		private bool m_autoOpened = false;
		private bool m_closeOnTrans = false;
		private DbConnection m_connection = null;
		DbTransaction m_transaction = null;

		private void TrackAutoOpened()
		{
			m_autoOpened = !Opened;
		}

		private void CloseIfAutoOpened()
		{
			if (m_autoOpened)
			{
				Close();
			}
		}
		#endregion
	}

	#region ProcedureParameter	
	/// <summary>
	/// 数据库查询或执行，DbParameter类的重载太过繁琐，所以另外定义这个简单的类
	/// </summary>
	public class ProcedureParameter
	{
		/// <summary>
		/// 参数名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 参数值
		/// </summary>
		public object Value { get; set; }

		/// <summary> 
		/// 构造函数
		/// </summary> 
		/// <param name="name">参数名称</param>
		/// <param name="value">参数的值</param>
		public ProcedureParameter(string name, object value)
		{
			Name = name;
			Value = value;
		}
	}
	#endregion

	#region TransactionData
	/// <summary>
	/// 数据库事务列表中的一个具体事务的数据，包括SQL语句和参数表
	/// </summary>
	public class TransactionData
	{
		/// <summary>
		/// SQL语句
		/// </summary>
		public string Sql { get; set; }

		/// <summary>
		/// SQL参数列表
		/// </summary>
		public ProcedureParameter[] Parameters { get; set; }

		/// <summary>
		/// 默认构造函数
		/// </summary>
		public TransactionData()
		{
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="sql">SQL语句</param>
		/// <param name="parameters">SQL参数列表</param>
		public TransactionData(string sql, ProcedureParameter[] parameters)
		{
			Sql = sql;
			Parameters = parameters;
		}
	}
	#endregion
}