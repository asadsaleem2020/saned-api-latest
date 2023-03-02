using Executer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;



namespace CoreInfrastructure.Accounts.Transaction.BankJournal
{
    public class D_DB
    {
        private SqlConnection con;

        public D_DB(SqlConnection V_connection)
        {
            con = V_connection;

        }
        public string getUpdateMasterCount(string Voucher_type, string yearmonth, M_Header M)
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            string no = "";

            strsql = @"
	 select  isnull(max(cast(right(voucherid, 4) as bigint)),1000)	  as code 
		  from bankjournal_header 
		  where isnumeric(voucherid) = 1 and  vouchertype='" + Voucher_type + "' and  left(voucherid ,4 ) ='" + yearmonth + "' and company_Code='" + M.COMPANY_CODE + "'";

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


            strsql = @"SELECT ISNULL(MAX(CAST(VoucherId AS bigint)),0)   as code   FROM bankjournal_header WHERE " + WHERE;

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
        public M_Header GetSingle(string whereCondition)
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "select * from Bankjournal_header where   " + whereCondition;
            sqldr = dp.ExecuteReader(con, strsql);
            M_Header fields = null;
            fields = new M_Header();
            if (sqldr.Read())
            {
                fields.Id = int.Parse(sqldr["ID"].ToString());
                fields.VoucherId = sqldr["VoucherId"].ToString();
                fields.VoucherType = sqldr["VoucherType"].ToString();
                fields.AccountCode = sqldr["AccountCode"].ToString();
                fields.AccountName = sqldr["AccountName"].ToString();
                fields.COMPANY_CODE = sqldr["COMPANY_CODE"].ToString();
                fields.DocumentDate = Convert.ToDateTime(sqldr["DocumentDate"]);
                fields.ChequeNumber = sqldr["ChequeNumber"].ToString();
                fields.Remarks = sqldr["Remarks"].ToString();
                fields.Status = sqldr["Status"].ToString();
                fields.Checked = sqldr["Checked"].ToString();
                fields.Rejected = sqldr["Rejected"].ToString();
                fields.Total = Convert.ToDecimal(sqldr["Total"]);


            }
            sqldr.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            //  fields.CashJournal_Detail = GetDetailRowForEdit(whereCondition); 

            return fields;


        }
        public string GetClosingBalance(string CompanyCode, string Accountcode)
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            string no = "";


