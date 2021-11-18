using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoungLearn.MessageWindows;

namespace YoungLearn.Utility
{
    public class Httpclass
    {
        public string get_tablename = "http://dxx.ahyouth.org.cn/api/peopleRankList?";
        public string get_level = "http://dxx.ahyouth.org.cn/api/peopleRankStage?";


        public string GetHttpResponse(string url, int Timeout)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                request.UserAgent = null;
                request.Timeout = Timeout;
            
                Stream responseStream = ((HttpWebResponse)request.GetResponse()).GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                string str = reader.ReadToEnd();
                reader.Close();
                responseStream.Close();
                return str;
            }
            catch (WebException)
            {
                MessageWindow message = new MessageWindow("网络未连接，请及时处理！");
                _ = message.ShowDialog();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return null;
            }
        }

        /// <summary>
        /// 获取青年大学习首个视频名称
        /// </summary>
        /// <param name="level">顶级单位名称</param>
        /// <returns>首个名称</returns>
        public Dictionary<string, string> GetTableName(string level)
        {
            string url = get_tablename + "level1=" + level;
            string httpResponse = GetHttpResponse(url, 6000);
            
            
            if (httpResponse != null)
            {
                JObject jobject = (JObject)JsonConvert.DeserializeObject(httpResponse);
                int length = ((JContainer)jobject["list"]).Count;

                //MessageWindow m = new MessageWindow(length.ToString());
                //_ = m.ShowDialog();

                Dictionary<string, string> dictionary = new Dictionary<string, string> { { jobject["list"][0]["name"].ToString(), jobject["list"][0]["table_name"].ToString()} };    
                return dictionary;
            }
            return null;
        }

        /// <summary>
        /// 获取指定个数的青年大学习视频名称
        /// </summary>
        /// <param name="level">顶级单位名称</param>
        /// <param name="i">指定个数</param>
        /// <returns>指定个数个视频名称</returns>  
        public Dictionary<string, string> GetTableName(string level, int i)
        {
            string url = get_tablename + "level1=" + level;
            string httpResponse = GetHttpResponse(url, 6000);
            if (httpResponse != null)
            {
                JObject jobject = (JObject)JsonConvert.DeserializeObject(httpResponse);
                Dictionary<string, string> dictionary = new Dictionary<string, string> { };
                int length = ((JContainer)jobject["list"]).Count;
                if (i > length)
                {
                    i = length;
                }
                for (int a = 0; a < i; a++)
                {
                    dictionary.Add(jobject["list"][a]["name"].ToString(), jobject["list"][a]["table_name"].ToString());
                }
                return dictionary;
            }
            return null;
        }

        /// <summary>
        /// 获取指定范围的青年大学习视频名称
        /// </summary>
        /// <param name="level">顶级单位名称</param>
        /// <param name="firstone">开始序号</param>
        /// <param name="lastone">结束序号</param>
        /// <returns>指定范围的视频名称</returns>
        public Dictionary<string, string> GetTableName(string level, int firstone, int lastone)
        {
            string url = get_tablename + "level1=" + level;
            string httpResponse = GetHttpResponse(url, 6000);
            if (httpResponse != null)
            {
                JObject jobject = (JObject)JsonConvert.DeserializeObject(httpResponse);
                Dictionary<string, string> dictionary = new Dictionary<string, string> { };
                int length = ((JContainer)jobject["list"]).Count;
                if (firstone > length)
                {
                    firstone = length - 1;
                    lastone = length;
                }
                else if (lastone > length)
                {
                    lastone = length;
                }
                for (int a = firstone; a < lastone; a++)
                {
                    dictionary.Add(jobject["list"][a]["name"].ToString(), jobject["list"][a]["table_name"].ToString());
                }
                return dictionary;
            }
            return null;
        }

        public Dictionary<string, string> GetLevel(string url, int level_int)
        {
            string httpResponse = GetHttpResponse(url, 6000);
            if (httpResponse != null)
            {
                JObject jobject = (JObject)JsonConvert.DeserializeObject(httpResponse);
                Dictionary<string, string> dictionary = new Dictionary<string, string> { };
                int length;
                if (jobject["list"].Type == JTokenType.Object)
                {
                    length = ((JContainer)jobject["list"]["list"]).Count;
                    if (((JObject)jobject["list"]["list"][0]).Property("username") == null)
                    {
                        for (int a = 0; a < length; a++)
                        {
                            dictionary.Add(jobject["list"]["list"][a]["level" + level_int.ToString()].ToString(), jobject["list"]["list"][a]["num"].ToString());
                        }
                    }
                    else
                    {
                        dictionary.Add("姓名", "完成日期");
                        for (int a = 0; a < length; a++)
                        {
                            dictionary.Add(jobject["list"]["list"][a]["username"].ToString(), jobject["list"]["list"][a]["addtime"].ToString());
                        }
                    }
                }
                else
                {
                    length = ((JContainer)jobject["list"]).Count;
                    for (int a = 0; a < length; a++)
                    {
                        dictionary.Add(jobject["list"][a]["level" + level_int.ToString()].ToString(), jobject["list"][a]["num"].ToString());
                    }
                }
                
                return dictionary;
            }
            return null;
        }
        public Dictionary<string, string> GetLevel(string url)
        {
            string httpResponse = GetHttpResponse(url, 6000);
            if (httpResponse != null)
            {
                JObject jobject = (JObject)JsonConvert.DeserializeObject(httpResponse);
                Dictionary<string, string> dictionary = new Dictionary<string, string> { };
                int length;
                if (jobject["list"].Type == JTokenType.Object)
                {
                    length = ((JContainer)jobject["list"]["list"]).Count;
                    dictionary.Add("姓名", "完成日期");
                    for (int a = 0; a < length; a++)
                    {
                        dictionary.Add(jobject["list"]["list"][a]["username"].ToString(), jobject["list"]["list"][a]["addtime"].ToString());
                    }

                }
                return dictionary;
            }
            return null;
        }
    }

    static class DictionaryTools
    {
        /// <summary>
        /// 获取字典第一个值的键
        /// </summary>
        /// <param name="a"></param>
        /// <returns>返回字符串</returns>
        public static string DictionaryTools_First_Key(Dictionary<string, string> a)
        {
            foreach (string key in a.Keys)
            {
                return key;
            }
            return null;
        }

        /// <summary>
        /// 获取字典的键
        /// </summary>
        /// <param name="a">字典</param>
        /// <returns>返回列表</returns>
        public static List<string> DictionaryTools_Keys(Dictionary<string, string> a)
        {
            var keys_list = new List<string>();
            foreach (string key in a.Keys)
            {
                keys_list.Add(key);
            }
            return keys_list;
        }

        /// <summary>
        /// 获取字典的第一个值
        /// </summary>
        /// <param name="a">字典</param>
        /// <returns>返回字符串</returns>
        public static string DictionaryTools_First_Value(Dictionary<string, string> a)
        {
            foreach (string value in a.Values)
            {
                return value;
            }
            return null;
        }

        public static DataTable Dictionary_To_DataTable(Dictionary<string, string> a)
        {
            DataTable dt = new DataTable();

            List<string> keyList = a.Keys.ToList();
            List<string> valueList = a.Values.ToList();

            dt.Columns.Add("姓名", typeof(string));
            dt.Columns.Add("完成日期", typeof(string));

            //keyList.ForEach(t => dt.Rows.Add("名称", t));
            //var list = dt.AsEnumerable().Select(t => new { a = t.Field<string>("a"), b = t.Field<string>("b") }).ToList();

            for (int x = 1; x < keyList.Count; x++)
            {
                dt.Rows.Add(keyList[x], valueList[x]);
            }
            return dt;

        }

        public static DataTable ObjectToTable(object obj)
        {
            try
            {
                Type t = obj.GetType().IsGenericType ? obj.GetType().GetGenericTypeDefinition() : obj.GetType();
                if (t == typeof(List<>) ||
                    t == typeof(IEnumerable<>))
                {
                    DataTable dt = new DataTable();
                    IEnumerable<object> lstenum = obj as IEnumerable<object>;
                    if (lstenum.Count() > 0)
                    {
                        IEnumerator<object> ob1 = lstenum.GetEnumerator();
                        ob1.MoveNext();
                        foreach (System.Reflection.PropertyInfo item in ob1.Current.GetType().GetProperties())
                        {
                            dt.Columns.Add(new DataColumn() { ColumnName = item.Name });
                        }
                        //数据
                        foreach (object item in lstenum)
                        {
                            DataRow row = dt.NewRow();
                            foreach (System.Reflection.PropertyInfo sub in item.GetType().GetProperties())
                            {
                                row[sub.Name] = sub.GetValue(item, null);
                            }
                            dt.Rows.Add(row);
                        }
                        return dt;
                    }
                }
                else if (t == typeof(DataTable))
                {
                    return (DataTable)obj;
                }
                else   //(t==typeof(Object))
                {
                    DataTable dt = new DataTable();
                    foreach (System.Reflection.PropertyInfo item in obj.GetType().GetProperties())
                    {
                        dt.Columns.Add(new DataColumn() { ColumnName = item.Name });
                    }
                    DataRow row = dt.NewRow();
                    foreach (System.Reflection.PropertyInfo item in obj.GetType().GetProperties())
                    {
                        row[item.Name] = item.GetValue(obj, null);
                    }
                    dt.Rows.Add(row);
                    return dt;
                }

            }
            catch (Exception ex)
            {
            }
            return null;
        }
    }
}
