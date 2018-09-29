using Gfx.GfxDataManagerServer;
using GfxServiceContractClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using GfxCommonInterfaces;

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
        List<DispatchBackMember> _dispatchList = new List<DispatchBackMember>();

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

            List<DispatchBackMember> dispatchList = null;

            while (true)
            {
                Thread.Sleep(500);

                try
                {
                    getList = JTWcfHelper.WcfMainHelper.GetDevList();

                    dispatchList = JTWcfHelper.WcfMainHelper.GetDispatchList();

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
                SensorBackImf sens = dev.SensorList.Find(c => { return c.SenId == string.Format("{0}{1}", devId, sensNum); });

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

                return _dispatchList.Find(c => { return c.DisDevId == devid && c.OrderStatue != ResultTypeEnum.Suc; }) != null;
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
                DeviceBackImf dev = _devList.Find(c => { return c.DevType == "AGV" && c.SensorList[ConstSetBA.地标].RValue == site; });

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
        public List<F_AGV> IGetDevNotOnWaitSite()
        {
            try
            {
                List<DeviceBackImf> devs = _devList.FindAll(c =>
                {
                    return c.DevType == "AGV" && c.SensorList[ConstSetBA.地标].RValue != ConstSetBA.窑头卸载等待区 && c.SensorList[ConstSetBA.货物状态].RValue == ConstSetBA.AGV有货;
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
        public List<F_AGV> IGetDevNotLoadOnWaitSite()
        {
            try
            {
                List<DeviceBackImf> devs = _devList.FindAll(c => { return c.DevType == "AGV" && c.SensorList[ConstSetBA.地标].RValue != ConstSetBA.窑尾装载等待区 && c.SensorList[ConstSetBA.货物状态].RValue == ConstSetBA.AGV无货; });

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
                List<DeviceBackImf> devs = _devList.FindAll(c =>{return c.DevType == "AGV" &&
                (c.DevStatue == "False" || c.SensorList[ErrorType.脱轨].RValue == "1" || c.SensorList[ErrorType.急停触发].RValue == "1" ||
                 c.SensorList[ErrorType.驱动器故障].RValue == "1" || c.SensorList[ErrorType.轨道错误].RValue == "1");
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
    }

}
