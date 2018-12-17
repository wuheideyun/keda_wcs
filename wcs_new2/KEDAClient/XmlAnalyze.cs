using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

namespace KEDAClient
{
    /// <summary>
    /// 解析XML配置文档
    /// </summary>
    public class XmlAnalyze
    {
        /// <summary>
        /// xml解析类
        /// </summary>
        private XmlHelper _xmlHepler;

        private List<DisplayConfig> DisplayConfigList;

        private Thread saveThread;
        private DateTime LastSetTime = DateTime.Now;

        /// <summary>
        /// 构造函数
        /// </summary>
        public XmlAnalyze()
        {
            _xmlHepler = new XmlHelper();
            DisplayConfigList = new List<DisplayConfig>();
            DoAnalyze();
        }

        /// <summary>
        /// 解析文档，初始化图像数据
        /// </summary>
        public void DoAnalyze(string fileName = "conf.xml")//"mapconf.xml" 地图配置保存的文件明
        {
            _xmlHepler.CreateOrLoadXMLFile(fileName);

            AnalyzeLines(_xmlHepler.GetXmlNodeList("Display"));

        }

        /// <summary>
        /// 解析线路信息XML
        /// </summary>
        private void AnalyzeLines(XmlNodeList data)
        {
            DisplayConfigList.Clear();
            if (data.Count == 0) return;
            foreach(XmlElement e in data)
            {
                string n = e.GetAttribute("name");
                bool v   = e.GetAttribute("value").Equals("1");
                DisplayConfigList.Add(new DisplayConfig { name = n, value = v });
            }
        }

        /// <summary>
        /// 保存线路信息
        /// </summary>
        private void SaveToFile()
        {
            XmlElement config = _xmlHepler.GetSingleElement("Config");
            config.RemoveAll();

            foreach (var d in DisplayConfigList)
            {
                XmlElement conf = _xmlHepler.CreateElement("Display");
                conf.SetAttribute("name", d.name);
                conf.SetAttribute("value", d.value ? "1":"0");
                _xmlHepler.AddToNode("Config", conf);
            }
            _xmlHepler.SaveXMLFile("conf.xml");
        }

        /// <summary>
        /// 保存配置然后重读配置信息
        /// </summary>
        /// <param name="n"></param>
        /// <param name="v"></param>
        public void SetConfig(string n,bool v)
        {
            DisplayConfig display = DisplayConfigList.Find(c => { return c.name.Equals(n); });
            if (display != null)
            {
                display.value = v;
            }
            else
            {
                DisplayConfigList.Add(new DisplayConfig { name = n, value = v });
            }

            SaveToFile();
            DoAnalyze();
            //if (saveThread == null)
            //{
            //    LastSetTime = DateTime.Now;
            //    saveThread = new Thread(CountToSave);
            //    saveThread.IsBackground = true;
            //    saveThread.Start();
            //}
            //else
            //{
            //    if (!saveThread.IsAlive && (DateTime.Now - LastSetTime).TotalSeconds > 11)
            //    {
            //        LastSetTime = DateTime.Now;
            //        saveThread = new Thread(CountToSave);
            //        saveThread.IsBackground = true;
            //        saveThread.Start();
            //    }
            //    else
            //    {
            //        LastSetTime = DateTime.Now;
            //    }
                    
            //}
        }
        /// <summary>
        /// 保存到配置文件
        /// </summary>
        private void CountToSave()
        {
            while(true)
            {
                if ((DateTime.Now - LastSetTime).TotalSeconds > 10)
                {
                    SaveToFile();
                    DoAnalyze();
                    return;
                }
            }
        }

        /// <summary>
        /// 获取对应名称的显示配置
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public bool GetConfig(string n)
        {
            if (DisplayConfigList.Count == 0) return false;
            DisplayConfig display = DisplayConfigList.Find(c => { return c.name.Equals(n); });
            if (display != null) return display.value;
            return false;
        }
    }
    /// <summary>
    /// 展示配置
    /// </summary>
    public class DisplayConfig
    {
        public string name;
        public bool value;
    }
}
