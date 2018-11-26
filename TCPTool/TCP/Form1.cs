using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.IO;
using System.Net;
using AsyncTcp;
using System.Globalization;

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
            //client = new AsyncTcpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888));
            //client.ServerDisconnected += new EventHandler<TcpServerDisconnectedEventArgs>(client_ServerDisconnected);
            //client.PlaintextReceived += new EventHandler<TcpDatagramReceivedEventArgs<string>>(client_PlaintextReceived);
            //client.ServerConnected += new EventHandler<TcpServerConnectedEventArgs>(client_ServerConnected);
        }
        //ip和端口的默认值
        private const String IpText = "127.0.0.1";
        private const String PortText = "8888";
        private bool isConnect = false;/*是否已经连接的标识*/
        private string[] SplitOrder;/*接收后分离的代码*/

        AsyncTcpClient client;
        void client_ServerConnected(object sender, TcpServerConnectedEventArgs e)
        {
            receiveTb.BeginInvoke((MethodInvoker)delegate
            {
                receiveTb.AppendText(Line++ + "  连接服务端成功" + System.Environment.NewLine);
            });

        }

        void client_PlaintextReceived(object sender, TcpDatagramReceivedEventArgs<string> e)
        {
            if (e.Datagram != "Received")
            {
                textBox1.BeginInvoke((MethodInvoker)delegate
                {
                    System.DateTime currentTime = new System.DateTime();/*取当前年月日时分秒 */
                    currentTime = System.DateTime.Now;
                    textBox1.AppendText(currentTime + string.Format("系统： {0} --> ", e.TcpClient.Client.RemoteEndPoint.ToString()));
                    textBox1.AppendText(string.Format("{0}", e.Datagram) + System.Environment.NewLine);
                    string[] SplitOrder = Split(e.Datagram);
                });
            }

        }

        void client_ServerDisconnected(object sender, TcpServerDisconnectedEventArgs e)
        {
            receiveTb.BeginInvoke((MethodInvoker)delegate
            {
                receiveTb.AppendText(string.Format(CultureInfo.InvariantCulture, Line++ + "  与{0}服务器断开连接。", e.ToString()) + System.Environment.NewLine);
            });

        }
        /// <summary>
        /// 正规式分离
        /// </summary>
        /// <param name="x">分离的指令</param>
        /// <returns></returns>
        static string[] Split(string x)
        {
            var str = x;
            string[] result = Regex.Split(str, "(?<=\\G.{2})");
            return result;
        }
        /// <summary>
        /// 组合指令码
        /// </summary>
        /// <param name="x">输入指令码</param>
        /// <returns></returns>
        public string GroupOrdeer(string x)
        {
            string y, z;
            string[] h;
            z = x.Substring(6);
            h = Split(CRC.ToCRC16(z));
            y = x + h[1] + h[0];/*使低位在前*/
            return y;
        }
        public void StatusOrder(string x)
        {
            switch (SplitOrder[11])
            {
                case "01":
                    DirectionLb.Text = "小车状态：行驶";
                    break;
                case "02":
                    DirectionLb.Text = "小车状态：暂停";
                    break;
                case "03":
                    DirectionLb.Text = "小车状态：停站";
                    break;
                default:
                    DirectionLb.Text = "小车状态：异常";
                    break;
            }
            label11.Text = "当前站点：" + Convert.ToString(Convert.ToInt32(SplitOrder[27], 16));
            label12.Text = "当前速度比：" + Convert.ToString(Convert.ToInt32(SplitOrder[15], 16));
            switch (SplitOrder[16])
            {
                case "01":
                    label13.Text = "当前运行方向：正方向";
                    break;
                case "02":
                    label13.Text = "当前运行方向：反方向";
                    break;
                default:
                    label13.Text = "当前运行方向：异常";
                    break;
            }
            label14.Text = "当前电量比：" + Convert.ToString(Convert.ToInt32(SplitOrder[18], 16));
        }
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
        private static string
            Advance = ("574B083000000000A101"),
            Recede = ("574B083000000000A102"),
            Stop = ("574B083000000000A201"),
            Roller1 = ("1,1,1"),
            Roller2 = ("1,1,3"),
            Roller3 = ("1,2,3"),
            Roller4 = ("1,2,1"),
            Roller5 = ("1,2,1"),
            Point = ("1"),
            ClearPoint = ("574B083000000000A103"),
            Status = ("574B083000000000A500");

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
                MessageBox.Show("端口不是数字或端口不正确");
                return false;

            }
        }
        private bool IsPort()
        {
            bool blnTest = false;
            bool _Result = true;
            Regex regex = new Regex("^[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}$");
            blnTest = regex.IsMatch(this.ipTb.Text);
            if (blnTest == true)
            {
                string[] strTemp = this.ipTb.Text.Split(new char[] { '.' });
                for (int i = 0; i < strTemp.Length; i++)
                {
                    if (Convert.ToInt32(strTemp[i]) > 255)//大于255则提示，不符合IP格式 
                    {
                        MessageBox.Show("不符合IP格式");
                        return _Result = false;
                    }
                }
            }
            else
            {
                MessageBox.Show("不符合IP格式");//输入非数字则提示，不符合IP格式
                return _Result = false;

            }
            return _Result;
        }
        private void connectBtn_Click(object sender, EventArgs e)
        {
            bool blnText1 = false;//检查端口是否合法
            blnText1 = IsNumberic(portTb.Text);
            //连接服务端
            if (this.IsPort() && blnText1)
            {
                String ip = ipTb.Text;
                int port = Convert.ToInt32(portTb.Text);
                client = new AsyncTcpClient(new IPEndPoint(IPAddress.Parse(ip), port));
                client.ServerDisconnected += new EventHandler<TcpServerDisconnectedEventArgs>(client_ServerDisconnected);
                client.PlaintextReceived += new EventHandler<TcpDatagramReceivedEventArgs<string>>(client_PlaintextReceived);
                client.ServerConnected += new EventHandler<TcpServerConnectedEventArgs>(client_ServerConnected);
                if (connectBtn.Text == "连接")/*第一次连接*/
                {
                    try
                    {
                        client.Connect();
                        isConnect = true;
                    }
                    catch (Exception c)
                    {
                        MessageBox.Show(c.ToString() + "不符合IP格式");
                        receiveTb.AppendText(Line++ + "  未连接上了服务器：" + ip + "\n");
                    }
                }
                else if (connectBtn.Text == "重连")/*重新进行连接*/
                {
                    try
                    {
                        client.Close();
                        client.Connect();
                        isConnect = true;
                        receiveTb.AppendText(Line++ + " 成功与服务器重连" + ip + "\n");
                    }
                    catch (Exception c)
                    {
                        MessageBox.Show(c.ToString() + "不符合IP格式");
                        isConnect = false;
                    }
                }
            }
        }
        /// <summary>
        /// 发送指令
        /// </summary>
        private void ShowOrder(string x)
        {
            if (x == GroupOrdeer(Advance))
            {
                receiveTb.AppendText(Line++ + "  指令\t" + transmitTb.Text + "\t前进" + "\n");
                DirectionLb.Text = "小车状态：前进";
            }
            else if (x == GroupOrdeer(Recede))
            {
                receiveTb.AppendText(Line++ + "  指令\t" + transmitTb.Text + "\t后退" + "\n");
                DirectionLb.Text = "小车状态：后退";
            }
            else if (x == GroupOrdeer(Stop))
            {
                receiveTb.AppendText(Line++ + "  指令\t" + transmitTb.Text + "\t停止" + "\n");
                DirectionLb.Text = "小车状态：停止";
            }
            else if (x == GroupOrdeer(Roller1))
            {
                receiveTb.AppendText(Line++ + "  指令\t" + transmitTb.Text + "\t线边辊台上料停止" + "\n");
            }
            else if (x == GroupOrdeer(Roller2))
            {
                receiveTb.AppendText(Line++ + "  指令\t" + transmitTb.Text + "\t线边辊台上料反转" + "\n");
            }
            else if (x == GroupOrdeer(Roller3))
            {
                receiveTb.AppendText(Line++ + "  指令\t" + transmitTb.Text + "\t线边辊台下料停止" + "\n");
            }
            else if (x == GroupOrdeer(Roller4))
            {
                receiveTb.AppendText(Line++ + "  指令\t" + transmitTb.Text + "\t线边辊台下料正转" + "\t" + "车载辊台下料正转" + "\n");
                RollerLb.Text = "辊台状态：正转";
            }
            else if (x == GroupOrdeer(Roller5))
            {
                receiveTb.AppendText(Line++ + "  指令\t" + transmitTb.Text + "\t线边辊台下料正转" + "\t" + "车载辊台下料正转" + "\n");
                RollerLb.Text = "辊台状态：正转";

            }
            else if (x == GroupOrdeer(Point))
            {
                receiveTb.AppendText(Line++ + "  指令\t" + transmitTb.Text + "\t定点启动" + "\n");
            }
            else if (x == GroupOrdeer(ClearPoint))
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
                    try
                    {
                        client.Send(GroupOrdeer(transmitTb.Text) + "\n");
                        this.ShowOrder(transmitTb.Text);
                        transmitTb.Clear();//向对方发送消息
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
                client.Close();/*关闭连接*/
                client.Dispose();/*释放资源*/
                isConnect = false;
                receiveTb.AppendText(Line++ + "  已与服务器断开：" + "\n");
                connectBtn.Text = "重连";
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
                        Advance = GroupOrdeer(OrderTb.Text);
                        break;
                    case ("后退"):
                        Recede = GroupOrdeer(OrderTb.Text);
                        break;
                    case ("暂停"):
                        Stop = GroupOrdeer(OrderTb.Text);
                        break;
                    case ("辊台动"):
                        Roller5 = GroupOrdeer(OrderTb.Text);
                        break;
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }


        private void ReconnectBtn_Click(object sender, EventArgs e)
        {
            try
            {
                String ip = ipTb.Text;
                int port = Convert.ToInt32(portTb.Text);
                client.Close();
                client.Connect();
                isConnect = true;
            }
            catch (Exception c)
            {
                MessageBox.Show(c.ToString() + "不符合IP格式");
                isConnect = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.IsConnect())
            {
                transmitTb.Text = GroupOrdeer(Status);/*状态*/
                sentBtn.PerformClick();

            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            win32.ReleaseCapture();                                                                  //用来释放被当前线程中某个窗口捕获的光标                                                     
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
                    transmitTb.Text = GroupOrdeer(Stop);/*暂停*/
                    sentBtn.PerformClick();
                    transmitTb.Text = GroupOrdeer(Advance);/*前进*/
                    State = Order.暂停;
                    State = Order.前进;
                    sentBtn.PerformClick();
                }
                else
                {
                    transmitTb.Text = GroupOrdeer(Advance);/*前进*/
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
                    transmitTb.Text = GroupOrdeer(Stop);/*暂停*/
                    State = Order.暂停;
                    sentBtn.PerformClick();
                    transmitTb.Text = GroupOrdeer(Recede);/*后退*/
                    State = Order.后退;
                    sentBtn.PerformClick();
                }
                else if (State == Order.后退)
                {
                    receiveTb.AppendText(Line++ + "  小车已经在后退\n");
                }
                else
                {
                    transmitTb.Text = GroupOrdeer(Recede);/*后退*/
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
                    transmitTb.Text = GroupOrdeer(Stop);/*暂停*/
                    State = Order.暂停;
                    sentBtn.PerformClick();
                }
            }
        }

        private void rollBtn_Click(object sender, EventArgs e)
        {
            if (this.IsConnect())
            {
                transmitTb.Text = GroupOrdeer(Roller5);   /*辊台正转*/
                State = Order.车载辊台下料正转;
                sentBtn.PerformClick();
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            switch (comboBox1.Text)
            {
                case ("前进"):
                    OrderTb.Text = GroupOrdeer(Advance);
                    break;
                case ("后退"):
                    OrderTb.Text = GroupOrdeer(Recede);
                    break;
                case ("暂停"):
                    OrderTb.Text = GroupOrdeer(Stop);
                    break;
                case ("辊台动"):
                    OrderTb.Text = GroupOrdeer(Roller5);
                    break;
                default:
                    OrderTb.Text = "";
                    break;
            }
        }
        /// <summary>
        /// int转换为string
        /// </summary>
        /// <param name="asciiCode"></param>
        /// <returns></returns>
        public static string Chr(int asciiCode)
        {
            if (asciiCode >= 0 && asciiCode <= 255)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                byte[] byteArray = new byte[] { (byte)asciiCode };
                string strCharacter = asciiEncoding.GetString(byteArray);
                return (strCharacter);
            }
            else
            {
                throw new Exception("ASCII Code is not valid.");
            }
        }
        //#region 心跳Timer计数事件
        //private void heartbeatTimer_Tick(object sender, EventArgs e)
        //{
        //    currentCount++;
        //    if (currentCount == heartbeatCount)
        //    {
        //        txtMessage.Append("开始发送心跳包");
        //        MessageEntity entity = new MessageEntity();
        //        entity.MessageType = MessagePicks.Heartbeat;
        //        entity.NickName = loginName;

        //        WriteToStream(entity);
        //        currentCount = 0;
        //    }
        //}
        //#endregion
    }
}
