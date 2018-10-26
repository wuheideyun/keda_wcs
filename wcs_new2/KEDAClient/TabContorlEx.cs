using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KEDAClient
{
    public class TabControlEx : TabControl
    {
        private Color _BackColor; //背景颜色
        private Brush selectedBrush = new SolidBrush(Color.FromArgb(56, 56, 56));
        private Brush unSelectBrush = new SolidBrush(Color.FromArgb(98, 98, 98));
        //新建一个StringFormat对象，用于对标签文字的布局设置
        private StringFormat StrFormat = new StringFormat();
        private SolidBrush bruFont = new SolidBrush(Color.FromArgb(255, 255, 255));// 标签字体颜色
        private Font font = new System.Drawing.Font("宋体", 10F, FontStyle.Bold);//设置标签字体样式

        /// <summary>
        /// 构造函数
        /// </summary>
        public TabControlEx()
        {
            this.SetStyle(ControlStyles.UserPaint, true);//用户自己绘制
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);   //
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //让控件支持透明色
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.UpdateStyles();

            StrFormat.LineAlignment = StringAlignment.Center;// 设置文字垂直方向居中
            StrFormat.Alignment = StringAlignment.Center;// 设置文字水平方向居中          
        }

        /// <summary>
        /// 更改背景颜色
        /// </summary>
        public override Color BackColor
        {//重写backcolor属性 
            get
            {
                return this._BackColor;
            }
            set
            {
                this._BackColor = value;
            }
        }

        /// <summary>
        /// 绘制方法
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            this.DrawTitle(e.Graphics);
            base.OnPaint(e);
        }

        /// <summary>
        /// tab标题样式描绘方法
        /// </summary>
        /// <param name="g"></param>
        protected virtual void DrawTitle(Graphics g)
        {
            //绘制标签样式         
            foreach (TabPage tabPage in this.TabPages)
            {
                //获取标签头的工作区域
                Rectangle recChild = this.GetTabRect(this.TabPages.IndexOf(tabPage));
                Rectangle newRect = new Rectangle(recChild.Left, recChild.Top, recChild.Width, recChild.Height);
                //绘制标签头背景颜色
                //e.Graphics.DrawImage(imgButton, newRect);
                //当前选择Tab
                if (this.SelectedIndex == this.TabPages.IndexOf(tabPage))
                {
                    g.FillRectangle(selectedBrush, newRect);
                }
                else
                {
                    g.FillRectangle(unSelectBrush, newRect);
                }
                
                //绘制标签头的文字
                g.DrawString(tabPage.Text, font, bruFont, newRect, StrFormat);
            }
        }
    }

}
