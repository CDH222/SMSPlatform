using SMSPlatform.Model;
using SMSPlatform.SQL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
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
    /// ImportData.xaml 的交互逻辑
    /// </summary>
    public partial class ImportData : UserControl
    {
        public ImportData()
        {
            InitializeComponent();
        }
        TeacherInfo teacherInfo;
        TeacherSQL teacherSQL = new TeacherSQL();
        DataTable table = new DataTable();

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            teacherInfo = new TeacherInfo();
            DataLoade(teacherSQL.Query_DataTable(teacherInfo));
        }

        private void btnQuery(object sender, RoutedEventArgs e)
        {
            teacherInfo = new TeacherInfo();
            if (cbCondition.SelectedIndex == 0)
            {
                MessageBox.Show("请选择查询类型！","提示",MessageBoxButton.OK,MessageBoxImage.Information);
                return;
            }
            switch (cbCondition.SelectedIndex)
            {
                case 1:
                    teacherInfo.WorkID = txtCondition.Text;
                    table = teacherSQL.Query_DataTable(teacherInfo);
                    DataLoade(table);
                    break;
                case 2:
                    teacherInfo.RealName = txtCondition.Text;
                    table = teacherSQL.Query_DataTable(teacherInfo);
                    DataLoade(table);
                    break;
                default:
                    DataLoade(table);
                    break;
            }
            table.Dispose();
        }

        private void btnAdd(object sender, RoutedEventArgs e)
        {
            teacherInfo = new TeacherInfo();
            DataAddandUpdate dau = new DataAddandUpdate();
            dau.Title = "数据添加";
            Uri iconUri = new Uri("images/add.ico", UriKind.Relative);
            dau.Icon = BitmapFrame.Create(iconUri);
            dau.isAdd = true;
            dau.ShowDialog();
            DataLoade(teacherSQL.Query_DataTable(teacherInfo));
        }

        private void btnUpdate(object sender, RoutedEventArgs e)
        {
            teacherInfo = new TeacherInfo();
            DataRowView b = (DataRowView)grid1.SelectedItem;
            if (b == null)
            {
                MessageBox.Show("请选择数据！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            string workID = b["WorkID"].ToString();
            DataAddandUpdate dau = new DataAddandUpdate();
            dau.Title = "数据更新";
            Uri iconUri = new Uri("images/Update.ico", UriKind.Relative);
            dau.Icon = BitmapFrame.Create(iconUri);
            dau.isAdd = false;
            dau.Load(workID);
            dau.ShowDialog();
            DataLoade(teacherSQL.Query_DataTable(teacherInfo));
        }
        private void btnDelete(object sender, RoutedEventArgs e)
        {
            teacherInfo = new TeacherInfo();
            DataRowView b = (DataRowView)grid1.SelectedItem;
            if (b == null)
            {
                MessageBox.Show("请选择数据！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (MessageBox.Show("确认删除！", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                string workID = b["WorkID"].ToString();
                if (teacherSQL.Delete(workID) > 0)
                {
                    MessageBox.Show("删除成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    DataLoade(teacherSQL.Query_DataTable(teacherInfo));
                }
                else
                {
                    MessageBox.Show("删除失败，请重试！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        private void btnInto(object sender, RoutedEventArgs e)
        {
            teacherInfo = new TeacherInfo();
            ProgressBar pb = new ProgressBar();
            HwndSource winformWindow = HwndSource.FromDependencyObject(this) as HwndSource;
            new WindowInteropHelper(pb) { Owner = winformWindow.Handle };
            pb.ShowDialog();
            DataLoade(teacherSQL.Query_DataTable(teacherInfo));
        }

        private void btnRefresh(object sender, RoutedEventArgs e)
        {
            teacherInfo = new TeacherInfo();
            DataLoade(teacherSQL.Query_DataTable(teacherInfo));
        }
        //加载
        public void DataLoade(DataTable table)
        {
            grid1.RowHeight = 25;
            grid1.ItemsSource = null;
            gridpage.ShowPages(grid1, table, 30);
        }
    }
}
