using SMSPlatform.Model;
using SMSPlatform.SQL;
using SMSPlatform.usercontrol;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Yunpian.lib;
using Yunpian.model;

namespace SMSPlatform.common
{
    public class SendBirthdaySMS
    {
        TeacherSQL teacherSQL = new TeacherSQL();
        TplSQL tplSQL = new TplSQL();
        CommonControl comm = new CommonControl();
        public void Send()
        {
            SmsOperator sms = new SmsOperator(YunpianConfig.GetConfig());
            Dictionary<string, string> data = new Dictionary<string, string>();
            Result result = null;
            string tpl_value;

            IList<TeacherInfo> list = teacherSQL.QueryByBirthday();
            string tpl_id = tplSQL.QueryisCheckID(true);
            tpl_value = comm.GetTplContent(tpl_id);

            if (list.Count > 0)
            {
                // 发送模板短信
                for (int i = 0; i < list.Count; i++)
                {
                    Regex regex = new Regex("#.*?#"); // 定义一个Regex对象实例
                    Match match = regex.Match(tpl_value); // 在字符串中匹配
                    if (match.Success)
                    {
                        tpl_value = tpl_value.Replace(match.Value, list[i].RealName);
                    }

                    data.Clear();
                    data.Add("mobile", list[i].Phone);
                    data.Add("text", tpl_value);
                    result = sms.singleSend(data);
                    Thread.Sleep(200);
                }
            }
        }
    }
}
