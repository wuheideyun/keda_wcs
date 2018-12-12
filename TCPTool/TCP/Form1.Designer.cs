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
            this.components = new System.ComponentModel.Container();
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
            this.PonintLb = new System.Windows.Forms.Label();
            this.SpeedLb = new System.Windows.Forms.Label();
            this.Direction = new System.Windows.Forms.Label();
            this.Electric = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.checkBox16 = new System.Windows.Forms.CheckBox();
            this.checkBox17 = new System.Windows.Forms.CheckBox();
            this.checkBox18 = new System.Windows.Forms.CheckBox();
            this.checkBox19 = new System.Windows.Forms.CheckBox();
            this.checkBox20 = new System.Windows.Forms.CheckBox();
            this.checkBox11 = new System.Windows.Forms.CheckBox();
            this.checkBox12 = new System.Windows.Forms.CheckBox();
            this.checkBox13 = new System.Windows.Forms.CheckBox();
            this.checkBox14 = new System.Windows.Forms.CheckBox();
            this.checkBox15 = new System.Windows.Forms.CheckBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.checkBox8 = new System.Windows.Forms.CheckBox();
            this.checkBox9 = new System.Windows.Forms.CheckBox();
            this.checkBox10 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            this.receiveTb.Size = new System.Drawing.Size(464, 140);
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
            this.DirectionLb.Location = new System.Drawing.Point(6, 17);
            this.DirectionLb.Name = "DirectionLb";
            this.DirectionLb.Size = new System.Drawing.Size(89, 12);
            this.DirectionLb.TabIndex = 18;
            this.DirectionLb.Text = "小车状态：停止";
            // 
            // RollerLb
            // 
            this.RollerLb.AutoSize = true;
            this.RollerLb.Location = new System.Drawing.Point(139, 17);
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
            this.Set.Location = new System.Drawing.Point(624, 399);
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
            this.pictureBox1.Location = new System.Drawing.Point(1226, 0);
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
            this.panel1.Size = new System.Drawing.Size(1248, 39);
            this.panel1.TabIndex = 30;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(727, 68);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(509, 140);
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
            this.button1.Location = new System.Drawing.Point(727, 214);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 23);
            this.button1.TabIndex = 33;
            this.button1.Text = "接收AGV状态";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // PonintLb
            // 
            this.PonintLb.AutoSize = true;
            this.PonintLb.Location = new System.Drawing.Point(263, 17);
            this.PonintLb.Name = "PonintLb";
            this.PonintLb.Size = new System.Drawing.Size(65, 12);
            this.PonintLb.TabIndex = 34;
            this.PonintLb.Text = "当前站点：";
            // 
            // SpeedLb
            // 
            this.SpeedLb.AutoSize = true;
            this.SpeedLb.Location = new System.Drawing.Point(375, 17);
            this.SpeedLb.Name = "SpeedLb";
            this.SpeedLb.Size = new System.Drawing.Size(77, 12);
            this.SpeedLb.TabIndex = 35;
            this.SpeedLb.Text = "当前速度比：";
            // 
            // Direction
            // 
            this.Direction.AutoSize = true;
            this.Direction.Location = new System.Drawing.Point(6, 43);
            this.Direction.Name = "Direction";
            this.Direction.Size = new System.Drawing.Size(89, 12);
            this.Direction.TabIndex = 36;
            this.Direction.Text = "当前运行方向：";
            // 
            // Electric
            // 
            this.Electric.AutoSize = true;
            this.Electric.Location = new System.Drawing.Point(139, 43);
            this.Electric.Name = "Electric";
            this.Electric.Size = new System.Drawing.Size(77, 12);
            this.Electric.TabIndex = 37;
            this.Electric.Text = "当前电量比：";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(727, 243);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(113, 23);
            this.button2.TabIndex = 38;
            this.button2.Text = "关闭接收AGV信息";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.checkBox16);
            this.groupBox1.Controls.Add(this.checkBox17);
            this.groupBox1.Controls.Add(this.checkBox18);
            this.groupBox1.Controls.Add(this.checkBox19);
            this.groupBox1.Controls.Add(this.checkBox20);
            this.groupBox1.Controls.Add(this.checkBox11);
            this.groupBox1.Controls.Add(this.checkBox12);
            this.groupBox1.Controls.Add(this.checkBox13);
            this.groupBox1.Controls.Add(this.checkBox14);
            this.groupBox1.Controls.Add(this.checkBox15);
            this.groupBox1.Controls.Add(this.checkBox6);
            this.groupBox1.Controls.Add(this.checkBox7);
            this.groupBox1.Controls.Add(this.checkBox8);
            this.groupBox1.Controls.Add(this.checkBox9);
            this.groupBox1.Controls.Add(this.checkBox10);
            this.groupBox1.Controls.Add(this.checkBox5);
            this.groupBox1.Controls.Add(this.checkBox4);
            this.groupBox1.Controls.Add(this.checkBox3);
            this.groupBox1.Controls.Add(this.checkBox2);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.DirectionLb);
            this.groupBox1.Controls.Add(this.RollerLb);
            this.groupBox1.Controls.Add(this.Electric);
            this.groupBox1.Controls.Add(this.PonintLb);
            this.groupBox1.Controls.Add(this.Direction);
            this.groupBox1.Controls.Add(this.SpeedLb);
            this.groupBox1.Location = new System.Drawing.Point(727, 272);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(509, 233);
            this.groupBox1.TabIndex = 39;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "状态与故障";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(0, 177);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Size = new System.Drawing.Size(509, 56);
            this.textBox2.TabIndex = 58;
            // 
            // checkBox16
            // 
            this.checkBox16.AutoSize = true;
            this.checkBox16.Location = new System.Drawing.Point(400, 69);
            this.checkBox16.Name = "checkBox16";
            this.checkBox16.Size = new System.Drawing.Size(114, 16);
            this.checkBox16.TabIndex = 57;
            this.checkBox16.Text = "寻迹传感器2异常";
            this.checkBox16.UseVisualStyleBackColor = true;
            // 
            // checkBox17
            // 
            this.checkBox17.AutoSize = true;
            this.checkBox17.Location = new System.Drawing.Point(400, 91);
            this.checkBox17.Name = "checkBox17";
            this.checkBox17.Size = new System.Drawing.Size(114, 16);
            this.checkBox17.TabIndex = 56;
            this.checkBox17.Text = "寻迹传感器3异常";
            this.checkBox17.UseVisualStyleBackColor = true;
            // 
            // checkBox18
            // 
            this.checkBox18.AutoSize = true;
            this.checkBox18.Location = new System.Drawing.Point(401, 113);
            this.checkBox18.Name = "checkBox18";
            this.checkBox18.Size = new System.Drawing.Size(114, 16);
            this.checkBox18.TabIndex = 55;
            this.checkBox18.Text = "寻迹传感器4异常";
            this.checkBox18.UseVisualStyleBackColor = true;
            // 
            // checkBox19
            // 
            this.checkBox19.AutoSize = true;
            this.checkBox19.Location = new System.Drawing.Point(401, 135);
            this.checkBox19.Name = "checkBox19";
            this.checkBox19.Size = new System.Drawing.Size(114, 16);
            this.checkBox19.TabIndex = 54;
            this.checkBox19.Text = "寻迹传感器5异常";
            this.checkBox19.UseVisualStyleBackColor = true;
            // 
            // checkBox20
            // 
            this.checkBox20.AutoSize = true;
            this.checkBox20.Location = new System.Drawing.Point(401, 157);
            this.checkBox20.Name = "checkBox20";
            this.checkBox20.Size = new System.Drawing.Size(114, 16);
            this.checkBox20.TabIndex = 53;
            this.checkBox20.Text = "寻迹传感器6异常";
            this.checkBox20.UseVisualStyleBackColor = true;
            // 
            // checkBox11
            // 
            this.checkBox11.AutoSize = true;
            this.checkBox11.Location = new System.Drawing.Point(265, 69);
            this.checkBox11.Name = "checkBox11";
            this.checkBox11.Size = new System.Drawing.Size(114, 16);
            this.checkBox11.TabIndex = 52;
            this.checkBox11.Text = "寻迹传感器3离线";
            this.checkBox11.UseVisualStyleBackColor = true;
            // 
            // checkBox12
            // 
            this.checkBox12.AutoSize = true;
            this.checkBox12.Location = new System.Drawing.Point(265, 91);
            this.checkBox12.Name = "checkBox12";
            this.checkBox12.Size = new System.Drawing.Size(114, 16);
            this.checkBox12.TabIndex = 51;
            this.checkBox12.Text = "寻迹传感器4离线";
            this.checkBox12.UseVisualStyleBackColor = true;
            // 
            // checkBox13
            // 
            this.checkBox13.AutoSize = true;
            this.checkBox13.Location = new System.Drawing.Point(265, 113);
            this.checkBox13.Name = "checkBox13";
            this.checkBox13.Size = new System.Drawing.Size(114, 16);
            this.checkBox13.TabIndex = 50;
            this.checkBox13.Text = "寻迹传感器5离线";
            this.checkBox13.UseVisualStyleBackColor = true;
            // 
            // checkBox14
            // 
            this.checkBox14.AutoSize = true;
            this.checkBox14.Location = new System.Drawing.Point(265, 135);
            this.checkBox14.Name = "checkBox14";
            this.checkBox14.Size = new System.Drawing.Size(114, 16);
            this.checkBox14.TabIndex = 49;
            this.checkBox14.Text = "寻迹传感器6离线";
            this.checkBox14.UseVisualStyleBackColor = true;
            // 
            // checkBox15
            // 
            this.checkBox15.AutoSize = true;
            this.checkBox15.Location = new System.Drawing.Point(265, 157);
            this.checkBox15.Name = "checkBox15";
            this.checkBox15.Size = new System.Drawing.Size(114, 16);
            this.checkBox15.TabIndex = 48;
            this.checkBox15.Text = "寻迹传感器1异常";
            this.checkBox15.UseVisualStyleBackColor = true;
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Location = new System.Drawing.Point(138, 69);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(108, 16);
            this.checkBox6.TabIndex = 47;
            this.checkBox6.Text = "避障传感器错误";
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // checkBox7
            // 
            this.checkBox7.AutoSize = true;
            this.checkBox7.Location = new System.Drawing.Point(138, 91);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(84, 16);
            this.checkBox7.TabIndex = 46;
            this.checkBox7.Text = "驱动器故障";
            this.checkBox7.UseVisualStyleBackColor = true;
            // 
            // checkBox8
            // 
            this.checkBox8.AutoSize = true;
            this.checkBox8.Location = new System.Drawing.Point(138, 113);
            this.checkBox8.Name = "checkBox8";
            this.checkBox8.Size = new System.Drawing.Size(72, 16);
            this.checkBox8.TabIndex = 45;
            this.checkBox8.Text = "挂钩故障";
            this.checkBox8.UseVisualStyleBackColor = true;
            // 
            // checkBox9
            // 
            this.checkBox9.AutoSize = true;
            this.checkBox9.Location = new System.Drawing.Point(138, 133);
            this.checkBox9.Name = "checkBox9";
            this.checkBox9.Size = new System.Drawing.Size(114, 16);
            this.checkBox9.TabIndex = 44;
            this.checkBox9.Text = "寻迹传感器1离线";
            this.checkBox9.UseVisualStyleBackColor = true;
            // 
            // checkBox10
            // 
            this.checkBox10.AutoSize = true;
            this.checkBox10.Location = new System.Drawing.Point(138, 155);
            this.checkBox10.Name = "checkBox10";
            this.checkBox10.Size = new System.Drawing.Size(114, 16);
            this.checkBox10.TabIndex = 43;
            this.checkBox10.Text = "寻迹传感器2离线";
            this.checkBox10.UseVisualStyleBackColor = true;
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(8, 157);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(84, 16);
            this.checkBox5.TabIndex = 42;
            this.checkBox5.Text = "机械撞触发";
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(8, 135);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(72, 16);
            this.checkBox4.TabIndex = 41;
            this.checkBox4.Text = "轨道错误";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(8, 113);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(60, 16);
            this.checkBox3.TabIndex = 40;
            this.checkBox3.Text = "全轨道";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(8, 91);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(48, 16);
            this.checkBox2.TabIndex = 39;
            this.checkBox2.Text = "脱轨";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(8, 69);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(72, 16);
            this.checkBox1.TabIndex = 38;
            this.checkBox1.Text = "急停触发";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1248, 517);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.OrderTb);
            this.Controls.Add(this.Set);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.comboBox1);
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.Label PonintLb;
        private System.Windows.Forms.Label SpeedLb;
        private System.Windows.Forms.Label Direction;
        private System.Windows.Forms.Label Electric;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox16;
        private System.Windows.Forms.CheckBox checkBox17;
        private System.Windows.Forms.CheckBox checkBox18;
        private System.Windows.Forms.CheckBox checkBox19;
        private System.Windows.Forms.CheckBox checkBox20;
        private System.Windows.Forms.CheckBox checkBox11;
        private System.Windows.Forms.CheckBox checkBox12;
        private System.Windows.Forms.CheckBox checkBox13;
        private System.Windows.Forms.CheckBox checkBox14;
        private System.Windows.Forms.CheckBox checkBox15;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.CheckBox checkBox7;
        private System.Windows.Forms.CheckBox checkBox8;
        private System.Windows.Forms.CheckBox checkBox9;
        private System.Windows.Forms.CheckBox checkBox10;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox textBox2;
    }
}

