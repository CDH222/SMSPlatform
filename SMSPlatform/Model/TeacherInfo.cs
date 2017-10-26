using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSPlatform.Model
{
    public class TeacherInfo
    {
        string _departmentName;
        string _workID;
        string _realName;
        string _IDNumber;
        string _phone;
        string _position;
        string _pro_Title;



        public string DepartmentName
        {
            get { return _departmentName; }
            set { _departmentName = value; }
        }
        public string WorkID
        {
            get { return _workID; }
            set { _workID = value; }
        }
        public string RealName
        {
            get { return _realName; }
            set { _realName = value; }
        }
        public string IDNumber
        {
            get { return _IDNumber; }
            set { _IDNumber = value; }
        }
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }
        public string Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public string Pro_Title
        {
            get { return _pro_Title; }
            set { _pro_Title = value; }
        }
    }
}
