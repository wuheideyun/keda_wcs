using System;
using System.Collections.Generic;
using System.Linq;

namespace ArrayMap
{
    /// <summary>
    /// Map类
    /// 属性 Key    String
    /// 属性 Value  Ojbect
    /// </summary>
    public class Map
    {
        /// <summary>
        /// Map数据的键值
        /// </summary>
        public Object Key { internal set; get; }

        /// <summary>
        /// Map数据键对应的值
        /// </summary>
        public Object Value { internal set; get;  }


        internal Map(){ }

        internal Map(Object key,Object value)
        {
            this.Key = key;
            this.Value = value;
        }
    }

    /// <summary>
    /// Map数组，用于保存Key-value组合
    /// </summary>
    public class MapList
    {
        /// <summary>
        /// 对象锁
        /// </summary>
        private Object _obj = new object();

        private List<Map> _maps;

        /// <summary>
        /// Map的长度
        /// </summary>
        public long length { private set; get; }

        /// <summary>
        /// 
        /// </summary>
        public MapList()
        {
            length = 0;
            _maps = new List<Map>();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Put(String key,Object value)
        {
            lock (_obj)
            {
                if (key is null) return;
                if (key.Length == 0 || key.Equals("")) return;
                if (ContainsKey(key))
                {
                    Map map = GetMap(key);
                    int index = _maps.IndexOf(map);
                    _maps[index].Value = value;
                }
                else
                {
                    _maps.Add(new Map(key, value));
                    length++;
                }
            }
        }

        /// <summary>
        /// 返回ArrayMap的长度
        /// </summary>
        /// <returns></returns>
        public long Length()
        {
           return length;
        }

        /// <summary>
        /// 判断是否为空
        /// </summary>
        /// <returns>如果为空则返回True,否则返回False</returns>
        public Boolean IsEmpty()
        {
            return _maps.Count()==0 ? true : false;
        }

       /// <summary>
       /// 判断是否已经有Key值为key的Map数据
       /// </summary>
       /// <param name="key"></param>
       /// <returns>找到对应Key返回True,否则返回false</returns>
        public Boolean ContainsKey(String key)
        {
            lock (_obj)
            {
                if (_maps.Count == 0) return false;
                foreach (Map map in _maps)
                {
                    if (map.Key.Equals(key))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Object Get(String key)
        {
            lock (_obj)
            {
                if (key is null) return null;
                if (_maps is null || _maps.Count == 0) return null;
                foreach (Map map in _maps)
                {
                    if (map.Key.Equals(key))
                    {
                        return map.Value;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 根据Key返回map对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal Map GetMap(String key)
        {
            lock (_obj)
            {
                if (key is null) return null;
                if (_maps is null || _maps.Count == 0) return null;
                foreach (Map map in _maps)
                {
                    if (map.Key.Equals(key))
                    {
                        return map;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 根据key获取字符串的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public String GetStringValue(String key)
        {
            lock (_obj)
            {
                Object value = Get(key);
                
                return value is null ? null : (value is String ? (string)value : (value is Boolean && (Boolean)value ? "True":"False") );
            }
            
        }

        /// <summary>
        /// 根据Key返回布尔类型的结果
        /// 1.如果没有这个key则返回false
        /// 2.如果是字符串key则返回false
        /// 3.如果是布尔类型则返回布尔类型
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Boolean GetBooleanValue(String key)
        {
            lock (_obj)
            {
                Object value = Get(key);

                return value is null ? false : (value is Boolean ? (Boolean)value : false);
            }
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        public void Clear()
        {
            lock (_obj)
            {
                if (_maps != null) _maps.Clear();
                length = 0;
            }
        }


        /// <summary>
        /// 移除key的数据
        /// </summary>
        /// <param name="key"></param>
        public void Remove(String key)
        {
            lock (_obj)
            {
                if (ContainsKey(key))
                {
                    Map map = GetMap(key);
                    _maps.Remove(map);
                    length--;
                }
            }

        }

        /// <summary>
        /// 根据索引获取Map
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Map数据</returns>
        public Map GetMapAtIndex(int index)
        {
            lock (_obj)
            {
                if (_maps is null) return null;
                if (_maps.Count() == 0) return null;
                if (index >= _maps.Count()) return null;
                return _maps[index];
            }
        }
    }
}
