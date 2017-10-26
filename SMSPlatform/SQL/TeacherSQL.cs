using SMSPlatform.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSPlatform.SQL
{
    public class TeacherSQL
    {
        private const string INSERT = "insert into TeacherInfo (DepartmentName,WorkID,RealName,IDNumber,Phone,[Position],Pro_Title) values (@DepartmentName,@WorkID,@RealName,@IDNumber,@Phone,@Position,@Pro_Title)";
        private const string DELETE = "delete from TeacherInfo where WorkID=@WorkID";
        private const string UPDATE = "update TeacherInfo set RealName=@RealName,IDNumber=@IDNumber,Phone=@Phone,[Position]=@Position where WorkID=@WorkID";
        private const string SELECTALL = "select * from TeacherInfo";
        private const string SELECTByBirthday = "select * from TeacherInfo where mid(IDNumber, 11, 4) = @DateTime";

        public static int Insert(TeacherInfo teacherInfo)
        {
            OleDbParameter[] parm = new OleDbParameter[] { 
                  new OleDbParameter("@DepartmentName",teacherInfo.DepartmentName),
                  new OleDbParameter("@WorkID",teacherInfo.WorkID),
                  new OleDbParameter("@RealName",teacherInfo.RealName),
                  new OleDbParameter("@IDNumber",teacherInfo.IDNumber),
                  new OleDbParameter("@Phone",teacherInfo.Phone),
                  new OleDbParameter("@Position",teacherInfo.Position),
                  new OleDbParameter("@Pro_Title",teacherInfo.Pro_Title)
                };
            return SQLHelper.ExecuteNonQuery(INSERT, parm);
        }
        public static int Delete(TeacherInfo teacherInfo)
        {
            OleDbParameter[] parm = new OleDbParameter[] {
                  new OleDbParameter("@WorkID",teacherInfo.WorkID)
                };
            return SQLHelper.ExecuteNonQuery(DELETE, parm);
        }
        public static int Update(TeacherInfo teacherInfo)
        {
            OleDbParameter[] parm = new OleDbParameter[] { 
                  new OleDbParameter("@RealName",teacherInfo.RealName),
                  new OleDbParameter("@IDNumber",teacherInfo.IDNumber),
                  new OleDbParameter("@Phone",teacherInfo.Phone),
                  new OleDbParameter("@Position",teacherInfo.Position),
                  new OleDbParameter("@WorkID",teacherInfo.WorkID)
                };
            return SQLHelper.ExecuteNonQuery(UPDATE, parm);
        }
        public static OleDbDataReader QueryAll()
        {
            return SQLHelper.ExecuteReader(SELECTALL);
        }

        public static DataTable QueryAll_DataTable()
        {
            return SQLHelper.ExecuteDataTable(SELECTALL);
        }

        public static OleDbDataReader QueryByWorkIDandRealName(TeacherInfo teacherInfo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append("select * from TeacherInfo where 1=1");
            if (teacherInfo.WorkID != null && teacherInfo.WorkID != "")
            {
                sb.AppendFormat(" and WorkID='{0}'", teacherInfo.WorkID);
            }
            if (teacherInfo.RealName != null && teacherInfo.RealName != "")
            {
                sb.AppendFormat(" and RealName='{0}'", teacherInfo.RealName);
            }
            return SQLHelper.ExecuteReader(sb.ToString());
        }
        public static OleDbDataReader QueryByBirthday()
        {
            OleDbParameter[] parm = new OleDbParameter[] { 
                  new OleDbParameter("@DateTime",DateTime.Today.ToString("MMdd"))
                };
            return SQLHelper.ExecuteReader(SELECTByBirthday, parm);
        }
        public static OleDbDataReader QueryByPosition(TeacherInfo teacherInfo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append("select * from TeacherInfo where 1=1");
            if (teacherInfo.Pro_Title != null && teacherInfo.Pro_Title != "")
            {
                sb.AppendFormat(" and Pro_Title='{0}'", teacherInfo.Pro_Title);
            }
            if (teacherInfo.Position != null && teacherInfo.Position != "")
            {
                sb.AppendFormat(" and [Position]='{0}'", teacherInfo.Position);
            }
            return SQLHelper.ExecuteReader(sb.ToString());
        }
    }
}
