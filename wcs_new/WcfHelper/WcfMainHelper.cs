using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using GfxCommonInterfaces;
using GfxServiceContractPluginInterface;
using Gfx.GfxDataManagerServer;
using Gfx.RCommData;
using GfxServiceContractClient;

namespace JTWcfHelper
{
    /// <summary>
    /// 设备状态发生改变时间委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DevStatueEvent(object sender, DevStatueEventArgs e);

    /// <summary>
    /// 设备成员状态发生改变
    /// </summary>
    public class DevStatueEventArgs : EventArgs
    {
        /// <summary>
        /// 状态发生改变的成员链表
        /// </summary>
        private GfxList<DeviceBackImf> _devObjs;

        /// <summary>
        /// 状态发生改变的成员链表
        /// </summary>
        public GfxList<DeviceBackImf> DevObjs
        {
            get { return _devObjs; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="temp">设备链表</param>
        public DevStatueEventArgs(GfxList<DeviceBackImf> temp)
        {
            _devObjs = temp;
        }
    }

    /// <summary>
    /// 设备属性发生改变
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DevPtyEvent(object sender, DevPtyEventArgs e);

    /// <summary>
    /// 设备属性发生改变
    /// </summary>
    public class DevPtyEventArgs : EventArgs
    {
        /// <summary> 发生改变的对象成员 ///</summary>
        private DeviceBackImf _changeMember;

        /// <summary> 操作类型：增 删 改 ///</summary>
        private OperTypeEnum _operType;

        /// <summary> 发生改变的对象成员 ///</summary>
        public DeviceBackImf ChangeMember
        {
            get { return _changeMember; }
            set { _changeMember = value; }
        }

        /// <summary> 操作类型：增 删 改 ///</summary>
        public OperTypeEnum OperType
        {
            get { return _operType; }
            set { _operType = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dev">设备</param>
        /// <param name="oper">操作类型</param>
        public DevPtyEventArgs(DeviceBackImf dev, OperTypeEnum oper)
        {
            _changeMember = dev;

            _operType = oper;
        }
    }

    /// <summary>
    /// 拓展接口公共方法(基本控制)
    /// </summary>
    public class WcfMainHelper
    {
        #region 私有字段

        /// <summary>对象锁</summary>
        private static object _ans = new object();

        /// <summary>服务接口</summary>
        private static IPluginCtrInterface _backupsClass;

        /// <summary>回调对象</summary>
        private static IServerCtrBack _callBack;

        // <summary>WCF通道状态</summary>
        private static bool _channelState = false;

        // <summary>心跳定时器</summary>
        private static System.Timers.Timer timer;

        /// <summary>
        /// 服务地址
        /// </summary>
        //private static string _ipAdress = "192.168.0.2";

        /// <summary>
        /// 服务地址
        /// </summary>
        private static string _ipAdress = "127.0.0.1";

        /// <summary>
        /// 服务端口
        /// </summary>
        private static string _portNum = "8192";

        /// <summary>
        /// 绑定类型
        /// </summary>
        private static string _binding = "nettcpbinding";

        #endregion

        #region 公共属性
        /// <summary>
        /// 服务端连接状态
        /// </summary>
        public static bool IsConnected
        {
            get
            {
                return _channelState;
            }
        }
        #endregion

        #region 公共事件

        /// <summary>
        /// 设备状态发生改变事件
        /// </summary>
        public static DevStatueEvent GetNewMsgEvent;

        /// <summary>
        /// 设备成员发生改变事件（增、删、改）
        /// </summary>
        public static DevPtyEvent DevPtyChangeEvent;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        static WcfMainHelper()
        {
            try
            {
                _callBack = new BackupClass(SynchronizationContext.Current);

                Open();

                InitTimer();
            }
            catch { }
        }

        #endregion

        #region 私有函数

        /// <summary>
        /// 初始化心跳
        /// </summary>
        private static void InitTimer()
        {
            try
            {
                SendHeart();

                timer = new System.Timers.Timer();
                timer.Elapsed += new System.Timers.ElapsedEventHandler(TimerTick);
                timer.Interval = 10000;
                timer.Start();
                timer.Enabled = true;
            }
            catch { }
        }

        /// <summary>
        /// 心跳发送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void TimerTick(object sender, ElapsedEventArgs e)
        {
            if (timer == null) { return; }

            timer.Enabled = false;

            try 
            {
                if (SendHeart() == "true")
                { 
                    
                }
            }
            catch { }

            timer.Enabled = true;
        }

        /// <summary>新建一个WCF的通道,返回通道是否有效</summary>
        private static bool CreatNewChannel()
        {
            lock (_ans)
            {
                try
                {
                    WcfClientHelper.SetWCFParas(_ipAdress, _portNum, _binding);

                    _backupsClass = WcfClientHelper.CreateDuplexService<IPluginCtrInterface>("IPluginCtrInterface", _callBack);

                    IClientChannel channel = _backupsClass as IClientChannel;

                    if (channel == null) { return false; }

                    channel.Faulted += ChannelFaulted;

                    if (channel.State != CommunicationState.Opened) { channel.Open(); }

                    if (!_channelState) { _channelState = true; }

                    return true;
                }
                catch { if (_channelState) { _channelState = false; } return false; }
            }
        }

        // <summary>通道异常时，关闭当前通道</summary>
        private static void ChannelFaulted(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>显式打开当前的WCF通道，如果通道不存在或异常，则创建新的通道</summary>
        public static bool Open()
        {
            lock (_ans)
            {
                IClientChannel channel = _backupsClass as IClientChannel;

                if (null != _backupsClass && null != channel)
                {
                    if (channel.State == CommunicationState.Opened || channel.State == CommunicationState.Opening)
                    {
                        try { if (!_channelState) { _channelState = true; } return true; }
                        catch { }
                    }

                    if (channel.State == CommunicationState.Created || channel.State == CommunicationState.Closed || channel.State == CommunicationState.Closing)
                    {
                        try { channel.Open(); if (!_channelState) { _channelState = true; } return true; }
                        catch { }
                    }

                    Close();
                }

                return CreatNewChannel();
            }
        }

        /// <summary>显式关闭和销毁当前的WCF通道</summary>
        public static void Close()
        {
            lock (_ans)
            {
                IClientChannel channel = _backupsClass as IClientChannel;

                try
                {
                    if (null == channel) { return; }

                    ClearAllEvents(channel);

                    if (channel.State == CommunicationState.Opened ||
                        channel.State == CommunicationState.Opening ||
                        channel.State == CommunicationState.Created) { channel.Close(); }
                }
                catch { }
                finally { _backupsClass = null; _channelState = false; }
            }
        }

        /// <summary>清除一个对象的所有事件</summary>
        private static void ClearAllEvents(object obj)
        {
            if (null == obj) { return; }

            EventInfo[] events = obj.GetType().GetEvents(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            if (null == events || events.Length < 1) { return; }

            for (int i = 0; i < events.Length; ++i)
            {
                try
                {
                    EventInfo e = events[i];

                    FieldInfo fi = e.DeclaringType.GetField(e.Name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

                    if (null == fi) { continue; }

                    fi.SetValue(obj, null);
                }
                catch { }
            }
        }

        /// <summary>心跳发送</summary>
        private static string SendHeart()
        {
            if (!_channelState && !Open()) { return false.ToString(); }

            lock (_ans) { try { _backupsClass.GetHeart(); return true.ToString(); } catch { return false.ToString(); } }
        }

        #endregion

        #region 公共函数

        /// <summary>
        /// 参数初始化
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="banding"></param>
        public static void InitPara(string ip, string port, string banding)
        {
            if (!string.IsNullOrEmpty(ip))
            {
                _ipAdress = ip;
            }

            if (!string.IsNullOrEmpty(port))
            {
                _portNum = port;
            }

            if (!string.IsNullOrEmpty(banding))
            {
                _binding = banding;
            }
        }

        /// <summary>
        /// 查询所有设备
        /// </summary>
        /// <returns></returns>
        public static GfxList<DeviceBackImf> GetDevList()
        {
            if (!_channelState && !Open()) { return null; }

            lock (_ans) { try { return _backupsClass.GetDevList(); } catch { return null; } }
        }

        /// <summary>
        /// 客户端查询设备状态
        /// </summary>
        /// <param name="devid">设备id</param>
        /// <returns></returns>
        public static DeviceBackImf GetDev(string devid)
        {
            if (!_channelState && !Open()) { return null; }

            lock (_ans) { try { return _backupsClass.GetDev(devid); } catch { return null; } }
        }

        /// <summary>
        /// 客户端操作设备
        /// </summary>
        /// <param name="devid">操作设备id</param>
        /// <param name="order">操作指令类型</param>
        /// <returns></returns>
        public static bool SendOrder(string devid, CommonDeviceOrderObj order)
        {
            if (!_channelState && !Open()) { return false; }

            lock (_ans) { try { return _backupsClass.SendOrder(devid, order); } catch { return false; } }
        }

        /// <summary>
        /// 客户端获取任务执行状态
        /// </summary>
        /// <param name="taskId">任务</param>
        /// <param name="mark">客户端标记</param>
        /// <returns></returns>
        public static DispatchBackMember GetDispatch(string disGuid)
        {
            if (!_channelState && !Open()) { return null; }

            lock (_ans) { try { return _backupsClass.GetDispatch(disGuid); } catch { return null; } }
        }

        /// <summary>
        /// 客户端获取任务执行状态
        /// </summary>
        /// <param name="taskId">任务</param>
        /// <param name="mark">客户端标记</param>
        /// <returns></returns>
        public static List<DispatchBackMember> GetDispatchList()
        {
            if (!_channelState && !Open()) { return null; }

            lock (_ans) { try { return _backupsClass.GetDispatchList(); } catch { return null; } }
        }

        /// <summary>
        /// 获取任务状态
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public static ResultTypeEnum GetDipatchStatue(string disGuid)
        {
            DispatchBackMember temp = GetDispatch(disGuid);

            return (temp == null) ? ResultTypeEnum.Unknow : temp.OrderStatue;
        }

        /// <summary>
        /// 开启或暂停任务
        /// </summary>
        /// <param name="taskId">任务</param>
        /// <param name="oper">操作类型</param>
        /// <param name="mark">客户端标记</param>
        /// <returns></returns>
        public static bool CtrDispatch(string disGuid, DisOrderCtrTypeEnum oper)
        {
            if (!_channelState && !Open()) { return false; }

            lock (_ans) { try { return _backupsClass.CtrDispatch(disGuid, oper); } catch { return false; } }
        }

        /// <summary>
        /// 开启或暂停任务
        /// </summary>
        /// <param name="taskId">任务</param>
        /// <param name="oper">操作类型</param>
        /// <param name="mark">客户端标记</param>
        /// <returns></returns>
        public static bool StartDispatch(DispatchOrderObj order, out string msg)
        {
            msg = "未连接服务端！";

            if (!_channelState && !Open()) { return false; }

            lock (_ans) { try { msg = _backupsClass.StartDispatch(order); } catch { return false; } }

            return msg == "s" ? true : false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dev"></param>
        /// <param name="emergency"></param>
        /// <returns></returns>
        public static bool ISetDevEmergency(string dev, bool emergency)
        {
            if (!_channelState && !Open()) { return false; }

            lock (_ans) { try { return _backupsClass.ISetDevEmergency(dev, emergency); } catch { return false; } }
        }
        #endregion
    }

    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, IncludeExceptionDetailInFaults = true, UseSynchronizationContext = false, ValidateMustUnderstand = true)]
    public class BackupClass : IServerCtrBack
    {
        /// <summary>线程同步上下文对象</summary>
        private static SynchronizationContext _syncContext;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sysContext"></param>
        public BackupClass(SynchronizationContext sysContext)
        {
            if (null == _syncContext) { _syncContext = sysContext; }
        }

        /// <summary>
        /// 服务端更新设备状态
        /// </summary>
        /// <param name="devObjs"></param>
        public void UpdateDevStatue(GfxList<DeviceBackImf> devObjs)
        {
            try
            {
                if (devObjs == null || devObjs.Count == 0) { return; }

                if (WcfMainHelper.GetNewMsgEvent != null)
                {
                    WcfMainHelper.GetNewMsgEvent(null, new DevStatueEventArgs(devObjs));
                }
            }
            catch { }
        }

        /// <summary>
        /// 成员变化
        /// </summary>
        /// <param name="member"></param>
        public void MemberListUpdate(MemberChangeBackImf member)
        {
            try
            {
                DeviceBackImf temp = member.ChangeMemberDev;

                if (WcfMainHelper.DevPtyChangeEvent != null)
                {
                    WcfMainHelper.DevPtyChangeEvent(null, new DevPtyEventArgs(temp, member.OperType));
                }
            }
            catch { }
        }
    }
}
