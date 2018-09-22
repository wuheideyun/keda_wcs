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
        F_PLCLine _plcEnd = new F_PLCLine("PLC0000002");

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
        /// 构造函数
        /// </summary>
        public F_Logic(SynchronizationContext context, ListBox listBoxOutput)
        {
            mainThreadSynContext = context;

            listBox = listBoxOutput;
            _plcHead.Site = "60";

            _plcEnd.Site = "64";
            
            InitToEndWait();

            InitToHeadWait();

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
                Thread.Sleep(500);

                try
                {
                    TaskPlcEndGet();

                    TaskEndToHeadWait();

                    TaskPlcHeadPut();

                    TaskEndToEndWait();
                }
                catch { }
            }
        }


        /// <summary>
        /// 窑尾取货任务
        /// </summary>
        private void TaskPlcEndGet()
        {
            ///窑尾有货 并且 此次任务没有被响应
            if (!_plcEnd.IsLock && _plcEnd.Sta_Material == EnumSta_Material.有货)
            {
                ///派发一个从窑尾装载等待区到窑尾装载点取货的任务
                if (F_DataCenter.MTask.IStartTask(new F_ExcTask(_plcEnd, EnumOper.取货, ConstSetBA.窑尾装载等待区, _plcEnd.Site)))
                {
                    _plcEnd.IsLock = true;

                    sendServerLog("任务：派发一个从窑尾装载等待区到窑尾装载点取货的任务");

                }
            }
        }

        /// <summary>
        /// 窑尾取货完成Agv从窑尾装载点到窑头卸载等待区
        /// </summary>
        private void TaskEndToHeadWait()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(_plcEnd.Site);

            if (agv != null && agv.IsFree)
            {
                F_ExcTask task = new F_ExcTask(null, EnumOper.无动作, _plcEnd.Site, ConstSetBA.窑头卸载等待区);

                task.Id = agv.Id;

                F_DataCenter.MTask.IStartTask(task);

                sendServerLog("任务："+ agv.Id+",窑尾取货完成Agv从窑尾装载点到窑头卸载等待区");

            }
        }


        /// <summary>
        /// 窑头放货任务
        /// </summary>
        private void TaskPlcHeadPut()
        {
            ///窑头无货 并且 此次任务没有被响应
            if (!_plcHead.IsLock && _plcHead.Sta_Material == EnumSta_Material.无货)
            {
                ///派发一个从窑头卸载等待区到窑头卸载点的任务
                if (F_DataCenter.MTask.IStartTask(new F_ExcTask(_plcHead, EnumOper.放货, ConstSetBA.窑头卸载等待区, ConstSetBA.窑头卸载点)))
                {
                    _plcHead.IsLock = true;

                    sendServerLog("任务：派发一个从窑头卸载等待区到窑头卸载点的任务");

                }
            }
        }


        /// <summary>
        /// 窑头卸货完成Agv从窑头卸载点到窑尾装载等待区
        /// </summary>
        private void TaskEndToEndWait()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(_plcHead.Site);

            if (agv != null && agv.IsFree)
            {
                F_ExcTask task = new F_ExcTask(null, EnumOper.无动作, ConstSetBA.窑头卸载点, ConstSetBA.窑尾装载等待区);

                task.Id = agv.Id;

                F_DataCenter.MTask.IStartTask(task);

                sendServerLog("任务：AGV:" + agv.Id + ",从窑头卸载点到窑尾装载等待区");

            }
        }

        /// <summary>
        /// 如果agv有货 回到卸载等待区
        /// </summary>
        private void InitToHeadWait()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(_plcHead.Site);

            if (agv != null)
            {
                F_ExcTask task = new F_ExcTask(null, EnumOper.无动作, agv.Site, ConstSetBA.窑头卸载等待区);

                task.Id = agv.Id;

                F_DataCenter.MTask.IStartTask(task);

                sendServerLog("任务：AGV:" + agv.Id + ",回到窑头卸载等待区");
                
            }
        }

        /// <summary>
        /// 如果agv没货 回到装载等待区
        /// </summary>
        private void InitToEndWait()
        {
            F_AGV agv = F_DataCenter.MDev.IGetDevOnSite(_plcEnd.Site);

            if (agv != null)
            {
                F_ExcTask task = new F_ExcTask(null, EnumOper.无动作, agv.Site, ConstSetBA.窑尾装载等待区);

                task.Id = agv.Id;

                F_DataCenter.MTask.IStartTask(task);

                sendServerLog("任务：AGV:"+agv.Id+ ",回到窑尾装载等待区");

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
