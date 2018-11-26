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
        /// <summary>
        /// 构造函数
        /// </summary>
        public ControlForm()
        {
            InitializeComponent();
            InitPara();
        }

        private void ControlForm_Load(object sender, EventArgs e)
        {

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
