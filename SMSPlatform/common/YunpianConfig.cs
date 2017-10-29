using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yunpian.conf;

namespace SMSPlatform.common
{
    public class YunpianConfig
    {
        static readonly string apikey = ConfigurationManager.AppSettings["apikey"];
        public static Config GetConfig()
        {
            //设置apikey
            Config config = new Config(apikey);
            return config;
        }
    }
}
