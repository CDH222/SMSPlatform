using SMSPlatform.common;
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
using Yunpian.lib;
using Yunpian.model;

namespace SMSPlatform.usercontrol
{
    /// <summary>
    /// TemplateManager.xaml 的交互逻辑
    /// </summary>
    public partial class TemplateManager : UserControl
    {
        public TemplateManager()
        {
            InitializeComponent();
        }
        TplInfo tplInfo = new TplInfo();
        TplOperator tpl = new TplOperator(YunpianConfig.GetConfig());
        Dictionary<string, string> data = new Dictionary<string, string>();
        Result result = null;
        string index = null;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                btnUpdate.IsEnabled = false;
                data.Clear();
                cbtpl_id.Items.Clear();
                //获取模板编号
                result = tpl.get(data);
                for (int i = 0; i < result.data.Count; i++)
                {
                    cbtpl_id.Items.Add(result.data[i]["tpl_id"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cbtpl_id_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                btnUpdate.IsEnabled = true;
                txtContent.IsEnabled = true;
                index = cbtpl_id.SelectedItem.ToString().Trim();
                tplInfo.Tpl_id = int.Parse(index);
                using (OleDbDataReader rdr = TplSQL.Query(tplInfo))
                {
                    if (!rdr.HasRows)
                    {
                        cbtpl_name.SelectedIndex = 0;
                    }
                    else
                    {
                        if (rdr.Read())
                        {
                            cbtpl_name.SelectedIndex = int.Parse(rdr["tpl_name"].ToString());
                        }
                    }
                }
                data.Clear();
                data.Add("tpl_id", index);
                result = tpl.get(data);
                string[] responseText = result.responseText.Split(',');
                txtContent.Text = responseText[1].Split('\"')[3];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (cbtpl_name.SelectedIndex == 0)
            {
                MessageBox.Show("请选择模板类型！");
                return;
            }
            index = cbtpl_id.SelectedItem.ToString().Trim();
            tplInfo.Tpl_id = int.Parse(index);
            tplInfo.Tpl_name = cbtpl_name.SelectedIndex;
            if (!TplSQL.Query(tplInfo).HasRows)
            {
                TplSQL.Insert(tplInfo);
            }
            else
            {
                TplSQL.Update(tplInfo);
            }
            string tpl_content = txtContent.Text;
            data.Clear();
            data.Add("tpl_id", index);
            data.Add("tpl_content", tpl_content);
            result = tpl.upd(data);
            MessageBox.Show("修改成功，请等待审核！");
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (txtAddContent.Text == "")
            {
                MessageBox.Show("请填写内容!");
                return;
            }
            string tpl_content = txtAddContent.Text;
            data.Clear();
            data.Add("tpl_content", tpl_content);
            result = tpl.add(data);
            if (result.success)
            {
                MessageBox.Show("添加成功，请等待审核！");
            }
        }
    }
}
