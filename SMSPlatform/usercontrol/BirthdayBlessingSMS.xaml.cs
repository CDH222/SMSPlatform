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
        TplSQL tplSQL = new TplSQL();

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            Grid grid = new Grid();
            IList<TplInfo> list = tplSQL.QueryByTpl_name("生日祝福");
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    Expander epd = new Expander();
                    epd.Header = commonControl.SetExpanderHeader("生日祝福模板", list[i].IsCheck, true);
                    Button btn = new Button();
                    btn.Height = 30;
                    btn.Width = 80;
                    btn.Content = "选择模板";
                    btn.Click += btnSet_Click;
                    btn.Margin = new Thickness(0, 20, 0, 0);
                    grid = commonControl.SetExpanderContent(list[i].Tpl_id, "生日祝福", false);
                    grid.Children.Add(btn);
                    Grid.SetRow(btn, 3);
                    epd.Content = grid;

                    sptpl.Children.Add(epd);
                }
            }
        }

        void btnSet_Click(object sender, RoutedEventArgs e)
        {
            Button btn = e.Source as Button;
            string tpl_text = (VisualTreeHelper.GetChild(btn.Parent as Grid, 0) as Label).Content.ToString();
            if (tpl_text.IndexOf("\r\n\r\n提示：") > 0)
            {
                MessageBox.Show("当前无法选择！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            tplInfo.Tpl_id = (VisualTreeHelper.GetChild(btn.Parent as Grid, 3) as TextBlock).Text;
            tplInfo.IsCheck = true;
            if(tplSQL.Update_Bir(tplInfo)>0)
            {
                MessageBox.Show("选择成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
