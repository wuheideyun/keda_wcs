using GfxCommonInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NJDSClient
{
    /// <summary>
    /// 货物状态
    /// </summary>
    public enum EnumSta_Material
    {
        有货 = 1,

        无货 = 2,

        传送中 = 3,

        未知
    }

    /// <summary>
    /// 电机状态
    /// </summary>
    public enum EnumSta_Monitor
    {
        正转 = 1,

        反转 = 2,

        停止 = 3,

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
        正向启动 = 1,

        反向启动 = 2,

        辊台停止 = 0,
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
            return JTWcfHelper.WcfMainHelper.SendOrder(_id, new CommonDeviceOrderObj(DeviceOrderTypeEnum.OrderIndexOne, (int)oper, (int)para));
        }
    }
}
