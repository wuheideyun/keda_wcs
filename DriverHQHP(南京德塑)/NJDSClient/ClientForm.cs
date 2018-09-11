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
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NJDSClient
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ClientForm : Form
    {
        /// <summary>
        /// 站点个数
        /// </summary>
        private int _staCount = 10;

        /// <summary>
        /// 站点集合
        /// </summary>
        private Dictionary<int, StationMember> _staDic = new Dictionary<int, StationMember>();

        /// <summary>
        /// 区域二待命点个数
        /// </summary>
        private int _waitCountAreaTwo = 3;

        /// <summary>
        /// 区域二待命点集合
        /// </summary>
        private List<AreaTwoWaitPointMember> _waitAreaTwoList = new List<AreaTwoWaitPointMember>();

        /// <summary>
        /// 按钮宽度
        /// </summary>
        private int _btnWidth = 50;

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
        /// 当前的站点号
        /// </summary>
        public int _locTar = -1;

        /// <summary>
        /// 服务端IP
        /// </summary>
        private string _severIp = "";

        /// <summary>
        /// 客户端标志
        /// </summary>
        private string _clientMark = "";

        /// <summary>
        /// 
        /// </summary>
        private bool _isCalling = false;

        TaskDispatch _callingTask = null;

        /// <summary>
        /// 运行至区域2的特殊车辆
        /// </summary>
        private DeviceBackImf _devArea2 = null;

        /// <summary>
        /// 当前被呼叫的AGV
        /// </summary>
        string _callingAGV = null;

        /// <summary>
        /// 当前的呼叫点
        /// </summary>
        string _callingSite = null;

        /// <summary>
        /// 线程处理
        /// </summary>
        Thread _dealThread = null;

        /// <summary>
        /// 地图控件
        /// </summary>
        AgvMapControl _mapCtr = null;

        /// <summary>
        /// agv需要去的地方
        /// </summary>
        private AreaTwoWaitPointMember _devTarWait = null;

        private bool _isLogin = false;

        /// <summary>
        /// 已经选择的站点集合
        /// </summary>
        private List<StationMember> _selectStation = new List<StationMember>();

        /// <summary>
        /// 当前任务链表
        /// </summary>
        private List<TaskDispatch> _curTaskList = new List<TaskDispatch>();

        /// <summary>服务接口</summary>
        private static IUserOperation_TaskExcute _backupsClass;

        private string  markMsg;
        /// <summary>对象锁</summary>
        private  object _ans = new object();
        // <summary>WCF通道状态</summary>
        private  bool _channelState = false;
        private string _ipAdress = "";
        private string _portNum = null;
        private string _binding =null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ClientForm()
        {
            InitializeComponent();

            InitPara();

            InitStaMember();

            InitBtnMember();

            InitWaitPointAreaTwoMember();

            labelLogo.Text = APPConfig.LogoStr();

            

            toolStripLabelVersion.Text = "版本号：V14.1";

            _dealThread = new Thread(ThreadFunc);

            _dealThread.IsBackground = true;

            _dealThread.Start();

            InitMap();

            this.WindowState = FormWindowState.Maximized;

            TaskInform();

            DevsInform();

            //ShowCurTime(); // 获取当前时间方法
            timerFunc.Enabled = true;
        }

        /// <summary>
        /// 初始化梯度
        /// </summary>
        private void InitMap()
        {
            _mapCtr = new AgvMapControl();

            _mapCtr.Dock = DockStyle.Fill;

            panelMap.Controls.Add(_mapCtr);
        }

        /// <summary>
        /// 线程处理 放置程序卡顿
        /// </summary>
        private void ThreadFunc()
        {
            while (true)
            {
                Thread.Sleep(2000);

                try
                {
                    if (!JtWcfDispatchHelper.IsConnected) { JtWcfDispatchHelper.Open(); }

                    if (!JtWcfMainHelper.IsConnected) { JtWcfMainHelper.Open(); }

                    if (!JtWcfTaskHelper.IsConnected) { JtWcfTaskHelper.Open(); }

                    CheckDispatch();

                    UpdateMap();
                }
                catch
                { }
            }
        }

        /// <summary>
        /// 更新地图
        /// </summary>
        private void UpdateMap()
        {
            List<IAgvData> agvs = new List<IAgvData>();

            List<DeviceBackImf> devs = JtWcfMainHelper.GetDevList();

            List<IAgvData> updateList = new List<IAgvData>();

            if (devs != null)
            {
                devs.ForEach(c =>
                {
                    if (c.DevType == "AGV")
                    {
                        updateList.Add(new AgvData(c));
                    }
                });

                _mapCtr.UpdateAgvMessage(updateList);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientForm_Load(object sender, EventArgs e)
        {
            textBoxCurTar.Text = _locTar.ToString();
        }

        /// <summary>
        /// 初始化站点成员
        /// </summary>
        private void InitStaMember()
        {
            string section = "StationConfigNum";

            string keyPre = string.Format("站点个数");

            string read = ConfigHelper.IniReadValue(section, keyPre, 100);

            if (string.IsNullOrEmpty(read))
            {
                read = "10";

                ConfigHelper.IniWriteValue(section, keyPre, read);
            }

            if (!Int32.TryParse(read, out _staCount))
            {
                _staCount = 10;
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

                btn.Width = _btnWidth; btn.Height = _btnHeight;

                btn.Tag = item;

                btn.Text = item.Describ;

                btn.BackColor = Control.DefaultBackColor;

                panelBtn.Controls.Add(btn);
            }
        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        private void InitPara()
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

            _clientMark = string.Format("嘉腾机器人_客户端【{0}】", _locTar);

            this.Text = string.Format("{0} IP:{1}", _clientMark, IPHelper.GetLocalIntranetIP());
        }

        /// <summary>
        /// 初始化区域二待命点成员
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
        /// 更新位置
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

            _selectStation.Clear();
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
        /// 返回指定成员
        /// </summary>
        /// <param name="id"></param>
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

        /// <summary>
        /// 返回指定成员
        /// </summary>
        /// <param name="id"></param>
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
        /// 返回指定站点的位置
        /// </summary>
        /// <param name="tar"></param>
        /// <returns></returns>
        private StationMember GetRelateMember(int tar)
        {
            return _staDic.Values.ToList().Find(c => { return c.StaTarget == tar; });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelBtn_SizeChanged(object sender, EventArgs e)
        {
            UpdateBtnMember();

            //InitBtnMember();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 更新参数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitPara();

            UpdateBtnMember();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitPara();

            InitStaMember();

            InitBtnMember();

            InitWaitPointAreaTwoMember();

            _selectStation.Clear();
        }

        /// <summary>
        ///启动AGV
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSendRun_Click(object sender, EventArgs e)
        {
            JtWcfMainHelper.InitPara(_severIp, "", "");

            List<DeviceBackImf> devs = JtWcfMainHelper.GetDevList();

            SensorBackImf sens = null;

            if (devs != null && devs.Count > 0)
            {
                foreach (var item in devs)
                {
                    if (item.DevType == "AGV")
                    {
                        sens = item.SensorList.Find(c => { return c.SenId == string.Format("{0}0002", item.DevId); });

                        if (sens != null && sens.RValue == LocSite.ToString())
                        {
                            if (MessageBox.Show(string.Format("确定要启动【{0}】地标上AGV【{1}】", LocSite, item.DevId), "提示", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            {
                                if (JtWcfMainHelper.SendOrder(item.DevId, new CommonDeviceOrderObj("定点启动" + LocSite, 1,1)))
                                {
                                    SetOutputMsg(string.Format("启动【{0}】地标上AGV【{1}】", LocSite, item.DevId));

                                    MessageBox.Show("启动成功！", "提示");
                                }
                                else
                                {
                                    MessageBox.Show("请尝试再操作一次", "提示");
                                }
                            }

                            return;
                        }
                    }
                }
            }
            else
            {
                SetOutputMsg(string.Format("未获取到服务端的AGV"));
            }

            SetOutputMsg(string.Format("地标【{0}】上未找到AGV", LocSite));

            MessageBox.Show(string.Format("地标【{0}】上未找到AGV", LocSite), "提示");
        }

        /// <summary>
        /// 派发任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStartTask_Click(object sender, EventArgs e)
        {
            SendTask();
        }

        /// <summary>
        /// 派发任务？
        /// </summary>
        private void SendTask()
        {
            #region SendNow

            List<int> selectTar = GetSelectedTars();

            if (selectTar == null || selectTar.Count < 1)
            {
                MessageBox.Show("未选择有效任务", "提示");

                return;
            }

            ///构建任务
            TaskDispatch task = new TaskDispatch();

            task.Descip = _clientMark;

            TaskPathNode node = null;

            int lastTar = 0;

            ///添加任务节点
            foreach (var item in selectTar)
            {
                node = new TaskPathNode("", item, false);

                task.TaskPathNode.Add(node);

                lastTar = item;
            }

            ///启动本地AGV
            task.StartSiteList.Add(LocSite.ToString());

            ///执行任务
            if (MessageBox.Show(string.Format("确定要执行任务【{0}】", textBoxNextTars.Text), "提示", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                JtWcfDispatchHelper.InitPara(_severIp, "", "");

                string result = JtWcfDispatchHelper.IStartTaskDispatch(task);

                if (result != "s")
                {
                    SetOutputMsg(result);

                    MessageBox.Show("当前没有空闲AGV，" + "请稍后再试！", "提示");
                }
                else
                {
                    ResetBackCorlor();

                    SetOutputMsg(string.Format("开启新任务"));

                    MessageBox.Show("派发任务成功,点击启动开始运行！", "提示");
                }
            }
            #endregion
        }

        /// <summary>
        /// 返回待命点
        /// </summary>
        private void BackWaitPoint()
        {
            #region BackWaitPoint
            StationMember staMem = GetRelateMember(_locTar);

            if (staMem == null)
            {
                MessageBox.Show("没有配置相关的站点，请检查！", "提示");

                return;
            }

            ///构建任务
            TaskDispatch task = new TaskDispatch();

            task.Descip = _clientMark;

            TaskPathNode node = null;

            int lastTar = 0;

            ///添加任务节点
            foreach (var item in staMem.BackTarList)
            {
                node = new TaskPathNode("", item, false);

                task.TaskPathNode.Add(node);

                lastTar = item;
            }

            ///启动本地AGV
            task.StartSiteList.Add(LocSite.ToString());

            ///执行任务
            if (MessageBox.Show(string.Format("确定要返回待命点？"), "提示", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                JtWcfDispatchHelper.InitPara(_severIp, "", "");

                string result = JtWcfDispatchHelper.IStartTaskDispatch(task);

                if (result != "s")
                {
                    SetOutputMsg(result);

                    MessageBox.Show("当前没有空闲AGV，" + "请稍后再试！", "提示");
                }
                else
                {
                    ResetBackCorlor();

                    SetOutputMsg(string.Format("开启新任务"));

                    MessageBox.Show("派发任务成功,点击启动开始运行！", "提示");
                }
            }
            #endregion
        }

        /// <summary>
        /// 判断调度中是否有AGV即将到达指定点
        /// </summary>
        /// <returns></returns>
        private bool IsDispatchContainPath(List<DispatchBackMember> dis, int tar)
        {
            if (dis == null || dis.Count < 1) { return false; }

            foreach (var item in dis)
            {
                string[] tokens = item.PathMsg.Split(',');

                for (int i = 0; i < tokens.Length; i++)
                {
                    if (tokens[i] == tar.ToString())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 判断AGV是否处于指定地标上
        /// </summary>
        /// <param name="devs"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        private bool IsDevOnSite(List<DeviceBackImf> devs, int site)
        {
            if (devs == null || devs.Count < 1) { return false; }

            foreach (var item in devs)
            {
                if (item.DevType == "AGV")
                {
                    if (item.SensorList.Find(c => { return c.SenId == string.Format("{0}0002", item.DevId); }).RValue == site.ToString())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 获取选中站点表
        /// </summary>
        /// <returns></returns>
        private List<int> GetSelectedTars()
        {
            List<int> result = new List<int>();

            if (textBoxNextTars.Text != null)
            {
                string[] tokens = textBoxNextTars.Text.Split(',');

                int num = 0;

                for (int i = 0; i < tokens.Length; i++)
                {
                    if (Int32.TryParse(tokens[i], out num))
                    {
                        result.Add(num);
                    }
                }
            }

            return result;
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
        ///获取当前运行时间值,以及任务触发时间 
        /// </summary>
        private void ShowCurTime()
        {
            if (_isLogin &&( _severIp!=null))
            {
                ListBox.Items.Add("运行时间：" + DateTime.Now.ToString());
                ListBox.Items.Add(toolStripLabelTime.Text); // 添加当前系统时间值
                ListBox.Items.Add("服务端连接状态：" + JtWcfMainHelper.IsConnected); // 判断服务端连接状态

                // 获取所有任务信息GetAllTask()的方法调用

                GfxList<GfxServiceContractTaskExcute.TaskBackImf> taskList = JtWcfTaskHelper.GetAllTask();

                for (int i = 0; i < taskList.Count; i++)
                {
                    // 获取一个任务的执行情况，调用方法 public static TaskBackImf GetExcTask(string disGuid){ } 
                    // TaskBackImf task = JtWcfTaskHelper.GetExcTask(taskList[i].TriggerTime.ToString());

                    ListBox.Items.Add("任务的Guid为：" + taskList[i].DisGuid);
                    ListBox.Items.Add(" 任务触发时间为 ：" + taskList[i].TriggerTime.ToString());
                    ListBox.Items.Add("任务信息为：" + taskList[i].BackMsg);
                    ListBox.Items.Add("控制参数为：" + taskList[i].TaskCtrType);
                }
            }
            else
            {
                MessageBox.Show("请先登录账号");
            }

        }

     
        /// <summary>
        /// 检测调度状态
        /// </summary>
        private void CheckDispatch()
        {
            JtWcfMainHelper.InitPara(_severIp, "", "");

            List<DispatchBackMember> result = JtWcfMainHelper.GetDispatchList();

            if (result != null)
            {
                foreach (var item in result)
                {
                    if (item.TaskImf == _clientMark && item.OrderStatue == ResultTypeEnum.Suc)
                    {
                        JtWcfMainHelper.CtrDispatch(item.DisGuid, DisOrderCtrTypeEnum.Stop);
                    }
                }

                if (_callingTask != null)
                {
                    if (result.Find(c => { return c.DisGuid == _callingTask.DisGuid; }) == null)
                    {
                        _callingTask = null;
                    }
                }
            }

            if (_callingTask == null) { _isCalling = false; }
        }

        /// <summary>
        /// 返回区域二附近的待命点
        /// </summary>
        private void BackToPointAround()
        {
            if (_devArea2 == null) { return; }

            if (_devTarWait == null)
            {
                JtWcfMainHelper.InitPara(_severIp, "", "");

                List<DispatchBackMember> diss = JtWcfMainHelper.GetDispatchList();

                List<DeviceBackImf> devs = JtWcfMainHelper.GetDevList();

                foreach (var item in _waitAreaTwoList)
                {
                    if ((!IsDispatchContainPath(diss, item.StaTarget)) && (!IsDevOnSite(devs, item.StaSite))) 
                    {
                        _devTarWait = item;

                        break;
                    }
                }
            }

            if (_devTarWait != null)
            {
                JtWcfMainHelper.InitPara(_severIp, "", "");

                DeviceBackImf result = JtWcfMainHelper.GetDev(_devArea2.DevId);

                if(result!=null)
                {
                    if (result.SensorList.Find(c => { return c.SenId == string.Format("{0}0003", _devArea2.DevId); }).RValue == _devTarWait.StaTarget.ToString())
                    {
                        _devTarWait = null;

                        _devArea2 = null;
                    }
                    else 
                    {
                        JtWcfMainHelper.SendOrder(_devArea2.DevId, new CommonDeviceOrderObj(DeviceOrderTypeEnum.OrderIndexThree, _devTarWait.StaTarget));
                    }
                }
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        private void UpdateShow()
        {
            toolStripLabelConnect.Text = string.Format("服务端连接状态：{0}", JtWcfMainHelper.IsConnected ? "已连接" : "未连接");

            toolStripLabelTime.Text = string.Format("当前系统时间：{0}", DateTime.Now.TimeOfDay.ToString());

            toolStripLabelLogin.Text = string.Format("用户状态：{0}", _isLogin ? "已登录" : "未登录");
        }

        /// <summary>
        /// 设置输出信息
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
        /// 获取已选择的站点
        /// </summary>
        /// <returns></returns>
        private string SelectStaStr()
        {
            if (_selectStation == null || _selectStation.Count<1) { return string.Empty; }

            _selectStation = _selectStation.OrderBy(c=>c.OrderIndex).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var item in _selectStation)
            {
                sb.Append(string.Format("{0},", item.StaTarget));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 呼叫AGV
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCallAGv_Click(object sender, EventArgs e)
        {
            try
            {
                if (_isCalling) { MessageBox.Show("AGV过来的途中,请等待!", "提示"); return; }

                TaskDispatch task = new TaskDispatch();

                task.Descip = _clientMark;

                StationMember sta = _staDic.Values.ToList().Find(c => { return c.StaSite == LocSite; });

                if (sta == null) { MessageBox.Show("未找到与本客户端绑定的站点", "提示"); return; }

                TaskPathNode node = new TaskPathNode(sta.StaSite.ToString(), sta.StaTarget, true);

                task.TaskPathNode.Add(node);

                if (sta != null && sta.WaitList != null)
                {
                    foreach (var item in sta.WaitList)
                    {
                        task.StartSiteList.Add(item.ToString());
                    }
                }

                if (task.StartSiteList == null || task.StartSiteList.Count < 1)
                {
                    MessageBox.Show("未正确配置其相应的待命点,请检查配置!", "提示");

                    return;
                }

                if (MessageBox.Show(string.Format("确定要呼叫AGV到【{0}】位置", sta.StaTarget), "提示", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    JtWcfDispatchHelper.InitPara(_severIp, "", "");

                    string result = JtWcfDispatchHelper.IStartTaskDispatch(task);

                    SetOutputMsg(string.Format("开启新任务"));

                    if (result != "s")
                    {
                        SetOutputMsg(result);

                        MessageBox.Show("当前没有空闲的AGV，请稍后再试！", "提示");
                    }
                    else
                    {
                        _isCalling = true;

                        _callingTask = task;

                        DispatchBackMember disback = JtWcfMainHelper.GetDispatch(task.DisGuid);

                        if (disback != null)
                        {
                            _callingAGV = disback.DisDevId;

                            DeviceBackImf devback = JtWcfMainHelper.GetDev(_callingAGV);

                            if (devback != null)
                            {
                                _callingSite = devback.SensorList.Find(c => { return c.SenId == string.Format("{0}0002", devback.DevId); }).RValue;
                            }
                            else
                            {
                                _callingSite = null;
                            }
                        }
                        else { _callingAGV = null; }

                        MessageBox.Show("呼叫成功,请等待AGV过来!", "提示");
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 修改（当前站点号的修改）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAlter_Click(object sender, EventArgs e)
        {
            if (_isLogin)
            {
                #region
                if (buttonAlter.Text != "保  存")
                {
                    JtWcfMainHelper.InitPara(_severIp, "", "");

                    List<DispatchBackMember> result = JtWcfMainHelper.GetDispatchList();

                    if (_devArea2 != null)
                    {
                        MessageBox.Show("当前客户端目前有派发任务未完成,不允许修改！", "提示");

                        return;
                    }

                    if (result != null)
                    {
                        foreach (var item in result)
                        {
                            if (item.TaskImf == _clientMark)
                            {
                                MessageBox.Show("当前客户端目前有派发任务未完成,不允许修改！", "提示");

                                return;
                            }
                        }
                    }

                    buttonAlter.Text = "保  存";

                    textBoxCurTar.ReadOnly = false;
                }
                else
                {
                
                        if (Int32.TryParse(textBoxCurTar.Text, out _locTar))
                        {
                            buttonAlter.Text = "修 改";

                            textBoxCurTar.ReadOnly = true;

                            string Section = "CientConfig";

                            string key = "LoctionSite";

                            ConfigHelper.IniWriteValue(Section, key, _locTar.ToString());

                            InitPara();

                            MessageBox.Show("修改成功！", "提示");
                        }
                        else
                        {
                            MessageBox.Show("输入站点格式有误！", "提示");
                        }
                }
                #endregion
            }
            else
            {
                MessageBox.Show("请先登录后再操作！", "提示");
            }
        }

        /// <summary>
        /// 自动模式按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonManSend_Click(object sender, EventArgs e)
        {
            if (!_isLogin) { MessageBox.Show("请先登录后再操作！", "提示"); return; }

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
        /// 关闭客户表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            JtWcfMainHelper.InitPara(_severIp, "", "");

            List<DispatchBackMember> result = JtWcfMainHelper.GetDispatchList();

            if (result != null)
            {
                _isCalling = false;

                foreach (var item in result)
                {
                    if (item.TaskImf == _clientMark)
                    {
                        MessageBox.Show("当前还有任务未完成,不允许关闭客户端！","提示");

                        e.Cancel = true;

                        return;
                    }
                }
            }

            if(_devArea2!=null || _isCalling)
            {
                MessageBox.Show("当前还有任务未完成,不允许关闭客户端！","提示");

                e.Cancel = true;

                return;
            }


            if (MessageBox.Show("确定关闭客户端？", "提示", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 改变客户表尺寸
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientForm_SizeChanged(object sender, EventArgs e)
        {
            labelLogo.Text = APPConfig.LogoStr();

            labelLogo.Location = new Point((this.Width - labelLogo.Width)/2, labelLogo.Location.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBackPoint_Click(object sender, EventArgs e)
        {
            BackWaitPoint();
        }

        /// <summary>
        /// 确认完成任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonTaskDone_Click(object sender, EventArgs e)
        {
            JtWcfMainHelper.InitPara(_severIp, "", "");

            List<DeviceBackImf> devs = JtWcfMainHelper.GetDevList();

            SensorBackImf sens = null;

            if (devs != null && devs.Count > 0)
            {
                foreach (var item in devs)
                {
                    if (item.DevType == "AGV")
                    {
                        sens = item.SensorList.Find(c => { return c.SenId == string.Format("{0}0002", item.DevId); });

                        if (sens != null && sens.RValue == LocSite.ToString())
                        {
                            if (MessageBox.Show(string.Format("确定【{0}】地标上AGV【{1}】任务已完成", LocSite, item.DevId), "提示", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            {
                                if (JtWcfMainHelper.SendOrder(item.DevId, new CommonDeviceOrderObj("清除站点" + LocSite, 3,0)))
                                {
                                    SetOutputMsg(string.Format("清除【{0}】地标上AGV【{1}】的站点,任务已完成", LocSite, item.DevId));

                                    MessageBox.Show("确认成功！", "提示");
                                }
                                else
                                {
                                    MessageBox.Show("请尝试再操作一次", "提示");
                                }
                            }

                            return;
                        }
                    }
                }
            }
            else
            {
                SetOutputMsg(string.Format("未获取到服务端的AGV"));
            }

            SetOutputMsg(string.Format("地标【{0}】上未找到AGV", LocSite));

            MessageBox.Show(string.Format("地标【{0}】上未找到AGV", LocSite), "提示");
        }


        /// <summary>
        /// 注销登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 注销登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _isLogin = false;

            MessageBox.Show("当前用户已注销成功！");
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 用户登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogForm form = new LogForm();

            form.ShowDialog();

            _isLogin = form._isLogin;
        }


        /// <summary>
        ///  查询任务Guid和触发源
        ///  
        /// </summary>
           
        private void button1_Click(object sender, EventArgs e)
        {
            JtWcfTaskHelper.InitPara(_severIp, "", "");

            GfxList<GfxServiceContractTaskExcute.TaskBackImf> taskList = JtWcfTaskHelper.GetAllTask();

            for (int i = 0; i < taskList.Count; i++)
            {
                MessageBox.Show(taskList[i].DisGuid);  // 获取任务Guid
                MessageBox.Show(taskList[i].TaskImf);  // 获取任务信息
                MessageBox.Show(taskList[i].OrderSource);  // 获取触发源
            }

        }
      

        // 获取所配置任务信息
        private void taskstate_Click(object sender, EventArgs e)
        {
            JtWcfMainHelper.InitPara(_severIp, "", "");
            GfxList<TaskRelationMember> memberlist = JtWcfTaskHelper.GetDefineTask();
           

            for (int i = 0; i < memberlist.Count; i++)
            {
                richTextBox1.AppendText("任务id：" + memberlist[i].MemberId + "\r\n");  // 获取任务id
              
                richTextBox1.AppendText("任务描述：" + memberlist[i].TaskRelatDecirbe + "\r\n");  // 获取任务描述
               
                richTextBox1.AppendText("任务名称：" + memberlist[i].TaskRelatName + "\r\n");  // 获取任务名称
              
                richTextBox1.AppendText("任务优先级："+memberlist[i].Priority.ToString() + "\r\n");  // 任务优先级

              //  richTextBox1.AppendText("任务节点列表：" + memberlist[i].TaskNodeListStr.ToString() + "\r\n");   // 触发模式列表

                richTextBox1.AppendText(System.Environment.NewLine); //  通用的系统默认换行符

            }
            // 获取操作类型：增、删、改
            OperTypeEnum oper = new OperTypeEnum();
            richTextBox1.AppendText(oper.ToString() + "\r\n");
            // 构建任务
            TaskDispatch task = new TaskDispatch();
            richTextBox1.AppendText("任务的guid: " + "\r\n" + task.DisGuid +"\r\n");
            richTextBox1.AppendText("任务描述: "+ task.Descip + "\r\n");
         // richTextBox1.AppendText("设备链表: " + task.DevList + "\r\n");
            //  获取任务调度的结果
            string result = JtWcfDispatchHelper.IStartTaskDispatch(task);
            if(result=="s")
            {
                richTextBox1.AppendText("启动任务成功"+ "\r\n");
            }
            else
                richTextBox1.AppendText("当前没有空闲AGV，" + "请稍后再试！" + "\r\n");

                StartTask(task.DisGuid, "任务启动成功");


        }
        // 客户端通过调用此方法触发服务端执行指定id的任务
        private  bool StartTask(string taskId, string v)
        {
            richTextBox1.AppendText(v+" 且任务id为： "+ taskId);
            bool _channelState = false;
            if (!_channelState && !Open()) { return false; }

            lock (_ans) { try { return _backupsClass.StartTask(taskId, markMsg); } catch { return false; } }

         ;
        }

        /// <summary>显式打开当前的WCF通道，如果通道不存在或异常，则创建新的通道</summary>
        private bool Open()
        {
            lock (_ans)
            {
                IClientChannel channel = _backupsClass as IClientChannel;

                if (null != _backupsClass && null != channel)
                {
                    if (channel.State == CommunicationState.Opened || channel.State == CommunicationState.Opening)
                    {
                        try { if (!_channelState) { _channelState = true; } return true; }
                        catch { }
                    }

                    if (channel.State == CommunicationState.Created || channel.State == CommunicationState.Closed || channel.State == CommunicationState.Closing)
                    {
                        try { channel.Open(); if (!_channelState) { _channelState = true; } return true; }
                        catch { }
                    }

                    Close();
                }

                return CreatNewChannel();
            }
        }

        /// <summary>新建一个WCF的通道,返回通道是否有效</summary>
        private bool CreatNewChannel()
        {
            lock (_ans)
            {
                try
                {
                    WcfClientHelper.SetWCFParas(_ipAdress, _portNum, _binding);

                    _backupsClass = WcfClientHelper.CreateService<IUserOperation_TaskExcute>();

                    IClientChannel channel = _backupsClass as IClientChannel;

                    if (channel == null) { return false; }

                    channel.Faulted += ChannelFaulted;

                    if (channel.State != CommunicationState.Opened) { channel.Open(); }

                    if (!_channelState) { _channelState = true; }

                    return true;
                }
                catch { if (_channelState) { _channelState = false; } return false; }
            }
        }

        // <summary>通道异常时，关闭当前通道</summary>
        private void ChannelFaulted(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        ///  获取所有任务信息
        /// </summary>
        private void TaskInform()
        {
            // 添加表头，即列名
            listView1.Columns.Add("任务id", 200,HorizontalAlignment.Left);
            listView1.Columns.Add("任务信息", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("任务状态", 150, HorizontalAlignment.Left);
            listView1.Columns.Add("触发源",150 , HorizontalAlignment.Left);
            listView1.Columns.Add("任务设备", 150, HorizontalAlignment.Left);
            listView1.Columns.Add("任务路径", 150, HorizontalAlignment.Left);
            listView1.Columns.Add("控制参数",150, HorizontalAlignment.Left);
            listView1.Columns.Add("指令信息", 150, HorizontalAlignment.Left);
            listView1.Columns.Add("信息", 200, HorizontalAlignment.Left);
            listView1.Columns.Add("触发时间", 100, HorizontalAlignment.Left);

            // 若没有，无法显示数据 
            listView1.View = System.Windows.Forms.View.Details;

            ///添加数据项
            ///UI暂时挂起，直到EndUpdate绘制控件，可提高加载速度

            GfxList<GfxServiceContractTaskExcute.TaskBackImf> taskList = JtWcfTaskHelper.GetAllTask();
            for (int i = 0; i < taskList.Count; i++)
            {
                listView1.BeginUpdate();
                ListViewItem item = new ListViewItem(taskList[i].DisGuid); // 任务id
                item.SubItems.Add(taskList[i].TaskImf); // 任务信息
                item.SubItems.Add(taskList[i].Statue.ToString()); // 任务状态
                item.SubItems.Add(taskList[i].OrderSource); // 触发源
                item.SubItems.Add(taskList[i].DisDevId); // 任务设备
                item.SubItems.Add(taskList[i].PathMsg); // 任务路径
                item.SubItems.Add(taskList[i].TaskCtrType.ToString()); // 控制参数，枚举类型
                item.SubItems.Add(taskList[i].CurDisOrderMsg); // 指令信息
                item.SubItems.Add(taskList[i].BackMsg); // 信息
                item.SubItems.Add(taskList[i].TriggerTime.ToLongTimeString()); // 触发时间

                // 显示项
                listView1.Items.Add(item);
            }

            // 结束数据处理
            // UI界面一次性绘制
            listView1.EndUpdate();

        }

        /// <summary>
        /// 获取所有设备信息
        /// </summary>
        private void DevsInform()
        {
            // 添加表头，即列名
            listView2.Columns.Add("设备id", 200, HorizontalAlignment.Left);
            listView2.Columns.Add("设备类型", 100, HorizontalAlignment.Left);
            listView2.Columns.Add("设备状态", 150, HorizontalAlignment.Left);
            listView2.Columns.Add("关键值", 150, HorizontalAlignment.Left);
            listView2.Columns.Add("报警描述", 150, HorizontalAlignment.Left);
            listView2.Columns.Add("报警时间", 150, HorizontalAlignment.Left);
            listView2.Columns.Add("传感器列表", 150, HorizontalAlignment.Left);


            // 若没有，无法显示数据 
            listView2.View = System.Windows.Forms.View.Details;

            ///添加数据项
            ///UI暂时挂起，直到EndUpdate绘制控件，可提高加载速度

            GfxList<DeviceBackImf> devsList = JtWcfMainHelper.GetDevList();

           
            for (int i = 0; i < devsList.Count; i++)
            {
                listView2.BeginUpdate();
                ListViewItem item = new ListViewItem(devsList[i].DevId); // 设备id
                item.SubItems.Add(devsList[i].DevModel); // 设备信息
                item.SubItems.Add(devsList[i].DevStatue); // 设备状态
                item.SubItems.Add(devsList[i].KeyValue); //关键值
                item.SubItems.Add("报警描述字段不清楚"); // 报警描述
                item.SubItems.Add("报警时间字段不清楚" ); // 报警时间
                item.SubItems.Add(devsList[i].SensorList.ToString());


                // 显示项
                listView2.Items.Add(item);
            }

            // 结束数据处理
            // UI界面一次性绘制
            listView2.EndUpdate();
        }

      
         /// <summary>
         /// 心跳发送中的刷新按钮
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            // 刷新心跳发送tabpage 中的数据
            ShowCurTime();  
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
}
