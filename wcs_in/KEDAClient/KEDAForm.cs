using Gfx.GfxDataManagerServer;
using Gfx.RCommData;
using GfxCommonInterfaces;
using GfxServiceContractClient;
using GfxServiceContractTaskExcute;
using JTWcfHelper;
using LogHelper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
        /// 已经选择的站点集合(车辆）
        /// </summary>
        private List<StationMember> _selectStation1 = new List<StationMember>();


        /// <summary>
        /// 任务ID与自身状态对应关系：状态值：startmission、pausemission、endmission
        /// </summary>
        //  Dictionary<string, string> taskStatus = new Dictionary<string, string>();

        /// <summary>
        /// 设备ID与自身状态对应关系(车辆）：状态值：stop、forwardmove、backmove
        /// </summary>
        Dictionary<string, string> agvStatus = new Dictionary<string, string>();


        private readonly DeviceBackImf devid;

        //用于查询数据库数据的线程
        private Thread queryDataThread;

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
        List<DispatchBackMember> dislist;
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
        /// 当前调度列表选中项
        /// </summary>
        private string dispatchliselect = "";

        /// <summary>
        /// 当前调度列表选中项索引
        /// </summary>
        private int _dispatSelectIndex = 0;

        private string taskInformliselect = "";
        private int _taskInformliSelectIndex = 0;

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

            comboBox1.SelectedIndex = 0;

            Alarm(); // 报警
            Logger(); // 日志
            Otherdev(); // 设备
            Vehicles(); //车辆
            DispatchTask(); // 调度任务        
            CurrentTaskInform(); // 当前任务
            AllTaskList(); //任务列表
            toolStripLabelVersion.Text = "版本号：V14.1";  //版本号
            timerFunc.Enabled = true;  // 系统时间
            labelLogo.Text = APPConfig.LogoStr();  //公司名称        

            //启动作业线程
            //F_DataCenter.Init();

            LogFactory.Init();//日志初始化

            LogFactory.LogRunning("启动工具");

            this.WindowState = FormWindowState.Maximized;

            queryDataThread = new Thread(queryDataThreadFunc);
            queryDataThread.IsBackground = true;
            queryDataThread.Start();


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
            vehicleslist.Columns.Add("地标", 150, HorizontalAlignment.Left);
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
                    int[] sens = new int[] { 0, 1, 4, 6, 7 };
                    ListViewItem item = new ListViewItem(item1.DevId); // 车辆编号
                    item.SubItems.Add(item1.DevModel); // 车辆型号   
                    item.SubItems.Add(item1.DevStatue); // 车辆状态    
                    item.SubItems.Add(item1.SensorList[sens[0]].RValue); // 运行状态  
                    item.SubItems.Add(item1.SensorList[sens[1]].RValue); // 地标   
                    item.SubItems.Add(item1.SensorList[sens[3]].RValue); // 电量
                    item.SubItems.Add(item1.SensorList[sens[4]].RValue); // 充电状态

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
        private void CurrentTaskInform()
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
        /// 调度任务列表
        /// </summary>
        private void DispatchTask()
        {
            //添加表头，即列名
            dispatchlist.Columns.Add("调度id", 200, HorizontalAlignment.Center);
            dispatchlist.Columns.Add("调度信息", 100, HorizontalAlignment.Left);
            dispatchlist.Columns.Add("调度状态", 150, HorizontalAlignment.Left);
            dispatchlist.Columns.Add("触发源", 200, HorizontalAlignment.Left);
            dispatchlist.Columns.Add(" 调度设备", 150, HorizontalAlignment.Left);
            dispatchlist.Columns.Add("调度路径", 150, HorizontalAlignment.Left);
            dispatchlist.Columns.Add(" 控制参数", 100, HorizontalAlignment.Left);
            dispatchlist.Columns.Add(" 备注", 150, HorizontalAlignment.Left);

            dispatchlist.View = System.Windows.Forms.View.Details;
        }


        /// <summary>
        /// 调度任务列表展示
        /// </summary>
        private void DispatchListDisplay()
        {
            //List<DispatchBackMember> dislist = WcfMainHelper.GetDispatchList();
            if (dislist is null)
            {
                dispatchlist.Clear();
                return;
            }
            dispatchlist.BeginUpdate();

            foreach (var item1 in dislist)
            {
                ListViewItem item = new ListViewItem(item1.DisGuid); // 调度id
                                                                     // item.SubItems.Add(item1.TaskImf); // 调度信息
                item.SubItems.Add(" 测试"); // 调度信息
                item.SubItems.Add(item1.OrderStatue.ToString()); // 调度状态
                item.SubItems.Add(item1.OrderSource); // 触发源
                item.SubItems.Add(item1.DisDevId); // 调度设备
                item.SubItems.Add(item1.PathMsg); // 调度路径
                item.SubItems.Add(item1.TaskCtrType.ToString()); // 控制参数，枚举类型             
                item.SubItems.Add(item1.BackMsg); // 备注       

                dispatchlist.Items.Add(item);
            }

            // 结束数据处理
            dispatchlist.EndUpdate();
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
        /// 输出系统信息（任务）
        /// </summary>
        /// <param name="msg"></param>
        public void SetOutputMsg(ListBox listbox, string msg)
        {
            if (string.IsNullOrEmpty(msg)) { msg = "空消息"; }

            if (listbox.Items.Count > 200)
            {
                listbox.Items.RemoveAt(0);
            }

            listbox.Items.Add(string.Format("【{0}】：{1}", DateTime.Now.TimeOfDay.ToString(), msg));

            listbox.SelectedIndex = listbox.Items.Count - 1;
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
        ///  刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitPara();

            _selectStation.Clear();
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
        /// 指令发送（车辆界面）
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
                    SetOutputMsg(listBox1, "发送 " + this.comboBox1.Text + " 指令参数成功");
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
            // 1是快速停止、0是慢速
            if (JTWcfHelper.WcfMainHelper.SendOrder(vehicleslist.FocusedItem.Text, new CommonDeviceOrderObj("停止" + LocSite, 2, 0)))
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
        /// 自定义一个任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void defineDispatch_Click(object sender, EventArgs e)
        {
            if (GetSelectDevid())
            {
                return;
            }
            // 判断选定的AGV是否在线，若在线才可执行自定义任务
            else if (vehicleslist.FocusedItem.SubItems[2].Text == "True")
            {
                defineDispatch.Enabled = true;

                OnceTaskMember task = new OnceTaskMember();

                //任务ID
                task.DisGuid = "自定义任务ID";

                //任务名称
                task.TaskRelatName = "自定义一个调度任务";

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
                    SetOutputMsg(listBox1, "自定义一个调度任务");
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
            switch (tabControl2.SelectedIndex)
            {
                //调度任务列表
                case 0:
                    if (GetSelectDispatchid())
                    {
                        return;
                    }
                    else if (WcfMainHelper.CtrDispatch(dispatchliselect, DisOrderCtrTypeEnum.Start))
                    {
                        SetOutputMsg(listBoxOutput, "开始任务成功");
                    }
                    break;
                //当前任务列表
                case 1:
                    if (GetSelectTaskid())
                    {
                        return;
                    }
                    else if (WcfTaskHelper.AdminCtrTask(taskInformliselect , DisOrderCtrTypeEnum.Start))
                    {
                        SetOutputMsg(listBoxOutput, "开始任务成功");
                    }
                    break;
            }
        }


        /// <summary>
        /// 任务暂停
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pausemission_Click(object sender, EventArgs e)
        {
            switch (tabControl2.SelectedIndex)
            {
                // 调度任务
                case 0:
                    if (dislist != null && dislist.Count != 0)
                    {
                        if (GetSelectDispatchid())
                        {
                            return;
                        }
                        else
                        {
                            // 任务执行状态： unkonw  err  unready wait fail  doing  suc cancl
                            string taskrunstatus = dispatchlist.FocusedItem.SubItems[2].Text;
                            if (taskrunstatus == "suc" || taskrunstatus == "cancl")
                            {

                                MessageBox.Show("该任务已执行成功或取消，无法暂停！", "提示");
                            }
                            else if (MessageBox.Show("当前站点派发任务未完成，是否暂停？", "提示", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            {
                                if (WcfMainHelper.CtrDispatch(dispatchliselect, DisOrderCtrTypeEnum.Pause))
                                {
                                    SetOutputMsg(listBoxOutput, "任务暂停成功！");
                                }
                            }
                        }
                    }
                    break;
                // 当前任务
                case 1:
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
                                if (WcfTaskHelper.AdminCtrTask(taskInformliselect, DisOrderCtrTypeEnum.Pause))
                                {
                                    SetOutputMsg(listBoxOutput, "任务暂停成功！");
                                }
                            }
                        }
                    }
                    break;
            }

        }

        /// <summary>
        /// 任务结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void endmission_Click(object sender, EventArgs e)
        {
            switch (tabControl2.SelectedIndex)
            {
                // 调度任务列表
                case 0:
                    if (dislist != null && dislist.Count != 0)
                    {
                        if (GetSelectDispatchid())
                        {
                            return;
                        }
                        else
                        {
                            if (WcfMainHelper.CtrDispatch(dispatchliselect, DisOrderCtrTypeEnum.Stop))
                            {
                                SetOutputMsg(listBoxOutput, "结束任务成功");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("当前没有正在执行的任务！");
                    }
                    break;
                // 当前任务列表
                case 1:
                    //GfxList<TaskBackImf> taskList = JTWcfHelper.WcfTaskHelper.GetAllTask();
                    if (tasksList != null && tasksList.Count != 0)
                    {
                        if (GetSelectTaskid())
                        {
                            return;
                        }
                        else
                        {
                            if (WcfTaskHelper.AdminCtrTask(taskInformliselect, DisOrderCtrTypeEnum.Stop))
                            {
                                SetOutputMsg(listBoxOutput, "结束任务成功");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("当前没有正在执行的任务！");
                    }
                    break;
            }

        }


        /// <summary>
        /// 判断是否已选择某一任务id
        /// </summary>
        private bool GetSelectTaskid()
        {
            if (taskInformliselect  == null)
            {
                MessageBox.Show("请选择需要执行的任务", "提示");
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断是否已选择某一调度任务id
        /// </summary>
        private bool GetSelectDispatchid()
        {
            if (dispatchliselect == null)
            {
                MessageBox.Show("请选择需要执行的调度任务", "提示");
                return true;
            }
            return false;
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
            }
            else if (vehicleslist.FocusedItem.SubItems[2].Text == "False")
            {
                agvForwordMove.Enabled = false;
                agvBackMove.Enabled = false;
                agvStop.Enabled = false;
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
                                // 判断AGV是停止还是运行，1为运行、3为停止
                                if (dev.SensorList[0].RValue == "1")
                                {
                                    // 运行方向： 0 前进  1后退 
                                    if (dev.SensorList[ConstSetBA.运行方向].RValue == "0")
                                    {
                                        listv.Items[i].SubItems[3].Text = "前进"; // 运行状态：前进
                                    }
                                    else
                                    {
                                        listv.Items[i].SubItems[3].Text = "后退"; // 运行状态：后退
                                    }
                                }
                                else
                                {
                                    listv.Items[i].SubItems[3].Text = "停止";  // 运行状态：停止
                                }
                                listv.Items[i].SubItems[4].Text = dev.SensorList[1].RValue;  // 地标
                                listv.Items[i].SubItems[5].Text = dev.SensorList[6].RValue;  // 电量
                                // 充电状态            
                                if (dev.SensorList[ConstSetBA.充电状态].RValue == "1")
                                {
                                    listv.Items[i].SubItems[6].Text = "正充电";
                                }
                                else if (dev.SensorList[ConstSetBA.充电状态].RValue == "2")
                                {
                                    listv.Items[i].SubItems[6].Text = "充电完成";
                                }
                                else if (dev.SensorList[ConstSetBA.充电状态].RValue == "3")
                                {
                                    listv.Items[i].SubItems[6].Text = "未充电";
                                }
                                else
                                {
                                    listv.Items[i].SubItems[6].Text = "未知";
                                }
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

                // 显示项
                taskInformlist.Items.Add(item);
            }
            taskInformlist.EndUpdate();
        }

        /// <summary>
        /// 刷新调度任务信息
        /// </summary>
        private void RefreshdispatchInform()
        {
            if (dislist is null)
            {
                dispatchlist.Items.Clear();
                return;
            }
            dispatchlist.Items.Clear();

            dispatchlist.BeginUpdate();
            foreach (var item1 in dislist)
            {
                ListViewItem item = new ListViewItem(item1.DisGuid); // 调度id
                item.SubItems.Add(item1.TaskImf); // 调度信息
                item.SubItems.Add(item1.OrderStatue.ToString()); // 调度状态
                item.SubItems.Add(item1.OrderSource); // 触发源
                item.SubItems.Add(item1.DisDevId); // 调度设备
                item.SubItems.Add(item1.PathMsg); // 调度路径
                item.SubItems.Add(item1.TaskCtrType.ToString()); // 控制参数，枚举类型             
                item.SubItems.Add(item1.BackMsg); // 备注       

                dispatchlist.Items.Add(item);
            }

            // 结束数据处理
            dispatchlist.EndUpdate();

            // dispatchlist.Items[_dispatSelectIndex].Selected = true;

        }

        /// <summary>
        /// 调度任务列表选择改变
        /// </summary>
        private void dispatchlist_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (dispatchlist.FocusedItem is null)
            {
                dispatchliselect = null;
            }
            else
            {
                _dispatSelectIndex = dispatchlist.Items.IndexOf(dispatchlist.FocusedItem);
                dispatchliselect = dispatchlist.FocusedItem.Text;
            }

        }

        /// <summary>
        /// 当前任务列表选择改变
        /// </summary>
        private void taskInformlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (taskInformlist.FocusedItem is null)
            {
                taskInformliselect = null;
            }
            else
            {
                _taskInformliSelectIndex = taskInformlist.Items.IndexOf(taskInformlist.FocusedItem);
                taskInformliselect = taskInformlist.FocusedItem.Text;
            }
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
                DispatchListDisplay();
                taskFirst = false;
            }
            switch (tabControl1.SelectedIndex)
            {
                case 0://0 missions
                    RefreshListview(taskInformlist);
                    RefreshtaskInform();
                    RefreshdispatchInform();
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
        ///  启动服务
        /// </summary>
        private void startServer_Click(object sender, EventArgs e)
        {

            F_DataCenter.Init(SynchronizationContext.Current, listBoxOutput);
            SetOutputMsg(listBoxOutput, "服务启动");
            startServer.Enabled = false;
            stopServer.Enabled = true;

            LogFactory.LogRunning("服务启动");
        }

        /// <summary>
        ///  停止服务
        /// </summary>
        private void stopServer_Click(object sender, EventArgs e)
        {
            F_DataCenter.StopServer();
            SetOutputMsg(listBoxOutput, "服务停止");
            startServer.Enabled = true;
            stopServer.Enabled = false;

            LogFactory.LogRunning("服务停止");
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void executeTask_Click(object sender, EventArgs e)
        {
            try
            {
                if (definetasklist!=null )
                {
                    if (executeTasklist.SelectedItems==null)
                    {
                        MessageBox.Show("请选择相应的任务","系统提示",MessageBoxButtons.OK,MessageBoxIcon.Asterisk );
                    }
                    else
                    {
                        WcfClientHelper.CreateService<IUserOperation_TaskExcute>().StartTask(executeTasklist.SelectedItems[0].SubItems[0].Text, "MainTest");

                        SetOutputMsg(listBoxOutput, executeTasklist.SelectedItems[0].SubItems[1].Text);

                        LogFactory.LogDispatch(executeTasklist.SelectedItems[0].SubItems[0].Text, "执行任务", executeTasklist.SelectedItems[0].SubItems[1].Text);

                    }
                }
            }
            catch
            {
                MessageBox.Show("错误","系统提示",MessageBoxButtons.OK,MessageBoxIcon.Hand );
            }
            RefreshtaskInform();
        }


        /// <summary>
        /// 初始化按钮（任务界面）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void initButton_Click(object sender, EventArgs e)
        {
            F_Logic f_logic = F_DataCenter.MLogic;
            if (f_logic != null)
            {
                f_logic.initButton();
            }
        }


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabControl1SelectIndex = tabControl1.SelectedIndex;
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


                    //Boolean devDataChange, taskDataChange, definetaskDataChange;
                    devsList = JTWcfHelper.WcfMainHelper.GetDevList();
                    devOk = true;
                    switch (tabControl1SelectIndex)
                    {
                        case 0://0 task
                            dislist = WcfMainHelper.GetDispatchList();
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
}