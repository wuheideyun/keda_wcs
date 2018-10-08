using Gfx.GfxDataManagerServer;
using GfxCommonInterfaces;
using GfxServiceContractClient;
using GfxServiceContractTaskExcute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace KEDAClient
{
    /// <summary>
    /// 业务逻辑
    /// </summary>
    public class F_Logic
    {
        /// <summary>
        /// 窑尾PLC
        /// </summary>
        F_PLCLine _plcEnd = new F_PLCLine("PLC0000001");

        /// <summary>
        /// 窑头PLC
        /// </summary>
        F_PLCLine _plcHead = new F_PLCLine("PLC0000001");

        /// <summary>
        /// 事务处理线程
        /// </summary>
        Thread _thread = null;

        private SynchronizationContext mainThreadSynContext;

        ListBox listBox;

        /// <summary>
        /// 初始启动系统的时候，是否有在等待点和卸载点之间的车要回窑头卸载点
        /// </summary>
        public bool _ToPlcHead = false;

        /// <summary>
        /// 初始启动系统的时候，是否有在等待点和装载点之间的车要回窑尾装载点
        /// </summary>
        public bool _ToPlcEnd = false;

        /// <summary>
        /// 窑尾等待区AGV是否需要充电
        /// </summary>
        public bool _PlcEndNeedCharge = false;

        /// <summary>
        /// 窑头卸载区AGV是否需要充电
        /// </summary>
        public bool _PlcHeadNeedCharge = false;

        /// <summary>
        /// 窑尾有无充电完成的AGV
        /// </summary>
        public bool _PlcEndChargeSuc = false;

        /// <summary>
        /// 窑头有无充电完成的AGV
        /// </summary>
        public bool _PlcHeadChargeSuc = false;

        /// <summary>
        /// 异常AGV
        /// </summary>
        Dictionary<string, int> dic = new Dictionary<string, int>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public F_Logic(SynchronizationContext context, ListBox listBoxOutput)
        {
            mainThreadSynContext = context;

            listBox = listBoxOutput;

            _plcHead.Site = Site.窑头4;

            _plcEnd.Site = Site.窑尾1;

            _thread = new Thread(ThreadFunc);

            _thread.IsBackground = true;

            _thread.Start();

            Thread tr = new Thread(InitToHeadWait);
            tr.IsBackground = true;
            tr.Start();

            Thread tr2 = new Thread(InitToEndWait);
            tr2.IsBackground = true;
            tr2.Start();

            Thread tr3 = new Thread(ClearTask);
            tr3.IsBackground = true;
            tr3.Start();

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
        /// 
        /// </summary>
        private void ThreadFunc()
        {
            while (true)
            {
                Thread.Sleep(3000);

                try
                {
                    EndWaitToHolder(); // 从窑尾等待6 去 窑尾夹具点2

                    HolderToEndWait(); // 从窑尾夹具点2  去 窑尾等待5 

                    TaskPlcEndGet();// 窑尾5到1，取货任务

                    TaskEndToHeadWait();// 窑尾取货完成Agv从窑尾装载点1到窑头卸载等待点7

                    PlcHeadCharge();// 窑头等待点7的AGV去充电点50

                    PlcHeadChargeSuc();//窑尾充电点50有充电完成的AGV,优先派充电完成的车去装载等待点7

                    HeadWaitToHolder(); //  从窑头等待7 去 窑头夹具点3

                    HolderToHeadWait(); // 从窑头夹具点3 去 窑头等待8

                    TaskPlcHeadPut();// 窑头8到4，放货任务

                    TaskEndToEndWait();// 窑头卸货完成Agv从窑头卸载点4到窑尾装载等待点6

                }
                catch { }
            }
        }

        /// <summary>
        /// 窑尾取货任务
        /// </summary>
        private void TaskPlcEndGet()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(Site.窑尾5);

            ///窑尾等待点5的AGV没有锁定 并且 此次任务没有被响应
            if (!_plcEnd.IsLock && agv != null && !agv.IsLock)
            {
                ///派发一个从窑尾装载等待点2 到 窑尾装载点取货的任务
                if (F_DataCenter.MTask.IStartTask(new F_ExcTask(_plcEnd, EnumOper.取货, Site.窑尾5, _plcEnd.Site)))
                {
                    _plcEnd.IsLock = true;

                    sendServerLog(agv.Id + "从窑尾装载等待点5 到 窑尾装载点1取货");
                }
            }
            else
            {
                _ToPlcEnd = false;
            }
        }

        /// <summary>
        /// 窑尾取货完成Agv从窑尾装载点1到窑头卸载等待点7
        /// </summary>
        private void TaskEndToHeadWait()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(_plcEnd.Site);

            if (agv != null && agv.IsFree && !agv.IsLock
                && agv.Sta_Material == EnumSta_Material.有货
                )
            {
                F_ExcTask task = new F_ExcTask(null, EnumOper.无动作, Site.窑尾1, Site.窑头7);

                task.Id = agv.Id;

                agv.IsLock = true;

                F_DataCenter.MTask.IStartTask(task);

                sendServerLog(agv.Id + "窑尾取货完成Agv从窑尾装载点1到窑头卸载等待点7");

            }
        }

        /// <summary>
        /// 窑头放货任务
        /// </summary>
        private void TaskPlcHeadPut()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(Site.窑头8);

            /// 窑头AGV未锁定 并且 此次任务没有被响应
            if (!_plcHead.IsLock && agv != null && !agv.IsLock)
            {
                // 从窑头卸载等待点2 到 窑头卸载点的放货任务
                if (F_DataCenter.MTask.IStartTask(new F_ExcTask(_plcHead, EnumOper.放货, Site.窑头8, Site.窑头4)))
                {
                    _plcHead.IsLock = true;

                    sendServerLog(agv.Id + "从窑头卸载等待点8 到 窑头卸载点4的任务");
                }
            }
            else
            {
                _ToPlcHead = false;
            }
        }

        /// <summary>
        /// 窑头卸货完成Agv从窑头卸载点4到窑尾装载等待6
        /// </summary>
        private void TaskEndToEndWait()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(_plcHead.Site);

            if (agv != null && agv.IsFree
                //&& agv.Sta_Material == EnumSta_Material.无货
                )
            {
                F_ExcTask task = new F_ExcTask(null, EnumOper.无动作, Site.窑头4, Site.窑尾6);

                task.Id = agv.Id;

                F_DataCenter.MTask.IStartTask(task);

                sendServerLog(agv.Id + "从窑头卸载点4到窑尾装载等待点6");

            }
        }


        /// <summary>
        /// 初始化按钮方法
        /// </summary>
        public void initButton()
        {
            Thread tr = new Thread(InitToHeadWait);
            tr.IsBackground = true;
            tr.Start();

            Thread tr2 = new Thread(InitToEndWait);
            tr2.IsBackground = true;
            tr2.Start();
        }

            /// <summary>
            /// 如果agv有货 回到卸载等待点7 ，上电后处于等待点8与卸载点4之间的车去到卸载点4
            /// </summary>
            private  void InitToHeadWait()
        {
            Thread.Sleep(3000);

            List<F_AGV> agvs = F_DataCenter.MDev.IGetDevNotOnWaitSite();

            if (agvs != null)
            {
                foreach (F_AGV agv in agvs)
                {
                    if (agv.Site != Site.窑头8和4之间)
                    {
                        F_ExcTask task = new F_ExcTask(null, EnumOper.无动作, agv.Site, Site.窑头7);

                        task.Id = agv.Id;

                        F_DataCenter.MTask.IStartTask(task);

                        sendServerLog(agv.Id + ",回到窑头卸载等待点7");

                    }
                    else
                    {
                        /// 如果agv有货 且位于等待点8和 卸载点4之间，回到窑头卸载点
                        _ToPlcHead = true;

                        F_ExcTask task = new F_ExcTask(_plcHead, EnumOper.放货, Site.窑头8和4之间, Site.窑头4);

                        task.Id = agv.Id;

                        F_DataCenter.MTask.IStartTask(task);

                        sendServerLog(agv.Id + "位于等待点8和卸载点4之间的AGV去卸货");
                    }
                }

            }
        }

        /// <summary>
        /// 如果agv没货 回到装载等待点6，或者处于窑尾等待点5和装载点1之间的车去到装载点1
        /// </summary>
        private void InitToEndWait()
        {
            Thread.Sleep(3000);

            List<F_AGV> agvs = F_DataCenter.MDev.IGetDevNotLoadOnWaitSite();

            if (agvs != null)
            {
                foreach (F_AGV agv in agvs)
                {
                    if (agv.Site != Site.窑尾5和1之间)
                    {
                        F_ExcTask task = new F_ExcTask(null, EnumOper.无动作, agv.Site, Site.窑尾6);

                        task.Id = agv.Id;

                        F_DataCenter.MTask.IStartTask(task);

                        sendServerLog(agv.Id + ",回到窑尾装载等待点6");
                    }
                    else
                    {
                        /// 如果agv无货 且位于等待点和装载点之间，去到窑尾装载点
                        _ToPlcEnd = true;

                        F_ExcTask task = new F_ExcTask(_plcHead, EnumOper.放货, Site.窑尾5和1之间, Site.窑尾1);

                        task.Id = agv.Id;

                        F_DataCenter.MTask.IStartTask(task);

                        sendServerLog(agv.Id + "位于等待点5和装载载点1之间的AGV去装货");
                    }
                }

            }
        }

        /// <summary>
        /// 窑头等待点7的AGV去充电
        /// </summary>
        private void PlcHeadCharge()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(Site.窑头7);
            F_AGV agv1 = F_DataCenter.MDev.IGetDevOnSite(Site.充电点);

            // 让未上锁的、电量低于60且未充电的AGV去充电，且充电点没有AGV
            if (agv != null && agv.IsFree && agv.Electicity <= ConstSetBA.最低电量 &&
                agv.ChargeStatus == EnumChargeStatus.未充电 && agv1 == null)
            {
                _PlcHeadNeedCharge = true;

                F_ExcTask task = new F_ExcTask(null, EnumOper.充电, Site.窑头7, Site.充电点);

                agv.IsLock = true;

                task.Id = agv.Id;

                F_DataCenter.MTask.IStartTask(task);

                sendServerLog(agv.Id + ",去到充电点充电");

            }
            else
            {
                _PlcHeadNeedCharge = false;

            }
        }

        /// <summary>
        ///窑头充电点有充电完成的AGV
        ///派充电完成的车去卸货
        /// </summary>
        public void PlcHeadChargeSuc()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(Site.充电点);

            // 有未上锁的、充电完成的AGV,且窑尾装载点有货、AGV上无货
            if (agv != null && !agv.IsLock && agv.ChargeStatus == EnumChargeStatus.充电完成)
            {
                if (// _plcEnd.Sta_Material == EnumSta_Material.无货 &&
                    //agv.Sta_Material == EnumSta_Material.有货
                    true)
                {
                    _PlcHeadChargeSuc = true;

                    F_ExcTask task = new F_ExcTask(_plcEnd, EnumOper.无动作, Site.充电点, Site.窑头7);

                    agv.IsLock = true;

                    _plcHead.IsLock = true;

                    task.Id = agv.Id;

                    F_DataCenter.MTask.IStartTask(task);

                    sendServerLog(agv.Id + ",充电完成，派充电完成的车去卸载等待点7");


                }
            }
            else
            {
                _PlcHeadChargeSuc = false;
            }
        }

        /// <summary>
        /// 从窑尾等待6 去 窑尾夹具点2
        /// </summary>
        public void EndWaitToHolder()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(Site.窑尾6);

            //窑尾等待区的车不为空、且没有初始化时要去窑尾装载点4的车
            if (agv != null && !_ToPlcEnd && !agv.IsLock)
            {
                // 判断夹具的状态 及 窑尾货物状态
                if (true
                    && _plcEnd.Sta_Material == EnumSta_Material.有货
                    )
                {
                    // 从窑尾等待点6 到 窑尾夹具点2
                    if (F_DataCenter.MTask.IStartTask(new F_ExcTask(_plcEnd, EnumOper.无动作, Site.窑尾6, Site.窑尾2)))
                    {
                        sendServerLog(agv.Id + ", 从窑尾等待6 去 窑尾夹具点2");
                        agv.IsLock = true;
                    }
                }
            }
        }

        /// <summary>
        /// 从窑尾夹具点2  去 窑尾等待5 
        /// </summary>
        public void HolderToEndWait()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(Site.窑尾2);

            // 判断夹具的状态 
            if (agv != null && !agv.IsLock)
            {
                // 从窑尾夹具点2 到 窑尾等待点5  
                if (F_DataCenter.MTask.IStartTask(new F_ExcTask(_plcEnd, EnumOper.无动作, Site.窑尾2, Site.窑尾5)))
                {
                    sendServerLog(agv.Id + ",  从窑尾夹具点2  去 窑尾等待5");
                    agv.IsLock = true;
                }
            }
        }

        /// <summary>
        /// 从窑头等待7 去 窑头夹具点3
        /// </summary>
        public void HeadWaitToHolder()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(Site.窑头7);

            //窑头等待区7的车不需要充电、没有充电完成的车 、没有初始化时要去窑头装载点的车
            if (agv != null && !agv.IsLock && !_PlcHeadNeedCharge && !_PlcHeadChargeSuc && !_ToPlcHead)
            {
                // 判断夹具的状态 及 窑尾货物状态、AGV货物状态
                if (true
                    //&& _plcEnd.Sta_Material == EnumSta_Material.无货 
                    && agv.Sta_Material == EnumSta_Material.有货
                    )
                {
                    // 从窑头等待7 去 窑头夹具点3
                    if (F_DataCenter.MTask.IStartTask(new F_ExcTask(_plcHead, EnumOper.无动作, Site.窑头7, Site.窑头3)))
                    {
                        sendServerLog(agv.Id + ",  从窑头等待7 去 窑头夹具点3");
                        agv.IsLock = true;
                    }
                }
            }
        }

        /// <summary>
        /// 从窑头夹具点3 去 窑头等待8 
        /// </summary>
        public void HolderToHeadWait()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(Site.窑头3);

            // 判断夹具的状态 
            if (agv != null && !agv.IsLock && true)
            {
                // 从窑尾夹具点2 到 窑尾等待点5  
                if (F_DataCenter.MTask.IStartTask(new F_ExcTask(_plcHead, EnumOper.无动作, Site.窑头3, Site.窑头8)))
                {
                    sendServerLog(agv.Id + ", 从窑头夹具点3 去 窑头等待8 ");
                    agv.IsLock = true;
                }
            }
        }


        /// <summary>
        /// 发生故障、离线的车，清除其相应的任务
        /// </summary>
        public void ClearTask()
        {
            while (true)
            {
                Thread.Sleep(5000);
                List<F_AGV> agvs = F_DataCenter.MDev.ErrorOrFalse();
                List<DispatchBackMember> dispatchlist = JTWcfHelper.WcfMainHelper.GetDispatchList();
                if (agvs != null && dispatchlist.Count > 0)
                {
                    foreach (var agv in agvs)
                    {
                        foreach (var dispatch in dispatchlist)
                        {
                            // 有故障的车是否对应任务的设备ID
                            if (agv.Id == dispatch.DisDevId)
                            {
                                if (dic.ContainsKey(agv.Id))
                                {
                                    int count = 0;
                                    dic.TryGetValue(agv.Id, out count);
                                    if (count >= 1)
                                    {
                                        // 终止该任务
                                        JTWcfHelper.WcfMainHelper.CtrDispatch(dispatch.DisGuid, DisOrderCtrTypeEnum.Stop);
                                        sendServerLog("终止异常的 " + agv.Id + "正在执行的任务");
                                        count = 0;
                                    }
                                    else
                                    {
                                        count++;
                                        sendServerLog("异常的 " + agv.Id + "已等待处理 " + count + " 次");
                                    }
                                    dic.Remove(agv.Id);
                                    dic.Add(agv.Id, count);
                                }
                                else
                                {
                                    dic.Add(agv.Id, 0);
                                }
                            }
                        }
                    }
                }
                else
                {
                    dic.Clear();
                }
            }
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
