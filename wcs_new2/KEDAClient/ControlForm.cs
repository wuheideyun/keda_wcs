using DataContract;
using FLBasicHelper;
using FLCommonInterfaces;
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

namespace KEDAClient
{
    public partial class ControlForm : Form
    {
        #region 参数定义和初始化
        /// <summary>
        /// 原来的控制界面
        /// </summary>
        KEDAForm monitorForm;

        /// <summary>
        /// 保存Listview选中的index
        /// </summary>
        private int _agvListIndex = -1, _currentTaskIndex = -1;
        /// <summary>
        /// 保存listview选中的文字
        /// </summary>
        private String _agvListText, _currentTaskText;

        /// <summary>
        /// 选中的PLC名称
        /// </summary>
        private String _plcSelectName = "";

        /// <summary>
        /// 选中的AGV名称
        /// </summary>
        private String _agvSelectName = "";

        /// <summary>
        /// AGV信息
        /// </summary>
        private AGV _agv;

        /// <summary>
        /// PLC信息
        /// </summary>
        private PLC _plc;

        /// <summary>
        /// 设备ID与自身状态对应关系(车辆）：状态值：stop、forwardmove、backmove
        /// </summary>
        Dictionary<string, string> agvStatus = new Dictionary<string, string>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public ControlForm()
        {
            InitializeComponent();
            InitPara();
            F_DataCenter.Init(SynchronizationContext.Current, listBoxOutput);

            LogHelper.LogFactory.Init();
        }

        private void ControlForm_Load(object sender, EventArgs e)
        {
            ListView_Init();
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
            _severIp = "127.0.0.1";

            WcfMainHelper.InitPara(_severIp, "", "");
        }

        /// <summary>
        /// ListView初始化 标题
        /// </summary>
        private void ListView_Init()
        {
            //Agv列表
            agvList.Columns.Add("AGV", 80, HorizontalAlignment.Center);
            agvList.Columns.Add("状态", 109, HorizontalAlignment.Center);

            agvList.View = System.Windows.Forms.View.Details;

            //调度任务列表
            currentTaskList.Columns.Add("ID", 0, HorizontalAlignment.Center);
            currentTaskList.Columns.Add("任务", 340, HorizontalAlignment.Center);
            currentTaskList.Columns.Add("路径", 80, HorizontalAlignment.Center);

            currentTaskList.View = System.Windows.Forms.View.Details;


            List<DeviceBackImf> devDatas = WcfMainHelper.GetDevList();
            foreach (var data in devDatas)
            {
                agvStatus.Add(data.DevId, "stop");
            }
        }

        #endregion

        #region ListView刷新等
        /// <summary>
        /// ListBox的刷新线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerForListRefresh_Tick(object sender, EventArgs e)
        {
            timerForListRefresh.Enabled = false;
            AgvList_Refresh();
            CurrentTaskList_Refresh();
            agvData_Refresh();
            plcData_Refresh();
            timerForListRefresh.Enabled = true;
        }

        /// <summary>
        /// 任务列表刷新
        /// </summary>
        private void CurrentTaskList_Refresh()
        {
            List<TaskData> taskDatas = PublicDataContorl.GetTaskData();

            if (taskDatas == null)
            {
                currentTaskList.Clear();
                return;
            }

            currentTaskList.BeginUpdate();
            currentTaskList.Items.Clear();
            foreach (var data in taskDatas)
            {
                ListViewItem item = new ListViewItem(data.NO + ""); // 任务信息
                //item.Text = data.NO + "" ;
                item.SubItems.Add(data.Msg); //站点信息
                item.SubItems.Add(data.SiteMsg); //站点信息
                currentTaskList.Items.Add(item);
            }
            // 结束数据处理
            currentTaskList.EndUpdate();

            if (_currentTaskIndex != -1)
            {
                if (currentTaskList.Items.Count > _currentTaskIndex && _currentTaskText.Equals(currentTaskList.Items[_currentTaskIndex].Text))
                {
                    currentTaskList.FocusedItem = currentTaskList.Items[_currentTaskIndex];
                    currentTaskList.Items[_currentTaskIndex].BackColor = Color.LightGray;
                    return;
                }

                foreach (ListViewItem item in currentTaskList.Items)
                {
                    if (item.Text.Equals(_currentTaskText))
                    {
                        currentTaskList.FocusedItem = item;
                        currentTaskList.FocusedItem.BackColor = Color.LightGray;
                        return;
                    }
                }
                _currentTaskIndex = -1;
                _currentTaskText = "";
            }
        }

