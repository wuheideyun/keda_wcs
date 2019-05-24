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
using LogHelper;

namespace KEDAClient
{
    /// <summary>
    /// 业务逻辑
    /// </summary>
    public class F_Logic
    {
        /// <summary>
        /// 窑头PLC
        /// </summary>
        F_PLCLine _plcHead = new F_PLCLine("PLC0000001");

        /// <summary>
        /// 窑头PLC机械手
        /// </summary>
        F_PLCLine _plcHeadHolder = new F_PLCLine("PLC0000002");
        
        /// <summary>
        /// 窑尾PLC
        /// </summary>
        F_PLCLine _plcEnd = new F_PLCLine("PLC0000003");


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
        /// 窑头卸载区AGV是否需要充电
        /// </summary>
        public bool _PlcHeadNeedCharge = false;

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

            //Thread tr = new Thread(InitToHeadWait);
            //tr.IsBackground = true;
            //tr.Start();

            //Thread tr2 = new Thread(InitToEnd);
            //tr2.IsBackground = true;
            //tr2.Start();

            // 发生故障、离线的车，清除其相应的任务
            Thread tr3 = new Thread(ClearTask);
            tr3.IsBackground = true;
            tr3.Start();

            //检查线边辊台PLC连接状态
            Thread tr4 = new Thread(CheckPlcStatus);
            tr4.IsBackground = true;
            tr4.Start();


            _thread = new Thread(ThreadFunc);
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
        /// 
        /// </summary>
        private void ThreadFunc()
        {
            while (true)
            {
                Thread.Sleep(3000);

                try
                {
                    //PlcCharge();            //窑头4去充电

                    //PlcChargeSuc();         //充电完成去到窑尾1

                    //HeadHolder();           //窑头启动辊台

                    EndHolder();            //窑尾启动辊台

                    EndHolder1();           //窑尾机械手1操作

                    EndToEndWait();         // 从窑尾1 去 窑尾等待5

                    EndWaitToHolder2();     // 从窑尾等待5 去 窑尾夹具点2                                     

                    EndHolderToHeadWait();  // 窑尾机械手2 到 窑头等待点7

                    HeadWaitToHolder();     //  从窑头等待7 去 窑头夹具点3

                    HolderToHeadWait();     // 从窑头机械手3 去 窑头等待8

                    TaskPlcHeadPut();       // 窑头8到4，放货任务

                    TaskHeadToEnd();        // 窑头卸货完成Agv从窑头4 到 窑尾1


                }
                catch { }
            }
        }

        /// <summary>
        /// 窑尾1 去 窑尾等待5   
        /// </summary>
        public void EndToEndWait()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(Site.窑尾1);

            // 判断窑尾1 号机械手是否完成 
            if (agv != null && agv.IsFree && !agv.IsLock
                && agv.Sta_Material == EnumSta_AGV.AGV有货
                && _plcEnd.Sta_Material_End == EnumSta_Material_End.窑尾1号机械手完成

                )
            {
                // 从窑尾1 去 窑尾等待5
                F_ExcTask task = new F_ExcTask(_plcEnd, EnumOper.无动作, Site.窑尾1, Site.窑尾5);

                task.Id = agv.Id;

                agv.IsLock = true;

                F_DataCenter.MTask.IStartTask(task);

                sendServerLog(agv.Id + ", 从窑尾1 去 窑尾等待5");

                LogFactory.LogDispatch(agv.Id, "取货完成", ", 从窑尾1 去 窑尾等待5");
            }
        }


        /// <summary>
        /// 窑尾等待5 去 窑尾机械手2   
        /// </summary>
        public void EndWaitToHolder2()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(Site.窑尾5);

