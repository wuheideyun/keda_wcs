namespace TCP
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.connectBtn = new System.Windows.Forms.Button();
            this.disconnectBtn = new System.Windows.Forms.Button();
            this.portTb = new System.Windows.Forms.TextBox();
            this.ipTb = new System.Windows.Forms.TextBox();
            this.receiveTb = new System.Windows.Forms.TextBox();
            this.transmitTb = new System.Windows.Forms.TextBox();
            this.sentBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.advanceBtn = new System.Windows.Forms.Button();
            this.retreatBtn = new System.Windows.Forms.Button();
            this.pauseBtn = new System.Windows.Forms.Button();
            this.rollBtn = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.DirectionLb = new System.Windows.Forms.Label();
            this.RollerLb = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.Set = new System.Windows.Forms.Button();
            this.OrderTb = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label9 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // connectBtn
            // 
            this.connectBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.connectBtn.Location = new System.Drawing.Point(65, 159);
            this.connectBtn.Name = "connectBtn";
            this.connectBtn.Size = new System.Drawing.Size(75, 23);
            this.connectBtn.TabIndex = 0;
            this.connectBtn.Text = "连接";
            this.connectBtn.UseVisualStyleBackColor = true;
            this.connectBtn.Click += new System.EventHandler(this.connectBtn_Click);
            // 
            // disconnectBtn
            // 
            this.disconnectBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.disconnectBtn.Location = new System.Drawing.Point(165, 159);
            this.disconnectBtn.Name = "disconnectBtn";
            this.disconnectBtn.Size = new System.Drawing.Size(75, 23);
            this.disconnectBtn.TabIndex = 1;
            this.disconnectBtn.Text = "断开";
            this.disconnectBtn.UseVisualStyleBackColor = true;
            this.disconnectBtn.Click += new System.EventHandler(this.disconnectBtn_Click);
            // 
            // portTb
            // 
            this.portTb.Location = new System.Drawing.Point(65, 120);
            this.portTb.Name = "portTb";
            this.portTb.Size = new System.Drawing.Size(175, 21);
            this.portTb.TabIndex = 2;
            this.portTb.Enter += new System.EventHandler(this.portTb_Enter);
            this.portTb.KeyDown += new System.Windows.Forms.KeyEventHandler(this.portTb_KeyDown);
            this.portTb.Leave += new System.EventHandler(this.portTb_Leave);
            // 
            // ipTb
            // 
            this.ipTb.Location = new System.Drawing.Point(65, 56);
            this.ipTb.Name = "ipTb";
            this.ipTb.Size = new System.Drawing.Size(175, 21);
            this.ipTb.TabIndex = 3;
            this.ipTb.Enter += new System.EventHandler(this.ipTb_Enter);
            this.ipTb.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ipTb_KeyDown);
            this.ipTb.Leave += new System.EventHandler(this.ipTb_Leave);
            // 
            // receiveTb
            // 
            this.receiveTb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.receiveTb.Location = new System.Drawing.Point(257, 68);
            this.receiveTb.Multiline = true;
            this.receiveTb.Name = "receiveTb";
            this.receiveTb.ReadOnly = true;
            this.receiveTb.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.receiveTb.Size = new System.Drawing.Size(423, 140);
            this.receiveTb.TabIndex = 4;
            // 
            // transmitTb
            // 
            this.transmitTb.Location = new System.Drawing.Point(257, 237);
            this.transmitTb.Multiline = true;
            this.transmitTb.Name = "transmitTb";
            this.transmitTb.Size = new System.Drawing.Size(423, 49);
            this.transmitTb.TabIndex = 5;
            // 
            // sentBtn
            // 
            this.sentBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.sentBtn.Location = new System.Drawing.Point(257, 300);
            this.sentBtn.Name = "sentBtn";
            this.sentBtn.Size = new System.Drawing.Size(75, 23);
            this.sentBtn.TabIndex = 6;
            this.sentBtn.Text = "发送";
            this.sentBtn.UseVisualStyleBackColor = true;
            this.sentBtn.Click += new System.EventHandler(this.sentBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("华文中宋", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(12, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 21);
            this.label1.TabIndex = 7;
            this.label1.Text = "IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("华文中宋", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(3, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "端口";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(42, 196);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(209, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "连接断开后，资源已释放无法重新连接";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("华文新魏", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(253, 211);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 21);
            this.label4.TabIndex = 10;
            this.label4.Text = "发送区";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("华文新魏", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(253, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 21);
            this.label5.TabIndex = 11;
            this.label5.Text = "显示区";
            // 
            // advanceBtn
            // 
            this.advanceBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.advanceBtn.Location = new System.Drawing.Point(260, 369);
            this.advanceBtn.Name = "advanceBtn";
            this.advanceBtn.Size = new System.Drawing.Size(61, 23);
            this.advanceBtn.TabIndex = 12;
            this.advanceBtn.Text = "前进";
            this.advanceBtn.UseVisualStyleBackColor = true;
            this.advanceBtn.Click += new System.EventHandler(this.advanceBtn_Click);
            // 
            // retreatBtn
            // 
            this.retreatBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.retreatBtn.Location = new System.Drawing.Point(327, 369);
            this.retreatBtn.Name = "retreatBtn";
            this.retreatBtn.Size = new System.Drawing.Size(61, 23);
            this.retreatBtn.TabIndex = 13;
            this.retreatBtn.Text = "后退";
            this.retreatBtn.UseVisualStyleBackColor = true;
            this.retreatBtn.Click += new System.EventHandler(this.retreatBtn_Click);
            // 
            // pauseBtn
            // 
            this.pauseBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.pauseBtn.Location = new System.Drawing.Point(394, 369);
            this.pauseBtn.Name = "pauseBtn";
            this.pauseBtn.Size = new System.Drawing.Size(61, 23);
            this.pauseBtn.TabIndex = 14;
            this.pauseBtn.Text = "暂停";
            this.pauseBtn.UseVisualStyleBackColor = true;
            this.pauseBtn.Click += new System.EventHandler(this.pauseBtn_Click);
            // 
            // rollBtn
            // 
            this.rollBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.rollBtn.Location = new System.Drawing.Point(461, 369);
            this.rollBtn.Name = "rollBtn";
            this.rollBtn.Size = new System.Drawing.Size(61, 23);
            this.rollBtn.TabIndex = 15;
            this.rollBtn.Text = "辊台动";
            this.rollBtn.UseVisualStyleBackColor = true;
            this.rollBtn.Click += new System.EventHandler(this.rollBtn_Click);
            // 
            // button5
            // 
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button5.Location = new System.Drawing.Point(528, 369);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(61, 23);
            this.button5.TabIndex = 16;
            this.button5.Text = "待添加";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(201, 374);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 17;
            this.label6.Text = "快捷操作";
            // 
            // DirectionLb
            // 
            this.DirectionLb.AutoSize = true;
            this.DirectionLb.Location = new System.Drawing.Point(14, 235);
            this.DirectionLb.Name = "DirectionLb";
            this.DirectionLb.Size = new System.Drawing.Size(89, 12);
            this.DirectionLb.TabIndex = 18;
            this.DirectionLb.Text = "小车状态：停止";
            // 
            // RollerLb
            // 
            this.RollerLb.AutoSize = true;
            this.RollerLb.Location = new System.Drawing.Point(14, 260);
            this.RollerLb.Name = "RollerLb";
            this.RollerLb.Size = new System.Drawing.Size(89, 12);
            this.RollerLb.TabIndex = 19;
            this.RollerLb.Text = "辊台状态：停止";
            // 
            // comboBox1
            // 
            this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "前进",
            "后退",
            "暂停",
            "辊台动"});
            this.comboBox1.Location = new System.Drawing.Point(260, 402);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(132, 20);
            this.comboBox1.TabIndex = 20;
            this.comboBox1.SelectedValueChanged += new System.EventHandler(this.comboBox1_SelectedValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(189, 405);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 21;
            this.label7.Text = "指令的修改";
            // 
            // Set
            // 
            this.Set.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Set.Location = new System.Drawing.Point(646, 399);
            this.Set.Name = "Set";
            this.Set.Size = new System.Drawing.Size(75, 23);
            this.Set.TabIndex = 25;
            this.Set.Text = "设置";
            this.Set.UseVisualStyleBackColor = true;
            this.Set.Click += new System.EventHandler(this.Set_Click);
            // 
            // OrderTb
            // 
            this.OrderTb.Location = new System.Drawing.Point(422, 401);
            this.OrderTb.Name = "OrderTb";
            this.OrderTb.Size = new System.Drawing.Size(196, 21);
            this.OrderTb.TabIndex = 26;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.DarkRed;
            this.label8.Location = new System.Drawing.Point(420, 429);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 12);
            this.label8.TabIndex = 27;
            this.label8.Text = "请注意指令是否重复";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1046, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(22, 19);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 28;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("华文中宋", 18F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label9.Location = new System.Drawing.Point(3, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(352, 27);
            this.label9.TabIndex = 29;
            this.label9.Text = "科达洁能AGV小车串口测试工具";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Location = new System.Drawing.Point(1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1071, 39);
            this.panel1.TabIndex = 30;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(679, 68);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(382, 140);
            this.textBox1.TabIndex = 31;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("华文新魏", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(746, 44);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(270, 21);
            this.label10.TabIndex = 32;
            this.label10.Text = "接收区（接收服务器的反馈）";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(948, 230);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 23);
            this.button1.TabIndex = 33;
            this.button1.Text = "接收AGV状态";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(14, 282);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 34;
            this.label11.Text = "当前站点：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(14, 305);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(77, 12);
            this.label12.TabIndex = 35;
            this.label12.Text = "当前速度比：";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(15, 330);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(89, 12);
            this.label13.TabIndex = 36;
            this.label13.Text = "当前运行方向：";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(16, 352);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(77, 12);
            this.label14.TabIndex = 37;
            this.label14.Text = "当前电量比：";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1073, 485);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.OrderTb);
            this.Controls.Add(this.Set);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.RollerLb);
            this.Controls.Add(this.DirectionLb);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.rollBtn);
            this.Controls.Add(this.pauseBtn);
            this.Controls.Add(this.retreatBtn);
            this.Controls.Add(this.advanceBtn);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sentBtn);
            this.Controls.Add(this.transmitTb);
            this.Controls.Add(this.receiveTb);
            this.Controls.Add(this.ipTb);
            this.Controls.Add(this.portTb);
            this.Controls.Add(this.disconnectBtn);
            this.Controls.Add(this.connectBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TCP";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button connectBtn;
        private System.Windows.Forms.Button disconnectBtn;
        private System.Windows.Forms.TextBox portTb;
        private System.Windows.Forms.TextBox ipTb;
        private System.Windows.Forms.TextBox receiveTb;
        private System.Windows.Forms.TextBox transmitTb;
        private System.Windows.Forms.Button sentBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button advanceBtn;
        private System.Windows.Forms.Button retreatBtn;
        private System.Windows.Forms.Button pauseBtn;
        private System.Windows.Forms.Button rollBtn;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label DirectionLb;
        private System.Windows.Forms.Label RollerLb;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button Set;
        private System.Windows.Forms.TextBox OrderTb;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
    }
}

