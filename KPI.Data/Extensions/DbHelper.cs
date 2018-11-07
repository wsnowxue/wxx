using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace KPI.Data
{
    public class DbHelper
    {
        public static string connstring = ConfigurationManager.ConnectionStrings["KPIDbContext"].ConnectionString;
#region 构造函数
        /// <summary>
        /// 构造方法
        /// </summary>
        public DbHelper(DbConnection _conn)
        {
            conn = _conn;
            cmd = conn.CreateCommand();
        }
#endregion

#region 属性
        /// <summary>
        /// 数据库连接对象
        /// </summary>
        private DbConnection conn { get; set; }
        /// <summary>
        /// 执行命令对象
        /// </summary>
        private IDbCommand cmd { get; set; }
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void Close()
        {
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
            if (cmd != null)
            {
                cmd.Dispose();
            }
        }
#endregion
        public int ExecuteSqlCommand(string cmdText)
        {
            try
            {
                DbCommand cmd = new MySqlCommand();
                PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, null);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                Close();
                throw;
            }
        }
        public IDataReader ExecuteReader(string cmdText)
        {
            try
            {

                DbCommand cmd = new MySqlCommand();
                PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, null);
                return cmd.ExecuteReader();
            }
            catch
            {
                Close();
                throw;
            }

        }
        public object ExecuteScalar(string cmdText)
        {
            try
            {
                DbCommand cmd = new MySqlCommand();
                PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, null);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
            catch
            {
                Close();
                throw;
            }
        }
        private static void PrepareCommand(DbCommand cmd, DbConnection conn, DbTransaction isOpenTrans, CommandType cmdType, string cmdText, DbParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (isOpenTrans != null)
                cmd.Transaction = isOpenTrans;
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                cmd.Parameters.AddRange(cmdParms);
            }
        }
    }
}
