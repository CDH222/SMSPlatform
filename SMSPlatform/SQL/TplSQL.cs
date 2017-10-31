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
    public class TplSQL
    {
        private const string INSERT = "insert into Tpl(tpl_id,tpl_name,isCheck) values(@tpl_id,@tpl_name,@isCheck)";
        private const string DELETE = "delete from Tpl where tpl_id=@tpl_id";
        private const string UPDATE = "update Tpl set tpl_name=@tpl_name where tpl_id=@tpl_id";
        private const string UPDATE_Bir = "update Tpl set isCheck=@isCheck where tpl_id=@tpl_id";
        private const string SELECT = "select * from Tpl where tpl_id=@tpl_id";
        private const string SELECT_tpl_name = "select * from Tpl where tpl_name=@tpl_name";
        private const string SELECT_isCheckID = "select tpl_id from Tpl where isCheck=@isCheck";

        public int Insert(TplInfo tplInfo)
        {
            OleDbParameter[] parm = new OleDbParameter[] { 
                  new OleDbParameter("@tpl_id",tplInfo.Tpl_id),
                  new OleDbParameter("@tpl_name",tplInfo.Tpl_name),
                  new OleDbParameter("@isCheck",tplInfo.IsCheck)
                };
            return SQLHelper.ExecuteNonQuery(INSERT, parm);
        }
        public int Delete(string tpl_id)
        {
            OleDbParameter[] parm = new OleDbParameter[] { 
                  new OleDbParameter("@tpl_name",tpl_id)
                };
            return SQLHelper.ExecuteNonQuery(DELETE, parm);
        }
        public int Update(TplInfo tplInfo)
        {
            OleDbParameter[] parm = new OleDbParameter[] { 
                  new OleDbParameter("@tpl_name",tplInfo.Tpl_name),
                  new OleDbParameter("@tpl_id",tplInfo.Tpl_id)
                };
            return SQLHelper.ExecuteNonQuery(UPDATE, parm);
        }
        public int Update_Bir(TplInfo tplInfo)
        {
            OleDbParameter[] parm = new OleDbParameter[] { 
                  new OleDbParameter("@isCheck",tplInfo.IsCheck),
                  new OleDbParameter("@tpl_id",tplInfo.Tpl_id)
                };
            return SQLHelper.ExecuteNonQuery(UPDATE_Bir, parm);
        }

        public TplInfo Query(string tpl_id)
        {
            TplInfo tplInfo = new TplInfo();
            OleDbParameter[] parm = new OleDbParameter[] { 
                  new OleDbParameter("@tpl_id",tpl_id)
                };
            using (OleDbDataReader read = SQLHelper.ExecuteReader(SELECT, parm))
            {
                if (read.HasRows)
                {
                    while (read.Read())
                    {
                        tplInfo.Tpl_id = read["tpl_id"].ToString();
                        tplInfo.Tpl_name = read["tpl_name"].ToString();
                        tplInfo.IsCheck = (bool)read["isCheck"];
                    }
                }
            }
            return tplInfo;
        }
        public string QueryisCheckID(bool isCheck)
        {
            string tpl_id = null;
            OleDbParameter[] parm = new OleDbParameter[] { 
                  new OleDbParameter("@isCheck",isCheck)
                };
            using (OleDbDataReader read = SQLHelper.ExecuteReader(SELECT_isCheckID, parm))
            {
                if (read.HasRows)
                {
                    while (read.Read())
                    {
                        tpl_id = read["tpl_id"].ToString();
                    }
                }
            }
            return tpl_id;
        }
        public IList<TplInfo> QueryByTpl_name(string tpl_name)
        {
            IList<TplInfo> list = new List<TplInfo>();
            OleDbParameter[] parm = new OleDbParameter[] { 
                  new OleDbParameter("@tpl_name",tpl_name)
                };
            using (OleDbDataReader read = SQLHelper.ExecuteReader(SELECT_tpl_name, parm))
            {
                if (read.HasRows)
                {
                    while (read.Read())
                    {
                        TplInfo tplInfo = new TplInfo();
                        tplInfo.Tpl_id = read["tpl_id"].ToString();
                        tplInfo.Tpl_name = read["tpl_name"].ToString();
                        tplInfo.IsCheck = (bool)read["isCheck"];

                        list.Add(tplInfo);
                    }
                }
            }
            return list;
        }
    }
}
