using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace HD.Common
{
    public static class SQLHelper
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        static readonly string connStr = ConfigurationManager.ConnectionStrings["sql"].ConnectionString;

        /// <summary>
        /// 执行增删改方法
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="para">sql参数</param>
        /// <returns>更新的行数</returns>
        public static int ExcuteNonQuery(string sql, CommandType ct, params SqlParameter[] para)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (para != null)
                    {
                        cmd.Parameters.AddRange(para);
                    }
                    cmd.CommandType = ct;
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 执行不是存储过程的sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static int ExcuteNonQuery(string sql, params SqlParameter[] para)
        {
            return ExcuteNonQuery(sql, CommandType.Text, para);
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static int ExcuteNonQueryByProc(string sql, params SqlParameter[] para)
        {
            return ExcuteNonQuery(sql, CommandType.StoredProcedure, para);
        }

        /// <summary>
        /// 执行查询语句，返回首行首列方法
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="para">sql参数数组</param>
        /// <returns>第一行第一列的值</returns>
        public static object ExcuteScalar(string sql, CommandType ct, params SqlParameter[] para)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (para != null)
                    {
                        cmd.Parameters.AddRange(para);
                    }
                    cmd.CommandType = ct;
                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// 执行不是存储过程的查询语句，返回首行首列方法
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static object ExcuteScalar(string sql, params SqlParameter[] para)
        {
            return ExcuteScalar(sql, CommandType.Text, para);
        }

        /// <summary>
        /// 执行是存储过程的查询语句，返回首行首列方法
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static object ExcuteScalarByProc(string sql, params SqlParameter[] para)
        {
            return ExcuteScalar(sql, CommandType.StoredProcedure, para);
        }

        /// <summary>
        /// 执行查询语句，返回SqlDataReader方法,但连接未关闭，需要使用SqlDataReader关闭
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="para">sql参数数组</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string sql, CommandType ct, params SqlParameter[] para)
        {
            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (para != null)
                    {
                        cmd.Parameters.AddRange(para);
                    }
                    cmd.CommandType = ct;
                    conn.Open();
                    return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);//当返回的SqlDataReader断开连接时，则SqlDataReader关联的SqlDataReader也会断开连接
                }
            }
            catch (Exception ex)
            {
                conn.Dispose();
                throw ex;
            }
        }

        /// <summary>
        /// 执行非存储过程查询语句，返回SqlDataReader方法,但连接未关闭，需要使用SqlDataReader关闭
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="para">sql参数数组</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string sql, params SqlParameter[] para)
        {
            return ExecuteReader(sql, CommandType.Text, para);
        }

        /// <summary>
        /// 执行存储过程查询语句，返回SqlDataReader方法,但连接未关闭，需要使用SqlDataReader关闭
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="para">sql参数数组</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReaderByProc(string sql, params SqlParameter[] para)
        {
            return ExecuteReader(sql, CommandType.StoredProcedure, para);
        }

        /// <summary>
        /// 获得DataTable
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="para">参数SqlParameter列表</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataTable(string sql, CommandType ct, params SqlParameter[] para)
        {
            DataTable dt = new DataTable();
            using (SqlDataAdapter sda = new SqlDataAdapter(sql, connStr))
            {
                if (para != null)
                {
                    sda.SelectCommand.Parameters.AddRange(para);
                }
                sda.SelectCommand.CommandType = ct;
                sda.Fill(dt);
            }
            return dt;
        }

        /// <summary>
        /// 获得dataset
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="para">参数SqlParameter列表</param>
        /// <returns>DataSet</returns>
        public static DataTable GetDataTable(string sql, params SqlParameter[] para)
        {
            return GetDataTable(sql, CommandType.Text, para);
        }

        /// <summary>
        /// 获得dataset通过存储过程
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="para">参数SqlParameter列表</param>
        /// <returns>DataSet</returns>
        public static DataTable GetDataTableByProc(string sql, params SqlParameter[] para)
        {
            return GetDataTable(sql, CommandType.StoredProcedure, para);
        }

        #region 返回组合的查询条件
        /// <summary>
        /// 返回组合的查询条件
        /// </summary>
        /// <param name="i"> 1 and = ,2 or = ,3  and like ,4  or like, 5 时间段</param>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string BackSearchReson(int i, string columnName, string value)
        {
            string reson = "";
            if (columnName != string.Empty && value != string.Empty)
            {
                switch (i)
                {
                    case 1:
                        reson += " and " + columnName + "='" + value + "'";
                        break;
                    case 2:
                        reson += " or " + columnName + "='" + value + "'";
                        break;
                    case 3:
                        reson += " and " + columnName + " like'%" + value + "%'";
                        break;
                    case 4:
                        reson += " or " + columnName + " like'%" + value + "%'";
                        break;
                    case 5:
                        reson += " and (" + columnName + ">'" + Convert.ToDateTime(value).ToString() + "' and " + columnName + "<'" + Convert.ToDateTime(value).AddDays(1).ToString() + "')";
                        break;
                }
            }
            return reson;
        }
        #endregion

        #region 过滤字符串,过滤掉从输入框输入的一些非法字符，防注入！
        public static string InputText(string text, int maxLength)
        {
            text = text.Trim();
            if (text.Length == 0)
                return string.Empty;
            if (text.Length > maxLength)
                text = text.Substring(0, maxLength);
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n");//<br/>
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty);//any other tags
            text = text.Replace("'", "");
            text = text.Replace(" ", "");
            text = text.Replace("%", "");
            text = text.Replace("!", "");
            text = text.Replace("~", "");
            text = text.Replace("`", "");
            text = text.Replace("#", "");
            text = text.Replace("$", "");
            text = text.Replace("^", "");
            text = text.Replace("&", "");
            text = text.Replace("*", "");
            text = text.Replace("(", "");
            text = text.Replace(")", "");
            text = text.Replace("{", "");
            text = text.Replace("}", "");
            text = text.Replace("[", "");
            text = text.Replace("]", "");
            text = text.Replace(",", "");
            text = text.Replace("/", "");
            text = text.Replace("@", "");
            text = text.Replace("-", "");
            text = text.Replace("=", "");
            text = text.Replace("+", "");
            text = text.Replace("|", "");
            text = text.Replace(".", "");
            return text;
        }
        #endregion
    }

    #region 连接字符串
    
    //<?xml version="1.0" encoding="utf-8" ?>
    //<configuration>
    //    <connectionStrings>
    //        <add name="sql" connectionString="server=.\sql2008;database=mydb;uid=sa;pwd=spring;"/>
    //    </connectionStrings>
    //</configuration>

    #endregion

    #region ExecuteReader示例

    //using (SqlDataReader sdr = SQLHelper.ExecuteReader("select * from hd.student where name like @name",new SqlParameter("@name","%张三%")))
    //{
    //    while (sdr.Read())
    //    {
    //        Console.WriteLine(sdr[3]);
    //    }  
    //}

    #endregion

    #region 调用存储过程示例

    /// <summary>
    /// 得到分页的数据
    /// </summary>
    /// <param name="pageIndex">第几页</param>
    /// <param name="pageSize">每页的记录数</param>
    /// <param name="pageCount">总共多少页</param>
    /// <returns></returns>
    //public List<Photos> GetPagedPhotos(int pageIndex, int pageSize, out int pageCount)
    //{
    //    string sql = "usp_GetPagedPhotos";//存储过程名称
    //    SqlParameter[] param = { 
    //                                new SqlParameter("@PageIndex",SqlDbType.Int),
    //                                new SqlParameter("@PageSize",SqlDbType.Int),
    //                                new SqlParameter("@PageSize",SqlDbType.Int)
    //                           };
    //    param[0].Value = pageIndex;
    //    param[1].Value = pageSize;
    //    param[2].Direction = ParameterDirection.Output;//指定输出参数

    //    DataTable dt = SQLHelper.GetDataTableByProc(sql, param);
    //    pageCount = Convert.ToInt32(param[2].Value);
    //    return DTToList(dt);
    //}

    #endregion
}
