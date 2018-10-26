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
        }

        private void ControlForm_Load(object sender, EventArgs e)
        {
            
            ListView_Init();
        }

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

        /// <summary>
        /// ListView初始化 标题
        /// </summary>
        private void ListView_Init()
        {
            //调度任务列表
            currentTaskList.Columns.Add("任务", 150, HorizontalAlignment.Center);
            currentTaskList.Columns.Add("路径", 100, HorizontalAlignment.Center);

            currentTaskList.View = System.Windows.Forms.View.Details;


            //Agv列表
            //agvli
        }


        /// <summary>
        /// ListBox的刷新线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerForListRefresh_Tick(object sender, EventArgs e)
        {
            timerForListRefresh.Enabled = false;
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
            currentTaskList.BeginUpdate();

            foreach (var data in taskDatas)
            {
                ListViewItem item = new ListViewItem(data.Msg); // 任务信息
                item.SubItems.Add(data.SiteMsg); //站点信息
                currentTaskList.Items.Add(item);
            }

            // 结束数据处理
            currentTaskList.EndUpdate();
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
                ParamControl.Do_HeadCharge = false;
            }
            else
            {
                ParamControl.Do_HeadCharge = true;
            }
        }



        /// <summary>
        /// 初始化指定的AGV
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgvInitBtn_Click(object sender, EventArgs e)
        {
            
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
    }
}
