using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KEDAClient
{
    public class ConstSetBA
    {
        public static int    最低电量 =88;

        public static String AGV有货 = "1";

        public static String AGV无货 = "2";

        public static int 地标= 1;

        public static int 运行方向 = 4;

        public static int 充电状态 = 7;

        public static int 货物状态 = 26;
    }

    public class ErrorType
    {
        public static int 故障代码 = 8;

        public static int 脱轨 = 10;

        public static int 轨道错误 = 12;

        public static int 机械撞 = 13;

        public static int 避障异常 = 14;

        public static int 驱动器故障 = 15;

        public static int 挂钩故障 = 16;

        public static int 急停触发 = 20;
    }

    public class Site
    {
        public static String 窑尾6 = "33";

        public static String 窑尾5 = "42";

        public static String 窑尾2 = "32";

        public static String 窑尾1 = "35";

        public static String 窑头7 = "36";

        public static String 窑头8 = "41";

        public static String 窑头3 = "40";

        public static String 窑头4 = "38";

        public static String 充电点 = "50";

        public static String 窑尾5和1之间 = "34";

        public static String 窑头8和4之间 = "41";
    }
}
