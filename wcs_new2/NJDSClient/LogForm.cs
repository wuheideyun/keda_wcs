using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NJDSClient
{
    public partial class LogForm : Form
    {
        public bool _isLogin = false;

        public LogForm()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            _isLogin = APPConfig.UserLogin(textBoxName.Text, textBoxPassWord.Text);

            if (_isLogin)
            {
                MessageBox.Show("登录成功！");

                this.Close();
            }
            else
            {
                MessageBox.Show("验证失败！");
            }
        }
    }
}
