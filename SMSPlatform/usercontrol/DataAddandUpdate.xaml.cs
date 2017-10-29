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
using System.Windows.Shapes;

namespace SMSPlatform.usercontrol
{
    /// <summary>
    /// DataAddandUpdate.xaml 的交互逻辑
    /// </summary>
    public partial class DataAddandUpdate : Window
    {
        public bool isAdd { get; set; }
        public DataAddandUpdate()
        {
            InitializeComponent();
        }
        TeacherInfo teacherInfo = new TeacherInfo();
        TeacherSQL teacherSQL = new TeacherSQL();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (isAdd)
            {
                txtWorkID.IsEnabled = true;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TeacherInfo teacherInfo = new TeacherInfo();
            teacherInfo.WorkID = txtWorkID.Text;
            IList<TeacherInfo> lit = teacherSQL.QueryByWorkIDandRealName(teacherInfo);
            teacherInfo.RealName = txtRealName.Text;
            teacherInfo.IDNumber = txtIDNumber.Text;
            teacherInfo.Phone = txtPhone.Text;
            teacherInfo.DepartmentName = txtDepartmentName.Text;
            teacherInfo.Position = txtPosition.Text;
            teacherInfo.Pro_Title = txtPro_Title.Text;
            if (txtWorkID.Text != "" && txtRealName.Text != "")
            {
                if (isAdd)
                {
                    if (lit.Count > 0)
                    {
                        MessageBox.Show("信息已存在！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    if (teacherSQL.Insert(teacherInfo) > 0)
                    {
                        MessageBox.Show("添加成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("添加失败，请重试！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    if (teacherSQL.Update(teacherInfo) > 0)
                    {
                        MessageBox.Show("修改成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("修改失败，请重试！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("请输入相关内容！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void Load(string workID)
        {
            teacherInfo.WorkID = workID;
            IList<TeacherInfo> list = teacherSQL.QueryByWorkIDandRealName(teacherInfo);
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    txtWorkID.Text = list[i].WorkID;
                    txtDepartmentName.Text = list[i].DepartmentName;
                    txtRealName.Text = list[i].RealName;
                    txtPhone.Text = list[i].Phone;
                    txtIDNumber.Text = list[i].IDNumber;
                    txtPosition.Text = list[i].Position;
                    txtPro_Title.Text = list[i].Pro_Title;
                }
            }
            else
            {
                MessageBox.Show("无相关数据，请重试！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
