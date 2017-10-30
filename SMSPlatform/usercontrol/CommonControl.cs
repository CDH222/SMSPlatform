using SMSPlatform.common;
using SMSPlatform.Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;
using Yunpian.lib;
using Yunpian.model;

namespace SMSPlatform.usercontrol
{
    public class CommonControl
    {
        Brush brush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        Thickness thick1 = new Thickness(35, 0, 35, 0);
        Thickness thick2 = new Thickness(0, 20, 0, 0);
        Thickness thick3 = new Thickness(0, 0, 30, 0);
        Thickness thick4 = new Thickness(35, 0, 0, 0);
        Thickness thick5 = new Thickness(35, 20, 35, 0);
        int i = 1;

        /// <summary>
        /// 设置ExpanderHeader
        /// </summary>
        /// <param name="txtHeader"></param>
        /// <param name="tpl_id"></param>
        /// <param name="isShow">是否显示已选择</param>
        /// <returns></returns>
        public StackPanel SetExpanderHeader(string txtHeader, bool isCheck, bool isShow)
        {
            StackPanel spHeader = new StackPanel();
            TextBlock tbHeader = new TextBlock();

            spHeader.Orientation = Orientation.Horizontal;
            tbHeader.Text = txtHeader + i.ToString();
            tbHeader.FontSize = 16;
            tbHeader.Foreground = brush;
            tbHeader.VerticalAlignment = VerticalAlignment.Center;
            i++;
            spHeader.Children.Add(tbHeader);
            if (isShow)
            {
                if (isCheck)
                {
                    TextBlock tb = new TextBlock();
                    tb.Text = "当前模板已选择！";
                    tb.Margin = new Thickness(100, 0, 0, 0);
                    tb.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    spHeader.Children.Add(tb);
                }
            }
            return spHeader;
        }
        /// <summary>
        /// 设置ExpanderContent
        /// </summary>
        /// <param name="tpl_id"></param>
        /// <param name="isShow">是否显示变量输入框</param>
        /// <returns></returns>
        public Grid SetExpanderContent(string tpl_id,string name, bool isShow)
        {
            TextBlock tbID = new TextBlock();
            TextBox txt = new TextBox();
            Label lbContent = new Label();
            Grid gdContent = new Grid();
            WrapPanel wp = new WrapPanel();
            Grid gdtxt = new Grid();

            RowDefinition row0 = new RowDefinition();
            RowDefinition row1 = new RowDefinition();
            RowDefinition row2 = new RowDefinition();
            RowDefinition row3 = new RowDefinition();
            RowDefinition row4 = new RowDefinition();
            gdContent.RowDefinitions.Add(row0);
            gdContent.RowDefinitions.Add(row1);
            gdContent.RowDefinitions.Add(row2);
            gdContent.RowDefinitions.Add(row3);
            gdContent.RowDefinitions.Add(row4);

            string content = GetTplContent(tpl_id);

            lbContent.Content = content;
            lbContent.Visibility = Visibility.Collapsed;

            txt.Text = content;
            txt.Height = 100;
            txt.FontSize = 16;
            txt.IsReadOnly = true;
            txt.Margin = thick1;
            txt.TextWrapping = TextWrapping.Wrap;
            txt.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            if (isShow)
            {
                wp.Orientation = Orientation.Horizontal;
                wp.HorizontalAlignment = HorizontalAlignment.Center;
                wp.Margin = thick4;

                for (int i = 1; i <= Regex.Matches(content, @"#").Count / 2; i++)
                {
                    StackPanel sp = new StackPanel();
                    sp.VerticalAlignment = VerticalAlignment.Center;
                    sp.HorizontalAlignment = HorizontalAlignment.Center;
                    sp.Orientation = Orientation.Horizontal;
                    sp.Margin = thick2;
                    TextBlock tb = new TextBlock();
                    TextBox txtb = new TextBox();

                    tb.Text = GetVariable(content, name, i) + "：  ";
                    tb.FontSize = 16;
                    tb.Foreground = brush;
                    tb.VerticalAlignment = VerticalAlignment.Center;

                    txtb.Width = 200;
                    txtb.Height = 30;
                    txtb.FontSize = 16;
                    txtb.TextWrapping = TextWrapping.Wrap;
                    txtb.VerticalContentAlignment = VerticalAlignment.Center;
                    txtb.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                    txtb.Margin = thick3;

                    sp.Children.Add(tb);
                    sp.Children.Add(txtb);
                    wp.Children.Add(sp);
                }
            }

            tbID.Text = tpl_id;
            tbID.Visibility = Visibility.Collapsed;

            gdContent.Children.Add(lbContent);
            gdContent.Children.Add(txt);
            gdContent.Children.Add(wp);
            gdContent.Children.Add(tbID);

            Grid.SetRow(lbContent, 0);
            Grid.SetRow(txt, 1);
            Grid.SetRow(wp, 2);
            Grid.SetRow(tbID, 4);
            return gdContent;
        }
        /// <summary>
        /// 获取替换后的内容
        /// </summary>
        public void GetTpl_text(RoutedEventArgs e, out int textNum, out string tpl_text)
        {
            textNum = 0;
            IList<string> txtContent2 = new List<string>();
            TextBox tb = new TextBox();
            Button btn = e.Source as Button;
            tpl_text = (VisualTreeHelper.GetChild(btn.Parent as Grid, 0) as Label).Content.ToString();
            WrapPanel wp = VisualTreeHelper.GetChild(btn.Parent as Grid, 2) as WrapPanel;
            foreach (StackPanel sp in wp.Children)
            {
                foreach (UIElement element in sp.Children)
                {
                    if (element is TextBox)
                    {
                        tb = element as TextBox;
                        if (tb.Text == "")
                        {
                            textNum++;
                            break;
                        }
                        txtContent2.Add(tb.Text);
                    }
                }
            }
            Regex regex = new Regex("#.*?#"); // 定义一个Regex对象实例
            for (int i = 0; i < txtContent2.Count; i++)
            {
                Match match = regex.Match(tpl_text); // 在字符串中匹配
                if (match.Success)
                {
                    tpl_text = tpl_text.Replace(match.Value, txtContent2[i]);
                }
            }
        }
        /// <summary>
        /// 获取变量名
        /// </summary>
        private string GetVariable(string content,string name, int i)
        {
            string match, result = null;
            XmlDocument doc = new XmlDocument();
            doc.Load("xml/config.xml");
            XmlNodeList nodes = doc.SelectNodes("/config/variables/variable[@name='" + name + "']");
            
            Regex regex = new Regex("#.*?#"); // 定义一个Regex对象实例
            MatchCollection Matches = Regex.Matches(content, regex.ToString(), RegexOptions.ExplicitCapture);
            match = Matches[i - 1].Value.Replace('#', ' ');

            foreach (XmlNode node in nodes)
            {
                if (node.SelectSingleNode(match).InnerText != null)
                {
                    result = node.SelectSingleNode(match).InnerText;
                }
                else
                {
                    result = match;
                }
            }
            return result;
        }
        /// <summary>
        /// 获取模板信息
        /// </summary>
        public string GetTplContent(string tpl_id)
        {
            TplOperator tpl = new TplOperator(YunpianConfig.GetConfig());
            Dictionary<string, string> data = new Dictionary<string, string>();
            Result result = null;
            string content = null;

            data.Clear();
            data.Add("tpl_id", tpl_id);
            result = tpl.get(data);
            string[] responseText = result.responseText.Split(',');
            if (responseText[2].Split('\"')[3] == "CHECKING")
            {
                content = responseText[1].Split('\"')[3] + "\r\n\r\n" + "提示：正在审核中，请稍后。。。。。。";
            }
            else if (responseText[2].Split('\"')[3] == "FAIL")
            {
                content = responseText[1].Split('\"')[3] + "\r\n\r\n" + "提示：审核失败，请在模板管理中修改。原因：" + responseText[3].Split('\"')[3];
            }
            else if (responseText[2].Split('\"')[3] == "SUCCESS")
            {
                content = responseText[1].Split('\"')[3];
            }
            return content;
        }
    }
}
