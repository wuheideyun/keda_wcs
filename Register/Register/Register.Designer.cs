using System;
using System.Drawing;
using System.Windows.Forms;

namespace Register
{
    partial class Register
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
      

            this.SuspendLayout();
       
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Name = "Register";
            this.Text = "Register";
            this.ResumeLayout(false);


            this.textBoxInput = new TextBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.textBoxOutput = new TextBox();
            this.buttonGetMac = new Button();
            this.buttonGetCode = new Button();
            this.label3 = new Label();
            this.textBoxBac = new TextBox();
            this.dateTimePicker1 = new DateTimePicker();
            this.button1 = new Button();
            base.SuspendLayout();
            this.textBoxInput.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.textBoxInput.Location = new Point(0x2f, 0x41);
            this.textBoxInput.Name = "textBoxInput";
            this.textBoxInput.Size = new Size(0x27b, 0x15);
            this.textBoxInput.TabIndex = 0;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(12, 0x45);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x1d, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "MAC:";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(12, 110);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x1d, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "GET:";
            this.textBoxOutput.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.textBoxOutput.Location = new Point(0x2f, 0x6a);
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.Size = new Size(0x27b, 0x15);
            this.textBoxOutput.TabIndex = 2;
            this.buttonGetMac.Location = new Point(0x195, 0xbc);
            this.buttonGetMac.Name = "buttonGetMac";
            this.buttonGetMac.Size = new Size(0x4b, 0x17);
            this.buttonGetMac.TabIndex = 4;
            this.buttonGetMac.Text = "获取本地";
            this.buttonGetMac.UseVisualStyleBackColor = true;
            this.buttonGetMac.Click += new EventHandler(this.buttonGetMac_Click);
            this.buttonGetCode.Location = new Point(510, 0xbc);
            this.buttonGetCode.Name = "buttonGetCode";
            this.buttonGetCode.Size = new Size(0x4b, 0x17);
            this.buttonGetCode.TabIndex = 5;
            this.buttonGetCode.Text = "获取注册码";
            this.buttonGetCode.UseVisualStyleBackColor = true;
            this.buttonGetCode.Click += new EventHandler(this.buttonGetCode_Click);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(12, 0x92);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x1d, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "BAC:";
            this.textBoxBac.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.textBoxBac.Location = new Point(0x2f, 0x8e);
            this.textBoxBac.Name = "textBoxBac";
            this.textBoxBac.Size = new Size(0x27b, 0x15);
            this.textBoxBac.TabIndex = 6;
            this.dateTimePicker1.Location = new Point(0x2f, 0x15);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new Size(0x187, 0x15);
            this.dateTimePicker1.TabIndex = 8;
            this.dateTimePicker1.Visible = false;
            this.button1.Location = new Point(0x25f, 0xbc);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x4b, 0x17);
            this.button1.TabIndex = 9;
            this.button1.Text = "反演";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x2b6, 0xe9);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.dateTimePicker1);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.textBoxBac);
            base.Controls.Add(this.buttonGetCode);
            base.Controls.Add(this.buttonGetMac);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.textBoxOutput);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.textBoxInput);
            base.Name = "Register";
            this.Text = "IdeaRegister";
       
            base.ResumeLayout(false);
            base.PerformLayout();

        }

        #endregion

        private Button button1;
        private TextBox textBoxInput;
        private Label label1;
        private Label label2;
        private TextBox textBoxOutput;
        private Button buttonGetMac;
        private Button buttonGetCode;
        private Label label3;
        private TextBox textBoxBac;
        private DateTimePicker dateTimePicker1;
    }
}

