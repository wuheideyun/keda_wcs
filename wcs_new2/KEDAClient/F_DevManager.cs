using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DataContract;
using FLCommonInterfaces;
using WcfHelper;

namespace KEDAClient
{
    /// <summary>
    /// 设备管理器
    /// </summary>
    public class F_DevManager
    {
        /// <summary>
        /// 对象锁
        /// </summary>
        object _ans = new object();

        /// <summary>
        /// 对象锁
        /// </summary>
        object _ansDis = new object();

        /// <summary>
        /// 服务端设备链表
        /// </summary>
        List<DeviceBackImf> _devList = new List<DeviceBackImf>();

        /// <summary>
        /// 
        /// </summary>
        List<FDispatchBackImf> _dispatchList = new List<FDispatchBackImf>();

        /// <summary>
        /// 线程
        /// </summary>
        Thread _thread = null;

        private SynchronizationContext mainThreadSynContext;

        ListBox listBox;

        /// <summary>
        /// 构造函数
        /// </summary>
        public F_DevManager(SynchronizationContext context, ListBox listBoxOutput)
        {
            mainThreadSynContext = context;

            listBox = listBoxOutput;

            _thread = new Thread(ThreadFunc);

            _thread.Name = "服务端数据刷新线程";

            _thread.IsBackground = true;

            _thread.Start();
        }

        /// <summary>
        /// 展示服务日志到界面
        /// </summary>
        private void sendServerLog(String msg)
        {
            mainThreadSynContext.Post(new SendOrPostCallback(displayLogToUi), msg);

        }

        /// <summary>
        /// 回到主线程，操作日志框，展示日志
        /// </summary>
        private void displayLogToUi(object obj)
        {
            String msg = (String)obj;

            if (string.IsNullOrEmpty(msg)) { msg = "空消息"; }

            if (listBox.Items.Count > 200)
            {
                listBox.Items.RemoveAt(0);
            }

            listBox.Items.Add(string.Format("【{0}】：{1}", DateTime.Now.TimeOfDay.ToString(), msg));

            listBox.SelectedIndex = listBox.Items.Count - 1;
        }

