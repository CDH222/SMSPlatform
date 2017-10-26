using Microsoft.Win32;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SMSPlatform.common;
using SMSPlatform.Model;
using SMSPlatform.SQL;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// ProgressBar.xaml 的交互逻辑
    /// </summary>
    public partial class ProgressBar : Window
    {
        public ProgressBar()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem((a) =>
            {
                TeacherInfo teacherInfo = new TeacherInfo();
                OpenFileDialog fdlg = new OpenFileDialog();
                fdlg.Title = "请选择要打开的文件";
                fdlg.InitialDirectory = @"C:\";//默认C盘
                fdlg.Filter = "Excel文件|*.xls;*.xlsx";

                if (fdlg.ShowDialog() == true)
                {
                    progressBar1.Dispatcher.Invoke(() => progressBar1.Visibility = Visibility.Visible);
                    string path = fdlg.FileName;
                    //从Excel中读取数据
                    using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        IWorkbook workbook = null;  //新建IWorkbook对象
                        if (path.IndexOf(".xlsx") > 0) // 2007版本  
                        {
                            workbook = new XSSFWorkbook(fileStream);  //xlsx数据读入workbook  
                        }
                        else if (path.IndexOf(".xls") > 0) // 2003版本  
                        {
                            workbook = new HSSFWorkbook(fileStream);  //xls数据读入workbook  
                        }
                        //获取wk中的sheet
                        ISheet sheet = workbook.GetSheetAt(0);
                        IRow currentRow;  //新建当前工作表行数据
                        IList<ICell> listCells = new List<ICell>(); //list中保存当前行的所有的单元格内容
                        progressBar1.Dispatcher.Invoke(() => progressBar1.Value = 0);
                        progressBar1.Dispatcher.Invoke(() => progressBar1.Maximum = sheet.LastRowNum);
                        //遍历说有行
                        for (int r = 1; r <= sheet.LastRowNum; r++)
                        {
                            //获取每一行
                            currentRow = sheet.GetRow(r);
                            
                            teacherInfo.WorkID = currentRow.GetCell(0).ToString();
                            teacherInfo.RealName = currentRow.GetCell(1)== null ? "" : currentRow.GetCell(1).ToString();
                            teacherInfo.IDNumber = currentRow.GetCell(2)== null ? "" : currentRow.GetCell(2).ToString();
                            teacherInfo.Phone = currentRow.GetCell(3) == null ? "" : currentRow.GetCell(3).ToString();
                            teacherInfo.Position = currentRow.GetCell(4) == null ? "" : currentRow.GetCell(4).ToString();
                            using (OleDbDataReader reader = TeacherSQL.QueryByWorkIDandRealName(teacherInfo))
                            {
                                if (reader.HasRows)
                                {
                                    TeacherSQL.Update(teacherInfo);
                                }
                                else
                                {
                                    //执行sql语句
                                    TeacherSQL.Insert(teacherInfo);
                                }
                            }
                            progressBar1.Dispatcher.Invoke(() => progressBar1.Value++);
                        }
                    }
                    MessageBox.Show("数据导入成功！");
                }
                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    this.Close();
                }));
            });
        }
    }
}
