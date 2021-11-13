using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace YoungLearn.Utility
{
    public class XMLclass
    {
        public enum Appconfig
        {
            User_url = 1,
            Excel_path = 2,
            list_name = 3,
            User = 4,
            name_list = 5,
            num_list = 6,
        }

        XmlDocument xml = new XmlDocument();
        XmlNode root;

        public XMLclass()
        {
            if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/Appconfig.xml"))
            {
                Newxml();
            }
            xml.Load(AppDomain.CurrentDomain.BaseDirectory + "/Appconfig.xml");
            root = xml.SelectSingleNode("config");
        }

        ~XMLclass()
        {
            xml.Save(AppDomain.CurrentDomain.BaseDirectory + "/Appconfig.xml");
        }

        private void Newxml()
        {
            //首先创建 XmlDocument xml文档 
            XmlDocument xml_ = new XmlDocument();
            //创建根节点 config           
            XmlElement config = xml_.CreateElement("config");
            //把根节点加到xml文档中            
            xml_.AppendChild(config);
               
            XmlElement User_url = xml_.CreateElement("User_url");
            User_url.InnerText = "";
            _ = config.AppendChild(User_url);
     
            XmlElement Excel_path = xml_.CreateElement("Excel_path");
            Excel_path.InnerText = "";
            _ = config.AppendChild(Excel_path);

            XmlElement list_name = xml_.CreateElement("list_name");
            list_name.InnerText = "-1";
            _ = config.AppendChild(list_name);

            XmlElement User = xml_.CreateElement("User");
            User.InnerText = "";
            _ = config.AppendChild(User);

            XmlElement name_list = xml_.CreateElement("name_list");
            name_list.InnerText = "";
            _ = config.AppendChild(name_list);

            XmlElement num_list = xml_.CreateElement("num_list");
            num_list.InnerText = "";
            _ = config.AppendChild(num_list);

            //最后将整个xml文件保存在D盘             
            xml_.Save(AppDomain.CurrentDomain.BaseDirectory + "/Appconfig.xml");
        }

        public string GetValue(Appconfig appconfig)
        {
            XmlNode xn;
            switch (appconfig)
            {
                case Appconfig.User_url:
                    xn = root.SelectSingleNode("User_url");
                    return xn.InnerText;
                case Appconfig.Excel_path:
                    xn = root.SelectSingleNode("Excel_path");
                    return xn.InnerText;
                case Appconfig.list_name:
                    xn = root.SelectSingleNode("list_name");
                    return xn.InnerText;
                case Appconfig.User:
                    xn = root.SelectSingleNode("User");
                    return xn.InnerText;
                case Appconfig.name_list:
                    xn = root.SelectSingleNode("name_list");
                    return xn.InnerText;
                case Appconfig.num_list:
                    xn = root.SelectSingleNode("num_list");
                    return xn.InnerText;
                default:
                    return "";
            }
        }

        public Dictionary<string, string> GetAllValue()
        {
            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            keyValues.Add("User_url",root.SelectSingleNode("User_url").InnerText);
            keyValues.Add("Excel_path", root.SelectSingleNode("Excel_path").InnerText);
            keyValues.Add("list_name", root.SelectSingleNode("list_name").InnerText);
            keyValues.Add("User", root.SelectSingleNode("User").InnerText);
            keyValues.Add("name_list", root.SelectSingleNode("name_list").InnerText);
            keyValues.Add("num_list", root.SelectSingleNode("num_list").InnerText);
            return keyValues;
        }

        public void SetValue(Appconfig appconfig, string text)
        {
            XmlNode xn;
            switch (appconfig)
            {
                case Appconfig.User_url:
                    xn = root.SelectSingleNode("User_url");
                    xn.InnerText = text;
                    break;
                case Appconfig.Excel_path:
                    xn = root.SelectSingleNode("Excel_path");
                    xn.InnerText = text;
                    break;
                case Appconfig.list_name:
                    xn = root.SelectSingleNode("list_name");
                    xn.InnerText = text;
                    break;
                case Appconfig.User:
                    xn = root.SelectSingleNode("User");
                    xn.InnerText = text;
                    break;
                case Appconfig.name_list:
                    xn = root.SelectSingleNode("name_list");
                    xn.InnerText = text;
                    break;
                case Appconfig.num_list:
                    xn = root.SelectSingleNode("num_list");
                    xn.InnerText = text;
                    break;
                default:
                    break;
            }
        }

        public bool GetValue()
        {
            return GetValue(Appconfig.User_url).Length != 0 &&
                    GetValue(Appconfig.Excel_path).Length != 0 &&
                    GetValue(Appconfig.list_name).Length != 0 &&
                    GetValue(Appconfig.User).Length != 0 &&
                    GetValue(Appconfig.name_list).Length != 0;
        }
    }

    
}
