namespace DispatchAnmination.MapConfig
{
    partial class MapConfigForm
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
            this.MapConfigPB = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.LineDownBtn = new System.Windows.Forms.Button();
            this.LineUpBtn = new System.Windows.Forms.Button();
            this.SaveToMapFileBtn = new System.Windows.Forms.Button();
            this.LineSelectedLab = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.LineEndPyTB = new System.Windows.Forms.TextBox();
            this.LineEndPxTB = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.LineStartPyTB = new System.Windows.Forms.TextBox();
            this.LineStartPxTB = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.EditLineBtn = new System.Windows.Forms.Button();
            this.EditLineSiteBtn = new System.Windows.Forms.Button();
            this.DeleteLineBtn = new System.Windows.Forms.Button();
            this.DeleteSiteBtn = new System.Windows.Forms.Button();
            this.LineSiteListView = new System.Windows.Forms.ListView();
            this.LineListView = new System.Windows.Forms.ListView();
            this.SiteUpNameTB = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SiteNameTB = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SiteTypeCB = new System.Windows.Forms.ComboBox();
            this.SiteDirecationCB = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SiteRateTB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SiteIDTB = new System.Windows.Forms.TextBox();
            this.AddNewSiteBtn = new System.Windows.Forms.Button();
            this.AddNewLineBtn = new System.Windows.Forms.Button();
            this.MapTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.MapConfigPB)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MapConfigPB
            // 
            this.MapConfigPB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MapConfigPB.Location = new System.Drawing.Point(0, 0);
            this.MapConfigPB.Name = "MapConfigPB";
            this.MapConfigPB.Size = new System.Drawing.Size(965, 568);
            this.MapConfigPB.TabIndex = 0;
            this.MapConfigPB.TabStop = false;
            this.MapConfigPB.Paint += new System.Windows.Forms.PaintEventHandler(this.MapConfigPB_Paint);
            this.MapConfigPB.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MapConfigPB_MouseDown);
            this.MapConfigPB.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MapConfigPB_MouseMove);
            this.MapConfigPB.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MapConfigPB_MouseUp);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.LineDownBtn);
            this.panel1.Controls.Add(this.LineUpBtn);
            this.panel1.Controls.Add(this.SaveToMapFileBtn);
            this.panel1.Controls.Add(this.LineSelectedLab);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.LineEndPyTB);
            this.panel1.Controls.Add(this.LineEndPxTB);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.LineStartPyTB);
            this.panel1.Controls.Add(this.LineStartPxTB);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.EditLineBtn);
            this.panel1.Controls.Add(this.EditLineSiteBtn);
            this.panel1.Controls.Add(this.DeleteLineBtn);
            this.panel1.Controls.Add(this.DeleteSiteBtn);
            this.panel1.Controls.Add(this.LineSiteListView);
            this.panel1.Controls.Add(this.LineListView);
            this.panel1.Controls.Add(this.SiteUpNameTB);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.SiteNameTB);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.SiteTypeCB);
            this.panel1.Controls.Add(this.SiteDirecationCB);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.SiteRateTB);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.SiteIDTB);
            this.panel1.Controls.Add(this.AddNewSiteBtn);
            this.panel1.Controls.Add(this.AddNewLineBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 568);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(965, 257);
            this.panel1.TabIndex = 2;
            // 
            // LineDownBtn
            // 
            this.LineDownBtn.Location = new System.Drawing.Point(351, 6);
            this.LineDownBtn.Name = "LineDownBtn";
            this.LineDownBtn.Size = new System.Drawing.Size(38, 24);
            this.LineDownBtn.TabIndex = 35;
            this.LineDownBtn.Text = "Down";
            this.LineDownBtn.UseVisualStyleBackColor = true;
            this.LineDownBtn.Click += new System.EventHandler(this.LineDownBtn_Click);
            // 
            // LineUpBtn
            // 
            this.LineUpBtn.Location = new System.Drawing.Point(303, 7);
            this.LineUpBtn.Name = "LineUpBtn";
            this.LineUpBtn.Size = new System.Drawing.Size(42, 22);
            this.LineUpBtn.TabIndex = 34;
            this.LineUpBtn.Text = "Up";
            this.LineUpBtn.UseVisualStyleBackColor = true;
            this.LineUpBtn.Click += new System.EventHandler(this.LineUpBtn_Click);
            // 
            // SaveToMapFileBtn
            // 
            this.SaveToMapFileBtn.Location = new System.Drawing.Point(878, 217);
            this.SaveToMapFileBtn.Name = "SaveToMapFileBtn";
            this.SaveToMapFileBtn.Size = new System.Drawing.Size(75, 23);
            this.SaveToMapFileBtn.TabIndex = 33;
            this.SaveToMapFileBtn.Text = "保存配置";
            this.SaveToMapFileBtn.UseVisualStyleBackColor = true;
            this.SaveToMapFileBtn.Click += new System.EventHandler(this.SaveToMapFileBtn_Click);
            // 
            // LineSelectedLab
            // 
            this.LineSelectedLab.AutoSize = true;
            this.LineSelectedLab.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LineSelectedLab.Location = new System.Drawing.Point(491, 8);
            this.LineSelectedLab.Name = "LineSelectedLab";
            this.LineSelectedLab.Size = new System.Drawing.Size(17, 16);
            this.LineSelectedLab.TabIndex = 32;
            this.LineSelectedLab.Text = "*";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(395, 12);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(101, 12);
            this.label11.TabIndex = 31;
            this.label11.Text = "当前选择的线路：";
            // 
            // LineEndPyTB
            // 
            this.LineEndPyTB.Location = new System.Drawing.Point(337, 232);
            this.LineEndPyTB.Name = "LineEndPyTB";
            this.LineEndPyTB.Size = new System.Drawing.Size(100, 21);
            this.LineEndPyTB.TabIndex = 30;
            this.LineEndPyTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LineNumberInput_KeyPress);
            // 
            // LineEndPxTB
            // 
            this.LineEndPxTB.Location = new System.Drawing.Point(337, 198);
            this.LineEndPxTB.Name = "LineEndPxTB";
            this.LineEndPxTB.Size = new System.Drawing.Size(100, 21);
            this.LineEndPxTB.TabIndex = 29;
            this.LineEndPxTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LineNumberInput_KeyPress);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(314, 235);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 12);
            this.label10.TabIndex = 28;
            this.label10.Text = "EY";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(314, 203);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(17, 12);
            this.label9.TabIndex = 27;
            this.label9.Text = "EX";
            // 
            // LineStartPyTB
            // 
            this.LineStartPyTB.Location = new System.Drawing.Point(337, 166);
            this.LineStartPyTB.Name = "LineStartPyTB";
            this.LineStartPyTB.Size = new System.Drawing.Size(100, 21);
            this.LineStartPyTB.TabIndex = 26;
            this.LineStartPyTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LineNumberInput_KeyPress);
            // 
            // LineStartPxTB
            // 
            this.LineStartPxTB.Location = new System.Drawing.Point(337, 139);
            this.LineStartPxTB.Name = "LineStartPxTB";
            this.LineStartPxTB.Size = new System.Drawing.Size(100, 21);
            this.LineStartPxTB.TabIndex = 25;
            this.LineStartPxTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LineNumberInput_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(314, 169);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 12);
            this.label8.TabIndex = 24;
            this.label8.Text = "SY";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(314, 142);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 12);
            this.label7.TabIndex = 23;
            this.label7.Text = "SX";
            // 
            // EditLineBtn
            // 
            this.EditLineBtn.Location = new System.Drawing.Point(316, 72);
            this.EditLineBtn.Name = "EditLineBtn";
            this.EditLineBtn.Size = new System.Drawing.Size(75, 23);
            this.EditLineBtn.TabIndex = 22;
            this.EditLineBtn.Text = "修改线路";
            this.EditLineBtn.UseVisualStyleBackColor = true;
            this.EditLineBtn.Click += new System.EventHandler(this.EditLineBtn_Click);
            // 
            // EditLineSiteBtn
            // 
            this.EditLineSiteBtn.Location = new System.Drawing.Point(574, 35);
            this.EditLineSiteBtn.Name = "EditLineSiteBtn";
            this.EditLineSiteBtn.Size = new System.Drawing.Size(75, 23);
            this.EditLineSiteBtn.TabIndex = 21;
            this.EditLineSiteBtn.Text = "修改站点";
            this.EditLineSiteBtn.UseVisualStyleBackColor = true;
            this.EditLineSiteBtn.Click += new System.EventHandler(this.EditLineSiteBtn_Click);
            // 
            // DeleteLineBtn
            // 
            this.DeleteLineBtn.Location = new System.Drawing.Point(316, 104);
            this.DeleteLineBtn.Name = "DeleteLineBtn";
            this.DeleteLineBtn.Size = new System.Drawing.Size(75, 23);
            this.DeleteLineBtn.TabIndex = 20;
            this.DeleteLineBtn.Text = "删除线路";
            this.DeleteLineBtn.UseVisualStyleBackColor = true;
            this.DeleteLineBtn.Click += new System.EventHandler(this.DeleteLineBtn_Click);
            // 
            // DeleteSiteBtn
            // 
            this.DeleteSiteBtn.Location = new System.Drawing.Point(878, 6);
            this.DeleteSiteBtn.Name = "DeleteSiteBtn";
            this.DeleteSiteBtn.Size = new System.Drawing.Size(75, 23);
            this.DeleteSiteBtn.TabIndex = 19;
            this.DeleteSiteBtn.Text = "删除站点";
            this.DeleteSiteBtn.UseVisualStyleBackColor = true;
            // 
            // LineSiteListView
            // 
            this.LineSiteListView.FullRowSelect = true;
            this.LineSiteListView.GridLines = true;
            this.LineSiteListView.Location = new System.Drawing.Point(672, 6);
            this.LineSiteListView.Name = "LineSiteListView";
            this.LineSiteListView.Size = new System.Drawing.Size(200, 232);
            this.LineSiteListView.TabIndex = 18;
            this.LineSiteListView.UseCompatibleStateImageBehavior = false;
            this.LineSiteListView.View = System.Windows.Forms.View.Details;
            this.LineSiteListView.SelectedIndexChanged += new System.EventHandler(this.LineSiteListView_SelectedIndexChanged);
            // 
            // LineListView
            // 
            this.LineListView.FullRowSelect = true;
            this.LineListView.GridLines = true;
            this.LineListView.Location = new System.Drawing.Point(12, 6);
            this.LineListView.Name = "LineListView";
            this.LineListView.Size = new System.Drawing.Size(284, 241);
            this.LineListView.TabIndex = 17;
            this.LineListView.UseCompatibleStateImageBehavior = false;
            this.LineListView.View = System.Windows.Forms.View.Details;
            this.LineListView.SelectedIndexChanged += new System.EventHandler(this.LineListView_SelectedIndexChanged);
            // 
            // SiteUpNameTB
            // 
            this.SiteUpNameTB.Location = new System.Drawing.Point(528, 228);
            this.SiteUpNameTB.Name = "SiteUpNameTB";
            this.SiteUpNameTB.Size = new System.Drawing.Size(121, 21);
            this.SiteUpNameTB.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(479, 228);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 15;
            this.label6.Text = "upname";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(479, 200);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 14;
            this.label5.Text = "name";
            // 
            // SiteNameTB
            // 
            this.SiteNameTB.Location = new System.Drawing.Point(528, 192);
            this.SiteNameTB.Name = "SiteNameTB";
            this.SiteNameTB.Size = new System.Drawing.Size(121, 21);
            this.SiteNameTB.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(479, 165);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "type";
            // 
            // SiteTypeCB
            // 
            this.SiteTypeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SiteTypeCB.FormattingEnabled = true;
            this.SiteTypeCB.Items.AddRange(new object[] {
            "0 窑头窑尾",
            "1 等待点",
            "2 转弯点",
            "3 掉头点",
            "4 充电点",
            "5 交通管制点",
            "6 非交通管制点",
            "7 完成点",
            "8 加速点"});
            this.SiteTypeCB.Location = new System.Drawing.Point(528, 162);
            this.SiteTypeCB.Name = "SiteTypeCB";
            this.SiteTypeCB.Size = new System.Drawing.Size(121, 20);
            this.SiteTypeCB.TabIndex = 11;
            // 
            // SiteDirecationCB
            // 
            this.SiteDirecationCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SiteDirecationCB.FormattingEnabled = true;
            this.SiteDirecationCB.Items.AddRange(new object[] {
            "0",
            "1",
            "2"});
            this.SiteDirecationCB.Location = new System.Drawing.Point(528, 132);
            this.SiteDirecationCB.Name = "SiteDirecationCB";
            this.SiteDirecationCB.Size = new System.Drawing.Size(121, 20);
            this.SiteDirecationCB.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(479, 135);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "dire";
            // 
            // SiteRateTB
            // 
            this.SiteRateTB.Location = new System.Drawing.Point(528, 98);
            this.SiteRateTB.Name = "SiteRateTB";
            this.SiteRateTB.Size = new System.Drawing.Size(121, 21);
            this.SiteRateTB.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(477, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "rate";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(479, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "ID";
            // 
            // SiteIDTB
            // 
            this.SiteIDTB.Location = new System.Drawing.Point(528, 66);
            this.SiteIDTB.Name = "SiteIDTB";
            this.SiteIDTB.Size = new System.Drawing.Size(121, 21);
            this.SiteIDTB.TabIndex = 5;
            this.SiteIDTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LineNumberInput_KeyPress);
            // 
            // AddNewSiteBtn
            // 
            this.AddNewSiteBtn.Location = new System.Drawing.Point(481, 35);
            this.AddNewSiteBtn.Name = "AddNewSiteBtn";
            this.AddNewSiteBtn.Size = new System.Drawing.Size(75, 23);
            this.AddNewSiteBtn.TabIndex = 4;
            this.AddNewSiteBtn.Text = "添加站点";
            this.AddNewSiteBtn.UseVisualStyleBackColor = true;
            this.AddNewSiteBtn.Click += new System.EventHandler(this.AddNewSiteBtn_Click);
            // 
            // AddNewLineBtn
            // 
            this.AddNewLineBtn.Location = new System.Drawing.Point(316, 38);
            this.AddNewLineBtn.Name = "AddNewLineBtn";
            this.AddNewLineBtn.Size = new System.Drawing.Size(75, 23);
            this.AddNewLineBtn.TabIndex = 2;
            this.AddNewLineBtn.Text = "添加线路";
            this.AddNewLineBtn.UseVisualStyleBackColor = true;
            this.AddNewLineBtn.Click += new System.EventHandler(this.AddNewLineBtn_Click);
            // 
            // MapTimer
            // 
            this.MapTimer.Enabled = true;
            this.MapTimer.Interval = 500;
            this.MapTimer.Tick += new System.EventHandler(this.MapTimer_Tick);
            // 
            // MapConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(965, 825);
            this.Controls.Add(this.MapConfigPB);
            this.Controls.Add(this.panel1);
            this.Name = "MapConfigForm";
            this.Text = "地图配置";
            ((System.ComponentModel.ISupportInitialize)(this.MapConfigPB)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox MapConfigPB;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button AddNewLineBtn;
        private System.Windows.Forms.Timer MapTimer;
        private System.Windows.Forms.Button AddNewSiteBtn;
        private System.Windows.Forms.TextBox SiteIDTB;
        private System.Windows.Forms.TextBox SiteRateTB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox SiteNameTB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox SiteTypeCB;
        private System.Windows.Forms.ComboBox SiteDirecationCB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox SiteUpNameTB;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button DeleteSiteBtn;
        private System.Windows.Forms.ListView LineSiteListView;
        private System.Windows.Forms.ListView LineListView;
        private System.Windows.Forms.Button DeleteLineBtn;
        private System.Windows.Forms.Button EditLineSiteBtn;
        private System.Windows.Forms.Button EditLineBtn;
        private System.Windows.Forms.TextBox LineStartPyTB;
        private System.Windows.Forms.TextBox LineStartPxTB;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox LineEndPyTB;
        private System.Windows.Forms.TextBox LineEndPxTB;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label LineSelectedLab;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button SaveToMapFileBtn;
        private System.Windows.Forms.Button LineDownBtn;
        private System.Windows.Forms.Button LineUpBtn;
    }
}