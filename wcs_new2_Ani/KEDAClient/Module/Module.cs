using DispatchAnmination.AgvLine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using XMLHelper;

namespace DispatchAnmination
{
    /// <summary>
    /// 模型数据控制类
    /// 用于转换xml数据到对应的Module数据
    /// </summary>
    public class ModuleControl
    {
        
        internal static List<AgvModule> _agvModules = new List<AgvModule>();

        internal static List<PlcModule> _plcModules = new List<PlcModule>();

        internal static List<LineModule> _lineModules= new List<LineModule>();
        
        private static AgvPoint agvPoint;
        /// <summary>
        /// 新更新AGV坐标方法方法
        /// </summary>
        /// <param name="name"></param>
        /// <param name="siteid"></param>
        /// <param name="rate"></param>
        /// <param name="dessite"></param>
        public static void UpdateAgvSiteNew(bool isalive,string name, int siteid, int dessite = 0, float rate =-1)
        {
            AgvModule agv = _agvModules.Find(c => { return c._name.Equals(name); });

            if (agv != null)
            {
                agv.UpdateAgvStatus(isalive);
                agvPoint = AgvLineMaster.GetMPointOnLine(name,siteid, dessite, rate);
                if (agvPoint != null)
                {
                    agv.Update(new Point(agvPoint.X, agvPoint.Y));
                }
            }
        }

        public static void AddAgvNotMoving(string name, int siteid, int dessite = 0, float rate = -1)
        {

        }

        public static void RemoveAgvModule(string name)
        {
            AgvModule agv = _agvModules.Find(c => { return c._name.Equals(name); });
            if (agv != null)
            {
                _agvModules.Remove(agv);
            }
        }

        /// <summary>
        /// AGV添加到地图的新方法
        /// </summary>
        /// <param name="agvname"></param>
        /// <param name="site"></param>
        /// <param name="rate"></param>
        public static void AddAgvToModuleNew(bool isalive,string agvname,int site = 33,int dessite=0,  float rate = 0)
        {
            AgvPoint p = AgvLineMaster.GetMPointOnLine(agvname,site, dessite, rate);
            if (p != null)
            {
                AgvModule agvm = new AgvModule(agvname, new Point(p.X, p.Y), site);
                agvm.UpdateAgvStatus(isalive);
                _agvModules.Add(agvm);
                FLog.Log("添加" + agvname + ",地标:" + site + ",地标:" + p.X + "," + p.Y);
            }
            else
            {
                FLog.Log("找不到地标对应地图位置：" + agvname + ",地标:" + site);
            }
        }


        public static void AddAgvToMapNew(bool isalive, string name, int site = 33, int dessite = 0, float rate = 0)
        {
            AgvModule agv = _agvModules.Find(c => { return c._name.Equals(name); });
            if (agv == null)
            {
                AddAgvToModuleNew(isalive, name, site, dessite, rate);
            }
            
        }

        private static MPoint point;
        /// <summary>
        /// 更新AGV当前所在位置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="siteid"></param>
        /// <param name="rate"></param>
        public static void UpdateAgvSite(string name, int siteid, float rate)
        {
            AgvModule agv = _agvModules.Find(c => { return c._name.Equals(name); });

            if (agv != null)
            {
                point = LineDateCenter.GetMPointOnLine(siteid, rate);
                if (point != null)
                {
                    agv.Update(new Point(point.X, point.Y));
                }
            }
        }

        /// <summary>
        /// 添加AGV到地图上
        /// </summary>
        /// <param name="agvname"></param>
        /// <param name="site"></param>
        /// <param name="rate"></param>
        public static void AddAgvToModule(string agvname,int site = 23, float rate = 0)
        {
            MPoint p = LineDateCenter.GetMPointOnLine(site, rate);
            if (p != null)
            {
                _agvModules.Add(new AgvModule(agvname, new Point(p.X, p.Y), site));
                AgvSiteMaster.AddAgvSiteRate(agvname, site, rate);
            }
        }

        /// <summary>
        /// 将XML配置文档解析到线路配置添加到地图中
        /// </summary>
        /// <param name="lineDatas"></param>
        public static void AddLinesToModule(List<LineData> lineDatas)
        {
            _lineModules.Clear();
            foreach (LineData data in lineDatas)
            {
                _lineModules.Add(new LineModule(data));
            }
        }
    }

    /// <summary>
    /// 模型抽象类
    /// </summary>
    public abstract class Module
    {
        /// <summary>
        /// 模型名称
        /// </summary>
        internal string _name;

        /// <summary>
        /// 模型中心点
        /// </summary>
        internal Point _centerP;

        /// <summary>
        /// 尺寸比例：默认1
        /// </summary>
        internal int _scale = 1;

        /// <summary>
        /// 默认尺寸
        /// </summary>
        internal int _size = 10;

        /// <summary>
        /// 文字描述点
        /// </summary>
        internal Point _describP;

        /// <summary>
        /// 画笔
        /// </summary>
        internal Pen _pen = new Pen(new SolidBrush(Color.Black));

        internal Brush _brush = new SolidBrush(Color.Black);
        internal Brush _orageBrush = new SolidBrush(Color.Orange);
        internal Brush _brushRed = new SolidBrush(Color.OrangeRed);
        internal Brush _brushGreen = new SolidBrush(Color.Green);
        internal Brush _brushGray = new SolidBrush(Color.Gray);

        internal Font _font = new Font("宋体", 10);
        internal Font _fontMin = new Font("宋体", 8);
        internal Font _fontMax = new Font("宋体", 15);

        /// <summary>
        /// 更新模型中心点
        /// </summary>
        /// <param name="centerPoint"></param>
        public abstract void Update(Point centerPoint);

        /// <summary>
        /// 将模型画用GUI画出来
        /// </summary>
        /// <param name="g">GUI绘画图面</param>
        public abstract void Draw(Graphics g);
    }
}
