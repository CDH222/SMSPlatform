using SMSPlatform.Model;
using SMSPlatform.SQL;
using System;
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
        TeacherInfo teacherInfo = new TeacherInfo();
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DataLoade(TeacherSQL.QueryAll_DataTable());
        }

        private void btnQuery(object sender, RoutedEventArgs e)
        {
            DataTable table = new DataTable();
            try
            {
                if (cbCondition.SelectedIndex == 0)
                {
                    MessageBox.Show("请选择查询类型！");
                    return;
                }
                if (cbCondition.SelectedIndex == 1)
                {
                    teacherInfo.WorkID = txtCondition.Text;
                    using (OleDbDataReader reader = TeacherSQL.QueryByWorkIDandRealName(teacherInfo))
                    {
                        table.Load(reader);
                        DataLoade(table);
                    }
                }
                if (cbCondition.SelectedIndex == 2)
                {
                    teacherInfo.RealName = txtCondition.Text;
                    using (OleDbDataReader reader = TeacherSQL.QueryByWorkIDandRealName(teacherInfo))
                    {
                        table.Load(reader);
                        DataLoade(table);
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            table.Dispose();
        }

        private void btnAdd(object sender, RoutedEventArgs e)
        {
            try
            {
                DataAddandUpdate dau = new DataAddandUpdate();
                dau.Title = "数据添加";
                Uri iconUri = new Uri("images/add.ico", UriKind.Relative);
                dau.Icon = BitmapFrame.Create(iconUri);
                dau.isAdd = true;
                dau.ShowDialog();
                DataLoade(TeacherSQL.QueryAll_DataTable());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnUpdate(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView b = (DataRowView)grid1.SelectedItem;
                if (b == null)
                {
                    MessageBox.Show("请选择数据！");
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
                DataLoade(TeacherSQL.QueryAll_DataTable());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void btnDelete(object sender, RoutedEventArgs e)
        {
            try
            {
                TeacherInfo teacherInfo = new TeacherInfo();
                DataRowView b = (DataRowView)grid1.SelectedItem;
                if (b == null)
                {
                    MessageBox.Show("请选择数据！");
                    return;
                }
                string workID = b["WorkID"].ToString();

                teacherInfo.WorkID = workID;
                if (TeacherSQL.Delete(teacherInfo) > 0)
                {
                    MessageBox.Show("删除成功！");
                    DataLoade(TeacherSQL.QueryAll_DataTable());
                }
                else
                {
                    MessageBox.Show("删除失败，请重试！");
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        private void btnInto(object sender, RoutedEventArgs e)
        {
            try
            {
                ProgressBar pb = new ProgressBar();
                HwndSource winformWindow = HwndSource.FromDependencyObject(this) as HwndSource;
                new WindowInteropHelper(pb) { Owner = winformWindow.Handle };
                pb.ShowDialog();
                DataLoade(TeacherSQL.QueryAll_DataTable());
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void btnRefresh(object sender, RoutedEventArgs e)
        {
            DataLoade(TeacherSQL.QueryAll_DataTable());
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
