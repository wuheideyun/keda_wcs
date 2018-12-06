using DispatchAnmination.AgvLine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XMLHelper;

namespace DispatchAnmination
{
    public partial class LineInfoForm : Form
    {
        private static LineInfoForm lineInfoForm;
        private XmlAnalyze xml;
        private List<AgvLineData> AgvLineDatas;
        private int AgvLineSelectedIndex = -1;
        public LineInfoForm()
        {
            InitializeComponent();
            AgvLineListView.Columns.Add("开始点", 60, HorizontalAlignment.Left);
            AgvLineListView.Columns.Add("特殊", 45, HorizontalAlignment.Left);
            AgvLineListView.Columns.Add("目标点", 60, HorizontalAlignment.Left);
            AgvLineListView.Columns.Add("移动尺量", 70, HorizontalAlignment.Left);

            LinePointListView.Columns.Add("X", 50, HorizontalAlignment.Center);
            LinePointListView.Columns.Add("Y", 50, HorizontalAlignment.Center);

            xml = new XmlAnalyze();
            AgvLineDatas = new List<AgvLineData>();
        }
        public static LineInfoForm NewInstance()
        {
            if(lineInfoForm == null || lineInfoForm.IsDisposed)
            {
                lineInfoForm = new LineInfoForm();
            }
            return lineInfoForm;
        }

        private void LineInfoForm_Load(object sender, EventArgs e)
        {
            xml.DoAnalyze();
            AgvLineDatas = xml.AgvLineList;
            AgvLineListViewRefresh();
        }

        /// <summary>
        /// Agv线路选择改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgvLineListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            AgvLineSelectedIndex = AgvLineListView.FocusedItem.Index;
            LinePointListViewRefresh();
            LineNowSiteTB.Text = AgvLineDatas[AgvLineSelectedIndex].NowSite + "";
            LineSpecialCB.Checked = AgvLineDatas[AgvLineSelectedIndex].IsSpecial;
            LineDesSiteCB.SelectedIndex = GetDestSiteIndex(AgvLineDatas[AgvLineSelectedIndex].DesSite);
            LineMoveSizeTB.Text = AgvLineDatas[AgvLineSelectedIndex].MoveSize + "";
        }


        /// <summary>
        /// Agv线路更新
        /// </summary>
        private void AgvLineListViewRefresh()
        {
            AgvLineListView.Items.Clear();
            LinePointListView.Items.Clear();
            if (AgvLineDatas.Count == 0) return;
            AgvLineListView.BeginUpdate();
            foreach (var agvline in AgvLineDatas)
            {
                ListViewItem item = new ListViewItem(agvline.NowSite + ""); // 起始地标
                item.SubItems.Add(agvline.IsSpecial ? "是":"否"); //特殊节点
                item.SubItems.Add(agvline.DesSite + ""); //目标站点
                item.SubItems.Add(agvline.MoveSize + ""); //移动尺量
                AgvLineListView.Items.Add(item);
            }
            //// 结束数据处理
            AgvLineListView.EndUpdate();
            AgvLineSelectedIndex = -1;
        }


        /// <summary>
        /// 线路坐标更新
        /// </summary>
        private void LinePointListViewRefresh()
        {
            if (AgvLineSelectedIndex == -1)
            {
                MessageBox.Show("请先选择线路！");
                return;
            }

            LinePointListView.Items.Clear();
            LinePointListView.BeginUpdate();
            foreach (var point in AgvLineDatas[AgvLineSelectedIndex].Points)
            {
                ListViewItem item = new ListViewItem(point.X + ""); // 起始地标
                item.SubItems.Add(point.Y + ""); //特殊节点
                LinePointListView.Items.Add(item);
            }
            //// 结束数据处理
            LinePointListView.EndUpdate();
        }

