using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DispatchAnmination.AgvLine
{
    class LinePublic
    {
    }

    /// <summary>
    /// 站点对应的后面线路
    /// </summary>
    public class Line
    {
        /// <summary>
        /// 地标
        /// </summary>
        public int LineID { set; get; }
        /// <summary>
        /// 地标的正负信息
        /// 0 负卡 1 正卡 2正负卡
        /// </summary>
        internal Direction Direction { set; get; }
        public int Lenght { set; get; }
        public List<MPoint> _points = new List<MPoint>();
        public void AddPoint(MPoint point)
        {
            point.ID = _points.Count();
            _points.Add(point);
            if (_points.Count > 1)
            {
                Lenght = Lenght + Line.GetLenght(_points[_points.Count() - 2], _points[_points.Count() - 1]);
            }
        }

        /// <summary>
        /// 获取两点间的距离
        /// </summary>
        /// <returns></returns>
        public static int GetLenght(MPoint p1, MPoint p2)
        {
            int x = p1.X - p2.X;
            int y = p1.Y - p2.Y;
            return (int)Math.Sqrt((double)(x * x) + (double)(y * y));
        }


        /// <summary>
        /// 根据百分比获取当前所在地方地标
        /// </summary>
        /// <param name="rate"></param>
        public MPoint GetPositionPOnRate(float rate)
        {
            int rateleng = (int)(rate / (float)100 * (float)Lenght);
            foreach (MPoint p in _points)
            {
                if (p.ID == 0)
                { continue; }
                else
                {
                    int len = Line.GetLenght(_points[p.ID - 1], p);
                    rateleng = rateleng - len;
                    if (rateleng > 0)//还需判断下个点
                    {
                        continue;
                    }
                    else
                    {
                        rateleng = rateleng + len;//源剩下长度

                        double rateP = Convert.ToDouble(rateleng) / Convert.ToDouble(len);
                        return GetMP(rateP, _points[p.ID - 1], p);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 根据百分比获取两个点直接的百分比的坐标值
        /// </summary>
        /// <param name="rate"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public MPoint GetMP(double rate, MPoint p1, MPoint p2)
        {
            int x = ((int)((double)rate * (p2.X - p1.X)) + p1.X);
            int y = ((int)((double)rate * (p2.Y - p1.Y)) + p1.Y);
            return new MPoint(x, y);
        }
    }



    /// <summary>
    /// 地图上的坐标点
    /// </summary>
    public class MPoint
    {
        /// <summary>
        /// 
        /// </summary>
        internal int Id { set; get; }
        public int ID
        {
            set
            {
                Id = value;
            }
            get
            {
                return Id;
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public MPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        internal int X { set; get; }
        internal int Y { set; get; }
    }

    /// <summary>
    /// 正负卡常量
    /// </summary>
    enum Direction : int
    {
        /// <summary>
        /// 负卡
        /// </summary>
        negative = 0,
        /// <summary>
        /// 正卡
        /// </summary>
        positive = 1,
        /// <summary>
        /// 正负卡
        /// </summary>
        both = 2
    }
}
