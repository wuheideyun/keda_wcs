using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace KEDAClient
{
    class FLog
    {
        private static List<string> Exceptions = new List<string>();
        private List<string> ExceptionsOnUse = new List<string>();
        private Object _obj = new object();
        public static void Init()
        {
            CreateFolder();
            Thread thread = new Thread(new FLog().LogServer)
            {
                IsBackground = true
            };
            thread.Start();
        }

        private void LogServer()
        {
            while (true)
            {
                Thread.Sleep(500);
                lock (_obj)
                {
                    if (ExceptionsOnUse.Count == 0)
                    {
                        ExceptionsOnUse.AddRange(Exceptions);
                        Exceptions.Clear();
                        WriteLog();
                    }

                }
            }

        }


        public static void Log(string e)
        {
            Exceptions.Add(e);
        }

        private static void CreateFolder()
        {
            string patch = Environment.CurrentDirectory + '\\' + "Log";

             if (!Directory.Exists(patch))
            {
                Directory.CreateDirectory(patch);
            }
        }

        /// <summary>
        /// 则默认在Debug目录下新建 YYYY-mm-dd_Log.log文件
        /// </summary>
        /// <returns></returns>
        private string GetLogFileName()
        {

            return Environment.CurrentDirectory + '\\' +"Log"+'\\'+
                               DateTime.Now.Year + '-' +
                               DateTime.Now.Month + '-' +
                               DateTime.Now.Day + "_Log.log";
        }

        private void WriteLog()
        {
            if (ExceptionsOnUse.Count == 0) return;
            StreamWriter fs = new StreamWriter(GetLogFileName(), true);
            foreach (var ex in ExceptionsOnUse)
            {
                fs.WriteLine("时间：" + DateTime.Now.ToString());
                fs.WriteLine(ex);
                //把异常信息输出到文件，因为异常文件由这几部分组成，这样就不用我们自己复制到文档中了
                //fs.WriteLine("异常时间：" + DateTime.Now.ToString());
                //fs.WriteLine("异常信息：" + ex.Message);
                //fs.WriteLine("异常对象：" + ex.Source);
                //fs.WriteLine("调用堆栈：\n" + ex.StackTrace.Trim());
                //fs.WriteLine("触发方法：" + ex.TargetSite);
                fs.WriteLine();
            }
            fs.Close();
            ExceptionsOnUse.Clear();
        }
    }
}
