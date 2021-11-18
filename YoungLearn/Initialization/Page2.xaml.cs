using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using YoungLearn.Utility;
using YoungLearn.MessageWindows;

namespace YoungLearn.Initialization
{
    public delegate void Change_UserHandler(User_initialization user_Initialization);


    /// <summary>
    /// Page2.xaml 的交互逻辑
    /// </summary>
    public partial class Page2 : Page
    {
        public event Change_UserHandler Change_User;
        public User_initialization user_Initialization;

        private readonly List<string> level1_name = new List<string> { "团省委", "地市", "直属高校", "直属企业团委", "省直团工委", "省国资委团工委", "独立院校", "各直接联系组织", "其他团组织", "系统团委" };
        private Httpclass httpclass = new Httpclass();
        

        private int level = 1;
        private string url;
        private Dictionary<string, string> level_d;

        public Page2(User_initialization user)
        {
            InitializeComponent();
            user_Initialization = user;
            combobox.ItemsSource = level1_name;
            Updata();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Multiselect = false,//该值确定是否可以选择多个文件
                Title = "请选择表格",
                Filter = "Excel表格(*.xls,*.xlsx)|*.*"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                user_Initialization.Excel_path = System.IO.Path.GetFullPath(dialog.FileName);
                Excelname.Text = "文件名：" + System.IO.Path.GetFileName(user_Initialization.Excel_path);
                Get_excel();
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            grid.Visibility = (bool)radiobutton_level1.IsChecked ? Visibility.Visible : Visibility.Hidden;
            text.Text = (bool)radiobutton_level1.IsChecked ? "组织名称" : "姓名列";
            user_Initialization.User_level = (bool)radiobutton_level1.IsChecked ? User_initialization.User.Superior : User_initialization.User.Basic;
            Change_User(user_Initialization);
            Button_Click_1(sender, e);
        }

        private void Updata()
        {
            if(user_Initialization.Get_value())
            {
                if(user_Initialization.User_level == User_initialization.User.Basic)
                {
                    radiobutton_level1.IsChecked = true;
                    grid.Visibility = Visibility.Visible;
                }
                else
                {
                    radiobutton_level2.IsChecked = true;
                    grid.Visibility = Visibility.Hidden;
                }

                text_url.Text = user_Initialization.User_url;
                Excelname.Text = "文件名：" + System.IO.Path.GetFileName(user_Initialization.Excel_path);

                if (user_Initialization.list_name)
                {
                    list1.IsChecked = true;
                }
                else
                {
                    list2.IsChecked = true;
                }
                Get_excel();
            }
        }

        private void Get_excel()
        {
            string path = System.IO.Path.GetFullPath(user_Initialization.Excel_path);
            DataTable dataTable = new DataTable();
            if (".xls" == System.IO.Path.GetExtension(path))
            {
                dataTable = Excelclass.RenderDataTableFromExcel(path, 0, 0);
            }
            else if(".xlsx" == System.IO.Path.GetExtension(path))
            {
                dataTable = Excelxclass.Readxlsx(path);
            }
            else
            {
                MessageWindow message = new MessageWindow("导入表格错误");
                message.ShowDialog();
                
            }
            
            combobox_name.Items.Clear();
            combobox_num.Items.Clear();

            foreach (DataColumn data in dataTable.Columns)
            {
                string str = data.Caption.ToString();
                _ = combobox_name.Items.Add(str);
                _ = combobox_num.Items.Add(str);
            }
        }

        private void combobox_name_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            user_Initialization.name_list = combobox_name.SelectedIndex;
            Change_User(user_Initialization);
        }

        private void combobox_num_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            user_Initialization.num_list = combobox_num.SelectedIndex;
            Change_User(user_Initialization);
        }

        private void combobox_DropDownClosed(object sender, EventArgs e)
        {
            if (combobox.SelectedItem == null)
            {
            }
            else
            {
                switch (level)
                {
                    case 1:
                        Dictionary<string, string> table_name = httpclass.GetTableName(combobox.SelectedItem.ToString());
                        text_url.Text += "组织：" + combobox.SelectedItem.ToString();
                        level += 1;
                        url = httpclass.get_level + "level1=" + combobox.SelectedItem.ToString() + "&table_name=" + DictionaryTools.DictionaryTools_First_Value(table_name);
                        //MessageWindows.MessageWindow message = new MessageWindows.MessageWindow(url);
                        //message.ShowDialog();
                        level_d = httpclass.GetLevel(url, 2);
                        combobox.ItemsSource = DictionaryTools.DictionaryTools_Keys(level_d);
                        break;
                    default:
                        level += 1;
                        url += "&level" + (level - 1).ToString() + "=" + combobox.SelectedItem.ToString();
                        level_d = httpclass.GetLevel(url, level);
                        if (DictionaryTools.DictionaryTools_First_Key(level_d) == "姓名")
                        {
                            if((bool)radiobutton_level1.IsChecked)
                            {
                                url = url.Substring(0, url.Length - combobox.SelectedItem.ToString().Length - 7);
                                MessageWindow message = new MessageWindow("请注意您的组织层级，\r\n如果选择错误需要重新输入");
                                _ = message.ShowDialog();
                            }
                            else
                            {
                                text_url.Text += " + " + combobox.SelectedItem.ToString();
                            }
                            combobox.ItemsSource = null;
                        }
                        else
                        {
                            text_url.Text += " + " + combobox.SelectedItem.ToString();
                            combobox.ItemsSource = DictionaryTools.DictionaryTools_Keys(level_d);
                        }
                        break;
                }
                user_Initialization.User_url = url;
                Change_User(user_Initialization);

            }
        }

        /// <summary>
        /// 清空名单地址
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            level = 1;
            text_url.Text = "";
            combobox.ItemsSource = level1_name;
        }
    }
}
