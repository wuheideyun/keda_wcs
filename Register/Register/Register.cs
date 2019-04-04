using FLBasicHelper;
using System;
using System.Windows.Forms;

namespace Register
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
            this.dateTimePicker1.Value = DateTime.Now.AddDays(1.0);
            this.buttonGetCode_Click(null, null);

        }
             

        private void buttonGetMac_Click(object sender, EventArgs e)
        {
            try
            {
                string str = MacHelper.cpuId();
                string str2 = MacHelper.baseId();
                this.textBoxInput.Text = str.Insert(5, str2.Substring(str2.Length - 6, 4));
            }
            catch (Exception exception)
            {
                this.textBoxInput.Text = exception.ToString();
            }

        }

        private void buttonGetCode_Click(object sender, EventArgs e)
        {
            string text = this.textBoxInput.Text;
            string input = this.dateTimePicker1.Value.ToString();
            input = "2025/01/01 00:00:00";
            this.textBoxOutput.Text = ISecretHelper.IEncrypt(text, input);
            this.textBoxBac.Text = ISecretHelper.IDecode(text, this.textBoxOutput.Text);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = this.textBoxInput.Text;
            this.textBoxBac.Text = ISecretHelper.IDecode(text, this.textBoxOutput.Text);
        }
    }
}
