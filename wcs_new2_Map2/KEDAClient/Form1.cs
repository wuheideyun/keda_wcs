﻿using DataContract;
using DispatchAnmination;
using GfxMapEditor;
using KEDAClient;
using KedaMap.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WcfHelper;

namespace FormTest
{
    public partial class Form1 : Form
    {

        Random _rd = new Random();

        public Form1()
        {
            InitializeComponent();

            mapEditorControlMap.GraphClicked += GraphClickedHandle;

            Thread thread = new Thread(InitPara)
            {
                IsBackground = true
            };
            thread.Start();

            DevMaster.Init();

            FLog.Init();
        }
        /// <summary>
        /// 服务端IP地址
        /// </summary>
        private string _severIp = "";

        /// <summary>
        ///  初始化参数
        /// </summary>
        public void InitPara()
        {
            _severIp = "192.168.6.79"; // 吕超电脑的IP

            WcfMainHelper.InitPara(_severIp, "", "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 验证ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Func1();
        }


        public void Func1()
        {
            mapEditorControlMap.MapObj.IGExpand.IClear();

            Random rd = new Random();

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    IStoreObj store = new IStoreObj();

                    store.Id = string.Format("Loc_{0}_{1}", i, j);

                    store.ILocPoint = new Point(i * 2000, j * 2000);

                    store.IBackColor = Color.FromArgb(rd.Next(int.MaxValue));

                    mapEditorControlMap.MapObj.IGExpand.ISetDot(store);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GraphClickedHandle(IGraphInfo sender, MyEventArgs e)
        {
            if (e != null)
            {

            }
        }

        IWord word = new IWord();

        IMap map = new IMap();

        bool _flag = false;

        private void timer1_Tick(object sender, EventArgs e)
        {
            //1.AGV地图的坐标图片信息放在 MapItemMaster.Mapitmes
            foreach (var agv in MapItemMaster.Mapitmes)
            {
                mapEditorControlMap.MapObj.IGExpand.ISetWoed(agv.word);
                mapEditorControlMap.MapObj.IGExpand.ISetBitMap(agv.map);
            }

            foreach (var msg in MapDevMsgMaster.DevMsg)
            {
                mapEditorControlMap.MapObj.IGExpand.ISetWoed(msg);
            }

            try
            {
                mapEditorControlMap.MapObj.IGExpand.ISetBitMap(PlcItem.HeadPlcMap);
                mapEditorControlMap.MapObj.IGExpand.ISetBitMap(PlcItem.TailPlcMap);
                mapEditorControlMap.MapObj.IGExpand.ISetWoed(PlcItem.HeadPlcword);
                mapEditorControlMap.MapObj.IGExpand.ISetWoed(PlcItem.TailPlcword);
            }
            catch (Exception eee)
            {
                Console.WriteLine(eee.Message);
            }

            //word.Id = "1";

            //word.ILocPoint = new Point(_rd.Next(10000) - 10000, _rd.Next(10000) - 10000);

            //word.Text = DateTime.Now.ToString();

            //mapEditorControlMap.MapObj.IGExpand.ISetWoed(word);


            //_flag = !_flag;

            //if (_flag)
            //{
            //    map.IBitMap = Resources.XP_megaphone;
            //}
            //else { map.IBitMap = Resources.QQ; }

            //map.ILocPoint = new Point(20000, -10000);

            //map.IAngel = _rd.Next(100);

            //map.Id = "2";

            //mapEditorControlMap.MapObj.IGExpand.ISetBitMap(map);
        }
    }
    public class MapItemMaster
    {
        private static Object _obj = new object();
        public static List<MapItem> Mapitmes = new List<MapItem>();
        private static List<MapItem> _mapitmes = new List<MapItem>();


        /// <summary>
        /// 2.后台获取了AGV的信息在这个方法组装
        /// </summary>
        /// <param name="list"></param>
        public static void UpdateItems(List<DeviceBackImf> list)
        {
            _mapitmes.Clear();

            foreach (var agv in list)
            {
                if (agv.DevType.Equals("Magnet_Basic"))//AGV
                {
                    try
                    {
                        //3.如果是 AGV类型的设备 进入这个方法
                        //在这里可以对AGV做判断，如果离线不显示则不添加
                        if (agv.IsAlive)
                        {
                            _mapitmes.Add(new MapItem(agv));
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                else//PLC
                {
                    PlcItem.Update(agv);
                }
            }


            lock (_obj)
            {
                Mapitmes.Clear();
                Mapitmes.AddRange(_mapitmes);
            }

        }
    }
    public class PlcItem
    {

        public static IWord HeadPlcword = new IWord();
        public static IWord TailPlcword = new IWord();

        public static IMap HeadPlcMap = new IMap();
        public static IMap TailPlcMap = new IMap();

        public static Size MapSize = new Size(500, 500);

        public static void Update(DeviceBackImf plc)
        {
            if (plc.DevId.Equals("PLC01"))//窑尾
            {
                //1.有货后逐渐右移动
                if (plc.IGet("0001").RValue.Equals("2"))//有货
                {
                    TailPlcMap.IBitMap = Resources.load;
                    TailPlcMap.ILocPoint = GetPlcPoint(false, 1);
                    TailPlcword.Text = "有货";
                }
                else if(plc.IGet("0001").RValue.Equals("1") || plc.IGet("0001").RValue.Equals("7"))//无货
                {
                    TailPlcMap.IBitMap = Resources.bg;
                    TailPlcMap.ILocPoint = GetPlcPoint(false, 2);
                    TailPlcword.Text = "无货";
                }
                else if (plc.IGet("0001").RValue.Equals("3"))//传输中
                {
                    TailPlcMap.ILocPoint = GetPlcPoint(false, 3);
                    TailPlcMap.IBitMap = Resources.load;
                    TailPlcword.Text = "传输中";
                }

                
                TailPlcMap.Id = plc.DevId;
                TailPlcMap.ISize = MapSize;
                TailPlcword.Id = plc.DevId + "100";
                TailPlcword.IFont = new Font("宋体", 350, FontStyle.Bold);
                TailPlcword.IColor = Color.Black;
                TailPlcword.ILocPoint=new Point(492,6050);

            }
            else//窑头
            {
                if (DevMaster.F_Dev.FindDevInHeadPlc() != null)
                {
                    HeadPlcMap.ILocPoint = GetPlcPoint(true, 3);
                    HeadPlcMap.IBitMap = Resources.load;
                    HeadPlcword.Text = "传输中";
                }
                else if (plc.IGet("0001").RValue.Equals("2"))//有货
                {
                    HeadPlcMap.ILocPoint = GetPlcPoint(true, 1);
                    HeadPlcMap.IBitMap = Resources.load;
                    HeadPlcword.Text = "有货";
                }
                else if (plc.IGet("0001").RValue.Equals("4"))//无货/传输中
                {
                    HeadPlcMap.ILocPoint = GetPlcPoint(true, 2);
                    HeadPlcMap.IBitMap = Resources.bg;
                    HeadPlcword.Text = "无货";
                }
                
                HeadPlcMap.Id = plc.DevId;
                HeadPlcMap.ISize = MapSize;
                HeadPlcword.Id = plc.DevId + "100";
                HeadPlcword.IFont = new Font("宋体", 350, FontStyle.Bold);
                HeadPlcword.IColor = Color.Black;
                HeadPlcword.ILocPoint = new Point(26376, 6050);
            }
        }
        private static Point HeadLoadP = new Point(26646, 5146);//窑头有货地标
        private static Point TailLoadP = new Point(1680, 5146);//窑尾有货地标
        private static float MoveHead = -500, MoveTail = 0;
        public static Point GetPlcPoint(bool isHead,int status)
        {
            switch (status)
            {
                case 1://有货
                    return isHead ? HeadLoadP : TailLoadP;
                case 2://无货
                    if (!isHead)
                    {
                        MoveTail = 0;
                    }
                    else
                    {
                        MoveHead = -200;
                    }
                    return isHead ? HeadLoadP : TailLoadP;
                case 3://传输中
                    if (isHead)
                    {
                        MoveHead += 1F;
                    }
                    else
                    {
                        if (MoveTail < 500)
                            MoveTail += 1F ;
                    }
                    return isHead ? new Point(HeadLoadP.X+(int)MoveHead,HeadLoadP.Y) : new Point(TailLoadP.X+(int)MoveTail,TailLoadP.Y);
            }

            return new Point(200, 200);
        }
    }


    public class MapItem
    {

        public IWord word = new IWord();

        public IMap map = new IMap();


        /// <summary>
        /// 4. 这里是组装一个AGV的图片和agv名字的构造方法
        /// </summary>
        /// <param name="agv"></param>
        public MapItem(DeviceBackImf agv)
        {
            //获取AGV的 X坐标值
            int x = agv.IGet("B02") == null ? 0 : int.Parse(agv.IGet("B02").RValue);

            //获取AGV Y坐标值
            int y = agv.IGet("B03") == null ? 0 : int.Parse(agv.IGet("B03").RValue);

            word.Id = agv.DevId;

            word.IColor = Color.Black;

            word.IFont = new Font("宋体", 350, FontStyle.Bold);

            word.ILocPoint = new Point(x - 300, y - 250);

            word.Text = agv.DevId.Replace("AGV","");

            //根据AGV的货物状态 设置图片资源
            if ("1".Equals(agv.IGet("0036").RValue))
            {
                //AGV有货的图片
                map.IBitMap = Resources.loadagv;
            }
            else
            {
                //AGV没货的图片
                map.IBitMap = Resources.AGV;
            }

            map.ISize = new Size(500, 500);

            map.ILocPoint = new Point(x, y);

            //map.IAngel = agv.IGet("B04")==null ? 0 : float.Parse(agv.IGet("B04").RValue);

            map.Id = agv.DevId;
        }
    }

    /// <summary>
    /// 设备信息
    /// </summary>
    public class MapDevMsgMaster
    {
        private static Point StartP = new Point(600, 18000);
        private static int Disten_X = 3700, Disten_Togeter = 3300;
        private static int Disten_Y = -2000;
        private static int Now_X = StartP.X, Now_Y = StartP.Y;
        private static Object _obj = new object();
        public static List<IWord> DevMsg = new List<IWord>();
        public static Font _font = new Font("宋体", 900, FontStyle.Bold);

        private static int SwichLineCount = 0;
        public static Point NextPoint()
        {

            if (SwichLineCount % 2 == 1)
            {
                Now_X += Disten_Togeter;
            }
            else
            {
                Now_X += Disten_X;
            }

            if (SwichLineCount % 8 == 0)
            {
                Now_Y += Disten_Y;
                Now_X =  StartP.X;
            }
            SwichLineCount++;
            return new Point(Now_X, Now_Y);
        }
        private static void InitP()
        {
            Now_X = StartP.X;
            Now_Y = StartP.Y;
            DevMsg.Clear();
            SwichLineCount = 0;
        }

        /// <summary>
        /// 6.组装上方的AGV状态列表信息
        /// </summary>
        /// <param name="list"></param>
        public static void UpdateDevInfo(List<DeviceBackImf> list)
        {
            lock (_obj)
            {
                InitP();
                foreach (var agv in list)
                {
                    if (!agv.DevType.Equals("Magnet_Basic")) continue;
                    DevMsg.Add(new IWord
                    {
                        Id = agv.DevId + 10,
                        Text = agv.DevId,
                        ILocPoint = NextPoint(),
                        IColor = Color.Black,
                        IFont = _font

                    });
                    Color color = Color.Black;
                    DevMsg.Add(new IWord
                    {
                        Id = agv.DevId + 100,
                        Text = GetDevStatus(agv, out color),
                        ILocPoint = NextPoint(),
                        IColor = color,
                        IFont = _font
                    });
                }
            }
        }

        /// <summary>
        /// 获取设备状态
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetDevStatus(DeviceBackImf item,out Color color)
        {
            try
            {
                string status = "";
                if (!item.IsAlive)
                {
                    status = "离线";
                    color = Color.Gray;
                }
                else if (item.IGet("0007").RValue.Equals("10"))
                {
                    status = "在线";
                    color = Color.LightGreen;
                }
                else if (item.DevType == "WK_PLC")
                {
                    status = "在线";
                    color = Color.LightGreen;
                }
                else if (item.IGet("0029").RValue.Equals("0"))
                {
                    status = "脱轨";
                    color = Color.Blue ;
                }
                else if (item.IGet("0001").RValue.Equals("2"))
                {
                    status = "障碍";
                    color = Color.LightGreen;
                }
                else if (item.IGet("T01").RValue.Equals("True"))
                {
                    status = "交管";// (" + item.IGet("T02").RValue + ")";
                    color = Color.LightGreen;
                }
                else if (item.IGet("0008").RValue.Equals("1"))
                {
                    status = "充电";
                    color = Color.Yellow;
                }
                else if (item.IGet("0002").RValue.Equals("11") || item.IGet("0002").RValue.Equals("14"))
                {
                    status = "对接";
                    color = Color.LightGreen;
                }
                else if (!item.IGet("0010").RValue.Equals("True") && DevMaster.F_Dev.IsDevInDispath(item.DevId))
                {
                    status = "任务";
                    color = Color.LightGreen;
                }
                else if (item.IGet("0010").RValue.Equals("True") && !DevMaster.F_Dev.IsDevInDispath(item.DevId))
                {
                    status = "空闲";
                    color = Color.LightGreen;
                }
                else if(DevMaster.F_Dev.IsDevInDispath(item.DevId))
                {
                    status = "任务";
                    color = Color.LightGreen;
                }
                else
                {
                    status = "未知";
                    color = Color.LightGreen;
                }
                return status;
            }
            catch (Exception e)
            {
                FLog.Log(e.Message);
                color = Color.Black;
                return "程序错误";
            }

        }
    }

    public class MapDevMsg
    {
        public string Name;
        public string Status;
    }
}
