using FLCommonInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcfHelper;

namespace KEDAClient
{
    /// <summary>
    /// 货物状态
    /// </summary>
    public enum EnumSta_Material
    {
        无货 = 1,

        有货 = 2,

        传送中 = 3,

        允许下料 = 4,

        未知
    }

    /// <summary>
    /// 电机状态
    /// </summary>
    public enum EnumSta_Monitor
    {

        未知,

        电机正转 = 1,

        电机反转 = 2,

        电机停止 = 3,
    }

    /// <summary>
    /// 充电状态
    /// </summary>
    public enum EnumChargeStatus
    {
        正在充电 = 1,

        充电完成 = 2,

        未充电 = 3,

        未知

    }

    /// <summary>
    /// 上下料操作枚举
    /// </summary>
    public enum EnumType
    {
        上料操作 = 1,

        下料操作 = 2,

        其他位置 = 0,
    }

    /// <summary>
    /// 操作参数枚举
    /// </summary>
    public enum EnumPara
    {

        agv上料启动 = 1,
        agv下料启动 = 1,
        agv辊台停止 = 3,

        窑尾辊台允许下料 = 1,
        窑尾辊台禁止下料 = 2,

        窑头辊台上料中 = 1,
        窑头辊台上料完成 = 2,
        窑头辊台其他状态 = 3,
    }

    /// <summary>
    /// 线边滚筒PLC对象
    /// </summary>
    public class F_PLCLine
    {
        /// <summary>
        /// PLC系统ID
        /// </summary>
        string _id = "";

        /// <summary>
        /// 是否被锁
        /// </summary>
        bool _isLock = false;

        /// <summary>
        /// 进充电站是否被锁
        /// </summary>
        bool _isEnterBatteryLock = false;

        /// <summary>
        /// 出充电站是否被锁
        /// </summary>
        bool _isExitBatteryLock = false;

        /// <summary>
        /// 进充电车辆
        /// </summary>
        string _enterChargeAgv = "";

        /// <summary>
        /// 出充电车辆
        /// </summary>
        string _exitChargeAgv = "";

        /// <summary>
        /// 可出站标志
        /// </summary>
        bool _exitFlag = true;

        /// <summary>
        /// 对应地标
        /// </summary>
        string _site = "0";

        /// <summary>
        /// 货物状态
        /// </summary>
        public EnumSta_Material Sta_Material
        {
            get
            {
                EnumSta_Material result = EnumSta_Material.未知;

                try
                {
                    result = (EnumSta_Material)Convert.ToInt32((F_DataCenter.MDev.IGetSenValue(_id, "0001")));
                }
                catch { result = EnumSta_Material.未知; }

                return result;
            }
        }

        /// <summary>
        /// 电机状态
        /// </summary>
        public EnumSta_Monitor Sta_Monitor
        {
            get
            {
                EnumSta_Monitor result = EnumSta_Monitor.未知;

                try
                {
                    result = (EnumSta_Monitor)Convert.ToInt32((F_DataCenter.MDev.IGetSenValue(_id, "0002")));
                }
                catch { result = EnumSta_Monitor.未知; }

                return result;
            }
        }

        /// <summary>
        /// 故障代码
        /// </summary>
        /// <returns></returns>
        public string Error_Code
        {
            get
            {
                return F_DataCenter.MDev.IGetSenValue(_id, "0003");
            }
        }

        /// <summary>
        /// 备用信息
        /// </summary>
        /// <returns></returns>
        public string Tag_Code
        {
            get
            {
                return F_DataCenter.MDev.IGetSenValue(_id, "0004");
            }
        }

        /// <summary>
        /// 是否被锁
        /// </summary>
        public bool IsLock
        {
            get { return _isLock; }
            set { _isLock = value; }
        }

        /// <summary>
        /// 可出站标志
        /// </summary>
        public bool ExitFlag
        {
            get { return _exitFlag; }
            set { _exitFlag = value; }
        }

        /// <summary>
        /// 进窑头、窑尾充电车辆
        /// </summary>
        public string EnterChargeAgv
        {
            get { return _enterChargeAgv; }
            set { _enterChargeAgv = value; }
        }

        /// <summary>
        /// 出窑头、窑尾充电车辆
        /// </summary>
        public string ExitChargeAgv
        {
            get { return _exitChargeAgv; }
            set { _exitChargeAgv = value; }
        }

        /// <summary>
        /// 进窑头、窑尾充电桩是否被锁
        /// </summary>
        public bool IsEnterBatteryLock
        {
            get { return _isEnterBatteryLock; }
            set { _isEnterBatteryLock = value; }
        }

        /// <summary>
        /// 出窑头、窑尾充电桩是否被锁
        /// </summary>
        public bool IsExitBatteryLock
        {
            get { return _isExitBatteryLock; }
            set { _isExitBatteryLock = value; }
        }

        /// <summary>
        /// 对应地标
        /// </summary>
        public string Site
        {
            get { return _site; }
            set { _site = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        public F_PLCLine(string id)
        {
            _id = id;
        }

        /// <summary>
        /// 给棍台发送相应动作
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public bool SendOrdr(EnumType oper, EnumPara para)
        {
            return WcfMainHelper.SendOrder(_id, new FControlOrder("远程", 1, (int)oper, (int)para));
        }
    }
}
