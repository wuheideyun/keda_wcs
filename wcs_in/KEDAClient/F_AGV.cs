using GfxCommonInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KEDAClient
{

    /// <summary>
    ///运行状态
    /// </summary>
    public enum EnumSta_AGV
    {
        运行 = 1,

        暂停 = 2,

        停止 = 3,

        未知
    }

    /// <summary>
    /// 线边滚筒PLC对象
    /// </summary>
    public class F_AGV
    {
        /// <summary>
        /// 是否被锁
        /// </summary>
        bool _isLock = false;


        /// <summary>
        /// PLC系统ID
        /// </summary>
        string _id = "";
        public string Id
        {
            get { return _id; }
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
        /// 货物状态
        /// </summary>
        public EnumSta_Material Sta_Material
        {
            get 
            {
                EnumSta_Material result = EnumSta_Material.未知;

                try
                {
                    result = (EnumSta_Material)Convert.ToInt32((F_DataCenter.MDev.IGetSenValue(_id, "0036")));
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
                    result = (EnumSta_Monitor)Convert.ToInt32((F_DataCenter.MDev.IGetSenValue(_id, "0037")));
                }
                catch { result = EnumSta_Monitor.未知; }

                return result;
            }
        }

        /// <summary>
        /// 当前地标
        /// </summary>
        /// <returns></returns>
        public string Site
        {
            get 
            {
                return F_DataCenter.MDev.IGetSenValue(_id, "0002");
            }
        }

        /// <summary>
        /// AGV电量
        /// </summary>
        /// <returns></returns>
        public int Electicity
        {
            get
            {
                return Convert.ToInt32(F_DataCenter.MDev.IGetSenValue(_id, "0007"));
            }
        }

        /// <summary>
        /// 充电状态
        /// </summary>
        /// <returns></returns>
        public EnumChargeStatus ChargeStatus
        {
            get
            {
                EnumChargeStatus result = EnumChargeStatus.未知;

                try
                {
                    result = (EnumChargeStatus)Convert.ToInt32((F_DataCenter.MDev.IGetSenValue(_id, "0008")));
                }
                catch { result = EnumChargeStatus.未知; }

                return result;
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
        /// AGV是否处于空闲状态
        /// </summary>
        public bool IsFree
        {
            get
            {
                return F_DataCenter.MDev.IGetSenValue(_id, "0010") == "true" && !(F_DataCenter.MDev.IsDevInDispath(_id));
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        public F_AGV(string id)
        {
            _id = id;
        }

        /// <summary>
        /// 给车载PLC发送相应动作
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public bool SendOrdr(EnumType oper, EnumPara para)
        {
            return JTWcfHelper.WcfMainHelper.SendOrder(_id, new CommonDeviceOrderObj(DeviceOrderTypeEnum.OrderIndexEleven, (int)oper, (int)para));
        }
    }
}
