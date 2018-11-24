using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XMLHelper;

namespace DispatchAnmination.MapConfig
{
    public partial class MapConfigForm : Form
    {
        public MapConfigForm()
        {
            InitializeComponent();
            LineListView.Columns.Add("Index", 60, HorizontalAlignment.Center);
            LineListView.Columns.Add("SX", 40, HorizontalAlignment.Center);
            LineListView.Columns.Add("SY", 40, HorizontalAlignment.Center);
            LineListView.Columns.Add("EX", 40, HorizontalAlignment.Center);
            LineListView.Columns.Add("EY", 40, HorizontalAlignment.Center);

            LineSiteListView.Columns.Add("ID", 60, HorizontalAlignment.Center);
            LineSiteListView.Columns.Add("Rate", 60, HorizontalAlignment.Center);



            XmlInit();
        }
        private XmlAnalyze xmlAnalyze;

        private void XmlInit()
        {
            xmlAnalyze = new XmlAnalyze();
            xmlAnalyze.DoAnalyze("mapconf.xml");
            foreach(var lineDate in xmlAnalyze._lineDatas)
            {
                _lineModule.Add(new LineModule(lineDate));
            }
            LineListViewRefresh();
            LineSiteListViewRefresh();
        }

        private static MapConfigForm _form;
        public static MapConfigForm NewInstance()
        {
            if(_form==null || _form.IsDisposed)
            {
                _form = new MapConfigForm();
            }
            return _form;
        }

        private bool IsNewLine = false;
        private Point LineStartP, LineEndP;

        private List<LineModule> _lineModule = new List<LineModule>();

        private void MapConfigPB_MouseDown(object sender, MouseEventArgs e)
        {
            if (IsNewLine)
            {
                LineStartP = new Point(e.X, e.Y);
            }
        }

        private void AddNewLineBtn_Click(object sender, EventArgs e)
        {
            IsNewLine = true;
        }


        private void MapConfigPB_MouseUp(object sender, MouseEventArgs e)
        {
            if (IsNewLine)
            {
                LineEndP = new Point(e.X, e.Y);
                _lineModule.Add(new LineModule(LineStartP, LineEndP));
                LineListViewRefresh();
                IsNewLine = false;
            }
        }
        Pen redPen = new Pen(new SolidBrush(Color.Red));
        Pen blackPen = new Pen(new SolidBrush(Color.Black));


        private void MapConfigPB_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            foreach(var line in _lineModule)
            {
                if (LineSelectedIndex>=0 && line == _lineModule[LineSelectedIndex])
                {
                    line.SetRedPen(redPen);
                }else
                {
                    line.SetRedPen(blackPen);
                }
                line.Draw(g);
                
            }
        }

        private void AddNewSiteBtn_Click(object sender, EventArgs e)
        {
            if (LineSelectedIndex == -1)
            {
                MessageBox.Show("请选择线路！");
                return;
            } else if (SiteIDTB.Text.Length == 0 || SiteRateTB.Text.Length==0 ||
                SiteTypeCB.SelectedIndex == -1 || SiteDirecationCB.SelectedIndex==-1)
            {
                MessageBox.Show("请先填写和选择站点信息");
                return;
            }
            _lineModule[LineSelectedIndex].AddSitePos(int.Parse(SiteIDTB.Text), int.Parse(SiteRateTB.Text),
                SiteDirecationCB.SelectedIndex,(SiteType) SiteTypeCB.SelectedIndex, SiteNameTB.Text,SiteUpNameTB.Text);
            LineSiteListViewRefresh();
        }

        private void MapConfigPB_MouseMove(object sender, MouseEventArgs e)
        {
            LineEndP = new Point(e.X,e.Y);
        }
        LineModule module;
        private void LineListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            LineSelectedIndex = LineListView.FocusedItem.Index;
            module = _lineModule[LineSelectedIndex];
            LineStartPxTB.Text = module._centerP.X + "";
            LineStartPyTB.Text = module._centerP.Y + "";
            LineEndPxTB.Text = module._endP.X + "";
            LineEndPyTB.Text = module._endP.Y + "";

