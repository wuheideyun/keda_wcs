using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XMLHelper;

namespace DispatchAnmination
{
    /// <summary>
    /// AGV站点，进行进度更新类
    /// </summary>
    public class AgvSiteMaster
    {
        private static List<AgvSiteRate> AgvSiteList = new List<AgvSiteRate>();
        public static void AddAgvSiteRate(string name, int site, float rate)
        {
            AgvSiteRate agvSiteRate = AgvSiteList.Find(c => { return c.AgvName.Equals(name); });
            if (agvSiteRate == null)
            {
                AgvSiteList.Add(new AgvSiteRate(name, site, rate));
            }
        }
        public static void UpDateAgv(string name, int site = 0)
        {
            AgvSiteRate agvSiteRate = AgvSiteList.Find(c => { return c.AgvName.Equals(name); });
            if (agvSiteRate == null)
            {
                AgvSiteRate agv = new AgvSiteRate(name, 23, 0);
                AgvSiteList.Add(agv);
                ModuleControl.UpdateAgvSite(name, 23, 0);
            }
            else
            {
                ModuleControl.UpdateAgvSite(name, agvSiteRate.GetSite(site), agvSiteRate.GetRate());
            }

        }
    }


    /// <summary>
    /// AGV站点进度记录类
    /// </summary>
    public class AgvSiteRate
    {
        public string AgvName;
        public int Site;
        private float Rate = 0;
        private int LineIndex = 0;


        public AgvSiteRate()
        {
        }
        public AgvSiteRate(string name, int site = 23, float rate = 0)
        {
            AgvName = name;
            Site = site;
            Rate = rate;
            if (site != 23)
            {
                if (lines.Contains(site))
                {
                    LineIndex = GetLineIndex(site);
                }
                else
                {
                    Rate = 23;
                    LineIndex = 0;
                }
            }
        }
        private static int GetLineIndex(int site)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                if (site == lines[i])
                {
                    return i;
                }
            }
            return 0;
        }


        /// <summary>
        /// 所有地标
        /// </summary>
        private static int[] lines = { 23, 33, 15, 13, 21, 11, 22, 34, 24, 14 };
        public float GetRate()
        {
            Rate = Rate + LineMoveSize.GetLineMoveSize(lines[LineIndex]);
            if (Rate < 100)
            {
                return Rate;
            }
            else
            {
                LineIndex = LineIndex == lines.Length - 1 ? 0 : LineIndex + 1;
                Rate = 0;
                return 100;
            }
        }

        /// <summary>
        /// 获取当前
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public int GetSite(int site)
        {
            if (site != 0)
            {
                if (lines[LineIndex] != site)
                {
                    LineIndex = GetLineIndex(site);
                    Rate = 0;
                }
            }
            return lines[LineIndex];
        }
    }
}
