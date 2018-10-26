using FLBasicHelper;
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
            //调度任务列表
            currentTaskList.Columns.Add("ID", 50, HorizontalAlignment.Center);
            currentTaskList.Columns.Add("任务", 150, HorizontalAlignment.Center);
            currentTaskList.Columns.Add("路径", 100, HorizontalAlignment.Center);

            currentTaskList.View = System.Windows.Forms.View.Details;


            //Agv列表
            agvList.Columns.Add("AGV名称", 80, HorizontalAlignment.Center);
            agvList.Columns.Add("状态", 100, HorizontalAlignment.Center);

            agvList.View = System.Windows.Forms.View.Details;
        }

        #endregion

        #region ListView刷新
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

            String text = null;
            
            if (currentTaskList.FocusedItem != null)
            {
                text = currentTaskList.FocusedItem.Text;
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

            if(text != null)
            {
                int index = currentTaskList.Items.IndexOfKey(text);
                if(index != -1)
                {
                    currentTaskList.Items[index].Selected = true;
                }
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

            String text = null;
            if (agvList.FocusedItem != null) {
                text = agvList.FocusedItem.Text;
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

            if (text != null)
            {
                int index = agvList.Items.IndexOfKey(text);
                if (index != -1)
                {
                    agvList.Items[index].Selected = true;
                }
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



        #endregion

        #region 任务操作
        /// <summary>
        /// 停止选中的任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void taskStopBtn_Click(object sender, EventArgs e)
        {
            if(currentTaskList.FocusedItem == null)
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
            if (tailChargSucBtn.Checked)
            {
                ParamControl.Do_TailChargeSucc = true;
            }
            else
            {
                ParamControl.Do_TailChargeSucc = false;
            }
        }

        /// <summary>
        /// 更改窑头充电任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headChargeBtn_Click(object sender, EventArgs e)
        {
            if (headChargeBtn.Checked)
            {
                ParamControl.Do_HeadCharge = true;
            }
            else
            {
                ParamControl.Do_HeadCharge = false;
            }
        }
        
        /// <summary>
        /// 自动生成任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoGenerateTaskBtn_Click(object sender, EventArgs e)
        {
            if (AutoGenerateTaskBtn.Checked)
            {
                ParamControl.Is_AutoAddTask = true;
            }
            else
            {
                ParamControl.Is_AutoAddTask = false;
            }
        }

        /// <summary>
        /// 自动执行任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExecuteTaskBtn_Click(object sender, EventArgs e)
        {
            if (ExecuteTaskBtn.Checked)
            {
                ParamControl.Is_AutoExecuteTask = true;

            }
            else
            {
                ParamControl.Is_AutoExecuteTask = false;
            }
        }

        /// <summary>
        /// 是否执行窑尾充电任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tailChargeBtn_Click(object sender, EventArgs e)
        {
            if (tailChargeBtn.Checked)
            {
                ParamControl.Do_TailCharge = true;
            }
            else
            {
                ParamControl.Do_TailCharge = false;
            }
        }

        /// <summary>
        /// 是否执行窑头卸货任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headUnloadBtn_Click(object sender, EventArgs e)
        {
            if (headUnloadBtn.Checked)
            {
                ParamControl.Do_HeadUnload = true;
            }
            else
            {
                ParamControl.Do_HeadUnload = false;
            }
        }

        /// <summary>
        /// 是否执行窑尾接货任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tailLoadBtn_Click(object sender, EventArgs e)
        {
            if (tailLoadBtn.Checked)
            {
                ParamControl.Do_TailLoad = true;
            }
            else
            {
                ParamControl.Do_TailLoad = false;
            }
        }

        /// <summary>
        /// 是否执行到窑头等待点任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headWaitBtn_Click(object sender, EventArgs e)
        {
            if (headWaitBtn.Checked)
            {
                ParamControl.Do_ToHeadWait = true;
            }
            else
            {
                ParamControl.Do_ToHeadWait = false;
            }
        }

        /// <summary>
        /// 是否执行去窑尾等待点任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tailWaitBtn_Click(object sender, EventArgs e)
        {
            if (tailWaitBtn.Checked)
            {
                ParamControl.Do_ToTailWait = true;
            }
            else
            {
                ParamControl.Do_ToTailWait = false;
            }
        }

        /// <summary>
        /// 是否执行去窑头等待点的初始任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headWaitInitBtn_Click(object sender, EventArgs e)
        {
            if (headWaitInitBtn.Checked)
            {
                ParamControl.Do_InitToHeadWait = true;
            }
            else
            {
                ParamControl.Do_InitToHeadWait = false;
            }
        }

        /// <summary>
        /// 是否执行去窑尾等待点的初始化任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tailWaitInitBtn_Click(object sender, EventArgs e)
        {
            if (tailWaitInitBtn.Checked)
            {
                ParamControl.Do_InitToTailWait = true;
            }
            else
            {
                ParamControl.Do_InitToTailWait = false;
            }
        }
        /// <summary>
        /// 是否执行窑头充电完成任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headChargSucBtn_Click(object sender, EventArgs e)
        {
            if (headChargSucBtn.Checked)
            {
                ParamControl.Do_HeadChargeSucc = true;
            }
            else
            {
                ParamControl.Do_HeadChargeSucc = false;
            }
        }
        #endregion
    }
}
