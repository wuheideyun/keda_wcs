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

            Alarm(); // 报警
            Logger(); // 日志
            Devices(); // 设备
            Vehicles(); //车辆
            TaskInform(); // 任务列表
            toolStripLabelVersion.Text = "版本号：V14.1";  //版本号
            timerFunc.Enabled = true;  // 系统时间
            labelLogo.Text = APPConfig.LogoStr();  //公司名称
            UpdateBtnMember();

            this.WindowState = FormWindowState.Maximized;
        }


        /// <summary>
        /// 报警
        /// </summary>
        private void Alarm()
        {

            // 创建列表头
            alarmlist.Columns.Add("传感器ID", 150, HorizontalAlignment.Left);
            alarmlist.Columns.Add("类型名称", 200, HorizontalAlignment.Left);
            alarmlist.Columns.Add("通道编号", 200, HorizontalAlignment.Left);
            alarmlist.Columns.Add("实时值", 200, HorizontalAlignment.Left);
            alarmlist.Columns.Add("描述值", 200, HorizontalAlignment.Left);

            // 若没有，无法显示数据 
            alarmlist.View = System.Windows.Forms.View.Details;

            ///添加数据项
            ///UI暂时挂起，直到EndUpdate绘制控件，可提高加载速度

            JtWcfMainHelper.InitPara(_severIp, "", "");

            List<DeviceBackImf> devs = JtWcfMainHelper.GetDevList();

            // 传感器
            SensorBackImf sens = null;

            alarmlist.BeginUpdate();

            if (devs != null && devs.Count > 0)
            {
                foreach (var item1 in devs)
                {
                    if (item1.DevId != null)
                    {
                       
                        int[] a = new int[]{8,10,12,13,14,15,16,20 };
                        for (int i = 0; i <a.Length ; i++)
                        {
                            if (item1.SensorList[a[i]].RValue == "未知")
                            {
                                ListViewItem item = new ListViewItem(item1.DevId);  // AGV设备ID
                                item.SubItems.Add(item1.SensorList[a[i]].SensName); // 类型名称
                                item.SubItems.Add(item1.SensorList[a[i]].ChannelId.ToString());  // 通道编号
                                item.SubItems.Add(item1.SensorList[a[i]].RValue);  // 实时值
                                item.SubItems.Add(item1.SensorList[a[i]].EValue); // 描述值                                 
                                alarmlist.Items.Add(item);
                            }
                        }
                        


                        // 先假定报警状态为 未知  字段，判断是否能获取到值
                        //if (sens.RValue == "未知")
                        //{
                        //    ListViewItem item = new ListViewItem(sens.SenId);  // 传感器ID
                        //    item.SubItems.Add(sens.SensName); // 类型名称
                        //    item.SubItems.Add(sens.ChannelId.ToString());  // 通道编号
                        //    item.SubItems.Add(sens.RValue);  // 实时值
                        //    item.SubItems.Add(sens.EValue); // 描述值                                 
                        //    alarmlist.Items.Add(item);
                        //}
                        //else
                        //{
                        //    SetOutputMsg("没有异常信息！");
                        //}
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
        /// 设备
        /// </summary>
        private void Devices()
        {

            // 创建列表头
            deviceslist.Columns.Add("Devsid", 150, HorizontalAlignment.Left);
            deviceslist.Columns.Add("DevsName", 200, HorizontalAlignment.Left);
            deviceslist.Columns.Add("DevsType", 200, HorizontalAlignment.Left); // 若宽度改为0，将会隐藏此列
            deviceslist.Columns.Add("DevsDesc", 200, HorizontalAlignment.Left);  // 设备描述
            deviceslist.Columns.Add("DevsState", 200, HorizontalAlignment.Left);  // 通讯状态
            deviceslist.Columns.Add("CommMode", 200, HorizontalAlignment.Left);  // 通讯模式


            // 若没有，无法显示数据 
            deviceslist.View = System.Windows.Forms.View.Details;

            ///添加数据项
            ///UI暂时挂起，直到EndUpdate绘制控件，可提高加载速度
            deviceslist.BeginUpdate();
            ListViewItem item = new ListViewItem("设备id");
            item.SubItems.Add("设备名称");
            item.SubItems.Add("设备类型");
            item.SubItems.Add("设备描述");
            item.SubItems.Add("设备状态");
            item.SubItems.Add("通讯模式");

            // 显示项
            deviceslist.Items.Add(item);

            // 结束数据处理
            // UI界面一次性绘制
            deviceslist.EndUpdate();

        }

        /// <summary>
        /// 车辆
        /// </summary>
        private void Vehicles()
        {

            GfxList<DeviceBackImf> devsList = JtWcfMainHelper.GetDevList();
            //vehicleslist.Clear();
            if (devsList.Count > 0)
            {
                // 创建列表头
                vehicleslist.Columns.Add("DevId", 200, HorizontalAlignment.Left);
                vehicleslist.Columns.Add("DevModel", 100, HorizontalAlignment.Left); // 设备型号
                vehicleslist.Columns.Add("DevStatue", 150, HorizontalAlignment.Left);// 若宽度改为0，将会隐藏此列
                vehicleslist.Columns.Add("RunStatueName", 150, HorizontalAlignment.Left); // 设备运行状态
                vehicleslist.Columns.Add("RunStatueValue", 150, HorizontalAlignment.Left); // 设备运行状态实时值
                vehicleslist.Columns.Add("Timestamp", 150, HorizontalAlignment.Left);

                // 若没有，无法显示数据 
                vehicleslist.View = System.Windows.Forms.View.Details;

                // 传感器
                SensorBackImf sens = null;

                ///添加数据项
                ///UI暂时挂起，直到EndUpdate绘制控件，可提高加载速度
                if (devsList is null) return;
                foreach (var item1 in devsList)
                {
                    // 获取AGV 运行方向 的相关数据
                    sens = item1.SensorList.Find(c => { return c.SenId == string.Format("{0}0005", item1.DevId); });
                    //vehicleslist.BeginUpdate();
                    ListViewItem item = new ListViewItem(item1.DevId); // 设备id
                    item.SubItems.Add(item1.DevModel); // 设备信息
                    item.SubItems.Add(item1.DevStatue); // 设备状态 
                    item.SubItems.Add(sens.SensName);
                    item.SubItems.Add(sens.RValue);
                    item.SubItems.Add("  ");
                    //vehicleslist.BeginUpdate();
                    //初始化车辆状态
                    agvStatus.Add(item1.DevId, "stop");

                    // 显示项
                    vehicleslist.Items.Add(item);
                }
                // 结束数据处理
                // UI界面一次性绘制
                //vehicleslist.EndUpdate();
            }
        }

        /// <summary>
        ///  获取所有任务信息
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

            ///添加数据项
            ///UI暂时挂起，直到EndUpdate绘制控件，可提高加载速度
            ///

            GfxList<GfxServiceContractTaskExcute.TaskBackImf> taskList = JtWcfTaskHelper.GetAllTask();
            if (taskList is null) return;
            for (int i = 0; i < taskList.Count; i++)
            {
                taskInformlist.BeginUpdate();
                ListViewItem item = new ListViewItem(taskList[i].DisGuid); // 任务id
                item.SubItems.Add(taskList[i].TaskImf); // 任务信息
                item.SubItems.Add(taskList[i].Statue.ToString()); // 任务状态
                item.SubItems.Add(taskList[i].OrderSource); // 触发源
                item.SubItems.Add(taskList[i].DisDevId); // 任务设备
                item.SubItems.Add(taskList[i].PathMsg); // 任务路径
                item.SubItems.Add(taskList[i].TaskCtrType.ToString()); // 控制参数，枚举类型
                item.SubItems.Add(taskList[i].CurDisOrderMsg); // 指令信息
                item.SubItems.Add(taskList[i].BackMsg); // 信息
                item.SubItems.Add(taskList[i].TriggerTime.ToString()); // 触发时间

                // 显示项
                taskInformlist.Items.Add(item);
            }

            // 结束数据处理
            // UI界面一次性绘制
            taskInformlist.EndUpdate();

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

            JtWcfDispatchHelper.InitPara(_severIp, "", "");

            JtWcfMainHelper.InitPara(_severIp, "", "");

            JtWcfTaskHelper.InitPara(_severIp, "", "");

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
        private void SetOutputMsg(string msg)
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
            toolStripLabelConnect.Text = string.Format("服务端连接状态：{0}", JtWcfMainHelper.IsConnected ? "已连接" : "未连接");

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
                    default:
                        MessageBox.Show("指令类型不存在，请重试！", "提示");
                        return;
                }
                JtWcfMainHelper.SendOrder(vehicleslist.FocusedItem.Text, new CommonDeviceOrderObj(this.comboBox1.Text, order, Convert.ToInt32(textBox1.Text)));
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
            JtWcfMainHelper.InitPara(_severIp, "", "");

            if (JtWcfMainHelper.SendOrder(vehicleslist.FocusedItem.Text, new CommonDeviceOrderObj("停止" + LocSite, 2, 1)))
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

                if (JtWcfMainHelper.SendOrder(vehicleslist.FocusedItem.Text, new CommonDeviceOrderObj("前进启动" + LocSite, 1, 1)))
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
                if (JtWcfMainHelper.SendOrder(vehicleslist.FocusedItem.Text, new CommonDeviceOrderObj("后退" + LocSite, 1, 2)))
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
        /// 设置指令参数输入只为数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))//如果不是输入数字就不让输入
            {
                MessageBox.Show("请输入数字！");
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
            else
            {
                //记录任务执行状态
                taskStatus[taskInformlist.FocusedItem.Text] = "taskstart";
                SetOutputMsg("开始任务成功");
                startmission.Enabled = false;
            }
        }


        /// <summary>
        /// 任务暂停
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pausemission_Click(object sender, EventArgs e)
        {
            if (GetSelectTaskid())
            {
                return;
            }
            else
            {
                //记录任务执行状态
                taskStatus[taskInformlist.FocusedItem.Text] = "taskpause";
                SetOutputMsg("暂停任务成功");
                if (pausemission.Text != "继续")
                {
                    if (MessageBox.Show("当前站点派发任务未完成，是否暂停？", "提示", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        pausemission.Text = "继续";
                    }
                }
                else
                {
                    pausemission.Text = "暂停";
                    pausemission.Enabled = false;
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
            // 判断是否有开始、暂停的任务正在执行
            if (startmission.Enabled == false || pausemission.Enabled == false)
            {
                //记录任务执行状态
                taskStatus[taskInformlist.FocusedItem.Text] = "taskend";
                SetOutputMsg("停止任务成功");
                StopTask();
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

        private void StopTask()
        {
            JtWcfMainHelper.InitPara(_severIp, "", "");

            if (taskInformlist.FocusedItem.Text != null)
            {
                startmission.Enabled = true;
                pausemission.Enabled = true;
            }
            else
            {
                MessageBox.Show("请尝试再操作一次", "提示");
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
                charge.Enabled = true;
                buttonSend.Enabled = true;
            }
            else if (vehicleslist.FocusedItem.SubItems[2].Text == "False")
            {
                agvForwordMove.Enabled = false;
                agvBackMove.Enabled = false;
                agvStop.Enabled = false;
                charge.Enabled = false;
                buttonSend.Enabled = false;
            }
        }
        private void RefreshListview()
        {
            GfxList<DeviceBackImf> devsList = JtWcfMainHelper.GetDevList();
            vehicleslist.BeginUpdate();

            for (int i = 0; i < devsList.Count; i++)             //只对第三列进行刷新
            {
                vehicleslist.Items[i].SubItems[2].Text = devsList[i].DevStatue;
            }
            vehicleslist.EndUpdate();
        }

        /// <summary>
        /// 定时刷新车辆信息
        /// </summary>
        private void Statetimer_Tick(object sender, EventArgs e)
        {
            RefreshListview();
        }

        private void KEDAForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("确定关闭客户端？", "提示", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }
    }
}