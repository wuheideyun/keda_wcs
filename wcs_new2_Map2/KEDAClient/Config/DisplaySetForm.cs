using DispatchAnmination.Const;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XMLHelper;

namespace DispatchAnmination.Config
{
    public partial class DisplaySetForm : Form
    {
        private static DisplaySetForm _DisplaSetForm;
        public DisplaySetForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化窗口
        /// </summary>
        /// <returns></returns>
        public static DisplaySetForm NewInstance()
        {
            if(_DisplaSetForm==null || _DisplaSetForm.IsDisposed)
            {
                _DisplaSetForm = new DisplaySetForm();
            }
            return _DisplaSetForm;
        }
        /// <summary>
        /// 是否展示站点名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IsShowSiteNameCB_CheckedChanged(object sender, EventArgs e)
        {
            ConstBA.IsShow_SiteName = IsShowSiteNameCB.Checked;
        }

        private void IsShowSitePointCB_CheckedChanged(object sender, EventArgs e)
        {
            ConstBA.IsShow_SitePoint = IsShowSitePointCB.Checked;
        }

        private void IsShowSiteUpCB_CheckedChanged(object sender, EventArgs e)
        {
            ConstBA.IsShow_SiteUpName = IsShowSiteUpCB.Checked;

        }

        private void IsShowSiteCB_CheckedChanged(object sender, EventArgs e)
        {
            ConstBA.IsShow_Site = IsShowSiteCB.Checked;
        }

        private void DisplaySetForm_Load(object sender, EventArgs e)
        {
            IsShowSiteUpCB.Checked = ConstBA.IsShow_SiteUpName;
            IsShowSiteCB.Checked = ConstBA.IsShow_Site; 
            IsShowSitePointCB.Checked = ConstBA.IsShow_SitePoint;
            IsShowSiteNameCB.Checked = ConstBA.IsShow_SiteName;
            IsShowHeadTialSiteCB.Checked = ConstBA.IsShow_HeadTialSite ;
            IsShowWaiteSiteCB.Checked = ConstBA.IsShow_WaiteSite ;
            IsShowSwerveSiteCB.Checked = ConstBA.IsShow_SwerveSite;
            IsShowTrunRoundSiteCB.Checked = ConstBA.IsShow_TrunRoundSite;
            IsShowChargeSiteCB.Checked = ConstBA.IsShow_ChargeSite;
            IsShowTrafficSiteCB.Checked = ConstBA.IsShow_TrafficSite;
            IsShowNotTrafficSiteCB.Checked = ConstBA.IsShow_NotTrafficSite;
            IsShowSiteFinishCB.Checked = ConstBA.IsShow_FinishSite;
            IsShowIncreCB.Checked = ConstBA.IsShow_IncreSite;
            IsShowOfflineCB.Checked = ConstBA.IsShow_OffLineAGV;
            IsShowLinePCB.Checked = ConstBA.IsShow_LinePoint;

        }

        private void IsShowHeadTialSiteCB_CheckedChanged(object sender, EventArgs e)
        {
            ConstBA.IsShow_HeadTialSite = IsShowHeadTialSiteCB.Checked;
        }

        private void IsShowWaiteSiteCB_CheckedChanged(object sender, EventArgs e)
        {
            ConstBA.IsShow_WaiteSite = IsShowWaiteSiteCB.Checked;
        }

        private void IsShowSwerveSiteCB_CheckedChanged(object sender, EventArgs e)
        {
            ConstBA.IsShow_SwerveSite = IsShowSwerveSiteCB.Checked;
        }

        private void IsShowTrunRoundSiteCB_CheckedChanged(object sender, EventArgs e)
        {
            ConstBA.IsShow_TrunRoundSite = IsShowTrunRoundSiteCB.Checked;
        }

        private void IsShowChargeSiteCB_CheckedChanged(object sender, EventArgs e)
        {
            ConstBA.IsShow_ChargeSite = IsShowChargeSiteCB.Checked;
        }

        private void IsShowTrafficSiteCB_CheckedChanged(object sender, EventArgs e)
        {
            ConstBA.IsShow_TrafficSite = IsShowTrafficSiteCB.Checked;
        }

        private void IsShowNotTrafficSiteCB_CheckedChanged(object sender, EventArgs e)
        {
            ConstBA.IsShow_NotTrafficSite = IsShowNotTrafficSiteCB.Checked;
        }

        private void IsShowSiteFinishCB_CheckedChanged(object sender, EventArgs e)
        {
            ConstBA.IsShow_FinishSite = IsShowSiteFinishCB.Checked;
        }

        private void IsShowIncreCB_CheckedChanged(object sender, EventArgs e)
        {
            ConstBA.IsShow_IncreSite = IsShowIncreCB.Checked;
        }

        private void IsShowOfflineCB_CheckedChanged(object sender, EventArgs e)
        {
            ConstBA.IsShow_OffLineAGV = IsShowOfflineCB.Checked;
            ConstBA.Init_ShowLineAGV = IsShowOfflineCB.Checked;
        }

        private void IsShowLinePCB_CheckedChanged(object sender, EventArgs e)
        {
            ConstBA.IsShow_LinePoint = IsShowLinePCB.Checked;
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveDisconfigBtn_Click(object sender, EventArgs e)
        {
            XmlAnalyze xml = new XmlAnalyze();
            xml.DoAnalyze();
            xml.SaveDispalyConfig(GetConfigList());
        }


        private List<DisplayConfig> GetConfigList()
        {
            List<DisplayConfig> list = new List<DisplayConfig>();
            list.Add(new DisplayConfig { name="all",value= ConstBA.IsShow_Site ? "1":"0"});
            list.Add(new DisplayConfig { name="upname",value = ConstBA.IsShow_SiteUpName ? "1" : "0" });
            list.Add(new DisplayConfig { name="sitename",value = ConstBA.IsShow_SiteName ? "1" : "0" });
            list.Add(new DisplayConfig { name="sitepoint",value = ConstBA.IsShow_SitePoint ? "1" : "0" });
            list.Add(new DisplayConfig { name= "headtialsite", value = ConstBA.IsShow_HeadTialSite ? "1" : "0" });
            list.Add(new DisplayConfig { name="waitesite",value = ConstBA.IsShow_WaiteSite ? "1" : "0" });
            list.Add(new DisplayConfig { name="swervesite",value = ConstBA.IsShow_SwerveSite ? "1" : "0" });
            list.Add(new DisplayConfig { name="trunroundsite",value = ConstBA.IsShow_TrunRoundSite ? "1" : "0" });
            list.Add(new DisplayConfig { name="chargesite",value = ConstBA.IsShow_ChargeSite ? "1" : "0" });
            list.Add(new DisplayConfig { name="trafficesite",value = ConstBA.IsShow_TrafficSite ? "1" : "0" });
            list.Add(new DisplayConfig { name="nottrafficsite",value = ConstBA.IsShow_NotTrafficSite ? "1" : "0" });
            list.Add(new DisplayConfig { name="sitefinish",value = ConstBA.IsShow_FinishSite ? "1" : "0" });
            list.Add(new DisplayConfig { name="incresite",value = ConstBA.IsShow_IncreSite ? "1" : "0" });
            list.Add(new DisplayConfig { name="offline",value = ConstBA.IsShow_OffLineAGV ? "1" : "0" });
            list.Add(new DisplayConfig { name="showlinepoint",value = ConstBA.IsShow_LinePoint ? "1" : "0" });

            return list;
        }
    }
}
