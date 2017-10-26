using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yunpian.conf;

namespace SMSPlatform.common
{
    public class YunpianConfig
    {
        public static Config GetConfig()
        {
            //设置apikey
            Config config = new Config("");
            return config;
        }
    }
}
