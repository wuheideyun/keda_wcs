using ArrayMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KEDAClient
{
    /// <summary>
    /// 用于参数的配置
    /// </summary>
    public class ParamControl
    {
        /// <summary>
        /// 最低充电量
        /// </summary>
        public static int MinElectricityCharge = 80;

        //任务执行的参数配置：默认是执行true
        /// <summary>
        /// 是否执行窑头充电任务
        /// </summary>
        public static bool Do_HeadCharge = true;

        /// <summary>
        /// 是否执行窑尾充电任务
        /// </summary>
        public static bool Do_TailCharge = true;

        /// <summary>
        /// 是否执行窑头卸货任务
        /// </summary>
        public static bool Do_HeadUnload = true;

        /// <summary>
        /// 是否执行窑尾取货任务
        /// </summary>
        public static bool Do_TailLoad = true;

        /// <summary>
        /// 是否之前去窑头等待区任务
        /// </summary>
        public static bool Do_ToHeadWait = true;

        /// <summary>
        /// 是否执行去窑尾等待区任务
        /// </summary>
        public static bool Do_ToTailWait = true;

        /// <summary>
        /// 是否执行初始化去窑头等待点的任务
        /// </summary>
        public static bool Do_InitToHeadWait = true;


        /// <summary>
        /// 是否执行初始化去窑尾等待点的任务
        /// </summary>
        public static bool Do_InitToTailWait = true;

        /// <summary>
        /// 是否自动执行任务，和界面的执行任务进行对应，
        /// 如果启动 则启动后生成的任务都会自动执行，启动前的任务需要手动确认执行
        /// </summary>
        public static bool Is_AutoExecuteTask = false;

        /// <summary>
        /// 是否自动添加任务
        /// </summary>
        public static bool Is_AutoAddTask = false;


    }

    /// <summary>
    /// 公共数据共享
    /// </summary>
    public class PublicDataContorl
    {
        Object _currentTaskObj = new object();

        private static List<TaskData> _taskDatas = new List<TaskData>();

        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <returns></returns>
        public static List<TaskData> GetTaskData()
        {
            return _taskDatas;
        }

        /// <summary>
        /// 任务完成
        /// </summary>
        /// <param name="no"></param>
        public static void TaskIsSucc(int no)
        {
            TaskData task = _taskDatas.Find(c => { return c.NO == no; });
            if(task != null)
            {
                _taskDatas.Remove(task);
            }
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="task"></param>
        public static void AddTaskData(TaskData task)
        {
            _taskDatas.Add(task);
        }

        
    }

    public class TaskData
    {
        private int _no;

        private string _msg;

        private string _sitemsg;

        public int NO
        {
            get { return _no; }
        }

        public String Msg
        {
            get { return _msg; }
        }

        public String SiteMsg
        {
            get { return _sitemsg; }
        }


        public TaskData(int no,String msg,String sitemsg)
        {
            _no = no;
            _msg = msg;
            _sitemsg = sitemsg;
        }
        
    }
}
