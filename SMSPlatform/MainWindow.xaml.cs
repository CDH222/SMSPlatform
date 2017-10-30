using SMSPlatform.usercontrol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SMSPlatform
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            icon();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WaitingWindow.Show(this);
            FestivalGreetingsSMS fgs = new FestivalGreetingsSMS();
            mainShow.Children.Clear();
            mainShow.Children.Add(fgs);
        }

        private void btnNoticeSMS_Click(object sender, RoutedEventArgs e)
        {
            WaitingWindow.Show(this);
            NoticeSMS ns = new NoticeSMS();
            mainShow.Children.Clear();
            mainShow.Children.Add(ns);
        }

        private void btnFestivalGreetingsSMS_Click(object sender, RoutedEventArgs e)
        {
            WaitingWindow.Show(this);
            FestivalGreetingsSMS fgs = new FestivalGreetingsSMS();
            mainShow.Children.Clear();
            mainShow.Children.Add(fgs);
        }

        private void btnBirthdayBlessingSMS_Click(object sender, RoutedEventArgs e)
        {
            WaitingWindow.Show(this);
            BirthdayBlessingSMS bbs = new BirthdayBlessingSMS();
            mainShow.Children.Clear();
            mainShow.Children.Add(bbs);
        }
        
        private void btnTemplateManager_Click(object sender, RoutedEventArgs e)
        {
            WaitingWindow.Show(this);
            TemplateManager tm = new TemplateManager();
            mainShow.Children.Clear();
            mainShow.Children.Add(tm);
        }

        private void btnImportData_Click(object sender, RoutedEventArgs e)
        {
            WaitingWindow.Show(this);
            ImportData id = new ImportData();
            mainShow.Children.Clear();
            mainShow.Children.Add(id);
        }

        #region 通知栏
        NotifyIcon notifyIcon = new NotifyIcon();
        private void icon()
        {
            this.notifyIcon.BalloonTipText = "程序已经缩小到托盘，打开窗口请双击图标即可。"; //设置程序启动时显示的文本
            this.notifyIcon.BalloonTipTitle = "提示";
            this.notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            this.notifyIcon.Text = "太原工业学院短信平台";//最小化到托盘时，鼠标点击时显示的文本
            this.notifyIcon.Icon = new System.Drawing.Icon("images/sms.ico");//程序图标
            this.notifyIcon.Visible = false;

            //右键菜单--打开菜单项
            System.Windows.Forms.MenuItem open = new System.Windows.Forms.MenuItem("显示");
            open.Click += new EventHandler(ShowWindow);
            //右键菜单--退出菜单项
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("退出");
            exit.Click += new EventHandler(CloseWindow);
            //关联托盘控件
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { open, exit };
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

            notifyIcon.MouseDoubleClick += IconDoubleClick;
        }

        private void IconDoubleClick(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                //还原窗体显示    
                WindowState = WindowState.Normal;
                //激活窗体并给予它焦点
                this.Activate();
                //任务栏区显示图标
                this.notifyIcon.Visible = false;
                this.ShowInTaskbar = true;
            }
        }

        private void ShowWindow(object sender, EventArgs e)
        {
            //还原窗体显示    
            WindowState = WindowState.Normal;
            //激活窗体并给予它焦点
            this.Activate();
            //任务栏区显示图标
            this.notifyIcon.Visible = false;
            this.ShowInTaskbar = true;
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            // 关闭所有的线程
            System.Windows.Application.Current.Shutdown();
        }

        #endregion

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            WindowState = WindowState.Minimized;
            //隐藏任务栏区图标
            this.ShowInTaskbar = false;
            this.notifyIcon.Visible = true;
            this.notifyIcon.ShowBalloonTip(1000);
        }
    }
}
