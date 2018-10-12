using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArrayMap;

namespace TestCenter
{
    class Program
    {
        /// <summary>
        /// 用于测试
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            TestArrayMap();
        }

        /// <summary>
        /// 测试数组内容
        /// </summary>
        static void TestArrayMap()
        {
            //   使用前导入ArrayMap.DLL文件 

            //简单使用
            MapList mapList = new MapList();
            mapList.Put("agv0001", false);//没锁定

            mapList.Put("agv0001", true);//锁定

            //获取agv锁定情况
            Boolean isLock = mapList.GetBooleanValue("agv0001");


            MapList map = new MapList();
            //新增
            map.Put("001", true);
            map.Put("002", false);
            map.Put("003", "HELLO");

            MapList map2 = new MapList();
            //新增
            map2.Put("001", "yes");
            map2.Put("003", true);
            map2.Put("004", "NONONO");


            //替换
            map.Put("001", "HI");
            map2.Put("003", "OK");

            map.GetBooleanValue("001");

            Console.WriteLine(map.GetStringValue("002"));


            for (int i = 0; i < map.Length(); i++)
            {
                Map m = map.GetMapAtIndex(i);
                Map m2 = map2.GetMapAtIndex(i);

                Console.WriteLine(m.Key + ": " + m.Value);
                Console.WriteLine(m2.Key + ": " + m2.Value);

            }
            Console.WriteLine(map.length);
            Console.WriteLine(map2.length);

            Console.ReadKey();
        }
    }
}
