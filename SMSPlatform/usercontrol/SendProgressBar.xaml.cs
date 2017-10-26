using SMSPlatform.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Yunpian.lib;
using Yunpian.model;

namespace SMSPlatform.usercontrol
{
    /// <summary>
    /// SendProgressBar.xaml 的交互逻辑
    /// </summary>
    public partial class SendProgressBar : Window
    {
        TplOperator tpl = new TplOperator(YunpianConfig.GetConfig());
        SmsOperator sms = new SmsOperator(YunpianConfig.GetConfig());
        Dictionary<string, string> data = new Dictionary<string, string>();
        Result result = null;

        public SendProgressBar()
        {
            InitializeComponent();
        }
        public void SendSms(int maximum, IList<string> list, string tpl_text)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = maximum;

            
            ThreadPool.QueueUserWorkItem((a) =>
            {
                //发送短信
                foreach (string phone in list)
                {
                    data.Clear();
                    data.Add("mobile", phone);
                    data.Add("text", tpl_text);
                    result = sms.singleSend(data);
                    progressBar1.Dispatcher.Invoke(() => progressBar1.Value++);
                    Thread.Sleep(100);
                }
                MessageBox.Show("发送成功！");
                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    this.Close();
                }));
            });
        }
    }
}
