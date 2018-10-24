using Gfx.GfxDataManagerServer;
using Gfx.RCommData;
using GfxAgvMapEx;
using GfxCommonInterfaces;
using GfxServiceContractClient;
using GfxServiceContractClientDispatch;
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
        private Point _startLoc = new Point(10,20);

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

            timerFunc.Enabled = true;

            toolStripLabelVersion.Text = "版本号：V14.1";

            _dealThread = new Thread(ThreadFunc);

            _dealThread.IsBackground = true;

            _dealThread.Start();

            InitMap();

            this.WindowState = FormWindowState.Maximized;
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
                    if (!WcfDispatchHelper.IsConnected) { WcfDispatchHelper.Open(); }

                    if (!WcfMainHelper.IsConnected) { WcfMainHelper.Open(); }

                    if (!WcfTaskHelper.IsConnected) { WcfTaskHelper.Open(); }

                    CheckDispatch();

                    UpdateMap();
                }
                catch
                { }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateMap()
        {
            List<IAgvData> agvs = new List<IAgvData>();

            List<DeviceBackImf> devs = WcfMainHelper.GetDevList();

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
        /// 初始化成员
        /// </summary>
        private void InitStaMember()
        {
            string section = "StationConfigNum";

            string keyPre = string.Format("站点个数");

            string read = ConfigHelper.IniReadValue(section, keyPre,100);

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

                btn.Location = new Point(x,y);

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

                ConfigHelper.IniWriteValue(Section,key,read);
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

            _startLoc = new Point(x,y);
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

            WcfDispatchHelper.InitPara(_severIp, "", "");

            WcfMainHelper.InitPara(_severIp, "", "");

            WcfTaskHelper.InitPara(_severIp, "", "");

            _clientMark = string.Format("微科创新_客户端【{0}】", _locTar);

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
        /// 
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
            int dibiao = 0, tar = 0,index = 0,backTar = 0;

            string des = "";

            string section = "StationConfig";

            string keyPre = string.Format("STA_N0.{0}_", id);

            #region 地标
            string key = string.Format(string.Format("{0}{1}",keyPre,"地标"));

            string read = ConfigHelper.IniReadValue(section, key, 100);

            if (string.IsNullOrEmpty(read)) 
            {
                read = id.ToString();

                ConfigHelper.IniWriteValue(section,key,read);
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
                read = string.Format("站点{0}",id);

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
                read ="0,";

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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSendRun_Click(object sender, EventArgs e)
        {
            WcfMainHelper.InitPara(_severIp, "", "");

            List<DeviceBackImf> devs = WcfMainHelper.GetDevList();

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
                                if (WcfMainHelper.SendOrder(item.DevId, new CommonDeviceOrderObj("定点启动" + LocSite, 1)))
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStartTask_Click(object sender, EventArgs e)
        {
            SendTask();
        }

        /// <summary>
        /// 
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
                WcfDispatchHelper.InitPara(_severIp, "", "");

                string result = WcfDispatchHelper.IStartTaskDispatch(task);

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
                WcfDispatchHelper.InitPara(_severIp, "", "");

                string result = WcfDispatchHelper.IStartTaskDispatch(task);

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
        private bool IsDispatchContainPath(List<DispatchBackMember> dis,int tar)
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
        /// 
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
        /// 
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
        /// 检测调度状态
        /// </summary>
        private void CheckDispatch()
        {
            WcfMainHelper.InitPara(_severIp, "", "");

            List<DispatchBackMember> result = WcfMainHelper.GetDispatchList();

            if (result != null)
            {
                foreach (var item in result)
                {
                    if (item.TaskImf == _clientMark && item.OrderStatue == ResultTypeEnum.Suc)
                    {
                        WcfMainHelper.CtrDispatch(item.DisGuid, DisOrderCtrTypeEnum.Stop);
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
                WcfMainHelper.InitPara(_severIp, "", "");

                List<DispatchBackMember> diss = WcfMainHelper.GetDispatchList();

                List<DeviceBackImf> devs = WcfMainHelper.GetDevList();

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
                WcfMainHelper.InitPara(_severIp, "", "");

                DeviceBackImf result = WcfMainHelper.GetDev(_devArea2.DevId);

                if(result!=null)
                {
                    if (result.SensorList.Find(c => { return c.SenId == string.Format("{0}0003", _devArea2.DevId); }).RValue == _devTarWait.StaTarget.ToString())
                    {
                        _devTarWait = null;

                        _devArea2 = null;
                    }
                    else 
                    {
                        WcfMainHelper.SendOrder(_devArea2.DevId, new CommonDeviceOrderObj(DeviceOrderTypeEnum.OrderIndexThree, _devTarWait.StaTarget));
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateShow()
        {
            toolStripLabelConnect.Text = string.Format("服务端连接状态：{0}", WcfMainHelper.IsConnected ? "已连接" : "未连接");

            toolStripLabelTime.Text = string.Format("当前时间：{0}", DateTime.Now.TimeOfDay.ToString());

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
                    WcfDispatchHelper.InitPara(_severIp, "", "");

                    string result = WcfDispatchHelper.IStartTaskDispatch(task);

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

                        DispatchBackMember disback = WcfMainHelper.GetDispatch(task.DisGuid);

                        if (disback != null)
                        {
                            _callingAGV = disback.DisDevId;

                            DeviceBackImf devback = WcfMainHelper.GetDev(_callingAGV);

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
        /// 
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
                    WcfMainHelper.InitPara(_severIp, "", "");

                    List<DispatchBackMember> result = WcfMainHelper.GetDispatchList();

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
        /// 
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            WcfMainHelper.InitPara(_severIp, "", "");

            List<DispatchBackMember> result = WcfMainHelper.GetDispatchList();

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
        /// 
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
        private void panelBtn_Paint(object sender, PaintEventArgs e)
        {

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
            WcfMainHelper.InitPara(_severIp, "", "");

            List<DeviceBackImf> devs = WcfMainHelper.GetDevList();

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
                                if (WcfMainHelper.SendOrder(item.DevId, new CommonDeviceOrderObj("清除站点" + LocSite, 3,0)))
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {

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

        private void 系统初始化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_DataCenter.Init();
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
