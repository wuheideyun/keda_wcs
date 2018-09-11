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
using GfxServiceContractTaskExcute;
using GfxServiceContractTaskRelate;
using GfxServiceContractClientDispatch;

namespace JTWcfHelper
{
    /// <summary>
    /// 拓展接口公共方法(基本控制)
    /// </summary>
    public class JtWcfDispatchHelper
    {
        #region 私有字段

        /// <summary>对象锁</summary>
        private static object _ans = new object();

        /// <summary>服务接口</summary>
        private static IRemoteDispatch _backupsClass;

        // <summary>WCF通道状态</summary>
        private static bool _channelState = false;

        /// <summary>
        /// 客户端IP地址
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
        static JtWcfDispatchHelper()
        {
            try
            {
                Open();
            }
            catch { }
        }

        #endregion

        #region 私有函数
        /// <summary>新建一个WCF的通道,返回通道是否有效</summary>
        private static bool CreatNewChannel()
        {
            lock (_ans)
            {
                try
                {
                    WcfClientHelper.SetWCFParas(_ipAdress, _portNum, _binding);

                    _backupsClass = WcfClientHelper.CreateService<IRemoteDispatch>();

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
        #endregion

        #region 公共函数

        /// <summary>
        /// 参数初始化
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="banding"></param>
        public static void InitPara(string ip,string port,string banding)
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
        /// 远程调度接口
        /// TaskDispatch task： 自定义的调度
        /// 返回结果   s：成功    Other：具体的异常信息
        /// </summary>
        /// <returns></returns>
        public static string IStartTaskDispatch(TaskDispatch task)
        {
            if (!_channelState && !Open()) { return "通道未打开"; }

            lock (_ans) { try { return _backupsClass.IStartTaskDispatch(task); } catch { return "远程操作异常"; } }
        }
        #endregion
    }
}
