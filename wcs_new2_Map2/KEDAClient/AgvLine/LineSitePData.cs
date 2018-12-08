using DispatchAnmination.AgvLine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DispatchAnmination
{
    /// <summary>
    /// 线路站点，地图坐标范围
    /// 根据AGV所在站点获取所在地图线路站点的位置
    /// </summary>
    public class LineDateCenter
    {
        /// <summary>
        /// 线路列表
        /// </summary>
        public static List<Line> _linesPositive = new List<Line>();
        public static List<Line> _linesNagetivie = new List<Line>();

        /// <summary>
        /// 获取AGV所在的地标
        /// </summary>
        /// <param name="lineid"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static MPoint GetMPointOnLine(int lineid,float rate)
        {
            foreach(Line line in _linesPositive)
            {
                if(line.LineID == lineid)
                {
                    return line.GetPositionPOnRate(rate);
                }
            }

            return null;
        }

        //private static readonly MPoint FirstP;
        private static MPoint SecconP;
        private static bool First = true;

        /// <summary>
        /// 解析线路形成AGV行作路线
        /// </summary>
        public static void AddLineData()
        {
            _linesPositive.Clear();
            _linesNagetivie.Clear();
            foreach(LineModule lineM in ModuleControl._lineModules)
            {
                //有站点
                if(lineM.SitePos != null && lineM.SitePos.Count()>0)
                {
                    foreach (SitePos site in lineM.SitePos)
                    {
                        if (First)
                        {
                            if (site._rate == 0)
                            {
                                //FirstP = new MPoint(site._siteP.X, site._siteP.Y);
                            }
                            else
                            {
                                //FirstP = new MPoint(lineM._centerP.X, lineM._centerP.Y);
                                SecconP = new MPoint(site._siteP.X, site._siteP.Y);
                            }
                            First = false;
                        }
                        if (site._direction == (int)Direction.positive || site._direction == (int)Direction.both)//正卡
                        {
                            Line line = new Line
                            {
                                Direction = (Direction)site._direction,
                                LineID = site.ID
                            };
                            line.AddPoint(new MPoint(site._siteP.X, site._siteP.Y));
                            if (_linesPositive.Count() > 0)
                            {
                                _linesPositive[_linesPositive.Count() - 1].AddPoint(new MPoint(site._siteP.X, site._siteP.Y));
                            }
                            _linesPositive.Add(line);
                        }

                        if (site._direction == (int)Direction.negative || site._direction == (int)Direction.both)//反卡
                        {
                            Line line = new Line
                            {
                                Direction = (Direction)site._direction,
                                LineID = site.ID
                            };
                            line.AddPoint(new MPoint(site._siteP.X, site._siteP.Y));
                            if (_linesNagetivie.Count() > 0)
                            {
                                _linesNagetivie[_linesNagetivie.Count() - 1].AddPoint(new MPoint(site._siteP.X, site._siteP.Y));
                            }
                            _linesNagetivie.Add(line);
                        }
                    }
                }
                else  //没站点，只是一条线
                {
                    if (_linesPositive.Count() > 0)
                    {
                        _linesPositive[_linesPositive.Count()-1].AddPoint(new MPoint(lineM._centerP.X, lineM._centerP.Y));
                        _linesPositive[_linesPositive.Count()- 1].AddPoint(new MPoint(lineM._endP.X, lineM._endP.Y));

                    }

                    if (_linesNagetivie.Count() > 0)
                    {
                        _linesNagetivie[_linesNagetivie.Count() - 1].AddPoint(new MPoint(lineM._centerP.X, lineM._centerP.Y));
                        _linesNagetivie[_linesNagetivie.Count() - 1].AddPoint(new MPoint(lineM._endP.X, lineM._endP.Y));

                    }
                }
                
            }

            //处理第一个点和最后一个点连起来
            //if (FirstP != null) _linesPositive[_linesPositive.Count() - 1].AddPoint(FirstP);
            //if (SecconP != null) _linesPositive[_linesPositive.Count() - 1].AddPoint(SecconP);
        }
    }
}