        /// <summary>
        /// 刷新AGV列表
        /// </summary>
        public void AgvList_Refresh()
        {
            List<DevData> devDatas = F_DataCenter.MDev.GetDevData();

            if (devDatas == null)
            {
                agvList.Clear();
                return;
            }

            agvList.BeginUpdate();
            agvList.Items.Clear();
            foreach (var data in devDatas)
            {
                ListViewItem item = new ListViewItem(data.DevID); // AGV名称
                item.SubItems.Add(data.Status); //AGV状态
                agvList.Items.Add(item);
            }
            // 结束数据处理
            agvList.EndUpdate();

            if (_agvListIndex != -1)
            {
                if (agvList.Items.Count > _agvListIndex && _agvListText.Equals(agvList.Items[_agvListIndex].Text))
                {
                    agvList.FocusedItem = agvList.Items[_agvListIndex];
                    agvList.Items[_agvListIndex].BackColor = Color.LightGray;
                    return;
                }

                foreach (ListViewItem item in agvList.Items)
                {
                    if (item.Text.Equals(_agvListText))
                    {
                        agvList.FocusedItem = item;
                        agvList.FocusedItem.BackColor = Color.LightGray;
                        return;
                    }
                }
                if (agvList.FocusedItem == null && agvList.Items.Count > 0)
                {
                    agvList.FocusedItem = agvList.Items[0];
                    agvList.FocusedItem.BackColor = Color.LightGray;
                }
                else
                {
                    _agvListIndex = -1;
                    _agvSelectName = "";
                    _plcSelectName = "";
                }
            }
        }
        /// <summary>
        /// Agv列表选择改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void agvList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (agvList.FocusedItem == null)
            {
                //_agvListIndex = -1;
                //_agvListText = "";
            }
            else
            {
                _agvListIndex = agvList.FocusedItem.Index;
                _agvListText = agvList.FocusedItem.Text;
                agvList.FocusedItem.BackColor = Color.LightGray;
                if (agvList.FocusedItem.Text.StartsWith("AGV"))
                {
                    mainTabControl.SelectedIndex = 0;
                    _agvSelectName = agvList.FocusedItem.Text;
                    agvData_Refresh();

                    string status = agvStatus[agvList.FocusedItem.Text];

                    if (AgvStatusLab.Text == "在线")
                    {
                        if (status == "forwardmove")
                        {
                            AgvForwardBtn.Enabled = false;
                            AgvBackwardBtn.Enabled = true;
                            AgvStopBtn.Enabled = true;
                        }
                        else if (status == "backmove")
                        {
                            AgvForwardBtn.Enabled = true;
                            AgvBackwardBtn.Enabled = false;
                            AgvStopBtn.Enabled = true;
                        }
                        else if (status == "stop")
                        {
                            AgvForwardBtn.Enabled = true;
                            AgvBackwardBtn.Enabled = true;
                            AgvStopBtn.Enabled = true;
                        }
                        AgvClearSiteBtn.Enabled = true;
                        AgvInitBtn.Enabled = true;
                    }
                    else if (AgvStatusLab.Text == "离线")
                    {
                        AgvForwardBtn.Enabled = false;
                        AgvBackwardBtn.Enabled = false;
                        AgvStopBtn.Enabled = false;
                        AgvClearSiteBtn.Enabled = false;
                        AgvInitBtn.Enabled = false;
                    }
                }
                else
                {
                    mainTabControl.SelectedIndex = 1;
                    _plcSelectName = agvList.FocusedItem.Text;
                    plcData_Refresh();
                }

            }
        }


        /// <summary>
        /// 刷新Agv信息
        /// </summary>
        private void agvData_Refresh()
        {
            if (!_agvSelectName.Equals(""))
            {
                agvNameLab.Text = _agvSelectName;
                _agv = new AGV(F_DataCenter.MDev.IGetDev(_agvSelectName));
                AgvSiteLab.Text = _agv.Site();
                AgvNowPoitLab.Text = _agv.NowPoint();
                AgvStatusLab.Text = _agv.AgvStatus();
                AgvAimLab.Text = _agv.Point();
                AgvDirectionLab.Text = _agv.Direction();
                AgvElectricityLab.Text = _agv.Electicity();
                AgvSpeedLab.Text = _agv.Speed();
                AgvFreeLab.Text = _agv.FreeStatus();
                AgvStaMaterialLab.Text = _agv.Sta_Material();
                AgvStaMonitorLab.Text = _agv.Sta_Monitor();
                AgvTrafficLab.Text = _agv.Traffic();
            }
        }

