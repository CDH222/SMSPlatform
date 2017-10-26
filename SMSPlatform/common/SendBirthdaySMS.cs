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
        public void Send()
        {
            SmsOperator sms = new SmsOperator(YunpianConfig.GetConfig());
            Dictionary<string, string> data = new Dictionary<string, string>();
            Dictionary<string, string> teacherInfo = new Dictionary<string, string>();
            Result result = null;
            string tpl_id = Properties.Settings.Default.tpl_id;

            using (OleDbDataReader reader = TeacherSQL.QueryByBirthday())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        teacherInfo.Add(reader["Phone"].ToString(), reader["RealName"].ToString());
                    }
                }
            }
            if (teacherInfo != null)
            {
                // 发送模板短信
                foreach (KeyValuePair<string, string> kv in teacherInfo)
                {
                    data.Clear();
                    string tpl_value = HttpUtility.UrlEncode("#number#", Encoding.UTF8) + "=" + HttpUtility.UrlEncode(kv.Value, Encoding.UTF8);
                    data.Add("mobile", kv.Key);
                    data.Add("tpl_value", tpl_value);
                    data.Add("tpl_id", tpl_id);
                    result = sms.tplSingleSend(data);
                }
            }
        }
    }
}