            LineSiteListViewRefresh();
            LineSelectedLab.Text = LineSelectedIndex + "";
        }

        /// <summary>
        /// 定时任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapTimer_Tick(object sender, EventArgs e)
        {
            MapTimer.Enabled = false;
            MapConfigPB.Invalidate();
            MapTimer.Enabled = true;
        }

        /// <summary>
        /// 更新线路列表
        /// </summary>
        private void LineListViewRefresh()
        {
            LineListView.Items.Clear();
            LineSiteListView.Items.Clear();
            if (_lineModule.Count == 0)
            {
                return;
            }

            LineListView.BeginUpdate();
            foreach(var line in _lineModule)
            {
                ListViewItem item = new ListViewItem(_lineModule.IndexOf(line) + "");
                item.SubItems.Add(line._centerP.X + "");
                item.SubItems.Add(line._centerP.Y + "");
                item.SubItems.Add(line._endP.X + "");
                item.SubItems.Add(line._endP.Y + "");
                LineListView.Items.Add(item);
            }
            LineListView.EndUpdate();
            ClearLineSite();
            ClearLine();
        }

        /// <summary>
        /// 站点选择更改触发方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LineSiteListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            SitePos site = _lineModule[LineSelectedIndex].SitePos[LineSiteListView.FocusedItem.Index];
            SiteIDTB.Text = site.ID + "";
            SiteNameTB.Text = site.Name;
            SiteRateTB.Text = site._rate + "";
            SiteUpNameTB.Text = site.UpName;
            SiteDirecationCB.SelectedIndex = site._direction;
            SiteTypeCB.SelectedIndex = (int)site._type;
        }
        /// <summary>
        /// 当前选择的线路
        /// </summary>
        private int LineSelectedIndex = -1;

        /// <summary>
        /// 修改线路
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditLineBtn_Click(object sender, EventArgs e)
        {
            if (LineSelectedIndex == -1)
            {
                MessageBox.Show("请选择线路！");
                return;
            }
            _lineModule[LineSelectedIndex]._centerP.X = int.Parse(LineStartPxTB.Text);
            _lineModule[LineSelectedIndex]._centerP.Y = int.Parse(LineStartPyTB.Text);
            _lineModule[LineSelectedIndex]._endP.X = int.Parse(LineEndPxTB.Text);
            _lineModule[LineSelectedIndex]._endP.Y = int.Parse(LineEndPyTB.Text);
            _lineModule[LineSelectedIndex].UpdateSiteP();
        }

        /// <summary>
        /// 删除线路
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteLineBtn_Click(object sender, EventArgs e)
        {
            if (LineSelectedIndex == -1)
            {
                MessageBox.Show("请选择线路！");
                return;
            }
            _lineModule.RemoveAt(LineSelectedIndex);
            LineSelectedIndex = -1;
            LineListViewRefresh();
            ClearLine();
        }

        /// <summary>
        /// 站点列表刷新
        /// </summary>
        private void LineSiteListViewRefresh()
        {
            LineSiteListView.Items.Clear();
            if (_lineModule.Count < LineSelectedIndex || LineSelectedIndex==-1)
            {
                return;
            }

            LineSiteListView.BeginUpdate();
            foreach (var site in _lineModule[LineSelectedIndex].SitePos)
            {
                ListViewItem item = new ListViewItem(site.ID+ "");
                item.SubItems.Add(site._rate + "");
                LineSiteListView.Items.Add(item);
            }
            LineSiteListView.EndUpdate();
            ClearLineSite();
        }

        private void SaveToMapFileBtn_Click(object sender, EventArgs e)
        {
            List<LineData> lineDatas = new List<LineData>();
            foreach(var line in _lineModule)
            {
                lineDatas.Add(line.ToLineData());
            }
            xmlAnalyze.SaveLineToFile(lineDatas);
            //xmlAnalyze.SaveMapConfigFile();
        }

        private void EditLineSiteBtn_Click(object sender, EventArgs e)
        {
            if (LineSelectedIndex == -1)
            {
                MessageBox.Show("请选择线路！");
                return;
            }
            else if (SiteIDTB.Text.Length == 0 || SiteRateTB.Text.Length == 0 ||
              SiteTypeCB.SelectedIndex == -1 || SiteDirecationCB.SelectedIndex == -1)
            {
                MessageBox.Show("请先填写和选择站点信息");
                return;
            }
            _lineModule[LineSelectedIndex].SitePos[LineSiteListView.FocusedItem.Index].ID = int.Parse(SiteIDTB.Text);
            _lineModule[LineSelectedIndex].SitePos[LineSiteListView.FocusedItem.Index]._rate = int.Parse(SiteRateTB.Text);
            _lineModule[LineSelectedIndex].SitePos[LineSiteListView.FocusedItem.Index]._direction = SiteDirecationCB.SelectedIndex;
            _lineModule[LineSelectedIndex].SitePos[LineSiteListView.FocusedItem.Index]._type = (SiteType)SiteTypeCB.SelectedIndex;
            _lineModule[LineSelectedIndex].SitePos[LineSiteListView.FocusedItem.Index].Name = SiteNameTB.Text;
            _lineModule[LineSelectedIndex].SitePos[LineSiteListView.FocusedItem.Index].UpName = SiteUpNameTB.Text;


                //.AddSitePos(int.Parse(SiteIDTB.Text), int.Parse(SiteRateTB.Text),
                //SiteDirecationCB.SelectedIndex, (SiteType)SiteTypeCB.SelectedIndex, SiteNameTB.Text, SiteUpNameTB.Text);
            LineSiteListViewRefresh();
        }

        /// <summary>
        /// 清空线路输入框
        /// </summary>
        private void ClearLineSite()
        {
            SiteIDTB.Text = "";
            SiteNameTB.Text = "";
            SiteRateTB.Text =  "";
            SiteUpNameTB.Text = "";
            SiteDirecationCB.SelectedIndex =0;
            SiteTypeCB.SelectedIndex =0;
        }

        private void LineNumberInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            //如果输入的不是数字键，也不是回车键、Backspace键，则取消该输入
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void LineUpBtn_Click(object sender, EventArgs e)
        {
            if (LineSelectedIndex == -1)
            {
                MessageBox.Show("请选择线路！");
                return;
            }
            if (LineSelectedIndex ==0) { return; }
            LineModule line = _lineModule[LineSelectedIndex-1];
            _lineModule[LineSelectedIndex-1] = _lineModule[LineSelectedIndex];
            _lineModule[LineSelectedIndex] = line;
            LineListViewRefresh();
        }

        private void LineDownBtn_Click(object sender, EventArgs e)
        {
            if (LineSelectedIndex == -1)
            {
                MessageBox.Show("请选择线路！");
                return;
            }
            if (LineSelectedIndex == _lineModule.Count-1) { return; }
            LineModule line = _lineModule[LineSelectedIndex + 1];
            _lineModule[LineSelectedIndex + 1] = _lineModule[LineSelectedIndex];
            _lineModule[LineSelectedIndex] = line;
            LineListViewRefresh();
        }

        /// <summary>
        /// 清空站点输入框
        /// </summary>
        private void ClearLine()
        {
            LineStartPxTB.Text = "";
            LineStartPyTB.Text =  "";
            LineEndPxTB.Text = "";
            LineEndPyTB.Text = "";
        }
    }
}
