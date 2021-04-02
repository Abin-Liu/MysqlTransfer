using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace MFGLib
{
	/// <summary>
	/// Safely convert data from one type to another without raising any exception
	/// </summary>
    public class SafeConvert
    {
		/// <summary>
		/// Convert object to string
		/// </summary>
		/// <param name="obj">The object to be converted</param>
		/// <param name="defultVal">The default value to be returned if convertion fails</param>
		/// <returns>The convertion result</returns>
		public static string ToString(object obj, string defultVal = "")
        {
            if (obj == null || obj == DBNull.Value)
            {
                return defultVal;
            }

            return obj.ToString();
        }

		/// <summary>
		/// Convert object to int
		/// </summary>
		/// <param name="obj">The object to be converted</param>
		/// <param name="defultVal">The default value to be returned if convertion fails</param>
		/// <returns>The convertion result</returns>
		public static int ToInt(object obj, int defultVal = 0)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return defultVal;
            }

            try
            {
               return Convert.ToInt32(obj);
            }
            catch
            {
                return defultVal;
            }
        }

		/// <summary>
		/// Convert object to DateTime
		/// </summary>
		/// <param name="obj">The object to be converted</param>
		/// <returns>Return a DateTime value if the convertion was success, or DateTime.MinValue otherwise</returns>
		public static DateTime ToDateTime(object obj)
        {
            if (obj == null || obj == DBNull.Value)
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
		/// Convert object to date string
		/// </summary>
		/// <param name="obj">The object to be converted</param>
		/// <param name="format">Format of the date string</param>
		/// <returns>Return a date string if success, null otherwise</returns>
		public static string ToDateString(object obj, string format = "yyyy-MM-dd")
		{			
			return ToDateTimeString(obj, format);
		}

		/// <summary>
		/// Convert object to date-time string
		/// </summary>
		/// <param name="obj">The object to be converted</param>
		/// <param name="format">Format of the date-time string</param>
		/// <returns>Return a date-time string if success, null otherwise</returns>
		public static string ToDateTimeString(object obj, string format = "yyyy-MM-dd HH:mm:ss")
		{
			DateTime dt = ToDateTime(obj);
			if (dt > DateTime.MinValue)
			{
				return dt.ToString(format);
			}
			return null;
		}

		/// <summary>
		/// Convert object to double
		/// </summary>
		/// <param name="obj">The object to be converted</param>
		/// <param name="defultVal">The default value to be returned if convertion fails</param>
		/// <returns>The convertion result</returns>
		public static double ToDouble(object obj, double defultVal = 0.0)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return defultVal;
            }

            double dValue = defultVal;
            try
            {
                return Convert.ToDouble(obj);
            }
            catch
            {
                return defultVal;
            }
        }

		/// <summary>
		/// Convert object to decimal
		/// </summary>
		/// <param name="obj">The object to be converted</param>
		/// <param name="defultVal">The default value to be returned if convertion fails</param>
		/// <returns>The convertion result</returns>
		public static decimal ToDecimal(object obj, decimal defultVal = 0)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return defultVal;
            }

            try
            {
               return Convert.ToDecimal(obj);
            }
            catch
            {
                return defultVal;
            }
		}		

		/// <summary>
		/// 将一个DataRow对象赋值给某个实例
		/// </summary>
		/// <typeparam name="T">实例类型模板</typeparam>
		/// <param name="dr">DataRow</param>		
		/// <returns>转化成的对象</returns>
		public static T ToEntity<T>(DataRow dr) where T : new()
		{
			if (dr == null)
			{
				throw new ArgumentNullException("dr");
			}			

			T entity = new T();
			foreach (PropertyInfo prop in entity.GetType().GetProperties())
			{
				if (!prop.CanWrite || !dr.Table.Columns.Contains(prop.Name))
				{
					continue;
				}

				object value = dr[prop.Name];
				if (value == null || value == DBNull.Value)
				{
					continue;
				}
				
				switch (Type.GetTypeCode(prop.PropertyType))
				{
					case TypeCode.Char:						
						try
						{
							prop.SetValue(entity, Convert.ToChar(value), null);
						}
						catch
						{
						}

						break;

					case TypeCode.Boolean:
						try
						{
							prop.SetValue(entity, Convert.ToBoolean(value), null);
						}
						catch
						{
						}

						break;

					case TypeCode.Byte:
						try
						{
							prop.SetValue(entity, Convert.ToByte(value), null);
						}
						catch
						{
						}

						break;

					case TypeCode.SByte:
						try
						{
							prop.SetValue(entity, Convert.ToSByte(value), null);
						}
						catch
						{
						}
						break;

					case TypeCode.Int16:
						try
						{
							prop.SetValue(entity, Convert.ToInt16(value), null);
						}
						catch
						{
						}
						break;

					case TypeCode.UInt16:
						try
						{
							prop.SetValue(entity, Convert.ToUInt16(value), null);
						}
						catch
						{
						}
						break;

					case TypeCode.Int32:
						try
						{
							prop.SetValue(entity, Convert.ToInt32(value), null);
						}
						catch
						{
						}
						break;

					case TypeCode.UInt32:
						try
						{
							prop.SetValue(entity, Convert.ToUInt32(value), null);
						}
						catch
						{
						}
						break;

					case TypeCode.Int64:
						try
						{
							prop.SetValue(entity, Convert.ToInt64(value), null);
						}
						catch
						{
						}
						break;

					case TypeCode.UInt64:
						try
						{
							prop.SetValue(entity, Convert.ToUInt64(value), null);
						}
						catch
						{
						}
						break;

					case TypeCode.Single:
						try
						{
							prop.SetValue(entity, Convert.ToSingle(value), null);
						}
						catch
						{
						}
						break;

					case TypeCode.Double:
						try
						{
							prop.SetValue(entity, Convert.ToDouble(value), null);
						}
						catch
						{
						}
						break;

					case TypeCode.Decimal:
						try
						{
							prop.SetValue(entity, Convert.ToDecimal(value), null);
						}
						catch
						{
						}
						break;

					case TypeCode.String:
						try
						{
							prop.SetValue(entity, value.ToString(), null);
						}
						catch
						{
						}
						break;

					case TypeCode.DateTime:
						DateTime date = DateTime.MinValue;
						if (DateTime.TryParse(value.ToString(), out date))
						{
							prop.SetValue(entity, date, null);
						}

						break;

					default:
						break;
				}
			}

			return entity;
		}

		/// <summary>
		/// 将一个DataTable对象转化成实例列表
		/// </summary>
		/// <typeparam name="T">实例类型模板</typeparam>
		/// <param name="dt">DataTable对象</param>
		/// <returns>转化的实例列表</returns>
		public static List<T> ToEntities<T>(DataTable dt) where T : new()
		{
			if (dt == null)
			{
				throw new ArgumentNullException("dt");
			}

			List<T> dataList = new List<T>();			
			foreach (DataRow dr in dt.Rows)
			{
				dataList.Add(ToEntity<T>(dr));
			}

			return dataList;
		}
	}
}