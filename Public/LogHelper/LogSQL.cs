using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogHelper
{
    internal class LogSQL
    {
        /// <summary>
        /// 数据库文件明
        /// </summary>
        protected static string m_dbName = "log.sqlite";

        /// <summary>
        /// 数据库打开密码：默认为空
        /// </summary>
        protected static string m_password = "";//"1234";

        /// <summary>
        /// 数据库存放路径
        /// </summary>
        protected static string m_dbfile_folder = "";//@"/data/";// @"\data\";

        /// <summary>
        /// 数据库版本
        /// </summary>
        private static int SQL_VERSION = 3;


        /// <summary>
        /// 设置数据库登陆密码
        /// </summary>
        /// <param name="pasword">数据库文件密码</param>
        internal static void SetDbPaswd(String pasword)
        {
            m_password = pasword;
        }

        /// <summary>
        /// 设置数据库文件名字：默认 LogData名称  sqlite格式
        /// </summary>
        /// <param name="fileName"></param>
        internal static void SetDbFileName(String fileName)
        {
            if (fileName != null && fileName.Length != 0 && !fileName.Equals(""))
            {
                m_dbName = fileName + ".sqlite";
            }
        }

        /// <summary>
        /// 返回连接数据库路径
        /// </summary>
        /// <returns>数据库连接字符串</returns>
        internal static string GetConnectString()
        {
            SQLiteConnectionStringBuilder connstr = new SQLiteConnectionStringBuilder();
            connstr.DataSource = m_dbfile_folder + m_dbName;
            connstr.Version = SQL_VERSION;
            if (m_password != "")
                connstr.Password = m_password;
            return connstr.ToString();
        }

        /// <summary>
        /// 首次创建数据库，打开数据库
        /// </summary>
        internal static void InitSQLiteDB()
        {
            string path = System.Environment.CurrentDirectory + m_dbfile_folder;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string dbFileName = path + "\\" + m_dbName;
            if (!File.Exists(dbFileName))
            {
                SQLiteConnection.CreateFile(dbFileName);
                InitCreateTables();
            }
        }
        /// <summary>
        /// 在数据库初次创建的时候创建表
        /// </summary>
        internal static void InitCreateTables()
        {
            using (SQLiteConnection con = new SQLiteConnection())
            {
                con.ConnectionString = GetConnectString();

                con.Open();
                using (SQLiteCommand cmd = con.CreateCommand())
                {
                    foreach (int type in Enum.GetValues(typeof(LOGTYPE)))
                    {
                        cmd.CommandText = LOGTABLE.GetTableCreateSQL(type);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        /// <summary>
        /// 执行非查询的数据库操作
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="sqlString">要执行的sql语句</param>
        /// <returns>返回受影响的条数</returns>

        internal static int ExecuteNonQuery(SQLiteCommand cmd, string sqlString)
        {
            cmd.CommandText = sqlString;
            cmd.Parameters.Clear();
           
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 执行非查询的数据库操作
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="sqlString">要执行的sql语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>返回受影响的条数</returns>

        internal static int ExecuteNonQueryParam(SQLiteCommand cmd, string sqlString,params SQLiteParameter[] parameters)
        {
            cmd.CommandText = sqlString;
            cmd.Parameters.Clear();
            foreach (SQLiteParameter parameter in parameters)
            {
                cmd.Parameters.Add(parameter);
            }
            return cmd.ExecuteNonQuery();
        }
    }
}
