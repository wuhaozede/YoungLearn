using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoungLearn.Utility;

namespace YoungLearn
{
    class WindowsFrame
    {
        /// <summary>
        /// 带有外边框和标题的windows的样式
        /// </summary>
        public const int WS_CAPTION = 0X00C0000;

        /// <summary>
        /// window的基本样式
        /// </summary>
        public const int GWL_STYLE = -16;

        /// <summary>
        /// 设置窗体的样式
        /// </summary>
        /// <param name="handle">操作窗体的句柄</param>
        /// <param name="oldStyle">进行设置窗体的样式类型.</param>
        /// <param name="newStyle">新样式</param>
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern void SetWindowLong(IntPtr handle, int oldStyle, int newStyle);

        /// <summary>
        /// 获取窗体指定的样式.
        /// </summary>
        /// <param name="handle">操作窗体的句柄</param>
        /// <param name="style">要进行返回的样式</param>
        /// <returns>当前window的样式</returns>
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern int GetWindowLong(IntPtr handle, int style);
    }


    public class User_initialization
    {
        public enum User
        {
            Basic = 0,
            Superior = 1
        }

        public string User_url = "";

        public string Excel_path = "";

        public bool list_name = false;

        public User User_level = User.Basic;

        public int name_list = -1;

        public int num_list = -1;

        public User_initialization()
        {

        }

        ~User_initialization()
        {
            Save_value();
        }

        public void Save_value()
        {
            if (Get_value())
            {
                XMLclass xmlclass = new XMLclass();
                xmlclass.SetValue(XMLclass.Appconfig.User_url, User_url);
                xmlclass.SetValue(XMLclass.Appconfig.Excel_path, Excel_path);
                xmlclass.SetValue(XMLclass.Appconfig.list_name, list_name ? "1" : "0");
                xmlclass.SetValue(XMLclass.Appconfig.User, User_level.ToString("d"));
                xmlclass.SetValue(XMLclass.Appconfig.name_list, name_list.ToString("d"));
                xmlclass.SetValue(XMLclass.Appconfig.num_list, num_list.ToString("d"));
            }
        }
        public bool Get_value()
        {
            return User_url.Length != 0 && Excel_path.Length != 0 && name_list >= 0;
        }

        public string Get_value(string a)
        {
            switch (a)
            {
                case "User_url":
                    return User_url;
                case "Excel_path":
                    return Excel_path;
                case "list_name":
                    return list_name.ToString();
                case "User_level":
                    return User_level.ToString();
                case "name_list":
                    return name_list.ToString();
                case "num_list":
                    return num_list.ToString();
                case "all":
                case "All":
                    return Get_All_value();
                default:
                    return null;
            }
        }

        private string Get_All_value()
        {
            string str = "";
            str += "User_url:" + User_url + "\r\n";
            str += "Excel_path:" + Excel_path + "\r\n";
            str += "list_name:" + list_name.ToString() + "\r\n";
            str += "User_level" + User_level.ToString() + "\r\n";
            str += "name_list" + name_list.ToString() + "\r\n";
            str += "num_list" + num_list.ToString() + "\r\n";
            return str;
        }

        public void Set_value(Dictionary<string, string> keyValues)
        {
            User_url = keyValues["User_url"];
            Excel_path = keyValues["Excel_path"];
            list_name = keyValues["list_name"] == "1";
            User_level = keyValues["User"] == "2" ? User.Basic : User.Superior;
            name_list = int.Parse(keyValues["name_list"]);
            num_list = int.Parse(keyValues["num_list"]);
        }
    }
}
