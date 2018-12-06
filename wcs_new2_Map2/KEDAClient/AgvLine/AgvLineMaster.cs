using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XMLHelper;

namespace DispatchAnmination.AgvLine
{
    /// <summary>
    /// 获取AGV在地图上的坐标
    /// </summary>
    public class AgvLineMaster
    {
        /// <summary>
        /// 保存所以线路的信息
        /// </summary>
        private static List<AgvLineData> AgvLineList = new List<AgvLineData>();
        

        public static void AddLine(List<AgvLineData> lineList)
        {
            AgvLineList.Clear();
            AgvLineList = lineList;
        }
        private static AgvLineData agvLine;

        /// <summary>
        /// 获取Agv地标
        /// </summary>
        /// <param name="nowsite"></param>
        /// <param name="dessite"></param>
        /// <returns></returns>
        public static AgvPoint GetMPointOnLine(string name,int nowsite,int dessite,float rate=-1)
        {
            nowsite = AgvOnLineMaster.GetNowSite(name, nowsite);
            agvLine = AgvLineList.Find(c => { return c.NowSite == nowsite 
                && ((c.IsSpecial && c.DesSite == dessite)||(!c.IsSpecial)); });

            if (agvLine != null)
            {
                return agvLine.GetAgvLinePoint(name,rate);
            }
            return null;
        }

        
    }

    public class AgvOnLineMaster
    {
        /// <summary>
        /// 加减速度点 忽略，获取上一个点的线路
        /// </summary>
        private static List<int> IgnoreSiteList = new List<int>
        {
            42,43,45,46
        };
        private static List<AgvLineInfo> agvs = new List<AgvLineInfo>();
        private static AgvLineInfo agv;
        public static int GetNowSite(string name,int nowsite)
        {
            agv = agvs.Find(c => { return c.Name.Equals(name); });
            if (agv == null)
            {
                agv = new AgvLineInfo { Name = name, LastSite = nowsite };
                agvs.Add(agv);
            }

            if (IgnoreSiteList.Contains(nowsite))
            {
                return agv.LastSite;
            }
            else
            {
                agv.LastSite = nowsite;
                return nowsite;
            }
        }
    }

    /// <summary>
    /// 保存AGV名称，上一个地标
    /// </summary>
    public class AgvLineInfo
    {
        public string Name;
        public int LastSite;
    }
}
