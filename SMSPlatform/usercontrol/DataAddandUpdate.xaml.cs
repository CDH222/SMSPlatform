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
            teacherInfo.RealName = txtRealName.Text;
            teacherInfo.IDNumber = txtIDNumber.Text;
            teacherInfo.Phone = txtPhone.Text;
            teacherInfo.Position = txtPosition.Text;
            teacherInfo.WorkID = txtWorkID.Text;
            teacherInfo.Pro_Title = txtPro_Title.Text;
            if (isAdd)
            {
                if (TeacherSQL.Insert(teacherInfo) > 0)
                {
                    MessageBox.Show("添加成功！");
                }
                else
                {
                    MessageBox.Show("添加失败，请重试！");
                }
            }
            else
            {
                if (TeacherSQL.Update(teacherInfo) > 0)
                {
                    MessageBox.Show("修改成功！");
                }
                else
                {
                    MessageBox.Show("修改失败，请重试！");
                }
            }
        }

        public void Load(string workID)
        {
            teacherInfo.WorkID = workID;
            using (OleDbDataReader reader = TeacherSQL.QueryByWorkIDandRealName(teacherInfo))
            {
                if (reader.Read())
                {
                    txtWorkID.Text = reader["WorkID"].ToString();
                    txtRealName.Text = reader["RealName"].ToString();
                    txtPhone.Text = reader["Phone"].ToString();
                    txtIDNumber.Text = reader["IDNumber"].ToString();
                    txtPosition.Text = reader["Position"].ToString();
                    txtPro_Title.Text = reader["Pro_Title"].ToString();
                }
            }
        }
    }
}
