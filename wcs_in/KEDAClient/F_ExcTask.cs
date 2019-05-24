using Gfx.GfxDataManagerServer;
using GfxCommonInterfaces;
using LogHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace KEDAClient
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum EnumOper
    {
        无动作,
        取货,
        放货,
        充电,
        窑尾1号机械手,
        窑尾2号机械手,
        窑头机械手,
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
        /// 计数器
        /// </summary>
        int _triggerCounter = 0;

        /// <summary>
        /// 任务已经开始
        /// </summary>
        bool _isStart = false;

        /// <summary>
        /// 是否已经完成
        /// </summary>
        bool _isSuc = false;

        /// <summary>
        /// 此次任务的调度结果
        /// </summary>
        DispatchBackMember _taskDispatch = null;


        /// <summary>
        /// 对象的唯一标识
        /// </summary>
        public int NO
        {
            get { return _objID; }
        }


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

            _objID = this.GetHashCode();
        }

        /// <summary>
        /// 任务完成
        /// </summary>
        public void ISetTaskSuc()
        {
            //if (_plc != null) { _plc.IsLock = false; }
            if (_agv != null) { _agv.IsLock= false; }

            if (_taskDispatch != null) { if (JTWcfHelper.WcfMainHelper.CtrDispatch(_taskDispatch.DisGuid, DisOrderCtrTypeEnum.Stop)) { _isSuc = true; } }
            else { _isSuc = true; }
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

            _taskDispatch = JTWcfHelper.WcfMainHelper.GetDispatch(Id);

            if (_taskDispatch == null)
            {
                DispatchOrderObj dis = new DispatchOrderObj();

                dis.DisGuid = Id;

                dis.EndSite = _endSite;


                if (!string.IsNullOrEmpty(_startSite)) { dis.StartSiteList.Add(_startSite); }

                ///修改By fujun
                dis.DevList.Add(Id);

                string back = "";

                JTWcfHelper.WcfMainHelper.StartDispatch(dis, out back);
                return back;
            }
            else
            {
                ///修改By fujun
                _isStart = true;

                ///确定此时任务的AGV
                if (_agv == null) { _agv = new F_AGV(_taskDispatch.DisDevId); }

                ///此次调度任务已经完成
                if (_taskDispatch.OrderStatue == ResultTypeEnum.Suc)
                {

                    if (_operType == EnumOper.取货)
                    {
                        ///当前AGV的到达的地标 与 棍台绑定地标一致
                        if (_agv.Site == _plc.Site)
                        {
                            // 判断窑尾PLC的货物状态和AGV货物状态
                            if (
                                (_plc.Sta_Material_End == EnumSta_Material_End.窑尾传输中
                                //|| _plc.Sta_Material_End == EnumSta_Material_End.状态未知
                                || _plc.Sta_Material_End == EnumSta_Material_End.窑尾有货) &&
                                (_agv.Sta_Material == EnumSta_AGV.AGV无货
                                || _agv.Sta_Material == EnumSta_AGV.AGV传输中)
                                )
                            {

                                _agv.SendOrdr(EnumType.上料操作, EnumPara.agv上料启动);

                                _plc.SendOrdr(EnumType.下料操作, EnumPara.窑尾辊台启动);

                            }

                            // // 判断窑尾是否出料完成
                            if (//_plc.Sta_Material_End == EnumSta_Material_End.窑尾出料完成 &&
                                _agv.Sta_Material == EnumSta_AGV.AGV有货 &&
                               true)
                            {

                                _agv.SendOrdr(EnumType.上料操作, EnumPara.agv辊台停止);

                                _plc.SendOrdr(EnumType.下料操作, EnumPara.窑尾辊台停止);

                                if (true
                                    && _agv.Sta_Monitor == EnumSta_AGVMonitor.AGV电机停止
                                    )
                                {
                                    //取货完成，解锁窑尾
                                    if (_plc != null)
                                    {
                                        _plc.IsLock = false;

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
                    else if (_operType == EnumOper.放货)
                    {
                        ///当前AGV的到达的地标 与 棍台绑定地标一致
                        if (_agv.Site == _plc.Site)
                        {

                            if ((_plc.Sta_Material_Head == EnumSta_Material_Head.窑头无货
                                || _plc.Sta_Material_Head == EnumSta_Material_Head.窑头传输中)
                                && (_agv.Sta_Material == EnumSta_AGV.AGV传输中
                                || _agv.Sta_Material == EnumSta_AGV.AGV有货) &&
                                true)
                            {

                                _plc.SendOrdr(EnumType.上料操作, EnumPara.窑头辊台启动);

                                _agv.SendOrdr(EnumType.下料操作, EnumPara.agv下料启动);

                            }


                            if (//_plc.Sta_Material_Head == EnumSta_Material_Head.窑头接料完成 &&
                                _agv.Sta_Material == EnumSta_AGV.AGV无货 &&
                                true)
                            {
                                _plc.SendOrdr(EnumType.上料操作, EnumPara.窑头辊台停止);

                                _agv.SendOrdr(EnumType.下料操作, EnumPara.agv辊台停止);

                                if (_agv.Sta_Monitor == EnumSta_AGVMonitor.AGV电机停止 &&
                                    true)
                                {
                                    //放货完成，解锁窑头
                                    if (_plc != null)
                                    {
                                        _plc.IsLock = false;

                                    }

                                    Thread.Sleep(5000);
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
                        ISetTaskSuc();
                        return "";
                    }
                    else if (_operType == EnumOper.窑尾1号机械手)
                    {
                        ///当前AGV的地标是否为窑尾1号机械手位置
                        if (_agv.Site == _plc.Site)
                        {

                            _plc.SendOrdr(EnumType.下料操作, EnumPara.窑尾1号机械手启动);

                            if (_plc.Sta_Material_End == EnumSta_Material_End.窑尾1号机械手完成 &&
                                true
                                )
                            {
                                //1号机械手完成动作
                                ISetTaskSuc();
                            }
                        }
                        return "";
                    }
                    else if (_operType == EnumOper.窑尾2号机械手)
                    {
                        ///当前AGV的地标是否为窑尾2号机械手位置
                        if (_agv.Site == Site.窑尾2)
                        {

                            _plc.SendOrdr(EnumType.下料操作, EnumPara.窑尾2号机械手启动);

                            if (_plc.Sta_Material_End == EnumSta_Material_End.窑尾2号机械手完成 &&
                                true
                                )
                            {
                                ISetTaskSuc();
                            }
                        }
                        return "";
                    }
                    else if (_operType == EnumOper.窑头机械手)
                    {
                        ///当前AGV的地标是否为窑头机械手位置
                        if (_agv.Site == Site.窑头3)
                        {

                            _plc.SendOrdr(EnumType.上料操作, EnumPara.窑头机械手启动);

                            if (true
                                && _plc.Sta_Material_HeadMLP == EnumSta_Material_HeadMLP.窑头机械手完成
                                )
                            {
                                ISetTaskSuc();
                            }
                        }

                        return "";
                    }
                    else if (_operType == EnumOper.无动作)
                    {
                        ISetTaskSuc();
                        return "";
                    }
                }
                return "";
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

                    foreach (var item in taskList)
                    {
                        String msg = item.DoWork();
                        if (msg != "") sendServerLog(msg);

                        ///任务完成 或者 超时 或者离线超时
                        if (item.IsSuc || item.IsOverTime || item.IsDisaliveOverTime) { IDeletTask(item.Id); }
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
        public bool IStartTask(F_ExcTask task)
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
                    return true;
                }
                else if (task.EndSite != exit.EndSite)
                {
                    _taskList.Remove(exit);
                    _taskList.Add(task);
                    return true;
                }
                else if (task.StartSite != exit.StartSite)
                {
                    _taskList.Remove(exit);
                    _taskList.Add(task);
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

                    //LogFactory.LogFinish(exit.Id ,"调度完成",exit.GetTaskInfo());
                    LogFactory.LogAdd(LOGTYPE.FINISH, exit.Id, exit.GetAgvId(), "调度完成", exit.GetTaskInfo());//任务完成日志

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
                    LogFactory.LogAdd(LOGTYPE.FINISH, excTask.Id, excTask.GetTaskInfo(), "调度终止", excTask.GetTaskInfo());//任务完成日志
                    //FLog.Log("调度终止" + excTask.GetTaskInfo());//任务完成日志
                    //PublicDataContorl.TaskIsSucc(excTask.NO);
                    excTask.ISetTaskSuc();
                    _taskList.Remove(excTask);
                }
                //else
                //{
                //    PublicDataContorl.TaskIsSucc(no);
                //}
            }
        }


        /// <summary>
        /// 清除不在任务链表中的调度任务
        /// </summary>
        public void ClearLeftDispatch()
        {
            List<DispatchBackMember> diss = F_DataCenter.MDev.DispatchList;

            if (diss != null)
            {
                diss.ForEach(c =>
                {
                    if (!IsDispatchInTask(c.DisDevId))
                    {
                        JTWcfHelper.WcfMainHelper.CtrDispatch(c.DisGuid, DisOrderCtrTypeEnum.Stop);
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
