using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
//using LogHelper;
using FLCommonInterfaces;
using WcfHelper;
using System.Collections;
using DataContract;

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
        F_PLCLine _plcEnd = new F_PLCLine("PLC01");

        /// <summary>
        /// 窑头PLC
        /// </summary>
        F_PLCLine _plcHead = new F_PLCLine("PLC02");

        /// <summary>
        /// 事务处理线程
        /// </summary>
        Thread _thread = null;

        private SynchronizationContext mainThreadSynContext;

        ListBox listBox;

        /// <summary>
        /// 初始启动系统的时候，是否有在等待点和卸载点之间的车要回窑头卸载点
        /// </summary>
        private bool _ToPlcHead = false;

        /// <summary>
        /// 初始启动系统的时候，是否有在等待点和装载点之间的车要回窑尾装载点
        /// </summary>
        private bool _ToPlcEnd = false;

        /// <summary>
        /// 窑尾等待区AGV是否需要充电
        /// </summary>
        private bool _PlcEndNeedCharge = false;

        /// <summary>
        /// 窑头卸载区AGV是否需要充电
        /// </summary>
        private bool _PlcHeadNeedCharge = false;

        /// <summary>
        /// 出窑尾有无充电完成的AGV
        /// </summary>
        private bool _ExitPlcEndChargeSuc = false;

        /// <summary>
        /// 进窑尾有无充电完成的AGV
        /// </summary>
        private bool _EnterPlcEndChargeSuc = false;

        /// <summary>
        /// 出窑头有无充电完成的AGV
        /// </summary>
        private bool _ExitPlcHeadChargeSuc = false;

        /// <summary>
        /// 进窑头有无充电完成的AGV
        /// </summary>
        private bool _EnterPlcHeadChargeSuc = false;

        /// <summary>
        /// 异常AGV
        /// </summary>
        Dictionary<string, int> dic = new Dictionary<string, int>();

        /// <summary>
        /// 窑尾交通管制点的地标集合
        /// </summary>
        ArrayList PLCEndTrafficSite = new ArrayList();

        /// <summary>
        /// 窑头交通管制点的地标集合
        /// </summary>
        ArrayList PLCHeadTrafficSite = new ArrayList();

        /// <summary>
        /// 出窑尾完去到窑头等地标集合
        /// </summary>
        ArrayList PLCEndToHeadWaitSite = new ArrayList();

        /// <summary>
        /// 出窑头完去到窑尾等地标集合
        /// </summary>
        ArrayList PLCHeadToEndWaitSite = new ArrayList();


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


            // 发生故障、离线的车，清除其相应的任务
            Thread tr3 = new Thread(ClearTask);

            tr3.IsBackground = true;

            tr3.Start();

            //检查线边辊台PLC连接状态
            Thread tr4 = new Thread(CheckPlcStatus);

            tr4.IsBackground = true;

            tr4.Start();
        }

        /// <summary>
        /// 1.不传参数则初始化所有AGV
        /// 2.传Agvid则初始化指定AGV
        /// </summary>
        /// <param name="id"></param>
        public void InitAgv(String id = null)
        {
            agvid = id;
            Thread tr = new Thread(Init);
            tr.IsBackground = true;
            tr.Start();
        }

        private string agvid = null;

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            InitToAllAGV();
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
                    //是否自动生成任务
                    if (ParamControl.Is_AutoAddTask)
                    {
                        //充电逻辑
                        //if (ParamControl.Do_EnterHeadCharge) TaskHeadToEnterBattery();        // 窑头等 到 进窑头充

                        if (ParamControl.Do_EnterEndCharge) TaskEndToEnterBattery();         // 窑尾等 到 进窑尾充

                        if (ParamControl.Do_ExitHeadCharge) TaskHeadToExitBattery();        // 窑头放 到 出窑头充   

                        //if (ParamControl.Do_ExitEndCharge) TaskEndToExitBattery();          // 窑尾取 到 出窑尾充

                        //充电完成逻辑
                        //if (ParamControl.Do_EnterHeadChargeSucc) TaskEnterHeadChargeSuc();    // 进窑头充 到 窑头卸

                        if (ParamControl.Do_EnterEndChargeSucc) TaskEnterEndChargeSuc();     // 进窑尾充 到 窑尾取

                        if (ParamControl.Do_ExitHeadChargeSucc) TaskExitHeadChargeSuc();    //出窑头充 到 窑头完

                        //if (ParamControl.Do_ExitEndChargeSucc) TaskExitEndChargeSuc();      //出窑尾充 到 窑尾完



                        //正常取卸逻辑
                        if (ParamControl.Do_HeadUnload) TaskPlcHeadPut();        // 窑头等 到 窑头卸

                        if (ParamControl.Do_ToHeadSuc) TaskHeadToHeadSuc();     // 窑头卸 到 窑头完

                        if (ParamControl.Do_ToEndWait) TaskHeadSucToEndWait();  // 窑头完 到 窑尾等

                        if (ParamControl.Do_EndLoad) TaskPlcEndGet();         // 窑尾等 到 窑尾取

                        if (ParamControl.Do_ToEndSuc) TaskEndToEndSuc();       // 窑尾取 到 窑尾完

                        if (ParamControl.Do_ToHeadWait) TaskEndSucToHeadWait();  // 窑尾完 到 窑头等

                        if (ParamControl.Do_EndPlcLoad) TaskPlcEndStart();        //窑尾启动辊台

                        if (ParamControl.Do_HeadPlcUnload) TaskPlcHeadStart();      //窑头启动辊台

                    }
                }
                catch (Exception e)
                {

                    sendServerLog("出现异常：" + e.Message);

                }
            }
        }


        private string TaskPlcHeadPutMsg = "窑头等 到 窑头卸";
        /// <summary>
        /// 窑头等 到 窑头卸
        /// </summary>
        private void TaskPlcHeadPut()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(ConstSetBA.窑头卸载等待区);
            //查找是否存在目标站点是窑头的AGV
            F_AGV d_agv = F_DataCenter.MDev.IGetDevOnDestination(ConstSetBA.窑头卸载站);
            F_AGV d_agv2 = F_DataCenter.MDev.IGetAliveDevOnSite(ConstSetBA.窑头卸载点);


            ///窑头无货 窑头AGV未锁定 并且 此次任务没有被响应
            if (//ParamControl.Do_HeadPlcLock && 
                (d_agv == null && d_agv2 == null)// !_plcHead.IsLock
                && agv != null && agv.IsFree
                //&& agv.Electicity > F_DataCenter.MDev.IGetDevElectricity()
                )
            {
                //窑头等待区的车不需要充电、没有充电完成的车、没有回卸载点的车
                if (true
                    //!_EnterPlcHeadChargeSuc && !_ToPlcHead
                    )
                {

                    ///派发一个从窑头卸载等待区到窑头卸载点的任务

                    F_ExcTask task = new F_ExcTask(_plcHead, EnumOper.放货, ConstSetBA.窑头卸载等待区, ConstSetBA.窑头卸载点);

                    task.Id = agv.Id;

                    F_DataCenter.MTask.IStartTask(task, agv.Id + TaskPlcHeadPutMsg);

                    //去卸货，锁定窑头
                    _plcHead.IsLock = true;

                    ParamControl.Do_HeadPlcLock = false;

                    sendServerLog(agv.Id + TaskPlcHeadPutMsg);

                    //LogFactory.LogDispatch(agv.Id, "卸货", TaskPlcHeadPutMsg);
                    FLog.Log(agv.Id + "卸货" + TaskPlcHeadPutMsg);

                }
                else
                {
                    // _ToPlcHead = false;
                }
            }
        }

        private string TaskHeadToExitBatteryMsg = "窑头放 到 出窑头充电站";
        /// <summary>
        /// 窑头放 到 出窑头充电站
        /// </summary>
        private void TaskHeadToExitBattery()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(ConstSetBA.窑头卸载点);

            //搜索车辆电量列表
            List<int> list = F_DataCenter.MDev.IGetDevElectricity();

            //如果电量小于百分之80才进行充电判断
            if (list[0] <= 80)
            {
                //是否到达电池最低点的标志
                int BatteryLim = 0;

                if (list[0] <= 40)
                {
                    BatteryLim = 1;/*电量有低于40的车*/
                }
                if(agv.Electicity<=list[0])
                {
                    switch(BatteryLim)
                    {
                        case 0:
                            //本来的充电逻辑
                            {
                                //查找是否存在目标站点是出窑头充电站的AGV
                                F_AGV d_agv = F_DataCenter.MDev.IGetDevOnDestination(ConstSetBA.充电桩站);//目的站点
                                F_AGV d_agv2 = F_DataCenter.MDev.IGetAliveDevOnSite(ConstSetBA.出窑头充电点);//地标

                                //去出窑头充电站的车是否是窑头的车
                                if (d_agv != null && (d_agv.Site != "24" && d_agv.Site != "65" && d_agv.Site != "25" && d_agv.Site != "14"))
                                {
                                    d_agv = null;
                                }

                                if (d_agv != null)
                                {
                                    _plcHead.IsExitBatteryLock = true;
                                    ParamControl.Do_ExitHeadChargeLock = false;

                                    _plcHead.ExitChargeAgv = d_agv.Id;

                                    ParamControl.HeadChargeAGV = _plcHead.ExitChargeAgv;
                                    ParamControl.HeadChargeLock = _plcHead.IsExitBatteryLock;
                                }
                                else if (d_agv2 != null)
                                {
                                    _plcHead.IsExitBatteryLock = true;
                                    ParamControl.Do_ExitHeadChargeLock = false;

                                    _plcHead.ExitChargeAgv = d_agv2.Id;

                                    ParamControl.HeadChargeAGV = _plcHead.ExitChargeAgv;
                                    ParamControl.HeadChargeLock = _plcHead.IsExitBatteryLock;
                                }
                                if (agv != null
                                    && agv.IsFree
                                    //&& agv.Electicity < list[0]
                                    && agv.Sta_Material == EnumagvSta_Material.无货
                                    && agv.ChargeStatus == EnumChargeStatus.未充电
                                    )
                                {
                                    // 判断出窑头充电站是否被锁
                                    if ((!_plcHead.IsExitBatteryLock)
                                        && (d_agv == null && d_agv2 == null)
                                        )
                                    {
                                        F_ExcTask task = new F_ExcTask(_plcHead, EnumOper.充电, ConstSetBA.窑头卸载点, ConstSetBA.出窑头充电点);

                                        task.Id = agv.Id;

                                        //出窑头充电，锁定出窑头充电桩
                                        _plcHead.IsExitBatteryLock = true;

                                        ParamControl.Do_ExitHeadChargeLock = false;

                                        _plcHead.ExitChargeAgv = agv.Id;

                                        ParamControl.HeadChargeAGV = _plcHead.ExitChargeAgv;
                                        ParamControl.HeadChargeLock = _plcHead.IsExitBatteryLock;

                                        _plcHead.ExitFlag = false;

                                        F_DataCenter.MTask.IStartTask(task, agv.Id + TaskHeadToExitBatteryMsg);

                                        sendServerLog(agv.Id + TaskHeadToExitBatteryMsg);

                                        //LogFactory.LogDispatch(agv.Id, "AGV出窑头充电", TaskHeadToExitBatteryMsg);
                                        FLog.Log(agv.Id + TaskHeadToExitBatteryMsg);
                                    }
                                }
                            }
                            break;
                        case 1:
                            //本来的充电逻辑
                            {
                                //查找是否存在目标站点是出窑头充电站的AGV
                                F_AGV d_agv = F_DataCenter.MDev.IGetDevOnDestination(ConstSetBA.充电桩站);//目的站点
                                F_AGV d_agv2 = F_DataCenter.MDev.IGetAliveDevOnSite(ConstSetBA.出窑头充电点);//地标

                                //去出窑头充电站的车是否是窑头的车
                                if (d_agv != null && (d_agv.Site != "24" && d_agv.Site != "65" && d_agv.Site != "25" && d_agv.Site != "14"))
                                {
                                    d_agv = null;
                                }

                                if (d_agv != null)
                                {
                                    _plcHead.IsExitBatteryLock = true;
                                    ParamControl.Do_ExitHeadChargeLock = false;

                                    _plcHead.ExitChargeAgv = d_agv.Id;

                                    ParamControl.HeadChargeAGV = _plcHead.ExitChargeAgv;
                                    ParamControl.HeadChargeLock = _plcHead.IsExitBatteryLock;
                                }
                                else if (d_agv2 != null)
                                {
                                    _plcHead.IsExitBatteryLock = true;
                                    ParamControl.Do_ExitHeadChargeLock = false;

                                    _plcHead.ExitChargeAgv = d_agv2.Id;

                                    ParamControl.HeadChargeAGV = _plcHead.ExitChargeAgv;
                                    ParamControl.HeadChargeLock = _plcHead.IsExitBatteryLock;
                                }
                                if (agv != null
                                    && agv.IsFree
                                    //&& agv.Electicity < list[0]
                                    && agv.Sta_Material == EnumagvSta_Material.无货
                                    && agv.ChargeStatus == EnumChargeStatus.未充电
                                    )
                                {
                                    // 判断出窑头充电站是否被锁
                                    if ((!_plcHead.IsExitBatteryLock)
                                        && (d_agv == null && d_agv2 == null)
                                        )
                                    {
                                        F_ExcTask task = new F_ExcTask(_plcHead, EnumOper.充电, ConstSetBA.窑头卸载点, ConstSetBA.出窑头充电点);

                                        task.Id = agv.Id;

                                        //出窑头充电，锁定出窑头充电桩
                                        _plcHead.IsExitBatteryLock = true;

                                        ParamControl.Do_ExitHeadChargeLock = false;

                                        _plcHead.ExitChargeAgv = agv.Id;

                                        ParamControl.HeadChargeAGV = _plcHead.ExitChargeAgv;
                                        ParamControl.HeadChargeLock = _plcHead.IsExitBatteryLock;

                                        _plcHead.ExitFlag = false;

                                        F_DataCenter.MTask.IStartTask(task, agv.Id + TaskHeadToExitBatteryMsg);

                                        sendServerLog(agv.Id + TaskHeadToExitBatteryMsg);

                                        //LogFactory.LogDispatch(agv.Id, "AGV出窑头充电", TaskHeadToExitBatteryMsg);
                                        FLog.Log(agv.Id + TaskHeadToExitBatteryMsg);
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private string TaskHeadToHeadSucMsg = "窑头放 到 窑头对接完成点";
        /// <summary>
        /// 窑头去窑头对接完成点
        /// </summary>
        private void TaskHeadToHeadSuc()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(_plcHead.Site);

            //查找是否存在目标站点是窑头对接完成点的AGV
            F_AGV d_agv = F_DataCenter.MDev.IGetDevOnDestination(ConstSetBA.窑头对接完成站);

            //增加车辆电量列表
            List<int> list = F_DataCenter.MDev.IGetDevElectricity();

            // 如果需要充电但是充电桩有车、被锁，或者不需要充电直接去到对接完成点
            if (agv != null
                && agv.IsFree
                && agv.Sta_Material == EnumagvSta_Material.无货
                && (agv.Electicity > list[0]
                || (!ParamControl.Do_ExitHeadCharge || _plcHead.IsExitBatteryLock && (_plcHead.ExitChargeAgv != agv.Id)))
                )
            {
                // 判断是否可出
                if (d_agv == null
                    && _plcHead.ExitFlag
                    )
                {
                    // 从窑头到窑头对接完成点
                    F_ExcTask task1 = new F_ExcTask(_plcHead, EnumOper.对接完成, ConstSetBA.窑头卸载点, ConstSetBA.窑头对接完成点);

                    task1.Id = agv.Id;

                    _plcHead.ExitFlag = false;

                    F_DataCenter.MTask.IStartTask(task1, agv.Id + TaskHeadToHeadSucMsg);

                    sendServerLog(agv.Id + TaskHeadToHeadSucMsg);

                    //LogFactory.LogDispatch(agv.Id, "卸货完成", TaskHeadToHeadSucMsg);
                    FLog.Log(agv.Id + TaskHeadToHeadSucMsg);
                }
                //else
                //{
                //    _plcHead.ExitFlag = false;
                //}
            }
        }

        private string TaskHeadSucToEndWaitMsg = "窑头对接完成点 到 窑尾等";
        /// <summary>
        /// 窑头对接完成点到窑尾等待点
        /// </summary>
        private void TaskHeadSucToEndWait()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(ConstSetBA.窑头对接完成点);
            if (agv != null && agv.IsFree
                )
            {
                //如果窑头对接完成点出现一台车，则解锁窑头可出标志
                _plcHead.ExitFlag = true;

                //如果窑头对接完成点的车是出窑头充电桩出来的车，则解锁出窑头充电桩
                if (_plcHead.ExitChargeAgv == agv.Id)
                {
                    _plcHead.IsExitBatteryLock = false;

                    _plcHead.ExitChargeAgv = null;
                }

                F_ExcTask task1 = new F_ExcTask(null, EnumOper.无动作, ConstSetBA.窑头对接完成点, ConstSetBA.窑尾装载等待区);

                task1.Id = agv.Id;

                F_DataCenter.MTask.IStartTask(task1, agv.Id + TaskHeadSucToEndWaitMsg);

                sendServerLog(agv.Id + TaskHeadSucToEndWaitMsg);

                //LogFactory.LogDispatch(agv.Id, " ", TaskHeadSucToEndWaitMsg);
                FLog.Log(agv.Id + TaskHeadSucToEndWaitMsg);
            }

        }

        private string TaskPlcEndGetMsg = "窑尾等 到 窑尾取";
        /// <summary>
        /// 窑尾等 到 窑尾取
        /// </summary>
        private void TaskPlcEndGet()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(ConstSetBA.窑尾装载等待区);

            //查找是否存在目标站点是窑尾的AGV
            F_AGV d_agv = F_DataCenter.MDev.IGetDevOnDestination(ConstSetBA.窑尾装载站);
            F_AGV d_agv2 = F_DataCenter.MDev.IGetAliveDevOnSite(ConstSetBA.窑尾装载点);

            F_AGV d_agv3 = F_DataCenter.MDev.IGetAliveDevOnSite(ConstSetBA.进窑尾充电点);

            //增加车辆电量列表
            List<int> list = F_DataCenter.MDev.IGetDevElectricity();

            ///窑尾有货 窑尾等待点的AGV没有锁定 并且 此次任务没有被响应
            if (//(ParamControl.Do_EndPlcLock && !_plcEnd.IsLock) && 
                (d_agv == null
                //&& d_agv2 == null
                ) &&
                agv != null && agv.IsFree
                && (agv.Electicity > list[0]
                || (!ParamControl.Do_EnterEndCharge || _plcEnd.IsEnterBatteryLock && _plcEnd.EnterChargeAgv != agv.Id))
                //&& _plcEnd.IsLock == false
                && (d_agv3 == null || (d_agv3 != null && d_agv3.ChargeStatus != EnumChargeStatus.充电完成))
                )
            {
                //窑尾等待区的车不需要充电、没有充电完成的车 、没有初始化时要去窑尾装载点的车
                if (//!_PlcEndNeedCharge && !_EnterPlcEndChargeSuc && !_ToPlcEnd
                    true)
                {

                    ///派发一个从窑尾装载等待区到窑尾装载点取货的任务
                    F_ExcTask task = new F_ExcTask(_plcEnd, EnumOper.取货, ConstSetBA.窑尾装载等待区, _plcEnd.Site);

                    task.Id = agv.Id;

                    F_DataCenter.MTask.IStartTask(task, (agv.Id + TaskPlcEndGetMsg));

                    //去取货，锁定窑尾
                    _plcEnd.IsLock = true;

                    ParamControl.Do_EndPlcLock = false;

                    F_AGV.AgvLock(agv.Id);

                    sendServerLog(agv.Id + TaskPlcEndGetMsg);

                    //LogFactory.LogDispatch(agv.Id, "到窑尾取货", TaskPlcEndGetMsg);
                    FLog.Log(agv.Id + TaskPlcEndGetMsg);
                }
                else
                {
                    //_ToPlcEnd = false;
                }
            }
        }

        //private string TaskEndToExitBatteryMsg = "窑尾取 到 出窑尾充电站";
        ///// <summary>
        ///// 窑尾取 到 出窑尾充电站
        ///// </summary>
        //private void TaskEndToExitBattery()
        //{
        //    F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(_plcEnd.Site);
        //    F_AGV agv1 = F_DataCenter.MDev.IGetDevOnSite(ConstSetBA.出窑尾充电点);
        //    if (agv1 != null)
        //    {
        //        ParamControl.Do_ExitEndChargeLock = false;
        //    }
        //    if (agv != null && agv.IsFree
        //        && !F_AGV.IsLock(agv.Id)
        //        && (ParamControl.IgnoreAgvLoadTask || agv.Sta_Material == EnumagvSta_Material.有货))
        //    {
        //        // 判断窑尾可出站标志是否为True
        //        if (_plcEnd.ExitFlag)
        //        {
        //            // 判断窑尾接货完成的车是否需要充电,且出窑尾充电站没有车、未被锁定
        //            if (agv.Electicity <= F_DataCenter.MDev.IGetDevElectricity() && agv.ChargeStatus == EnumChargeStatus.未充电
        //                && agv1 == null
        //                //&& !_plcEnd.IsExitBatteryLock 
        //                && ParamControl.Do_ExitEndChargeLock)
        //            {
        //                F_ExcTask task = new F_ExcTask(_plcEnd, EnumOper.充电, ConstSetBA.窑尾装载点, ConstSetBA.出窑尾充电点);

        //                F_AGV.AgvLock(agv.Id);

        //                task.Id = agv.Id;

        //                //出窑尾充电，锁定出窑尾充电桩
        //                //_plcEnd.IsExitBatteryLock = true;
        //                ParamControl.Do_ExitEndChargeLock = false;

        //                _plcEnd.ExitChargeAgv = agv.Id;

        //                F_DataCenter.MTask.IStartTask(task, agv.Id + TaskEndToExitBatteryMsg);

        //                sendServerLog(agv.Id + TaskEndToExitBatteryMsg);

        //                //LogFactory.LogDispatch(agv.Id, "AGV出窑尾充电", TaskEndToExitBatteryMsg);
        //                FLog.Log(agv.Id + TaskEndToExitBatteryMsg);
        //            }
        //        }
        //    }
        //}

        private string TaskEndToEndSucMsg = "窑尾取 到 窑尾对接完成点";
        /// <summary>
        /// 窑尾到窑尾对接完成点
        /// </summary>
        private void TaskEndToEndSuc()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(_plcEnd.Site);
            F_AGV agv1 = F_DataCenter.MDev.IGetDevOnSite(ConstSetBA.出窑尾充电点);


            F_AGV d_agv = F_DataCenter.MDev.IGetDevOnDestination(ConstSetBA.窑尾对接完成点);

            if (agv != null && agv.IsFree
                //&& !F_AGV.IsLock(agv.Id)
                && (ParamControl.IgnoreAgvLoadTask || agv.Sta_Material == EnumagvSta_Material.有货)
               )
            {
                // 判断是否存在目标站点是对接完成点的agv
                if (//_plcEnd.ExitFlag
                    d_agv == null)
                {
                    // 如果需要充电但是充电桩有车、被锁，或者不需要充电直接去到对接完成点
                    if (//agv.Electicity <= F_DataCenter.MDev.IGetDevElectricity()  && 
                        //agv1 != null                         &&
                        //_plcEnd.IsExitBatteryLock && 
                        //ParamControl.Do_ExitEndChargeLock))
                        //|| agv.Electicity > F_DataCenter.MDev.IGetDevElectricity()
                        true
                        )
                    {
                        // 从窑尾到窑尾对接完成点
                        F_ExcTask task = new F_ExcTask(_plcEnd, EnumOper.对接完成, ConstSetBA.窑尾装载点, ConstSetBA.窑尾对接完成点);

                        F_AGV.AgvLock(agv.Id);

                        task.Id = agv.Id;

                        _plcEnd.ExitFlag = false;

                        F_DataCenter.MTask.IStartTask(task, agv.Id + TaskEndToEndSucMsg);

                        sendServerLog(agv.Id + TaskEndToEndSucMsg);

                        //LogFactory.LogDispatch(agv.Id, "取货完成", TaskEndToEndSucMsg);
                        FLog.Log(agv.Id + TaskEndToEndSucMsg);
                    }
                }
            }
        }

        private string TaskEndSucToHeadWaitMsg = "窑尾对接完成点 到 窑头等";
        /// <summary>
        /// 窑尾对接完成点到窑头等待点
        /// </summary>
        private void TaskEndSucToHeadWait()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(ConstSetBA.窑尾对接完成点);

            if (agv != null && agv.IsFree)
            {
                F_ExcTask task = new F_ExcTask(null, EnumOper.无动作, ConstSetBA.窑尾对接完成点, ConstSetBA.窑头卸载等待区);

                F_AGV.AgvLock(agv.Id);

                task.Id = agv.Id;

                F_DataCenter.MTask.IStartTask(task, agv.Id + TaskEndSucToHeadWaitMsg);

                sendServerLog(agv.Id + TaskEndSucToHeadWaitMsg);

                //LogFactory.LogDispatch(agv.Id, " ", TaskEndSucToHeadWaitMsg);
                FLog.Log(agv.Id + TaskEndSucToHeadWaitMsg);
            }
        }

        //private string ExitPlcEndChargeSucMsg = "出窑尾充 到 窑尾对接完成点";
        ///// <summary>
        ///// 出窑尾充 到 窑尾对接完成点
        ///// </summary>
        //public void TaskExitEndChargeSuc()
        //{
        //    F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(ConstSetBA.出窑尾充电点);
        //    // 有未上锁的、充电完成的AGV,且窑尾装载点有货、AGV上无货
        //    if (agv != null && agv.IsFree
        //        && !F_AGV.IsLock(agv.Id)
        //        && agv.ChargeStatus == EnumChargeStatus.充电完成)
        //    {
        //        if (_plcEnd.ExitFlag &&
        //            true)
        //        {
        //            _ExitPlcEndChargeSuc = true;

        //            F_ExcTask task = new F_ExcTask(null, EnumOper.对接完成, ConstSetBA.出窑尾充电点, ConstSetBA.窑尾对接完成点);

        //            F_AGV.AgvLock(agv.Id);

        //            _plcEnd.ExitFlag = false;

        //            task.Id = agv.Id;

        //            Thread.Sleep(2000);

        //            F_DataCenter.MTask.IStartTask(task, agv.Id + ExitPlcEndChargeSucMsg);

        //            sendServerLog(agv.Id + ExitPlcEndChargeSucMsg);

        //            //LogFactory.LogDispatch(agv.Id, "充电完成", ExitPlcEndChargeSucMsg);
        //            FLog.Log(agv.Id + ExitPlcEndChargeSucMsg);
        //        }
        //    }
        //    else
        //    {
        //        _ExitPlcEndChargeSuc = false;
        //    }
        //}

        private string ExitPlcHeadChargeSucMsg = "出窑头充 到 窑头对接完成点";
        /// <summary>
        /// 出窑头充 到 窑头对接完成点
        /// </summary>
        public void TaskExitHeadChargeSuc()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(ConstSetBA.出窑头充电点);

            //查找是否存在目标站点是窑头对接完成点的AGV
            F_AGV d_agv = F_DataCenter.MDev.IGetDevOnDestination(ConstSetBA.窑头对接完成站);

            // 有未上锁的、充电完成的AGV
            if (agv != null
                && agv.IsFree
                && agv.ChargeStatus == EnumChargeStatus.充电完成)
            {
                if (d_agv == null
                    && _plcHead.ExitFlag
                    )
                {
                    F_ExcTask task = new F_ExcTask(_plcHead, EnumOper.对接完成, ConstSetBA.出窑头充电点, ConstSetBA.窑头对接完成点);

                    _plcHead.ExitFlag = false;

                    task.Id = agv.Id;

                    F_DataCenter.MTask.IStartTask(task, agv.Id + ExitPlcHeadChargeSucMsg);

                    sendServerLog(agv.Id + ExitPlcHeadChargeSucMsg);

                    //LogFactory.LogDispatch(agv.Id, "充电完成", ExitPlcHeadChargeSucMsg);
                    FLog.Log(agv.Id + ExitPlcHeadChargeSucMsg);

                }
                else
                {
                    _plcHead.ExitFlag = false;
                }
            }
        }

        private string PlcEndChargeMsg = "窑尾等 到 进窑尾充";
        /// <summary>
        /// 窑尾等 到 进窑尾充
        /// </summary>
        private void TaskEndToEnterBattery()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(ConstSetBA.窑尾装载等待区);

            //搜索车辆电量列表
            List<int> list = F_DataCenter.MDev.IGetDevElectricity();

            //如果电量小于百分之80才进行充电判断
            if (list[0] <= 80)
            {
                //是否到达电池最低点的标志
                int BatteryLim = 0;

                if (list[0] <= 40)
                {
                    BatteryLim = 1;/*电量有低于40的车*/
                }
                if (agv.Electicity <= list[0])
                {
                    switch (BatteryLim)
                    {
                        case 0:
                            //本来的充电逻辑
                            {
                                //查找是否存在目标站点是进窑尾充电站的AGV
                                F_AGV d_agv = F_DataCenter.MDev.IGetDevOnDestination(ConstSetBA.充电桩站);/*在充电桩*/
                                F_AGV d_agv2 = F_DataCenter.MDev.IGetAliveDevOnSite(ConstSetBA.进窑尾充电点);/*进充电桩*/

                                //去进窑尾充电站的车是否是窑尾的车
                                if (d_agv != null && (d_agv.Site != "26" && d_agv.Site != "36" && d_agv.Site != "66"))
                                {
                                    d_agv = null;
                                }

                                if (d_agv != null || d_agv2 != null)
                                {
                                    ParamControl.Do_EnterEndChargeLock = false;

                                    if (d_agv != null)
                                    {
                                        _plcEnd.IsEnterBatteryLock = true;

                                        _plcEnd.EnterChargeAgv = d_agv.Id;

                                        ParamControl.EndChargeLock = _plcEnd.IsEnterBatteryLock;
                                        ParamControl.EndChargeAGV = _plcEnd.EnterChargeAgv;
                                    }
                                    else if (d_agv2 != null)
                                    {
                                        _plcEnd.IsEnterBatteryLock = true;

                                        _plcEnd.EnterChargeAgv = d_agv2.Id;
                                        ParamControl.EndChargeLock = _plcEnd.IsEnterBatteryLock;
                                        ParamControl.EndChargeAGV = _plcEnd.EnterChargeAgv;
                                    }
                                }


                                // 让未上锁的、电量低于60且未充电的AGV去充电，且接货充电点没有AGV
                                if (agv != null
                                    && agv.IsFree
                                    //&& agv.Electicity <= list[0]
                                    && agv.ChargeStatus == EnumChargeStatus.未充电
                                    && (d_agv == null && d_agv2 == null)
                                    && !_plcEnd.IsEnterBatteryLock
                                    //&& ParamControl.Do_EnterEndChargeLock
                                    )
                                {
                                    _PlcEndNeedCharge = true;

                                    F_ExcTask task = new F_ExcTask(_plcEnd, EnumOper.充电, ConstSetBA.窑尾装载等待区, ConstSetBA.进窑尾充电点);

                                    F_AGV.AgvLock(agv.Id);

                                    //进窑尾充电，锁定进窑尾充电桩
                                    _plcEnd.IsEnterBatteryLock = true;

                                    ParamControl.Do_EnterEndChargeLock = false;

                                    _plcEnd.EnterChargeAgv = agv.Id;

                                    ParamControl.EndChargeLock = _plcEnd.IsEnterBatteryLock;

                                    ParamControl.EndChargeAGV = _plcEnd.EnterChargeAgv;

                                    task.Id = agv.Id;

                                    F_DataCenter.MTask.IStartTask(task, agv.Id + PlcEndChargeMsg);

                                    sendServerLog(agv.Id + PlcEndChargeMsg);

                                    //LogFactory.LogDispatch(agv.Id, "充电", PlcEndChargeMsg);
                                    FLog.Log(agv.Id + PlcEndChargeMsg);

                                }
                                else
                                {
                                    _PlcEndNeedCharge = false;
                                }
                            }
                            break;
                        case 1:
                            //本来的充电逻辑
                            {
                                //查找是否存在目标站点是进窑尾充电站的AGV
                                F_AGV d_agv = F_DataCenter.MDev.IGetDevOnDestination(ConstSetBA.充电桩站);/*在充电桩*/
                                F_AGV d_agv2 = F_DataCenter.MDev.IGetAliveDevOnSite(ConstSetBA.进窑尾充电点);/*进充电桩*/

                                //去进窑尾充电站的车是否是窑尾的车
                                if (d_agv != null && (d_agv.Site != "26" && d_agv.Site != "36" && d_agv.Site != "66"))
                                {
                                    d_agv = null;
                                }

                                if (d_agv != null || d_agv2 != null)
                                {
                                    ParamControl.Do_EnterEndChargeLock = false;

                                    if (d_agv != null)
                                    {
                                        _plcEnd.IsEnterBatteryLock = true;

                                        _plcEnd.EnterChargeAgv = d_agv.Id;

                                        ParamControl.EndChargeLock = _plcEnd.IsEnterBatteryLock;
                                        ParamControl.EndChargeAGV = _plcEnd.EnterChargeAgv;
                                    }
                                    else if (d_agv2 != null)
                                    {
                                        _plcEnd.IsEnterBatteryLock = true;

                                        _plcEnd.EnterChargeAgv = d_agv2.Id;
                                        ParamControl.EndChargeLock = _plcEnd.IsEnterBatteryLock;
                                        ParamControl.EndChargeAGV = _plcEnd.EnterChargeAgv;
                                    }
                                }


                                // 让未上锁的、电量低于60且未充电的AGV去充电，且接货充电点没有AGV
                                if (agv != null
                                    && agv.IsFree
                                    //&& agv.Electicity <= list[0]
                                    && agv.ChargeStatus == EnumChargeStatus.未充电
                                    && (d_agv == null && d_agv2 == null)
                                    && !_plcEnd.IsEnterBatteryLock
                                    //&& ParamControl.Do_EnterEndChargeLock
                                    )
                                {
                                    _PlcEndNeedCharge = true;

                                    F_ExcTask task = new F_ExcTask(_plcEnd, EnumOper.充电, ConstSetBA.窑尾装载等待区, ConstSetBA.进窑尾充电点);

                                    F_AGV.AgvLock(agv.Id);

                                    //进窑尾充电，锁定进窑尾充电桩
                                    _plcEnd.IsEnterBatteryLock = true;

                                    ParamControl.Do_EnterEndChargeLock = false;

                                    _plcEnd.EnterChargeAgv = agv.Id;

                                    ParamControl.EndChargeLock = _plcEnd.IsEnterBatteryLock;

                                    ParamControl.EndChargeAGV = _plcEnd.EnterChargeAgv;

                                    task.Id = agv.Id;

                                    F_DataCenter.MTask.IStartTask(task, agv.Id + PlcEndChargeMsg);

                                    sendServerLog(agv.Id + PlcEndChargeMsg);

                                    //LogFactory.LogDispatch(agv.Id, "充电", PlcEndChargeMsg);
                                    FLog.Log(agv.Id + PlcEndChargeMsg);

                                }
                                else
                                {
                                    _PlcEndNeedCharge = false;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }


            }
        }


        //private string PlcHeadChargeMsg = "窑头等 到 进窑头充";
        ///// <summary>
        ///// 窑头等 到 进窑头充
        ///// </summary>
        //private void TaskHeadToEnterBattery()
        //{
        //    F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(ConstSetBA.窑头卸载等待区);
        //    F_AGV agv1 = F_DataCenter.MDev.IGetDevOnSite(ConstSetBA.进窑头充电点);

        //    if (agv != null && agv.IsFree
        //       && agv.Electicity <= F_DataCenter.MDev.IGetDevElectricity()
        //       && agv.ChargeStatus == EnumChargeStatus.未充电
        //       && agv1 == null
        //       && !_plcHead.IsEnterBatteryLock
        //       )
        //    {

        //        F_ExcTask task = new F_ExcTask(_plcHead, EnumOper.充电, ConstSetBA.窑头卸载等待区, ConstSetBA.进窑头充电点);

        //        task.Id = agv.Id;

        //        //进窑头充电，锁定进窑头充电桩
        //        _plcHead.IsEnterBatteryLock = true;

        //        ParamControl.Do_EnterHeadChargeLock = false;

        //        _plcHead.EnterChargeAgv = agv.Id;

        //        F_DataCenter.MTask.IStartTask(task, agv.Id + PlcHeadChargeMsg);

        //        sendServerLog(agv.Id + PlcHeadChargeMsg);

        //        //LogFactory.LogDispatch(agv.Id, "充电", PlcHeadChargeMsg);
        //        FLog.Log(agv.Id + PlcHeadChargeMsg);

        //    }
        //}

        private string PlcEndChargeSucMsg = "进窑尾充 到 窑尾取";
        /// <summary>
        /// 进窑尾充 到 窑尾取
        /// </summary>
        public void TaskEnterEndChargeSuc()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(ConstSetBA.进窑尾充电点);

            //查找是否存在目标站点是窑尾的AGV
            F_AGV d_agv = F_DataCenter.MDev.IGetDevOnDestination(ConstSetBA.窑尾装载站);
            F_AGV d_agv2 = F_DataCenter.MDev.IGetAliveDevOnSite(ConstSetBA.窑尾装载点);

            // 有未上锁的、充电完成的AGV,且窑尾装载点有货、AGV上无货
            if (//(ParamControl.Do_EndPlcLock && !_plcEnd.IsLock)&&
                //(d_agv == null && d_agv2 == null)
                 agv != null && agv.IsFree
                //&& !F_AGV.IsLock(agv.Id)
                && agv.ChargeStatus == EnumChargeStatus.充电完成
                //&& _plcEnd.IsLock == false
                )
            {
                if (//_plcEnd.Sta_Material == EnumSta_Material.有货 &&
                    //agv.Sta_Material == EnumagvSta_Material.无货
                    true
                    )
                {
                    _EnterPlcEndChargeSuc = true;

                    F_ExcTask task = new F_ExcTask(_plcEnd, EnumOper.取货, ConstSetBA.进窑尾充电点, ConstSetBA.窑尾装载点);

                    F_AGV.AgvLock(agv.Id);

                    //去取货，锁定窑尾
                    _plcEnd.IsLock = true;

                    ParamControl.Do_EndPlcLock = false;

                    task.Id = agv.Id;

                    //Thread.Sleep(2000);

                    F_DataCenter.MTask.IStartTask(task, agv.Id + PlcEndChargeSucMsg);

                    sendServerLog(agv.Id + PlcEndChargeSucMsg);

                    //LogFactory.LogDispatch(agv.Id, "充电完成", PlcEndChargeSucMsg);
                    FLog.Log(agv.Id + PlcEndChargeSucMsg);

                }
            }
            else
            {
                _EnterPlcEndChargeSuc = false;
            }
        }


        //private string PlcHeadChargeSucMsg = "进窑头充 到 窑头卸";
        ///// <summary>
        ///// 进窑头充 到 窑头卸
        ///// </summary>
        //public void TaskEnterHeadChargeSuc()
        //{
        //    F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(ConstSetBA.进窑头充电点);
        //    // 有充电完成的AGV,且窑头卸载点没货
        //    if ((ParamControl.Do_HeadPlcLock && !_plcHead.IsLock) && agv != null && agv.IsFree
        //        && agv.ChargeStatus == EnumChargeStatus.充电完成
        //        && !F_AGV.IsLock(agv.Id)
        //       )
        //    {
        //        _EnterPlcHeadChargeSuc = true;

        //        if (_plcHead.Sta_Material == EnumSta_Material.无货
        //            && agv.Sta_Material == EnumagvSta_Material.有货
        //            )
        //        {
        //            F_ExcTask task = new F_ExcTask(_plcHead, EnumOper.放货, ConstSetBA.进窑头充电点, ConstSetBA.窑头卸载点);

        //            F_AGV.AgvLock(agv.Id);

        //            //去卸货，锁定窑头
        //            _plcHead.IsLock = true;
        //            ParamControl.Do_HeadPlcLock = false;

        //            task.Id = agv.Id;

        //            Thread.Sleep(2000);

        //            F_DataCenter.MTask.IStartTask(task, agv.Id + PlcHeadChargeSucMsg);

        //            sendServerLog(agv.Id + PlcHeadChargeSucMsg);

        //            //LogFactory.LogDispatch(agv.Id, "充电完成", PlcHeadChargeSucMsg);
        //            FLog.Log(agv.Id + PlcHeadChargeSucMsg);

        //        }
        //    }
        //    else
        //    {
        //        _EnterPlcHeadChargeSuc = false;
        //    }
        //}

        private string PlcEndStartSucMsg = "窑尾启动辊台";
        /// <summary>
        /// 窑尾启动辊台
        /// </summary>
        public void TaskPlcEndStart()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(ConstSetBA.窑尾装载点);

            if (agv != null &&
                agv.IsFree &&
                agv.Sta_Material == EnumagvSta_Material.无货
               )
            {

                F_ExcTask task = new F_ExcTask(_plcEnd, EnumOper.取货, ConstSetBA.窑尾装载点, ConstSetBA.窑尾装载点);

                task.Id = agv.Id;

                _plcEnd.IsLock = true;

                F_DataCenter.MTask.IStartTask(task, agv.Id + PlcEndStartSucMsg);

                sendServerLog(agv.Id + PlcEndStartSucMsg);

                //LogFactory.LogDispatch(agv.Id, "窑尾启动辊台", PlcEndStartSucMsg);
                FLog.Log(agv.Id + PlcEndStartSucMsg);

            }

        }

        private string PlcHeadStartSucMsg = "窑头启动辊台";
        /// <summary>
        /// 窑头启动辊台
        /// </summary>
        public void TaskPlcHeadStart()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(ConstSetBA.窑头卸载点);

            if (agv != null &&
                agv.IsFree &&
                agv.Sta_Material == EnumagvSta_Material.有货
               )
            {

                F_ExcTask task = new F_ExcTask(_plcHead, EnumOper.放货, ConstSetBA.窑头卸载点, ConstSetBA.窑头卸载点);

                task.Id = agv.Id;

                _plcHead.IsLock = true;

                F_DataCenter.MTask.IStartTask(task, agv.Id + PlcHeadStartSucMsg);

                sendServerLog(agv.Id + PlcHeadStartSucMsg);

                //LogFactory.LogDispatch(agv.Id, "窑头启动辊台", PlcHeadStartSucMsg);
                FLog.Log(agv.Id + PlcHeadStartSucMsg);

            }

        }

        private string toEndGrtMsg = "(初)继续 窑尾取";
        private string toHeadGrtMsg = "(初)继续 窑头卸";
        private string initToEndSucMsg = "(初)Agv有货 到 窑尾完";
        private string initToHeadSucMsg = "(初)Agv无货 到 窑头完";
        private string initToEndWaitMsg = "(初)Agv无货 到 窑尾等";
        private string initToHeadWaitMsg = "(初)Agv有货 到 窑头等";
        /// <summary>
        /// 初始化，让AGV回到相应的点或者执行相应的任务
        /// </summary>
        private void InitToAllAGV()
        {
            List<F_AGV> agvs = F_DataCenter.MDev.InitGetDevNot(agvid);
            PLCEndTrafficSite.AddRange(new string[] { "26", "16", "21", "11" });
            PLCHeadTrafficSite.AddRange(new string[] { "23", "13", "24", "14" });
            PLCEndToHeadWaitSite.AddRange(new string[] { "12", "62" });
            PLCHeadToEndWaitSite.AddRange(new string[] { "15", "65" });

            if (agvs != null)
            {
                foreach (F_AGV agv in agvs)
                {
                    // 窑尾交通管制点是否有车
                    if (PLCEndTrafficSite.Contains(agv.Site))
                    {
                        if (agv.Site == ConstSetBA.窑尾装载点)
                        {
                            // 初始化，在窑尾装载点且有货，去到窑尾对接完成点
                            if (agv.Sta_Material == EnumagvSta_Material.有货)
                            {
                                F_ExcTask task = new F_ExcTask(null, EnumOper.对接完成, agv.Site, ConstSetBA.窑尾对接完成点);

                                task.Id = agv.Id;

                                F_DataCenter.MTask.IStartTask(task, agv.Id + initToEndSucMsg);

                                sendServerLog(agv.Id + initToEndSucMsg);

                                //LogFactory.LogDispatch(agv.Id, "车辆初始化", initToEndSucMsg);
                                FLog.Log(agv.Id + initToEndSucMsg);
                            }
                            // 初始化，在窑尾装载点且无货，执行取货任务
                            else
                            {
                                _ToPlcEnd = true;

                                F_ExcTask task = new F_ExcTask(_plcEnd, EnumOper.取货, agv.Site, ConstSetBA.窑尾装载点);

                                F_AGV.AgvLock(agv.Id);

                                task.Id = agv.Id;

                                F_DataCenter.MTask.IStartTask(task, agv.Id + toEndGrtMsg);

                                sendServerLog(agv.Id + "初始化，位于装载点且无货的AGV，执行取货任务");

                                //LogFactory.LogDispatch(agv.Id, "车辆初始化", "位于装载点且无货的AGV，执行取货任务");
                                FLog.Log(agv.Id + "位于装载点且无货的AGV，执行取货任务");
                            }
                        }
                        // 不在装载点的车，判断地标是否为正反卡的21,若为取货完成的车去到窑尾对接完成点
                        else if (agv.Site == ConstSetBA.窑尾装载点的前一地标 && agv.Sta_Material == EnumagvSta_Material.有货)
                        {
                            F_ExcTask task = new F_ExcTask(null, EnumOper.对接完成, agv.Site, ConstSetBA.窑尾对接完成点);

                            task.Id = agv.Id;

                            F_DataCenter.MTask.IStartTask(task, agv.Id + initToEndSucMsg);

                            sendServerLog(agv.Id + initToEndSucMsg);

                            //LogFactory.LogDispatch(agv.Id, "车辆初始化 ,有货的AGV", initToEndSucMsg);
                            FLog.Log(agv.Id + initToEndSucMsg);
                        }
                        else
                        {
                            // 准备取货的AGV，去到窑尾装载点取货
                            _ToPlcEnd = true;

                            F_ExcTask task = new F_ExcTask(_plcEnd, EnumOper.取货, agv.Site, ConstSetBA.窑尾装载点);

                            F_AGV.AgvLock(agv.Id);

                            task.Id = agv.Id;

                            F_DataCenter.MTask.IStartTask(task, agv.Id + toEndGrtMsg);

                            sendServerLog(agv.Id + "初始化，准备取货的AGV，去到窑尾装载点取货");

                            //LogFactory.LogDispatch(agv.Id, "车辆初始化", "准备取货的AGV，去到窑尾装载点取货");
                            FLog.Log(agv.Id + "车辆初始化" + "准备取货的AGV，去到窑尾装载点取货");
                        }
                    }
                    // 窑头交通管制点是否有车
                    else if (PLCHeadTrafficSite.Contains(agv.Site))
                    {
                        if (agv.Site == ConstSetBA.窑头卸载点)
                        {
                            // 初始化，在窑头卸载点且无货，去到窑头对接完成点
                            if (agv.Sta_Material == EnumagvSta_Material.无货)
                            {
                                F_ExcTask task = new F_ExcTask(null, EnumOper.对接完成, agv.Site, ConstSetBA.窑头对接完成点);

                                task.Id = agv.Id;

                                F_DataCenter.MTask.IStartTask(task, agv.Id + initToHeadSucMsg);

                                sendServerLog(agv.Id + TaskHeadToHeadSucMsg);

                                //LogFactory.LogDispatch(agv.Id, "车辆初始化", initToHeadSucMsg);
                                FLog.Log(agv.Id + initToHeadSucMsg);
                            }
                            // 初始化，在窑头卸载点且无货，执行放货任务
                            else
                            {
                                _ToPlcHead = true;

                                F_ExcTask task = new F_ExcTask(_plcHead, EnumOper.放货, agv.Site, ConstSetBA.窑头卸载点);

                                F_AGV.AgvLock(agv.Id);

                                task.Id = agv.Id;

                                F_DataCenter.MTask.IStartTask(task, agv.Id + toHeadGrtMsg);

                                sendServerLog(agv.Id + "初始化，位于窑头卸载点且有货，执行放货任务");

                                //LogFactory.LogDispatch(agv.Id, "车辆初始化", "位于窑头卸载点且有货，执行放货任务");
                                FLog.Log(agv.Id + "车辆初始化，" + "位于窑头卸载点且有货，执行放货任务");
                            }
                        }
                        // 不在卸载点的车，判断地标是否为正反卡的24，若为放货完成的车去到窑头对接完成点
                        else if (agv.Site == ConstSetBA.窑头卸载点的前一地标 && agv.Sta_Material == EnumagvSta_Material.无货)
                        {
                            F_ExcTask task = new F_ExcTask(null, EnumOper.对接完成, agv.Site, ConstSetBA.窑头对接完成点);

                            task.Id = agv.Id;

                            F_DataCenter.MTask.IStartTask(task, agv.Id + initToHeadSucMsg);

                            sendServerLog(agv.Id + initToHeadSucMsg);

                            //LogFactory.LogDispatch(agv.Id, "车辆初始化 ,无货的AGV", initToHeadSucMsg);
                            FLog.Log(agv.Id + "车辆初始化 ,无货的AGV" + initToHeadSucMsg);
                        }
                        else
                        {
                            // 准备卸货的AGV，去到窑头卸载点放货
                            _ToPlcEnd = true;

                            F_ExcTask task = new F_ExcTask(_plcHead, EnumOper.放货, agv.Site, ConstSetBA.窑头卸载点);

                            F_AGV.AgvLock(agv.Id);

                            task.Id = agv.Id;

                            F_DataCenter.MTask.IStartTask(task, agv.Id + toHeadGrtMsg);

                            sendServerLog(agv.Id + "初始化， 准备卸货的AGV，去到窑头卸载点放货");

                            //LogFactory.LogDispatch(agv.Id, "车辆初始化", " 准备卸货的AGV，去到窑头卸载点放货");
                            FLog.Log(agv.Id + "车辆初始化， 准备卸货的AGV，去到窑头卸载点放货");
                        }
                    }
                    /// 不在任何交通管制点的车，去到相应的等待点
                    /// 窑头卸货完成的车，直接去到窑尾装载等待点
                    else if (PLCHeadToEndWaitSite.Contains(agv.Site) && agv.Sta_Material == EnumagvSta_Material.无货)
                    {
                        F_ExcTask task = new F_ExcTask(null, EnumOper.无动作, agv.Site, ConstSetBA.窑尾装载等待区);

                        task.Id = agv.Id;

                        F_DataCenter.MTask.IStartTask(task, agv.Id + initToEndWaitMsg);

                        sendServerLog(agv.Id + initToEndWaitMsg);

                        //LogFactory.LogDispatch(agv.Id, "车辆初始化 ,窑头交管解除点的AGV", initToEndWaitMsg);
                        FLog.Log(agv.Id + "车辆初始化 ,窑头交管解除点的AGV");
                    }
                    ///  窑尾取货完成的车，直接去到窑头卸载等待点
                    else if (PLCEndToHeadWaitSite.Contains(agv.Site) && agv.Sta_Material == EnumagvSta_Material.有货)
                    {
                        F_ExcTask task = new F_ExcTask(null, EnumOper.无动作, agv.Site, ConstSetBA.窑头卸载等待区);

                        task.Id = agv.Id;

                        F_DataCenter.MTask.IStartTask(task, agv.Id + initToHeadWaitMsg);

                        sendServerLog(agv.Id + initToHeadWaitMsg);

                        //LogFactory.LogDispatch(agv.Id, "车辆初始化 ,窑尾交管解除点的AGV", initToHeadWaitMsg);
                        FLog.Log(agv.Id + "车辆初始化 ,窑尾交管解除点的AGV");
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

                List<DeviceBackImf> devsList = WcfMainHelper.GetDevList();

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

        /// <summary>
        /// 发生故障、离线的车，清除其相应的任务
        /// </summary>
        public void ClearTask()
        {
            while (true)
            {
                Thread.Sleep(5000);
                List<F_AGV> agvs = F_DataCenter.MDev.ErrorOrFalse();
                List<FDispatchBackImf> dispatchList = WcfMainHelper.GetDispatchList();
                if (dispatchList != null && agvs != null && dispatchList.Count > 0)
                {
                    foreach (var agv in agvs)
                    {
                        foreach (var dispatch in dispatchList)
                        {
                            // 有故障的车是否对应任务的设备ID
                            if (agv.Id == dispatch.Dev)
                            {
                                if (dic.ContainsKey(agv.Id))
                                {
                                    int count = 0;
                                    dic.TryGetValue(agv.Id, out count);
                                    if (count >= 10)
                                    {
                                        // 终止该任务
                                        WcfMainHelper.CtrDispatch(dispatch.Id, EnumCtrType.Stop);
                                        sendServerLog("终止异常的 " + agv.Id + "正在执行的任务");

                                        //LogFactory.LogRunning("终止异常的 " + agv.Id + "正在执行的任务");
                                        FLog.Log("终止异常的 " + agv.Id + "正在执行的任务");

                                        count = 0;

                                        //异常终止的任务释放AGV
                                        F_AGV.AgvRelease(agv.Id);
                                    }
                                    else
                                    {
                                        count++;
                                        sendServerLog("异常的 " + agv.Id + "已等待处理 " + count + " 次");

                                        //LogFactory.LogRunning("异常的 " + agv.Id + "已等待处理 " + count + " 次");
                                        FLog.Log("异常的 " + agv.Id + "已等待处理 " + count + " 次");

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
