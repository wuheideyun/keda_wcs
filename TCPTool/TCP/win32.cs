using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace TCP
{
    class win32
    {
        public const int WM_SYSCOMMAND = 0x0112;                //该变量表示将向Windows发送的消息类型
        public const int SC_MOVE = 0xF010;                  //该变量表示发送消息的附加消息
        public const int HTCAPTION = 0x0002;                    //该变量表示发送消息的附加消息
        //用来释放被当前线程中某个窗口捕获的光标
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        //向指定的窗体发送Windows消息
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwdn, int wMsg, int mParam, int lParam);


    }
}
