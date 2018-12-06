using DispatchAnmination.AgvLine;
using DispatchAnmination.Config;
using DispatchAnmination.MapConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using XMLHelper;
using WcfHelper;
using DataContract;
using FLBasicHelper;
using FLCommonInterfaces;
using KEDAClient;
using DispatchAnmination.Const;
using System.Threading;

namespace DispatchAnmination
{
    public partial class AnminationForm : Form
    {
        Anmination anmination;
        XmlAnalyze xml;
        public AnminationForm()
        {
            InitializeComponent();

            Thread thread = new Thread(InitPara)
            {
                IsBackground = true
            };
            thread.Start();

            F_DevManager dev = new F_DevManager();

            FLog.Init();
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
        

        private void AnminationForm_Load(object sender, EventArgs e)
        {
            anmination = new Anmination(imageList);

            xml = new XmlAnalyze();
            xml.DoAnalyze();

            ModuleControl.AddLinesToModule(xml._lineDatas);
            AgvLineMaster.AddLine(xml.AgvLineList);

            LineDateCenter.AddLineData();

            InitDispConfig();
        }

        private void InitDispConfig()
        {
            string value = GetBoolDisplayConfig("all");
            if (value != null)
            {
                ConstBA.IsShow_Site = value.Equals("1") ? true : false;
            }

            value = GetBoolDisplayConfig("upname");
            if (value != null)
            {
                ConstBA.IsShow_SiteUpName = value.Equals("1") ? true : false;
            }

            value = GetBoolDisplayConfig("sitename");
            if (value != null)
            {
                ConstBA.IsShow_SiteName = value.Equals("1") ? true : false;
            }

            value = GetBoolDisplayConfig("sitepoint");
            if (value != null)
            {
                ConstBA.IsShow_SitePoint = value.Equals("1") ? true : false;
            }

            value = GetBoolDisplayConfig("headtialsite");
            if (value != null)
            {
                ConstBA.IsShow_WaiteSite = value.Equals("1") ? true : false;
            }

            value = GetBoolDisplayConfig("waitesite");
            if (value != null)
            {
                ConstBA.IsShow_HeadTialSite = value.Equals("1") ? true : false;
            }

            value = GetBoolDisplayConfig("swervesite");
            if (value != null)
            {
                ConstBA.IsShow_SwerveSite = value.Equals("1") ? true : false;
            }

            value = GetBoolDisplayConfig("trunroundsite");
            if (value != null)
            {
                ConstBA.IsShow_TrunRoundSite = value.Equals("1") ? true : false;
            }

            value = GetBoolDisplayConfig("chargesite");
            if (value != null)
            {
                ConstBA.IsShow_ChargeSite = value.Equals("1") ? true : false;
            }

            value = GetBoolDisplayConfig("trafficesite");
            if (value != null)
            {
                ConstBA.IsShow_TrafficSite = value.Equals("1") ? true : false;
            }

            value = GetBoolDisplayConfig("nottrafficsite");
            if (value != null)
            {
                ConstBA.IsShow_NotTrafficSite = value.Equals("1") ? true : false;
            }

            value = GetBoolDisplayConfig("sitefinish");
            if (value != null)
            {
                ConstBA.IsShow_FinishSite = value.Equals("1") ? true : false;
            }

            value = GetBoolDisplayConfig("incresite");
            if (value != null)
            {
                ConstBA.IsShow_IncreSite = value.Equals("1") ? true : false;
            }

            value = GetBoolDisplayConfig("offline");
            if (value != null)
            {
                ConstBA.IsShow_OffLineAGV = value.Equals("1") ? true : false;
            }

            value = GetBoolDisplayConfig("showlinepoint");
            if (value != null)
            {
                ConstBA.IsShow_LinePoint = value.Equals("1") ? true : false;
            }
            
        }
        public string GetBoolDisplayConfig(string namex )
        {
           DisplayConfig display =  xml._disConfig.Find(c => { return c.name.Equals(namex); });
            if (display != null)
            {
                return display.value;
            }
            return null;
        }


        private Point point = new Point(500, 500);

        /// <summary>
        /// 画面更新方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Anmination_picBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            anmination.Draw(graphics, point);
        }


        private void AnminateTimer_Tick(object sender, EventArgs e)
        {
            point = Control.MousePosition;
            anminationPicBox.Invalidate();
        }

        private void LinePosNegBtn_Click(object sender, EventArgs e)
        {
            LineInfoForm.NewInstance().Show();
        }

        private void DisplaySetBtn_Click(object sender, EventArgs e)
        {
            DisplaySetForm.NewInstance().Show();
        }

        private void MapConfigBtn_Click(object sender, EventArgs e)
        {
            MapConfigForm.NewInstance().Show();
        }

        private void ReReadConfBtn_Click(object sender, EventArgs e)
        {
            anminateTimer.Enabled = false;
            xml = new XmlAnalyze();
            xml.DoAnalyze();

            ModuleControl.AddLinesToModule(xml._lineDatas);
            AgvLineMaster.AddLine(xml.AgvLineList);

            LineDateCenter.AddLineData();
            anminateTimer.Enabled = true;
        }
    }
}
