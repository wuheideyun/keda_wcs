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
    public class WaitPointMember
    {
        /// <summary>
        /// ID
        /// </summary>
        private int _idIndex = -1;

        /// <summary>
        /// 绑定设备
        /// </summary>
        private IDev _bandingDev = null;

        /// <summary>
        /// 地标
        /// </summary>
        public string SiteMark
        {
            get { return AppConfig.GetWaitRelateSiteMark(_idIndex); }
        }

        /// <summary>
        /// 当AGV等于这些地标时，需要释放
        /// </summary>
        public List<string> ReleaseSiteMarkList
        {
            get { return AppConfig.GetRealseSiteMark(_idIndex); }
        }

        /// <summary>
        /// 绑定设备
        /// </summary>
        public IDev BandingDev
        {
            get { return _bandingDev; }
            set { _bandingDev = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        public WaitPointMember(int id)
        {
            _idIndex = id;
        }

        /// <summary>
        /// 清除绑定
        /// </summary>
        public void ClearBanding()
        {
            if (_bandingDev != null)
            {
                if (_bandingDev.SiteMark != SiteMark || !_bandingDev.IsAlive)
                {
                    _bandingDev = null;
                }
            }
        }

        /// <summary>
        /// AGV地标与待命点一致且AGV在线,则绑定
        /// </summary>
        /// <param name="dev"></param>
        public void CheckBangding(IDev dev)
        {
            if (dev != null)
            {
                if (_bandingDev == null)
                {
                    if (dev.SiteMark == SiteMark && dev.IsAlive)
                    {
                        _bandingDev = dev;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void AotuReset()
        { 
            
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class LogicWaitPoint
    {
        WaitPointMember _WP1 = new WaitPointMember(1);

        WaitPointMember _WP2 = new WaitPointMember(2);

        WaitPointMember _WP3 = new WaitPointMember(3);

        WaitPointMember _WP4 = new WaitPointMember(4);

        WaitPointMember _WP5 = new WaitPointMember(5);

        WaitPointMember _WP6 = new WaitPointMember(6);

        WaitPointMember _WP7 = new WaitPointMember(7);

        public Dictionary<int, WaitPointMember> WaitDic = new Dictionary<int, WaitPointMember>();

        List<IDev> _devList = new List<IDev>();

        /// <summary>
        /// 
        /// </summary>
        public LogicWaitPoint()
        {
            WaitDic.Add(1, _WP1);

            WaitDic.Add(2, _WP2);

            WaitDic.Add(3, _WP3);

            WaitDic.Add(4, _WP4);

            WaitDic.Add(5, _WP5);

            WaitDic.Add(6, _WP6);

            WaitDic.Add(7, _WP7);
        }

        /// <summary>
        /// 
        /// </summary>
        private void FunThread_Tick()
        {
            #region
            ///要是待命点2上有AGV
            if (_WP1.BandingDev == null)
            {
                _WP1.BandingDev = _WP2.BandingDev;

                _WP2.BandingDev = null;
            }



            ///要是待命点3上有AGV
            if (_WP2.BandingDev == null)
            {
                _WP2.BandingDev = _WP3.BandingDev;

                _WP3.BandingDev = null;
            }

            ///要是待命点4上有AGV
            if (_WP3.BandingDev == null)
            {
                _WP3.BandingDev = _WP4.BandingDev;

                _WP4.BandingDev = null;
            }


            ///要是待命点6上有AGV
            if (_WP5.BandingDev == null)
            {
                _WP5.BandingDev = _WP6.BandingDev;

                _WP6.BandingDev = null;
            }

            ///要是待命点6上有AGV
            if (_WP5.BandingDev != null)
            {
                if (_WP2.BandingDev == null && _WP3.BandingDev == null && _WP4.BandingDev == null)
                {
                    _WP2.BandingDev = _WP5.BandingDev;

                    _WP5.BandingDev = null;
                }
            }
            #endregion

            if (_WP7.BandingDev != null)
            {
                ///要是待命点7上有AGV
                if (_WP6.BandingDev == null)
                {
                    _WP6.BandingDev = _WP7.BandingDev;

                    _WP7.BandingDev = null;
                }
                else
                {
                    if (_WP4.BandingDev == null)
                    {
                        _WP4.BandingDev = _WP5.BandingDev;

                        _WP5.BandingDev = _WP6.BandingDev;

                        _WP6.BandingDev = null;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DoWork()
        {
            ///清除无效绑定
            ClearOutDate();

            _devList = IDevManager.IGetDevs();

            ///设置绑定设备
            foreach (var item in _devList)
            {
                ///绑定的临近地标
                CheckInDate(item);
            }

            FunThread_Tick();

            ///当AGV处于待命地标且不等于绑定点的地标时，AGV自启动
            foreach (var item in WaitDic.Values)
            {
                if (item.BandingDev != null)
                {
                    if (IsDevOnWaitSite(item.BandingDev))
                    {
                        if (item.BandingDev.SiteMark != item.SiteMark)
                        {
                            if (item == _WP1)
                            {
                                item.BandingDev.ISendTar(AppConfig.GetWPTarNum());
                            }

                            item.BandingDev.ISendRun("处于待命地标但不等于目的地");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 清除无效绑定
        /// </summary>
        public void ClearOutDate()
        {
            List<DispatchBackMember> backs =  JtWcfMainHelper.GetDispatchList();
           
            foreach (var item in WaitDic.Values)
            {
                if (item.BandingDev != null)
                {
                    if (backs != null)
                    {
                        ////当AGV已经处于某个调度时，绑定AGV无效
                        if (backs.Find(c => { return c.DisDevId == item.BandingDev.DevID; }) != null)
                        {
                            item.BandingDev = null;
                        }
                        else 
                        {
                            ///当设备处于释放地标时，绑定设备无效
                            if (item.ReleaseSiteMarkList.Contains(item.BandingDev.SiteMark))
                            {
                                item.BandingDev = null;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 添加新绑定
        /// </summary>
        public void CheckInDate(IDev dev)
        {
            if (!DevIsInBangding(dev))
            {
                foreach (var item in WaitDic.Values)
                {
                    item.CheckBangding(dev);
                }
            }
        }

        /// <summary>
        /// 判断设备是否处于绑定中
        /// </summary>
        /// <param name="dev"></param>
        /// <returns></returns>
        public bool DevIsInBangding(IDev dev)
        {
            foreach (var item in WaitDic.Values)
            {
                if (item.BandingDev == dev)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 判断设备是否处于待命地标上
        /// </summary>
        /// <param name="dev"></param>
        /// <returns></returns>
        public bool IsDevOnWaitSite(IDev dev)
        {
            foreach (var item in WaitDic.Values)
            {
                if (item.SiteMark == dev.SiteMark)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
