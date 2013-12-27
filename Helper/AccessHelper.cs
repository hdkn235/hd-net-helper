using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Configuration;
using System.Data;

namespace HD.Helper
{
    public static class AccessHelper
    {
        static readonly string connStr = ConfigurationManager.ConnectionStrings["sql"].ConnectionString;

        public static void ExecuteNonQuery(string sql, params OleDbParameter[] para)
        {
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    if (para != null)
                    {
                        cmd.Parameters.AddRange(para);
                    }
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static object ExecuteScalar(string sql, params OleDbParameter[] para)
        {
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    if (para != null)
                    {
                        cmd.Parameters.AddRange(para);
                    }
                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        public static OleDbDataReader ExecuteReader(string sql, params OleDbParameter[] para)
        {
            OleDbConnection conn = new OleDbConnection(connStr);
            try
            {
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    if (para != null)
                    {
                        cmd.Parameters.AddRange(para);
                    }
                    conn.Open();
                    return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                }
            }
            catch (Exception ex)
            {
                conn.Dispose();
                throw ex;
            }
        }

        public static DataSet GetDataSet(string sql, params OleDbParameter[] para)
        {
            DataSet ds = new DataSet();
            using (OleDbDataAdapter odda = new OleDbDataAdapter(sql, connStr))
            {
                if (para != null)
                {
                    odda.SelectCommand.Parameters.AddRange(para);
                }
                odda.Fill(ds);
            }
            return ds;
        }

    }

    #region 使用示例
    ////string sql = "select * from users";
    ////object o =AccessHelper.ExecuteScalar(sql);
    //string sql = @"insert into Users([UID],[Pwd],[Domain],[SendName],[Email],[Service],[ServiceFullName]) values('@UID','@Pwd','@Domain','@SendName','@Email','@Service','@ServiceFullName')";
    ////DataSet ds = AccessHelper.GetDataSet(sql);
    ////Console.WriteLine(o);
    //AccessHelper.ExecuteNonQuery(sql);
    //Console.ReadKey(); 
    #endregion

    #region 连接字符串
    
    //<?xml version="1.0" encoding="utf-8" ?>
    //<configuration>
    //    <connectionStrings>
    //        <add name="sql" connectionString="Provider=Microsoft.ACE.OLEDB.12.0;Data Source=db.accdb;Jet OleDb:DataBase Password=spring"/>
    //    </connectionStrings>
    //</configuration>

    #endregion
}
