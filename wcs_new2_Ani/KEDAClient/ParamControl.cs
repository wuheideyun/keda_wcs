using ArrayMap;
using DataContract;
using DispatchAnmination;
using DispatchAnmination.Const;
using FLCommonInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ArrayMap;

namespace KEDAClient
{
    /// <summary>
    /// 处理AGV的获取数据并且更新当前地图上的信息
    /// </summary>
    public class AGVDataMaster
    {
        private static List<AGV> AGVS = new List<AGV>();
        public static void UpdateAgvs(List<DeviceBackImf> list)
        {
            AGVS.Clear();
            foreach(DeviceBackImf imf in list)
            {
                AGVS.Add(new AGV(imf));
            }
            if (ConstBA.Init_ShowLineAGV) InitAgvMapSpan();
            UpdateAgvMap();
        }
        public static bool InitAgvShowOffLine = true;
        private static MapList map = new MapList();
        private static string value;
        private static int GetRate(string name)
        {
           value =  map.GetStringValue(name);
            if(value!=null && !value.Equals(""))
            {
                int rate =0;
                try
                {
                    rate = int.Parse(value) + 10;
                    
                }
                catch
                {

                }
                map.Put(name, rate + "");
                return rate;

            }
            map.Put(name, "0");
            return 0;
        }

        /// <summary>
        /// 相同地标的AGV地图上分开距离
        /// </summary>
        private static void InitAgvMapSpan()
        {
            foreach (AGV agv in AGVS)
            {
                if (agv.IsAlive && agv.IsAgvDev)
                {//在线停止中
                    ModuleControl.AddAgvToMapNew(agv.IsAlive,agv.Name, agv.Site, agv.Point, GetRate(agv.Site+""));
                }

                if (ConstBA.IsShow_OffLineAGV && !agv.IsAlive && agv.IsAgvDev)
                {//显示离线AGV
                    ModuleControl.AddAgvToMapNew(agv.IsAlive, agv.Name, agv.Site, agv.Point, GetRate(agv.Site + ""));
                }
            }
            ConstBA.Init_ShowLineAGV = false;
        }

        public static void UpdateAgvMap()
        {
            
            foreach(AGV agv in AGVS)
            {
                if (agv.IsAlive && agv.IsAgvDev &&agv.IsRunning)
                {//在线运动中
                    ModuleControl.UpdateAgvSiteNew(agv.IsAlive, agv.Name, agv.Site, agv.Point);//走动
                }
                else if(agv.IsAlive && agv.IsAgvDev && !agv.IsRunning)
                {//在线停止中
                    ModuleControl.AddAgvToMapNew(agv.IsAlive, agv.Name, agv.Site, agv.Point, 0);
                }
                else if (ConstBA.IsShow_OffLineAGV && !agv.IsAlive && agv.IsAgvDev)
                {//显示离线AGV
                    ModuleControl.AddAgvToMapNew(agv.IsAlive, agv.Name, agv.Site, agv.Point, 0);
                }
                else if (!ConstBA.IsShow_OffLineAGV && !agv.IsAlive && agv.IsAgvDev)
                {//移除离线AGV
                    ModuleControl.RemoveAgvModule(agv.Name);
                }
            }
        }

        public bool IsShowAgv(AGV agv)
        {

            if (!agv.IsAgvDev) return false;
            if (agv.IsAlive) return false;
            if (agv.IsReady) return false;



            return false;
        }
    }

    

    /// <summary>
    /// 用于获取AGV的详细信息
    /// </summary>
    public class AGV
    {
        private DeviceBackImf _dev;
        private ProtyBackImf _devR;
        private string _value;

