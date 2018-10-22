namespace KEDAClient
{
    partial class TaskForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxCurTar = new System.Windows.Forms.TextBox();
            this.buttonAlter = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxNextTars = new System.Windows.Forms.TextBox();
            this.buttonManSend = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panelMap = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBoxOutput = new System.Windows.Forms.ListBox();
            this.buttonCallAGv = new System.Windows.Forms.Button();
            this.buttonSendRun = new System.Windows.Forms.Button();
            this.buttonStartTask = new System.Windows.Forms.Button();
            this.buttonBackPoint = new System.Windows.Forms.Button();
            this.buttonTaskDone = new System.Windows.Forms.Button();
            this.timerFunc = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(11, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "本站点:";
            // 
            // textBoxCurTar
            // 
            this.textBoxCurTar.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxCurTar.ForeColor = System.Drawing.Color.Red;
            this.textBoxCurTar.Location = new System.Drawing.Point(93, 25);
            this.textBoxCurTar.Name = "textBoxCurTar";
            this.textBoxCurTar.ReadOnly = true;
            this.textBoxCurTar.Size = new System.Drawing.Size(100, 29);
            this.textBoxCurTar.TabIndex = 2;
            // 
            // buttonAlter
            // 
            this.buttonAlter.Location = new System.Drawing.Point(209, 28);
            this.buttonAlter.Name = "buttonAlter";
            this.buttonAlter.Size = new System.Drawing.Size(75, 23);
            this.buttonAlter.TabIndex = 3;
            this.buttonAlter.Text = "修改";
            this.buttonAlter.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(332, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 19);
            this.label2.TabIndex = 4;
            this.label2.Text = "目的地:";
            // 
            // textBoxNextTars
            // 
            this.textBoxNextTars.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxNextTars.ForeColor = System.Drawing.Color.Red;
            this.textBoxNextTars.Location = new System.Drawing.Point(405, 28);
            this.textBoxNextTars.Name = "textBoxNextTars";
            this.textBoxNextTars.ReadOnly = true;
            this.textBoxNextTars.Size = new System.Drawing.Size(273, 29);
            this.textBoxNextTars.TabIndex = 5;
            // 
            // buttonManSend
            // 
            this.buttonManSend.Location = new System.Drawing.Point(693, 31);
            this.buttonManSend.Name = "buttonManSend";
            this.buttonManSend.Size = new System.Drawing.Size(75, 23);
            this.buttonManSend.TabIndex = 6;
            this.buttonManSend.Text = "自动模式";
            this.buttonManSend.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabControl1.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(12, 75);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(759, 267);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabPage1.Location = new System.Drawing.Point(4, 31);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(751, 232);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "地图展示";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panelMap);
            this.tabPage2.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabPage2.Location = new System.Drawing.Point(4, 31);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(751, 232);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "配送站点";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panelMap
            // 
            this.panelMap.BackColor = System.Drawing.Color.LightBlue;
            this.panelMap.Location = new System.Drawing.Point(3, 6);
            this.panelMap.Name = "panelMap";
            this.panelMap.Size = new System.Drawing.Size(743, 220);
            this.panelMap.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBoxOutput);
            this.groupBox1.Location = new System.Drawing.Point(12, 364);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(227, 137);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "系统信息";
            // 
            // listBoxOutput
            // 
            this.listBoxOutput.FormattingEnabled = true;
            this.listBoxOutput.ItemHeight = 12;
            this.listBoxOutput.Location = new System.Drawing.Point(6, 20);
            this.listBoxOutput.Name = "listBoxOutput";
            this.listBoxOutput.Size = new System.Drawing.Size(215, 112);
            this.listBoxOutput.TabIndex = 9;
            // 
            // buttonCallAGv
            // 
            this.buttonCallAGv.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonCallAGv.Location = new System.Drawing.Point(282, 392);
            this.buttonCallAGv.Name = "buttonCallAGv";
            this.buttonCallAGv.Size = new System.Drawing.Size(88, 84);
            this.buttonCallAGv.TabIndex = 16;
            this.buttonCallAGv.Text = "呼叫AGV";
            this.buttonCallAGv.UseVisualStyleBackColor = false;
            // 
            // buttonSendRun
            // 
            this.buttonSendRun.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonSendRun.Location = new System.Drawing.Point(389, 392);
            this.buttonSendRun.Name = "buttonSendRun";
            this.buttonSendRun.Size = new System.Drawing.Size(88, 84);
            this.buttonSendRun.TabIndex = 17;
            this.buttonSendRun.Text = "启动AGV";
            this.buttonSendRun.UseVisualStyleBackColor = false;
            // 
            // buttonStartTask
            // 
            this.buttonStartTask.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonStartTask.Location = new System.Drawing.Point(505, 392);
            this.buttonStartTask.Name = "buttonStartTask";
            this.buttonStartTask.Size = new System.Drawing.Size(88, 84);
            this.buttonStartTask.TabIndex = 18;
            this.buttonStartTask.Text = "派发任务";
            this.buttonStartTask.UseVisualStyleBackColor = false;
            // 
            // buttonBackPoint
            // 
            this.buttonBackPoint.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonBackPoint.Location = new System.Drawing.Point(616, 392);
            this.buttonBackPoint.Name = "buttonBackPoint";
            this.buttonBackPoint.Size = new System.Drawing.Size(88, 84);
            this.buttonBackPoint.TabIndex = 19;
            this.buttonBackPoint.Text = "返回待命";
            this.buttonBackPoint.UseVisualStyleBackColor = false;
            // 
            // buttonTaskDone
            // 
            this.buttonTaskDone.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonTaskDone.Location = new System.Drawing.Point(730, 392);
            this.buttonTaskDone.Name = "buttonTaskDone";
            this.buttonTaskDone.Size = new System.Drawing.Size(88, 84);
            this.buttonTaskDone.TabIndex = 20;
            this.buttonTaskDone.Text = "确认完成";
            this.buttonTaskDone.UseVisualStyleBackColor = false;
            // 
            // TaskForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 505);
            this.Controls.Add(this.buttonTaskDone);
            this.Controls.Add(this.buttonBackPoint);
            this.Controls.Add(this.buttonStartTask);
            this.Controls.Add(this.buttonSendRun);
            this.Controls.Add(this.buttonCallAGv);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.buttonManSend);
            this.Controls.Add(this.textBoxNextTars);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonAlter);
            this.Controls.Add(this.textBoxCurTar);
            this.Controls.Add(this.label1);
            this.Name = "TaskForm";
            this.Text = "任务管理";
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxCurTar;
        private System.Windows.Forms.Button buttonAlter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxNextTars;
        private System.Windows.Forms.Button buttonManSend;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panelMap;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listBoxOutput;
        private System.Windows.Forms.Button buttonCallAGv;
        private System.Windows.Forms.Button buttonSendRun;
        private System.Windows.Forms.Button buttonStartTask;
        private System.Windows.Forms.Button buttonBackPoint;
        private System.Windows.Forms.Button buttonTaskDone;
        private System.Windows.Forms.Timer timerFunc;
    }
}

