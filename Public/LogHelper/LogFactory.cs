using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogHelper
{
    /// <summary>
    /// 日志工厂：1.初始化；2.记录日志
    /// </summary>
    public class LogFactory
    {
        /// <summary>
        /// 初始化完成标准
        /// </summary>
        private static Boolean _init;

        /// <summary>
        /// 日志操作类
        /// </summary>
        private static LogHelper LOG { get; set; }

        /// <summary>
        /// 初始化日志服务：没有密码，默认数据文件名
        /// </summary>
        public static void Init()
        {
            if (!_init)
            {
                LogSQL.InitSQLiteDB();
                LOG = new LogHelper();
            }
        }

        /// <summary>
        /// 带有数据库密码，和文件明的初始化
        /// </summary>
        /// <param name="passwd"></param>
        /// <param name="dbName"></param>
        public static void InitParam(String passwd,String dbName)
        {
            if (!_init)
            {
                LogSQL.SetDbFileName(dbName);
                LogSQL.SetDbPaswd(passwd);
                LogSQL.InitSQLiteDB();
                LOG = new LogHelper();
            }
        }

        /// <summary>
        /// 设置日志文件打开密码，默认没有密码
        /// </summary>
        public static void SetDbPwd(String passworkd)
        {
            LogSQL.SetDbPaswd(passworkd);
        }


        /// <summary>
        ///记录错误信息
        /// </summary>
        public static void LogError(String errorname, String errorcontent)
        {
            LOG.LogAdd(LOGTYPE.ERROR,"","",errorname, errorcontent);
        }

        /// <summary>
        ///记录请求调度信息
        /// </summary>
        public static void LogDispatch(String dispatchid, String dispatchname, String dispatchcontent)
        {
            LOG.LogAdd(LOGTYPE.DISPATCH,dispatchid,"", dispatchname, dispatchcontent);
        }

        /// <summary>
        ///记录请求调度信息
        /// </summary>
        public static void LogFinish(String dispatchid, String dispatchname, String dispatchcontent)
        {
            LOG.LogAdd(LOGTYPE.FINISH, dispatchid,"", dispatchname, dispatchcontent);
        }

        /// <summary>
        /// 记录运行日志：如启动工具，启动服务等
        /// </summary>
        /// <param name="runningmsg"></param>
        public static void LogRunning(String runningmsg)
        {
            LOG.LogAdd(LOGTYPE.RUNNING, "", "","", runningmsg);
        }

        /// <summary>
        /// 自定义添加日志
        /// </summary>
        /// <param name="logtype">日志类型：</param>
        /// <param name="logid">日志ID</param>
        /// <param name="logobject">日志对象</param>
        /// <param name="logsubject">日志主题</param>
        /// <param name="logcontent">日志内容</param>
        /// <param name="key01">保留字段1</param>
        /// <param name="key02">保留字段2</param>
        /// <param name="key03">保留字段3</param>
        /// <param name="key04">保留字段4</param>
        /// <param name="key05">保留字段5</param>
        public static void LogAdd(LOGTYPE logtype, String logid,String logobject, String logsubject,
            String logcontent, String key01 = "",
            String key02 = "", String key03 = "",
            String key04 = "", String key05 = "")
        {
            LOG.LogAdd(logtype, logid,logobject, logsubject,logcontent,key01,key02,key03,key04,key05);
        }
    }
}
