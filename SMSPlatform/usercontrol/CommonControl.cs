using SMSPlatform.common;
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
using Yunpian.lib;
using Yunpian.model;

namespace SMSPlatform.usercontrol
{
    public class CommonControl
    {
        Brush brush = new SolidColorBrush(Color.FromRgb(0, 0, 255));
        Thickness thick1 = new Thickness(35, 0, 35, 0);
        Thickness thick2 = new Thickness(0, 20, 0, 0);
        Thickness thick3 = new Thickness(0, 0, 30, 0);
        Thickness thick4 = new Thickness(35, 0, 0, 0);
        Thickness thick5 = new Thickness(35, 20, 35, 0);
        int i = 1;

        public StackPanel SetExpanderHeader(string txtHeader)
        {
            StackPanel spHeader = new StackPanel();
            TextBlock tbHeader = new TextBlock();

            spHeader.Orientation = Orientation.Horizontal;
            tbHeader.Text = txtHeader + i.ToString();
            tbHeader.FontSize = 16;
            tbHeader.Foreground = brush;
            tbHeader.VerticalAlignment = VerticalAlignment.Center;
            spHeader.Children.Add(tbHeader);
            i++;
            return spHeader;
        }
        public Grid SetExpanderContent(OleDbDataReader rdr,bool isShow)
        {
            string tpl_id = rdr["tpl_id"].ToString();
            TextBlock tbID = new TextBlock();
            TextBox txt = new TextBox();
            TextBox txtPreview = new TextBox();
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
            txt.Text = content;
            txt.Height = 90;
            txt.FontSize = 16;
            txt.IsReadOnly = true;
            txt.Margin = thick1;
            txt.TextWrapping = TextWrapping.Wrap;
            txt.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            txtPreview.Height = 90;
            txtPreview.FontSize = 16;
            txtPreview.IsReadOnly = true;
            txtPreview.Margin = thick5;
            txtPreview.TextWrapping = TextWrapping.Wrap;
            txtPreview.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            txtPreview.Visibility = Visibility.Collapsed;

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

                    tb.Text = GetVariable(content, i) + "：  ";
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
            tbID.Visibility = Visibility.Hidden;

            gdContent.Children.Add(txt);
            gdContent.Children.Add(txtPreview);
            gdContent.Children.Add(wp);
            gdContent.Children.Add(tbID);

            Grid.SetRow(txt, 0);
            Grid.SetRow(txtPreview, 1);
            Grid.SetRow(wp, 2);
            Grid.SetRow(tbID, 4);
            return gdContent;
        }
        public void GetTpl_text(RoutedEventArgs e, out int textNum, out string tpl_text)
        {
            textNum = 0;
            IList<string> txtContent2 = new List<string>();
            TextBox tb = new TextBox();
            Button btn = e.Source as Button;
            tpl_text = (VisualTreeHelper.GetChild(btn.Parent as Grid, 0) as TextBox).Text;
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
        private string GetVariable(string content, int i)
        {
            string result = null;
            Regex regex = new Regex("#.*?#"); // 定义一个Regex对象实例
            MatchCollection Matches = Regex.Matches(content, regex.ToString(), RegexOptions.ExplicitCapture);
            result = Matches[i - 1].Value;
            return result;
        }
        /// <summary>
        /// 获取模板信息
        /// </summary>
        private string GetTplContent(string tpl_id)
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
