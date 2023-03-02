using System;
using System.Data;
using System.Data.SqlClient;
using Executer;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CoreInfrastructure.ItemInformation.ItemInformation
{
 public   class D_DB 
    {
        private SqlConnection con;

        public D_DB(SqlConnection V_connection)
        {
            con = V_connection;

        }
        public string getUpdateMasterCount(string Company_Code, string level1, string level2, string level3)
        {

            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            strsql = "select  max(  cast(item_code as bigint)) +1  as code from Product_Information Where category_id='" + level1 + "' and sub_category_id='" + level2 + "' and sub_category_detail_id='" + level3 + "' and Company_Code='" + Company_Code + "' ";
            sqldr = dp.ExecuteReader(con, strsql);
            if (sqldr.Read())
            {
                no = sqldr["code"].ToString().Trim();
            }
            sqldr.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (no.Trim() == "") no = level1.Trim() + level2.Trim() + level3.Trim() + "1001";
            else no = no.Trim();
            return no;
        }
        public List<M_ItemInformation> GetList(string whereCondition)
        {
            string strsql;
            strsql = "select * from Product_Information where   " + whereCondition;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlDataAdapter sda = new SqlDataAdapter(strsql, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            List<M_ItemInformation> list = null;
            if (dt.Rows.Count > 0)
            {
                list = new List<M_ItemInformation>();
                foreach (DataRow sqldr in dt.Rows)
                {


                    var m = new M_ItemInformation();
                  //  m.ID = int.Parse(sqldr["ID"].ToString());
                    m.COMPANY_CODE = sqldr["COMPANY_CODE"].ToString();
                    m.ITEM_CODE = sqldr["ITEM_CODE"].ToString();
                    m.ITEM_NAME = sqldr["ITEM_NAME"].ToString();
                    //m.SHORT_NAME = sqldr["SHORT_NAME"].ToString();
                    //m.ITEM_BARCODE = sqldr["ITEM_BARCODE"].ToString();
                    //m.LOCKED = bool.Parse(sqldr["Locked"].ToString());
                    //m.CATEGORY_ID = sqldr["CATEGORY_ID"].ToString();
                    //m.CATEGORY_NAME = sqldr["CATEGORY_NAME"].ToString();
                    //m.SUB_CATEGORY_ID= sqldr["SUB_CATEGORY_ID"].ToString();
                    //m.SUB_CATEGORY_NAME = sqldr["SUB_CATEGORY_NAME"].ToString();
                    //m.SUB_CATEGORY_DETAIL_ID= sqldr["SUB_CATEGORY_DETAIL_ID"].ToString();
                    //m.SUB_CATEGORY_DETAIL_NAME= sqldr["SUB_CATEGORY_DETAIL_NAME"].ToString();
                    //m.MEASURING_UNIT_ID = sqldr["MEASURING_UNIT_ID"].ToString();
                    //m.MEASURING_UNIT_NAME = sqldr["MEASURING_UNIT_NAME"].ToString();
                    //m.LOCAL_SALE_ACCOUNT_CODE = sqldr["LOCAL_SALE_ACCOUNT_CODE"].ToString();
                    //m.LOCAL_SALE_ACCOUNT_NAME = sqldr["LOCAL_SALE_ACCOUNT_NAME"].ToString(); 
                    //m.EXPORT_SALE_ACCOUNT_CODE = sqldr["EXPORT_SALE_ACCOUNT_CODE"].ToString();
                    //m.EXPORT_SALE_ACCOUNT_NAME = sqldr["EXPORT_SALE_ACCOUNT_NAME"].ToString();
                    //m.PURCHASE_ACCOUNT_CODE = sqldr["PURCHASE_ACCOUNT_CODE"].ToString();
                    //m.PURCHASE_ACCOUNT_NAME = sqldr["PURCHASE_ACCOUNT_NAME"].ToString(); 
                    //m.SALES_CONSUMPTION_ACCOUNT_CODE = sqldr["SALES_CONSUMPTION_ACCOUNT_CODE"].ToString();
                    //m.SALES_CONSUMPTION_ACCOUNT_NAME = sqldr["SALES_CONSUMPTION_ACCOUNT_NAME"].ToString(); 
                    //m.FINISHED_STOCK_ACCOUNT_CODE = sqldr["FINISHED_STOCK_ACCOUNT_CODE"].ToString();
                    //m.FINISHED_STOCK_ACCOUNT_NAME = sqldr["FINISHED_STOCK_ACCOUNT_NAME"].ToString();
                    //m.WIP_CONSUMPTION_ACCOUNT_CODE = sqldr["WIP_CONSUMPTION_ACCOUNT_CODE"].ToString();
                    //m.WIP_CONSUMPTION_ACCOUNT_NAME = sqldr["WIP_CONSUMPTION_ACCOUNT_NAME"].ToString();
                    m.PURCHASE_RATE = Decimal.Parse(sqldr["PURCHASE_RATE"].ToString());
                    m.SALE_RATE = Decimal.Parse(sqldr["SALE_RATE"].ToString());
                    m.Whole_Sale_Rate = Decimal.Parse(sqldr["Whole_Sale_Rate"].ToString());
                    m.Retail_Rate = Decimal.Parse(sqldr["Retail_Rate"].ToString());
                    m.Colour = sqldr["Colour"].ToString();
                    m.Packing_Qty = Decimal.Parse(sqldr["Packing_Qty"].ToString());
                    m.MIN_LEVEL = Decimal.Parse(sqldr["MIN_LEVEL"].ToString());
                    m.MAX_LEVEL = Decimal.Parse(sqldr["MAX_LEVEL"].ToString()); 
                    list.Add(m);

                }
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return list;


        }
        public M_ItemInformation GetSingle(string whereCondition)
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "select * from Product_Information where   " + whereCondition;
            sqldr = dp.ExecuteReader(con, strsql);
            M_ItemInformation m = null;
            if (sqldr.Read())
            {


                m = new M_ItemInformation();
            //    m.ID = int.Parse(sqldr["ID"].ToString());
                m.COMPANY_CODE = sqldr["COMPANY_CODE"].ToString();
                m.ITEM_CODE = sqldr["ITEM_CODE"].ToString();
                m.ITEM_NAME = sqldr["ITEM_NAME"].ToString();
               // m.SHORT_NAME = sqldr["SHORT_NAME"].ToString();
                m.ITEM_BARCODE = sqldr["ITEM_BARCODE"].ToString();
                //m.LOCKED = bool.Parse(sqldr["Locked"].ToString());
                //m.CATEGORY_ID = sqldr["CATEGORY_ID"].ToString();
                //m.CATEGORY_NAME = sqldr["CATEGORY_NAME"].ToString();
                //m.SUB_CATEGORY_ID = sqldr["SUB_CATEGORY_ID"].ToString();
                //m.SUB_CATEGORY_NAME = sqldr["SUB_CATEGORY_NAME"].ToString();
                //m.SUB_CATEGORY_DETAIL_ID = sqldr["SUB_CATEGORY_DETAIL_ID"].ToString();
                //m.SUB_CATEGORY_DETAIL_NAME = sqldr["SUB_CATEGORY_DETAIL_NAME"].ToString();
                //m.MEASURING_UNIT_ID = sqldr["MEASURING_UNIT_ID"].ToString();
                //m.MEASURING_UNIT_NAME = sqldr["MEASURING_UNIT_NAME"].ToString();
                //m.LOCAL_SALE_ACCOUNT_CODE = sqldr["LOCAL_SALE_ACCOUNT_CODE"].ToString();
                //m.LOCAL_SALE_ACCOUNT_NAME = sqldr["LOCAL_SALE_ACCOUNT_NAME"].ToString();
                //m.EXPORT_SALE_ACCOUNT_CODE = sqldr["EXPORT_SALE_ACCOUNT_CODE"].ToString();
                //m.EXPORT_SALE_ACCOUNT_NAME = sqldr["EXPORT_SALE_ACCOUNT_NAME"].ToString();
                //m.PURCHASE_ACCOUNT_CODE = sqldr["PURCHASE_ACCOUNT_CODE"].ToString();
                //m.PURCHASE_ACCOUNT_NAME = sqldr["PURCHASE_ACCOUNT_NAME"].ToString();
                //m.SALES_CONSUMPTION_ACCOUNT_CODE = sqldr["SALES_CONSUMPTION_ACCOUNT_CODE"].ToString();
                //m.SALES_CONSUMPTION_ACCOUNT_NAME = sqldr["SALES_CONSUMPTION_ACCOUNT_NAME"].ToString();
                //m.FINISHED_STOCK_ACCOUNT_CODE = sqldr["FINISHED_STOCK_ACCOUNT_CODE"].ToString();
                //m.FINISHED_STOCK_ACCOUNT_NAME = sqldr["FINISHED_STOCK_ACCOUNT_NAME"].ToString();
                //m.WIP_CONSUMPTION_ACCOUNT_CODE = sqldr["WIP_CONSUMPTION_ACCOUNT_CODE"].ToString();
                //m.WIP_CONSUMPTION_ACCOUNT_NAME = sqldr["WIP_CONSUMPTION_ACCOUNT_NAME"].ToString();
                m.PURCHASE_RATE = Decimal.Parse(sqldr["PURCHASE_RATE"].ToString());
                m.SALE_RATE = Decimal.Parse(sqldr["SALE_RATE"].ToString());
                m.Whole_Sale_Rate = Decimal.Parse(sqldr["Whole_Sale_Rate"].ToString());
                m.Retail_Rate = Decimal.Parse(sqldr["Retail_Rate"].ToString());
                m.Colour = sqldr["Colour"].ToString();
                m.Packing_Qty = Decimal.Parse(sqldr["Packing_Qty"].ToString());
                m.MIN_LEVEL = Decimal.Parse(sqldr["MIN_LEVEL"].ToString());
                m.MAX_LEVEL = Decimal.Parse(sqldr["MAX_LEVEL"].ToString());
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return m; 
        }
        public void insertintodb(string[] array) { }

        //public void Set_c(int Mode, M_ItemInformation m)
        //{

        //    DataProcess DataProcess = new DataProcess();
        //    string strsql;
        //    if (con.State == ConnectionState.Closed)
        //    {
        //        con.Open();
        //    }
        //    strsql = "Execute sp_ITEM_INFORMATION " + Mode.ToString() + ",";
        //    strsql += m.ID + ",";
        //    strsql += "'" + m.COMPANY_CODE.Replace("'", "''") + "',";
        //    strsql += "'" + m.ITEM_CODE.Replace("'", "''") + "',";
        //    strsql += "'" + m.ITEM_NAME.Replace("'", "''") + "',";
        //    strsql += "'" + m.SHORT_NAME.Replace("'", "''") + "',";
        //    strsql += "'" + m.ITEM_BARCODE.Replace("'", "''") + "',";
        //    if (m.LOCKED == true)
        //        strsql += "1,";
        //    else
        //        strsql += "0,";
        //    strsql += "'" + m.CATEGORY_ID.Replace("'", "''") + "',";
        //    strsql += "'" + m.CATEGORY_NAME.Replace("'", "''") + "',";
        //    strsql += "'" + m.SUB_CATEGORY_ID.Replace("'", "''") + "',";
        //    strsql += "'" + m.SUB_CATEGORY_NAME.Replace("'", "''") + "',";
        //    strsql += "'" + m.SUB_CATEGORY_DETAIL_ID.Replace("'", "''") + "',";
        //    strsql += "'" + m.SUB_CATEGORY_DETAIL_NAME.Replace("'", "''") + "',";
        //    strsql += "'" + m.MEASURING_UNIT_ID.Replace("'", "''") + "',";
        //    strsql += "'" + m.MEASURING_UNIT_NAME.Replace("'", "''") + "',";            
        //    strsql += "'" + m.LOCAL_SALE_ACCOUNT_CODE.Replace("'", "''") + "',";
        //    strsql += "'" + m.LOCAL_SALE_ACCOUNT_NAME.Replace("'", "''") + "',";
        //    strsql += "'" + m.EXPORT_SALE_ACCOUNT_CODE.Replace("'", "''") + "',";
        //    strsql += "'" + m.EXPORT_SALE_ACCOUNT_NAME.Replace("'", "''") + "',";
        //    strsql += "'" + m.PURCHASE_ACCOUNT_CODE.Replace("'", "''") + "',";
        //    strsql += "'" + m.PURCHASE_ACCOUNT_NAME.Replace("'", "''") + "',";
        //    strsql += "'" + m.WIP_CONSUMPTION_ACCOUNT_CODE.Replace("'", "''") + "',";
        //    strsql += "'" + m.WIP_CONSUMPTION_ACCOUNT_NAME.Replace("'", "''") + "',";
        //    strsql += "'" + m.SALES_CONSUMPTION_ACCOUNT_CODE.Replace("'", "''") + "',";
        //    strsql += "'" + m.SALES_CONSUMPTION_ACCOUNT_NAME.Replace("'", "''") + "',";
        //    strsql += "'" + m.FINISHED_STOCK_ACCOUNT_CODE.Replace("'", "''") + "',";
        //    strsql += "'" + m.FINISHED_STOCK_ACCOUNT_NAME.Replace("'", "''") + "',";
        //    strsql += m.PURCHASE_RATE + ",";
        //    strsql += m.SALE_RATE + ",";
        //    strsql += m.Whole_Sale_Rate + ",";
        //    strsql += m.Retail_Rate + ",";
        //    strsql += "'" + m.Colour.Replace("'", "''") + "',";
        //    strsql += m.Packing_Qty + ",";
        //    strsql += m.MAX_LEVEL + ",";
        //    strsql += m.MIN_LEVEL + ",";
        //    strsql += "'" + m.CREATED_ON.ToString() + "',";
        //    strsql += "'" + m.CREATED_BY.Replace("'", "''") + "',";
        //    strsql += "'" + m.IS_DELETED.Replace("'", "''") + "',";
        //    strsql += "'" + m.DELETED_BY.Replace("'", "''") + "',";
        //    strsql += "'" + m.DELETED_ON.ToString() + "',";
        //    strsql += "'" + m.UPDATED_BY.Replace("'", "''") + "',";
        //    strsql += "'" + m.UPDATED_ON.ToString() + "'";            
        //    DataProcess.ExecuteTransaction(con, strsql);
        //    if (con.State == ConnectionState.Open)
        //    {
        //        con.Close();
        //    }
        //}
    }
}
