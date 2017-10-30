using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSPlatform.SQL
{
    public class SQLHelper
    {
        //连接Access字符串
        private static readonly string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        private static OleDbConnection Conn
        {
            set
            { Conn = value; }
            get
            {
                try
                {
                    OleDbConnection conn = new OleDbConnection(connStr);
                    conn.Open();
                    return conn;
                }
                catch(Exception)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 摘要: 针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。
        /// </summary>
        /// <returns>
        /// 返回结果: 受影响的行数。
        /// </returns>
        public static int ExecuteNonQuery(string sql, params OleDbParameter[] parms)
        {
            using (Conn)
            {
                using (OleDbCommand cmd = Conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    if (parms.Length != 0)
                    {
                        cmd.Parameters.AddRange(parms);
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// 摘要: 执行查询，并返回查询所返回的结果集中第一行的第一列。 忽略额外的列或行。
        /// </summary>
        /// <returns>
        /// 返回结果: 结果集中第一行的第一列。
        /// </returns>
        public static object ExecuteScalar(string sql, params OleDbParameter[] parms)
        {
            using (Conn)
            {
                using (OleDbCommand cmd = Conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    if (parms.Length != 0)
                    {
                        cmd.Parameters.AddRange(parms);
                    }
                    return cmd.ExecuteScalar();
                }
            }
        }
        public static OleDbDataReader ExecuteReader(string sql, params OleDbParameter[] parms)
        {
            try
            {
                using (OleDbCommand cmd = Conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    if (parms.Length != 0)
                    {
                        cmd.Parameters.AddRange(parms);
                    }
                    return cmd.ExecuteReader();
                }
            }
            catch (Exception)
            {
                Conn.Close();
                throw;
            }
        }
        public static DataTable ExecuteDataTable(string sql, params OleDbParameter[] parms)
        {
            DataTable table = new DataTable();
            DataSet dataset = new DataSet();
            using (Conn)
            {
                using (OleDbDataAdapter apter = new OleDbDataAdapter(sql, Conn))
                {
                    if (parms.Length != 0)
                    {
                        apter.SelectCommand.Parameters.AddRange(parms);
                    }
                    apter.Fill(dataset, "DataSet");
                    table = dataset.Tables["DataSet"];
                    return table;
                }
            }
        }
    }
}
