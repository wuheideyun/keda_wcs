using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XMLHelper;

namespace DispatchAnmination
{
    /// <summary>
    /// 画调度画板类
    /// </summary>
    class Anmination
    {
        /// <summary>
        /// imagelist资源
        /// </summary>
        private System.Windows.Forms.ImageList _imageList;

        public Anmination(ImageList image)
        {
            _imageList = image;

        }

        public void Draw(Graphics g,Point centerPoint)
        {
            foreach(LineModule m in ModuleControl._lineModules)
            {
                m.Draw(g);
            }

            foreach (AgvModule m in ModuleControl._agvModules)
            {
                m.Draw(g);
            }

            //agv = new AgvModule("AGV001", new Point(100,200));
            //agv.update(centerPoint);
            //agv.Draw(g);

            //plc = new PlcModule("窑尾PLC", new Point(200, 200));
            //plc.Draw(g);


        }
    }
}
