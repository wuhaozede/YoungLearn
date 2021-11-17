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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Drawing;
using System.Data;
using System.Configuration;
using YoungLearn.Utility;
using YoungLearn.MessageWindows;
using System.Text.RegularExpressions;
using System.Windows.Forms.DataVisualization.Charting;
using AutoUpdaterDotNET;


namespace YoungLearn
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public XMLclass xmlclass = new XMLclass();
        public User_initialization initialization = new User_initialization();
        public MainWindow()
        {
            AutoUpdater.Start("http://miaochunhuaixia.top/YoungLearn/update.xml");

            InitializeComponent();

            if (!xmlclass.GetValue())
            {
                Initialization.Newyounglearn newyounglearn = new Initialization.Newyounglearn();
                _ = newyounglearn.ShowDialog();
            }
            else
            {
                initialization.Set_value(xmlclass.GetAllValue());
                MessageWindow message = new MessageWindow(initialization.Get_value("all"));
                _ = message.ShowDialog();
            }
            try
            {
                Initialization_SQLite();
                Initialization_YoungLearn();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void Initialization_SQLite()
        {
            string dbpath = "testDB.db";
            if (!System.IO.File.Exists(dbpath))
            {
                MessageWindow message = new MessageWindow("软件数据库存在异常，请准备好您组织的名单，并重新启动软件！");
                message.Show();
                
                Initialization.Newyounglearn newyounglearn = new Initialization.Newyounglearn();
                _ = newyounglearn.ShowDialog();
            }
        }

        private void Initialization_YoungLearn()
        {
            Httpclass httpclass = new Httpclass();
            
            string name = DictionaryTools.DictionaryTools_First_Value(httpclass.GetTableName("直属高校"));
            string pattern = "reason_stage\\d+";
            Regex rgx = new Regex(pattern);
            string result = rgx.Replace(initialization.User_url, name);
            initialization.User_url = result;

            if (int.Parse(initialization.Get_value("num_list")) == -1)
            {
                DataTable data = DictionaryTools.Dictionary_To_DataTable(httpclass.GetLevel(result));
                string dbpath = "testDB.db";
                SQLclass sqlclass = new SQLclass(dbpath);
                if (sqlclass.ExistTable(name))
                {
                    sqlclass.ExitTable(name);
                }
                _ = sqlclass.Insert_table(name, data);

                /*
                 * 此处为图标初始化入口
                 */
                Initialization_pant(sqlclass, name);

                /*
                 * 此处为表格初始化入口
                 */
                Initialization_DataGrid(sqlclass, name);
            }
            else
            {
                MessageWindow message = new MessageWindow("暂时不对上级开放");
                _ = message.ShowDialog();
            }
        }

        private void Initialization_pant(SQLclass sqlclass, string table_name)
        {
            /*
             * 此处使用到了sql里面特殊方法GetNowData
             * 建议重写该方法
             */
            Dictionary<string,int> dic = sqlclass.GetNowData("user", table_name);

            chart.ChartAreas.Clear(); //图表区
            chart.Titles.Clear(); //图表标题
            chart.Series.Clear(); //图表序列
            chart.Legends.Clear(); //图表图例

            //新建chart图表要素
            chart.ChartAreas.Add(new ChartArea("chartArea"));
            chart.ChartAreas["chartArea"].AxisX.IsMarginVisible = false;
            chart.ChartAreas["chartArea"].Area3DStyle.Enable3D = false;
            chart.Titles.Add("完成情况"); //标题
            chart.Titles[0].Font = new Font("宋体", 18);
            chart.Series.Add("data");
            chart.Series["data"].ChartType = SeriesChartType.Doughnut;  //图标类型
            chart.Series["data"]["PieLabelStyle"] = "Outside";
            chart.Series["data"]["PieLineColor"] = "Black";
            chart.Legends.Add(new Legend("legend"));
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.ChartAreas[0].BackColor = System.Drawing.Color.WhiteSmoke;
            

            foreach (KeyValuePair<string, int> item in dic)//注意类型是KeyValuePair
            {
                string key = item.Key;
                int value = item.Value;

                if (key.Length < 1)
                {
                    key = "未完成";
                }

                int idxA = chart.Series["data"].Points.AddY(value);
                DataPoint pointA = chart.Series["data"].Points[idxA];
                pointA.Label = key;
                pointA.LegendText = "#LABEL(#VAL) #PERCENT{P2}";
                
            }
        }

        private void Initialization_DataGrid(SQLclass sqlclass, string table_name)
        {
            sqlclass.GetGoodLearn("user", table_name);
            DataView dv = new DataView(sqlclass.Data);
            GoodLearnDataGrid.ItemsSource = dv;

            sqlclass.GetBadLearn("user", table_name);
            DataView dv_ = new DataView(sqlclass.Data);
            BadLearnDataGrid.ItemsSource = dv_;
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
            System.Windows.Application.Current.Shutdown();
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
