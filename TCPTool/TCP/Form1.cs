using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace TCP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //为TextBox设置默认值和默认值的前景色（字体颜色）
            ipTb.Text = IpText;
            ipTb.ForeColor = Color.Gray;
            portTb.Text = PortText;
            portTb.ForeColor = Color.Gray;
        }
        //ip和端口的默认值
        private const String IpText = "127.0.0.1";
        private const String PortText = "8888";
        private bool isConnect = false;/*是否已经连接的标识*/
        NetworkStream streamToServer;
        TcpClient tcp = new TcpClient();
        enum Order          /*当前命令标识*/
        {
            前进 = 1,
            后退 = 2,
            暂停 = 3,
            线边辊台上料停止 = 4,
            线边辊台上料正转 = 5,
            线边辊台下料停止 = 6,
            线边辊台下料正转 = 7,
            车载辊台下料正转 = 8,
            定点启动 = 9,
            清除站点 = 10,
            待命令 = 0,
        }
        private Order State = Order.待命令;
        private int Line = 0;
        private static string Advance = ("1,1"),
            Recede = ("1,2"),
            Stop = ("2,0"),
            Roller1 = ("1,1,1"),
            Roller2 = ("1,1,3"),
            Roller3 = ("1,2,3"),
            Roller4 = ("1,2,1"),
            Roller5 = ("1,2,1"),
            Point = ("1"),
            ClearPoint = ("3,0");
        /// <summary>
        /// 检查端口和ip
        /// </summary>
        private void SetDefaultText()
        {
            //检查端口是否为空或者默认值
            if (String.IsNullOrEmpty(portTb.Text))
            {

                portTb.Text = PortText;
                portTb.ForeColor = Color.Gray;
            }
            //检查ip是否为空或者默认值
            if (String.IsNullOrEmpty(ipTb.Text))
            {
                ipTb.Text = IpText;
                ipTb.ForeColor = Color.Gray;
            }
        }
        /// <summary>
        ///检查是否为数字
        /// </summary>    
        private bool IsNumberic(string oText)
        {
            try
            {
                int var1 = Convert.ToInt32(oText);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private void connectBtn_Click(object sender, EventArgs e)
        {
            //检查ip是否合法
            bool blnTest = false;
            bool _Result = true;
            Regex regex = new Regex("^[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}$");
            blnTest = regex.IsMatch(ipTb.Text);
            if (blnTest == true)
            {
                string[] strTemp = this.ipTb.Text.Split(new char[] { '.' });
                for (int i = 0; i < strTemp.Length; i++)
                {
                    if (Convert.ToInt32(strTemp[i]) > 255)//大于255则提示，不符合IP格式 
                    {
                        MessageBox.Show("不符合IP格式");
                        _Result = false;
                    }
                }
            }
            else
            {
                MessageBox.Show("不符合IP格式");//输入非数字则提示，不符合IP格式
                _Result = false;
            }
            bool blnText1 = false;//检查端口是否合法
            blnText1 = IsNumberic(portTb.Text);
            if (blnText1 == false)
            {
                MessageBox.Show("端口不是数字或端口不正确");
            }
            //连接服务端
            if (_Result && blnText1)
            {
                String ip = ipTb.Text;
                int port = Convert.ToInt32(portTb.Text);
                try
                {
                    tcp.Connect(ip, port);//根据服务器的IP地址和侦听的端口连接
                    if (tcp.Connected)
                    {
                        isConnect = true;//连接成功的消息机制
                        receiveTb.AppendText(Line++ + "  成功连接上了服务器：" + ip + "\n");
                        streamToServer = tcp.GetStream();
                    }
                    else
                    {
                        receiveTb.AppendText("未找到服务器：" + ip + "\n");
                    }
                }
                catch (Exception c)
                {
                    MessageBox.Show(c.ToString() + "不符合IP格式");
                    receiveTb.AppendText(Line++ + "  未连接上了服务器：" + ip + "\n");
                }

            }
        }
        /// <summary>
        /// 发送指令
        /// </summary>
        private void SendOrder(string x)
        {
            if (x == Advance)
            {
                receiveTb.AppendText(Line++ + "  指令\t" + transmitTb.Text + "\t前进" + "\n");
                DirectionLb.Text = "小车状态：前进";
            }
            else if (x == Recede)
            {
                receiveTb.AppendText(Line++ + "  指令\t" + transmitTb.Text + "\t后退" + "\n");
                DirectionLb.Text = "小车状态：后退";
            }
            else if (x == Stop)
            {
                receiveTb.AppendText(Line++ + "  指令\t" + transmitTb.Text + "\t停止" + "\n");
                DirectionLb.Text = "小车状态：停止";
            }
            else if (x == Roller1)
            {
                receiveTb.AppendText(Line++ + "  指令\t" + transmitTb.Text + "\t线边辊台上料停止" + "\n");
            }
            else if (x == Roller2)
            {
                receiveTb.AppendText(Line++ + "  指令\t" + transmitTb.Text + "\t线边辊台上料反转" + "\n");
            }
            else if (x == Roller3)
            {
                receiveTb.AppendText(Line++ + "  指令\t" + transmitTb.Text + "\t线边辊台下料停止" + "\n");
            }
            else if (x == Roller4)
            {
                receiveTb.AppendText(Line++ + "  指令\t" + transmitTb.Text + "\t线边辊台下料正转" + "\t" + "车载辊台下料正转" + "\n");
                RollerLb.Text = "辊台状态：正转";
            }
            else if (x == Roller5)
            {
                receiveTb.AppendText(Line++ + "  指令\t" + transmitTb.Text + "\t线边辊台下料正转" + "\t" + "车载辊台下料正转" + "\n");
                RollerLb.Text = "辊台状态：正转";

            }
            else if (x == Point)
            {
                receiveTb.AppendText(Line++ + "  指令\t" + transmitTb.Text + "\t定点启动" + "\n");
            }
            else if (x == ClearPoint)
            {
                receiveTb.AppendText(Line++ + "  指令\t" + transmitTb.Text + "\t清除站点" + "\n");
            }
            else
            {
                receiveTb.AppendText(Line++ + transmitTb.Text + "\n");
            }          
        }
        /// <summary>
        /// 发送指令到服务器
        /// </summary>
        private void Sent()
        {
            if (isConnect)
            {
                if (transmitTb.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("请输入指令");
                }
                else
                {
                    byte[] data = Encoding.ASCII.GetBytes(transmitTb.Text + "\n");
                    try
                    {
                        lock (streamToServer)
                        {
                            streamToServer.Write(data, 0, data.Length); // 发往服务器
                            this.SendOrder(Convert.ToString(transmitTb.Text));
                            transmitTb.Clear();
                        }
                    }
                    catch (Exception c)
                    {
                        MessageBox.Show(c.ToString());
                    }
                }
            }
            else
            {
                MessageBox.Show("请先进行服务器连接");
            }
        }
        /// <summary>
        /// 服务器是否连接
        /// </summary>
        private bool IsConnect()
        {
            if (!isConnect)
            {
                MessageBox.Show("请先连接服务器");
                return false;
            }
            else
            {
                return true;
            }
        }
        private void sentBtn_Click(object sender, EventArgs e)
        {
            this.Sent();
        }
        private void ipTb_Enter(object sender, EventArgs e)
        {
            if (ipTb.Text == IpText)
            {
                ipTb.Text = "";
                ipTb.ForeColor = Color.Black;
            }
        }

        private void ipTb_Leave(object sender, EventArgs e)
        {

            SetDefaultText();
        }

        private void portTb_Enter(object sender, EventArgs e)
        {
            if (portTb.Text == PortText)
            {
                portTb.Text = "";
                portTb.ForeColor = Color.Black;
            }
        }

        private void portTb_Leave(object sender, EventArgs e)
        {
            SetDefaultText();
        }

        private void disconnectBtn_Click(object sender, EventArgs e)
        {
            if (isConnect)
            {
                String ip = ipTb.Text;
                this.tcp.Close();/*关闭连接   tcp.Dispose();/*释放资源*/
                isConnect = false;
                receiveTb.AppendText(Line++ + "  已与服务器断开：" + ip + "\n");
            }
            else
            {
                MessageBox.Show("未创建连接，请先创建连接");
                ipTb.Focus();
            }

        }

        private void Set_Click(object sender, EventArgs e)
        {
            if (OrderTb.Text == null)
            {
                MessageBox.Show("请选择要修改的命令");
            }
            else
            {
                switch (comboBox1.Text)
                {
                    case ("前进"):
                        Advance = OrderTb.Text;
                        break;
                    case ("后退"):
                        Recede = OrderTb.Text;
                        break;
                    case ("暂停"):
                        Stop = OrderTb.Text;
                        break;
                    case ("辊台动"):
                        Roller5 = OrderTb.Text;
                        break;
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            win32.ReleaseCapture();                     //用来释放被当前线程中某个窗口捕获的光标                                                     
            win32.SendMessage(this.Handle, win32.WM_SYSCOMMAND, win32.SC_MOVE + win32.HTCAPTION, 0); //向Windows发送拖动窗体的消息
        }

        private void ipTb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                portTb.Focus();
            }
        }

        private void portTb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                connectBtn.PerformClick();
            }
        }

        private void advanceBtn_Click(object sender, EventArgs e)
        {
            if (this.IsConnect())
            {
                if (State == Order.前进)
                {
                    receiveTb.AppendText(Line++ + "  小车已经在前进\n");
                }
                else if (State == Order.后退)
                {
                    transmitTb.Text = Stop;/*暂停*/
                    sentBtn.PerformClick();
                    transmitTb.Text = Advance;/*前进*/
                    State = Order.暂停;
                    State = Order.前进;
                    sentBtn.PerformClick();
                }
                else
                {
                    transmitTb.Text = Advance;/*前进*/
                    State = Order.前进;
                    sentBtn.PerformClick();
                }
            }
        }

        private void retreatBtn_Click(object sender, EventArgs e)
        {
            if (this.IsConnect())
            {
                if (State == Order.前进)
                {
                    transmitTb.Text = Stop;/*暂停*/
                    State = Order.暂停;
                    sentBtn.PerformClick();
                    transmitTb.Text = Recede;/*后退*/
                    State = Order.后退;
                    sentBtn.PerformClick();
                }
                else if (State == Order.后退)
                {
                    receiveTb.AppendText(Line++ + "  小车已经在后退\n");
                }
                else
                {
                    transmitTb.Text = Recede;/*后退*/
                    State = Order.后退;
                    sentBtn.PerformClick();
                }
            }
        }

        private void pauseBtn_Click(object sender, EventArgs e)
        {
            if (this.IsConnect())
            {
                if (State == Order.暂停)
                {
                    receiveTb.AppendText(Line++ + "  小车已暂停，请继续下一步操作\n");
                }
                else
                {
                    transmitTb.Text = Stop;/*暂停*/
                    State = Order.暂停;
                    sentBtn.PerformClick();
                }
            }
        }

        private void rollBtn_Click(object sender, EventArgs e)
        {
            if (this.IsConnect())
            {
                transmitTb.Text = Roller5;/*辊台正转*/
                State = Order.车载辊台下料正转;
                sentBtn.PerformClick();
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            switch (comboBox1.Text)
            {
                case ("前进"):
                    OrderTb.Text = Advance;
                    break;
                case ("后退"):
                    OrderTb.Text = Recede;
                    break;
                case ("暂停"):
                    OrderTb.Text = Stop;
                    break;
                case ("辊台动"):
                    OrderTb.Text = Roller5;
                    break;
                default:
                    OrderTb.Text = "";
                    break;
            }
        }
    }
}
