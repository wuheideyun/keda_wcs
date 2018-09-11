using Gfx.GfxDataManagerServer;
using Gfx.RCommData;
using GfxCommonInterfaces;
using GfxServiceContractClient;
using GfxServiceContractTaskExcute;
using JTWcfHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace HQHPCtrol
{
    /// <summary>
    /// 德塑控制
    /// </summary>
    public class LogicCtroCenter
    {
        /// <summary>
        /// 
        /// </summary>
        Thread _threadhande = null;

        /// <summary>
        /// 
        /// </summary>
        LogicWaitPoint _waitManager = new LogicWaitPoint();

        LogicAotuSendTar _aotuSendTarManager = new LogicAotuSendTar();

        /// <summary>
        /// 
        /// </summary>
        public LogicWaitPoint WaitManager
        {
            get { return _waitManager; }
        }

        /// <summary>
        /// 连接状态
        /// </summary>
        bool _mainConnected = false;

        /// <summary>
        /// 连接状态
        /// </summary>
        bool _taskConnected = false;

        /// <summary>
        /// 连接状态
        /// </summary>
        public bool MainConnected
        {
            get { return _mainConnected; }
        }

        /// <summary>
        /// 连接状态
        /// </summary>
        public bool TaskConnected
        {
            get { return _taskConnected; }
        }

        /// <summary>
        /// 
        /// </summary>
        public LogicCtroCenter()
        {
            _threadhande = new Thread(timer1_Tick);

            _threadhande.IsBackground = true;

            _threadhande.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick()
        {
            string IP = "";

            while (true)
            {
                Thread.Sleep(1000);

                try
                {
                    IP = AppConfig.GetIPAdress();

                    JtWcfMainHelper.InitPara(IP, "", "");

                    JtWcfTaskHelper.InitPara(IP, "", "");

                    _mainConnected = JtWcfMainHelper.IsConnected;

                    _taskConnected = JtWcfTaskHelper.IsConnected;

                    IDevManager.ISetDevBacks(JtWcfMainHelper.GetDevList());

                    DoWork();
                }
                catch { }
            }
        }

        /// <summary>
        /// 执行待命点逻辑
        /// </summary>
        private void DoWork()
        {
            try
            {
                _waitManager.DoWork();
            }
            catch { }

            try
            {
                _aotuSendTarManager.DoWork();
            }
            catch { }
        }
    }

    /// <summary>
    /// 位置类型
    /// </summary>
    public enum enumOper
    { 
        上料点,
        下料点,
        待命点
    }
}
