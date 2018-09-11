using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace HQHPCtrol
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createNew = false;

            Mutex hMutex = new Mutex(false, "HQHPAppMutex", out createNew);

            if (!createNew)
            {
                MessageBox.Show("已经存在一个运行的应用程序！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormCTR());
        }
    }
}
