using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Configuration;
using YoungLearn.MessageWindows;
using System.Data;
using YoungLearn.Utility;

namespace YoungLearn.Initialization
{
    /// <summary>
    /// Newyounglearn.xaml 的交互逻辑
    /// </summary>
    public partial class Newyounglearn : Window
    {
        private User_initialization user_Initialization = new User_initialization();

        public Newyounglearn()
        {
            InitializeComponent();
            Page1 p = new Page1();
            _ = frmMain.Navigate(p);
        }

        private void btn_minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btn_maximize_Click(object sender, RoutedEventArgs e)
        {
            ChangeWindowSize();
        }
        private void ChangeWindowSize()
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Title_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        private void Title_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                ChangeWindowSize();
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 获取窗体句柄
            IntPtr windowsHandle = new System.Windows.Interop.WindowInteropHelper(this).Handle;

            // 获得窗体的样式
            int oldStyle = WindowsFrame.GetWindowLong(windowsHandle, WindowsFrame.GWL_STYLE);

            // 更改窗体的样式为无边框窗体
            WindowsFrame.SetWindowLong(windowsHandle, WindowsFrame.GWL_STYLE, oldStyle & ~WindowsFrame.WS_CAPTION);
        }

        private void Button_Click_Next(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = sender as System.Windows.Controls.Button;

            if (btn.Tag.ToString() == "Page1")
            {
                first_button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0E0E0"));
                second_button.Background = Brushes.WhiteSmoke;
                Page2 p = new Page2(user_Initialization);
                _ = frmMain.Navigate(p);
                p.Change_User += new Change_UserHandler(Frm_Change_User);
                btn.Tag = "Page2";
            }
            else if (btn.Tag.ToString() == "Page2")
            {
                if (user_Initialization.Get_value())
                {
                    //MessageWindow message = new MessageWindow(user_Initialization.Get_value("all"));
                    //_ = message.ShowDialog();
                    WriteSQL();
                    Close();
                }
                else
                {
                    _ = System.Windows.Forms.MessageBox.Show("Error", "!");
                    System.Windows.Forms.Application.Exit();
                }
            }
        }

        private void Frm_Change_User(User_initialization user)
        {
            user_Initialization = user;
            user_Initialization.Save_value();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Next_Button.Tag.ToString() == "Page1")
            {
                first_button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0E0E0"));
                second_button.Background = Brushes.WhiteSmoke;
                Page2 p = new Page2(user_Initialization);
                _ = frmMain.Navigate(p);
                p.Change_User += new Change_UserHandler(Frm_Change_User);
                Next_Button.Tag = "Page2";
            }
            if (Next_Button.Tag.ToString() == "Page2")
            {
                
            }
            else
            {
                first_button.Background = Brushes.WhiteSmoke;
                second_button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0E0E0"));
                Page1 p = new Page1();
                _ = frmMain.Navigate(p);
                Next_Button.Tag = "Page1";
            }
        }

        private void WriteSQL()
        {
            try
            {

                string dbpath = "testDB.db";
                SQLclass sqlclass = new SQLclass(dbpath);

                DataTable dataTable = Excelclass.RenderDataTableFromExcel(user_Initialization.Excel_path, 0, 0);
                if (int.Parse(user_Initialization.Get_value("num_list")) != -1)
                {
                    List<string> l_name = new List<string> { "组织名称", "人数" };
                    List<string> l_type = new List<string> { "TEXT", "INT" };
                    _ = sqlclass.Create_table("user", l_name, l_type);
                    List<string> name_list = (from r in dataTable.AsEnumerable() select r.Field<string>(dataTable.Columns[user_Initialization.name_list].Caption)).ToList();
                    List<string> num_list = (from r in dataTable.AsEnumerable() select r.Field<string>(dataTable.Columns[user_Initialization.num_list].Caption)).ToList();

                    int x = user_Initialization.list_name ? 1 : 0;
                    List<List<string>> data = new List<List<string>> { };
                    for (; x < name_list.Count; x++)
                    {
                        List<string> a = new List<string> { name_list[x], num_list[x] }; ;
                        data.Add(a);
                    }
                    _ = sqlclass.Insert_table("user", l_name, data);
                }
                else
                {
                    List<string> l_name = new List<string> { "名称" };
                    List<string> l_type = new List<string> { "TEXT" };
                    _ = sqlclass.Create_table("user", l_name, l_type);
                    List<string> name_list = (from r in dataTable.AsEnumerable() select r.Field<string>(dataTable.Columns[user_Initialization.name_list].Caption)).ToList();

                    int x = user_Initialization.list_name ? 1 : 0;
                    List<List<string>> data = new List<List<string>> { };
                    for (; x < name_list.Count; x++)
                    {
                        List<string> a = new List<string> { name_list[x] }; ;
                        data.Add(a);
                    }
                    _ = sqlclass.Insert_table("user", l_name, data);
                }
            }
            catch(System.Data.SQLite.SQLiteException ex)
            {
                MessageWindow message = new MessageWindow("SQLError:" + ex.Message);
                message.Show();
            }
            catch (Exception ex)
            {
                MessageWindow message = new MessageWindow(ex.Message);
                message.Show();
            }
        }
    }
}
