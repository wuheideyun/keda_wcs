﻿using System;

namespace KEDAClient
{
    partial class KEDAForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KEDAForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabelConnect = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabelTime = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabelLogin = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabelVersion = new System.Windows.Forms.ToolStripLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.登录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.注销登录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.用户登录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startServer = new System.Windows.Forms.ToolStripMenuItem();
            this.stopServer = new System.Windows.Forms.ToolStripMenuItem();
            this.labelLogo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.missions = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.initButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBoxOutput = new System.Windows.Forms.ListBox();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.dispatch = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dispatchlist = new System.Windows.Forms.ListView();
            this.currentTask = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.taskInformlist = new System.Windows.Forms.ListView();
            this.taskList = new System.Windows.Forms.TabPage();
            this.executeTask = new System.Windows.Forms.Button();
            this.executeTasklist = new System.Windows.Forms.ListView();
            this.map = new System.Windows.Forms.TabPage();
            this.pausemission = new System.Windows.Forms.Button();
            this.endmission = new System.Windows.Forms.Button();
            this.startmission = new System.Windows.Forms.Button();
            this.alarms = new System.Windows.Forms.TabPage();
            this.alarmlist = new System.Windows.Forms.ListView();
            this.logger = new System.Windows.Forms.TabPage();
            this.loggerlist = new System.Windows.Forms.ListView();
            this.listView2 = new System.Windows.Forms.ListView();
            this.devices = new System.Windows.Forms.TabPage();
            this.otherdevlist = new System.Windows.Forms.ListView();
            this.vehicles = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.defineDispatch = new System.Windows.Forms.Button();
            this.agvStop = new System.Windows.Forms.Button();
            this.agvBackMove = new System.Windows.Forms.Button();
            this.agvForwordMove = new System.Windows.Forms.Button();
            this.buttonSend = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.AGVdev = new System.Windows.Forms.TabPage();
            this.vehicleslist = new System.Windows.Forms.ListView();
            this.timerFunc = new System.Windows.Forms.Timer(this.components);
            this.Statetimer = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.missions.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.dispatch.SuspendLayout();
            this.panel4.SuspendLayout();
            this.currentTask.SuspendLayout();
            this.panel3.SuspendLayout();
            this.taskList.SuspendLayout();
            this.alarms.SuspendLayout();
            this.logger.SuspendLayout();
            this.devices.SuspendLayout();
            this.vehicles.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.AGVdev.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelConnect,
            this.toolStripSeparator1,
            this.toolStripLabelTime,
            this.toolStripSeparator2,
            this.toolStripLabelLogin,
            this.toolStripSeparator3,
            this.toolStripLabelVersion});
            this.toolStrip1.Location = new System.Drawing.Point(0, 602);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1011, 25);
            this.toolStrip1.TabIndex = 16;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabelConnect
            // 
            this.toolStripLabelConnect.Name = "toolStripLabelConnect";
            this.toolStripLabelConnect.Size = new System.Drawing.Size(56, 22);
            this.toolStripLabelConnect.Text = "连接状态";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabelTime
            // 
            this.toolStripLabelTime.Name = "toolStripLabelTime";
            this.toolStripLabelTime.Size = new System.Drawing.Size(56, 22);
            this.toolStripLabelTime.Text = "系统时间";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabelLogin
            // 
            this.toolStripLabelLogin.Name = "toolStripLabelLogin";
            this.toolStripLabelLogin.Size = new System.Drawing.Size(56, 22);
            this.toolStripLabelLogin.Text = "用户状态";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabelVersion
            // 
            this.toolStripLabelVersion.Name = "toolStripLabelVersion";
            this.toolStripLabelVersion.Size = new System.Drawing.Size(44, 22);
            this.toolStripLabelVersion.Text = "版本号";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem,
            this.登录ToolStripMenuItem,
            this.startServer,
            this.stopServer});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1011, 25);
            this.menuStrip1.TabIndex = 17;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ToolStripMenuItem
            // 
            this.ToolStripMenuItem.Name = "ToolStripMenuItem";
            this.ToolStripMenuItem.Size = new System.Drawing.Size(60, 21);
            this.ToolStripMenuItem.Text = "刷新(&R)";
            this.ToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // 登录ToolStripMenuItem
            // 
            this.登录ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.注销登录ToolStripMenuItem,
            this.用户登录ToolStripMenuItem});
            this.登录ToolStripMenuItem.Name = "登录ToolStripMenuItem";
            this.登录ToolStripMenuItem.Size = new System.Drawing.Size(61, 21);
            this.登录ToolStripMenuItem.Text = "用户(&U)";
            // 
            // 注销登录ToolStripMenuItem
            // 
            this.注销登录ToolStripMenuItem.Name = "注销登录ToolStripMenuItem";
            this.注销登录ToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.注销登录ToolStripMenuItem.Text = "注销登录(&C)";
            this.注销登录ToolStripMenuItem.Click += new System.EventHandler(this.注销登录ToolStripMenuItem_Click_1);
            // 
            // 用户登录ToolStripMenuItem
            // 
            this.用户登录ToolStripMenuItem.Name = "用户登录ToolStripMenuItem";
            this.用户登录ToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.用户登录ToolStripMenuItem.Text = "用户登录(&L)";
            this.用户登录ToolStripMenuItem.Click += new System.EventHandler(this.用户登录ToolStripMenuItem_Click_1);
            // 
            // startServer
            // 
            this.startServer.Name = "startServer";
            this.startServer.Size = new System.Drawing.Size(68, 21);
            this.startServer.Text = "启动服务";
            this.startServer.Click += new System.EventHandler(this.startServer_Click);
            // 
            // stopServer
            // 
            this.stopServer.Enabled = false;
            this.stopServer.Name = "stopServer";
            this.stopServer.Size = new System.Drawing.Size(68, 21);
            this.stopServer.Text = "停止服务";
            this.stopServer.Click += new System.EventHandler(this.stopServer_Click);
            // 
            // labelLogo
            // 
            this.labelLogo.AutoSize = true;
            this.labelLogo.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelLogo.Location = new System.Drawing.Point(316, 13);
            this.labelLogo.Name = "labelLogo";
            this.labelLogo.Size = new System.Drawing.Size(401, 37);
            this.labelLogo.TabIndex = 0;
            this.labelLogo.Text = "广东科达洁能股份有限公司";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.panel1.Controls.Add(this.labelLogo);
            this.panel1.Location = new System.Drawing.Point(0, 37);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1011, 70);
            this.panel1.TabIndex = 18;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.missions);
            this.tabControl1.Controls.Add(this.alarms);
            this.tabControl1.Controls.Add(this.logger);
            this.tabControl1.Controls.Add(this.devices);
            this.tabControl1.Controls.Add(this.vehicles);
            this.tabControl1.Font = new System.Drawing.Font("宋体", 16F);
            this.tabControl1.Location = new System.Drawing.Point(0, 126);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1011, 473);
            this.tabControl1.TabIndex = 19;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // missions
            // 
            this.missions.Controls.Add(this.panel2);
            this.missions.Location = new System.Drawing.Point(4, 31);
            this.missions.Name = "missions";
            this.missions.Padding = new System.Windows.Forms.Padding(3);
            this.missions.Size = new System.Drawing.Size(1003, 438);
            this.missions.TabIndex = 0;
            this.missions.Text = "任务";
            this.missions.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel2.Controls.Add(this.initButton);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.tabControl2);
            this.panel2.Controls.Add(this.pausemission);
            this.panel2.Controls.Add(this.endmission);
            this.panel2.Controls.Add(this.startmission);
            this.panel2.Font = new System.Drawing.Font("宋体", 12F);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1003, 438);
            this.panel2.TabIndex = 0;
            // 
            // initButton
            // 
            this.initButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.initButton.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.initButton.Location = new System.Drawing.Point(844, 358);
            this.initButton.Name = "initButton";
            this.initButton.Size = new System.Drawing.Size(74, 51);
            this.initButton.TabIndex = 38;
            this.initButton.Text = "初始化";
            this.initButton.UseVisualStyleBackColor = false;
            this.initButton.Click += new System.EventHandler(this.initButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.listBoxOutput);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 12F);
            this.groupBox1.Location = new System.Drawing.Point(8, 261);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(666, 171);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "系统信息";
            // 
            // listBoxOutput
            // 
            this.listBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxOutput.Font = new System.Drawing.Font("宋体", 10F);
            this.listBoxOutput.FormattingEnabled = true;
            this.listBoxOutput.HorizontalScrollbar = true;
            this.listBoxOutput.Location = new System.Drawing.Point(14, 25);
            this.listBoxOutput.Name = "listBoxOutput";
            this.listBoxOutput.Size = new System.Drawing.Size(646, 134);
            this.listBoxOutput.TabIndex = 9;
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.dispatch);
            this.tabControl2.Controls.Add(this.currentTask);
            this.tabControl2.Controls.Add(this.taskList);
            this.tabControl2.Controls.Add(this.map);
            this.tabControl2.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabControl2.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl2.Location = new System.Drawing.Point(20, 19);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(965, 224);
            this.tabControl2.TabIndex = 27;
            // 
            // dispatch
            // 
            this.dispatch.Controls.Add(this.panel4);
            this.dispatch.Location = new System.Drawing.Point(4, 31);
            this.dispatch.Name = "dispatch";
            this.dispatch.Size = new System.Drawing.Size(957, 189);
            this.dispatch.TabIndex = 4;
            this.dispatch.Text = "调度任务";
            this.dispatch.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.Controls.Add(this.dispatchlist);
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(951, 183);
            this.panel4.TabIndex = 1;
            // 
            // dispatchlist
            // 
            this.dispatchlist.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dispatchlist.FullRowSelect = true;
            this.dispatchlist.GridLines = true;
            this.dispatchlist.Location = new System.Drawing.Point(3, 3);
            this.dispatchlist.Name = "dispatchlist";
            this.dispatchlist.Size = new System.Drawing.Size(945, 180);
            this.dispatchlist.TabIndex = 0;
            this.dispatchlist.UseCompatibleStateImageBehavior = false;
            this.dispatchlist.SelectedIndexChanged += new System.EventHandler(this.dispatchlist_SelectedIndexChanged);
            // 
            // currentTask
            // 
            this.currentTask.Controls.Add(this.panel3);
            this.currentTask.Location = new System.Drawing.Point(4, 31);
            this.currentTask.Name = "currentTask";
            this.currentTask.Padding = new System.Windows.Forms.Padding(3);
            this.currentTask.Size = new System.Drawing.Size(957, 189);
            this.currentTask.TabIndex = 2;
            this.currentTask.Text = "当前任务";
            this.currentTask.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.AutoScroll = true;
            this.panel3.AutoSize = true;
            this.panel3.Controls.Add(this.taskInformlist);
            this.panel3.Location = new System.Drawing.Point(6, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(948, 180);
            this.panel3.TabIndex = 38;
            // 
            // taskInformlist
            // 
            this.taskInformlist.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.taskInformlist.FullRowSelect = true;
            this.taskInformlist.GridLines = true;
            this.taskInformlist.Location = new System.Drawing.Point(3, 3);
            this.taskInformlist.Name = "taskInformlist";
            this.taskInformlist.Size = new System.Drawing.Size(942, 177);
            this.taskInformlist.TabIndex = 0;
            this.taskInformlist.UseCompatibleStateImageBehavior = false;
            this.taskInformlist.View = System.Windows.Forms.View.Details;
            this.taskInformlist.SelectedIndexChanged += new System.EventHandler(this.taskInformlist_SelectedIndexChanged);
            // 
            // taskList
            // 
            this.taskList.Controls.Add(this.executeTask);
            this.taskList.Controls.Add(this.executeTasklist);
            this.taskList.Location = new System.Drawing.Point(4, 31);
            this.taskList.Name = "taskList";
            this.taskList.Size = new System.Drawing.Size(957, 189);
            this.taskList.TabIndex = 3;
            this.taskList.Text = "任务列表";
            this.taskList.UseVisualStyleBackColor = true;
            // 
            // executeTask
            // 
            this.executeTask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.executeTask.Location = new System.Drawing.Point(808, 71);
            this.executeTask.Name = "executeTask";
            this.executeTask.Size = new System.Drawing.Size(106, 37);
            this.executeTask.TabIndex = 1;
            this.executeTask.Text = "执行任务";
            this.executeTask.UseVisualStyleBackColor = true;
            this.executeTask.Click += new System.EventHandler(this.executeTask_Click);
            // 
            // executeTasklist
            // 
            this.executeTasklist.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.executeTasklist.FullRowSelect = true;
            this.executeTasklist.GridLines = true;
            this.executeTasklist.Location = new System.Drawing.Point(3, 3);
            this.executeTasklist.Name = "executeTasklist";
            this.executeTasklist.Size = new System.Drawing.Size(753, 183);
            this.executeTasklist.TabIndex = 0;
            this.executeTasklist.UseCompatibleStateImageBehavior = false;
            // 
            // map
            // 
            this.map.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.map.Location = new System.Drawing.Point(4, 31);
            this.map.Name = "map";
            this.map.Padding = new System.Windows.Forms.Padding(3);
            this.map.Size = new System.Drawing.Size(957, 189);
            this.map.TabIndex = 0;
            this.map.Text = "地图展示";
            this.map.UseVisualStyleBackColor = true;
            // 
            // pausemission
            // 
            this.pausemission.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pausemission.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pausemission.Location = new System.Drawing.Point(721, 360);
            this.pausemission.Name = "pausemission";
            this.pausemission.Size = new System.Drawing.Size(74, 51);
            this.pausemission.TabIndex = 35;
            this.pausemission.Text = "暂停";
            this.pausemission.UseVisualStyleBackColor = false;
            this.pausemission.Click += new System.EventHandler(this.pausemission_Click);
            // 
            // endmission
            // 
            this.endmission.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.endmission.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.endmission.Location = new System.Drawing.Point(844, 286);
            this.endmission.Name = "endmission";
            this.endmission.Size = new System.Drawing.Size(74, 51);
            this.endmission.TabIndex = 37;
            this.endmission.Text = "结束";
            this.endmission.UseVisualStyleBackColor = false;
            this.endmission.Click += new System.EventHandler(this.endmission_Click);
            // 
            // startmission
            // 
            this.startmission.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startmission.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.startmission.Location = new System.Drawing.Point(721, 286);
            this.startmission.Name = "startmission";
            this.startmission.Size = new System.Drawing.Size(74, 51);
            this.startmission.TabIndex = 34;
            this.startmission.Text = "开始";
            this.startmission.UseVisualStyleBackColor = false;
            this.startmission.Click += new System.EventHandler(this.startmission_Click);
            // 
            // alarms
            // 
            this.alarms.Controls.Add(this.alarmlist);
            this.alarms.Location = new System.Drawing.Point(4, 31);
            this.alarms.Name = "alarms";
            this.alarms.Padding = new System.Windows.Forms.Padding(3);
            this.alarms.Size = new System.Drawing.Size(1003, 438);
            this.alarms.TabIndex = 1;
            this.alarms.Text = "报警";
            this.alarms.UseVisualStyleBackColor = true;
            // 
            // alarmlist
            // 
            this.alarmlist.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.alarmlist.FullRowSelect = true;
            this.alarmlist.GridLines = true;
            this.alarmlist.Location = new System.Drawing.Point(3, 3);
            this.alarmlist.Name = "alarmlist";
            this.alarmlist.Size = new System.Drawing.Size(1004, 435);
            this.alarmlist.TabIndex = 0;
            this.alarmlist.UseCompatibleStateImageBehavior = false;
            // 
            // logger
            // 
            this.logger.Controls.Add(this.loggerlist);
            this.logger.Controls.Add(this.listView2);
            this.logger.Location = new System.Drawing.Point(4, 31);
            this.logger.Name = "logger";
            this.logger.Size = new System.Drawing.Size(1003, 438);
            this.logger.TabIndex = 2;
            this.logger.Text = "日志";
            this.logger.UseVisualStyleBackColor = true;
            // 
            // loggerlist
            // 
            this.loggerlist.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.loggerlist.FullRowSelect = true;
            this.loggerlist.GridLines = true;
            this.loggerlist.Location = new System.Drawing.Point(-4, 0);
            this.loggerlist.Name = "loggerlist";
            this.loggerlist.Size = new System.Drawing.Size(1011, 438);
            this.loggerlist.TabIndex = 1;
            this.loggerlist.UseCompatibleStateImageBehavior = false;
            // 
            // listView2
            // 
            this.listView2.Location = new System.Drawing.Point(-4, 0);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(835, 440);
            this.listView2.TabIndex = 0;
            this.listView2.UseCompatibleStateImageBehavior = false;
            // 
            // devices
            // 
            this.devices.Controls.Add(this.otherdevlist);
            this.devices.Location = new System.Drawing.Point(4, 31);
            this.devices.Name = "devices";
            this.devices.Size = new System.Drawing.Size(1003, 438);
            this.devices.TabIndex = 3;
            this.devices.Text = "设备";
            this.devices.UseVisualStyleBackColor = true;
            // 
            // otherdevlist
            // 
            this.otherdevlist.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otherdevlist.FullRowSelect = true;
            this.otherdevlist.GridLines = true;
            this.otherdevlist.Location = new System.Drawing.Point(-4, 0);
            this.otherdevlist.Name = "otherdevlist";
            this.otherdevlist.Size = new System.Drawing.Size(1011, 442);
            this.otherdevlist.TabIndex = 0;
            this.otherdevlist.UseCompatibleStateImageBehavior = false;
            // 
            // vehicles
            // 
            this.vehicles.Controls.Add(this.groupBox2);
            this.vehicles.Controls.Add(this.defineDispatch);
            this.vehicles.Controls.Add(this.agvStop);
            this.vehicles.Controls.Add(this.agvBackMove);
            this.vehicles.Controls.Add(this.agvForwordMove);
            this.vehicles.Controls.Add(this.buttonSend);
            this.vehicles.Controls.Add(this.textBox1);
            this.vehicles.Controls.Add(this.label4);
            this.vehicles.Controls.Add(this.label1);
            this.vehicles.Controls.Add(this.comboBox1);
            this.vehicles.Controls.Add(this.tabControl3);
            this.vehicles.Font = new System.Drawing.Font("宋体", 12F);
            this.vehicles.Location = new System.Drawing.Point(4, 31);
            this.vehicles.Name = "vehicles";
            this.vehicles.Size = new System.Drawing.Size(1003, 438);
            this.vehicles.TabIndex = 4;
            this.vehicles.Text = "车辆";
            this.vehicles.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox2.Controls.Add(this.listBox1);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 12F);
            this.groupBox2.Location = new System.Drawing.Point(713, 304);
            this.groupBox2.MaximumSize = new System.Drawing.Size(500, 400);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(287, 127);
            this.groupBox2.TabIndex = 44;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "系统提示";
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.Font = new System.Drawing.Font("宋体", 9F);
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(6, 25);
            this.listBox1.MaximumSize = new System.Drawing.Size(500, 400);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(275, 88);
            this.listBox1.TabIndex = 0;
            // 
            // defineDispatch
            // 
            this.defineDispatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.defineDispatch.Enabled = false;
            this.defineDispatch.Location = new System.Drawing.Point(853, 219);
            this.defineDispatch.Name = "defineDispatch";
            this.defineDispatch.Size = new System.Drawing.Size(106, 36);
            this.defineDispatch.TabIndex = 43;
            this.defineDispatch.Text = "自定义任务";
            this.defineDispatch.UseVisualStyleBackColor = true;
            this.defineDispatch.Click += new System.EventHandler(this.defineDispatch_Click);
            // 
            // agvStop
            // 
            this.agvStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.agvStop.Enabled = false;
            this.agvStop.Location = new System.Drawing.Point(722, 219);
            this.agvStop.Name = "agvStop";
            this.agvStop.Size = new System.Drawing.Size(104, 36);
            this.agvStop.TabIndex = 42;
            this.agvStop.Text = "停止";
            this.agvStop.UseVisualStyleBackColor = true;
            this.agvStop.Click += new System.EventHandler(this.agvStop_Click);
            // 
            // agvBackMove
            // 
            this.agvBackMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.agvBackMove.Enabled = false;
            this.agvBackMove.Location = new System.Drawing.Point(853, 157);
            this.agvBackMove.Name = "agvBackMove";
            this.agvBackMove.Size = new System.Drawing.Size(106, 36);
            this.agvBackMove.TabIndex = 41;
            this.agvBackMove.Text = "后退";
            this.agvBackMove.UseVisualStyleBackColor = true;
            this.agvBackMove.Click += new System.EventHandler(this.agvBackMove_Click);
            // 
            // agvForwordMove
            // 
            this.agvForwordMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.agvForwordMove.Enabled = false;
            this.agvForwordMove.Location = new System.Drawing.Point(722, 157);
            this.agvForwordMove.Name = "agvForwordMove";
            this.agvForwordMove.Size = new System.Drawing.Size(106, 36);
            this.agvForwordMove.TabIndex = 40;
            this.agvForwordMove.Text = "前进";
            this.agvForwordMove.UseVisualStyleBackColor = true;
            this.agvForwordMove.Click += new System.EventHandler(this.agvForwordMove_Click);
            // 
            // buttonSend
            // 
            this.buttonSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSend.Enabled = false;
            this.buttonSend.Font = new System.Drawing.Font("宋体", 10F);
            this.buttonSend.Location = new System.Drawing.Point(904, 113);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(75, 23);
            this.buttonSend.TabIndex = 39;
            this.buttonSend.Text = "发送";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(719, 110);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(160, 26);
            this.textBox1.TabIndex = 38;
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10F);
            this.label4.Location = new System.Drawing.Point(719, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 14);
            this.label4.TabIndex = 37;
            this.label4.Text = "指令参数：";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10F);
            this.label1.Location = new System.Drawing.Point(719, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 14);
            this.label1.TabIndex = 36;
            this.label1.Text = "指令类型：";
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.BackColor = System.Drawing.SystemColors.Menu;
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "发送站点",
            "速度",
            "清除站点",
            "开启声音",
            "关闭声音",
            "心跳指令",
            "自定义一个任务"});
            this.comboBox1.Location = new System.Drawing.Point(719, 55);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(160, 24);
            this.comboBox1.TabIndex = 35;
            // 
            // tabControl3
            // 
            this.tabControl3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl3.Controls.Add(this.AGVdev);
            this.tabControl3.Location = new System.Drawing.Point(3, 3);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(677, 432);
            this.tabControl3.TabIndex = 32;
            // 
            // AGVdev
            // 
            this.AGVdev.Controls.Add(this.vehicleslist);
            this.AGVdev.Location = new System.Drawing.Point(4, 26);
            this.AGVdev.Name = "AGVdev";
            this.AGVdev.Padding = new System.Windows.Forms.Padding(3);
            this.AGVdev.Size = new System.Drawing.Size(669, 402);
            this.AGVdev.TabIndex = 0;
            this.AGVdev.Text = "AGV设备";
            this.AGVdev.UseVisualStyleBackColor = true;
            // 
            // vehicleslist
            // 
            this.vehicleslist.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vehicleslist.Font = new System.Drawing.Font("宋体", 16F);
            this.vehicleslist.FullRowSelect = true;
            this.vehicleslist.GridLines = true;
            this.vehicleslist.Location = new System.Drawing.Point(-4, 3);
            this.vehicleslist.Name = "vehicleslist";
            this.vehicleslist.Size = new System.Drawing.Size(673, 399);
            this.vehicleslist.TabIndex = 0;
            this.vehicleslist.UseCompatibleStateImageBehavior = false;
            this.vehicleslist.SelectedIndexChanged += new System.EventHandler(this.vehicleslist_SelectedIndexChanged);
            // 
            // timerFunc
            // 
            this.timerFunc.Tick += new System.EventHandler(this.timerFunc_Tick);
            // 
            // Statetimer
            // 
            this.Statetimer.Enabled = true;
            this.Statetimer.Interval = 1000;
            this.Statetimer.Tick += new System.EventHandler(this.Statetimer_Tick);
            // 
            // KEDAForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1011, 627);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "KEDAForm";
            this.Text = "客户端信息表";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.KEDAForm_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.missions.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.dispatch.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.currentTask.ResumeLayout(false);
            this.currentTask.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.taskList.ResumeLayout(false);
            this.alarms.ResumeLayout(false);
            this.logger.ResumeLayout(false);
            this.devices.ResumeLayout(false);
            this.vehicles.ResumeLayout(false);
            this.vehicles.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.AGVdev.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabelConnect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabelTime;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabelLogin;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel toolStripLabelVersion;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 登录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 注销登录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 用户登录ToolStripMenuItem;
        private System.Windows.Forms.Label labelLogo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage missions;
        private System.Windows.Forms.TabPage alarms;
        private System.Windows.Forms.TabPage logger;
        private System.Windows.Forms.TabPage devices;
        private System.Windows.Forms.TabPage vehicles;
        private System.Windows.Forms.Timer timerFunc;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listBoxOutput;
        private System.Windows.Forms.ListView alarmlist;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ListView loggerlist;
        private System.Windows.Forms.ListView otherdevlist;
        private System.Windows.Forms.ListView vehicleslist;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage map;
        private System.Windows.Forms.TabPage currentTask;
        private System.Windows.Forms.ListView taskInformlist;
        private System.Windows.Forms.Button endmission;
        private System.Windows.Forms.Button pausemission;
        private System.Windows.Forms.Button startmission;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage AGVdev;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Button defineDispatch;
        private System.Windows.Forms.Button agvStop;
        private System.Windows.Forms.Button agvBackMove;
        private System.Windows.Forms.Button agvForwordMove;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Timer Statetimer;
        private System.Windows.Forms.TabPage taskList;
        private System.Windows.Forms.ListView executeTasklist;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ToolStripMenuItem startServer;
        private System.Windows.Forms.ToolStripMenuItem stopServer;
        private System.Windows.Forms.Button initButton;
        private System.Windows.Forms.TabPage dispatch;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ListView dispatchlist;
        private System.Windows.Forms.Button executeTask;
    }
}