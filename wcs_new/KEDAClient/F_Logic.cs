using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

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

        /// <summary>
        /// 构造函数
        /// </summary>
        public F_Logic()
        {
            _plcHead.Site = "2";

            _plcEnd.Site = "1";

            _thread = new Thread(ThreadFunc);

            _thread.IsBackground = true;

            _thread.Start();
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
                ///派发一个从窑尾装载等待区到此PLC（窑尾装载点）取货的任务
                if (F_DataCenter.MTask.IStartTask(new F_ExcTask(_plcEnd, EnumOper.取货, EnumSite.窑尾装载等待区.ToString(), _plcEnd.Site)))
                {
                    _plcEnd.IsLock = true;
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
                F_ExcTask task = new F_ExcTask(null, EnumOper.无动作, _plcEnd.Site, EnumSite.窑头卸载等待区.ToString());

                task.Id = agv.Id;

                F_DataCenter.MTask.IStartTask(task);

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
                ///派发一个从窑头卸载等待区到此PLC取货（窑头卸载点）的任务
                if (F_DataCenter.MTask.IStartTask(new F_ExcTask(_plcHead, EnumOper.放货, EnumSite.窑头卸载等待区.ToString(), _plcHead.Site)))
                {
                    _plcHead.IsLock = true;
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
                F_ExcTask task = new F_ExcTask(null, EnumOper.无动作, _plcHead.Site, EnumSite.窑尾装载等待区.ToString());

                task.Id = agv.Id;

                F_DataCenter.MTask.IStartTask(task);

            }
        }
    }
}