            // 判断窑尾2号机械手的状态 
            if (agv != null && agv.IsFree&& !agv.IsLock
                && agv.Sta_Material == EnumSta_AGV.AGV有货
                )
            {
                // 从窑尾夹具点2 到 窑尾等待点5  
                F_ExcTask task = new F_ExcTask(_plcEnd, EnumOper.窑尾2号机械手, Site.窑尾5, Site.窑尾2);

                task.Id = agv.Id;

                agv.IsLock = true;

                F_DataCenter.MTask.IStartTask(task);

                sendServerLog(agv.Id + ",  从窑尾等待5  去 窑尾夹具点2");

                LogFactory.LogDispatch(agv.Id, "取货完成", "从窑尾等待5  去 窑尾夹具点2");
            }
        }


        /// <summary>
        /// 窑尾机械手2 到 窑头等待7
        /// </summary>
        private void EndHolderToHeadWait()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(Site.窑尾2);

            // 判断窑尾机械手2号是否完成
            if (agv != null && agv.IsFree && !agv.IsLock
                  && agv.Sta_Material == EnumSta_AGV.AGV有货
                  && _plcEnd.Sta_Material_End == EnumSta_Material_End.窑尾2号机械手完成
                  )
            {
                F_ExcTask task = new F_ExcTask(null, EnumOper.无动作, Site.窑尾2, Site.窑头7);

                task.Id = agv.Id;

                agv.IsLock = true;

                F_DataCenter.MTask.IStartTask(task);

                sendServerLog(agv.Id + "窑尾取货完成Agv从窑尾夹具2 到 窑头等待点7");

                LogFactory.LogDispatch(agv.Id, "送货", "窑尾取货完成Agv从窑尾夹具2 到 窑头等待点7");
            }
        }


        /// <summary>
        /// 窑头等待7 去 窑头机械手3
        /// </summary>
        public void HeadWaitToHolder()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(Site.窑头7);

            //窑头等待区7的车不需要充电、没有充电完成的车 、没有初始化时要去窑头装载点的车
            if (agv != null && agv.IsFree && !agv.IsLock
                 //&& !_ToPlcHead
                )
            {
                // 判断夹具的状态 及 窑尾货物状态、AGV货物状态
                if (true
                   && agv.Sta_Material == EnumSta_AGV.AGV有货
                   && _plcHeadHolder.Sta_Material_HeadMLP == EnumSta_Material_HeadMLP.窑头机械手就绪
                    )
                {
                    // 从窑头等待7 去 窑头夹具点3
                    F_ExcTask task = new F_ExcTask(_plcHeadHolder, EnumOper.窑头机械手, Site.窑头7, Site.窑头3);

                    task.Id = agv.Id;

                    agv.IsLock = true;

                    F_DataCenter.MTask.IStartTask(task);

                    sendServerLog(agv.Id + ",  从窑头等待7 去 窑头夹具点3");

                    LogFactory.LogDispatch(agv.Id, "卸货", "从窑头等待7去窑头夹具点3");
                }
            }
        }


        /// <summary>
        /// 窑头机械手3 去 窑头等待8 
        /// </summary>
        public void HolderToHeadWait()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(Site.窑头3);

            // 判断窑头机械手是否完成的状态 
            if (agv != null && agv.IsFree && !agv.IsLock
                && agv.Sta_Material == EnumSta_AGV.AGV有货
                && _plcHeadHolder.Sta_Material_HeadMLP == EnumSta_Material_HeadMLP.窑头机械手完成
                )
            {
                // 从窑头机械手3 到窑头8
                F_ExcTask task = new F_ExcTask(_plcHeadHolder, EnumOper.无动作, Site.窑头3, Site.窑头8);

                task.Id = agv.Id;

                agv.IsLock = true;

                F_DataCenter.MTask.IStartTask(task);

                sendServerLog(agv.Id + ", 从窑头夹具点3 去 窑头等待8 ");

                LogFactory.LogDispatch(agv.Id, "卸货", "从窑头夹具点3去窑头等待8");
            }
        }


        /// <summary>
        /// 窑头等待8 去 窑头4
        /// </summary>
        private void TaskPlcHeadPut()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(Site.窑头8);

            /// 窑头AGV未锁定 并且 此次任务没有被响应
            if (agv != null && agv.IsFree && !agv.IsLock 
                                          //&& !_plcHead.IsLock
                && agv.Sta_Material == EnumSta_AGV.AGV有货
               )
            {
                // 从窑头卸载等待点2 到 窑头卸载点的放货任务
                F_ExcTask task = new F_ExcTask(_plcHead, EnumOper.放货, Site.窑头8, Site.窑头4);

                task.Id = agv.Id;

                _plcHead.IsLock = true;

                agv.IsLock = true;

                F_DataCenter.MTask.IStartTask(task);

                sendServerLog(agv.Id + "从窑头卸载等待点8 到 窑头卸载点4的任务");

                LogFactory.LogDispatch(agv.Id, "窑头卸货", "从窑头卸载等待点8 到 窑头卸载点4的任务");
            }
            else
            {
                _ToPlcHead = false;
            }
        }


        /// <summary>
        /// 窑头4 到 窑尾1
        /// </summary>
        private void TaskHeadToEnd()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(Site.窑头4);

            if (agv != null && agv.IsFree && !agv.IsLock
                 && agv.Sta_Material == EnumSta_AGV.AGV无货
                && _plcHead.Sta_Material_Head == EnumSta_Material_Head.窑头接料完成
                 //&& agv.Electicity > ConstSetBA.最低电量
                 )
            {
                F_ExcTask task = new F_ExcTask(_plcEnd, EnumOper.取货, Site.窑头4, Site.窑尾1);

                task.Id = agv.Id;

                agv.IsLock = true;

                _plcEnd.IsLock = true;

                F_DataCenter.MTask.IStartTask(task);

                sendServerLog(agv.Id + "从窑头卸载点4 到 窑尾装载点1");

                LogFactory.LogDispatch(agv.Id, "到窑尾接货", "从窑头卸载点4到窑尾装载点1");

            }
        }


        /// <summary>
        /// 窑头4去充电
        /// </summary>
        private void PlcCharge()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(Site.窑头4);

            if (agv != null && agv.IsFree&&!agv.IsLock
                && agv.Sta_Material == EnumSta_AGV.AGV无货
                && _plcHead.Sta_Material_Head == EnumSta_Material_Head.窑头接料完成
                && agv.Electicity <= ConstSetBA.最低电量
                && agv.ChargeStatus == EnumChargeStatus.未充电)
            {
                _PlcHeadNeedCharge = true;

                F_ExcTask task = new F_ExcTask(_plcHead, EnumOper.充电, Site.窑头4, Site.充电点);

                agv.IsLock = true;

                task.Id = agv.Id;

                F_DataCenter.MTask.IStartTask(task);

                sendServerLog(agv.Id + ",去到充电点充电");

                LogFactory.LogDispatch(agv.Id, "充电", "去到充电点充电");

            }
            else
            {
                _PlcHeadNeedCharge = false;
            }
        }


        /// <summary>
        ///充电完成的AGV去窑尾1
        /// </summary>
        public void PlcChargeSuc()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(Site.充电点);

            // 有未上锁的、充电完成的AGV,且窑头卸载点无货、AGV上有货
            if (agv != null && agv.IsFree && !agv.IsLock
                && agv.ChargeStatus == EnumChargeStatus.充电完成
                && agv.Sta_Material == EnumSta_AGV.AGV无货
                )
            {

                //return;
                _PlcHeadChargeSuc = true;

                F_ExcTask task = new F_ExcTask(_plcEnd, EnumOper.取货, Site.充电点, Site.窑尾1);

                agv.IsLock = true;

                _plcEnd.IsLock = true;

                task.Id = agv.Id;

                F_DataCenter.MTask.IStartTask(task);

                sendServerLog(agv.Id + ",充电完成，派充电完成的车去窑尾1");

                LogFactory.LogDispatch(agv.Id, "充电完成", "派充电完成的车去窑尾1");

            }
            else
            {
                _PlcHeadChargeSuc = false;
            }
        }


        /// <summary>
        /// 窑尾机械手1操作
        /// </summary>
        private void EndHolder1()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(Site.窑尾1);

            // AGV已经取货完成，
            if (agv != null && agv.IsFree && !agv.IsLock 
                && !_plcEnd.IsLock
                && agv.Sta_Material == EnumSta_AGV.AGV有货
                &&
                !(_plcEnd.Sta_Material_End == EnumSta_Material_End.窑尾1号机械手完成)
                && true
                )
            {
                F_ExcTask task = new F_ExcTask(_plcEnd, EnumOper.窑尾1号机械手, Site.窑尾1, Site.窑尾1);

                agv.IsLock = true;

                task.Id = agv.Id;

                _plcEnd.IsLock = true;

                F_DataCenter.MTask.IStartTask(task);

                sendServerLog(agv.Id + "窑尾1号机械手启动");

                LogFactory.LogDispatch(agv.Id, "取货完成", " 窑尾1号机械手启动");

            }
        }

        /// <summary>
        /// 窑头启动辊台
        /// </summary>
        private void HeadHolder()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(Site.窑头4);


            if (agv != null && agv.IsFree && !agv.IsLock 
                && agv.Sta_Material == EnumSta_AGV.AGV有货
                &&agv.Sta_Monitor== EnumSta_AGVMonitor.AGV电机停止
                && true
                )
            {

                F_ExcTask task = new F_ExcTask(_plcHead, EnumOper.放货, Site.窑头4, Site.窑头4);

                agv.IsLock = true;

                _plcHead.IsLock = true;

                task.Id = agv.Id;

                F_DataCenter.MTask.IStartTask(task);

                sendServerLog(agv.Id + "窑头辊台启动卸货");

                LogFactory.LogDispatch(agv.Id, "窑头卸货", " 窑头辊台启动");

            }
        }

        /// <summary>
        /// 窑尾启动辊台
        /// </summary>
        private void EndHolder()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(Site.窑尾1);


            if (agv != null && agv.IsFree && !agv.IsLock 
                && agv.Sta_Material == EnumSta_AGV.AGV无货
                &&agv.Sta_Monitor == EnumSta_AGVMonitor.AGV电机停止
                && (_plcEnd.Sta_Material_End == EnumSta_Material_End.窑尾有货
                || _plcEnd.Sta_Material_End == EnumSta_Material_End.窑尾传输中)
                && true
                )
            {

                F_ExcTask task = new F_ExcTask(_plcEnd, EnumOper.取货, Site.窑尾1, Site.窑尾1);

                agv.IsLock = true;

                _plcEnd.IsLock = true;

                task.Id = agv.Id;

                F_DataCenter.MTask.IStartTask(task);

                sendServerLog(agv.Id + "窑尾辊台启动取货");

                LogFactory.LogDispatch(agv.Id, "窑尾取货", " 窑尾辊台启动");

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

            Thread tr2 = new Thread(InitToEnd);
            tr2.IsBackground = true;
            tr2.Start();
        }

        /// <summary>
        /// 如果agv有货 回到卸载等待点7 ，上电后处于等待点8与卸载点4之间的车去到卸载点4
        /// </summary>
        private void InitToHeadWait()
        {
            Thread.Sleep(1000);

            List<F_AGV> agvs = F_DataCenter.MDev.IGetDevNotOnWaitSite();

            if (agvs != null)
            {
                foreach (F_AGV agv in agvs)
                {
                    if (agv.Site != Site.窑头8和4之间 && agv.Site != Site.窑头4)
                    {
                        F_ExcTask task = new F_ExcTask(null, EnumOper.无动作, agv.Site, Site.窑头7);

                        task.Id = agv.Id;

                        F_DataCenter.MTask.IStartTask(task);

                        sendServerLog(agv.Id + " 初始化,回到窑头卸载等待点7");

                        LogFactory.LogDispatch(agv.Id, "车辆初始化", "回到窑头卸载等待点7");
                    }
                    else
                    {
                        /// 如果agv有货 且位于等待点8和 卸载点4之间，回到窑头卸载点
                        _ToPlcHead = true;

                        F_ExcTask task = new F_ExcTask(_plcHead, EnumOper.放货, agv.Site, Site.窑头4);

                        task.Id = agv.Id;

                        F_DataCenter.MTask.IStartTask(task);

                        sendServerLog(agv.Id + "位于等待点8和卸载点4之间的AGV去卸货");

                        LogFactory.LogDispatch(agv.Id, "车辆初始化", "位于等待点8和卸载点4之间的AGV去卸货");

                    }
                }

            }
        }

        /// <summary>
        /// 如果agv没货 回到装载点1
        /// </summary>
        private void InitToEnd()
        {
            Thread.Sleep(1000);

            List<F_AGV> agvs = F_DataCenter.MDev.IGetDevNotLoadOnWaitSite();

            if (agvs != null)
            {
                foreach (F_AGV agv in agvs)
                {
                    if (agv.IsFree)
                    {
                        _ToPlcEnd = true;

                        F_ExcTask task = new F_ExcTask(_plcEnd, EnumOper.取货, agv.Site, Site.窑尾1);

                        task.Id = agv.Id;

                        F_DataCenter.MTask.IStartTask(task);

                        sendServerLog(agv.Id + " 初始化,回到窑尾装载点1");

                        LogFactory.LogDispatch(agv.Id, "车辆初始化", "回到窑尾装载点1");

                    }
                }
            }
        }



        /// <summary>
        /// 检查线边辊台PLC连接状态
        /// </summary>
        public void CheckPlcStatus()
        {
            while (true)
            {
                Thread.Sleep(5000);

                List<DeviceBackImf> devsList = JTWcfHelper.WcfMainHelper.GetDevList();
                if (devsList != null && devsList.Count > 0)
                {
                    foreach (var dev in devsList)
                    {
                        if (dev.DevType == "WK_PLC" && !dev.IsAlive)
                        {
                            F_PLCLine plc = new F_PLCLine(dev.DevId);

                            plc.SendOrdr(0, 0);
                        }
                    }
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
                List<DispatchBackMember> dispatchList = JTWcfHelper.WcfMainHelper.GetDispatchList();
                if (agvs != null && dispatchList != null && dispatchList.Count > 0)
                {
                    foreach (var agv in agvs)
                    {
                        foreach (var dispatch in dispatchList)
                        {
                            // 有故障的车是否对应任务的设备ID
                            if (agv.Id == dispatch.DisDevId)
                            {
                                if (dic.ContainsKey(agv.Id))
                                {
                                    int count = 0;
                                    dic.TryGetValue(agv.Id, out count);
                                    if (count >= 5)
                                    {
                                        // 终止该任务
                                        JTWcfHelper.WcfMainHelper.CtrDispatch(dispatch.DisGuid, DisOrderCtrTypeEnum.Stop);
                                        F_DataCenter.MTask.IDeletTask(dispatch.DisGuid );
                                        sendServerLog("终止异常的 " + agv.Id + "正在执行的任务");

                                        LogFactory.LogRunning("终止异常的 " + agv.Id + "正在执行的任务");

                                        count = 0;
                                    }
                                    else
                                    {
                                        count++;

                                        sendServerLog("异常的 " + agv.Id + "已等待处理 " + count + " 次");

                                        LogFactory.LogRunning("异常的 " + agv.Id + "已等待处理 " + count + " 次");
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
