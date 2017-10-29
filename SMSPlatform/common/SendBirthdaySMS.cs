using SMSPlatform.Model;
using SMSPlatform.SQL;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Yunpian.lib;
using Yunpian.model;

namespace SMSPlatform.common
{
    public class SendBirthdaySMS
    {
        TeacherSQL teacherSQL = new TeacherSQL();
        public void Send()
        {
            SmsOperator sms = new SmsOperator(YunpianConfig.GetConfig());
            Dictionary<string, string> data = new Dictionary<string, string>();
            Result result = null;
            string tpl_id = null;

            IList<TeacherInfo> list = teacherSQL.QueryByBirthday();
            if (list.Count > 0)
            {
                // 发送模板短信
                for (int i = 0; i < list.Count; i++)
                {
                    data.Clear();
                    string tpl_value = HttpUtility.UrlEncode("#number#", Encoding.UTF8) + "=" + HttpUtility.UrlEncode(list[i].RealName, Encoding.UTF8);
                    data.Add("mobile", list[i].Phone);
                    data.Add("tpl_value", tpl_value);
                    data.Add("tpl_id", tpl_id);
                    result = sms.tplSingleSend(data);
                }
            }
        }
    }
}
