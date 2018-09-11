using Gfx.GfxDataManagerServer;
using Gfx.RCommData;
using GfxCommonInterfaces;
using GfxServiceContractClient;
using GfxServiceContractTaskExcute;
using JTWcfHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HQHPCtrol
{
    public partial class FormCTR : Form
    {
        LogicCtroCenter _ctr = null;

        public FormCTR()
        {
            InitializeComponent();

            labelStartTime.Text = string.Format("启动时间：{0}",DateTime.Now);

            _ctr = new LogicCtroCenter();

            labelTask.Visible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            try
            {
                labelMain.Text = string.Format("服务端：{0}", _ctr.MainConnected ? "已连接" : "未连接");

                labelTask.Text = string.Format("任务端：{0}", _ctr.TaskConnected ? "已连接" : "未连接");

                textBox1.Text = (_ctr.WaitManager.WaitDic[1].BandingDev == null) ? "无" : _ctr.WaitManager.WaitDic[1].BandingDev.DevID;

                textBox2.Text = (_ctr.WaitManager.WaitDic[2].BandingDev == null) ? "无" : _ctr.WaitManager.WaitDic[2].BandingDev.DevID;

                textBox3.Text = (_ctr.WaitManager.WaitDic[3].BandingDev == null) ? "无" : _ctr.WaitManager.WaitDic[3].BandingDev.DevID;

                textBox4.Text = (_ctr.WaitManager.WaitDic[4].BandingDev == null) ? "无" : _ctr.WaitManager.WaitDic[4].BandingDev.DevID;

                textBox5.Text = (_ctr.WaitManager.WaitDic[5].BandingDev == null) ? "无" : _ctr.WaitManager.WaitDic[5].BandingDev.DevID;

                textBox6.Text = (_ctr.WaitManager.WaitDic[6].BandingDev == null) ? "无" : _ctr.WaitManager.WaitDic[6].BandingDev.DevID;
               
                textBox7.Text = (_ctr.WaitManager.WaitDic[7].BandingDev == null) ? "无" : _ctr.WaitManager.WaitDic[7].BandingDev.DevID;
            }
            catch { }

            timer1.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;  
           
            e.Cancel = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("重置任务后会重新开始当前触发任务,确定触发？","警告", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
            
            }
        }

        /// <summary>
        /// 清除绑定AGV
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("清除数据可能会到时逻辑异常,确定清除？", "警告", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                if (sender is Button)
                {
                    Button btn = sender as Button;

                    int num = 0;

                    string s = btn.Name.Remove(0, btn.Name.Length - 1);

                    if (Int32.TryParse(s, out num))
                    {
                        _ctr.WaitManager.WaitDic[num].BandingDev = null;
                    }
                }
            }
        }
    }
}
