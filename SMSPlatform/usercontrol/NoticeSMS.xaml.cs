using SMSPlatform.Model;
using SMSPlatform.SQL;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
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
    /// NoticeSMS.xaml 的交互逻辑
    /// </summary>
    public partial class NoticeSMS : UserControl
    {
        public NoticeSMS()
        {
            InitializeComponent();
        }
        TplInfo tplInfo = new TplInfo();
        TeacherInfo teacherInfo = new TeacherInfo();
        CommonControl commonControl = new CommonControl();

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            Grid grid = new Grid();
            tplInfo.Tpl_name = 1;
            #region 教师信息
            Brush brush = new SolidColorBrush(Color.FromRgb(0, 0, 255));
            Thickness thick1 = new Thickness(5, 0, 5, 0);
            using (OleDbDataReader rdr = TeacherSQL.QueryAll())
            {
                if (rdr.RecordsAffected !=0)
                {
                    while (rdr.Read())
                    {
                        CheckBox cbPro_Title = new CheckBox();
                        CheckBox cbPosition = new CheckBox();

                        cbPro_Title.Foreground = brush;
                        cbPro_Title.FontSize = 16;
                        cbPro_Title.Margin = thick1;
                        cbPro_Title.Content = rdr["Pro_Title"].ToString();

                        cbPosition.Foreground = brush;
                        cbPosition.FontSize = 16;
                        cbPosition.Margin = thick1;
                        cbPosition.Content = rdr["Position"].ToString();

                        spPro_Title.Children.Add(cbPro_Title);
                        spPosition.Children.Add(cbPosition);
                    }
                }
            }
            #endregion

            #region 模板信息
            using (OleDbDataReader rdr = TplSQL.QueryByTpl_name(tplInfo))
            {
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        Expander epd = new Expander();
                        epd.Header = commonControl.SetExpanderHeader("开会通知");
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

                        grid = commonControl.SetExpanderContent(rdr, true);
                        grid.Children.Add(btnPreview);
                        grid.Children.Add(btnSend);
                        Grid.SetRow(btnPreview, 3);
                        Grid.SetRow(btnSend, 3);
                        epd.Content = grid;

                        sptpl.Children.Add(epd);
                    }
                }
            }
            #endregion
        }
        void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            int textNum = 0;
            string txtPreview;
            IList<string> list = new List<string>();
            commonControl.GetTpl_text(e, out textNum, out txtPreview);
            if (textNum > 0)
            {
                MessageBox.Show("请填写相关内容！");
                return;
            }
            Button btnPreview = e.Source as Button;
            TextBox tbPreview = VisualTreeHelper.GetChild(btnPreview.Parent as Grid, 1) as TextBox;
            tbPreview.Text = "预览：" + txtPreview;
            tbPreview.Visibility = Visibility.Visible;
        }
        void btnSend_Click(object sender, RoutedEventArgs e)
        {
            int textNum = 0;
            string tpl_text;
            List<string> list = new List<string>();
            List<string> listPro_Title = new List<string>();
            List<string> listPosition = new List<string>();

            commonControl.GetTpl_text(e, out textNum, out tpl_text);
            if (tpl_text.IndexOf("\r\n\r\n提示：") > 0)
            {
                MessageBox.Show("当前无法发送！");
                return;
            }
            if (textNum > 0)
            {
                MessageBox.Show("请填写相关内容！");
                return;
            }

            foreach (CheckBox cbPro_Title in spPro_Title.Children.OfType<CheckBox>())
            {
                if (cbPro_Title.IsChecked == true)
                {
                    listPro_Title.Add(cbPro_Title.Content.ToString());
                }
            }
            foreach (CheckBox cbPosition in spPro_Title.Children.OfType<CheckBox>())
            {
                if (cbPosition.IsChecked == true)
                {
                    listPosition.Add(cbPosition.Content.ToString());
                }
            }
            if (listPro_Title.Count == 0 && listPosition.Count == 0)
            {
                MessageBox.Show("请选择职称！");
                return;
            }
            if (listPro_Title.Count > 0 && listPosition.Count > 0)
            {
                MessageBox.Show("请选择一种类型！");
                return;
            }
            if (listPro_Title.Count > 0)
            {
                for (int i = 0; i < listPro_Title.Count; i++)
                {
                    teacherInfo.Pro_Title = listPro_Title[i].ToString();
                    using (OleDbDataReader reader = TeacherSQL.QueryByPosition(teacherInfo))
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                list.Add(reader["Phone"].ToString());
                            }
                        }
                        else
                        {
                            MessageBox.Show("未查询到相关数据！");
                            return;
                        }
                    }
                }
            }
            if (listPosition.Count > 0)
            {
                for (int i = 0; i < listPosition.Count; i++)
                {
                    teacherInfo.Position = listPosition[i].ToString();
                    using (OleDbDataReader reader = TeacherSQL.QueryByPosition(teacherInfo))
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                list.Add(reader["Phone"].ToString());
                            }
                        }
                        else
                        {
                            MessageBox.Show("未查询到相关数据！");
                            return;
                        }
                    }
                }
            }
            SendProgressBar spb = new SendProgressBar();
            HwndSource winformWindow = HwndSource.FromDependencyObject(this) as HwndSource;
            new WindowInteropHelper(spb) { Owner = winformWindow.Handle };
            //spb.SendSms(list.Count, list, tpl_text);
            //spb.ShowDialog();
        }
    }
}
