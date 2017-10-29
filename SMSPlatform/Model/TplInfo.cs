using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSPlatform.Model
{
    public class TplInfo
    {
        string _tpl_id;
        string _tpl_name;
        bool _isCheck;

        public string Tpl_id
        {
            get { return _tpl_id; }
            set { _tpl_id = value; }
        }
        public string Tpl_name
        {
            get { return _tpl_name; }
            set { _tpl_name = value; }
        }
        public bool IsCheck
        {
            get { return _isCheck; }
            set { _isCheck = value; }
        }
    }
}
