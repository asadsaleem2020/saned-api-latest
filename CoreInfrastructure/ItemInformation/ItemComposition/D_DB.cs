using System;
using System.Data;
using System.Data.SqlClient;
using Executer;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CoreInfrastructure.ItemInformation.ItemComposition
{
    public class D_DB
    {
        private SqlConnection con;

        public D_DB(SqlConnection V_connection)
        {
            con = V_connection;

        }
        public List<M_ItemCompositionHeader> GetList(string whereCondition)
        {

            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "select * from ITEMCOMPOSITIONHEADER where   " + whereCondition;

            SqlCommand cmd = new SqlCommand(strsql, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            List<M_ItemCompositionHeader> list = null;
            if (dt.Rows.Count > 0)
            {
                list = new List<M_ItemCompositionHeader>();
                foreach (DataRow sqldr in dt.Rows)
                {
                    var fields = new M_ItemCompositionHeader
                    {
                        ITEMCODE = sqldr["ITEMCODE"].ToString(),
                        
                        
                        COMPANY_CODE = sqldr["COMPANY_CODE"].ToString(),
                        
                        REMARKS = sqldr["Remarks"].ToString(),
                        //     STATUS = sqldr["Status"].ToString(),
                        

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
     
        public List<M_ItemCompositionDetail> GetDetailRowForEdit(string whereCondition)
        {
            DataProcess dp = new DataProcess();

            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "select * from ITEMCOMPOSITIONDETAIL where   " + whereCondition;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(strsql, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            List<M_ItemCompositionDetail> list = null;
            if (dt.Rows.Count > 0)
            {
                list = new List<M_ItemCompositionDetail>();
                foreach (DataRow row in dt.Rows)
                {
                    var fields = new M_ItemCompositionDetail
                    {

                        COMPANY_CODE = row["COMPANY_CODE"].ToString(),
                         PARENTCODE= row["PARENTCODE"].ToString(),
                        SEQNO = int.Parse(row["SEQNO"].ToString()),
                        ItemCode = row["ItemCode"].ToString(),
                        ItemName = row["ItemName"].ToString(),                        
                        Quantity = Convert.ToDecimal(row["Quantity"]),
                        Rate = Convert.ToDecimal(row["Rate"]),
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
            strsql = "delete  from ITEMCOMPOSITIONDETAIL where " + where;
            SqlCommand cmd = new SqlCommand(strsql, con);
            int i = cmd.ExecuteNonQuery();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return i;
        }
        public void Set_c(int Mode, M_ItemCompositionHeader m)
        {

            DataProcess DataProcess = new DataProcess();
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            strsql = "Execute SP_ITEMCOMPOSITIONHEADER " + Mode.ToString() + ",";
            strsql += "'" + m.COMPANY_CODE.Replace("'", "''") + "',";
            strsql += "'" + m.ITEMCODE.Replace("'", "''") + "',";
            strsql += m.STATUS + ",";
            strsql += "'" + m.REMARKS.Replace("'", "''") + "',";
            strsql += "'" + m.CREATED_BY.Replace("'", "''") + "',";
            strsql += "'" + m.CREATED_ON.ToString() + "'";
            DataProcess.ExecuteTransaction(con, strsql);


            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        public void Set_d(int Mode, M_ItemCompositionDetail m)
        {

            DataProcess DataProcess = new DataProcess();
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            strsql = "Execute SP_ITEMCOMPOSITIONDETAIL " + Mode.ToString() + ",";
            strsql += "'" + m.COMPANY_CODE.Replace("'", "''") + "',";
            strsql += "'" + m.PARENTCODE.Replace("'", "''") + "',";
            strsql += m.SEQNO + ",";
            strsql += "'" + m.ItemCode.Replace("'", "''") + "',";
            strsql += "'" + m.ItemName.Replace("'", "''") + "',";
            strsql += m.Quantity + ",";            
            strsql += m.Rate + ",";
            strsql += m.Amount;
            DataProcess.ExecuteTransaction(con, strsql);


            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }

    }
}