            strsql = @"select  [dbo].[UF_CLOSING_BALANCE ] ('" + CompanyCode.ToString() + "', GETDATE(),  " + Accountcode.ToString() + ") as code";

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            sqldr = dp.ExecuteReader(con, strsql);
            if (sqldr.Read())
            {
                no = Convert.ToDecimal(sqldr["code"]).ToString().Trim();

            }
            sqldr.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (no.Trim() == "") no = "";
            return no;
        }
        public List<M_Header> GetList(string whereCondition)
        {
            DataProcess dp = new DataProcess();

            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "select * from Bankjournal_header where   " + whereCondition;
            SqlCommand cmd = new SqlCommand(strsql, con);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable _dt = new DataTable();
            sda.Fill(_dt);
            List<M_Header> list = null;
            if (_dt.Rows.Count > 0)
            {
                list = new List<M_Header>();
                foreach (DataRow sqldr in _dt.Rows)
                {
                    var fields = new M_Header
                    {

                        Id = int.Parse(sqldr["ID"].ToString()),
                        VoucherId = sqldr["VoucherId"].ToString(),
                        VoucherType = sqldr["VoucherType"].ToString(),
                        AccountCode = sqldr["AccountCode"].ToString(),
                        AccountName = sqldr["AccountName"].ToString(),
                        COMPANY_CODE = sqldr["COMPANY_CODE"].ToString(),
                        DocumentDate = Convert.ToDateTime(sqldr["DocumentDate"]),
                        Remarks = sqldr["Remarks"].ToString(),
                        Status = sqldr["Status"].ToString(),
                        Total = Convert.ToDecimal(sqldr["Total"])



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
            strsql = "select * from Bankjournal_Detail where   " + whereCondition;

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

                        ID = int.Parse(row["ID"].ToString()),
                        COMPANY_CODE = row["COMPANY_CODE"].ToString(),
                        VoucherId = row["VoucherId"].ToString(),
                        SeqNo = int.Parse(row["SeqNo"].ToString()),
                        Vouchertype = row["VoucherType"].ToString(),
                        AccountCode = row["AccountCode"].ToString(),
                        AccountName = row["AccountName"].ToString(),
                        ChequeNumber = row["ChequeNumber"].ToString(),
                        Remarks = row["Remarks"].ToString(),
                        Amount = Convert.ToDecimal(row["Amount"])



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
        public int Delete_Voucher_for_Edit_mode(string where)
        {

            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "delete  from BankJournal_Detail where " + where;
            SqlCommand cmd = new SqlCommand(strsql, con);
            int i = cmd.ExecuteNonQuery();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return i;
        }
        #region Set Method for Members
        public void Set_a(int Mode, M_Header m)
        {

            DataProcess DataProcess = new DataProcess();
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "Execute sp_Bank_journal " + Mode.ToString() + ",";
            strsql += m.Id + ",";
            strsql += "'" + m.COMPANY_CODE.Replace("'", "''") + "',";
            strsql += "'" + m.VoucherId.Replace("'", "''") + "',";
            strsql += "'" + m.VoucherType.Replace("'", "''") + "',";
            strsql += "'" + m.DocumentDate + "',";
            strsql += "'" + m.AccountCode.Replace("'", "''") + "',";
            strsql += "'" + m.AccountName.Replace("'", "''") + "',";
            strsql += "'" + m.ChequeNumber.Replace("'", "''") + "',";
            strsql += "'" + m.Checked.Replace("'", "''") + "',";
            strsql += "'" + m.Status.Replace("'", "''") + "',";
            strsql += "'" + m.Rejected.Replace("'", "''") + "',";
            strsql += "'" + m.Remarks.Replace("'", "''") + "',";
            strsql += m.Total + ",";
            strsql += "'" + m.CREATED_BY.Replace("'", "''") + "',";
            strsql += "'" + m.CREATED_ON.ToString() + "',";
            strsql += "'" + m.DELETED_BY.Replace("'", "''") + "',";
            strsql += "'" + m.DELETED_ON.ToString() + "',";
            strsql += "'" + m.IS_DELETED.Replace("'", "''") + "',";
            strsql += "'" + m.UPDATED_ON.ToString() + "',";
            strsql += "'" + m.UPDATED_BY.Replace("'", "''") + "',";
            strsql += "'" + m.APPROVED_ON.ToString() + "',";
            strsql += "'" + m.APPROVED_BY.Replace("'", "''") + "',";
            strsql += "'" + m.IMAGE.Replace("'", "''") + "'";

            DataProcess.ExecuteTransaction(con, strsql);
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        #endregion
        #region Set Method for Members  detail 
        public void Set_detail(int Mode, M_Detail m)
        {

            DataProcess DataProcess = new DataProcess();
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "Execute sp_Bank_journal_detail " + Mode.ToString() + ",";
            strsql += m.ID + ",";
            strsql += "'" + m.COMPANY_CODE.Replace("'", "''") + "',";
            strsql += "'" + m.VoucherId.Replace("'", "''") + "',";
            strsql += m.SeqNo + ",";
            strsql += "'" + m.Vouchertype.Replace("'", "''") + "',";
            strsql += "'" + m.AccountCode.Replace("'", "''") + "',";
            strsql += "'" + m.AccountName.Replace("'", "''") + "',";
            strsql += "'" + m.ChequeNumber.Replace("'", "''") + "',";
            strsql += m.Amount + ",";
            strsql += "'" + m.Remarks.Replace("'", "''") + "'";
            DataProcess.ExecuteTransaction(con, strsql);
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        #endregion

    }
}
