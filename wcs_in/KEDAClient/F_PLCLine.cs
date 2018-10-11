using GfxCommonInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KEDAClient
{
    /// <summary>
    /// 货物状态
    /// </summary>
    public enum EnumSta_Material
    {
        
        未知 ,
        有货 = 1,
        无货 = 2,
        传输中 = 3,

        窑尾无货 = 0 ,        
        窑尾有货 = 1,
        窑尾传输中 =2,    
        窑尾出料完成 =3,
        窑尾1号机械手运动 = 4,
        窑尾1号机械手完成 = 5,
        窑尾2号机械手运动 = 6,
        窑尾2号机械手完成 = 7,

        窑头机械手就绪 =1,
        窑头机械手运动 =2,
        窑头机械手完成 =3,
        窑头无货 =4,
        窑头传输中 =5,
        窑头接料完成 =6,


    }

    /// <summary>
    /// 电机状态
    /// </summary>
    public enum EnumSta_Monitor
    {     

        未知,

        窑尾正常 = 1,

       窑尾电机运行 = 2,

       窑尾电机停止 = 3,

       窑尾1号机械手启动 = 4,

       窑尾1号机械手停止 = 5,

       窑尾2号机械手启动 = 6,

       窑尾2号机械手停止 = 7,

       窑头正常 = 1,

       窑头机械手启动 = 2,

       窑头机械手停止 = 3,

       窑头电机运行 = 4,

       窑头电机停止 = 5,
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

        窑尾辊台启动 = 1,
        窑尾辊台停止 = 2,
        窑尾1号机械手启动 = 3 ,
        窑尾2号机械手启动 = 4,

        窑头辊台启动 = 1,
        窑头辊台停止 = 2,
        窑头机械手启动 = 3,
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
