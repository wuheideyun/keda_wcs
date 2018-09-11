using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HQHPCtrol
{
    public class LogicAotuSendTar
    {
        List<string> _ignoreSiteList = new List<string>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public LogicAotuSendTar()
        {
            _ignoreSiteList = AppConfig.SendTarIgnoreSite();

            if (_ignoreSiteList == null) { _ignoreSiteList = new List<string>(); }
        }

        /// <summary>
        /// 执行函数
        /// </summary>
        public void DoWork()
        {
            if (!AppConfig.IsSendTarEnable()) { return; }

            List<IDev> devs = IDevManager.IGetDevs();

            if (devs == null) { return; }

            foreach (var item in devs)
            {
                if (item.IsAlive && item.CurTar == "0" && item.IsRunning && item.RunTime >= AppConfig.RunTime())
                {
                    if (!_ignoreSiteList.Contains(item.SiteMark))
                    {
                        item.ISendTar(AppConfig.SendTarNum());
                    }
                }
            }
        }
    }
}