        /// <summary>
        /// 使用地图解析的初步地标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UseXmlAnBtn_Click(object sender, EventArgs e)
        {
            AgvLineDatas.Clear();
            foreach(Line line in LineDateCenter._linesPositive)
            {
                AgvLineData data = new AgvLineData
                {
                   NowSite = line.LineID,
                   MoveSize = 0.1F
                };
                foreach( var p in line._points)
                {
                    data.Points.Add(new AgvPoint { X = p.X, Y = p.Y });
                }
                AgvLineDatas.Add(data);
            }

            AgvLineListViewRefresh();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAgvLineBtn_Click(object sender, EventArgs e)
        {
            xml.SaveAgvLineToFile(AgvLineDatas);
            xml.DoAnalyze();
            AgvLineDatas = xml.AgvLineList;
            AgvLineListViewRefresh();
        }

        /// <summary>
        /// 添加线路
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddLineBtn_Click(object sender, EventArgs e)
        {
            if (LineNowSiteTB.Text.Length == 0)
            {
                MessageBox.Show("请先填写信息");
                return;
            }
            AgvLineData line = new AgvLineData
            {
                NowSite = int.Parse(LineNowSiteTB.Text),
                IsSpecial = LineSpecialCB.Checked,
                DesSite = GetDestSite(LineDesSiteCB.SelectedIndex),
                MoveSize = float.Parse(LineMoveSizeTB.Text)
            };

            AgvLineDatas.Add(line);
            AgvLineListViewRefresh();
        }
        /// <summary>
        //1号站点(11地标)
        //4号站点(14地标)
        //12完成点
        //15完成点
        //31等待点
        //33等待点
        //52充电点
        //53充电点
        //55充电点
        //56充电点
        /// </summary>
        private int[] DesSite = {1,4,12,15,33,36,52,53,55,56 };
        public int GetDestSite(int index)
        {
            if (index >= DesSite.Length) return 0;
            return DesSite[index];
        }

        public int GetDestSiteIndex(int dessite)
        {
            for(int i = 0; i< DesSite.Length; i++)
            {
                if (DesSite[i] == dessite)
                {
                    return i;
                }
            }
            return 0;
        }

        private void EditeLineBtn_Click(object sender, EventArgs e)
        {
            if (AgvLineSelectedIndex == -1)
            {
                MessageBox.Show("请先选择线路！");
                return;
            }
            AgvLineDatas[AgvLineSelectedIndex].NowSite = int.Parse(LineNowSiteTB.Text);
            AgvLineDatas[AgvLineSelectedIndex].DesSite = GetDestSite(LineDesSiteCB.SelectedIndex);
            AgvLineDatas[AgvLineSelectedIndex].IsSpecial = LineSpecialCB.Checked;
            AgvLineDatas[AgvLineSelectedIndex].MoveSize = float.Parse(LineMoveSizeTB.Text);
            
            AgvLineListViewRefresh();
        }

        private void LineMoveSizeTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            //如果输入的不是数字键，也不是回车键、Backspace键，则取消该输入
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8 && e.KeyChar !=(char)46)
            {
                e.Handled = true;
            }
        }

        private void AddPointBtn_Click(object sender, EventArgs e)
        {
            if (AgvLineSelectedIndex == -1)
            {
                MessageBox.Show("请先选择线路！");
                return;
            }
            if(PointXTB.Text.Length == 0 || PointYTB.Text.Length == 0)
            {
                MessageBox.Show("请先输入点的信息！");
                return;
            }
            AgvLineDatas[AgvLineSelectedIndex].AddPoint(new AgvPoint{ X=int.Parse(PointXTB.Text),Y= int.Parse(PointYTB.Text)});
            LinePointListViewRefresh();
        }

        private int LinePointSelectedIndex = -1;
        private void LinePointListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            LinePointSelectedIndex = LinePointListView.FocusedItem.Index;
            PointXTB.Text = AgvLineDatas[AgvLineSelectedIndex].Points[LinePointSelectedIndex].X + "";
            PointYTB.Text = AgvLineDatas[AgvLineSelectedIndex].Points[LinePointSelectedIndex].Y + "";

        }

        private void EditPointBtn_Click(object sender, EventArgs e)
        {
            if (AgvLineSelectedIndex == -1)
            {
                MessageBox.Show("请先选择线路！");
                return;
            }
            if (PointXTB.Text.Length == 0 || PointYTB.Text.Length == 0)
            {
                MessageBox.Show("请先输入点的信息！");
                return;
            }
            AgvLineDatas[AgvLineSelectedIndex].Points[LinePointSelectedIndex].X = int.Parse(PointXTB.Text);
            AgvLineDatas[AgvLineSelectedIndex].Points[LinePointSelectedIndex].Y = int.Parse(PointYTB.Text);
            LinePointListViewRefresh();
        }

        private void PointXTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            //如果输入的不是数字键，也不是回车键、Backspace键，则取消该输入
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void PointYTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            //如果输入的不是数字键，也不是回车键、Backspace键，则取消该输入
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void PointUpBtn_Click(object sender, EventArgs e)
        {
            if (LinePointSelectedIndex == -1)
            {
                MessageBox.Show("请先选择点");
                return;
            }

            if (LinePointSelectedIndex == 0) { return; }
            AgvPoint upP = AgvLineDatas[AgvLineSelectedIndex].Points[LinePointSelectedIndex - 1];
            AgvLineDatas[AgvLineSelectedIndex].Points[LinePointSelectedIndex - 1] = AgvLineDatas[AgvLineSelectedIndex].Points[LinePointSelectedIndex];
            AgvLineDatas[AgvLineSelectedIndex].Points[LinePointSelectedIndex] = upP;

            LinePointListViewRefresh();
        }

        private void PontDownBtn_Click(object sender, EventArgs e)
        {
            if (LinePointSelectedIndex == -1)
            {
                MessageBox.Show("请先选择点");
                return;
            }

            if (LinePointSelectedIndex+1 >= AgvLineDatas[AgvLineSelectedIndex].Points.Count-1) { return; }
            AgvPoint downP = AgvLineDatas[AgvLineSelectedIndex].Points[LinePointSelectedIndex + 1];
            AgvLineDatas[AgvLineSelectedIndex].Points[LinePointSelectedIndex +1] = AgvLineDatas[AgvLineSelectedIndex].Points[LinePointSelectedIndex];
            AgvLineDatas[AgvLineSelectedIndex].Points[LinePointSelectedIndex] = downP;

            LinePointListViewRefresh();
        }

        private void DeleteLineBtn_Click(object sender, EventArgs e)
        {
            AgvLineDatas.RemoveAt(AgvLineSelectedIndex);
            AgvLineListViewRefresh();
            LinePointListViewRefresh();
        }

        private void DeletePointBtn_Click(object sender, EventArgs e)
        {
            if (LinePointSelectedIndex == -1)
            {
                MessageBox.Show("请先选择点");
                return;
            }

            AgvLineDatas[AgvLineSelectedIndex].Points.RemoveAt(LinePointSelectedIndex);
            LinePointListViewRefresh();
        }

        private void LineUpBtn_Click(object sender, EventArgs e)
        {
            if (AgvLineSelectedIndex == -1)
            {
                MessageBox.Show("请先选择线路！");
                return;
            }
            if (AgvLineSelectedIndex == 0) return;
            AgvLineData line = AgvLineDatas[AgvLineSelectedIndex-1];
            AgvLineDatas[AgvLineSelectedIndex - 1] = AgvLineDatas[AgvLineSelectedIndex];
            AgvLineDatas[AgvLineSelectedIndex] = line;
            AgvLineSelectedIndex--;
            AgvLineListViewRefresh();
        }

        private void LineDownBtn_Click(object sender, EventArgs e)
        {
            if (AgvLineSelectedIndex == -1)
            {
                MessageBox.Show("请先选择线路！");
                return;
            }
            if (AgvLineSelectedIndex+1 >= AgvLineDatas.Count-1) return;
            AgvLineData line = AgvLineDatas[AgvLineSelectedIndex + 1];
            AgvLineDatas[AgvLineSelectedIndex + 1] = AgvLineDatas[AgvLineSelectedIndex];
            AgvLineDatas[AgvLineSelectedIndex] = line;
            AgvLineSelectedIndex++;
            AgvLineListViewRefresh();
        }
    }
}
