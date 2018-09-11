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
    /// 
    /// </summary>
    public static class AppConfig
    {
        /// <summary>
        /// 获取配置好的地址
        /// </summary>
        /// <returns></returns>
        public static string GetIPAdress()
        {
            string section = string.Format("ConfigIPAdress");

            string key = string.Format("SeverIPAdress");

            string read = ConfigHelper.IniReadValue(section, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = "127.0.0.1";

                ConfigHelper.IniWriteValue(section, key, read);
            }

            return read;
        }

        /// <summary>
        /// 获取待命点序列对应的地标号
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GetWaitRelateSiteMark(int num)
        {
            string section = string.Format("ConfigWaitSite");

            string key = string.Format("{0}", num);

            string read = ConfigHelper.IniReadValue(section, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = "0";

                ConfigHelper.IniWriteValue(section, key, read);
            }

            return read;
        }

        /// <summary>
        /// 获取释放绑定的地标
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static List<string> GetRealseSiteMark(int num)
        {
            string section = string.Format("ConfigRealse");

            string key = string.Format("{0}", num);

            string read = ConfigHelper.IniReadValue(section, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = "0";

                ConfigHelper.IniWriteValue(section, key, read);
            }

            return read.Split(',').ToList();
        }

        /// <summary>
        /// 获取任务所对应的站点
        /// </summary>
        /// <param name="type"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public static int GetStationNum(enumOper oper, string type, string site)
        {
            int result = 0;

            string section = string.Format("StationConfig【{0}】", oper);

            string key = string.Format("【{0}】【{1}】", type, site);

            string read = ConfigHelper.IniReadValue(section, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = "0";

                ConfigHelper.IniWriteValue(section, key, read);
            }

            Int32.TryParse(read, out result);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int GetWPTarNum()
        {
            int result = 0;

            string section = string.Format("StationTarConfig");

            string key = string.Format("待命点1站点");

            string read = ConfigHelper.IniReadValue(section, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = "30";

                ConfigHelper.IniWriteValue(section, key, read);
            }

            Int32.TryParse(read, out result);

            return result;
        }

        /// <summary>
        /// 是否启用自动发送站点逻辑
        /// </summary>
        public static bool IsSendTarEnable()
        {
            string section = string.Format("ConfigAotuSendTar");

            string key = string.Format("是否启用");

            string read = ConfigHelper.IniReadValue(section, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = "1";

                ConfigHelper.IniWriteValue(section, key, read);
            }

            return read == "1";
        }

        /// <summary>
        /// 是否启用自动发送站点逻辑
        /// </summary>
        public static List<string> SendTarIgnoreSite()
        {
            string section = string.Format("ConfigAotuSendTar");

            string key = string.Format("不受控地标");

            string read = ConfigHelper.IniReadValue(section, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = "0";

                ConfigHelper.IniWriteValue(section, key, read);
            }

            return read.Split(',').ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int SendTarNum()
        {
            string section = string.Format("ConfigAotuSendTar");

            string key = string.Format("站点号");

            string read = ConfigHelper.IniReadValue(section, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = "28";

                ConfigHelper.IniWriteValue(section, key, read);
            }

            int result = 0;

            Int32.TryParse(read, out result);

            return result;
        }

        /// <summary>
        /// 运行时间
        /// </summary>
        /// <returns></returns>
        public static int RunTime()
        {
            string section = string.Format("ConfigAotuSendTar");

            string key = string.Format("运行时间");

            string read = ConfigHelper.IniReadValue(section, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = "10";

                ConfigHelper.IniWriteValue(section, key, read);
            }

            int result = 0;

            Int32.TryParse(read, out result);

            return result;
        }
    }
}
