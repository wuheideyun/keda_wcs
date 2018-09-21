using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KEDAClient
{
    
    public static class F_DataCenter
    {
        static bool _init = false;

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
        public static void Init()
        {
            if (!_init)
            {
                _init = true;

                _mDev = new F_DevManager();

                _mTask = new F_ExcTaskManager();

                _mLogic = new F_Logic();
            }
        }
    }
}
