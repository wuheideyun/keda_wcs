namespace DispatchAnmination
{
    partial class LineInfoForm
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
            this.AgvLineListView = new System.Windows.Forms.ListView();
            this.LinePointListView = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.LineNowSiteTB = new System.Windows.Forms.TextBox();
            this.LineDesSiteCB = new System.Windows.Forms.ComboBox();
            this.PointUpBtn = new System.Windows.Forms.Button();
            this.PontDownBtn = new System.Windows.Forms.Button();
            this.PointXTB = new System.Windows.Forms.TextBox();
            this.PointYTB = new System.Windows.Forms.TextBox();
            this.AddPointBtn = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.EditeLineBtn = new System.Windows.Forms.Button();
            this.AddLineBtn = new System.Windows.Forms.Button();
            this.UseXmlAnBtn = new System.Windows.Forms.Button();
            this.SaveAgvLineBtn = new System.Windows.Forms.Button();
            this.DeletePointBtn = new System.Windows.Forms.Button();
            this.DeleteLineBtn = new System.Windows.Forms.Button();
            this.LineSpecialCB = new System.Windows.Forms.CheckBox();
            this.EditPointBtn = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.LineMoveSizeTB = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.LineUpBtn = new System.Windows.Forms.Button();
            this.LineDownBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AgvLineListView
            // 
            this.AgvLineListView.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AgvLineListView.FullRowSelect = true;
            this.AgvLineListView.GridLines = true;
            this.AgvLineListView.Location = new System.Drawing.Point(0, 34);
            this.AgvLineListView.Name = "AgvLineListView";
            this.AgvLineListView.Size = new System.Drawing.Size(240, 652);
            this.AgvLineListView.TabIndex = 1;
            this.AgvLineListView.UseCompatibleStateImageBehavior = false;
            this.AgvLineListView.View = System.Windows.Forms.View.Details;
            this.AgvLineListView.SelectedIndexChanged += new System.EventHandler(this.AgvLineListView_SelectedIndexChanged);
            // 
            // LinePointListView
            // 
            this.LinePointListView.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LinePointListView.FullRowSelect = true;
            this.LinePointListView.GridLines = true;
            this.LinePointListView.Location = new System.Drawing.Point(603, 34);
            this.LinePointListView.Name = "LinePointListView";
            this.LinePointListView.Size = new System.Drawing.Size(100, 652);
            this.LinePointListView.TabIndex = 2;
            this.LinePointListView.UseCompatibleStateImageBehavior = false;
            this.LinePointListView.View = System.Windows.Forms.View.Details;
            this.LinePointListView.SelectedIndexChanged += new System.EventHandler(this.LinePointListView_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(448, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "线路路标";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "Agv地图行走信息";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(274, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "开始地标:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(276, 139);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "特殊节点：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(276, 192);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 7;
            this.label5.Text = "目标节点:";
            // 
            // LineNowSiteTB
            // 
            this.LineNowSiteTB.Location = new System.Drawing.Point(345, 84);
            this.LineNowSiteTB.Name = "LineNowSiteTB";
            this.LineNowSiteTB.Size = new System.Drawing.Size(122, 21);
            this.LineNowSiteTB.TabIndex = 8;
            // 
            // LineDesSiteCB
            // 
            this.LineDesSiteCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LineDesSiteCB.FormattingEnabled = true;
            this.LineDesSiteCB.Items.AddRange(new object[] {
            "1号站点(11地标)",
            "4号站点(14地标)",
            "12完成点",
            "15完成点",
            "33等待点",
            "36等待点",
            "52充电点",
            "53充电点",
            "55充电点",
            "56充电点"});
            this.LineDesSiteCB.Location = new System.Drawing.Point(345, 192);
            this.LineDesSiteCB.Name = "LineDesSiteCB";
            this.LineDesSiteCB.Size = new System.Drawing.Size(122, 20);
            this.LineDesSiteCB.TabIndex = 11;
            // 
            // PointUpBtn
            // 
            this.PointUpBtn.Location = new System.Drawing.Point(711, 87);
            this.PointUpBtn.Name = "PointUpBtn";
            this.PointUpBtn.Size = new System.Drawing.Size(42, 42);
            this.PointUpBtn.TabIndex = 12;
            this.PointUpBtn.Text = "Up";
            this.PointUpBtn.UseVisualStyleBackColor = true;
            this.PointUpBtn.Click += new System.EventHandler(this.PointUpBtn_Click);
            // 
            // PontDownBtn
            // 
            this.PontDownBtn.Location = new System.Drawing.Point(711, 139);
            this.PontDownBtn.Name = "PontDownBtn";
            this.PontDownBtn.Size = new System.Drawing.Size(42, 42);
            this.PontDownBtn.TabIndex = 13;
            this.PontDownBtn.Text = "Down";
            this.PontDownBtn.UseVisualStyleBackColor = true;
            this.PontDownBtn.Click += new System.EventHandler(this.PontDownBtn_Click);
            // 
            // PointXTB
            // 
            this.PointXTB.Location = new System.Drawing.Point(741, 291);
            this.PointXTB.Name = "PointXTB";
            this.PointXTB.Size = new System.Drawing.Size(115, 21);
            this.PointXTB.TabIndex = 15;
            this.PointXTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PointXTB_KeyPress);
            // 
            // PointYTB
            // 
            this.PointYTB.Location = new System.Drawing.Point(741, 336);
            this.PointYTB.Name = "PointYTB";
            this.PointYTB.Size = new System.Drawing.Size(115, 21);
            this.PointYTB.TabIndex = 16;
            this.PointYTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PointYTB_KeyPress);
            // 
            // AddPointBtn
            // 
            this.AddPointBtn.Location = new System.Drawing.Point(711, 389);
            this.AddPointBtn.Name = "AddPointBtn";
            this.AddPointBtn.Size = new System.Drawing.Size(75, 23);
            this.AddPointBtn.TabIndex = 17;
            this.AddPointBtn.Text = "添加点";
            this.AddPointBtn.UseVisualStyleBackColor = true;
            this.AddPointBtn.Click += new System.EventHandler(this.AddPointBtn_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(709, 300);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 18;
            this.label6.Text = "X:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(709, 339);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 12);
            this.label7.TabIndex = 19;
            this.label7.Text = "Y:";
            // 
            // EditeLineBtn
            // 
            this.EditeLineBtn.Location = new System.Drawing.Point(392, 277);
            this.EditeLineBtn.Name = "EditeLineBtn";
            this.EditeLineBtn.Size = new System.Drawing.Size(75, 23);
            this.EditeLineBtn.TabIndex = 20;
            this.EditeLineBtn.Text = "修改路线";
            this.EditeLineBtn.UseVisualStyleBackColor = true;
            this.EditeLineBtn.Click += new System.EventHandler(this.EditeLineBtn_Click);
            // 
            // AddLineBtn
            // 
            this.AddLineBtn.Location = new System.Drawing.Point(275, 277);
            this.AddLineBtn.Name = "AddLineBtn";
            this.AddLineBtn.Size = new System.Drawing.Size(75, 23);
            this.AddLineBtn.TabIndex = 21;
            this.AddLineBtn.Text = "添加路线";
            this.AddLineBtn.UseVisualStyleBackColor = true;
            this.AddLineBtn.Click += new System.EventHandler(this.AddLineBtn_Click);
            // 
            // UseXmlAnBtn
            // 
            this.UseXmlAnBtn.Location = new System.Drawing.Point(258, 628);
            this.UseXmlAnBtn.Name = "UseXmlAnBtn";
            this.UseXmlAnBtn.Size = new System.Drawing.Size(92, 23);
            this.UseXmlAnBtn.TabIndex = 22;
            this.UseXmlAnBtn.Text = "使用内存解析";
            this.UseXmlAnBtn.UseVisualStyleBackColor = true;
            this.UseXmlAnBtn.Click += new System.EventHandler(this.UseXmlAnBtn_Click);
            // 
            // SaveAgvLineBtn
            // 
            this.SaveAgvLineBtn.Location = new System.Drawing.Point(392, 628);
            this.SaveAgvLineBtn.Name = "SaveAgvLineBtn";
            this.SaveAgvLineBtn.Size = new System.Drawing.Size(75, 23);
            this.SaveAgvLineBtn.TabIndex = 23;
            this.SaveAgvLineBtn.Text = "保存到配置文件";
            this.SaveAgvLineBtn.UseVisualStyleBackColor = true;
            this.SaveAgvLineBtn.Click += new System.EventHandler(this.SaveAgvLineBtn_Click);
            // 
            // DeletePointBtn
            // 
            this.DeletePointBtn.Location = new System.Drawing.Point(711, 217);
            this.DeletePointBtn.Name = "DeletePointBtn";
            this.DeletePointBtn.Size = new System.Drawing.Size(75, 23);
            this.DeletePointBtn.TabIndex = 24;
            this.DeletePointBtn.Text = "删除点";
            this.DeletePointBtn.UseVisualStyleBackColor = true;
            this.DeletePointBtn.Click += new System.EventHandler(this.DeletePointBtn_Click);
            // 
            // DeleteLineBtn
            // 
            this.DeleteLineBtn.Location = new System.Drawing.Point(258, 336);
            this.DeleteLineBtn.Name = "DeleteLineBtn";
            this.DeleteLineBtn.Size = new System.Drawing.Size(75, 23);
            this.DeleteLineBtn.TabIndex = 25;
            this.DeleteLineBtn.Text = "删除路线";
            this.DeleteLineBtn.UseVisualStyleBackColor = true;
            this.DeleteLineBtn.Click += new System.EventHandler(this.DeleteLineBtn_Click);
            // 
            // LineSpecialCB
            // 
            this.LineSpecialCB.AutoSize = true;
            this.LineSpecialCB.Location = new System.Drawing.Point(348, 139);
            this.LineSpecialCB.Name = "LineSpecialCB";
            this.LineSpecialCB.Size = new System.Drawing.Size(15, 14);
            this.LineSpecialCB.TabIndex = 26;
            this.LineSpecialCB.UseVisualStyleBackColor = true;
            // 
            // EditPointBtn
            // 
            this.EditPointBtn.Location = new System.Drawing.Point(802, 389);
            this.EditPointBtn.Name = "EditPointBtn";
            this.EditPointBtn.Size = new System.Drawing.Size(75, 23);
            this.EditPointBtn.TabIndex = 27;
            this.EditPointBtn.Text = "修改点";
            this.EditPointBtn.UseVisualStyleBackColor = true;
            this.EditPointBtn.Click += new System.EventHandler(this.EditPointBtn_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(275, 239);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 12);
            this.label8.TabIndex = 28;
            this.label8.Text = "移动尺量:";
            // 
            // LineMoveSizeTB
            // 
            this.LineMoveSizeTB.Location = new System.Drawing.Point(348, 236);
            this.LineMoveSizeTB.Name = "LineMoveSizeTB";
            this.LineMoveSizeTB.Size = new System.Drawing.Size(119, 21);
            this.LineMoveSizeTB.TabIndex = 29;
            this.LineMoveSizeTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LineMoveSizeTB_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(473, 239);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 30;
            this.label9.Text = "(默认0.1）";
            // 
            // LineUpBtn
            // 
            this.LineUpBtn.Location = new System.Drawing.Point(258, 411);
            this.LineUpBtn.Name = "LineUpBtn";
            this.LineUpBtn.Size = new System.Drawing.Size(45, 34);
            this.LineUpBtn.TabIndex = 31;
            this.LineUpBtn.Text = "Up";
            this.LineUpBtn.UseVisualStyleBackColor = true;
            this.LineUpBtn.Click += new System.EventHandler(this.LineUpBtn_Click);
            // 
            // LineDownBtn
            // 
            this.LineDownBtn.Location = new System.Drawing.Point(258, 467);
            this.LineDownBtn.Name = "LineDownBtn";
            this.LineDownBtn.Size = new System.Drawing.Size(45, 35);
            this.LineDownBtn.TabIndex = 32;
            this.LineDownBtn.Text = "Down";
            this.LineDownBtn.UseVisualStyleBackColor = true;
            this.LineDownBtn.Click += new System.EventHandler(this.LineDownBtn_Click);
            // 
            // LineInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 685);
            this.Controls.Add(this.LineDownBtn);
            this.Controls.Add(this.LineUpBtn);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.LineMoveSizeTB);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.EditPointBtn);
            this.Controls.Add(this.LineSpecialCB);
            this.Controls.Add(this.DeleteLineBtn);
            this.Controls.Add(this.DeletePointBtn);
            this.Controls.Add(this.SaveAgvLineBtn);
            this.Controls.Add(this.UseXmlAnBtn);
            this.Controls.Add(this.AddLineBtn);
            this.Controls.Add(this.EditeLineBtn);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.AddPointBtn);
            this.Controls.Add(this.PointYTB);
            this.Controls.Add(this.PointXTB);
            this.Controls.Add(this.PontDownBtn);
            this.Controls.Add(this.PointUpBtn);
            this.Controls.Add(this.LineDesSiteCB);
            this.Controls.Add(this.LineNowSiteTB);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LinePointListView);
            this.Controls.Add(this.AgvLineListView);
            this.Name = "LineInfoForm";
            this.Text = "Agv线路配置";
            this.Load += new System.EventHandler(this.LineInfoForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListView AgvLineListView;
        private System.Windows.Forms.ListView LinePointListView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox LineNowSiteTB;
        private System.Windows.Forms.ComboBox LineDesSiteCB;
        private System.Windows.Forms.Button PointUpBtn;
        private System.Windows.Forms.Button PontDownBtn;
        private System.Windows.Forms.TextBox PointXTB;
        private System.Windows.Forms.TextBox PointYTB;
        private System.Windows.Forms.Button AddPointBtn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button EditeLineBtn;
        private System.Windows.Forms.Button AddLineBtn;
        private System.Windows.Forms.Button UseXmlAnBtn;
        private System.Windows.Forms.Button SaveAgvLineBtn;
        private System.Windows.Forms.Button DeletePointBtn;
        private System.Windows.Forms.Button DeleteLineBtn;
        private System.Windows.Forms.CheckBox LineSpecialCB;
        private System.Windows.Forms.Button EditPointBtn;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox LineMoveSizeTB;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button LineUpBtn;
        private System.Windows.Forms.Button LineDownBtn;
    }
}