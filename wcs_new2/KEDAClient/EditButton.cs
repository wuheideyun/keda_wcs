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
    
    public partial class EditButton : UserControl
    {


        public EditButton()
        {
            InitializeComponent();
        }

        bool isSave = false;

        protected override void OnPaint(PaintEventArgs e)
        {

            Rectangle rec = new Rectangle(0, 0, this.Size.Width, this.Size.Height);

            if (isSave)
            {
                e.Graphics.DrawString("保存", this.Font, new SolidBrush(Color.Black), rec);

            }
            else
            {
                e.Graphics.DrawString("修改", this.Font, new SolidBrush(Color.Black), rec);

            }
        }

        private void EditButtonCheck_Click(object sender, EventArgs e)
        {
            isSave = !isSave;
            this.Invalidate();
        }
    }
}
