using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogHelper
{
    /// <summary>
    /// 日志类型定义
    /// </summary>
    public enum LOGTYPE :int
    {

        /// <summary>
        /// 错误
        /// </summary>
        ERROR = 0,

        /// <summary>
        /// 警告
        /// </summary>
        WARNING = 1,

        /// <summary>
        /// 调度
        /// </summary>
        DISPATCH = 2,

        /// <summary>
        /// 完成
        /// </summary>
        FINISH = 3,

        /// <summary>
        /// 运行日志
        /// </summary>
        RUNNING = 4,

        /// <summary>
        /// 其他
        /// </summary>
        OTHER = 5 
    }
    
    /// <summary>
    /// 
    /// </summary>
    internal class LOGTABLE
    {
        /// <summary>
        /// 根据日志类型获取日志表名
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <returns></returns>
        internal static String GetTableName(int type)
        {
            String name;
            switch (type)
            {
                case (int)LOGTYPE.ERROR:
                    name = "error";
                    break;
                case (int)LOGTYPE.WARNING:
                    name = "warning";
                    break;
                case (int)LOGTYPE.DISPATCH:
                    name = "dispatch";
                    break;
                case (int)LOGTYPE.FINISH:
                    name = "finish";
                    break;
                case (int)LOGTYPE.RUNNING:
                    name = "running";
                    break;
                default:
                    name = "other";
                    break;
            }
            return name;
        }

        /// <summary>
        /// 根据日志类型获取表的创建语句
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static String GetTableCreateSQL(int type)
        {
            String name = GetTableName(type);
            String sql = "create table "+name+"("+
                "id INTEGER PRIMARY KEY AUTOINCREMENT,"+
                "logid  VARCHAR(255)," +
                "logobject VARCHAR(255)," +
                "logsubject VARCHAR(255),"+
                "logcontent VARCHAR(255)," +
                "logtime TIME,"+
                "logkey01 TEXT,"+
                "logkey02 TEXT," +
                "logkey03 TEXT," +
                "logkey04 TEXT," +
                "logkey05 TEXT" +
                ");";

            return sql;
        }

        /// <summary>
        /// 根据日志类型获取插入数据语句
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static String GetTableInsertSQL(LogData data)
        {
            String name = GetTableName((int)data.LogType);
            String sql = "insert into " + name + "(" +
                "logid," +
                "logobject," +
                "logsubject," +
                "logcontent," +
                "logtime," +
                "logkey01," +
                "logkey02," +
                "logkey03," +
                "logkey04," +
                "logkey05" +
                ") values ("+
                ToDbValue(data.LogID)+","+
                ToDbValue(data.LogObject)+","+
                ToDbValue(data.LogSubject) + "," +
                ToDbValue(data.LogContent) + "," +
                ToDbValue(data.LogTime) + "," +
                ToDbValue(data.LogKey01) + "," +
                ToDbValue(data.LogKey02) + "," +
                ToDbValue(data.LogKey03) + "," +
                ToDbValue(data.LogKey04) + "," +
                ToDbValue(data.LogKey05)+")";

            return sql;
        }

        /// <summary>
        /// 对插入到数据库中的空值进行处理
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static object ToDbValue(object value)
        {
            if (value == null)
            {
                return "''";// DBNull.Value;
            }
            else
            {
                if (value is String) {
                    return "'" + value + "'";
                }else if(value is DateTime)
                {
                    return "'"+((DateTime)value).ToString("yyyy/MM/dd HH:mm:ss")+"'";
                }
                else
                {
                    return value;
                }
                    
                
            }
        }

        /// <summary>
        /// 对从数据库中读取的空值进行处理
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static object FromDbValue(object value)
        {
            if (value == DBNull.Value)
            {
                return null;
            }
            else
            {
                return value;
            }
        }
    }
}
