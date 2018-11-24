namespace DispatchAnmination
{
    partial class AnminationForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnminationForm));
            this.anminationPicBox = new System.Windows.Forms.PictureBox();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.anminateTimer = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.LinePosNegBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.DisplaySetBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.MapConfigBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.ReReadConfBtn = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.anminationPicBox)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // anminationPicBox
            // 
            this.anminationPicBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.anminationPicBox.Location = new System.Drawing.Point(0, 25);
            this.anminationPicBox.Name = "anminationPicBox";
            this.anminationPicBox.Size = new System.Drawing.Size(984, 736);
            this.anminationPicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.anminationPicBox.TabIndex = 0;
            this.anminationPicBox.TabStop = false;
            this.anminationPicBox.Paint += new System.Windows.Forms.PaintEventHandler(this.Anmination_picBox_Paint);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "icon.ico");
            this.imageList.Images.SetKeyName(1, "agv_car.png");
            this.imageList.Images.SetKeyName(2, "agv_load.png");
            this.imageList.Images.SetKeyName(3, "car.png");
            // 
            // anminateTimer
            // 
            this.anminateTimer.Enabled = true;
            this.anminateTimer.Interval = 500;
            this.anminateTimer.Tick += new System.EventHandler(this.AnminateTimer_Tick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.ReReadConfBtn});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(984, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LinePosNegBtn,
            this.DisplaySetBtn,
            this.MapConfigBtn});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(85, 22);
            this.toolStripDropDownButton1.Text = "线路配置";
            // 
            // LinePosNegBtn
            // 
            this.LinePosNegBtn.Name = "LinePosNegBtn";
            this.LinePosNegBtn.Size = new System.Drawing.Size(124, 22);
            this.LinePosNegBtn.Text = "运动尺寸";
            this.LinePosNegBtn.Click += new System.EventHandler(this.LinePosNegBtn_Click);
            // 
            // DisplaySetBtn
            // 
            this.DisplaySetBtn.Name = "DisplaySetBtn";
            this.DisplaySetBtn.Size = new System.Drawing.Size(124, 22);
            this.DisplaySetBtn.Text = "显示设置";
            this.DisplaySetBtn.Click += new System.EventHandler(this.DisplaySetBtn_Click);
            // 
            // MapConfigBtn
            // 
            this.MapConfigBtn.Name = "MapConfigBtn";
            this.MapConfigBtn.Size = new System.Drawing.Size(124, 22);
            this.MapConfigBtn.Text = "地图配置";
            this.MapConfigBtn.Click += new System.EventHandler(this.MapConfigBtn_Click);
            // 
            // ReReadConfBtn
            // 
            this.ReReadConfBtn.Image = ((System.Drawing.Image)(resources.GetObject("ReReadConfBtn.Image")));
            this.ReReadConfBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ReReadConfBtn.Name = "ReReadConfBtn";
            this.ReReadConfBtn.Size = new System.Drawing.Size(76, 22);
            this.ReReadConfBtn.Text = "重读配置";
            this.ReReadConfBtn.Click += new System.EventHandler(this.ReReadConfBtn_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.anminationPicBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 25, 0, 0);
            this.panel1.Size = new System.Drawing.Size(984, 761);
            this.panel1.TabIndex = 2;
            // 
            // AnminationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 761);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AnminationForm";
            this.Text = "DispatchAnmination";
            this.Load += new System.EventHandler(this.AnminationForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.anminationPicBox)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox anminationPicBox;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Timer anminateTimer;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem LinePosNegBtn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem DisplaySetBtn;
        private System.Windows.Forms.ToolStripMenuItem MapConfigBtn;
        private System.Windows.Forms.ToolStripButton ReReadConfBtn;
    }
}

