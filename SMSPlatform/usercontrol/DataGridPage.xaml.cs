using SMSPlatform.common;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// DataGridPage.xaml 的交互逻辑
    /// </summary>
    public partial class DataGridPage : UserControl
    {
        public DataGridPage()
        {
            InitializeComponent();
        }
        private DataTable _dt = new DataTable();
        /// <summary>
        /// 每页显示多少条
        /// </summary>
        private int pageNum = 30;
        /// <summary>
        /// 当前是第几页
        /// </summary>
        private int pIndex = 1;
        /// <summary>
        /// 对象
        /// </summary>
        private DataGrid dataGrid = new DataGrid();
        /// <summary>
        /// 最大页数
        /// </summary>
        private int MaxIndex = 1;
        /// <summary>
        /// 一共多少条
        /// </summary>
        private int allNum = 0;

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="grd"></param>
        /// <param name="dtt"></param>
        /// <param name="Num"></param>
        public void ShowPages(DataGrid grid, DataTable table, int Num)
        {
            _dt = table;
            dataGrid = grid;
            pageNum = Num;
            pIndex = 1;

            SetMaxIndex();
            ReadDataTable();
            table.Dispose();
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void ReadDataTable()
        {
            try
            {
                txtCurrentPage.Text = pIndex.ToString();
                TotalCount.Text = MaxIndex + "页";
                pageSize.Text = allNum.ToString();
                DataTable tmpTable = new DataTable();
                tmpTable = this._dt.Clone();
                int first = this.pageNum * (this.pIndex - 1);
                first = (first > 0) ? first : 0;
                //如何总数量大于每页显示数量
                if (this._dt.Rows.Count >= this.pageNum * this.pIndex)
                {
                    for (int i = first; i < pageNum * this.pIndex; i++)
                        tmpTable.ImportRow(this._dt.Rows[i]);
                }
                else
                {
                    for (int i = first; i < this._dt.Rows.Count; i++)
                        tmpTable.ImportRow(this._dt.Rows[i]);
                }
                this.dataGrid.ItemsSource = tmpTable.DefaultView;
                tmpTable.Dispose();
            }
            catch
            {
                MessageBox.Show("错误");
            }
            finally
            {
                DisplayPagingInfo();
            }

        }

        /// <summary>
        /// 每页显示等数据
        /// </summary>
        private void DisplayPagingInfo()
        {
            if (this.pIndex == 1)
            {
                this.btnPrev.IsEnabled = false;
                this.btnFirst.IsEnabled = false;
            }
            else
            {
                this.btnPrev.IsEnabled = true;
                this.btnFirst.IsEnabled = true;
            }
            if (this.pIndex == this.MaxIndex)
            {
                this.btnNext.IsEnabled = false;
                this.btnLast.IsEnabled = false;
            }
            else
            {
                this.btnNext.IsEnabled = true;
                this.btnLast.IsEnabled = true;
            }
            TotalCount.Text = MaxIndex + "页";
            int first = (this.pIndex - 4) > 0 ? (this.pIndex - 4) : 1;
            int last = (first + 9) > this.MaxIndex ? this.MaxIndex : (first + 9);
        }

        /// <summary>
        /// 设置最多大页面
        /// </summary>
        private void SetMaxIndex()
        {
            //多少页
            MaxIndex = _dt.Rows.Count / pageNum + 1;
            allNum = _dt.Rows.Count;
        }

        private void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            this.pIndex = 1;
            ReadDataTable();
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (this.pIndex <= 1)
                return;
            this.pIndex--;
            ReadDataTable();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (this.pIndex >= this.MaxIndex)
                return;
            this.pIndex++;
            ReadDataTable();
        }

        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            this.pIndex = this.MaxIndex;
            ReadDataTable();
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            int result;
            if (this.txtCurrentPage.Text != null && txtCurrentPage.Text != "")
            {
                if (int.TryParse(txtCurrentPage.Text, out result))
                {
                    this.pIndex = Convert.ToInt32(txtCurrentPage.Text);
                    ReadDataTable();
                }
                else
                {
                    MessageBox.Show("输入数字格式错误！");
                }
            }
        }
    }
}
