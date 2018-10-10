using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogHelper
{
    /// <summary>
    /// 日志操作类
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// 对象锁
        /// </summary>
        Object logA = new object();

        /// <summary>
        /// 对象锁
        /// </summary>
        Object logB = new object();

        /// <summary>
        /// 储存用户当前日志
        /// </summary>
        private List<LogData> _logList_temp;

        /// <summary>
        /// 将要保存到数据库的日志
        /// </summary>
        private List<LogData> _logList;

        /// <summary>
        /// 进行报错数据库时保存的错误日志
        /// </summary>
        private List<LogData> _errorList;

        /// <summary>
        /// 将缓存日志给到数据库保存线程
        /// </summary>
        private Thread _switchThread;

        /// <summary>
        /// 保存日志到数据库线程
        /// </summary>
        private Thread _saveToDBThread;


        /// <summary>
        /// 构造函数
        /// </summary>
        internal LogHelper(){ 

            _logList_temp = new List<LogData>();
            _logList = new List<LogData>();
            _errorList = new List<LogData>();

            _switchThread = new Thread(SwicthLog);
            _switchThread.IsBackground = true;
            _switchThread.Start();

            _saveToDBThread = new Thread(SaveLogToDB);
            _saveToDBThread.IsBackground = true;
            _saveToDBThread.Start();
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="logtype">日志类型</param>
        /// <param name="logid">日志ID</param>
        /// <param name="logobject">日志对象</param>
        /// <param name="logsubject">日志主题</param>
        /// <param name="logcontent">日志内容</param>
        /// <param name="key01">保留字段1</param>
        /// <param name="key02">保留字段2</param>
        /// <param name="key03">保留字段3</param>
        /// <param name="key04">保留字段4</param>
        /// <param name="key05">保留字段5</param>
        public void LogAdd(LOGTYPE logtype, String logid,String logobject,String logsubject,
            String logcontent,String key01 = "",
            String key02 = "", String key03 = "",
            String key04 = "", String key05 = "")
        {
            LogData data = new LogData();
            data.LogType = logtype;
            data.LogID = logid;
            data.LogObject = logobject;
            data.LogSubject = logsubject;
            data.LogContent = logcontent;
            data.LogTime = System.DateTime.Now;
            data.LogKey01 = key01;
            data.LogKey02 = key02;
            data.LogKey03 = key03;
            data.LogKey04 = key04;
            data.LogKey05 = key05;
            _logList_temp.Add(data);
        }


        /// <summary>
        /// 更新保存日志到数据库的队列
        /// </summary>
        private void SwicthLog()
        {
            while (true)
            {
                Thread.Sleep(500);
                try
                {
                    if (_logList_temp.Count() > 0 && _logList.Count==0)
                    {
                        lock (logA)
                        {
                            _logList.Clear();
                            _logList.AddRange(_logList_temp);
                            _logList_temp.Clear();
                        }
                    }
                }
                catch { };
            }
        }

        /// <summary>
        /// 保存日志到数据库
        /// </summary>
        private void SaveLogToDB()
        {
            while(true){
                Thread.Sleep(500);
                if (_logList.Count > 0)
                {
                    lock (logB)
                    {
                        string conStr = LogSQL.GetConnectString();
                        using (SQLiteConnection conn = new SQLiteConnection(conStr))
                        {
                            conn.Open();

                            using (SQLiteCommand cmd = conn.CreateCommand())
                            {
                                foreach (LogData data in _logList)
                                {
                                    if (LogSQL.ExecuteNonQuery(cmd, LOGTABLE.GetTableInsertSQL(data)) == -1)
                                    {
                                        _errorList.Add(data);
                                    }
                                }
                                _logList.Clear();
                            }
                        }
                    }
                }
            }
        }
        
    }
}
