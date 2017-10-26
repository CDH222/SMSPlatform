using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSPlatform.Model
{
    public class TplInfo
    {
        int _tpl_id;
        int _tpl_name;

        public int Tpl_id
        {
            get { return _tpl_id; }
            set { _tpl_id = value; }
        }
        public int Tpl_name
        {
            get { return _tpl_name; }
            set { _tpl_name = value; }
        }
    }
}
