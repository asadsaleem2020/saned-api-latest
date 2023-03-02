using Executer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;



namespace CoreInfrastructure.Accounts.Transaction.ShiftClose
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
		  from ShiftClose 
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


            strsql = @"SELECT ISNULL(MAX(CAST(VoucherId AS bigint)),0)   as code   FROM ShiftClose WHERE " + WHERE;

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
            strsql = "select * from ShiftClose where   " + whereCondition;
            sqldr = dp.ExecuteReader(con, strsql);
            M_Header fields = null;
            fields = new M_Header();
            if (sqldr.Read())
            {
                
                fields.VoucherId = sqldr["VoucherId"].ToString();
                fields.VoucherType = sqldr["VoucherType"].ToString();
                fields.Payee = sqldr["Payee"].ToString();
                fields.Receiver = sqldr["Receiver"].ToString();
                fields.COMPANY_CODE = sqldr["COMPANY_CODE"].ToString();
                fields.DocumentDate = Convert.ToDateTime(sqldr["DocumentDate"]);
                fields.CounterNumber = sqldr["CounterNumber"].ToString();
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
    
        public List<M_Header> GetList(string whereCondition)
        {
            DataProcess dp = new DataProcess();

            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "select * from ShiftClose where   " + whereCondition;
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

                      
                        VoucherId = sqldr["VoucherId"].ToString(),
                        VoucherType = sqldr["VoucherType"].ToString(),
                        CounterNumber= sqldr["CounterNumber"].ToString(),
                        Payee = sqldr["Payee "].ToString(),
                        Receiver = sqldr["Receiver "].ToString(),
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
     
     
       
     

    }
}
