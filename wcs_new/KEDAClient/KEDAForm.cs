using Gfx.GfxDataManagerServer;
using Gfx.RCommData;
using GfxAgvMapEx;
using GfxCommonInterfaces;
using GfxServiceContractClient;
using GfxServiceContractClientDispatch;
using GfxServiceContractTaskExcute;
using JTWcfHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace KEDAClient
{

    public partial class KEDAForm : Form
    {
        /// <summary>
        /// 登录状态
        /// </summary>
        private bool _isLogin = false;

        /// <summary>
        /// 服务端IP地址
        /// </summary>
        private string _severIp = "";

        /// <summary>
        ///  客户端标志
        /// </summary>
        private string _clientMark = "";

        /// <summary>
        /// 按钮宽度
        /// </summary>
        private int _btnWidth = 100;

        /// <summary>
        /// 按钮高度
        /// </summary>
        private int _btnHeight = 40;

        /// <summary>
        /// 横向间隔
        /// </summary>
        private int _xDis = 10;

        /// <summary>
        /// 纵向间隔
        /// </summary>
        private int _yDis = 10;

        /// <summary>
        /// 起始坐标
        /// </summary>
        private Point _startLoc = new Point(10, 20);

        /// <summary>
        /// 运行至区域2的特殊车辆
        /// </summary>
        private DeviceBackImf _devArea2 = null;

        /// <summary>
        ///  当前站点号
        /// </summary>
        public int _locTar = -1;

        /// <summary>
        /// 站点集合
        /// </summary>
        private Dictionary<int, StationMember> _staDic = new Dictionary<int, StationMember>();

        /// <summary>
        /// 已经选择的站点集合（任务）
        /// </summary>
        private List<StationMember> _selectStation = new List<StationMember>();

        /// <summary>
        /// 站点个数
        /// </summary>
        private int _staCount = 100;

        /// <summary>
        /// 区域二待命点个数
        /// </summary>
        private int _waitCountAreaTwo = 3;


        /// <summary>
        ///  窑尾装载站点
        /// </summary>
        private string _loadgoodsSta = "1";

        /// <summary>
        ///  窑头卸载站点
        /// </summary>
        private string _unloadgoodsSta = "2";

        /// <summary>
        ///  装载等待区站点
        /// </summary>
        private string _loadWaitSta = "5";

        /// <summary>
        ///  卸载等待区站点
        /// </summary>
        private string _unloadWaitSta = "6";

        /// <summary>
        ///  窑尾装载站点是否有AGV
        /// </summary>
        private bool _loadStaHasAGV = false;

        /// <summary>
        ///  窑头卸载站点是否有有货状态AGV
        /// </summary>
        private bool _unloadStaHasAGV = false;

        /// <summary>
        ///  窑尾装载辊台上是否有货
        /// </summary>
        private bool _loadStaHasGoods = false;

        /// <summary>
        ///  窑头卸载辊台上是否有货
        /// </summary>
        private bool _unloadStaHasGoods = false;

        /// <summary>
        ///  AGV上是否有货
        /// </summary>
        private bool _AGVHasGoods = false;

        /// <summary>
        ///  卸货后的AGV上是否有货
        /// </summary>
        private bool _AGVGoodsStatue = false;

        /// <summary>
        ///  窑尾装载等待区是否有空闲AGV
        /// </summary>
        private bool _HasFreeAGV = false;

        /// <summary>
        ///  窑头卸载等待区是否有卸货AGV
        /// </summary>
        private bool _HasUnloadAGV = false;

        /// <summary>
        ///  窑头卸载点辊台是否转动
        /// </summary>
        private bool _isRorate = false;

        /// <summary>
        /// 区域二待命点集合
        /// </summary>
        private List<AreaTwoWaitPointMember> _waitAreaTwoList = new List<AreaTwoWaitPointMember>();

        /// <summary>
        /// 已经选择的站点集合(车辆）
        /// </summary>
        private List<StationMember> _selectStation1 = new List<StationMember>();




        /// <summary>
        /// 任务ID与自身状态对应关系：状态值：startmission、pausemission、endmission
        /// </summary>
        Dictionary<string, string> taskStatus = new Dictionary<string, string>();

        /// <summary>
        /// 设备ID与自身状态对应关系(车辆）：状态值：stop、forwardmove、backmove
        /// </summary>
        Dictionary<string, string> agvStatus = new Dictionary<string, string>();


        private readonly DeviceBackImf devid;

        //用于查询数据库数据的线程
        private Thread queryDataThread;

        /// <summary>
        /// 线程1：判断窑尾是否有货
        /// </summary>
        Thread thread1;

        /// <summary>
        /// 线程2：判断装载等待位是否有空闲AGV
        /// </summary>
        Thread thread2;

        /// <summary>
        /// 线程3：AGV从窑尾装载等待位到窑尾装载点
        /// </summary>
        Thread thread3;

        /// <summary>
        /// 线程4：判断窑尾装载点是否有AGV
        /// </summary>
        Thread thread4;

        /// <summary>
        /// 线程5：WCS给线边辊台发送下料、电机正转指令
        /// </summary>
        Thread thread5;

        /// <summary>
        /// 线程6：判断窑尾装载点的AGV上是否有货
        /// </summary>
        Thread thread6;

        /// <summary>
        /// 线程7：WCS给线边辊台发送下料料、电机停止指令，接货完成
        /// </summary>
        Thread thread7;

        /// <summary>
        /// 线程8：AGV从窑尾装载点到窑头卸载等待区域
        /// </summary>
        Thread thread8;

        /// <summary>
        /// 线程9：判断卸载等待区域是否有准备卸货的AGV
        /// </summary>
        Thread thread9;

        /// <summary>
        /// 线程10：判断窑头卸载辊台上是否有货
        /// </summary>
        Thread thread10;

        /// <summary>
        /// 线程11：WCS给AGV下发任务：从卸载等待位到窑头卸载点
        /// </summary>
        Thread thread11;

        /// <summary>
        /// 线程12：窑头卸载点是否有有货状态的AGV
        /// </summary>
        Thread thread12;

        /// <summary>
        /// 线程13：WCS给线边辊台发送上料、电机正转指令
        /// </summary>
        Thread thread13;

        /// <summary>
        /// 线程14：同时，启动AGV的车载辊台转动，开始卸货
        /// </summary>
        Thread thread14;

        /// <summary>
        /// 线程15：判断执行卸货任务后的AGV，货物状态是否为无货
        /// </summary>
        Thread thread15;

        /// <summary>
        /// 线程16：AGV上无货，WCS给线边辊台发送上料、电机停止的指令
        /// </summary>
        Thread thread16;

        /// <summary>
        /// 线程17：同时，AGV从窑头卸载点到窑尾装载等待区
        /// </summary>
        Thread thread17;

        /// <summary>
        /// 当前装载状态 
        /// 1.有货 开始找车
        /// 2.正在找车
        /// 3.车正去接货
        /// 4.
        /// </summary>
        int unloadState = 0;

        /// <summary>
        /// 当前装载状态 
        /// 
        int loadState = 0;


        //全局的列表数据变量
        GfxList<DeviceBackImf> devsList;                    // = JTWcfHelper.WcfMainHelper.GetDevList();
        GfxList<GfxServiceContractTaskExcute.TaskBackImf> tasksList;// = JTWcfHelper.WcfTaskHelper.GetAllTask();
        GfxList<TaskRelationMember> definetasklist;// = JTWcfHelper.WcfTaskHelper.GetDefineTask();
        //Ok为true的时候代表 查询到了相应的数据然后可以进行更新
        //xxFirst代表第一次展示数据
        Boolean devFirst = true, devOk = false,
            taskFirst = true, taskOk = false;

        //保存当前tab的位置
        private int tabControl1SelectIndex = 0;
        private string _startSite;
        private string _endSite;

        /// <summary>
        /// 当前客户端的站点编号对应地标
        /// </summary>
        public int LocSite
        {
            get
            {
                StationMember loc = _staDic.Values.ToList().Find(c => { return c.StaTarget == _locTar; });

                return loc != null ? loc.StaSite : -1;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public KEDAForm()
        {
            InitializeComponent();
            InitPara();
            InitStaMember();  // 初始化站点成员
            InitBtnMember();
            comboBox1.SelectedIndex = 0;

            Alarm(); // 报警
            Logger(); // 日志
            Otherdev(); // 设备
            Vehicles(); //车辆
            TaskInform(); // 当前任务
            AllTaskList(); //任务列表
            toolStripLabelVersion.Text = "版本号：V14.1";  //版本号
            timerFunc.Enabled = true;  // 系统时间
            labelLogo.Text = APPConfig.LogoStr();  //公司名称
            UpdateBtnMember();

            //启动作业线程
            //F_DataCenter.Init();

            this.WindowState = FormWindowState.Maximized;

            queryDataThread = new Thread(queryDataThreadFunc);
            queryDataThread.IsBackground = true;
            queryDataThread.Start();


            //thread1 = new Thread(LoadStaHasGoods);
            //thread1.IsBackground = true;
            //thread1.Start();

            //thread2 = new Thread(HasFreeAGV);
            //thread2.IsBackground = true;
            //thread2.Start();

            //thread3 = new Thread(SendAGVtoLoadSta);
            //thread3.IsBackground = true;
            //thread3.Start();

            //thread4 = new Thread(LoadStaHasAGV);
            //thread4.IsBackground = true;
            //thread4.Start();

            //thread5 = new Thread(LoadLineRollerTable);
            //thread5.IsBackground = true;
            //thread5.Start();

            //thread6 = new Thread(AGVHasGoods);
            //thread6.IsBackground = true;
            //thread6.Start();

            //thread7 = new Thread(LoadLineRollerTableStop);
            //thread7.IsBackground = true;
            //thread7.Start();

            //thread8 = new Thread(SendAGVtoUnloadWaitSta);
            //thread8.IsBackground = true;
            //thread8.Start();

            //thread9 = new Thread(UnloadWaitStaHasAGV);
            //thread9.IsBackground = true;
            //thread9.Start();

            //thread10 = new Thread(UnloadStaHasGoods);
            //thread10.IsBackground = true;
            //thread10.Start();

            //thread11 = new Thread(SendAGVtoUnloadSta);
            //thread11.IsBackground = true;
            //thread11.Start();

            //thread12 = new Thread(UnloadStaHasAGV);
            //thread12.IsBackground = true;
            //thread12.Start();

            //thread13 = new Thread(UnloadLineRollerTable);
            //thread13.IsBackground = true;
            //thread13.Start();

            //thread14 = new Thread(ReadyToUnload);
            //thread14.IsBackground = true;
            //thread14.Start();

            //thread15 = new Thread(AGVGoodsStatue);
            //thread15.IsBackground = true;
            //thread15.Start();

            //thread16 = new Thread(UnloadLineRollerTableStop);
            //thread16.IsBackground = true;
            //thread16.Start();

            //thread17 = new Thread(SendAGVtoLoadWaitSta);
            //thread17.IsBackground = true;
            //thread17.Start();
        }


        /// <summary>
        /// 报警
        /// </summary>
        private void Alarm()
        {

            // 创建列表头
            alarmlist.Columns.Add("AGV设备ID", 150, HorizontalAlignment.Left);
            alarmlist.Columns.Add("类型名称", 200, HorizontalAlignment.Left);
            alarmlist.Columns.Add("通道编号", 200, HorizontalAlignment.Left);
            alarmlist.Columns.Add("实时值", 200, HorizontalAlignment.Left);
            alarmlist.Columns.Add("描述值", 200, HorizontalAlignment.Left);

            // 若没有，无法显示数据 
            alarmlist.View = System.Windows.Forms.View.Details;
        }

        /// <summary>
        /// 报警数据展示
        /// </summary>
        private void AlarmDataDisplay()
        {
            ///添加数据项
            ///UI暂时挂起，直到EndUpdate绘制控件，可提高加载速度

            //List<DeviceBackImf> devs = JTWcfHelper.WcfMainHelper.GetDevList();
            if (devsList is null)
            {
                alarmlist.Clear();
                return;
            }
            alarmlist.BeginUpdate();
            foreach (var item1 in devsList)
            {
                if (item1.DevId != null && item1.DevType == "AGV")
                {
                    int[] a = new int[] { 8, 10, 12, 13, 14, 15, 16, 20 };
                    for (int i = 0; i < a.Length; i++)
                    {
                        if (item1.SensorList[a[i]].RValue == "1")
                        {
                            ListViewItem item = new ListViewItem(item1.DevId);  // AGV设备ID
                            item.SubItems.Add(item1.SensorList[a[i]].SensName); // 类型名称
                            item.SubItems.Add(item1.SensorList[a[i]].ChannelId.ToString());  // 通道编号
                            item.SubItems.Add(item1.SensorList[a[i]].RValue);  // 实时值
                            item.SubItems.Add(item1.SensorList[a[i]].EValue); // 描述值                                 
                            alarmlist.Items.Add(item);
                        }
                    }
                }
            }
            // 结束数据处理
            // UI界面一次性绘制
            alarmlist.EndUpdate();
        }

        /// <summary>
        /// 日志
        /// </summary>
        private void Logger()
        {
            // 创建列表头
            loggerlist.Columns.Add("Event", 200, HorizontalAlignment.Left);
            loggerlist.Columns.Add("Timestamp", 200, HorizontalAlignment.Left);
            loggerlist.Columns.Add("Source", 200, HorizontalAlignment.Left);
            loggerlist.Columns.Add("Description", 200, HorizontalAlignment.Left);

            // 若没有，无法显示数据 
            loggerlist.View = System.Windows.Forms.View.Details;

            ///添加数据项
            ///UI暂时挂起，直到EndUpdate绘制控件，可提高加载速度
            loggerlist.BeginUpdate();
            ListViewItem item = new ListViewItem("事件");
            item.SubItems.Add("时间");
            item.SubItems.Add("来源");
            item.SubItems.Add("描述");

            // 显示项
            loggerlist.Items.Add(item);

            // 结束数据处理
            // UI界面一次性绘制
            loggerlist.EndUpdate();
        }

        /// <summary>
        /// 设备 PLC
        /// </summary>
        private void Otherdev()
        {

            // 创建列表头
            otherdevlist.Columns.Add("PLC的ID", 150, HorizontalAlignment.Left);
            otherdevlist.Columns.Add("PLC的型号", 200, HorizontalAlignment.Left);
            otherdevlist.Columns.Add("PLC的状态", 200, HorizontalAlignment.Left);
            otherdevlist.Columns.Add("货物状态", 200, HorizontalAlignment.Left);
            otherdevlist.Columns.Add("电机状态", 200, HorizontalAlignment.Left);
            otherdevlist.Columns.Add("故障状态", 200, HorizontalAlignment.Left);
            otherdevlist.Columns.Add("备用信息", 200, HorizontalAlignment.Left);


            // 若没有，无法显示数据 
            otherdevlist.View = System.Windows.Forms.View.Details;

        }
        /// <summary>
        /// 设备 PLC 数据展示
        /// </summary>
        private void OtherdevDisplay()
        {
            //GfxList<DeviceBackImf> devsList = JTWcfHelper.WcfMainHelper.GetDevList();
            if (devsList is null)
            {
                otherdevlist.Clear();
                return;
            }
            otherdevlist.BeginUpdate();
            foreach (var item1 in devsList)
            {
                if (item1.DevType == "PLC")
                {
                    int[] sens = new int[] { 0, 1, 2, 3 };
                    ListViewItem item = new ListViewItem(item1.DevId); // PLC的id
                    item.SubItems.Add(item1.DevModel);  // PLC的型号
                    item.SubItems.Add(item1.DevStatue);  // PLC的状态
                    item.SubItems.Add(item1.SensorList[sens[0]].RValue); // 货物状态
                    item.SubItems.Add(item1.SensorList[sens[1]].RValue);// 电机状态
                    item.SubItems.Add(item1.SensorList[sens[2]].RValue); // 故障状态
                    item.SubItems.Add(item1.SensorList[sens[3]].RValue); // 备用信息
                    otherdevlist.Items.Add(item);
                }
            }

            // 结束数据处理
            // UI界面一次性绘制
            otherdevlist.EndUpdate();

        }

        /// <summary>
        /// 车辆
        /// </summary>
        private void Vehicles()
        {
            // 创建列表头
            vehicleslist.Columns.Add("车辆编号", 200, HorizontalAlignment.Left);
            vehicleslist.Columns.Add("车辆型号", 100, HorizontalAlignment.Left); // 设备型号
            vehicleslist.Columns.Add("车辆状态", 150, HorizontalAlignment.Left);
            vehicleslist.Columns.Add("运行状态", 150, HorizontalAlignment.Left); // 运行状态：前进、后退、停止
            vehicleslist.Columns.Add("电量", 150, HorizontalAlignment.Left); // 电量
            vehicleslist.Columns.Add("充电状态", 150, HorizontalAlignment.Left); // 充电状态

            // 若没有，无法显示数据 
            vehicleslist.View = System.Windows.Forms.View.Details;
        }

        private void VehiclesDisplay()
        {
            //GfxList<DeviceBackImf> devsList = JTWcfHelper.WcfMainHelper.GetDevList();

            ///添加数据项
            ///UI暂时挂起，直到EndUpdate绘制控件，可提高加载速度
            if (devsList is null)
            {
                vehicleslist.Clear();
                return;
            }

            vehicleslist.BeginUpdate();
            foreach (var item1 in devsList)
            {
                if (item1.DevType == "AGV")
                {
                    // 状态 、运行方向、电量、充电状态
                    int[] sens = new int[] { 0, 4, 6, 7 };
                    ListViewItem item = new ListViewItem(item1.DevId); // 设备id
                    item.SubItems.Add(item1.DevModel); // 设备型号   
                    item.SubItems.Add(item1.DevStatue); // 设备状态  

                    // 判断AGV是停止还是运行，1为运行、3为停止
                    if (item1.SensorList[sens[0]].RValue == "1")
                    {
                        if (item1.SensorList[sens[1]].RValue == "0")
                        {
                            item.SubItems.Add("前进"); // 运行状态：前进
                        }

                        //  item1.SensorList[sens[1]].RValue == "1"  后退
                        else
                        {
                            item.SubItems.Add("后退"); // 运行状态：后退
                        }
                    }
                    else
                    {
                        item.SubItems.Add("停止");  // 运行状态：停止
                    }
                    item.SubItems.Add(item1.SensorList[sens[2]].RValue); // 电量
                    if(item1.SensorList[sens[3]].RValue == "1")
                    {
                        item.SubItems.Add("正充电");  
                    }
                    else if(item1.SensorList[sens[3]].RValue == "2")
                    {
                        item.SubItems.Add("充电完成");
                    }
                    else
                    {
                        item.SubItems.Add("未充电");
                    }                    

                    agvStatus.Add(item1.DevId, "stop");

                    // 显示项
                    vehicleslist.Items.Add(item);
                }
            }
            vehicleslist.EndUpdate();
        }

        /// <summary>
        ///  获取当前任务信息
        /// </summary>
        private void TaskInform()
        {
            // 添加表头，即列名
            taskInformlist.Columns.Add("任务id", 200, HorizontalAlignment.Left);
            taskInformlist.Columns.Add("任务信息", 100, HorizontalAlignment.Left);
            taskInformlist.Columns.Add("任务状态", 150, HorizontalAlignment.Left);
            taskInformlist.Columns.Add("触发源", 150, HorizontalAlignment.Left);
            taskInformlist.Columns.Add("任务设备", 150, HorizontalAlignment.Left);
            taskInformlist.Columns.Add("任务路径", 150, HorizontalAlignment.Left);
            taskInformlist.Columns.Add("控制参数", 150, HorizontalAlignment.Left);
            taskInformlist.Columns.Add("指令信息", 150, HorizontalAlignment.Left);
            taskInformlist.Columns.Add("信息", 200, HorizontalAlignment.Left);
            taskInformlist.Columns.Add("触发时间", 200, HorizontalAlignment.Left);

            // 若没有，无法显示数据 
            taskInformlist.View = System.Windows.Forms.View.Details;
        }

        /// <summary>
        ///  获取当前任务信息展示
        /// </summary>
        private void TaskInformDisplay()
        {
            ///添加数据项
            ///UI暂时挂起，直到EndUpdate绘制控件，可提高加载速度
            ///

            //GfxList<GfxServiceContractTaskExcute.TaskBackImf> tasksList = JTWcfHelper.WcfTaskHelper.GetAllTask();
            if (tasksList is null) return;
            taskInformlist.BeginUpdate();
            foreach (var item1 in tasksList)
            {
                ListViewItem item = new ListViewItem(item1.DisGuid); // 任务id
                item.SubItems.Add(item1.TaskImf); // 任务信息
                item.SubItems.Add(item1.Statue.ToString()); // 任务状态
                item.SubItems.Add(item1.OrderSource); // 触发源
                item.SubItems.Add(item1.DisDevId); // 任务设备
                item.SubItems.Add(item1.PathMsg); // 任务路径
                item.SubItems.Add(item1.TaskCtrType.ToString()); // 控制参数，枚举类型
                item.SubItems.Add(item1.CurDisOrderMsg); // 指令信息
                item.SubItems.Add(item1.BackMsg); // 信息
                item.SubItems.Add(item1.TriggerTime.ToString()); // 触发时间

                if (taskStatus.ContainsKey(item1.DisGuid))
                {
                    taskStatus[item1.DisGuid] = "startmissioin";
                }
                else
                {
                    taskStatus.Add(item1.DisGuid, "startmission");
                }
                //taskStatus.Add(item1.DisGuid, "startmission");
                // 显示项
                taskInformlist.Items.Add(item);
            }

            // 结束数据处理
            // UI界面一次性绘制
            taskInformlist.EndUpdate();
        }
        /// <summary>
        ///  获取任务列表
        /// </summary>
        private void AllTaskList()
        {
            //添加表头，即列名
            executeTasklist.Columns.Add("任务id", -2, HorizontalAlignment.Center);
            executeTasklist.Columns.Add("任务名称", -2, HorizontalAlignment.Left);
            executeTasklist.Columns.Add("任务描述", -2, HorizontalAlignment.Left);
            executeTasklist.Columns.Add("是否自动清除", -2, HorizontalAlignment.Left);
            executeTasklist.Columns.Add("优先级", -2, HorizontalAlignment.Left);


            executeTasklist.View = System.Windows.Forms.View.Details;
        }

        /// <summary>
        ///  获取任务列表信息展示
        /// </summary>
        private void AllTaskListDisplay()
        {
            //GfxList<TaskRelationMember> definetasklist = JTWcfHelper.WcfTaskHelper.GetDefineTask();

            if (definetasklist is null)
            {
                executeTasklist.Clear();
                return;
            }
            executeTasklist.BeginUpdate();
            for (int i = 0; i < definetasklist.Count; i++)
            {
                ListViewItem item = new ListViewItem(definetasklist[i].MemberId); // 任务id
                item.SubItems.Add(definetasklist[i].TaskRelatName);//任务名称
                item.SubItems.Add(definetasklist[i].TaskRelatDecirbe);//任务描述
                item.SubItems.Add(definetasklist[i].IsAotuRemove ? "True" : "False");
                item.SubItems.Add(definetasklist[i].Priority.ToString());

                // 显示项

                executeTasklist.Items.Add(item);
            }
            executeTasklist.EndUpdate();

        }
        /// <summary>
        /// 注销登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void 注销登录ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            _isLogin = false;

            MessageBox.Show("当前用户已注销成功！");
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void 用户登录ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            LogForm form = new LogForm();

            form.ShowDialog();

            _isLogin = form._isLogin;
        }


        /// <summary>
        ///  初始化参数
        /// </summary>
        public void InitPara()
        {
            string Section = "ShowConfig";

            int num = 0;

            #region 按钮宽度
            string key = "BtnWidth";

            string read = ConfigHelper.IniReadValue(Section, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = _btnWidth.ToString();

                ConfigHelper.IniWriteValue(Section, key, read);
            }

            if (Int32.TryParse(read, out num))
            {
                _btnWidth = num;
            }
            #endregion

            #region 按钮高度
            key = "BtnHeight";

            read = ConfigHelper.IniReadValue(Section, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = _btnHeight.ToString();

                ConfigHelper.IniWriteValue(Section, key, read);
            }

            if (Int32.TryParse(read, out num))
            {
                _btnHeight = num;
            }
            #endregion

            #region 横向距离
            key = "XDis";

            read = ConfigHelper.IniReadValue(Section, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = _xDis.ToString();

                ConfigHelper.IniWriteValue(Section, key, read);
            }

            if (Int32.TryParse(read, out num))
            {
                _xDis = num;
            }
            #endregion

            #region 纵向距离
            key = "YDis";

            read = ConfigHelper.IniReadValue(Section, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = _yDis.ToString();

                ConfigHelper.IniWriteValue(Section, key, read);
            }

            if (Int32.TryParse(read, out num))
            {
                _yDis = num;
            }
            #endregion

            #region 起始坐标

            int x = 0, y = 0;

            key = "StartPoint_X";

            read = ConfigHelper.IniReadValue(Section, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = x.ToString();

                ConfigHelper.IniWriteValue(Section, key, read);
            }

            if (Int32.TryParse(read, out num))
            {
                x = num;
            }


            key = "StartPoint_Y";

            read = ConfigHelper.IniReadValue(Section, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = y.ToString();

                ConfigHelper.IniWriteValue(Section, key, read);
            }

            if (Int32.TryParse(read, out num))
            {
                y = num;
            }

            _startLoc = new Point(x, y);
            #endregion


            #region 客户端配置
            Section = "CientConfig";

            key = "LoctionSite";

            read = ConfigHelper.IniReadValue(Section, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = "-1";

                ConfigHelper.IniWriteValue(Section, key, read);
            }

            Int32.TryParse(read, out _locTar);

            key = "SeverIP";

            _severIp = ConfigHelper.IniReadValue(Section, key, 100);

            if (string.IsNullOrEmpty(_severIp))
            {
                _severIp = "127.0.0.1";

                ConfigHelper.IniWriteValue(Section, key, _severIp);
            }
            #endregion

            JTWcfHelper.WcfDispatchHelper.InitPara(_severIp, "", "");

            JTWcfHelper.WcfMainHelper.InitPara(_severIp, "", "");

            JTWcfHelper.WcfTaskHelper.InitPara(_severIp, "", "");

            _clientMark = string.Format("科达研发院_客户端【{0}】", _locTar);

            this.Text = string.Format("{0} IP:{1}", _clientMark, IPHelper.GetLocalIntranetIP());
        }


        /// <summary>
        /// 初始化按钮
        /// </summary>
        private void InitBtnMember()
        {
            panelBtn.Controls.Clear();

            int xNum = (panelBtn.Width - _startLoc.X) / (_xDis + _btnWidth);

            foreach (var item in _staDic.Values)
            {
                Point point = GetIndexLoc(item.StaId - 1, xNum);


                int x = point.X * (_xDis + _btnWidth) + _startLoc.X;

                int y = point.Y * (_yDis + _btnHeight) + _startLoc.Y;

                Button btn = new Button();

                btn.Click += button_Click;

                btn.Location = new Point(x, y);

                btn.Width = _btnWidth;

                btn.Height = _btnHeight;

                btn.Tag = item;

                btn.Text = item.Describ;

                btn.BackColor = Control.DefaultBackColor;

                panelBtn.Controls.Add(btn);

            }

            panelBtn1.Controls.Clear();

            int x1Num = (panelBtn.Width - _startLoc.X) / (_xDis + _btnWidth);

            foreach (var item in _staDic.Values)
            {
                Point point1 = GetIndexLoc(item.StaId - 1, x1Num);


                int x = point1.X * (_xDis + _btnWidth) + _startLoc.X;

                int y = point1.Y * (_yDis + _btnHeight) + _startLoc.Y;

                Button btn1 = new Button();

                btn1.Click += button_Click;

                btn1.Location = new Point(x, y);

                btn1.Width = _btnWidth;

                btn1.Height = _btnHeight;

                btn1.Tag = item;

                btn1.Text = item.Describ;

                btn1.BackColor = Control.DefaultBackColor;

                panelBtn1.Controls.Add(btn1);

            }
        }

        /// <summary>
        ///  初始化站点成员
        /// </summary>
        private void InitStaMember()
        {
            string section = "StationConfigNum";

            string keyPre = string.Format("站点个数");

            string read = ConfigHelper.IniReadValue(section, keyPre, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = "100";

                ConfigHelper.IniWriteValue(section, keyPre, read);
            }

            if (!Int32.TryParse(read, out _staCount))
            {
                _staCount = 100;
            }

            _staDic.Clear();

            for (int i = 1; i <= _staCount; i++)
            {
                StationMember member = GetConfigMember(i);

                if (member != null && !_staDic.ContainsKey(member.StaId))
                {
                    _staDic.Add(member.StaId, member);
                }
            }
        }

        /// <summary>
        /// 获取指定成员
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private StationMember GetConfigMember(int id)
        {
            int dibiao = 0, tar = 0, index = 0, backTar = 0;

            string des = "";

            string section = "StationConfig";

            string keyPre = string.Format("STA_N0.{0}_", id);

            #region 地标
            string key = string.Format(string.Format("{0}{1}", keyPre, "地标"));

            string read = ConfigHelper.IniReadValue(section, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = id.ToString();

                ConfigHelper.IniWriteValue(section, key, read);
            }

            Int32.TryParse(read, out dibiao);
            #endregion

            #region 站点
            key = string.Format(string.Format("{0}{1}", keyPre, "站点"));

            read = ConfigHelper.IniReadValue(section, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = id.ToString();

                ConfigHelper.IniWriteValue(section, key, read);
            }

            Int32.TryParse(read, out tar);
            #endregion

            #region 描述
            key = string.Format(string.Format("{0}{1}", keyPre, "描述"));

            read = ConfigHelper.IniReadValue(section, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = string.Format("站点{0}", id);

                ConfigHelper.IniWriteValue(section, key, read);
            }

            des = read;
            #endregion

            #region 优先级
            key = string.Format(string.Format("{0}{1}", keyPre, "优先级"));

            read = ConfigHelper.IniReadValue(section, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = id.ToString(); ;

                ConfigHelper.IniWriteValue(section, key, read);
            }

            Int32.TryParse(read, out index);
            #endregion

            #region 待命点
            key = string.Format(string.Format("{0}{1}", keyPre, "叫车点地标"));

            read = ConfigHelper.IniReadValue(section, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = "0,";

                ConfigHelper.IniWriteValue(section, key, read);
            }

            List<int> wait = new List<int>();

            string[] tokens = read.Split(',');

            if (tokens != null)
            {
                int tempNum = 0;

                for (int i = 0; i < tokens.Length; i++)
                {
                    if (Int32.TryParse(tokens[i], out tempNum))
                    {
                        if (!wait.Contains(tempNum) && tempNum != 0)
                        {
                            wait.Add(tempNum);
                        }
                    }
                }
            }
            #endregion

            #region 返回站点
            key = string.Format(string.Format("{0}{1}", keyPre, "返回站点"));

            read = ConfigHelper.IniReadValue(section, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = "0,";

                ConfigHelper.IniWriteValue(section, key, read);
            }

            List<int> backTarList = new List<int>();

            tokens = read.Split(',');

            if (tokens != null)
            {
                int tempNum = 0;

                for (int i = 0; i < tokens.Length; i++)
                {
                    if (Int32.TryParse(tokens[i], out tempNum))
                    {
                        if (!backTarList.Contains(tempNum) && tempNum != 0)
                        {
                            backTarList.Add(tempNum);
                        }
                    }
                }
            }
            #endregion

            return new StationMember(id, dibiao, tar, des, index, wait, backTarList);
        }

        private void button_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button btn = sender as Button;
                StationMember sta = btn.Tag as StationMember;


                if (sta != null)
                {
                    if (!_selectStation.Contains(sta))
                    {
                        _selectStation.Add(sta);

                        btn.BackColor = Color.Red;
                    }
                    else
                    {
                        _selectStation.Remove(sta);

                        btn.BackColor = Control.DefaultBackColor;
                    }

                    textBoxNextTars.Text = SelectStaStr();

                    SetOutputMsg(string.Format("当前已选择{0}", textBoxNextTars.Text));

                }
            }
        }

        /// <summary>
        /// 获取已选择站点(任务）
        /// </summary>
        /// <returns></returns>
        private string SelectStaStr()
        {
            if (_selectStation == null || _selectStation.Count < 1) { return string.Empty; }

            _selectStation = _selectStation.OrderBy(c => c.OrderIndex).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var item in _selectStation)
            {
                sb.Append(string.Format("{0},", item.StaTarget));
            }

            return sb.ToString();
        }


        /// <summary>
        /// 获取逻辑位置
        /// </summary>
        /// <param name="id"></param>
        /// <param name="jiange"></param>
        /// <returns></returns>
        private Point GetIndexLoc(int id, int jiange)
        {
            if (jiange == 0) { return new Point(0, 0); }

            int x = id % jiange;

            int y = id / jiange;

            return new Point(x, y);
        }

        /// <summary>
        /// 自动模式和手动模式的目的地站点选择按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonManSend_Click(object sender, EventArgs e)
        {

            if (!_isLogin) { MessageBox.Show("请先登录后再操作！", "提示"); return; }
            //ddd

            if (buttonManSend.Text != "自动模式")
            {
                buttonManSend.Text = "自动模式";

                textBoxNextTars.ReadOnly = true;
            }
            else
            {
                buttonManSend.Text = "手动模式";

                textBoxNextTars.ReadOnly = false;
            }
        }

        /// <summary>
        /// 输出系统信息（任务）
        /// </summary>
        /// <param name="msg"></param>
        public void SetOutputMsg(string msg)
        {
            if (string.IsNullOrEmpty(msg)) { msg = "空消息"; }

            if (listBoxOutput.Items.Count > 200)
            {
                listBoxOutput.Items.RemoveAt(0);
            }

            listBoxOutput.Items.Add(string.Format("【{0}】：{1}", DateTime.Now.TimeOfDay.ToString(), msg));

            listBoxOutput.SelectedIndex = listBoxOutput.Items.Count - 1;
        }

        /// <summary>
        /// 输出系统信息（车辆）
        /// </summary>
        /// <param name="msg"></param>
        private void SetOutputMsg2(string msg)
        {
            if (string.IsNullOrEmpty(msg)) { msg = "空消息"; }

            if (listBox1.Items.Count > 200)
            {
                listBox1.Items.RemoveAt(0);
            }

            listBox1.Items.Add(string.Format("【{0}】：{1}", DateTime.Now.TimeOfDay.ToString(), msg));

            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        /// <summary>
        /// 时钟
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerFunc_Tick(object sender, EventArgs e)
        {
            timerFunc.Enabled = false;

            try
            {
                UpdateShow();
            }
            catch { }

            timerFunc.Enabled = true;
        }

        /// <summary>
        /// 状态及时间的更新
        /// </summary>
        private void UpdateShow()
        {
            toolStripLabelConnect.Text = string.Format("服务端连接状态：{0}", JTWcfHelper.WcfMainHelper.IsConnected ? "已连接" : "未连接");

            toolStripLabelTime.Text = string.Format("当前时间：{0}", DateTime.Now.TimeOfDay.ToString());

            toolStripLabelLogin.Text = string.Format("用户状态：{0}", _isLogin ? "已登录" : "未登录");
        }

        /// <summary>
        /// 重置背景色
        /// </summary>
        private void ResetBackCorlor()
        {
            foreach (var item in panelBtn.Controls)
            {
                if (item is Button)
                {
                    Button btn = item as Button;

                    if (btn != null && btn.BackColor != Control.DefaultBackColor)
                    {
                        btn.BackColor = Control.DefaultBackColor;
                    }
                }
            }
            _selectStation.Clear();  // 清除所有已选中站点
        }

        /// <summary>
        /// 返回指定站点的位置
        /// </summary>
        /// <param name="tar"></param>
        /// <returns></returns>
        private StationMember GetRelateMember(int tar)
        {
            return _staDic.Values.ToList().Find(c => { return c.StaTarget == tar; });
        }

        private void KEDAForm_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        ///  刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitPara();

            UpdateBtnMember();

            InitStaMember();

            InitBtnMember();

            InitWaitPointAreaTwoMember();

            _selectStation.Clear();
        }


        /// <summary>
        /// 更新按钮
        /// </summary>
        private void UpdateBtnMember()
        {
            int xNum = (panelBtn.Width - _startLoc.X) / (_xDis + _btnWidth);

            foreach (var item in panelBtn.Controls)
            {
                if (item is Button)
                {
                    Button btn = item as Button;

                    if (btn != null && btn.Tag is StationMember)
                    {
                        int id = (btn.Tag as StationMember).StaId - 1;

                        Point point = GetIndexLoc(id, xNum);

                        int x = point.X * (_xDis + _btnWidth) + _startLoc.X;

                        int y = point.Y * (_yDis + _btnHeight) + _startLoc.Y;

                        btn.Location = new Point(x, y);

                        btn.Width = _btnWidth;

                        btn.Height = _btnHeight;
                    }
                }
            }
        }

        /// <summary>
        ///  初始化区域二待命点成员
        /// </summary>
        private void InitWaitPointAreaTwoMember()
        {
            string section = "AreaTwoConfig";

            string keyPre = string.Format("待名点个数");

            string read = ConfigHelper.IniReadValue(section, keyPre, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = "3";

                ConfigHelper.IniWriteValue(section, keyPre, read);
            }

            if (!Int32.TryParse(read, out _waitCountAreaTwo))
            {
                _waitCountAreaTwo = 3;
            }

            _waitAreaTwoList.Clear();

            for (int i = 1; i <= _waitCountAreaTwo; i++)
            {
                AreaTwoWaitPointMember member = GetConfigArea2Member(i);

                _waitAreaTwoList.Add(member);

                _waitAreaTwoList = _waitAreaTwoList.OrderBy(c => c.OrderIndex).ToList();
            }
        }

        /// <summary>
        /// 返回指定成员
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private AreaTwoWaitPointMember GetConfigArea2Member(int id)
        {
            int dibiao = 0, tar = 0, index = 0;

            string section = "AreaTwoConfig";

            string keyPre = string.Format("STA_N0.{0}_", id);

            #region 地标
            string key = string.Format(string.Format("{0}{1}", keyPre, "地标"));

            string read = ConfigHelper.IniReadValue(section, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = id.ToString();

                ConfigHelper.IniWriteValue(section, key, read);
            }

            Int32.TryParse(read, out dibiao);
            #endregion

            #region 站点
            key = string.Format(string.Format("{0}{1}", keyPre, "站点"));

            read = ConfigHelper.IniReadValue(section, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = id.ToString();

                ConfigHelper.IniWriteValue(section, key, read);
            }

            Int32.TryParse(read, out tar);
            #endregion

            #region 优先级
            key = string.Format(string.Format("{0}{1}", keyPre, "优先级"));

            read = ConfigHelper.IniReadValue(section, key, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = id.ToString(); ;

                ConfigHelper.IniWriteValue(section, key, read);
            }

            Int32.TryParse(read, out index);
            #endregion

            return new AreaTwoWaitPointMember(id, dibiao, tar, index);
        }


        /// <summary>
        /// 区域二待命点成员
        /// </summary>
        public class AreaTwoWaitPointMember
        {
            /// <summary>
            /// 站点id
            /// </summary>
            private int _staId = -1;

            /// <summary>
            /// 站点地标
            /// </summary>
            private int _staSite = 0;

            /// <summary>
            /// 站点编号
            /// </summary>
            private int _staTarget = 0;

            /// <summary>
            /// 优先级
            /// </summary>
            private int _orderIndex = 0;

            /// <summary>
            /// 描述
            /// </summary>
            private string _describ = "暂无";

            /// <summary>
            /// 待命点集合
            /// </summary>
            private List<int> _waitList = new List<int>();

            /// <summary>
            /// 返回站点集合
            /// </summary>
            private List<int> _backTarList = new List<int>();

            /// <summary>
            /// 站点id
            /// </summary>
            public int StaId
            {
                get { return _staId; }
                set { _staId = value; }
            }

            /// <summary>
            /// 站点地标
            /// </summary>
            public int StaSite
            {
                get { return _staSite; }
                set { _staSite = value; }
            }

            /// <summary>
            /// 站点编号
            /// </summary>
            public int StaTarget
            {
                get { return _staTarget; }
                set { _staTarget = value; }
            }

            /// <summary>
            /// 优先级
            /// </summary>
            public int OrderIndex
            {
                get { return _orderIndex; }
                set { _orderIndex = value; }
            }

            /// <summary>
            /// 描述
            /// </summary>
            public string Describ
            {
                get { return _describ; }
                set { _describ = value; }
            }

            /// <summary>
            /// 待命点集合
            /// </summary>
            public List<int> WaitList
            {
                get { return _waitList; }
                set { _waitList = value; }
            }

            /// <summary>
            /// 返回站点集合
            /// </summary>
            public List<int> BackTarList
            {
                get { return _backTarList; }
                set { _backTarList = value; }
            }

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="id">id</param>
            /// <param name="mark">地标</param>
            /// <param name="tar">站点</param>
            /// <param name="index">优先级</param>
            public AreaTwoWaitPointMember(int id, int mark, int tar, int index)
            {
                _staId = id;

                _staSite = mark;

                _staTarget = tar;

                _orderIndex = index;
            }
        }


        /// <summary>
        /// 站点成员
        /// </summary>
        public class StationMember
        {
            /// <summary>
            /// 站点id
            /// </summary>
            private int _staId = -1;

            /// <summary>
            /// 站点地标
            /// </summary>
            private int _staSite = 0;

            /// <summary>
            /// 站点编号
            /// </summary>
            private int _staTarget = 0;

            /// <summary>
            /// 优先级
            /// </summary>
            private int _orderIndex = 0;

            /// <summary>
            /// 描述
            /// </summary>
            private string _describ = "暂无";

            /// <summary>
            /// 待命点集合
            /// </summary>
            private List<int> _waitList = new List<int>();

            /// <summary>
            /// 返回站点集合
            /// </summary>
            private List<int> _backTarList = new List<int>();

            /// <summary>
            /// 站点id
            /// </summary>
            public int StaId
            {
                get { return _staId; }
                set { _staId = value; }
            }

            /// <summary>
            /// 站点地标
            /// </summary>
            public int StaSite
            {
                get { return _staSite; }
                set { _staSite = value; }
            }

            /// <summary>
            /// 站点编号
            /// </summary>
            public int StaTarget
            {
                get { return _staTarget; }
                set { _staTarget = value; }
            }

            /// <summary>
            /// 优先级
            /// </summary>
            public int OrderIndex
            {
                get { return _orderIndex; }
                set { _orderIndex = value; }
            }

            /// <summary>
            /// 描述
            /// </summary>
            public string Describ
            {
                get { return _describ; }
                set { _describ = value; }
            }

            /// <summary>
            /// 待命点集合
            /// </summary>
            public List<int> WaitList
            {
                get { return _waitList; }
                set { _waitList = value; }
            }

            /// <summary>
            /// 返回站点集合
            /// </summary>
            public List<int> BackTarList
            {
                get { return _backTarList; }
                set { _backTarList = value; }
            }

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="id">id</param>
            /// <param name="mark">地标</param>
            /// <param name="tar">站点</param>
            /// <param name="des">描述</param>
            /// <param name="index">优先级</param>
            /// <param name="waitList">待命点集合</param>      
            /// <param name="backTar">返回站点</param> 
            public StationMember(int id, int mark, int tar, string des, int index, List<int> waitList, List<int> backTar)
            {
                _staId = id;

                _staSite = mark;

                _staTarget = tar;

                _describ = des;

                _orderIndex = index;

                if (waitList != null)
                {
                    _waitList = waitList;
                }

                if (backTar != null)
                {
                    _backTarList = backTar;
                }
            }

        }
        public static class APPConfig
        {
            static string _section = "WAITCOMBINECofing";
            /// <summary>
            /// 用户登录账号判断
            /// </summary>
            public static bool UserLogin(string user, string pass)
            {
                string sec = "UserConfig";

                if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass)) { return false; }

                string key = user;

                string read = ConfigHelper.IniReadValue(sec, key, 100);

                if (string.IsNullOrEmpty(read))
                {
                    read = "kdagv";

                    ConfigHelper.IniWriteValue(sec, key, read);
                }

                return read == pass;
            }


            /// <summary>
            /// 公司名称
            /// </summary>
            /// <returns></returns>
            public static string LogoStr()
            {
                string key = string.Format("公司名称");

                string read = ConfigHelper.IniReadValue(_section, key, 1000);

                if (string.IsNullOrEmpty(read))
                {
                    read = "广东科达洁能股份有限公司";

                    ConfigHelper.IniWriteValue(_section, key, read);
                }

                return read;
            }
        }

        /// <summary>
        /// 指令发送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入指令参数！", "提示");
                return;
            }
            int order = 0;
            if (GetSelectDevid())
            {
                return;
            }
            else
            {
                switch (this.comboBox1.Text)
                {
                    case "发送站点":
                        order = 3;
                        break;
                    case "速度":
                        order = 4;
                        break;
                    case "清除站点":
                        order = 5;
                        break;
                    case "开启声音":
                        order = 6;
                        break;
                    case "关闭声音":
                        order = 7;
                        break;
                    case "心跳指令":
                        order = 8;
                        break;
                    case "辊台":
                        order = 11;
                        break;

                    default:
                        MessageBox.Show("指令类型不存在，请重试！", "提示");
                        return;
                }
                JTWcfHelper.WcfMainHelper.SendOrder(vehicleslist.FocusedItem.Text, new CommonDeviceOrderObj(this.comboBox1.Text, order, Convert.ToInt32(textBox1.Text)));
                if (MessageBox.Show("确定发送指令参数？", "提示", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    SetOutputMsg2("发送 " + this.comboBox1.Text + " 指令参数成功");
                }

            }
        }

        /// <summary>
        /// 构建停止函数
        /// 使得前进、后退无法重复被操作
        /// </summary>
        public void StopAGV()
        {
            JTWcfHelper.WcfMainHelper.InitPara(_severIp, "", "");

            if (JTWcfHelper.WcfMainHelper.SendOrder(vehicleslist.FocusedItem.Text, new CommonDeviceOrderObj("停止" + LocSite, 2, 1)))
            {
                //MessageBox.Show(vehicleslist.FocusedItem.Text, "提示");
                agvForwordMove.Enabled = true;
                agvBackMove.Enabled = true;
            }
            else
            {
                MessageBox.Show("请尝试再操作一次", "提示");
            }
        }

        /// <summary>
        /// 选择某一AGV设备id
        /// </summary>
        public bool GetSelectDevid()
        {
            if (vehicleslist.FocusedItem is null)
            {
                MessageBox.Show("请选中需要操作的车辆", "提示");
                return true;
            }
            return false;
        }

        /// <summary>
        /// AGV 前进启动（车辆）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void agvForwordMove_Click(object sender, EventArgs e)
        {
            if (GetSelectDevid())
            {
                return;
            }
            else
            {
                if (!agvBackMove.Enabled)
                {
                    StopAGV();
                }

                if (JTWcfHelper.WcfMainHelper.SendOrder(vehicleslist.FocusedItem.Text, new CommonDeviceOrderObj("前进启动" + LocSite, 1, 1)))
                {
                    //记录agv状态
                    agvStatus[vehicleslist.FocusedItem.Text] = "forwardmove";
                    //MessageBox.Show(vehicleslist.FocusedItem.Text, "提示");
                    agvForwordMove.Enabled = false;
                }
                else
                {
                    MessageBox.Show("请尝试再操作一次", "提示");
                }
            }
        }

        /// <summary>
        /// AGV 后退启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void agvBackMove_Click(object sender, EventArgs e)
        {
            if (GetSelectDevid())
            {
                return;
            }
            else
            {
                if (!agvForwordMove.Enabled)
                {
                    StopAGV();
                }
                if (JTWcfHelper.WcfMainHelper.SendOrder(vehicleslist.FocusedItem.Text, new CommonDeviceOrderObj("后退" + LocSite, 1, 2)))
                {
                    //记录agv状态
                    agvStatus[vehicleslist.FocusedItem.Text] = "backmove";
                    //MessageBox.Show(vehicleslist.FocusedItem.Text, "提示");
                    agvBackMove.Enabled = false;
                }
                else
                {
                    MessageBox.Show("请尝试再操作一次", "提示");
                }
            }
        }

        /// <summary>
        /// AGV 停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void agvStop_Click(object sender, EventArgs e)
        {
            // 判断是否有前进、后退的小车在运行
            if (agvBackMove.Enabled == false || agvForwordMove.Enabled == false)
            {
                StopAGV();
                //记录agv状态
                agvStatus[vehicleslist.FocusedItem.Text] = "stop";
            }
            else
            {
                MessageBox.Show("当前没有运行的车辆！");
            }

        }

        /// <summary>
        /// 车辆充电
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void charge_Click(object sender, EventArgs e)
        {
            if (GetSelectDevid())
            {
                return;
            }
            // 判断电量低于百分80
            else //if (Convert.ToInt32(vehicleslist.FocusedItem.SubItems[4].Text) < 80)
            {
                OnceTaskMember task = new OnceTaskMember();

                //任务ID
                task.DisGuid = "测试ID";

                //任务名称
                task.TaskRelatName = "测试充电任务";

                ///任务优先级
                task.Priority = 1;

                //任务完成是否自动清除
                task.IsAotuRemove = false;

                //任务中一个调度节点
                DispatchOrderObj dis = new DispatchOrderObj();               


                ////调度的终点
                dis.EndSite = textBox1.Text;

                task.DisOrderList.Add(dis);
                
                if (JTWcfHelper.WcfTaskHelper.StartTaskTemp("客户端_KEDA", task))
                {      
                   
                    //记录agv状态
                    agvStatus[vehicleslist.FocusedItem.Text] = "charge";
                    SetOutputMsg2("AGV充电");
                    charge.Enabled = false;
                }
            }
        }

        /// <summary>
        /// 充电完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void endcharge_Click(object sender, EventArgs e)
        {
            if (GetSelectDevid())
            {
                return;
            }
            else
            {
                if (charge.Enabled)
                {
                    MessageBox.Show("目前AGV没有正在充电！", "提示");
                }
                else
                {
                    //记录agv状态
                    agvStatus[vehicleslist.FocusedItem.Text] = "endcharge";
                    SetOutputMsg2("测试下结束充电按钮，AGV向后启动");
                    endcharge.Enabled = false;
                }
            }
        }

        /// <summary>
        /// 设置指令参数输入只为数字或者逗号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar) && e.KeyChar != 44)//指令参数输入只为数字或者逗号
            {
                MessageBox.Show("只允许输入数字或者逗号！");
                e.Handled = true;
            }

        }

        /// <summary>
        /// 开始 任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startmission_Click(object sender, EventArgs e)
        {
            if (GetSelectTaskid())
            {
                return;
            }
            else if (!pausemission.Enabled)
            {
                //记录任务执行状态
                taskStatus[taskInformlist.FocusedItem.Text] = "startmission";
                if (JTWcfHelper.WcfTaskHelper.AdminCtrTask(taskInformlist.FocusedItem.Text, DisOrderCtrTypeEnum.Start))
                {
                    SetOutputMsg("开始任务成功");
                }
            }
        }


        /// <summary>
        /// 任务暂停
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pausemission_Click(object sender, EventArgs e)
        {
            //GfxList<TaskBackImf> taskList = JTWcfHelper.WcfTaskHelper.GetAllTask();
            if (tasksList != null && tasksList.Count != 0)
            {
                if (GetSelectTaskid())
                {
                    return;
                }
                else
                {
                    // 任务执行状态： unkonw  err  unready wait fail  doing  suc cancl
                    string taskrunstatus = taskInformlist.FocusedItem.SubItems[2].Text;
                    if (taskrunstatus == "suc" || taskrunstatus == "cancl")
                    {

                        MessageBox.Show("该任务已执行成功或取消，无法暂停！", "提示");
                    }
                    else if (MessageBox.Show("当前站点派发任务未完成，是否暂停？", "提示", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        //记录任务执行状态
                        taskStatus[taskInformlist.FocusedItem.Text] = "pausemission";
                        if (JTWcfHelper.WcfTaskHelper.AdminCtrTask(taskInformlist.FocusedItem.Text, DisOrderCtrTypeEnum.Pause))
                        {
                            SetOutputMsg("任务暂停成功！");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 任务结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void endmission_Click(object sender, EventArgs e)
        {
            //GfxList<TaskBackImf> taskList = JTWcfHelper.WcfTaskHelper.GetAllTask();
            if (tasksList != null && tasksList.Count != 0)
            {
                if (GetSelectTaskid())
                {
                    return;
                }
                else
                {
                    //记录任务执行状态
                    taskStatus[taskInformlist.FocusedItem.Text] = "endmission";
                    if (JTWcfHelper.WcfTaskHelper.AdminCtrTask(taskInformlist.FocusedItem.Text, DisOrderCtrTypeEnum.Stop))
                    {
                        SetOutputMsg("结束任务成功");
                    }
                }
            }
            else
            {
                MessageBox.Show("当前没有正在执行的任务！");
            }
        }


        /// <summary>
        /// 判断是否已选择某一任务id
        /// </summary>
        private bool GetSelectTaskid()
        {
            if (taskInformlist.FocusedItem is null)
            {
                MessageBox.Show("请选择需要执行的任务", "提示");
                return true;
            }
            return false;
        }


        private void taskInformlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            string taskstatus = taskStatus[taskInformlist.FocusedItem.Text];
            if (taskstatus == "pausemission")
            {
                startmission.Enabled = true;
                endmission.Enabled = true;
                pausemission.Enabled = false;
            }
            else if (taskstatus == "startmission")
            {
                startmission.Enabled = false;
                endmission.Enabled = true;
                pausemission.Enabled = true;
            }
            else if (taskstatus == "endmission")
            {
                startmission.Enabled = false;
                endmission.Enabled = true;
                pausemission.Enabled = true;
            }
        }

        private void vehicleslist_SelectedIndexChanged(object sender, EventArgs e)
        {
            string status = agvStatus[vehicleslist.FocusedItem.Text];

            if (vehicleslist.FocusedItem.SubItems[2].Text == "True")
            {
                if (status == "forwardmove")
                {
                    agvForwordMove.Enabled = false;
                    agvBackMove.Enabled = true;
                    agvStop.Enabled = true;
                }
                else if (status == "backmove")
                {
                    agvForwordMove.Enabled = true;
                    agvBackMove.Enabled = false;
                    agvStop.Enabled = true;
                }
                else if (status == "stop")
                {
                    agvForwordMove.Enabled = true;
                    agvBackMove.Enabled = true;
                    agvStop.Enabled = true;
                }
                else if (status == "charge")
                {
                    agvForwordMove.Enabled = false;
                    agvBackMove.Enabled = false;
                    agvStop.Enabled = false;
                }
                else if (status == "endcharge")
                {
                    agvForwordMove.Enabled = true;
                    agvBackMove.Enabled = true;
                    agvStop.Enabled = true;
                }
                charge.Enabled = true;
                endcharge.Enabled = true;
                buttonSend.Enabled = true;
            }
            else if (vehicleslist.FocusedItem.SubItems[2].Text == "False")
            {
                agvForwordMove.Enabled = false;
                agvBackMove.Enabled = false;
                agvStop.Enabled = false;
                charge.Enabled = false;
                endcharge.Enabled = false;
                buttonSend.Enabled = false;
            }
        }
        private void RefreshListview(ListView listv)
        {
            //System.Diagnostics.Debug.WriteLine(listv.Name);

            if (listv.Name.Equals("vehicleslist") || listv.Name.Equals("alarmlist"))
            {
                GfxList<DeviceBackImf> devsList = JTWcfHelper.WcfMainHelper.GetDevList();
                if (devsList is null) return;
                listv.BeginUpdate();
                for (int i = 0; i < devsList.Count; i++)
                {
                    if (devsList[i].DevType == "AGV" && devsList[i].DevStatue == "True")
                    {
                        if (listv.Name.Equals("vehicleslist"))
                        {
                            DeviceBackImf dev = devsList.Find(c => { return c.DevId == listv.Items[i].SubItems[0].Text; });
                            if (dev is null)
                            {

                            }
                            else
                            {
                                listv.Items[i].SubItems[2].Text = dev.DevStatue;  // AGV在线状态
                                listv.Items[i].SubItems[4].Text = dev.SensorList[6].RValue;  // 电量
                                listv.Items[i].SubItems[5].Text = dev.SensorList[7].RValue;   // 充电状态                         
                            }

                        }
                        else if (listv.Name.Equals("alarmlist"))
                        {
                            int[] a = new int[] { 8, 10, 12, 13, 14, 15, 16, 20 };

                            for (int j = 0; j < a.Length; j++)
                            {

                                if (devsList[i].SensorList[a[j]].RValue == "1")
                                {
                                    DeviceBackImf dev = devsList.Find(c => { return c.DevId == listv.Items[i].SubItems[0].Text; });

                                    listv.Items[i].SubItems[3].Text = dev.SensorList[a[j]].RValue;
                                    //listv.Items[i].SubItems[3].Text = devsList[i].SensorList[a[j]].RValue;
                                }
                            }
                        }
                    }
                }
                listv.EndUpdate();
            }
        }
        /// <summary>
        /// 刷新当前任务
        /// </summary>
        private void RefreshtaskInform()
        {
            //GfxList<GfxServiceContractTaskExcute.TaskBackImf> tasksList = JTWcfHelper.WcfTaskHelper.GetAllTask();
            if (tasksList is null)
            {
                taskInformlist.Items.Clear();
                return;
            }
            taskInformlist.Items.Clear();

            taskInformlist.BeginUpdate();
            for (int i = 0; i < tasksList.Count; i++)
            {
                ListViewItem item = new ListViewItem(tasksList[i].DisGuid); // 任务id
                item.SubItems.Add(tasksList[i].TaskImf); // 任务信息
                item.SubItems.Add(tasksList[i].Statue.ToString()); // 任务状态
                item.SubItems.Add(tasksList[i].OrderSource); // 触发源
                item.SubItems.Add(tasksList[i].DisDevId); // 任务设备
                item.SubItems.Add(tasksList[i].PathMsg); // 任务路径
                item.SubItems.Add(tasksList[i].TaskCtrType.ToString()); // 控制参数，枚举类型
                item.SubItems.Add(tasksList[i].CurDisOrderMsg); // 指令信息
                item.SubItems.Add(tasksList[i].BackMsg); // 信息
                item.SubItems.Add(tasksList[i].TriggerTime.ToString()); // 触发时间

                if (taskStatus.ContainsKey(tasksList[i].DisGuid))
                {
                    taskStatus[tasksList[i].DisGuid] = "startmissioin";
                }
                else
                {
                    taskStatus.Add(tasksList[i].DisGuid, "startmission");
                }
                // 显示项
                taskInformlist.Items.Add(item);
            }
            taskInformlist.EndUpdate();
        }


        /// <summary>
        /// 定时刷新车辆信息
        /// </summary>
        private void Statetimer_Tick(object sender, EventArgs e)
        {
            if (devFirst && devOk)
            {
                VehiclesDisplay();//dev
                OtherdevDisplay();//dev
                AlarmDataDisplay();//dev
                devFirst = false;
            }

            if (taskFirst && taskOk)
            {
                AllTaskListDisplay();
                TaskInformDisplay();
                taskFirst = false;
            }
            switch (tabControl1.SelectedIndex)
            {
                case 0://0 missions
                    RefreshListview(taskInformlist);
                    RefreshtaskInform();
                    break;
                case 1://1 alarms
                    RefreshListview(alarmlist);
                    break;
                case 2://2 logger

                    break;
                case 3://3 devices

                    break;
                case 4://4 vehicles
                    RefreshListview(vehicleslist);
                    RefreshListview(otherdevlist);
                    break;
                default:

                    break;
            }
        }

        private void KEDAForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("确定关闭客户端？", "提示", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        private void executeTask_Click(object sender, EventArgs e)
        {
            //GfxList<TaskRelationMember> memberlist = JTWcfHelper.WcfTaskHelper.GetDefineTask();
            try
            {
                if (definetasklist != null)
                {
                    if (executeTasklist.SelectedItems == null)
                    {
                        MessageBox.Show("请选择相应的任务", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    else
                    {
                        WcfClientHelper.CreateService<IUserOperation_TaskExcute>().StartTask(executeTasklist.SelectedItems[0].SubItems[0].Text, "MainTest");
                    }
                }
            }
            catch
            {
                MessageBox.Show("错误", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

            RefreshtaskInform();
        }
        /// <summary>
        /// 触发循环任务
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            GfxList<TaskRelationMember> definetasklist = JTWcfHelper.WcfTaskHelper.GetDefineTask();
            //GfxList<GfxServiceContractTaskExcute.TaskBackImf> taskList = JTWcfHelper.WcfTaskHelper.GetAllTask();
            if (definetasklist == null && definetasklist.Count <= 0)
            {
                MessageBox.Show("当前没有已定义的任务", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                if (tasksList.Count <= 0)
                {
                    if (executeTasklist.FocusedItem == null)
                    {
                        MessageBox.Show("请选择相应的任务", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    else
                    {
                        WcfClientHelper.CreateService<IUserOperation_TaskExcute>().StartTask(executeTasklist.SelectedItems[0].SubItems[0].Text, "MainTest");

                        timer1.Enabled = true;
                    }
                }
                else
                {
                    if (tasksList.Count == 1 && tasksList[0].TaskImf == definetasklist[0].TaskRelatName)
                    {
                        WcfClientHelper.CreateService<IUserOperation_TaskExcute>().StartTask(executeTasklist.Items[1].SubItems[0].Text, "MainTest");
                    }
                    else if (tasksList.Count == 1 && tasksList[0].TaskImf == definetasklist[1].TaskRelatName)
                    {
                        WcfClientHelper.CreateService<IUserOperation_TaskExcute>().StartTask(executeTasklist.Items[0].SubItems[0].Text, "MainTest");
                    }
                    timer1.Enabled = true;
                }
            }

            RefreshtaskInform();
        }

        /// <summary>
        /// 结束循环任务
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            JTWcfHelper.WcfMainHelper.InitPara(_severIp, "", "");

            GfxList<TaskBackImf> result = JTWcfHelper.WcfTaskHelper.GetAllTask();
            if (result != null)
            {
                foreach (var item in result)
                {
                    ResultTypeEnum s = JTWcfHelper.WcfMainHelper.GetDipatchStatue(item.DisGuid);

                    //JTWcfHelper.WcfMainHelper.CtrDispatch(item.DisGuid, DisOrderCtrTypeEnum.Stop);
                    JTWcfHelper.WcfTaskHelper.AdminCtrTask(item.DisGuid, DisOrderCtrTypeEnum.Stop);
                }

                timer1.Enabled = false;
            }
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        private void Refreshtask()
        {
            //GfxList<TaskRelationMember> definetasklist = JTWcfHelper.WcfTaskHelper.GetDefineTask();
            //GfxList<GfxServiceContractTaskExcute.TaskBackImf> taskList = JTWcfHelper.WcfTaskHelper.GetAllTask();
            if (tasksList.Count == 1 && tasksList[0].TaskImf == definetasklist[0].TaskRelatName)
            {
                WcfClientHelper.CreateService<IUserOperation_TaskExcute>().StartTask(executeTasklist.Items[1].SubItems[0].Text, "MainTest");
            }
            else if (tasksList.Count == 1 && tasksList[0].TaskImf == definetasklist[1].TaskRelatName)
            {
                WcfClientHelper.CreateService<IUserOperation_TaskExcute>().StartTask(executeTasklist.Items[0].SubItems[0].Text, "MainTest");
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabControl1SelectIndex = tabControl1.SelectedIndex;
        }

        /// <summary>
        /// 定时执行任务
        /// </summary>
        private void timer1_Tick(object sender, EventArgs e)
        {
            timerFunc.Enabled = false;

            try
            {
                Refreshtask();
            }
            catch { }

            timerFunc.Enabled = true;

        }

        /// <summary>
        /// 后台线程去更新获取设备，任务信息
        /// </summary>
        private void queryDataThreadFunc()
        {
            while (true)
            {
                if (!JTWcfHelper.WcfTaskHelper.IsConnected || !JTWcfHelper.WcfDispatchHelper.Open())
                {
                    Thread.Sleep(1000);
                    continue;
                }
                else
                {
                    ////全局的列表数据变量
                    //GfxList<DeviceBackImf> devsList;                    // = JTWcfHelper.WcfMainHelper.GetDevList();
                    //GfxList<GfxServiceContractTaskExcute.TaskBackImf> taskList;// = JTWcfHelper.WcfTaskHelper.GetAllTask();
                    //GfxList<TaskRelationMember> definetasklist;// = JTWcfHelper.WcfTaskHelper.GetDefineTask();

                    //Boolean devDataChange, taskDataChange, definetaskDataChange;
                    devsList = JTWcfHelper.WcfMainHelper.GetDevList();
                    devOk = true;
                    switch (tabControl1SelectIndex)
                    {
                        case 0://0 task
                            tasksList = JTWcfHelper.WcfTaskHelper.GetAllTask();
                            definetasklist = JTWcfHelper.WcfTaskHelper.GetDefineTask();
                            taskOk = true;
                            break;
                        case 1://1 alarms
                            
                            
                            break;
                        case 2://2 logger

                            break;
                        case 3://3 devices
                            //devsList = JTWcfHelper.WcfMainHelper.GetDevList();
                            //devOk = true;
                            break;
                        case 4://4 vehicles
                            //devsList = JTWcfHelper.WcfMainHelper.GetDevList();
                            //devOk = true;
                            break;
                        default:

                            break;
                    }
                    Thread.Sleep(1000);
                }
            }
        }

        /// <summary>
        /// 判断窑尾是否有货
        /// </summary>
        private void LoadStaHasGoods()
        {
            while (true)
            {
                Thread.Sleep(2000);
                if (unloadState == 0)
                {
                    // 判断窑尾是否有货           
                    if (devsList != null && devsList.Count != 0)
                    {
                        DeviceBackImf dev = devsList.Find(c => { return c.DevType == "PLC" && c.DevStatue == "True" && c.SensorList[0].RValue == "1" && c.DevId == "PLC0000001"; });

                        // PLC 在线，且货物状态为1
                        if (dev != null)
                        {
                            _loadStaHasGoods = true;
                            unloadState = 1;
                        }
                        else
                        {
                            _loadStaHasGoods = false;
                        }

                    }

                }
                
            }

        }


        /// <summary>
        /// 判断装载等待位是否有空闲的AGV
        /// </summary>
        private void HasFreeAGV()
        {
            while (true)
            {
                Thread.Sleep(2000);
                if (_loadStaHasGoods && unloadState == 1)
                {

                    if (devsList != null && devsList.Count != 0)
                    {
                        DeviceBackImf dev = devsList.Find(c => { return c.DevType == "AGV" && c.DevStatue == "True" && c.SensorList[1].RValue == _loadWaitSta && c.SensorList[9].RValue == "true" && c.SensorList[7].RValue == "0"; });

                        // 只有位于等待区首位置的在线、空闲、未充电的AGV 才能执行任务
                        if (dev != null)
                        {
                            _HasFreeAGV = true;
                            unloadState = 2;

                        }
                        else
                        {
                            _HasFreeAGV = false;
                        }

                    }
                }
                
            }
        }

        /// <summary>
        /// AGV从当前窑尾装载等待位到窑尾装载点
        /// </summary>
        private void SendAGVtoLoadSta()
        {
            while (true)
            {
                Thread.Sleep(2000);
                if (_HasFreeAGV && unloadState == 2)
                {
                    // 自定义一个任务：AGV从当前等待位到窑尾装载点
                    OnceTaskMember task = new OnceTaskMember();

                    ////任务ID
                    //task.DisGuid = "装货";

                    //任务名称
                    task.TaskRelatName = "AGV从当前等待位到窑尾装载点";

                    //任务完成是否自动清除
                    task.IsAotuRemove = false;

                    //任务中一个调度节点
                    DispatchOrderObj dis = new DispatchOrderObj();

                    //调度的终点（地标）：窑尾装载点
                    dis.EndSite = _loadgoodsSta;

                    task.DisOrderList.Add(dis);

                    JTWcfHelper.WcfTaskHelper.StartTaskTemp("AGV去窑尾接货", task);

                    unloadState = 3;

                }
                
            }

        }

        /// <summary>
        /// 判断窑尾装载点是否有AGV
        /// </summary>
        private void LoadStaHasAGV()
        {
            while (true)
            {

                Thread.Sleep(2000);
                if (unloadState == 3 && devsList != null && devsList.Count != 0)
                {
                    DeviceBackImf dev = devsList.Find(c => { return c.DevType == "AGV" && c.SensorList[1].RValue == _loadgoodsSta; });

                    //  窑尾装载点有没有AGV,若有直接跳出遍历
                    if (dev != null)
                    {
                        _loadStaHasAGV = true;
                        unloadState = 4;

                    }
                    else
                    {
                        _loadStaHasAGV = false;
                    }

                }

            }
        }

        /// <summary>
        ///WCS给线边辊台 PLC0000001 发送下料、电机正转的指令
        /// </summary>
        private void LoadLineRollerTable()
        {
            while (true)
            {
                Thread.Sleep(2000);
                if (_loadStaHasAGV && unloadState == 4)
                {
                    if (devsList != null && devsList.Count != 0)
                    {
                        DeviceBackImf dev = devsList.Find(c => { return c.DevId == "PLC0000001" && c.DevStatue == "True"; });
                        // 装载点有AGV，线边辊台准备出料
                        if (dev != null)
                        {
                            // 线边辊台下料正转  Order1: 2,1
                            JTWcfHelper.WcfMainHelper.SendOrder(dev.DevId, new CommonDeviceOrderObj("线边辊台下料 正转" + LocSite, 1, 2, 1));
                            unloadState = 5;

                        }

                    }
                }
            
            }
        }

        /// <summary>
        ///接货流程
        ///判断装载完成后，AGV上是否有货
        /// </summary>
        /// <returns></returns>
        private void AGVHasGoods()
        {
            while (true)
            {
                Thread.Sleep(2000);
                if (unloadState == 5)
                {
                    if (devsList != null && devsList.Count != 0)
                    {
                        DeviceBackImf dev = devsList.Find(c => { return c.SensorList[1].RValue == _loadgoodsSta && c.SensorList[35].RValue == "1" && c.DevType == "AGV"; });


                        //  判断处于装载位的AGV上是否有货,若有直接跳出遍历
                        if (dev != null)
                        {
                            _AGVHasGoods = true;
                            unloadState = 6;                          

                        }
                        else
                        {
                            _AGVHasGoods = false;
                        }

                    }
                }
            
            }
        }

        /// <summary>
        ///WCS给线边辊台 PLC0000001  发送下料、电机停止的指令
        /// </summary>
        private void LoadLineRollerTableStop()
        {
            while (true)
            {
                Thread.Sleep(2000);
                if (_AGVHasGoods && unloadState == 6)
                {
                    if (devsList != null && devsList.Count != 0)
                    {
                        DeviceBackImf dev = devsList.Find(c => { return c.DevId == "PLC0000001"; });
                        // 装载点有AGV，线边辊台准备出料
                        if (dev != null && dev.DevStatue == "True")
                        {
                            // 线边辊台上料正转  Order1: 1,3
                            JTWcfHelper.WcfMainHelper.SendOrder(dev.DevId, new CommonDeviceOrderObj("线边辊台下料 停止" + LocSite, 1, 2, 3));

                            unloadState = 7;
                            

                        }
                    }
                }
           
            }
        }

        /// <summary>
        /// 同时，AGV从窑尾装载点到窑头卸载等待区
        /// </summary>
        private void SendAGVtoUnloadWaitSta()
        {
            while (true)
            {
                Thread.Sleep(2000);
                if (unloadState ==7)
                {
                    // 自定义一个任务：AGV从窑尾装载点到窑头卸载等待区
                    OnceTaskMember task = new OnceTaskMember();

                    ////任务ID
                    //task.DisGuid = "去窑头卸载等待区";

                    //任务名称
                    task.TaskRelatName = "AGV从窑尾装载点到窑头卸载等待区";

                    //任务完成是否自动清除
                    task.IsAotuRemove = false;

                    //任务中一个调度节点
                    DispatchOrderObj dis = new DispatchOrderObj();

                    //调度的终点（地标）：窑尾装载点
                    dis.EndSite = _unloadWaitSta;

                    task.DisOrderList.Add(dis);

                    JTWcfHelper.WcfTaskHelper.StartTaskTemp("窑头卸载等待区", task);

                    unloadState = 0;
                }
             
            }

        }

        /// <summary>
        /// 判断窑头卸载等待位是否有准备卸货的AGV
        /// </summary>      
        private void UnloadWaitStaHasAGV()
        {
            while (true)
            {
                Thread.Sleep(2000);
                if (devsList != null && devsList.Count != 0 && loadState == 0)
                {
                    DeviceBackImf dev = devsList.Find(c => { return c.DevType == "AGV" && c.SensorList[1].RValue == _unloadWaitSta && c.SensorList[35].RValue == "1"; });

                    //  窑头卸载等待点有没有准备卸货的 AGV,若有直接跳出遍历
                    if (dev != null)
                    {
                        _HasUnloadAGV = true;
                        loadState = 1;

                    }
                    else
                    {
                        _HasUnloadAGV = false;
                    }

                }
              
            }

        }

        /// <summary>
        /// 判断窑头卸载辊台上是否有货
        /// </summary>
        private void UnloadStaHasGoods()
        {
            while (true)
            {
                Thread.Sleep(2000);
                if (_HasUnloadAGV && loadState == 1)
                {
                    //判断窑头卸载辊台上是否有货
                    if (devsList != null && devsList.Count != 0)
                    {
                        DeviceBackImf dev = devsList.Find(c => { return c.DevType == "PLC" && c.DevStatue == "True" && c.SensorList[0].RValue == "1" && c.DevId == "PLC0000002"; });
                        {
                            // PLC 在线，且货物状态为1
                            if (dev != null)
                            {
                                _unloadStaHasGoods = true;
                                loadState = 2;

                            }
                            else
                            {
                                _unloadStaHasGoods = false;
                            }
                        }
                    }
                }
               
            }

        }

        /// <summary>
        /// WCS给AGV下发任务：从卸载等待位到窑头卸载点
        /// </summary>
        private void SendAGVtoUnloadSta()
        {
            while (true)
            {
                Thread.Sleep(2000);
                if (!_unloadStaHasGoods && loadState == 2)
                {
                    // 自定义一个任务：AGV从卸载等待位到窑头卸载点
                    OnceTaskMember task = new OnceTaskMember();

                    ////任务ID
                    //task.DisGuid = "去窑头卸载点";

                    //任务名称
                    task.TaskRelatName = "AGV从卸载等待位到窑头卸载点";

                    //任务完成是否自动清除
                    task.IsAotuRemove = false;

                    //任务中一个调度节点
                    DispatchOrderObj dis = new DispatchOrderObj();

                    //调度的终点（地标）：窑头卸载点
                    dis.EndSite = _unloadgoodsSta;

                    task.DisOrderList.Add(dis);

                    JTWcfHelper.WcfTaskHelper.StartTaskTemp("去窑头卸载点", task);

                    loadState = 3;

                }
              
            }

        }

        /// <summary>
        /// 窑头卸载点是否有有货状态的AGV
        /// </summary>
        private void UnloadStaHasAGV()
        {
            while (true)
            {
                Thread.Sleep(2000);
                if (devsList != null && devsList.Count != 0 && loadState == 3)
                {
                    DeviceBackImf dev = devsList.Find(c => { return c.DevType == "AGV" && c.SensorList[1].RValue == _unloadgoodsSta && c.SensorList[35].RValue == "1"; });

                    //  窑尾装载点有没有有货状态的AGV,若有直接跳出遍历
                    if (dev != null)
                    {
                        _unloadStaHasAGV = true;
                        loadState = 4;

                    }
                    else
                    {
                        _unloadStaHasAGV = false;
                    }

                }
              
            }

        }

        /// <summary>
        ///WCS给线边辊台发送上料、电机正转的指令
        /// </summary>
        private void UnloadLineRollerTable()
        {
            while (true)
            {
                Thread.Sleep(2000);
                if (_unloadStaHasAGV && loadState == 4)
                {
                    if (devsList != null && devsList.Count != 0)
                    {
                        DeviceBackImf dev = devsList.Find(c => { return c.DevType == "PLC" && c.DevStatue == "True" && c.DevId == "PLC0000002"; });

                        // 装载点有有货的AGV，线边辊台准备接货
                        if (dev != null)
                        {
                            // 线边辊台上料 正转  Order1: 1,1
                            JTWcfHelper.WcfMainHelper.SendOrder(dev.DevId, new CommonDeviceOrderObj("线边辊台上料 停止" + LocSite, 1, 1, 1));

                            Thread.Sleep(3000);
                            // 判断线边辊台电机是否正在转动
                            if (dev.SensorList[1].RValue == "1")
                            {
                                _isRorate = true;
                                loadState =5;

                            }
                            else
                            {
                                _isRorate = false;
                            }
                        }

                    }
                }
                
            }
        }

        /// <summary>
        /// 启动车载辊台转动卸货
        /// </summary>
        private void ReadyToUnload()
        {
            while (true)
            {
                Thread.Sleep(2000);
                if (_isRorate && loadState == 5)
                {
                    if (devsList != null && devsList.Count != 0)
                    {
                        DeviceBackImf dev = devsList.Find(c => { return c.DevType == "AGV" && c.DevStatue == "True" && c.SensorList[1].RValue == _unloadgoodsSta && c.SensorList[9].RValue == "true" && c.SensorList[35].RValue == "1"; });

                        // 启动窑头卸载点的AGV 车载辊台转动卸货
                        if (dev != null)
                        {
                            JTWcfHelper.WcfMainHelper.SendOrder(dev.DevId, new CommonDeviceOrderObj("车载辊台下料 正转" + LocSite, 1, 2, 1));
                            loadState = 6;

                        }

                    }
                }
              
            }

        }

        /// <summary>
        ///  启动服务
        /// </summary>
        private void startServer_Click(object sender, EventArgs e)
        {
            
            F_DataCenter.Init(SynchronizationContext.Current,listBoxOutput);
            SetOutputMsg("服务启动");
            startServer.Enabled = false;
            stopServer.Enabled = true;
        }

        /// <summary>
        ///  停止服务
        /// </summary>
        private void stopServer_Click(object sender, EventArgs e)
        {
            F_DataCenter.StopServer();
            SetOutputMsg("服务停止");
            startServer.Enabled = true;
            stopServer.Enabled = false;
        }

        /// <summary>
        ///  判断执行卸货任务后的AGV，货物状态是否为无货
        /// </summary>
        private void AGVGoodsStatue()
        {
            while (true)
            {
                Thread.Sleep(2000);
                if (devsList != null && devsList.Count != 0 && loadState == 6)
                {
                    DeviceBackImf dev = devsList.Find(c => { return c.DevType == "AGV" && c.SensorList[35].RValue == "0"; });

                    // 判断AGV上的货物状态是否为无货
                    if (dev != null)
                    {
                        _AGVGoodsStatue = false;
                        loadState = 7;

                    }
                    else
                    {
                        _AGVGoodsStatue = true;
                    }

                }
               
            }
        }

        /// <summary>
        ///WCS给线边辊台发送上料、电机停止的指令
        /// </summary>
        private void UnloadLineRollerTableStop()
        {
            while (true)
            {
                Thread.Sleep(2000);
                if (!_AGVGoodsStatue && loadState == 7)
                {
                    if (devsList != null && devsList.Count != 0)
                    {
                        DeviceBackImf dev = devsList.Find(c => { return c.DevType == "PLC" && c.DevId == "PLC0000002" && c.DevStatue == "True"; });

                        // 装载点有AGV，线边辊台准备出料
                        if (dev != null)
                        {
                            // 线边辊台上料  停止  Order1: 1,3
                            JTWcfHelper.WcfMainHelper.SendOrder(dev.DevId, new CommonDeviceOrderObj("线边辊台上料 停止" + LocSite, 1, 1, 3));

                            loadState = 8;

                        }

                    }
                }
             
            }
        }

        /// <summary>
        /// 同时，AGV从窑头卸载点到窑尾装载等待区
        /// </summary>
        private void SendAGVtoLoadWaitSta()
        {
            while (true)
            {
                Thread.Sleep(2000);
                if (loadState == 8)
                {
                    // 自定义一个任务：AGV从窑头卸载点到窑尾装载等待区
                    OnceTaskMember task = new OnceTaskMember();

                    ////任务ID
                    //task.DisGuid = "去窑尾装载等待区";

                    //任务名称
                    task.TaskRelatName = "AGV从窑头卸载点到窑尾装载等待区";

                    //任务完成是否自动清除
                    task.IsAotuRemove = false;

                    //任务中一个调度节点
                    DispatchOrderObj dis = new DispatchOrderObj();

                    //调度的终点（地标）：窑尾装载等待区
                    dis.EndSite = _loadWaitSta;

                    task.DisOrderList.Add(dis);

                    JTWcfHelper.WcfTaskHelper.StartTaskTemp("窑尾装载等待区", task);

                    loadState = 0;
                }

              
            }

        }
    }
}