        /// <summary>
        /// 刷新Plc信息
        /// </summary>
        private void plcData_Refresh()
        {
            if (!_plcSelectName.Equals(""))
            {
                plcNameLab.Text = _plcSelectName;
                _plc = new PLC(F_DataCenter.MDev.IGetDev(_plcSelectName));
                PlcLocationLab.Text = _plc.PLCLocat();
                PlcStaMaterialLab.Text = _plc.Sta_Material();
                PlcStaMonitorLab .Text =_plc.Sta_Monitor();
                PlcErrorLab .Text = _plc.Sta_Error();
                PlcSpareLab .Text = _plc.SpareInform ();
                
            }
        }


        /// <summary>
        /// 当前任务列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void currentTaskList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currentTaskList.FocusedItem == null)
            {
                _currentTaskIndex = -1;
                _currentTaskText = "";
            }
            else
            {
                _currentTaskIndex = currentTaskList.FocusedItem.Index;
                _currentTaskText = currentTaskList.FocusedItem.Text;
            }
        }
        #endregion

        #region 其他方法
        /// <summary>
        /// 打开原控制界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void monitorBtn_Click(object sender, EventArgs e)
        {
            monitorForm = new KEDAForm();
            monitorForm.Show();
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

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
        #endregion

        #region  全局方法

        /// <summary>
        /// 初始化任务，查找所有AGV需要初始化的指派任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InitAllBtn_Click(object sender, EventArgs e)
        {

            if (InitAllBtn.Checked)
            {
                F_DataCenter.MLogic.InitAgv();
                InitAllBtn.Checked = false;
                MessageBox.Show("初始化了全部AGV");
            }
        }


        #endregion

        #region AGV操作方法
        /// <summary>
        /// 初始化指定的AGV
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgvInitBtn_Click(object sender, EventArgs e)
        {
            if (agvList.FocusedItem == null)
            {
                MessageBox.Show("请选选择AGV");
                return;
            }
            F_DataCenter.MLogic.InitAgv(agvList.FocusedItem.Text);
            MessageBox.Show(agvList.FocusedItem.Text + "已经初始化！");
        }

        /// <summary>
        /// 停止车辆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgvStopBtn_Click(object sender, EventArgs e)
        {
            if (AgvBackwardBtn.Enabled == false || AgvForwardBtn.Enabled == false)
            {
                StopAGV();
                //记录agv状态
                agvStatus[agvList.FocusedItem.Text] = "stop";
            }
            else
            {
                MessageBox.Show("当前没有运行的车辆！");
            }
        }

        /// <summary>
        /// 停止函数
        /// </summary>
        public void StopAGV()
        {
            //WcfMainHelper.InitPara(_severIp, "", "");
            // 1是快速停止、0是慢速
            if (WcfMainHelper.SendOrder(agvList.FocusedItem.Text, new FControlOrder("停止", 2, 0)))
            {
                AgvForwardBtn.Enabled = true;
                AgvBackwardBtn.Enabled = true;
            }
            else
            {
                MessageBox.Show("请尝试再操作一次", "提示");
            }
        }

        /// <summary>
        /// AGV前进
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgvForwardBtn_Click(object sender, EventArgs e)
        {
            if (agvList.FocusedItem == null)
            {
                MessageBox.Show("请选中需要操作的车辆", "提示");
                return;
            }
            else
            {
                if (!AgvBackwardBtn.Enabled)
                {
                    StopAGV();
                }

                if (WcfMainHelper.SendOrder(agvList.FocusedItem.Text, new FControlOrder("前进启动", 1, 1)))
                {
                    //记录agv状态
                    agvStatus[agvList.FocusedItem.Text] = "forwardmove";
                    AgvForwardBtn.Enabled = false;
                }
                else
                {
                    MessageBox.Show("请尝试再操作一次", "提示");
                }
            }
        }

        /// <summary>
        /// AGV后退
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgvBackwardBtn_Click(object sender, EventArgs e)
        {
            if (agvList.FocusedItem == null)
            {
                MessageBox.Show("请选中需要操作的车辆", "提示");
                return;
            }
            else
            {
                if (!AgvForwardBtn.Enabled)
                {
                    StopAGV();
                }
                if (WcfMainHelper.SendOrder(agvList.FocusedItem.Text, new FControlOrder("后退", 1, 2)))
                {
                    //记录agv状态
                    agvStatus[agvList.FocusedItem.Text] = "backmove";
                    AgvBackwardBtn.Enabled = false;
                }
                else
                {
                    MessageBox.Show("请尝试再操作一次", "提示");
                }
            }
        }


        #endregion

        #region 任务操作
        /// <summary>
        /// 停止选中的任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void taskStopBtn_Click(object sender, EventArgs e)
        {
            if (currentTaskList.FocusedItem == null)
            {
                MessageBox.Show("请选择任务");
                return;
            }

            PublicDataContorl.StopTask(int.Parse(currentTaskList.FocusedItem.Text));
        }
        #endregion

        #region 任务启动配置

        /// <summary>
        /// 是否执行窑尾充电完成任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tailChargSucBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Do_TailChargeSucc = tailChargSucBtn.Checked;

        }

        /// <summary>
        /// 更改窑头充电任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headChargeBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Do_HeadCharge = headChargeBtn.Checked;

        }

        /// <summary>
        /// 自动生成任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoGenerateTaskBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Is_AutoAddTask = AutoGenerateTaskBtn.Checked;

        }

        /// <summary>
        /// 自动执行任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExecuteTaskBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Is_AutoExecuteTask = ExecuteTaskBtn.Checked;

        }

        /// <summary>
        /// 是否执行窑尾充电任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tailChargeBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Do_TailCharge = tailChargeBtn.Checked;

        }

        /// <summary>
        /// 是否执行窑头卸货任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headUnloadBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Do_HeadUnload = headUnloadBtn.Checked;

        }

        /// <summary>
        /// 是否执行窑尾接货任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tailLoadBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Do_TailLoad = tailLoadBtn.Checked;

        }

        /// <summary>
        /// 是否执行到窑头等待点任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headWaitBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Do_ToHeadWait = headWaitBtn.Checked;

        }

        /// <summary>
        /// 是否执行去窑尾等待点任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tailWaitBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Do_ToTailWait = tailWaitBtn.Checked;

        }

        /// <summary>
        /// 是否执行去窑头等待点的初始任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headWaitInitBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Do_InitToHeadWait = headWaitInitBtn.Checked;

        }

        /// <summary>
        /// 是否执行去窑尾等待点的初始化任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tailWaitInitBtn_Click(object sender, EventArgs e)
        {

            ParamControl.Do_InitToTailWait = tailWaitInitBtn.Checked;

        }

        /// <summary>
        /// 是否执行窑头充电完成任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headChargSucBtn_Click(object sender, EventArgs e)
        {

            ParamControl.Do_HeadChargeSucc = headChargSucBtn.Checked;

        }


        /// <summary>
        /// 控制是否全部打开任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void allOnOffBtn_Click(object sender, EventArgs e)
        {
            headChargeBtn.Checked = allOnOffBtn.Checked;
            headChargSucBtn.Checked = allOnOffBtn.Checked;
            headUnloadBtn.Checked = allOnOffBtn.Checked;
            headWaitBtn.Checked = allOnOffBtn.Checked;
            headWaitInitBtn.Checked = allOnOffBtn.Checked;

            tailChargeBtn.Checked = allOnOffBtn.Checked;
            tailChargSucBtn.Checked = allOnOffBtn.Checked;
            tailLoadBtn.Checked = allOnOffBtn.Checked;
            tailWaitBtn.Checked = allOnOffBtn.Checked;
            tailWaitInitBtn.Checked = allOnOffBtn.Checked;


            headChargeBtn_Click(sender, e);
            headChargSucBtn_Click(sender, e);
            headUnloadBtn_Click(sender, e);
            headWaitBtn_Click(sender, e);
            headWaitInitBtn_Click(sender, e);

            tailChargeBtn_Click(sender, e);
            tailChargSucBtn_Click(sender, e);
            tailLoadBtn_Click(sender, e);
            tailWaitBtn_Click(sender, e);
            tailWaitInitBtn_Click(sender, e);
        }

        #endregion


    }
}
