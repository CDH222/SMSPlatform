using SMSPlatform.Model;
using SMSPlatform.SQL;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SMSPlatform.usercontrol
{
    /// <summary>
    /// FestivalGreetingsSMS.xaml 的交互逻辑
    /// </summary>
    public partial class FestivalGreetingsSMS : UserControl
    {
        public FestivalGreetingsSMS()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        CommonControl commonControl = new CommonControl();
        TeacherSQL teacherSQL = new TeacherSQL();
        TplSQL tplSQL = new TplSQL();
        
        private void LoadData()
        {
            Grid grid = new Grid();
            grid.HorizontalAlignment = HorizontalAlignment.Center;
            IList<TplInfo> list = tplSQL.QueryByTpl_name("节日祝福");
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    Expander epd = new Expander();
                    epd.Header = commonControl.SetExpanderHeader("节日祝福模板", list[i].IsCheck, false);
                    Button btnSend = new Button();
                    Button btnPreview = new Button();

                    btnPreview.Height = 30;
                    btnPreview.Width = 80;
                    btnPreview.Content = "预览";
                    btnPreview.Click += btnPreview_Click;
                    btnPreview.Margin = new Thickness(0, 20, 200, 0);

                    btnSend.Height = 30;
                    btnSend.Width = 80;
                    btnSend.Content = "发送";
                    btnSend.Click += btnSend_Click;
                    btnSend.Margin = new Thickness(200, 20, 0, 0);

                    grid = commonControl.SetExpanderContent(list[i].Tpl_id, true);
                    grid.Children.Add(btnPreview);
                    grid.Children.Add(btnSend);
                    Grid.SetRow(btnPreview, 3);
                    Grid.SetRow(btnSend, 3);
                    epd.Content = grid;

                    sptpl.Children.Add(epd);
                }
            }
        }

        void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            int textNum = 0;
            string tpl_text = null;
            commonControl.GetTpl_text(e, out textNum, out tpl_text);
            if (textNum > 0)
            {
                MessageBox.Show("请填写相关内容！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            Button btnPreview = e.Source as Button;
            TextBox tbContent = VisualTreeHelper.GetChild(btnPreview.Parent as Grid, 1) as TextBox;
            tbContent.Text = tpl_text;
        }

        void btnSend_Click(object sender, RoutedEventArgs e)
        {
            int textNum = 0;
            string tpl_text;
            commonControl.GetTpl_text(e, out textNum, out tpl_text);
            if (tpl_text.IndexOf("\r\n\r\n提示：") > 0)
            {
                MessageBox.Show("当前无法发送！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (textNum > 0)
            {
                MessageBox.Show("请填写相关内容！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            IList<TeacherInfo> list = teacherSQL.QueryAll();
            if (list.Count > 0)
            {
                SendProgressBar spb = new SendProgressBar();
                HwndSource winformWindow = HwndSource.FromDependencyObject(this) as HwndSource;
                new WindowInteropHelper(spb) { Owner = winformWindow.Handle };
                spb.SendSms(list, tpl_text);
                spb.ShowDialog();
            }
            else
            {
                MessageBox.Show("无相关数据！","提示",MessageBoxButton.OK,MessageBoxImage.Information);
            }

        }
    }
}
