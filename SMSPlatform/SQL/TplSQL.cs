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
        private const string INSERT = "insert into Tpl (tpl_id,tpl_name) values (@tpl_id,@tpl_name)";
        private const string UPDATE = "update Tpl set tpl_name=@tpl_name where tpl_id=@tpl_id";
        private const string SELECT = "select * from Tpl where tpl_id=@tpl_id";
        private const string SELECT_tpl_name = "select * from Tpl where tpl_name=@tpl_name";

        public static int Insert(TplInfo tplInfo)
        {
            OleDbParameter[] parm = new OleDbParameter[] { 
                  new OleDbParameter("@tpl_id",tplInfo.Tpl_id),
                  new OleDbParameter("@tpl_name",tplInfo.Tpl_name)
                };
            return SQLHelper.ExecuteNonQuery(INSERT, parm);
        }
        public static int Update(TplInfo tplInfo)
        {
            OleDbParameter[] parm = new OleDbParameter[] { 
                  new OleDbParameter("@tpl_name",tplInfo.Tpl_name),
                  new OleDbParameter("@tpl_id",tplInfo.Tpl_id)
                };
            return SQLHelper.ExecuteNonQuery(UPDATE, parm);
        }

        public static OleDbDataReader Query(TplInfo tplInfo)
        {
            OleDbParameter[] parm = new OleDbParameter[] { 
                  new OleDbParameter("@tpl_id",tplInfo.Tpl_id)
                };
            return SQLHelper.ExecuteReader(SELECT, parm);
        }
        public static OleDbDataReader QueryByTpl_name(TplInfo tplInfo)
        {
            OleDbParameter[] parm = new OleDbParameter[] { 
                  new OleDbParameter("@tpl_name",tplInfo.Tpl_name)
                };
            return SQLHelper.ExecuteReader(SELECT_tpl_name, parm);
        }
    }
}
