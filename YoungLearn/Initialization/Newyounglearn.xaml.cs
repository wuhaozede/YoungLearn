using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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
                    Close();
                }
                else
                {
                    _ = System.Windows.Forms.MessageBox.Show("Error, The Initialization Key is Null", "!");
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

    }
}
