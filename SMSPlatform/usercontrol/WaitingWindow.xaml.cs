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

namespace SMSPlatform.usercontrol
{
    /// <summary>
    /// WaitingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WaitingWindow : Window
    {
        public WaitingWindow()
        {
            InitializeComponent();
            this.Loaded += WaitingBox_Loaded;
        }

        Action Callback = () => { System.Threading.Thread.Sleep(1500); };

        void WaitingBox_Loaded(object sender, RoutedEventArgs e)
        {
            Callback.BeginInvoke(this.OnComplate, null);
        }

        void OnComplate(IAsyncResult ar)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                this.Close();
            }));
        }
        /// <summary>
        /// 显示等待框，owner指定宿主视图元素，callback为需要执行的方法体（需要自己做异常处理）。
        /// 目前等等框为模式窗体
        /// </summary>
        public static void Show(FrameworkElement owner)
        {
            WaitingWindow win = new WaitingWindow();
            Window pwin = Window.GetWindow(owner);
            win.Owner = pwin;
            var loc = owner.PointToScreen(new Point());
            win.Left = loc.X + (owner.ActualWidth - win.Width) / 2;
            win.Top = loc.Y + (owner.ActualHeight - win.Height) / 2;
            win.ShowDialog();
        }
    }
}
