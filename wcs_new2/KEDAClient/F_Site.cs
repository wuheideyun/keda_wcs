using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KEDAClient
{
    public class ConstSetBA
    {
        public static String 窑尾装载等待区 = "33";

        public static String 窑尾装载点 = "11";

        public static String 窑头卸载等待区 = "32";

        public static String 窑头卸载点 = "14";

        public static String 接货充电点 = "51";

        public static String 卸货充电点 = "52";

        public static String 窑尾等待点和装载点之间 = "23";

        public static String 窑头等待点和卸载点之间 = "22";

        public static int    最低电量 = 70;

        public static String AGV有货 = "1";

        public static String AGV无货 = "2";

        public static int 地标= 1;

        public static int 运行方向 = 4;

        public static int 充电状态 = 7;

        public static int 货物状态 = 26;

        public static int 空闲 = 9;

        public static int 在轨道上 = 22;

        public static int 交管状态 = 35;

        public static int 交管设备 = 36;

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
}