        public bool IsAlive
        {
            get
            {
                return _dev.IsAlive;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dev"></param>
        public AGV(DeviceBackImf dev)
        {
            _dev = dev;
        }

        public string Name
        {
            get
            {
                return _dev.DevId;
            }
            
        }

        /// <summary>
        /// 目标站点
        /// </summary>
        /// <returns></returns>
        public int Point
        {
            get
            {
                _value = GetRValue("0003");
                try
                {
                    return int.Parse(_value);
                }
                catch
                {

                }
                return 0;
            }
            
        }

        /// <summary>
        /// 当前所在站点
        /// </summary>
        /// <returns></returns>
        public int NowPoint
        {
            get
            {
                _value = GetRValue("0027");
                try
                {
                    return int.Parse(_value);
                }
                catch
                {

                }
                return 0;
            }
           
        }

        /// <summary>
        /// 地标
        /// </summary>
        /// <returns></returns>
        public int Site
        {
            get
            {
                _value = GetRValue("0002");
                try
                {
                    return int.Parse(_value);
                }
                catch
                {

                }
                return 0;
            }
            
        }

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetRValue(string id)
        {
            _devR = _dev.IGet(id);
            return _devR != null ? _devR.RValue : "";
        }

        /// <summary>
        /// 获取电量
        /// </summary>
        /// <returns></returns>
        public string Electicity()
        {
            return GetRValue("0007");
        }

        /// <summary>
        /// 获取运行方向
        /// </summary>
        /// <returns></returns>
        public string Direction()
        {
            try
            {
                int status = Convert.ToInt32(GetRValue("0005"));
                switch (status)
                {
                    case 0:
                        _value = "前进";
                        break;
                    case 1:
                        _value = "后退";
                        break;
                    default:
                        _value = "未知";
                        break;
                }
            }
            catch
            {
                _value = "未知";
            }
            return _value;
        }

        /// <summary>
        /// 是否在运动中
        /// </summary>
        /// <returns></returns>
        public bool IsRunning
        {
            get
            {
                _value = GetRValue("0001");
                return !_value.Equals("") && _value.Equals("3") ? false : true;
            }
            
        }        
        
        public bool IsAgvDev
        {
            get
            {
                return _dev.DevType.Equals("Magnet_Basic");
            }
        }

        /// <summary>
        /// 充电状态
        /// </summary>
        /// <returns></returns>
        public bool ChargeStatus
        {
            get
            {
                _value = GetRValue("0008");
                return _value!=null && _value.Equals("1") ? true:false;
            }
           
        }

        /// <summary>
        /// 空闲状态
        /// </summary>
        /// <returns></returns>
        public bool FreeStatus
        {
            get
            {
                _value = GetRValue("0010");
                return _value != null && _value.Equals("False") ? false : true;
            }
            
        }

        /// <summary>
        /// Agv状态
        /// </summary>
        /// <returns></returns>
        public string AgvStatus()
        {
            return GetDevStatus(_dev);
        }

        /// <summary>
        /// 速度
        /// </summary>
        /// <returns></returns>
        public int Speed
        {
            get
            {
                _value = GetRValue("0004");
                try
                {
                    return int.Parse(_value);
                }
                catch
                {

                }
                return 0;
            }
           
        }

        /// <summary>
        /// 货物状态
        /// </summary>
        /// <returns></returns>
        public string Sta_Material()
        {
            try
            {
                int status = Convert.ToInt32(GetRValue("0036"));
                switch (status)
                {
                    case 1:
                        _value = "有货";
                        break;
                    case 2:
                        _value = "无货";
                        break;
                    case 3:
                        _value = "传送中";
                        break;
                    default:
                        _value = "未知";
                        break;
                }
            }
            catch
            {
                _value = "未知";
            }
            return _value;
        }

        /// <summary>
        /// 电机状态
        /// </summary>
        /// <returns></returns>
        public string Sta_Monitor()
        {
            try
            {
                int status = Convert.ToInt32(GetRValue("0037"));
                switch (status)
                {
                    case 1:
                        _value = "电机正转";
                        break;
                    case 2:
                        _value = "电机反转";
                        break;
                    case 3:
                        _value = "电机停止";
                        break;
                    default:
                        _value = "未知";
                        break;
                }
            }
            catch
            {
                _value = "未知";
            }
            return _value;
        }

        /// <summary>
        /// 是否准备
        /// </summary>
        /// <returns></returns>
        public bool IsReady
        {
            get
            {
                _value = GetRValue("0028");
                return !_value.Equals("") && _value.Equals("0") ? false : true;
            }
        }

        /// <summary>
        /// 在轨道
        /// </summary>
        public bool OnTrack
        {
            get
            {
                _value = GetRValue("0029");
                return !_value.Equals("") && _value.Equals("0") ? false : true;
            }
            
        }

        /// <summary>
        /// 交管状态
        /// </summary>
        /// <returns></returns>
        public bool Traffic
        {
            get
            {
                _value = GetRValue("T01");
                return !_value.Equals("") && _value.Equals("False") ? false : true;//GetRValue("T02")
            }
            
        }

        /// <summary>
        /// 获取设备状态
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetDevStatus(DeviceBackImf item)
        {
            string status = "";
            if (!item.IsAlive)
            {
                status = "离线";
            }
            else if (item.DevType == "WK_PLC")
            {
                status = "在线";
            }
            else if (item.ProtyList[ConstSetBA.在轨道上].RValue == "0")
            {
                status = "脱轨";
            }
            else if (item.ProtyList[ConstSetBA.运行状态].RValue == "2")
            {
                status = "障碍物";
            }
            else if (item.ProtyList[ConstSetBA.交管状态].RValue == "True")
            {
                status = "被交管(" + item.ProtyList[ConstSetBA.交管设备].RValue + ")";
            }
            else if (item.ProtyList[ConstSetBA.空闲].RValue == "True")// && !F_DataCenter.MDev.IsDevInDispath(item.DevId))
            {
                status = "空闲";
            }
            else if (item.ProtyList[ConstSetBA.充电状态].RValue == "1")
            {
                status = "充电中";
            }
            else
            {
                status = "任务中";
            }
            return status;
        }

    }


    /// <summary>
    /// 用于获取PLC的详细信息
    /// </summary>
    public class PLC
    {
        private DeviceBackImf _dev;
        private ProtyBackImf _devR;
        private string _value;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dev"></param>
        public PLC(DeviceBackImf dev)
        {
            _dev = dev;
        }

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetRValue(string id)
        {
            _devR = _dev.IGet(id);
            return _devR != null ? _devR.RValue : "";
        }

        /// <summary>
        /// 货物状态
        /// </summary>
        /// <returns></returns>
        public string Sta_Material()
        {
            try
            {
                int status = Convert.ToInt32(GetRValue("0001"));
                switch (status)
                {
                    case 1:
                        _value = "有货";
                        break;
                    case 2:
                        _value = "无货";
                        break;
                    case 3:
                        _value = "传送中";
                        break;
                    default:
                        _value = "未知";
                        break;
                }
            }
            catch
            {
                _value = "未知";
            }
            return _value;
        }

        /// <summary>
        /// 电机状态
        /// </summary>
        /// <returns></returns>
        public string Sta_Monitor()
        {
            try
            {
                int status = Convert.ToInt32(GetRValue("0002"));
                switch (status)
                {
                    case 1:
                        _value = "电机正转";
                        break;
                    case 2:
                        _value = "电机反转";
                        break;
                    case 3:
                        _value = "电机停止";
                        break;
                    default:
                        _value = "未知";
                        break;
                }
            }
            catch
            {
                _value = "未知";
            }
            return _value;
        }

        /// <summary>
        /// 故障状态
        /// </summary>
        /// <returns></returns>
        public string Sta_Error()
        {
            return GetRValue("0003");
        }

        /// <summary>
        /// 备用信息
        /// </summary>
        /// <returns></returns>
        public string SpareInform()
        {
            return GetRValue("0004");
        }

        /// <summary>
        /// PLC位置
        /// </summary>
        /// <returns></returns>
        public string PLCLocat()
        {
            return PLC.GetDevLocat(_dev);
        }

        /// <summary>
        /// 获取PLC位置
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetDevLocat(DeviceBackImf item)
        {
            string status = "";
            if (item.DevId == "PLC01")
            {
                status = "窑头";
            }
            else if (item.DevId == "PLC02")
            {
                status = "窑尾";
            }
            else
            {
                status = "未知";
            }
            return status;
        }

    }
}
