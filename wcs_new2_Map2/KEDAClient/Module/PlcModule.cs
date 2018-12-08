using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DispatchAnmination
{
    /// <summary>
    /// PLC模型
    /// 长方形：内有PLC字样
    /// </summary>
    public class PlcModule :Module
    {
        /// <summary>
        /// PLC长方形
        /// </summary>
        private Rectangle _rectangle;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">PLC名称</param>
        /// <param name="point">PLC中心点</param>
        public PlcModule(string name, Point point)
        {
            _name = name;

            _centerP = point;

            _rectangle = new Rectangle();

            _pen = new Pen(new SolidBrush(Color.Black));

            Update(point);
        }

        /// <summary>
        /// 画图方法
        /// </summary>
        /// <param name="g">GUI绘画图面</param>
        public override void Draw(Graphics g)
        {
            g.DrawRectangle(_pen, _rectangle);

            g.DrawString(_name, _font, Brushes.Black, _describP);
        }

        /// <summary>
        /// 更新当前Module的图像位置
        /// </summary>
        /// <param name="centerPoint"></param>
        public override void Update(Point centerPoint)
        {
            _rectangle.X = _centerP.X - _size * _scale;

            _rectangle.Y = _centerP.Y - _size * _scale;

            _rectangle.Width = _size * _scale;

            _rectangle.Height = _size * 2 * _scale;

            _describP.X = _centerP.X - _size * _scale;
            _describP.Y = _centerP.Y + _size * _scale;
        }
    }
}
