
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KEDAClient
{
    public partial class LogForm : Form
    {
        // 用户登陆的初始状态为false
        public bool _isLogin = false;

        public LogForm()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 登录按钮操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            _isLogin = KEDAClient.KEDAForm.APPConfig.UserLogin(textBoxName.Text, textBoxPassWord.Text);

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
