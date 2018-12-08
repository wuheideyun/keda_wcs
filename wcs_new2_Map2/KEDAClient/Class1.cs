using GfxMapEditor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FormTest
{
    public class IStoreObj : IExDotInfo
    {
        string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        Point _iLocPoint = new Point();

        public Point ILocPoint
        {
            get { return _iLocPoint; }
            set { _iLocPoint = value; }
        }

        public float IAngel
        {
            get { return 0; }
        }

        Size _iSize = new Size(1500,1500);

        public Size ISize
        {
            get { return _iSize; }
            set { _iSize = value; }
        }

        Color _iBackColor = Color.Aquamarine;

        public Color IBackColor
        {
            get { return _iBackColor; }
            set { _iBackColor = value; }
        }
    }

    public class IMap : IExBitMapInfo
    {

        string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        Point _iLocPoint = new Point();

        public Point ILocPoint
        {
            get { return _iLocPoint; }
            set { _iLocPoint = value; }
        }

        float _iAngel = 0;

        public float IAngel
        {
            get { return _iAngel; }
            set { _iAngel = value; }
        }

        Size _iSize = new Size(15000, 15000);

        public Size ISize
        {
            get { return _iSize; }
            set { _iSize = value; }
        }

        Bitmap _iBitMap = null;

        public Bitmap IBitMap
        {
            get { return _iBitMap; }
            set { _iBitMap = value; }
        }
    }

    public class IWord : IExWordInfo
    {

        string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        Point _iLocPoint = new Point();

        public Point ILocPoint
        {
            get { return _iLocPoint; }
            set { _iLocPoint = value; }
        }

        float _iAngel = 0;

        public float IAngel
        {
            get { return _iAngel; }
            set { _iAngel = value; }
        }

        Size _iSize = new Size(1500, 1500);

        public Size ISize
        {
            get { return _iSize; }
            set { _iSize = value; }
        }

        Bitmap _iBitMap = null;

        public Bitmap IBitMap
        {
            get { return _iBitMap; }
            set { _iBitMap = value; }
        }

        Color _iBackColor = Color.Aquamarine;

        public Color IColor
        {
            get { return _iBackColor; }
            set { _iBackColor = value; }
        }

        Font _iFont = new Font("Arial", 1000);

        public Font IFont
        {
            get { return _iFont; }
            set { _iFont = value; }
        }

        string _value;

        public string Text
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
    }
}
