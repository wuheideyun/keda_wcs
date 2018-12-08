using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DispatchAnmination.Const
{
    /// <summary>
    /// 公共变量管理
    /// </summary>
    public class ConstBA
    {
        /// <summary>
        /// 是否展示站点信息
        /// </summary>
        public static bool IsShow_Site = true;

        /// <summary>
        /// 是否展示站点名称
        /// </summary>
        public static bool IsShow_SiteName = false;
        /// <summary>
        /// 是否展示站点顶部名称
        /// </summary>
        public static bool IsShow_SiteUpName = true;

        /// <summary>
        /// 是否展示站点坐标信息
        /// </summary>
        public static bool IsShow_SitePoint = false;

        /// <summary>
        /// 是否展示窑头窑尾地标
        /// </summary>
        public static bool IsShow_HeadTialSite = true;

        /// <summary>
        /// 是否展示等待点
        /// </summary>
        public static bool IsShow_WaiteSite = true;


        /// <summary>
        /// 是否展示转弯点
        /// </summary>
        public static bool IsShow_SwerveSite = true;


        /// <summary>
        /// 是否展示掉头点
        /// </summary>
        public static bool IsShow_TrunRoundSite = true;

        /// <summary>
        /// 是否展示充电点
        /// </summary>
        public static bool IsShow_ChargeSite = true;


        /// <summary>
        /// 是否展示交通管制点
        /// </summary>
        public static bool IsShow_TrafficSite = true;


        /// <summary>
        /// 是否展示非交通管制点
        /// </summary>
        public static bool IsShow_NotTrafficSite = true;

        /// <summary>
        /// 是否展示完成点
        /// </summary>
        public static bool IsShow_FinishSite = true;

        /// <summary>
        /// 是否展示加减速点
        /// </summary>
        public static bool IsShow_IncreSite = true;

        /// <summary>
        /// 是否展示线条两端坐标
        /// </summary>
        public static bool IsShow_LinePoint = false;

        /// <summary>
        /// 是否展示离线AGV
        /// </summary>
        public static bool IsShow_OffLineAGV = false;

        /// <summary>
        /// 初始化AGV展示
        /// </summary>
        public static bool Init_ShowLineAGV = true;
    }
}
