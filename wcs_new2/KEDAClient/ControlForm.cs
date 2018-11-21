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
            currentTaskList.Columns.Add("ID", 0, HorizontalAlignment.Left);
            currentTaskList.Columns.Add("任务", 220, HorizontalAlignment.Left);
            currentTaskList.Columns.Add("路径", 100, HorizontalAlignment.Left);

            currentTaskList.View = System.Windows.Forms.View.Details;


            List<DeviceBackImf> devDatas = WcfMainHelper.GetDevList();
            if (devDatas == null)
            {
                MessageBox.Show("请先启动服务");
                Close();
                return;
            }
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
                PlcStaMonitorLab.Text = _plc.Sta_Monitor();
                PlcErrorLab.Text = _plc.Sta_Error();
                PlcSpareLab.Text = _plc.SpareInform();

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

        #region 定时任务配置

        
        /// <summary>
        /// 是否执行进窑头充电任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enterheadChargeBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Do_EnterHeadCharge = enterheadChargeBtn.Checked;

        }
        
        /// <summary>
        /// 是否执行进窑尾充电任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enterendChargeBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Do_EnterEndCharge = enterendChargeBtn.Checked;

        }

        /// <summary>
        /// 是否执行出窑头充电任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitheadChargeBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Do_ExitHeadCharge = exitheadChargeBtn.Checked;

        }

        /// <summary>
        /// 是否执行出窑尾充电任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitendChargeBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Do_ExitEndCharge = exitendChargeBtn.Checked;
        }

        /// <summary>
        /// 是否执行进窑头充电完成任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enterheadChargSucBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Do_EnterHeadChargeSucc = enterheadChargSucBtn.Checked;
        }

        /// <summary>
        /// 是否执行进窑尾充电完成任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enterendChargSucBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Do_EnterEndChargeSucc = enterendChargSucBtn.Checked;
        }

        /// <summary>
        /// 是否执行出窑头充电完成任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitheadChargSucBtn_Click(object sender, EventArgs e)
        {

            ParamControl.Do_ExitHeadChargeSucc  = exitheadChargSucBtn.Checked;
        }

        /// <summary>
        /// 是否执行出窑尾充电完成任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitendChargSucBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Do_ExitEndChargeSucc = exitendChargSucBtn.Checked;
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
        private void endLoadBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Do_EndLoad = endLoadBtn.Checked;

        }

        /// <summary>
        /// 是否执行到窑尾对接完成点任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void endSucBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Do_ToEndSuc = endSucBtn.Checked;

        }

        /// <summary>
        /// 是否执行去窑头对接完成点任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headSucBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Do_ToHeadSuc = headSucBtn.Checked;

        }

        /// <summary>
        /// 是否执行去窑尾等任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void endWaitBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Do_ToEndWait = endWaitBtn.Checked;

        }

        /// <summary>
        /// 是否执行去窑头等任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headWaitBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Do_ToHeadWait = headWaitBtn.Checked;

        }
         

        /// <summary>
        /// 控制是否全部打开任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void allOnOffBtn_Click(object sender, EventArgs e)
        {
            enterheadChargeBtn.Checked = allOnOffBtn.Checked;
            enterheadChargSucBtn.Checked = allOnOffBtn.Checked;
            headUnloadBtn.Checked = allOnOffBtn.Checked;
            headSucBtn.Checked = allOnOffBtn.Checked;
            headWaitBtn.Checked = allOnOffBtn.Checked;

            enterendChargeBtn.Checked = allOnOffBtn.Checked;
            enterendChargSucBtn.Checked = allOnOffBtn.Checked;
            endLoadBtn.Checked = allOnOffBtn.Checked;
            endSucBtn.Checked = allOnOffBtn.Checked;
            endWaitBtn.Checked = allOnOffBtn.Checked;

            enterheadChargeBtn_Click(sender, e);
            exitheadChargeBtn_Click(sender, e);
            enterheadChargSucBtn_Click(sender, e);
            exitheadChargSucBtn_Click(sender, e);
            headUnloadBtn_Click(sender, e);
            headWaitBtn_Click(sender, e);
            headSucBtn_Click(sender, e);

            enterendChargeBtn_Click(sender, e);
            exitendChargeBtn_Click(sender, e);
            enterendChargSucBtn_Click(sender, e);
            exitendChargSucBtn_Click(sender, e);
            endLoadBtn_Click(sender, e);
            endWaitBtn_Click(sender, e);
            endSucBtn_Click(sender, e);

        }

        #endregion

        #region 测试配置

        /// <summary>
        /// 控制不能输入数字以外的字符
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headAgvPlcRunTimeBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))//如果不是输入数字就不让输入
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 控制不能输入数字以外的字符
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tailAgvPlcRunTimeBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))//如果不是输入数字就不让输入
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 窑头电机运行发送时间输入框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headAgvPlcRunTimeBox_TextChanged(object sender, EventArgs e)
        {
            if (headAgvPlcRunTimeBox.Text.Equals("") || int.Parse(headAgvPlcRunTimeBox.Text) < 30)
            {
                headAgvPlcRunTimeBox.Text = "30";
            }
            ParamControl.IgnoreHeadUnloadSecond = int.Parse(headAgvPlcRunTimeBox.Text);
        }

        /// <summary>
        /// 窑尾电机运行发送时间输入框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tailAgvPlcRunTimeBox_TextChanged(object sender, EventArgs e)
        {
            if (tailAgvPlcRunTimeBox.Text.Equals("") || int.Parse(tailAgvPlcRunTimeBox.Text) < 30)
            {
                tailAgvPlcRunTimeBox.Text = "30";
            }
            ParamControl.IgnoreTailUnloadSecond = int.Parse(tailAgvPlcRunTimeBox.Text);
        }

        /// <summary>
        /// 是否忽略窑头放料Agv棍台电机状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IgnoreHeadStaStatusBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Is_IgnoreHeadStaStatus = IgnoreHeadStaStatusBtn.Checked;

        }

        /// <summary>
        /// 是否忽略窑尾出货Agv棍台电机状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IgnoreTailStaStatusBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Is_IgnoreTailStaStatus = IgnoreTailStaStatusBtn.Checked;
        }

        #endregion

        #region 任务配置

        /// <summary>
        /// 是否忽略窑尾出货Agv和Plc的货物状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IgnoreTailStatusBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Is_IgnoreTailUnloadStatus = IgnoreTailStatusBtn.Checked;
        }

        /// <summary>
        /// 是否忽略窑头放料Agv和Plc的货物状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IgnoreHeadStatusBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Is_IgnoreHeadUnloadStatus = IgnoreHeadStatusBtn.Checked;
        }



        /// <summary>
        /// 窑头等 到 窑头卸  忽略窑头无货状态 让AGV过去窑头卸
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IgHeadUnLoadTask_Click(object sender, EventArgs e)
        {
            ParamControl.IgnoreHeadUnloadTask = IgHeadUnLoadTask.Checked;
        }

        /// <summary>
        /// 窑头卸 到 窑尾等  忽略AGV的无货状态 让AGV过去窑尾等
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IgAgvUnLoadTask_Click(object sender, EventArgs e)
        {
            ParamControl.IgnoreAgvUnloadTask = IgAgvUnLoadTask.Checked;
        }


        /// <summary>
        /// 窑尾等 到 窑尾取  忽略窑尾有货状态 让AGV过去接货
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IgTailLoadTask_Click(object sender, EventArgs e)
        {
            ParamControl.IgnoreTailLoadTask = IgTailLoadTask.Checked;
        }

        /// <summary>
        /// 窑尾取 到 窑头等  忽略AGV有货状态 让AGV窑头等
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IgAgvLoadTask_Click(object sender, EventArgs e)
        {
            ParamControl.IgnoreAgvLoadTask = IgAgvLoadTask.Checked;
        }
        #endregion

        #region 锁定状态控制


        /// <summary>
        /// 是否解锁窑头状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeadPlcLockBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Do_HeadPlcLock = HeadPlcLockBtn.Checked;
        }

        /// <summary>
        /// 是否解锁进窑头充电桩状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnterHeadChargeLBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Do_EnterHeadChargeLock = EnterHeadChargeLBtn.Checked;
        }

        /// <summary>
        /// 是否解锁出窑头充电桩状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitHeadChargeLBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Do_ExitHeadChargeLock = ExitHeadChargeLBtn.Checked;
        }


        /// <summary>
        /// 是否解锁窑尾状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EndPlcLockBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Do_EndPlcLock = EndPlcLockBtn.Checked;
        }

        /// <summary>
        /// 是否解锁进窑尾充电桩状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnterEndChargeLBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Do_EnterEndChargeLock = EnterEndChargeLBtn.Checked;
        }

        /// <summary>
        /// 是否解锁出窑尾充电桩状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitEndChargeLBtn_Click(object sender, EventArgs e)
        {
            ParamControl.Do_ExitEndChargeLock = ExitEndChargeLBtn.Checked;

        }

        private void RefreshLockBtn()
        {
            HeadPlcLockBtn.Checked = ParamControl.Do_HeadPlcLock;

            EndPlcLockBtn.Checked = ParamControl.Do_EndPlcLock;



        }


        #endregion
    }
}
