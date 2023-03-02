using System;
using System.Data;
using System.Data.SqlClient;
using Executer;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using CoreInfrastructure.Accounts.Transaction.GeneralJournal;

namespace CoreInfrastructure.Accounts.Transaction.GeneralJournal
{
   public class D_DB
    {
        private SqlConnection con;

        public D_DB(SqlConnection V_connection)
        {
            con = V_connection;

        }
        #region Get Members Populated
        public List<M_Header> GetList(string whereCondition)
        {
            DataProcess dp = new DataProcess();
            
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "select * from GeneralJournal where   " + whereCondition;
            SqlCommand cmd = new SqlCommand(strsql,con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable _dt = new DataTable();
            sda.Fill(_dt);
            
            List<M_Header> list = null;
            if (_dt.Rows.Count > 0 )
            {
                list = new List<M_Header>();
                foreach (DataRow sqldr in _dt.Rows)
                {
                    var fields = new M_Header
                    {
                        Id = int.Parse(sqldr["ID"].ToString()),
                        COMPANY_CODE = sqldr["COMPANY_CODE"].ToString(),
                        VoucherId = sqldr["VoucherId"].ToString(),
                        VoucherType = sqldr["VoucherType"].ToString(),
                        DocumentDate = Convert.ToDateTime(sqldr["DocumentDate"]),
                        SeqNo = int.Parse(sqldr["seqno"].ToString()),
                        AccountCode = sqldr["AccountCode"].ToString(),
                        AccountName = sqldr["AccountName"].ToString(),
                        Debit = decimal.Parse(sqldr["Debit"].ToString()),
                        Credit = decimal.Parse(sqldr["Credit"].ToString()),
                        Status = sqldr["Status"].ToString(),
                        Remarks = sqldr["Remarks"].ToString(),
                        CREATED_BY = sqldr["CREATED_BY"].ToString(),
                        CREATED_ON = Convert.ToDateTime(sqldr["CREATED_ON"].ToString()),
                        DELETED_BY = sqldr["DELETED_BY"].ToString(),
                        DELETED_ON = Convert.ToDateTime(sqldr["DELETED_ON"].ToString()),
                        IS_DELETED = sqldr["IS_DELETED"].ToString(),
                        UPDATED_ON = Convert.ToDateTime(sqldr["UPDATED_ON"].ToString()),
                        UPDATED_BY = sqldr["UPDATED_BY"].ToString()

                    };
                    list.Add(fields);
                } 
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return list;


        }
        public List<M_Detail> GetDetailRowForEdit(string whereCondition)
        {
            DataProcess dp = new DataProcess();

            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "select * from GeneralJournal where   " + whereCondition;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(strsql, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            List<M_Detail> list = null;
            if (dt.Rows.Count > 0)
            {
                list = new List<M_Detail>();
                foreach (DataRow row in dt.Rows)
                {
                    var fields = new M_Detail
                    {



                        AccountCode = row["AccountCode"].ToString(),
                        AccountName = row["AccountName"].ToString(),

                        Remarks = row["Remarks"].ToString(),
                        Debit = Convert.ToDecimal(row["Debit"]),
                        Credit= Convert.ToDecimal(row["Credit"])

                       
    };
                    list.Add(fields);

                }
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return list;


        }
        public M_Header GetSingle(string whereCondition)
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "select * from GeneralJournal where   " + whereCondition;
            sqldr = dp.ExecuteReader(con, strsql);
            M_Header fields = null;
            fields = new M_Header();
            if (sqldr.Read())
            { 
                fields.Id = int.Parse(sqldr["ID"].ToString());
                fields.COMPANY_CODE = sqldr["COMPANY_CODE"].ToString();
                fields.VoucherId = sqldr["VoucherId"].ToString();
                fields.VoucherType = sqldr["VoucherType"].ToString();
                fields.DocumentDate = Convert.ToDateTime(sqldr["DocumentDate"]);
                fields.SeqNo = int.Parse(sqldr["seqno"].ToString());
                fields.AccountCode = sqldr["AccountCode"].ToString();
                fields.AccountName = sqldr["AccountName"].ToString();
                fields.Debit = decimal.Parse(sqldr["Debit"].ToString());
                fields.Credit = decimal.Parse(sqldr["Credit"].ToString());
                fields.Status = sqldr["Status"].ToString();
                fields.Rejected = sqldr["Rejected"].ToString();
                fields.Checked = sqldr["Checked"].ToString();
                fields.Remarks = sqldr["Remarks"].ToString();
                fields.CREATED_BY = sqldr["CREATED_BY"].ToString();
                fields.CREATED_ON = Convert.ToDateTime(sqldr["CREATED_ON"].ToString());
                fields.DELETED_BY = sqldr["DELETED_BY"].ToString();
                fields.DELETED_ON = Convert.ToDateTime(sqldr["DELETED_ON"].ToString());
                fields.IS_DELETED = sqldr["IS_DELETED"].ToString();
                fields.UPDATED_ON = Convert.ToDateTime(sqldr["UPDATED_ON"].ToString());
                fields.UPDATED_BY = sqldr["UPDATED_BY"].ToString(); 
            }
            sqldr.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return fields; 
        }
        public string getUpdateMasterCount(string Voucher_type, string yearmonth, M_Header m)
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            //            strsql = @" select cast(right( year(getdate()),2) as varchar(2)) + 
            //(case when len(cast( month(getdate()) as varchar ) )=1 then  replicate('0',1) +cast( month(getdate()) as varchar )  end)
            //+cast(isnull(max(voucherid),1001) as varchar(4) ) as code
            //from cashjournal_header ";

            strsql = @"
	 select  isnull(max(cast(right(voucherid, 4) as bigint)),1000)	  as code 
		  from GeneralJournal 
		  where isnumeric(voucherid) = 1 and  vouchertype='" + Voucher_type + "' and  left(voucherid ,4 ) ='" + yearmonth + "' and company_Code='" + m.COMPANY_CODE + "'";
          

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            sqldr = dp.ExecuteReader(con, strsql);
            if (sqldr.Read())
            {
                no = sqldr["code"].ToString().Trim();
                no = (Convert.ToInt64(no) + 1).ToString();
                no = yearmonth + no;
            }
            sqldr.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (no.Trim() == "") no = "";
            return no;
        }
        public string getUpdateMasterCount(string WHERE)
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            string no = "";


            strsql = @"SELECT ISNULL(MAX(CAST(VoucherId AS bigint)),0)   as code   FROM GeneralJournal WHERE " + WHERE;

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            sqldr = dp.ExecuteReader(con, strsql);
            if (sqldr.Read())
            {
                no = sqldr["code"].ToString().Trim();
                no = (Convert.ToInt64(no) + 1).ToString();

            }
            sqldr.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (no.Trim() == "") no = "";
            return no;
        }

        public int Delete_Voucher_for_Edit_mode(string where)
        {

            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "delete  from generaljournal where " + where;
            SqlCommand cmd = new SqlCommand(strsql, con);
            int i = cmd.ExecuteNonQuery();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return i;
        }
        #endregion

        #region Set Method for Members
        public void Set_a(int Mode, M_Header m )
        {

            DataProcess DataProcess = new DataProcess();
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "Execute sp_general_journal " + Mode.ToString() + ",";
            strsql += m.Id + ",";
            strsql += "'" + m.COMPANY_CODE.Replace("'", "''") + "',";
            strsql += "'" + m.VoucherId.Replace("'", "''") + "',";
            strsql += "'" + m.VoucherType.Replace("'", "''") + "',";
            strsql += "'" + m.DocumentDate + "',";
            strsql += m.SeqNo + ",";
            strsql += "'" + m.AccountCode.Replace("'", "''") + "',";
            strsql += "'" + m.AccountName.Replace("'", "''") + "',";
            strsql += m.Debit + ",";
            strsql += m.Credit + ",";
            strsql += "'" + m.Checked.Replace("'", "''") + "',";
            strsql += "'" + m.Status.Replace("'", "''") + "',";
            strsql += "'" + m.Rejected.Replace("'", "''") + "',";
            strsql += "'" + m.Remarks.Replace("'", "''") + "',";
            strsql += "'" + m.CREATED_BY.Replace("'", "''") + "',";
            strsql += "'" + m.CREATED_ON.ToString() + "',";
            strsql += "'" + m.DELETED_BY.Replace("'", "''") + "',";
            strsql += "'" + m.DELETED_ON.ToString() + "',";
            strsql += "'" + m.IS_DELETED.Replace("'", "''") + "',";
            strsql += "'" + m.UPDATED_ON.ToString() + "',";
            strsql += "'" + m.UPDATED_BY.Replace("'", "''") + "',";
            strsql += "'" + m.APPROVED_ON.ToString() + "',";
            strsql += "'" + m.APPROVED_BY.Replace("'", "''") + "'";
            DataProcess.ExecuteTransaction(con, strsql);
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        #endregion
    }
}
