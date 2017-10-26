using SMSPlatform.Model;
using SMSPlatform.SQL;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
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

namespace SMSPlatform.usercontrol
{
    /// <summary>
    /// BirthdayBlessingSMS.xaml 的交互逻辑
    /// </summary>
    public partial class BirthdayBlessingSMS : UserControl
    {
        public BirthdayBlessingSMS()
        {
            InitializeComponent();
        }
        TplInfo tplInfo = new TplInfo();
        CommonControl commonControl = new CommonControl();

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            Grid grid = new Grid();
            tplInfo.Tpl_name = 3;
            using (OleDbDataReader rdr = TplSQL.QueryByTpl_name(tplInfo))
            {
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        Expander epd = new Expander();
                        epd.Header = commonControl.SetExpanderHeader("生日祝福");
                        Button btn = new Button();
                        btn.Height = 30;
                        btn.Width = 80;
                        btn.Content = "设置";
                        btn.Click += btnSend_Click;
                        btn.Margin = new Thickness(0, 20, 0, 0);
                        grid = commonControl.SetExpanderContent(rdr, false);
                        grid.Children.Add(btn);
                        Grid.SetRow(btn, 3);
                        epd.Content = grid;

                        sptpl.Children.Add(epd);
                    }
                }
            }
            string tpl_id = Properties.Settings.Default.tpl_id;
            TextBlock tb = new TextBlock();
            foreach (Expander ep in sptpl.Children.OfType<Expander>())
            {
                StackPanel spHeader = ep.Header as StackPanel;
                TextBlock tbID = VisualTreeHelper.GetChild(ep.Content as Grid, 3) as TextBlock;
                if (tbID.Text == tpl_id)
                {
                    tb.Text = "当前模板已选择！";
                    tb.Margin = new Thickness(100, 0, 0, 0);
                    tb.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 255));
                    spHeader.Children.Add(tb);
                }
            }
        }

        void btnSend_Click(object sender, RoutedEventArgs e)
        {
            Button btn = e.Source as Button;
            Properties.Settings.Default.tpl_id = (VisualTreeHelper.GetChild(btn.Parent as Grid, 3) as TextBlock).Text;
            Properties.Settings.Default.Save();//使用Save方法保存更改
            MessageBox.Show("设置成功！");
        }
    }
}
