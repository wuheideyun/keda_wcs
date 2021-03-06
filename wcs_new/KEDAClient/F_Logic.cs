﻿using Gfx.GfxDataManagerServer;
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
        /// 窑尾PLC
        /// </summary>
        F_PLCLine _plcEnd = new F_PLCLine("PLC0000001");

        /// <summary>
        /// 窑头PLC
        /// </summary>
        F_PLCLine _plcHead = new F_PLCLine("PLC0000002");

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

            _plcHead.Site = ConstSetBA.窑头卸载点;

            _plcEnd.Site = ConstSetBA.窑尾装载点;

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
                    PlcEndCharge();// 窑尾等待区的AGV去充电

                    PlcEndChargeSuc();//窑尾充电点有充电完成的AGV,优先派充电完成的车去接货

                    TaskPlcEndGet();// 窑尾取货任务

                    TaskEndToHeadWait();// 窑尾取货完成Agv从窑尾装载点到窑头卸载等待区

                    PlcHeadCharge();// 窑头卸载区的AGV去充电

                    PlcHeadChargeSuc();//窑头充电点有充电完成的AGV,优先派充电完成的车去卸货

                    TaskPlcHeadPut();// 窑头放货任务

                    TaskEndToEndWait();// 窑头卸货完成Agv从窑头卸载点到窑尾装载等待区

                }
                catch { }
            }
        }

        /// <summary>
        /// 窑尾取货任务
        /// </summary>
        private void TaskPlcEndGet()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(ConstSetBA.窑尾装载等待区);

            ///窑尾有货 窑尾等待点的AGV没有锁定 并且 此次任务没有被响应
            if (!_plcEnd.IsLock && agv != null && !F_AGV.IsLock(agv.Id)
                //&& _plcEnd.Sta_Material == EnumSta_Material.有货
                )
            {
                //窑尾等待区的车不需要充电、没有充电完成的车 、没有初始化时要去窑尾装载点的车
                if (!_PlcEndNeedCharge && !_PlcEndChargeSuc && !_ToPlcEnd)
                {
                    ///派发一个从窑尾装载等待区到窑尾装载点取货的任务
                    if (F_DataCenter.MTask.IStartTask(new F_ExcTask(_plcEnd, EnumOper.取货, ConstSetBA.窑尾装载等待区, _plcEnd.Site)))
                    {
                        _plcEnd.IsLock = true;

                        F_AGV.AgvLock(agv.Id);

                        sendServerLog(agv.Id + "从窑尾装载等待区到窑尾装载点取货");

                        LogFactory.LogDispatch(agv.Id, "到窑尾取货", "从窑尾装载等待区到窑尾装载点取货");

                    }
                }
                else
                {
                    _ToPlcEnd = false;
                }
            }
        }

        /// <summary>
        /// 窑尾取货完成Agv从窑尾装载点到窑头卸载等待区
        /// </summary>
        private void TaskEndToHeadWait()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(_plcEnd.Site);

            if (agv != null && agv.IsFree && !F_AGV.IsLock(agv.Id)
                //&& agv.Sta_Material == EnumSta_Material.有货
                )
            {
                F_ExcTask task = new F_ExcTask(null, EnumOper.无动作, ConstSetBA.窑尾装载点, ConstSetBA.窑头卸载等待区);

                F_AGV.AgvLock(agv.Id);

                task.Id = agv.Id;

                F_DataCenter.MTask.IStartTask(task);

                sendServerLog(agv.Id + "窑尾取货完成Agv从窑尾装载点到窑头卸载等待区");

                LogFactory.LogDispatch(agv.Id, "AGV送货", "窑尾取货完成Agv从窑尾装载点到窑头卸载等待区");

            }
        }


        /// <summary>
        /// 窑头放货任务
        /// </summary>
        private void TaskPlcHeadPut()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(ConstSetBA.窑头卸载等待区);

            ///窑头无货 窑头AGV未锁定 并且 此次任务没有被响应
            if (!_plcHead.IsLock
                //&& _plcHead.Sta_Material == EnumSta_Material.无货 
                && agv != null && !F_AGV.IsLock(agv.Id))
            {
                //窑头等待区的车不需要充电、没有充电完成的车、没有回卸载点的车
                if (!_PlcHeadNeedCharge && !_PlcHeadChargeSuc && !_ToPlcHead)
                {
                    ///派发一个从窑头卸载等待区到窑头卸载点的任务
                    if (F_DataCenter.MTask.IStartTask(new F_ExcTask(_plcHead, EnumOper.放货, ConstSetBA.窑头卸载等待区, ConstSetBA.窑头卸载点)))
                    {
                        _plcHead.IsLock = true;

                        F_AGV.AgvLock(agv.Id);

                        sendServerLog(agv.Id + "从窑头卸载等待区到窑头卸载点的任务");

                        LogFactory.LogDispatch(agv.Id, "卸货", "从窑头卸载等待区到窑头卸载点的任务");

                    }
                }
                else
                {
                    _ToPlcHead = false;
                }
            }
        }

        /// <summary>
        /// 窑头卸货完成Agv从窑头卸载点到窑尾装载等待区
        /// </summary>
        private void TaskEndToEndWait()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(_plcHead.Site);

            if (agv != null && agv.IsFree && !F_AGV.IsLock(agv.Id)
                //&& agv.Sta_Material == EnumSta_Material.无货
                )
            {
                F_ExcTask task = new F_ExcTask(null, EnumOper.无动作, ConstSetBA.窑头卸载点, ConstSetBA.窑尾装载等待区);

                F_AGV.AgvLock(agv.Id);

                task.Id = agv.Id;

                F_DataCenter.MTask.IStartTask(task);

                sendServerLog(agv.Id + "从窑头卸载点到窑尾装载等待区");

                LogFactory.LogDispatch(agv.Id, "接货", "从窑头卸载点到窑尾装载等待区");

            }
        }

        /// <summary>
        /// 如果agv有货 回到卸载等待区 ，或者回到卸载点
        /// </summary>
        private void InitToHeadWait()
        {
            Thread.Sleep(5000);
            List<F_AGV> agvs = F_DataCenter.MDev.IGetDevNotOnWaitSite();

            if (agvs != null)
            {
                foreach (F_AGV agv in agvs)
                {
                    if (//agv.Site != ConstSetBA.窑头等待点和卸载点之间 && agv.Site != ConstSetBA.窑头卸载点 &&
                        agv.Site == ConstSetBA.窑头等待点和卸载点之间)
                    {
                        F_ExcTask task = new F_ExcTask(null, EnumOper.无动作, agv.Site, ConstSetBA.窑头卸载等待区);

                        task.Id = agv.Id;

                        F_DataCenter.MTask.IStartTask(task);

                        sendServerLog(agv.Id + ",初始化，回到窑头卸载等待区");

                        LogFactory.LogDispatch(agv.Id, "车辆初始化", "回到窑头卸载等待区");

                    }
                    //货物状态异常，暂时屏蔽
                    //else
                    //{
                    //    /// 如果agv有货 且位于等待点和装载点之间，回到窑头卸载点
                    //    _ToPlcHead = true;

                    //    F_ExcTask task = new F_ExcTask(_plcHead, EnumOper.放货, agv.Site, ConstSetBA.窑头卸载点);

                    //    task.Id = agv.Id;

                    //    F_DataCenter.MTask.IStartTask(task);

                    //    sendServerLog(agv.Id + "初始化，位于等待点和卸载点之间的AGV去卸货");

                    //    LogFactory.LogDispatch(agv.Id, "车辆初始化", "位于等待点和卸载点之间的AGV去卸货");

                    //}
                }

            }
        }

        /// <summary>
        /// 如果agv没货 回到装载等待区，或者处于窑尾等待点和装载点之间的车去到装载点
        /// </summary>
        private void InitToEndWait()
        {
            Thread.Sleep(5000);
            List<F_AGV> agvs = F_DataCenter.MDev.IGetDevNotLoadOnWaitSite();

            if (agvs != null)
            {
                foreach (F_AGV agv in agvs)
                {
                    if (//agv.Site != ConstSetBA.窑尾等待点和装载点之间 && agv.Site != ConstSetBA.窑尾装载点
                        agv.Site == ConstSetBA.窑尾等待点和装载点之间)
                    {
                        F_ExcTask task = new F_ExcTask(null, EnumOper.无动作, agv.Site, ConstSetBA.窑尾装载等待区);

                        task.Id = agv.Id;

                        F_DataCenter.MTask.IStartTask(task);

                        sendServerLog(agv.Id + "初始化,回到窑尾装载等待区");

                        LogFactory.LogDispatch(agv.Id, "车辆初始化", "回到窑尾装载等待区");

                    }
                    //货物状态异常，暂时屏蔽
                    //else
                    //{
                    //    /// 如果agv无货 且位于等待点和装载点之间，去到窑尾装载点
                    //    _ToPlcEnd = true;

                    //    F_ExcTask task = new F_ExcTask(_plcEnd, EnumOper.取货, agv.Site, ConstSetBA.窑尾装载点);

                    //    task.Id = agv.Id;

                    //    F_DataCenter.MTask.IStartTask(task);

                    //    sendServerLog(agv.Id + "初始化，位于等待点和装载载点之间的AGV去装货");

                    //    LogFactory.LogDispatch(agv.Id, "车辆初始化", "位于等待点和装载载点之间的AGV去装货");

                    //}
                }

            }
        }

        /// <summary>
        /// 窑尾等待区的AGV去充电
        /// </summary>
        private void PlcEndCharge()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(ConstSetBA.窑尾装载等待区);
            F_AGV agv1 = F_DataCenter.MDev.IGetDevOnSite(ConstSetBA.接货充电点);
            // 让未上锁的、电量低于60且未充电的AGV去充电，且接货充电点没有AGV
            if (agv != null && agv.IsFree && !F_AGV.IsLock(agv.Id) && agv.Electicity <= ConstSetBA.最低电量 &&
                agv.ChargeStatus == EnumChargeStatus.未充电 && agv1 == null)
            {
                _PlcEndNeedCharge = true;

                F_ExcTask task = new F_ExcTask(null, EnumOper.充电, ConstSetBA.窑尾装载等待区, ConstSetBA.接货充电点);

                F_AGV.AgvLock(agv.Id);

                task.Id = agv.Id;

                F_DataCenter.MTask.IStartTask(task);

                sendServerLog(agv.Id + ",去到窑尾充电点充电");

                LogFactory.LogDispatch(agv.Id, "充电", "去到窑尾充电点充电");

            }
            else
            {
                _PlcEndNeedCharge = false;

            }
        }

        /// <summary>
        /// 窑头卸载区的AGV去充电
        /// </summary>
        private void PlcHeadCharge()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(ConstSetBA.窑头卸载等待区);
            F_AGV agv1 = F_DataCenter.MDev.IGetDevOnSite(ConstSetBA.卸货充电点);
            // 让电量低于60且未充电的AGV去充电
            if (agv != null && agv.Electicity <= ConstSetBA.最低电量 && !F_AGV.IsLock(agv.Id) &&
                //agv.ChargeStatus == EnumChargeStatus.未充电  &&
                agv1 == null)
            {
                _PlcHeadNeedCharge = true;

                F_ExcTask task = new F_ExcTask(null, EnumOper.充电, ConstSetBA.窑头卸载等待区, ConstSetBA.卸货充电点);

                F_AGV.AgvLock(agv.Id);

                task.Id = agv.Id;

                F_DataCenter.MTask.IStartTask(task);

                sendServerLog(agv.Id + ",去到窑头充电点充电");

                LogFactory.LogDispatch(agv.Id, "充电", "去到窑头充电点充电");

            }
            else
            {
                _PlcHeadNeedCharge = false;

            }
        }

        /// <summary>
        ///窑尾充电点有充电完成的AGV
        ///优先派充电完成的车去接货
        /// </summary>
        public void PlcEndChargeSuc()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(ConstSetBA.接货充电点);
            // 有未上锁的、充电完成的AGV,且窑尾装载点有货、AGV上无货
            if (agv != null && !F_AGV.IsLock(agv.Id) && agv.ChargeStatus == EnumChargeStatus.充电完成)
            {
                if (// _plcEnd.Sta_Material == EnumSta_Material.有货 &&
                    //agv.Sta_Material == EnumSta_Material.无货
                    true)
                {
                    _PlcEndChargeSuc = true;

                    F_ExcTask task = new F_ExcTask(_plcEnd, EnumOper.取货, ConstSetBA.接货充电点, ConstSetBA.窑尾装载点);

                    F_AGV.AgvLock(agv.Id);

                    _plcEnd.IsLock = true;

                    task.Id = agv.Id;

                    F_DataCenter.MTask.IStartTask(task);

                    sendServerLog(agv.Id + ",充电完成，派充电完成的车去接货");

                    LogFactory.LogDispatch(agv.Id, "充电完成", "派充电完成的车去接货");

                }
            }
            else
            {
                _PlcEndChargeSuc = false;
            }
        }

        /// <summary>
        ///窑头充电点有充电完成的AGV
        ///优先派充电完成的车去卸货
        /// </summary>
        public void PlcHeadChargeSuc()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(ConstSetBA.卸货充电点);
            // 有充电完成的AGV,且窑头卸载点没货
            if (agv != null &&
                //&& agv.ChargeStatus == EnumChargeStatus.充电完成 && 
                !F_AGV.IsLock(agv.Id))
            {
                _PlcHeadChargeSuc = true;

                if (//_plcHead.Sta_Material == EnumSta_Material.无货
                    //&&  agv.Sta_Material == EnumSta_Material.有货
                    true
                    )
                {
                    F_ExcTask task = new F_ExcTask(_plcHead, EnumOper.放货, ConstSetBA.卸货充电点, ConstSetBA.窑头卸载点);

                    F_AGV.AgvLock(agv.Id);

                    _plcHead.IsLock = true;

                    task.Id = agv.Id;

                    F_DataCenter.MTask.IStartTask(task);

                    sendServerLog(agv.Id + ",充电完成，派充电完成的车去卸货");

                    LogFactory.LogDispatch(agv.Id, "充电完成", "派充电完成的车去卸货");

                }
            }
            else
            {
                _PlcHeadChargeSuc = false;
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
                if (agvs != null && dispatchList.Count > 0)
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
                                    if (count >= 10)
                                    {
                                        // 终止该任务
                                        JTWcfHelper.WcfMainHelper.CtrDispatch(dispatch.DisGuid, DisOrderCtrTypeEnum.Stop);
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