        /// <summary>
        /// 事务线程
        /// </summary>
        private void ThreadFunc()
        {
            List<DeviceBackImf> getList = null;

            List<FDispatchBackImf> dispatchList = null;

            while (true)
            {
                Thread.Sleep(500);

                try
                {
                    getList = WcfMainHelper.GetDevList();

                    dispatchList = WcfMainHelper.GetDispatchList();

                    if (getList != null)
                    {
                        lock (_ans)
                        {
                            _devList.Clear();

                            _devList.AddRange(getList);
                        }
                    }

                    if (dispatchList != null)
                    {
                        lock (_ansDis)
                        {
                            _dispatchList.Clear();

                            _dispatchList.AddRange(dispatchList);
                        }
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// 获取指定设备
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DeviceBackImf IGetDev(string devId)
        {
            lock (_ans)
            {
                return _devList.Find(c => { return c.DevId == devId; });
            }
        }

        /// <summary>
        /// 获取设备传感器值
        /// </summary>
        /// <param name="devId"></param>
        /// <param name="sensNum"></param>
        /// <returns></returns>
        public string IGetSenValue(string devId, string sensNum)
        {
            DeviceBackImf dev = IGetDev(devId);

            if (dev != null)
            {
                ProtyBackImf sens = dev.IGet(sensNum);

                if (sens != null) { return sens.RValue; }
            }

            return "can't get value!";
        }

        /// <summary>
        /// 判定AGV是否处于调度中
        /// </summary>
        /// <param name="devid"></param>
        /// <returns></returns>
        public bool IsDevInDispath(string devid)
        {
            lock (_ans)
            {
                return _dispatchList.Find(c => { return c.Dev == devid && c.Statue != EnumResultType.Suc; }) != null;
            }
        }

        /// <summary>
        /// 获取在某个地标上的AGV
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public F_AGV IGetDevOnSite(string site)
        {
            try
            {
                DeviceBackImf dev = _devList.Find(c => { return c.DevType == "Magnet_Basic" 
                    && c.ProtyList[ConstSetBA.地标].RValue == site
                    && c.ProtyList[ConstSetBA.空闲].RValue == "True";
                });

                if (dev != null) { return new F_AGV(dev.DevId); }
            }
            catch { }

            return null;
        }

        /// <summary>
        /// 获取不在窑头卸载等待区，并且有货的AGV
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public List<F_AGV> IGetDevNotOnWaitSite(String devid = null)
        {
            try
            {
                List<DeviceBackImf> devs = _devList.FindAll(c =>
                {
                    return c.DevType == "Magnet_Basic" && c.IsAlive
                    && devid != null ? c.DevId == devid : true
                    && c.ProtyList[ConstSetBA.地标].RValue != ConstSetBA.窑头卸载等待区
                    && c.ProtyList[ConstSetBA.空闲].RValue == "True"
                    //&& (c.ProtyList[ConstSetBA.货物状态].RValue == ConstSetBA.AGV有货 || c.ProtyList[ConstSetBA.货物状态].RValue == "未知")
                    ;
                });

                if (devs != null)
                {
                    List<F_AGV> list = new List<F_AGV>();
                    foreach (DeviceBackImf dev in devs)
                    {
                        list.Add(new F_AGV(dev.DevId));
                    }
                    return list;
                }
            }
            catch { }

            return null;
        }

        /// <summary>
        /// 获取不在窑尾装载等待区，并且没货的AGV
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public List<F_AGV> IGetDevNotLoadOnWaitSite(String devid = null)
        {
            try
            {
                List<DeviceBackImf> devs = _devList.FindAll(c => { return c.IsAlive 
                    && devid!=null ? c.DevId == devid : true
                    && c.DevType == "Magnet_Basic"
                    && c.ProtyList[ConstSetBA.空闲].RValue == "True"
                    && c.ProtyList[ConstSetBA.地标].RValue != ConstSetBA.窑尾装载等待区 
                    //&& (c.ProtyList[ConstSetBA.货物状态].RValue == ConstSetBA.AGV无货 || c.ProtyList[ConstSetBA.货物状态].RValue == "未知")
                    ; });

                if (devs != null)
                {
                    List<F_AGV> list = new List<F_AGV>();
                    foreach (DeviceBackImf dev in devs)
                    {
                        list.Add(new F_AGV(dev.DevId));
                    }
                    return list;
                }
            }
            catch { }

            return null;
        }

        /// <summary>
        /// 获取发生错误，或者离线的AGV
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public List<F_AGV> ErrorOrFalse()
        {
            try
            {
                List<DeviceBackImf> devs = _devList.FindAll(c =>{return c.DevType == "Magnet_Basic" &&
                (!c.IsAlive|| c.ProtyList[ErrorType.脱轨].RValue == "1" || c.ProtyList[ErrorType.急停触发].RValue == "1" ||
                 c.ProtyList[ErrorType.驱动器故障].RValue == "1" || c.ProtyList[ErrorType.轨道错误].RValue == "1" || c.ProtyList[ErrorType.机械撞].RValue == "1");
                });

                if (devs != null)
                {
                    List<F_AGV> list = new List<F_AGV>();
                    foreach (DeviceBackImf dev in devs)
                    {
                        list.Add(new F_AGV(dev.DevId));
                    }
                    return list;
                }
            }
            catch { }

            return null;
        }

        /// <summary>
        /// 停止事务线程
        /// </summary>
        public void ThreadStop()
        {
            if (_thread != null) _thread.Abort();
        }

        /// <summary>
        /// 获取设备状态
        /// </summary>
        /// <returns></returns>
        public List<DevData> GetDevData()
        {
            List<DevData> list = new List<DevData>();
            String status = "";
            lock (_ans)
            {
                List<DeviceBackImf> devs = _devList.FindAll(c => { return c.DevType == "Magnet_Basic";});
                //&& c.ProtyList[ConstSetBA.空闲].RValue == "True"
                foreach (var item in devs)
                {
                    if (!item.IsAlive)
                    {
                        status = "离线";
                    }
                    else if (item.ProtyList[ConstSetBA.在轨道上].RValue == "0")
                    {
                        status = "脱轨";
                    }
                    else if (item.ProtyList[ConstSetBA.交管状态].RValue == "True")
                    {
                        status = "被交管("+ item.ProtyList[ConstSetBA.交管设备].RValue+")";
                    }
                    else if (item.ProtyList[ConstSetBA.空闲].RValue == "True")
                    {
                        status = "空闲";
                    }
                    else
                    {
                        status = "任务中";
                    }

                    list.Add(new DevData(item.DevId, status));
                }

                return list;
            }
        }
        
    }

    /// <summary>
    /// 保存简单的设备信息
    /// </summary>
    public class DevData
    {
        private String _devid;
        private String _status;

        public String DevID
        {
            get { return _devid; }
        }

        public String Status
        {
            get { return _status; }
        }

        public DevData(String devid,String status)
        {
            _devid = devid;
            _status = status;
        }
    }
}
