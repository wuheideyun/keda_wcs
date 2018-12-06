using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace KEDAClient
{

    public static class F_DataCenter
    {
        static bool _init = false;

        /// <summary>
        /// 自动生成任务开启关闭标识
        /// </summary>
        static bool _initF_logic = false;

        /// <summary>
        /// 执行任务启动与执行标识
        /// </summary>
        static bool _initF_ExcTask = false;

        /// <summary>
        /// 设备管理器
        /// </summary>
        static F_DevManager _mDev = null;

        /// <summary>
        /// 任务管理器
        /// </summary>
        static F_ExcTaskManager _mTask = null;

        /// <summary>
        /// 流程管理器
        /// </summary>
        static F_Logic _mLogic = null;

        /// <summary>
        /// 
        /// </summary>
        static int _clearTime = 30;

        /// <summary>
        /// 任务超时清除时间
        /// </summary>
        public static int ClearTime
        {
            get { return F_DataCenter._clearTime; }
        }

        /// <summary>
        /// 流程管理器
        /// </summary>
        public static F_Logic MLogic
        {
            get { return F_DataCenter._mLogic; }
        }

        /// <summary>
        /// 设备管理器
        /// </summary>
        public static F_DevManager MDev
        {
            get { return F_DataCenter._mDev; }
        }

        /// <summary>
        /// 任务管理器
        /// </summary>
        public static F_ExcTaskManager MTask
        {
            get { return F_DataCenter._mTask; }
            set { F_DataCenter._mTask = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Init(SynchronizationContext context, ListBox listBoxOutput)
        {
            if (!_init)
            {
                _init = true;

                _mDev = new F_DevManager(context, listBoxOutput);

                _mTask = new F_ExcTaskManager(context, listBoxOutput);

                _mLogic = new F_Logic(context, listBoxOutput);
            }
        }

        /// <summary>
        /// 停止后台服务
        /// </summary>
        public static void StopServer()
        {
            if (_init)
            {
                _mDev.ThreadStop();
                _mTask.ThreadStop();
                _mLogic.ThreadStop();
                _init = false;
            }
        }
    }
}
