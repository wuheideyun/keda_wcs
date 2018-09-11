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
    /// 设备
    /// </summary>
    public class IDev
    {
        /// <summary>
        /// 
        /// </summary>
        private DeviceBackImf _devBack = null;

        int _runTime = 0;

        /// <summary>
        /// 设备编号
        /// </summary>
        public string DevID
        {
            get
            {
                return _devBack != null ? _devBack.DevId : string.Empty;
            }
        }

        /// <summary>
        /// 地标编号
        /// </summary>
        public string SiteMark
        {
            get
            {
                if (_devBack == null) { return ""; }

                SensorBackImf sen = _devBack.SensorList.Find(c => { return c.SenId == string.Format("{0}{1}", DevID, "0002"); });

                return sen == null ? "" : sen.RValue;
            }
        }

        /// <summary>
        /// 地标编号
        /// </summary>
        public string CurStatue
        {
            get
            {
                if (_devBack == null) { return ""; }

                SensorBackImf sen = _devBack.SensorList.Find(c => { return c.SenId == string.Format("{0}{1}", DevID, "0001"); });

                return sen == null ? "" : sen.RValue;
            }
        }

        /// <summary>
        /// 站点编号
        /// </summary>
        public string CurTar
        {
            get
            {
                if (_devBack == null) { return ""; }

                SensorBackImf sen = _devBack.SensorList.Find(c => { return c.SenId == string.Format("{0}{1}", DevID, "0003"); });

                return sen == null ? "" : sen.RValue;
            }
        }

        /// <summary>
        /// 是否在线
        /// </summary>
        public bool IsAlive
        {
            get
            {
                return _devBack != null ? _devBack.IsAlive : false;
            }
        }

        /// <summary>
        /// 是否启动状态
        /// </summary>
        public bool IsRunning
        {
            get
            {
                if (_devBack == null) { return false; }

                SensorBackImf sen = _devBack.SensorList.Find(c => { return c.SenId == string.Format("{0}{1}", DevID, "0001"); });

                return sen == null ? false : sen.RValue == "1";
            }
        }

        /// <summary>
        /// 处于运行的时间
        /// </summary>
        public int RunTime
        {
            get { return _runTime; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="back"></param>
        public IDev(DeviceBackImf back)
        {
            _devBack = back;
        }

        /// <summary>
        /// 更新设备数据
        /// </summary>
        /// <param name="back"></param>
        public bool SetDevUpdate(DeviceBackImf back)
        {
            if (back == null) { return false; }

            if (_devBack != null)
            {
                if (_devBack.DevId == back.DevId)
                {
                    _devBack = back;

                    if (IsAlive)
                    {
                        if (IsRunning && CurTar == "0")
                        {
                            _runTime++;
                        }
                        else
                        {
                            _runTime = 0;
                        }
                    }
                    else
                    {
                        _runTime = 0;
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                _devBack = back;

                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ISendRun(string source)
        {
            if (_devBack != null && CurStatue == "3")
            {
                JtWcfMainHelper.InitPara(AppConfig.GetIPAdress(),"","");

                ///启动指令
                JtWcfMainHelper.SendOrder(_devBack.DevId, new CommonDeviceOrderObj(source, 1));
            }
        }

        /// <summary>
        /// 发送站点
        /// </summary>
        /// <param name="tar"></param>
        public void ISendTar(int tar)
        {
            if (_devBack != null && CurTar == "0")
            {
                JtWcfMainHelper.InitPara(AppConfig.GetIPAdress(), "", "");

                ///启动指令
                JtWcfMainHelper.SendOrder(_devBack.DevId, new CommonDeviceOrderObj("站点", 3, tar));
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class IDevManager
    {
        /// <summary>
        /// 对象锁
        /// </summary>
        private static object _ans = new object();

        /// <summary>
        /// 设备集合
        /// </summary>
        private static Dictionary<string, IDev> _devDic = new Dictionary<string, IDev>();

        /// <summary>
        /// 更新设备集合
        /// </summary>
        /// <param name="devs"></param>
        public static void ISetDevBacks(List<DeviceBackImf> devs)
        {
            if (devs == null) { return; }

            lock (_ans)
            {
                foreach (var item in devs)
                {
                    if (item.DevType != "AGV") { continue; }

                    if (_devDic.ContainsKey(item.DevId))
                    {
                        _devDic[item.DevId].SetDevUpdate(item);
                    }
                    else
                    {
                        _devDic.Add(item.DevId, new IDev(item));
                    }
                }
            }
        }

        /// <summary>
        /// 获取AGV
        /// </summary>
        /// <param name="dev"></param>
        /// <returns></returns>
        public static IDev IGetDev(string dev)
        {
            if (string.IsNullOrEmpty(dev)) { return null; }

            lock (_ans)
            {
                return _devDic.ContainsKey(dev) ? _devDic[dev] : null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<IDev> IGetDevs()
        {
            lock (_ans)
            {
                return _devDic.Values.ToList();
            }
        }
    }
}
