using Gfx.RCommData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJDSClient
{
    public static class APPConfig
    {
        static string _section = "WAITCOMBINECofing";

        /// <summary>
        /// 获取特殊区域的站点集合(区域二)
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public static List<int> GetEspAearTwo()
        {
            string key = string.Format("区域二站点集合");

            string read = ConfigHelper.IniReadValue(_section, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = "0,";

                ConfigHelper.IniWriteValue(_section, key, read);
            }

            List<int> result = new List<int>();

            string[] tokens = read.Split(',');

            if (tokens != null)
            {
                int tempNum = 0;

                for (int i = 0; i < tokens.Length; i++)
                {
                    if (Int32.TryParse(tokens[i], out tempNum))
                    {
                        if (!result.Contains(tempNum) && tempNum != 0)
                        {
                            result.Add(tempNum);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 获取特殊区域的站点集合(区域二)
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public static int GetTarRelated(string site)
        {
            string key = string.Format("地标【{0}】对应站点", site);

            string read = ConfigHelper.IniReadValue(_section, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = "0";

                ConfigHelper.IniWriteValue(_section, key, read);
            }

            int tempNum = 0;

            Int32.TryParse(read, out tempNum);

            return tempNum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsSendTaskNow()
        {
            string key = string.Format("派发任务模式");

            string read = ConfigHelper.IniReadValue(_section, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = "1";

                ConfigHelper.IniWriteValue(_section, key, read);
            }

            return ConfigHelper.IniReadValue(_section, key, 100) == "1";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string LogoStr()
        {
            string key = string.Format("公司名称");

            string read = ConfigHelper.IniReadValue(_section, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = "南京德塑";

                ConfigHelper.IniWriteValue(_section, key, read);
            }

            return read; 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        public static bool UserLogin(string user,string pass)
        {
            string sec = "UserConfig";

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass)) { return false; }

            string key = user;

            string read = ConfigHelper.IniReadValue(sec, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = "jtagv";

                ConfigHelper.IniWriteValue(sec, key, read);
            }

            return read == pass; 
        }
    }
}
