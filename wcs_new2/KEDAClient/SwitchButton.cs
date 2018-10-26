using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KEDAClient
{
    public enum CheckStyle
    {
        style1 = 0,
        style2 = 1,
        style3 = 2,
        style4 = 3,
        style5 = 4,
        style6 = 5,
        style7 = 6,
        style8 = 7
    };

    public partial class SwitchButton : UserControl
    {
        public SwitchButton()
        {
            InitializeComponent();

            //设置Style支持透明背景色并且双缓冲
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.BackColor = Color.Transparent;

            this.Cursor = Cursors.Hand;
            this.Size = new Size(87, 27);
        }

        bool isCheck = false;

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Checked
        {
            set { isCheck = value; this.Invalidate(); }
            get { return isCheck; }
        }

        CheckStyle checkStyle = CheckStyle.style1;

        /// <summary>
        /// 样式
        /// </summary>
        public CheckStyle CheckStyleX
        {
            set { checkStyle = value; this.Invalidate(); }
            get { return checkStyle; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String TextName
        {
            set;get;
        }

        /// <summary>
        /// 标题在控件中开始的位置点
        /// </summary>
        public Point TextStartPoint
        {
            set;get;
        } 

        protected override void OnPaint(PaintEventArgs e)
        {
            Bitmap bitMapOn = null;
            Bitmap bitMapOff = null;

            if (checkStyle == CheckStyle.style1)
            {
                bitMapOn = global::KEDAClient.Properties.Resources.btncheckon1;
                bitMapOff = global::KEDAClient.Properties.Resources.btncheckoff1;
            }
            else if (checkStyle == CheckStyle.style2)
            {
                bitMapOn = global::KEDAClient.Properties.Resources.btncheckon2;
                bitMapOff = global::KEDAClient.Properties.Resources.btncheckoff2;
            }
            else if (checkStyle == CheckStyle.style3)
            {
                bitMapOn = global::KEDAClient.Properties.Resources.btncheckon3;
                bitMapOff = global::KEDAClient.Properties.Resources.btncheckoff3;
            }
            else if (checkStyle == CheckStyle.style4)
            {
                bitMapOn = global::KEDAClient.Properties.Resources.btncheckon4;
                bitMapOff = global::KEDAClient.Properties.Resources.btncheckoff4;
            }
            else if (checkStyle == CheckStyle.style5)
            {
                bitMapOn = global::KEDAClient.Properties.Resources.btncheckon5;
                bitMapOff = global::KEDAClient.Properties.Resources.btncheckoff5;
            }
            else if (checkStyle == CheckStyle.style6)
            {
                bitMapOn = global::KEDAClient.Properties.Resources.btncheckon6;
                bitMapOff = global::KEDAClient.Properties.Resources.btncheckoff6;
            }
            else if(checkStyle == CheckStyle.style7)
            {
                bitMapOn = global::KEDAClient.Properties.Resources.btnunlock2;
                bitMapOff = global::KEDAClient.Properties.Resources.btnlock2;
            }
            else if (checkStyle == CheckStyle.style8)
            {
                bitMapOn = global::KEDAClient.Properties.Resources.btnUnlock;
                bitMapOff = global::KEDAClient.Properties.Resources.btnlock;
            }

            Rectangle rec = new Rectangle(0, 0, this.Size.Width, this.Size.Height);

            if (isCheck)
            {
                e.Graphics.DrawImage(bitMapOn, rec);
            }
            else
            {
                e.Graphics.DrawImage(bitMapOff, rec);
            }

            if(TextName != null && TextName.Length > 0)
            {
                e.Graphics.DrawString(TextName,this.Font,new SolidBrush(Color.Black),TextStartPoint);
            }
            
        }

        private void SwitchButtonCheck_Click(object sender, EventArgs e)
        {
            isCheck = !isCheck;
            this.Invalidate();
        }
    }
}
