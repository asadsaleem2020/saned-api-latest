using System;
using System.Data;
using System.Data.SqlClient;
using Executer;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CoreInfrastructure.Purchase.PurchaseReturn
{
    public class D_DB
    {
        private SqlConnection con;

        public D_DB(SqlConnection V_connection)
        {
            con = V_connection;

        }
        public List<M_PRHeader> GetList(string whereCondition)
        {

            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "select * from PRHEADER where   " + whereCondition;

            SqlCommand cmd = new SqlCommand(strsql, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            List<M_PRHeader> list = null;
            if (dt.Rows.Count > 0)
            {
                list = new List<M_PRHeader>();
                foreach (DataRow sqldr in dt.Rows)
                {
                    var fields = new M_PRHeader
                    {
                        INVOICECODE = sqldr["Invoice_Code"].ToString(),

                        PARTYCODE = sqldr["Party_Code"].ToString(),
                        NAME = sqldr["Name"].ToString(),
                        COMPANY_CODE = sqldr["COMPANY_CODE"].ToString(),
                        INVOICEDATE = Convert.ToDateTime(sqldr["Invoice_Date"]),
                        REMARKS = sqldr["Remarks"].ToString(),
                        //     STATUS = sqldr["Status"].ToString(),
                        NET_AMOUNT = Convert.ToDecimal(sqldr["NetAmount"])

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
        public M_PRHeader GetSingleContract(string whereCondition)
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = @"
select *,
(select name from currency where id = currency) as currency_name,
(select name from PaymentMode where value = LTRIM(RTRIM(Payment_Mode))) as Payment_Mode_name,
(select name from salestype where value = LTRIM(RTRIM(Sales_type))) as Sales_type_name
from sale_contract_header where   " + whereCondition;
            sqldr = dp.ExecuteReader(con, strsql);
            M_PRHeader fields = null;
            fields = new M_PRHeader();
            if (sqldr.Read())
            {

                //fields.Company_Code = sqldr["Company_Code"].ToString();
                //fields.Contract_Code = sqldr["Invoice_Code"].ToString();
                //fields.Party_Code = sqldr["Party_Code"].ToString();
                //fields.Name = sqldr["Name"].ToString();
                //fields.Address = sqldr["Address"].ToString();
                //fields.City = sqldr["City"].ToString();
                //fields.Payment_Mode = sqldr["Payment_Mode"].ToString().Trim();
                //fields.Payment_Mode_name = sqldr["Payment_Mode_name"].ToString().Trim();
                //fields.DocumentNo = sqldr["DocumentNo"].ToString(); 
                //fields.DELIVERY_DATE = Convert.ToDateTime(sqldr["DELIVERY_DATE"]);
                //fields.Broker_Code = sqldr["Broker_Code"].ToString();
                //fields.Broker_Name = sqldr["Broker_Name"].ToString();
                //fields.Contract_Remarks = sqldr["Remarks"].ToString();
                //fields.EMPLOYEE_CODE = sqldr["EMPLOYEE_CODE"].ToString();
                //fields.EMPLOYEE_NAME = sqldr["EMPLOYEE_NAME"].ToString();                
                //fields.BCO_percent = Convert.ToDecimal(sqldr["BCO_percent"]);
                //fields.BCO_perBag = Convert.ToDecimal(sqldr["BCO_perBag"]);
                //fields.BCO_per40kg = Convert.ToDecimal(sqldr["BCO_per40kg"]);
                //fields.BCS_percent = Convert.ToDecimal(sqldr["BCS_percent"]);
                //fields.BCS_perBag = Convert.ToDecimal(sqldr["BCS_perBag"]);
                //fields.BCS_per40kg = Convert.ToDecimal(sqldr["BCS_per40kg"]);
                //fields.Delivery_From = Convert.ToDateTime(sqldr["Delivery_From"]);
                //fields.Delivery_To = Convert.ToDateTime(sqldr["Delivery_To"]);
                //fields.Credit_Days = Convert.ToDecimal(sqldr["Credit_Days"]);
                //fields.Credit_Date = Convert.ToDateTime(sqldr["Credit_Date"]); 
                //fields.TotalAmount = Convert.ToDecimal(sqldr["TotalAmount"]); 
                //fields.NetAmount = Convert.ToDecimal(sqldr["NetAmount"]);
                //fields.Status = sqldr["Status"].ToString().Trim();
                //fields.Sales_type = sqldr["Sales_type"].ToString().Trim();
                //fields.Sales_type_name = sqldr["Sales_type_name"].ToString().Trim();
                //fields.Checked = sqldr["Checked"].ToString();

                //fields.Currency= sqldr["Currency"].ToString().Trim();
                //fields.Currency_name = sqldr["currency_name"].ToString().Trim();
                //fields.Exchange_rate= Convert.ToDecimal(sqldr["exch_rate"]); 
            }
            sqldr.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return fields;
        }
        public List<M_PRDetail> GetDetailRowForEdit(string whereCondition)
        {
            DataProcess dp = new DataProcess();

            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "select * from PRDETAIL where   " + whereCondition;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(strsql, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            List<M_PRDetail> list = null;
            if (dt.Rows.Count > 0)
            {
                list = new List<M_PRDetail>();
                foreach (DataRow row in dt.Rows)
                {
                    var fields = new M_PRDetail
                    {

                        COMPANY_CODE = row["COMPANY_CODE"].ToString(),
                        INVOICECODE = row["INVOICECODE"].ToString(),
                        SEQNO = int.Parse(row["SEQNO"].ToString()),
                        ItemCode = row["ItemCode"].ToString(),
                        ItemName = row["ItemName"].ToString(),
                        MeasuringUnit = row["MeasuringUnit"].ToString(),
                        Quantity = Convert.ToDecimal(row["Quantity"]),
                        Rate = Convert.ToDecimal(row["Rate"]),
                        Amount = Convert.ToDecimal(row["Amount"]),
                        DiscountRate = Convert.ToDecimal(row["DiscountRate"]),
                        DiscountAmount = Convert.ToDecimal(row["DiscountAmount"]),
                        TotalAmount = Convert.ToDecimal(row["TotalAmount"]),
                        USERCODE = row["USERCODE"].ToString(),
                        TaxAmount = Convert.ToDecimal(row["TaxAmount"]),
                        TaxRate = Convert.ToDecimal(row["TaxRate"]),
                        AmountAfterDiscount = Convert.ToDecimal(row["AmountAfterDiscount"]),
                        TotalQty = Convert.ToDecimal(row["TotalQty"]),
                        Ctn = Convert.ToDecimal(row["Ctn"]),
                        Packing = Convert.ToDecimal(row["Packing"])


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


        public M_PRHeader GetSingle(string whereCondition)
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = @"select *,
(select name from currency  where id = currency) as currency_name,
(select name from PaymentMode where value = LTRIM(RTRIM(Payment_Mode))) as Payment_Mode_name,
(select name from salestype where value = LTRIM(RTRIM(Sales_type))) as Sales_type_name from PRHEADER  where   " + whereCondition;
            sqldr = dp.ExecuteReader(con, strsql);
            M_PRHeader fields = null;
            fields = new M_PRHeader();
            if (sqldr.Read())
            {

                //fields.Company_Code = sqldr["Company_Code"].ToString();
                //fields.Invoice_Code = sqldr["Invoice_Code"].ToString();
                //fields.Contract_Code = sqldr["Contract_Code"].ToString();
                //fields.Invoice_Date = Convert.ToDateTime(sqldr["Invoice_Date"]);
                //fields.Party_Code = sqldr["Party_Code"].ToString();
                //fields.Name = sqldr["Name"].ToString();
                //fields.Address = sqldr["Address"].ToString();
                //fields.City = sqldr["City"].ToString();
                //fields.Broker_Code = sqldr["Broker_Code"].ToString();
                //fields.Broker_Name = sqldr["Broker_Name"].ToString();
                //fields.Sales_type = sqldr["Sales_type"].ToString();
                //fields.Delivery_Term = sqldr["Delivery_Term"].ToString();
                //fields.Payment_Mode = sqldr["Payment_Mode"].ToString();
                //fields.Payment_Mode_name = sqldr["Payment_Mode_name"].ToString();
                //fields.Sales_type = sqldr["Sales_type"].ToString();
                //fields.Sales_type_name = sqldr["Sales_type_name"].ToString();
                //fields.DocumentNo = sqldr["DocumentNo"].ToString(); 
                //fields.VoucherID = sqldr["voucherid"].ToString();
                //fields.Currency = sqldr["Currency"].ToString().Trim();
                //fields.Currency_name = sqldr["Currency_name"].ToString().Trim();
                //fields.OWN_BROKERY = Convert.ToDecimal(sqldr["Own_Brokery"]);
                //fields.Customer_BROKERY = Convert.ToDecimal(sqldr["Customer_Brokery"]);
                //fields.Exchange_rate = Convert.ToDecimal(sqldr["Exchange_rate"]);
                //fields.Credit_Days = Convert.ToDecimal(sqldr["Credit_Days"]);
                //fields.Credit_Date = Convert.ToDateTime(sqldr["Credit_Date"]);  
                //fields.ART_Invoice_NO= sqldr["ART_invoice_no"].ToString();
                //fields.Mandual_GP_NO = sqldr["Manual_GP_No"].ToString();
                //fields.EFORM_NO = sqldr["EForm_no"].ToString();
                //fields.Vehicle = sqldr["Vehicle"].ToString();
                //fields.Driver= sqldr["Driver"].ToString();
                //fields.Remarks = sqldr["Remarks"].ToString();
                //fields.Contract_Remarks = sqldr["Contract_Remarks"].ToString();
                //fields.TotalAmount = Convert.ToDecimal(sqldr["TotalAmount"]);
                //fields.NetAmount = Convert.ToDecimal(sqldr["NetAmount"]);
                //fields.Freight_Amount= Convert.ToDecimal(sqldr["Freightamount"]);
                //fields.SKT_Bag = 0;
                //fields.SKT_Wt = 0;
                //fields.Tolerated_Balance_Bags = 0;
                //fields.Tolerated_Balance_Wt = 0;
                //fields.Contract_Balance_Bags = 0;
                //fields.Contract_Balance_Wt = 0;
                //fields.Status = sqldr["Status"].ToString();
                //fields.Checked = sqldr["Checked"].ToString(); 
                //fields.BCO_percent = Convert.ToDecimal(sqldr["BCO_percent"]);
                //fields.BCO_perBag = Convert.ToDecimal(sqldr["BCO_perBag"]);
                //fields.BCO_per40kg = Convert.ToDecimal(sqldr["BCO_per40kg"]);
                //fields.BCS_percent = Convert.ToDecimal(sqldr["BCS_percent"]);
                //fields.BCS_perBag = Convert.ToDecimal(sqldr["BCS_perBag"]);
                //fields.BCS_per40kg = Convert.ToDecimal(sqldr["BCS_per40kg"]);





            }
            sqldr.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return fields;
        }
        public string getUpdateMasterCount(string yearmonth, M_PRHeader M)
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            string no = "";


            strsql = @"
	 select  isnull(max(cast(right(INVOICECODE, 4) as bigint)),1000)	  as code 
		  from PRHEADER 
		  where isnumeric(INVOICECODE) = 1 and     left(INVOICECODE ,4 ) ='" + yearmonth + "' and COMPANY_CODE='" + M.COMPANY_CODE + "'";

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


            strsql = @"SELECT ISNULL(MAX(CAST(INVOICECODE AS bigint)),0)   as code   FROM PRHEADER WHERE " + WHERE;

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
            strsql = "delete  from PRDETAIL where " + where;
            SqlCommand cmd = new SqlCommand(strsql, con);
            int i = cmd.ExecuteNonQuery();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return i;
        }
        public void Set_c(int Mode, M_PRHeader m)
        {

            DataProcess DataProcess = new DataProcess();
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            strsql = "Execute SP_PRHEADER " + Mode.ToString() + ",";

            strsql += "'" + m.COMPANY_CODE.Replace("'", "''") + "',";
            strsql += "'" + m.INVOICECODE.Replace("'", "''") + "',";
            strsql += "'" + m.INVOICEDATE + "',";
            strsql += "'" + m.PARTYCODE.Replace("'", "''") + "',";
            strsql += "'" + m.NAME.Replace("'", "''") + "',";
            strsql += "'" + m.TYPE.Replace("'", "''") + "',";
            strsql +=       m.STATUS + ",";
            strsql += "'" + m.SO_NO.Replace("'", "''") + "',";
            strsql += "'" + m.AREA_CODE.Replace("'", "''") + "',";
            strsql += "'" + m.DEPT_ID.Replace("'", "''") + "',";
            strsql += "'" + m.DESPATCH.Replace("'", "''") + "',";
            strsql += "'" + m.REMARKS.Replace("'", "''") + "',";
            strsql += "'" + m.DOCUMENT_NO.Replace("'", "''") + "',";
            strsql += "'" + m.SALESMAN.Replace("'", "''") + "',";
            strsql += "'" + m.PAYMENT_TYPE.Replace("'", "''") + "',";
            strsql +=       m.TOTAL_AMOUNT + ",";
            strsql +=       m.DISCOUNT_RATE + ",";
            strsql +=       m.DISCOUNT_AMOUNT + ",";
            strsql +=       m.NET_AMOUNT + ",";
            strsql +=       m.CASH_AMOUNT + ",";
            strsql +=       m.CREDIT_AMOUNT + ",";
            strsql += "'" + m.CREATED_BY.Replace("'", "''") + "',";
            strsql += "'" + m.CREATED_ON.ToString() + "',";
            strsql += "'" + m.UPDATED_BY.Replace("'", "''") + "',";
            strsql += "'" + m.UPDATED_ON.ToString() + "',";
            strsql += "'" + m.IS_DELETED.Replace("'", "''") + "',";
            strsql += "'" + m.DELETED_BY.Replace("'", "''") + "',";
            strsql += "'" + m.DELETED_ON.ToString() + "'";


            DataProcess.ExecuteTransaction(con, strsql);


            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        public void Set_d(int Mode, M_PRDetail m)
        {

            DataProcess DataProcess = new DataProcess();
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            strsql = "Execute SP_PRDETAIL " + Mode.ToString() + ",";

            strsql += "'" + m.COMPANY_CODE.Replace("'", "''") + "',";
            strsql += "'" + m.INVOICECODE.Replace("'", "''") + "',";
            strsql += m.SEQNO + ",";
            strsql += "'" + m.ItemCode.Replace("'", "''") + "',";
            strsql += "'" + m.ItemName.Replace("'", "''") + "',";
            strsql += "'" + m.MeasuringUnit.Replace("'", "''") + "',";
            strsql += m.Packing + ",";
            strsql += m.Ctn + ",";
            strsql += m.Quantity + ",";
            strsql += m.TotalQty + ",";
            strsql += m.Rate + ",";
            strsql += m.Amount + ",";
            strsql += m.DiscountRate + ",";
            strsql += m.DiscountAmount + ",";
            strsql += m.AmountAfterDiscount + ",";
            strsql += m.TaxRate + ",";
            strsql += m.TaxAmount + ",";
            strsql += m.TotalAmount + ",";
            strsql += "'" + m.USERCODE.Replace("'", "''") + "'";

            DataProcess.ExecuteTransaction(con, strsql);


            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }

    }
}

