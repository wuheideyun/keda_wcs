using GfxAgvMapEx;
using GfxServiceContractClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NJDSClient
{
    /// <summary>
    /// 兼容地图，中间类
    /// </summary>
    public class AgvData : IAgvData
    {
        /// <summary>
        /// AGV编号
        /// </summary>
        public int AgvNum
        {
            get;
            set;
        }

        /// <summary>
        /// 是否在线
        /// </summary>
        public bool IsOnLine
        {
            get;
            set;
        }

        /// <summary>
        /// AGV状态
        /// </summary>
        public byte AgvState
        {
            get;
            set;
        }

        /// <summary>
        /// 地标编号
        /// </summary>
        public int DbNum
        {
            get;
            set;
        }

        /// <summary>
        /// 目标站点
        /// </summary>
        public int Target
        {
            get;
            set;
        }

        public AgvData(int agvNum, bool isOnLine, byte agvState, int dbNum, int target)
        {
            AgvNum = agvNum;

            IsOnLine = isOnLine;

            AgvState = agvState;

            DbNum = dbNum;

            Target = target;
        }

        public AgvData(DeviceBackImf dev)
        {
            try
            {
                //string devNum = dev.DevId.Substring(dev.DevId.Length - 4);

                AgvNum = Convert.ToInt32(dev.DevId.Substring(dev.DevId.Length - 4));

                IsOnLine = Convert.ToBoolean(dev.DevStatue);

                if (!IsOnLine) { return; }

                //devNum = string.Format("{0}{1}", dev.DevId.Substring(0, 2), devNum);

                var sensor = dev.SensorList.Find(c => { return c.SenId == string.Format("{0}0001", dev.DevId); });

                //var sensor = dev.SensorList.Find(c => { return c.SenId == string.Format("{0}0001", devNum); });

                if (null != sensor) { AgvState = Convert.ToByte(sensor.RValue); }

                sensor = dev.SensorList.Find(c => { return c.SenId == string.Format("{0}0002", dev.DevId); });

                if (null != sensor) { DbNum = Convert.ToInt32(sensor.RValue); }

                sensor = dev.SensorList.Find(c => { return c.SenId == string.Format("{0}0003", dev.DevId); });

                if (null != sensor) { Target = Convert.ToInt32(sensor.RValue); }
            }
            catch
            {
                IsOnLine = false;
            }
        }

        public AgvData(DeviceBackImf dev, bool isOnLine)
        {
            try
            {
                string devNum = dev.DevId.Substring(dev.DevId.Length - 4);

                AgvNum = Convert.ToInt32(devNum);

                IsOnLine = isOnLine;

                if (!IsOnLine) { return; }

                devNum = string.Format("{0}{1}", dev.DevId.Substring(0, 2), devNum);

                var sensor = dev.SensorList.Find(c => { return c.SenId == string.Format("{0}0001", devNum); });

                if (null != sensor) { AgvState = Convert.ToByte(sensor.RValue); }

                sensor = dev.SensorList.Find(c => { return c.SenId == string.Format("{0}0002", devNum); });

                if (null != sensor) { DbNum = Convert.ToInt32(sensor.RValue); }

                sensor = dev.SensorList.Find(c => { return c.SenId == string.Format("{0}0003", devNum); });

                if (null != sensor) { Target = Convert.ToInt32(sensor.RValue); }
            }
            catch
            {
                IsOnLine = false;
            }
        }
    }
}
