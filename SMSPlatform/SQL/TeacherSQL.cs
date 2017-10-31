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
        private const string INSERT = "insert into TeacherInfo(WorkID,DepartmentName,RealName,IDNumber,Phone,Pro_Title,[Position]) values (@WorkID,@DepartmentName,@RealName,@IDNumber,@Phone,@Pro_Title,@Position)";
        private const string DELETE = "delete from TeacherInfo where WorkID=@WorkID";
        private const string UPDATE = "update TeacherInfo set DepartmentName=@DepartmentName,RealName=@RealName,IDNumber=@IDNumber,Phone=@Phone,Pro_Title=@Pro_Title,[Position]=@Position where WorkID=@WorkID";
        private const string SELECTALL = "select * from TeacherInfo where 1=1";
        private const string SELECTByBirthday = "select RealName,Phone from TeacherInfo where mid(IDNumber, 11, 4) = @DateTime";
        private const string SELECT_Pro_TitleorPosition = "select distinct Pro_Title,[Position] from TeacherInfo";

        public int Insert(TeacherInfo teacherInfo)
        {
            OleDbParameter[] parm = new OleDbParameter[] { 
                new OleDbParameter("@WorkID",teacherInfo.WorkID),
                new OleDbParameter("@DepartmentName",teacherInfo.DepartmentName),
                new OleDbParameter("@RealName",teacherInfo.RealName),
                new OleDbParameter("@IDNumber",teacherInfo.IDNumber),
                new OleDbParameter("@Phone",teacherInfo.Phone),
                new OleDbParameter("@Pro_Title",teacherInfo.Pro_Title),
                new OleDbParameter("@Position",teacherInfo.Position)
               };
            return SQLHelper.ExecuteNonQuery(INSERT, parm);
        }
        public int Delete(string workID)
        {
            OleDbParameter[] parm = new OleDbParameter[] {
                  new OleDbParameter("@WorkID",workID)
                };
            return SQLHelper.ExecuteNonQuery(DELETE, parm);
        }
        public int Update(TeacherInfo teacherInfo)
        {
            OleDbParameter[] parm = new OleDbParameter[] { 
                  new OleDbParameter("@DepartmentName",teacherInfo.DepartmentName),
                  new OleDbParameter("@RealName",teacherInfo.RealName),
                  new OleDbParameter("@IDNumber",teacherInfo.IDNumber),
                  new OleDbParameter("@Phone",teacherInfo.Phone),
                  new OleDbParameter("@Pro_Title",teacherInfo.Pro_Title),
                  new OleDbParameter("@Position",teacherInfo.Position),
                  new OleDbParameter("@WorkID",teacherInfo.WorkID)
                };
            return SQLHelper.ExecuteNonQuery(UPDATE, parm);
        }
        public IList<TeacherInfo> QueryAll()
        {
            IList<TeacherInfo> list = new List<TeacherInfo>();
            using (OleDbDataReader read = SQLHelper.ExecuteReader(SELECTALL))
            {
                if (read.HasRows)
                {
                    while (read.Read())
                    {
                        TeacherInfo teacherInfo = new TeacherInfo();
                        teacherInfo.DepartmentName = read["DepartmentName"].ToString();
                        teacherInfo.WorkID = read["WorkID"].ToString();
                        teacherInfo.RealName = read["RealName"].ToString();
                        teacherInfo.IDNumber = read["IDNumber"].ToString();
                        teacherInfo.Phone = read["Phone"].ToString();
                        teacherInfo.Position = read["Position"].ToString();
                        teacherInfo.Pro_Title = read["Pro_Title"].ToString();
                        list.Add(teacherInfo);
                    }
                }
            }
            return list;
        }

        public IList<TeacherInfo> QueryPro_TitleorPosition()
        {
            IList<TeacherInfo> list = new List<TeacherInfo>();
            using (OleDbDataReader read = SQLHelper.ExecuteReader(SELECT_Pro_TitleorPosition))
            {
                if (read.HasRows)
                {
                    while (read.Read())
                    {
                        TeacherInfo teacherInfo = new TeacherInfo();
                        teacherInfo.Pro_Title = read["Pro_Title"].ToString();
                        teacherInfo.Position = read["Position"].ToString();
                        list.Add(teacherInfo);
                    }
                }
            }
            return list;
        }

        public DataTable Query_DataTable(TeacherInfo teacherInfo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append(SELECTALL);
            if (teacherInfo.WorkID != null && teacherInfo.WorkID != "")
            {
                sb.AppendFormat(" and WorkID='{0}'", teacherInfo.WorkID);
            }
            if (teacherInfo.RealName != null && teacherInfo.RealName != "")
            {
                sb.AppendFormat(" and RealName='{0}'", teacherInfo.RealName);
            }
            return SQLHelper.ExecuteDataTable(sb.ToString());
        }

        public IList<TeacherInfo> QueryByWorkIDandRealName(TeacherInfo teacherInfo)
        {
            IList<TeacherInfo> list = new List<TeacherInfo>();
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
            using (OleDbDataReader read = SQLHelper.ExecuteReader(sb.ToString()))
            {
                if (read.HasRows)
                {
                    while (read.Read())
                    {
                        TeacherInfo ti = new TeacherInfo();
                        ti.DepartmentName = read["DepartmentName"].ToString();
                        ti.WorkID = read["WorkID"].ToString();
                        ti.RealName = read["RealName"].ToString();
                        ti.IDNumber = read["IDNumber"].ToString();
                        ti.Phone = read["Phone"].ToString();
                        ti.Position = read["Position"].ToString();
                        ti.Pro_Title = read["Pro_Title"].ToString();
                        list.Add(ti);
                    }
                }
            }
            return list;
        }
        public IList<TeacherInfo> QueryByBirthday()
        {
            IList<TeacherInfo> list = new List<TeacherInfo>();
            OleDbParameter[] parm = new OleDbParameter[] { 
                  new OleDbParameter("@DateTime",DateTime.Today.ToString("MMdd"))
                };
            using (OleDbDataReader read = SQLHelper.ExecuteReader(SELECTByBirthday, parm))
            {
                if (read.HasRows)
                {
                    while (read.Read())
                    {
                        TeacherInfo teacherInfo = new TeacherInfo();
                        teacherInfo.RealName = read["RealName"].ToString();
                        teacherInfo.Phone = read["Phone"].ToString();
                        list.Add(teacherInfo);
                    }
                }
            }
            return list;
        }
        public IList<TeacherInfo> QueryByPosition(TeacherInfo teacherInfo)
        {
            IList<TeacherInfo> list = new List<TeacherInfo>();
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append("select Phone from TeacherInfo where 1=1");
            if (teacherInfo.Pro_Title != null && teacherInfo.Pro_Title != "")
            {
                sb.AppendFormat(" and Pro_Title='{0}'", teacherInfo.Pro_Title);
            }
            if (teacherInfo.Position != null && teacherInfo.Position != "")
            {
                sb.AppendFormat(" and [Position]='{0}'", teacherInfo.Position);
            }
            using (OleDbDataReader read = SQLHelper.ExecuteReader(sb.ToString()))
            {
                if (read.HasRows)
                {
                    while (read.Read())
                    {
                        TeacherInfo ti = new TeacherInfo();
                        ti.Phone = read["Phone"].ToString();
                        list.Add(ti);
                    }
                }
            }
            return list;
        }
    }
}
