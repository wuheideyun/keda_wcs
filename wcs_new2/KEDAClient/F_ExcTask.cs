using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
//using LogHelper;
using FLCommonInterfaces;
using WcfHelper;

namespace KEDAClient
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum EnumOper
    {
        无动作,
        对接完成,
        取货,
        放货,
        充电,
    }

    /// <summary>
    /// 执行任务对象
    /// </summary>
    public class F_ExcTask
    {
        string _id = Guid.NewGuid().ToString();

        int _objID;

        /// <summary>
        /// 任务起点
        /// </summary>
        string _startSite = "";

        /// <summary>
        /// 任务终点
        /// </summary>
        string _endSite = "null";

        /// <summary>
        /// 操作PLC对象
        /// </summary>
        F_PLCLine _plc = null;

        /// <summary>
        /// 操作类型
        /// </summary>
        EnumOper _operType = EnumOper.无动作;

        /// <summary>
        /// 操作AGV对象
        /// </summary>
        F_AGV _agv = null;

        /// <summary>
        /// 离线计数
        /// </summary>
        int _disAliveCounter = 0;

        /// <summary>
        /// 任务已经开始
        /// </summary>
        bool _isStart = false;

        /// <summary>
        /// 是否离线超时
        /// </summary>
        bool _isDisaliveOverTime = false;

        /// <summary>
        /// 是否离线超时
        /// </summary>
        public bool IsDisaliveOverTime
        {
            get { return _isDisaliveOverTime; }
        }
        /// <summary>
        /// 计数器
        /// </summary>
        int _triggerCounter = 0;

        /// <summary>
        /// 是否超时
        /// </summary>
        bool _isOverTime = false;

        /// <summary>
        /// 是否超时
        /// </summary>
        public bool IsOverTime
        {
            get { return _isOverTime; }
        }

        /// <summary>
        /// 是否已经完成
        /// </summary>
        bool _isSuc = false;

        /// <summary>
        /// 此次任务的调度结果
        /// </summary>
        FDispatchBackImf _taskDispatch = null;

        /// <summary>
        /// 电机开始启动时间
        /// </summary>
        private DateTime BeginTime;

        /// <summary>
        /// 对象的唯一标识
        /// </summary>
        public int NO
        {
            get { return _objID; }
        }

        public string StartSite
        {
            get { return _startSite; }
            set { _startSite = value; }
        }

        public string EndSite
        {
            get { return _endSite; }
            set { _endSite = value; }
        }

        public string GetTaskInfo()
        {
            return "从地标" + StartSite + "到地标" + EndSite;
        }

        public string GetAgvId()
        {
            return _agv.Id;
        }

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 操作PLC对象
        /// </summary>
        public F_PLCLine Plc
        {
            get { return _plc; }
        }

        /// <summary>
        /// 是否已经完成
        /// </summary>
        public bool IsSuc
        {
            get { return _isSuc; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plc"></param>
        /// <param name="oper"></param>
        public F_ExcTask(F_PLCLine plc, EnumOper oper, string startSite, string endSite)
        {
            _plc = plc;

            _operType = oper;

            _startSite = startSite;

            _endSite = endSite;

            _objID = this.GetHashCode();// int.Parse(DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.DateTimeFormatInfo.InvariantInfo));
        }

        /// <summary>
        /// 任务完成
        /// </summary>
        public void ISetTaskSuc()
        {

            if (_taskDispatch != null) { if (WcfMainHelper.CtrDispatch(_taskDispatch.Id, EnumCtrType.Stop)) { _isSuc = true; } }

        }

        /// <summary>
        /// 发送窑头电机启动时间是否完成
        /// </summary>
        /// <returns></returns>
        public bool IsHeadRunTimeFinish()
        {
            return (DateTime.Now - BeginTime).TotalSeconds > ParamControl.IgnoreHeadUnloadSecond;
        }

        /// <summary>
        /// 发送窑尾电机启动时间是否完成
        /// </summary>
        /// <returns></returns>
        public bool IsTailRunTimeFinish()
        {
            return (DateTime.Now - BeginTime).TotalSeconds > ParamControl.IgnoreTailUnloadSecond;
        }

        /// <summary>
        /// 事务处理
        /// </summary>
        public String DoWork()
        {
            if (_isSuc) { return ""; }

            if (!_isStart)
            {
                if (++_triggerCounter > F_DataCenter.ClearTime) { _isOverTime = true; }
            }

            if (_agv != null)
            {
                if (_agv.IsAlive)
                {
                    _disAliveCounter = 0;
                }
                else
                {
                    if (++_disAliveCounter > F_DataCenter.ClearTime) { _isDisaliveOverTime = true; }
                }
            }
            else
            {
                if (++_triggerCounter > F_DataCenter.ClearTime) { _isOverTime = true; }
            }

            _taskDispatch = WcfMainHelper.GetDispatch(Id);

            if (_taskDispatch == null)
            {
                FDispatchOrder dis = new FDispatchOrder();

                dis.NavigationType = EnumNavigationType.Magnet;

                dis.Id = Id;

                dis.EndSite = _endSite;

                if (!string.IsNullOrEmpty(_startSite)) { dis.StartSiteList.Add(_startSite); }

                ///修改By fujun
                dis.DevList.Add(Id);

                string back = "";

                WcfMainHelper.StartDispatch(dis, out back);

                return back;
            }
            else
            {
                ///修改By fujun
                _isStart = true;

                #region 滚筒逻辑
                ///确定此时任务的AGV
                if (_agv == null) { _agv = new F_AGV(_taskDispatch.Dev); }

                ///此次调度任务已经完成
                if (_taskDispatch.Statue == EnumResultType.Suc)
                {

                    if (_operType == EnumOper.取货)       //窑尾
                    {
                        ///当前AGV的到达的地标 与 棍台绑定地标一致
                        if (_agv.Site == _plc.Site)
                        {
                            if (_agv != null && _plc.EnterChargeAgv == _agv.Id)
                            {
                                if (_plc.IsEnterBatteryLock
                                    //&& !ParamControl.Do_EnterEndChargeLock
                                    )
                                {
                                    _plc.IsEnterBatteryLock = false;

                                    ParamControl.Do_EnterEndChargeLock = true;

                                    _plc.EnterChargeAgv = null;
                                }
                            }

                            //如果界面打开忽略《窑尾》AGV货物状态和Plc货物状态则 直接发送棍台转动命令
                            if (ParamControl.Is_IgnoreTailUnloadStatus ||
                                (_plc.Sta_Material == EnumSta_Material.有货
                                && (_agv.Sta_Material == EnumagvSta_Material.无货 || _agv.Sta_Material == EnumagvSta_Material.传送中)))
                            {
                                if (_agv.Sta_Monitor != EnumSta_Monitor.电机反转)
                                {
                                    _agv.SendOrdr(EnumType.上料操作, EnumPara.agv上料启动);

                                }
                                else
                                {
                                    if (BeginTime == null) BeginTime = System.DateTime.Now;
                                    _agv.SendOrdr(EnumType.上料操作, EnumPara.agv上料启动);

                                    _plc.SendOrdr(EnumType.下料操作, EnumPara.窑尾辊台允许下料);
                                }

                            }

                            //如果界面打开忽略《窑尾》AGV货物状态，并且上面已经发送了指定时间的棍台转动时间
                            if ((ParamControl.Is_IgnoreTailUnloadStatus && IsTailRunTimeFinish()) ||
                                (//_plc.Sta_Material == EnumSta_Material.无货  && 
                                _agv.Sta_Material == EnumagvSta_Material.有货))
                            {
                                _agv.SendOrdr(EnumType.上料操作, EnumPara.agv辊台停止);

                                _plc.SendOrdr(EnumType.下料操作, EnumPara.窑头辊台上料完成);

                                //如果界面打开忽略《窑尾》AGV棍台状态，则进去结束任务
                                if (ParamControl.Is_IgnoreTailStaStatus ||
                                    _agv.Sta_Monitor == EnumSta_Monitor.电机停止
                                    )
                                {
                                    //取货完成，解锁窑尾
                                    if (_plc != null
                                        // && !ParamControl.Do_EndPlcLock
                                        )
                                    {
                                        _plc.IsLock = false;

                                        ParamControl.Do_EndPlcLock = true;
                                    }

                                    ISetTaskSuc();
                                }
                            }
                        }
                        else
                        {
                            ISetTaskSuc();
                        }
                        return "";
                    }
                    else if (_operType == EnumOper.放货)      //窑头
                    {
                        ///当前AGV的到达的地标 与 棍台绑定地标一致
                        if (_agv.Site == _plc.Site)
                        {
                            if (_agv != null && _plc.EnterChargeAgv == _agv.Id)
                            {
                                if (_plc.IsEnterBatteryLock
                                    )
                                {
                                    _plc.IsEnterBatteryLock = false;

                                    _plc.EnterChargeAgv = null;
                                }
                            }
                            //如果界面打开忽略《窑头》AGV货物状态和Plc货物状态则 直接发送棍台转动命令
                            if (ParamControl.Is_IgnoreHeadUnloadStatus ||
                                ((_plc.Sta_Material == EnumSta_Material.允许下料 || _plc.Sta_Material == EnumSta_Material.无货
                                || _plc.Sta_Material == EnumSta_Material.未知) &&
                                (_agv.Sta_Material == EnumagvSta_Material.传送中 || _agv.Sta_Material == EnumagvSta_Material.有货)))
                            {

                                if (_agv.Sta_Monitor != EnumSta_Monitor.电机反转)
                                {
                                    _agv.SendOrdr(EnumType.下料操作, EnumPara.agv下料启动);

                                }
                                else
                                {
                                    if (BeginTime == null) BeginTime = System.DateTime.Now;
                                    _plc.SendOrdr(EnumType.上料操作, EnumPara.窑头辊台上料中);

                                    _agv.SendOrdr(EnumType.下料操作, EnumPara.agv下料启动);
                                }

                            }

                            //如果界面打开忽略《窑头》AGV货物状态，并且上面已经发送了指定时间的棍台转动时间
                            if ((ParamControl.Is_IgnoreHeadUnloadStatus && IsHeadRunTimeFinish()) ||
                                (//(_plc.Sta_Material == EnumSta_Material.允许下料 || _plc.Sta_Material == EnumSta_Material.未知) &&
                               _agv.Sta_Material == EnumagvSta_Material.无货))
                            {
                                _plc.SendOrdr(EnumType.上料操作, EnumPara.窑头辊台上料完成);

                                _agv.SendOrdr(EnumType.下料操作, EnumPara.agv辊台停止);

                                //如果界面打开忽略《窑头》AGV棍台状态，则进去结束任务
                                if (ParamControl.Is_IgnoreHeadStaStatus ||
                                    _agv.Sta_Monitor == EnumSta_Monitor.电机停止
                                    )
                                {
                                    //放货完成，解锁窑头
                                    if (_plc != null
                                        //&& !ParamControl.Do_HeadPlcLock
                                        )
                                    {
                                        _plc.IsLock = false;

                                        ParamControl.Do_HeadPlcLock = true;
                                    }

                                    ISetTaskSuc();
                                }
                            }

                        }
                        else
                        {
                            ISetTaskSuc();
                        }
                        return "";
                    }
                    else if (_operType == EnumOper.充电)
                    {
                        if (!_plc.ExitFlag)
                        {
                            _plc.ExitFlag = true;
                        }
                        ISetTaskSuc();

                        return "";
                    }
                    else if (_operType == EnumOper.无动作)
                    {
                        ISetTaskSuc();

                        return "";
                    }
                    else if (_operType == EnumOper.对接完成)
                    {
                        if (_plc.IsExitBatteryLock && _plc.ExitChargeAgv == _agv.Id)
                        {

                            _plc.IsExitBatteryLock = false;

                            _plc.ExitChargeAgv = null;

                        }

                        if (!_plc.ExitFlag)
                        {
                            _plc.ExitFlag = true;
                        }
                        ISetTaskSuc();
                        return "";
                    }
                }
                return "";

                #endregion
            }
        }
    }

    /// <summary>
    /// 任务管理器
    /// </summary>
    public class F_ExcTaskManager
    {
        object _ans = new object();
        private SynchronizationContext mainThreadSynContext;

        ListBox listBox;

        List<F_ExcTask> _taskList = new List<F_ExcTask>();

        /// <summary>
        /// 线程
        /// </summary>
        Thread _thread = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public F_ExcTaskManager(SynchronizationContext context, ListBox listBoxOutput)
        {
            mainThreadSynContext = context;

            listBox = listBoxOutput;

            _thread = new Thread(ThreadFunc);

            _thread.Name = "任务处理线程";

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
            List<F_ExcTask> taskList = new List<F_ExcTask>();

            while (true)
            {
                Thread.Sleep(500);

                try
                {
                    //同步任务
                    ClearLeftDispatch();

                    lock (_ans) { taskList.Clear(); taskList.AddRange(_taskList); }
                    if (ParamControl.Is_AutoExecuteTask)
                    {
                        foreach (var item in taskList)
                        {
                            String msg = item.DoWork();
                            if (msg != "") sendServerLog(msg);



                            ///任务完成 或者 超时 或者离线超时
                            if (item.IsSuc || item.IsOverTime || item.IsDisaliveOverTime) { IDeletTask(item.Id); }
                        }
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// 开始一个新的操作任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public bool IStartTask(F_ExcTask task, String msg)
        {
            lock (_ans)
            {
                F_ExcTask exit = _taskList.Find(c =>
                {
                    return //(c.Plc == task.Plc && task.Plc != null) ||
                    c.Id == task.Id;
                });

                if (exit == null)
                {
                    _taskList.Add(task);
                    PublicDataContorl.AddTaskData(new TaskData(task.NO, msg, task.StartSite + "," + task.EndSite));
                    return true;
                }
                else if (task.EndSite != exit.EndSite)
                {
                    _taskList.Remove(exit);
                    PublicDataContorl.TaskIsSucc(exit.NO);
                    _taskList.Add(task);
                    PublicDataContorl.AddTaskData(new TaskData(task.NO, msg, task.StartSite + "," + task.EndSite));
                    return true;
                }
                else if (task.StartSite != exit.StartSite)
                {
                    _taskList.Remove(exit);
                    PublicDataContorl.TaskIsSucc(exit.NO);
                    _taskList.Add(task);
                    PublicDataContorl.AddTaskData(new TaskData(task.NO, msg, task.StartSite + "," + task.EndSite));
                    return true;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 删除一个任务
        /// </summary>
        /// <param name="Id"></param>
        public void IDeletTask(string Id)
        {
            lock (_ans)
            {
                F_ExcTask exit = _taskList.Find(c => { return c.Id == Id; });

                if (exit != null && _taskList.Contains(exit))
                {

                    //LogFactory.LogAdd(LOGTYPE.FINISH, exit.Id, exit.GetTaskInfo(), "调度完成", exit.GetTaskInfo());//任务完成日志
                    FLog.Log("调度完成" + exit.GetTaskInfo());//任务完成日志
                    PublicDataContorl.TaskIsSucc(exit.NO);
                    _taskList.Remove(exit);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="no"></param>
        public void StopTask(int no)
        {
            lock (_ans)
            {
                F_ExcTask excTask = _taskList.Find(c => { return c.NO == no; });
                if (excTask != null && _taskList.Contains(excTask))
                {
                    //LogFactory.LogAdd(LOGTYPE.FINISH, excTask.Id, excTask.GetTaskInfo(), "调度终止", excTask.GetTaskInfo());//任务完成日志
                    FLog.Log("调度终止"+ excTask.GetTaskInfo());//任务完成日志
                    PublicDataContorl.TaskIsSucc(excTask.NO);
                    excTask.ISetTaskSuc();
                    _taskList.Remove(excTask);
                }
                else
                {
                    PublicDataContorl.TaskIsSucc(no);
                }
            }
        }
        /// <summary>
        /// 清除不在任务链表中的调度任务
        /// </summary>
        public void ClearLeftDispatch()
        {
            List<FDispatchBackImf> diss = F_DataCenter.MDev.DispatchList;

            if (diss != null)
            {
                diss.ForEach(c =>
                {
                    if (!IsDispatchInTask(c.Id))
                    {
                        WcfHelper.WcfMainHelper.CtrDispatch(c.Id, EnumCtrType.Stop);
                    }
                });
            }
        }

        /// <summary>
        /// 调度任务是否在任务链表中
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsDispatchInTask(string id)
        {
            lock (_ans)
            {
                return _taskList.Find(c => { return c.Id == id; }) != null;
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
