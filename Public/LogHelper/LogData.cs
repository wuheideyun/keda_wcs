using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogHelper
{


    internal class LogData
    {
        /// <summary>
        /// 日志类型
        /// </summary>
        public LOGTYPE LogType { get; set; }

        /// <summary>
        /// 日志序号 自动增长
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// 日志ID  任务标识，调度ID等
        /// </summary>
        public String LogID { get; set; }

        /// <summary>
        /// 日志对象 AGV名称 PLC名称 等
        /// </summary>
        public String LogObject { get; set; }

        /// <summary>
        /// 日志主题   去等待点。。。
        /// </summary>
        public String LogSubject { get; set; }

        /// <summary>
        /// 日志内容 
        /// </summary>
        public String LogContent { get; set; }

        /// <summary>
        /// 日志记录时间
        /// </summary>
        public DateTime LogTime { get; set; }

        /// <summary>
        /// 备用字段1
        /// </summary>
        public String LogKey01 { get; set; }

        /// <summary>
        /// 备用字段2
        /// </summary>
        public String LogKey02{ get; set; }

        /// <summary>
        /// 备用字段3
        /// </summary>
        public String LogKey03{ get; set; }

        /// <summary>
        /// 备用字段4
        /// </summary>
        public String LogKey04 { get; set; }

        /// <summary>
        /// 备用字段5
        /// </summary>
        public String LogKey05 { get; set; }

        
    }
}
