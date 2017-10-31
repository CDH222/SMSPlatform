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
using System.Xml;
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
        TplSQL tplSQL = new TplSQL();
        TplOperator tpl = new TplOperator(YunpianConfig.GetConfig());
        Dictionary<string, string> data = new Dictionary<string, string>();
        Result result = null;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string tpl_id = null;
            btnUpdate.IsEnabled = false;
            btnUpdate_Type.IsEnabled = false;
            btnDelete_Delete.IsEnabled = false;
            data.Clear();
            cbtpl_id.Items.Clear();
            //获取模板编号
            result = tpl.get(data);
            for (int i = 0; i < result.data.Count; i++)
            {
                tpl_id = result.data[i]["tpl_id"];
                tplInfo = tplSQL.Query(tpl_id);
                if (tplInfo.Tpl_id == null)
                {
                    tplInfo.Tpl_id = tpl_id;
                    tplInfo.Tpl_name = "";
                    tplInfo.IsCheck = false;
                    tplSQL.Insert(tplInfo);
                }
                cbtpl_id.Items.Add(tpl_id);
                cbtpl_id_Type.Items.Add(tpl_id);
                cbtpl_id_Delete.Items.Add(tpl_id);
            }
            GetVariable();
        }

        #region 模板修改
        private void cbtpl_id_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnUpdate.IsEnabled = true;
            txtContent.IsEnabled = true;
            string tpl_id = cbtpl_id.SelectedItem.ToString().Trim();
            tplInfo = tplSQL.Query(tpl_id);
            if (tplInfo.Tpl_name == "")
            {
                cbtpl_name.Text = "模板类型：无类型！";
            }
            else
            {
                cbtpl_name.Text = "模板类型：" + tplInfo.Tpl_name;
            }
            data.Clear();
            data.Add("tpl_id", tpl_id);
            result = tpl.get(data);
            string[] responseText = result.responseText.Split(',');
            txtContent.Text = responseText[1].Split('\"')[3];
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string tpl_id = cbtpl_id.SelectedItem.ToString().Trim();
            string tpl_content = txtContent.Text;
            data.Clear();
            data.Add("tpl_id", tpl_id);
            data.Add("tpl_content", tpl_content);
            result = tpl.upd(data);
            MessageBox.Show("修改成功，请等待审核！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region 模板类型修改
        private void cbtpl_id_Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnUpdate_Type.IsEnabled = true;
            string tpl_id = cbtpl_id_Type.SelectedItem.ToString();
            tplInfo = tplSQL.Query(tpl_id);
            if (tplInfo.Tpl_name == "")
            {
                cbtpl_name_Type.SelectedIndex = 0;
            }
            else
            {
                cbtpl_name_Type.Text = tplInfo.Tpl_name;
            }
            data.Clear();
            data.Add("tpl_id", tpl_id);
            result = tpl.get(data);
            string[] responseText = result.responseText.Split(',');
            txtContent_Type.Text = responseText[1].Split('\"')[3];
        }
        private void btnUpdate_Type_Click(object sender, RoutedEventArgs e)
        {
            if (cbtpl_name_Type.SelectedIndex == 0)
            {
                MessageBox.Show("请选择模板类型！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            tplInfo.Tpl_id = cbtpl_id_Type.Text;
            tplInfo.Tpl_name = cbtpl_name_Type.Text;
            if (tplSQL.Update(tplInfo) > 0)
            {
                MessageBox.Show("修改成功！","提示",MessageBoxButton.OK,MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("修改失败，请重试！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
        
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (txtAddContent.Text == "")
            {
                MessageBox.Show("请填写内容!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (cbtpl_name_Add.SelectedIndex == 0)
            {
                MessageBox.Show("请选择模板内容!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            string tpl_content = txtAddContent.Text;
            data.Clear();
            data.Add("tpl_content", tpl_content);
            result = tpl.add(data);
            string[] responseText = result.responseText.Split(',');
            tplInfo.Tpl_id = responseText[0].Split(':')[1];
            tplInfo.Tpl_name = cbtpl_name_Add.Text;
            tplSQL.Insert(tplInfo);
            MessageBox.Show("添加成功，请等待审核！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #region 删除模板
        private void cbtpl_id_Delete_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnDelete_Delete.IsEnabled = true;
            string tpl_id = cbtpl_id_Delete.SelectedItem.ToString();
            tplInfo = tplSQL.Query(tpl_id);
            if (tplInfo.Tpl_name == "")
            {
                cbtpl_name_Type.SelectedIndex = 0;
            }
            else
            {
                cbtpl_name_Type.Text = tplInfo.Tpl_name;
            }
            data.Clear();
            data.Add("tpl_id", tpl_id);
            result = tpl.get(data);
            string[] responseText = result.responseText.Split(',');
            txtContent_Delete.Text = responseText[1].Split('\"')[3];
        }

        private void btnDelete_Delete_Click(object sender, RoutedEventArgs e)
        {
            string tpl_id = cbtpl_id_Delete.SelectedItem.ToString();
            if (MessageBox.Show("是否删除模板？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                data.Clear();
                data.Add("tpl_id", tpl_id);
                result = tpl.del(data);
                tplSQL.Delete(tpl_id);
                MessageBox.Show("删除成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion

        void GetVariable()
        {
            string attribute, name, innerText, text = null;
            XmlDocument doc = new XmlDocument();
            doc.Load("xml/config.xml");
            XmlNodeList nodes = doc.SelectNodes("/config/variables/variable");
            foreach (XmlNode node in nodes)
            {
                attribute = node.Attributes["name"].Value;
                text += attribute + "：" + "\n    ";
                foreach (XmlNode item in node)
                {
                    name = item.Name;
                    innerText = item.InnerText;
                    text += innerText + "：" + name + "    ";
                }
                text += "\n";
            }
            tbaddVariable.Text = text;
            tbUpdateVariable.Text = text;
        }
    }
}
