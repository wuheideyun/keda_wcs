using ArrayMap;
using DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
        /// 是否执行进窑头充电任务
        /// </summary>
        public static bool Do_EnterHeadCharge = true;

        /// <summary>
        /// 是否执行进窑尾充电任务
        /// </summary>
        public static bool Do_EnterEndCharge = true;

        /// <summary>
        /// 是否执行出窑头充电任务
        /// </summary>
        public static bool Do_ExitHeadCharge = true;

        /// <summary>
        /// 是否执行出窑尾充电任务
        /// </summary>
        public static bool Do_ExitEndCharge = true;

        /// <summary>
        /// 是否执行进窑头充电完成回去等待点的任务
        /// </summary>
        public static bool Do_EnterHeadChargeSucc = true;

        /// <summary>
        /// 是否执行进窑尾充电完成回去等待点的任务
        /// </summary>
        public static bool Do_EnterEndChargeSucc = true;

        /// <summary>
        /// 是否执行出窑头充电完成回去等待点的任务
        /// </summary>
        public static bool Do_ExitHeadChargeSucc = true;

        /// <summary>
        /// 是否执行出窑尾充电完成回去等待点的任务
        /// </summary>
        public static bool Do_ExitEndChargeSucc = true;

        /// <summary>
        /// 是否执行窑头卸货任务
        /// </summary>
        public static bool Do_HeadUnload = true;

        /// <summary>
        /// 是否执行窑尾取货任务
        /// </summary>
        public static bool Do_EndLoad = true;

        /// <summary>
        /// 是否执行去窑尾对接完成点任务
        /// </summary>
        public static bool Do_ToEndSuc = true;

        /// <summary>
        /// 是否执行去窑头对接完成点任务
        /// </summary>
        public static bool Do_ToHeadSuc = true;

        /// <summary>
        /// 是否执行去窑尾等任务
        /// </summary>
        public static bool Do_ToEndWait = true;

        /// <summary>
        /// 是否执行去窑头等任务
        /// </summary>
        public static bool Do_ToHeadWait = true;

        /// <summary>
        /// 是否执行启动窑头辊台
        /// </summary>
        public static bool Do_HeadPlcUnload = true;

        /// <summary>
        /// 是否执行启动窑尾辊台
        /// </summary>
        public static bool Do_EndPlcLoad = true;

        //状态锁定：默认是false
        /// <summary>
        /// 是否锁定窑头
        /// </summary>
        public static bool Do_HeadPlcLock = true   ;

        /// <summary>
        /// 是否锁定进窑头充电桩
        /// </summary>
        public static bool Do_EnterHeadChargeLock = true ;

        /// <summary>
        /// 是否锁定出窑头充电桩
        /// </summary>
        public static bool Do_ExitHeadChargeLock = true ;

        /// <summary>
        /// 是否锁定窑尾
        /// </summary>
        public static bool Do_EndPlcLock = true ;

        /// <summary>
        /// 是否锁定进窑尾充电桩
        /// </summary>
        public static bool Do_EnterEndChargeLock = true ;

        /// <summary>
        /// 是否锁定出窑尾充电桩
        /// </summary>
        public static bool Do_ExitEndChargeLock = true ;


        /// <summary>
        /// 是否自动执行任务，和界面的执行任务进行对应，
        /// 如果启动 则启动后生成的任务都会自动执行，启动前的任务需要手动确认执行
        /// </summary>
        public static bool Is_AutoExecuteTask = false;

        /// <summary>
        /// 是否自动添加任务
        /// </summary>
        public static bool Is_AutoAddTask = false;

        /// <summary>
        /// 是否忽略窑头放料Agv和Plc的货物状态
        /// </summary>
        public static bool Is_IgnoreHeadUnloadStatus = false;

        /// <summary>
        /// 是否忽略窑尾出货Agv和Plc的货物状态
        /// </summary>
        public static bool Is_IgnoreTailUnloadStatus = false;        
        
        /// <summary>
        /// 是否忽略窑头放料Agv棍台电机状态
        /// </summary>
        public static bool Is_IgnoreHeadStaStatus = false;

        /// <summary>
        /// 是否忽略窑尾出货Agv棍台电机状态
        /// </summary>
        public static bool Is_IgnoreTailStaStatus = false;

        /// <summary>
        /// 忽略窑头放料Agv和Plc的货物状态是发送点击启动时间
        /// </summary>
        private static int _IgnoreHeadUnloadSecond = 30;

        /// <summary>
        /// 忽略窑头放料Agv和Plc的货物状态是发送点击启动时间
        /// </summary>
        public static int IgnoreHeadUnloadSecond
        {
            get
            {   //最小30秒
                return _IgnoreHeadUnloadSecond < 30 ? 30 : _IgnoreHeadUnloadSecond;
            }
            set
            {
                _IgnoreHeadUnloadSecond = value;
            }
        }

        /// <summary>
        /// 忽略窑尾出货Agv和Plc的货物状态是发送电机启动时间
        /// </summary>
        private static int _IgnoreTailUnloadSecond = 30;
        /// <summary>
        /// 忽略窑尾出货Agv和Plc的货物状态是发送电机启动时间
        /// </summary>
        public static int IgnoreTailUnloadSecond
        {
            get
            {   //最小30秒
                return _IgnoreTailUnloadSecond < 30 ? 30 : _IgnoreTailUnloadSecond;
            }
            set
            {
                _IgnoreTailUnloadSecond = value;
            }
        }

        /// <summary>
        /// 窑头等 到 窑头卸  忽略窑头无货状态 让AGV过去窑头卸
        /// </summary>
        public static bool IgnoreHeadUnloadTask = false;

        /// <summary>
        /// 窑头卸 到 窑尾等  忽略AGV的无货状态 让AGV过去窑尾等
        /// </summary>
        public static bool IgnoreAgvUnloadTask = false;

        /// <summary>
        /// 窑尾等 到 窑尾取  忽略窑尾有货状态 让AGV过去接货
        /// </summary>
        public static bool IgnoreTailLoadTask = false;

        /// <summary>
        /// 窑尾取 到 窑头等  忽略AGV有货状态 让AGV窑头等
        /// </summary>
        public static bool IgnoreAgvLoadTask = false;


        public static MapList LowEleList = new MapList();
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
            if (task != null)
            {
                _taskDatas.Remove(task);
            }
        }

        public static void StopTask(int no)
        {
            F_DataCenter.MTask.StopTask(no);
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


    /// <summary>
    /// 用于获取AGV的详细信息
    /// </summary>
    public class AGV
    {
        private DeviceBackImf _dev;
        private ProtyBackImf _devR;
        private String _value;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dev"></param>
        public AGV(DeviceBackImf dev)
        {
            _dev = dev;
        }

        /// <summary>
        /// 目标站点
        /// </summary>
        /// <returns></returns>
        public String Point()
        {
            return GetRValue("0003");
        }

        /// <summary>
        /// 当前所在站点
        /// </summary>
        /// <returns></returns>
        public String NowPoint()
        {
            return GetRValue("0027");
        }

        /// <summary>
        /// 地标
        /// </summary>
        /// <returns></returns>
        public String Site()
        {
            return GetRValue("0002");
        }

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public String GetRValue(String id)
        {
            _devR = _dev.IGet(id);
            return _devR != null ? _devR.RValue : "";
        }

        /// <summary>
        /// 获取电量
        /// </summary>
        /// <returns></returns>
        public String Electicity()
        {
            return GetRValue("0007");
        }

        /// <summary>
        /// 获取运行方向
        /// </summary>
        /// <returns></returns>
        public String Direction()
        {
            try
            {
                int status = Convert.ToInt32(GetRValue("0005"));
                switch (status)
                {
                    case 0:
                        _value = "前进";
                        break;
                    case 1:
                        _value = "后退";
                        break;
                    default:
                        _value = "未知";
                        break;
                }
            }
            catch
            {
                _value = "未知";
            }
            return _value;
        }

        /// <summary>
        /// 充电状态
        /// </summary>
        /// <returns></returns>
        public String ChargeStatus()
        {
            return GetRValue("0008");
        }

        /// <summary>
        /// 空闲状态
        /// </summary>
        /// <returns></returns>
        public String FreeStatus()
        {
            return GetRValue("0010");
        }

        /// <summary>
        /// Agv状态
        /// </summary>
        /// <returns></returns>
        public String AgvStatus()
        {
            return AGV.GetDevStatus(_dev);
        }

        /// <summary>
        /// 速度
        /// </summary>
        /// <returns></returns>
        public String Speed()
        {
            return GetRValue("0004");
        }

        /// <summary>
        /// 货物状态
        /// </summary>
        /// <returns></returns>
        public String Sta_Material()
        {
            try
            {
                int status = Convert.ToInt32(GetRValue("0036"));
                switch (status)
                {
                    case 1:
                        _value = "有货";
                        break;
                    case 2:
                        _value = "无货";
                        break;
                    case 3:
                        _value = "传送中";
                        break;
                    default:
                        _value = "未知";
                        break;
                }
            }
            catch
            {
                _value = "未知";
            }
            return _value;
        }

        /// <summary>
        /// 电机状态
        /// </summary>
        /// <returns></returns>
        public String Sta_Monitor()
        {
            try
            {
                int status = Convert.ToInt32(GetRValue("0037"));
                switch (status)
                {
                    case 1:
                        _value = "电机正转";
                        break;
                    case 2:
                        _value = "电机反转";
                        break;
                    case 3:
                        _value = "电机停止";
                        break;
                    default:
                        _value = "未知";
                        break;
                }
            }
            catch
            {
                _value = "未知";
            }
            return _value;
        }

        /// <summary>
        /// 是否准备
        /// </summary>
        /// <returns></returns>
        public String IsReady()
        {
            _value = GetRValue("0028");
            return !_value.Equals("") && _value.Equals("0") ? "未准备" : "已准备";
        }

        /// <summary>
        /// 在轨道
        /// </summary>
        public String OnTrack()
        {
            _value = GetRValue("0029");
            return !_value.Equals("") && _value.Equals("0") ? "脱轨" : "在轨道上";
        }

        /// <summary>
        /// 交管状态
        /// </summary>
        /// <returns></returns>
        public String Traffic()
        {
            _value = GetRValue("T01");
            return !_value.Equals("") && _value.Equals("False") ? "未被交管" : GetRValue("T02");
        }

        public static int GetDevEle(DeviceBackImf item)
        {
            if (item.DevType.Equals("WK_PLC")) return 100;
            if (int.TryParse(item.IGet("0007").RValue ?? "0", out int ele))
            {
                return ele;
            }
            return ele;
        }

        /// <summary>
        /// 获取设备状态
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static String GetDevStatus(DeviceBackImf item)
        {
            String status = "";
            if (!item.IsAlive)
            {
                status = "离线";
            }
            else if (item.DevType == "WK_PLC")
            {
                status = "在线";
            }
            else if (item.ProtyList[ConstSetBA.在轨道上].RValue == "0")
            {
                status = "脱轨";
            }
            else if (item.ProtyList[ConstSetBA.运行状态].RValue == "2")
            {
                status = "障碍物";
            }
            else if (item.ProtyList[ConstSetBA.交管状态].RValue == "True")
            {
                status = "被交管(" + item.ProtyList[ConstSetBA.交管设备].RValue + ")";
            }
            else if (item.ProtyList[ConstSetBA.空闲].RValue == "True" && !F_DataCenter.MDev.IsDevInDispath(item.DevId))
            {
                status = "空闲";
            }
            else if (item.ProtyList[ConstSetBA.充电状态].RValue == "1")
            {
                status = "充电中";
            }
            else
            {
                status = "任务中";
            }
            return status;
        }

    }


    /// <summary>
    /// 用于获取PLC的详细信息
    /// </summary>
    public class PLC
    {
        private DeviceBackImf _dev;
        private ProtyBackImf _devR;
        private String _value;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dev"></param>
        public PLC(DeviceBackImf dev)
        {
            _dev = dev;
        }

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public String GetRValue(String id)
        {
            _devR = _dev.IGet(id);
            return _devR != null ? _devR.RValue : "";
        }

        /// <summary>
        /// 货物状态
        /// </summary>
        /// <returns></returns>
        public String Sta_Material()
        {
            try
            {
                int status = Convert.ToInt32(GetRValue("0001"));
                switch (status)
                {
                    case 1:
                        _value = "有货";
                        break;
                    case 2:
                        _value = "无货";
                        break;
                    case 3:
                        _value = "传送中";
                        break;
                    default:
                        _value = "未知";
                        break;
                }
            }
            catch
            {
                _value = "未知";
            }
            return _value;
        }

        /// <summary>
        /// 电机状态
        /// </summary>
        /// <returns></returns>
        public String Sta_Monitor()
        {
            try
            {
                int status = Convert.ToInt32(GetRValue("0002"));
                switch (status)
                {
                    case 1:
                        _value = "电机正转";
                        break;
                    case 2:
                        _value = "电机反转";
                        break;
                    case 3:
                        _value = "电机停止";
                        break;
                    default:
                        _value = "未知";
                        break;
                }
            }
            catch
            {
                _value = "未知";
            }
            return _value;
        }

        /// <summary>
        /// 故障状态
        /// </summary>
        /// <returns></returns>
        public String Sta_Error()
        {
            return GetRValue("0003");
        }

        /// <summary>
        /// 备用信息
        /// </summary>
        /// <returns></returns>
        public String SpareInform()
        {
            return GetRValue("0004");
        }

        /// <summary>
        /// PLC位置
        /// </summary>
        /// <returns></returns>
        public String PLCLocat()
        {
            return PLC.GetDevLocat(_dev);
        }

        /// <summary>
        /// 获取PLC位置
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static String GetDevLocat(DeviceBackImf item)
        {
            String status = "";
            if (item.DevId == "PLC01")
            {
                status = "窑尾";
            }
            else if (item.DevId == "PLC02")
            {
                status = "窑头";
            }
            else
            {
                status = "未知";
            }
            return status;
        }

    }


    public class DoubleBufferListView : ListView
    {
        /// <summary>
        /// 使用双缓冲：添加新类继承ListView 对其重写
        /// </summary>
        public DoubleBufferListView()
        {
            SetStyle(ControlStyles.DoubleBuffer |
              ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }
    }

    /// <summary>
    /// 保存任务信息
    /// </summary>
    public class TaskData
    {
        /// <summary>
        /// 任务对象的hash值，根据这个找回F_ExcTask对象
        /// </summary>
        private int _no;

        /// <summary>
        /// 任务信息
        /// </summary>
        private string _msg;

        /// <summary>
        /// 站点线路信息
        /// </summary>
        private string _sitemsg;

        /// <summary>
        /// 任务对象的hash值，根据这个找回F_ExcTask对象
        /// </summary>
        public int NO
        {
            get { return _no; }
        }

        /// <summary>
        /// 任务信息
        /// </summary>
        public String Msg
        {
            get { return _msg; }

        }

        /// <summary>
        /// 站点线路信息
        /// </summary>
        public String SiteMsg
        {
            get { return _sitemsg; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="no"></param>
        /// <param name="msg"></param>
        /// <param name="sitemsg"></param>
        public TaskData(int no, String msg, String sitemsg)
        {
            _no = no;
            _msg = msg;
            _sitemsg = sitemsg;
        }

    }
}
