using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLiteHelper
{
    /// <summary>
    /// 数据库字段类型
    /// </summary>
    public enum SQLType : int
    {
        /// <summary>
        /// 整型
        /// </summary>
        INT = 1,
        
        /// <summary>
        /// 字符串
        /// </summary>
        VARCHAR = 2,
        
        /// <summary>
        /// 文本
        /// </summary>
        TEXT = 3,
        
        /// <summary>
        /// 时间日期类型
        /// </summary>
        DATETIME = 4
    }


    /// <summary>
    /// 数据库表操作：增删改查
    /// </summary>
    class SQLiteTable
    {
        /// <summary>
        /// 表明
        /// </summary>
        private String _tableName;

        /// <summary>
        /// 外键名
        /// </summary>
        public String _pkName {
            set
            {
                if ("id".Equals(value))
                {
                    _pkName = _tableName + "id";
                }
                else
                {
                    _pkName = value;
                }
            }
            get{
                return _pkName;
            }
        }

        /// <summary>
        /// 字段定义List
        /// </summary>
        public List<Colum> _colums;

        public SQLiteTable(String tablename,String pkname)
        {
            _tableName = tablename;
            _pkName = pkname;
            _colums = new List<Colum>();
        }

        public void AddColum(String name,SQLType type)
        {
            _colums.Add(new Colum(name, type));
        }

        private void GetTableCreateSQL()
        {
            String sql = "create table " + _tableName + "(" +
                "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                _pkName +" "+ Colum.GetTypeStr(SQLType.INT);

            if(_colums.Count() > 0)
            {
                foreach(Colum colum in _colums)
                {
                    sql = sql + "," + colum._name + " " + Colum.GetTypeStr(colum._type);
                }
            }
            sql = sql + ");"; 
        }
    }


    public class Colum
    {
        internal String _name;
        internal SQLType _type;

        public Colum(String name, SQLType type)
        {
            _name = name;
            _type = type;
        }

        public static String GetTypeStr(SQLType type)
        {
            String str = "";
            switch (type)
            {
                case SQLType.INT:
                    str = "INTEGER";
                    break;
                case SQLType.VARCHAR:
                    str = "VARCHAR(255)";
                    break;
                case SQLType.TEXT:
                    str = "TEXT";
                    break;
                case SQLType.DATETIME:
                    str = "TIME";
                    break;
                default:
                    str = "TEXT";
                    break;
            }
            return str;
        }
    }
}
