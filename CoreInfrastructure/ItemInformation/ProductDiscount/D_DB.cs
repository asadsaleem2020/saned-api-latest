using System;
using System.Data;
using System.Data.SqlClient;
using Executer;

using System.Collections.Generic;

namespace CoreInfrastructure.ItemInformation.ProductDiscount
{
    public class D_DB
    {
        private SqlConnection con;

        public D_DB(SqlConnection V_connection)
        {
            con = V_connection;

        }

        #region Get Members Populated
        public List<M_ProductDiscount> GetList(string whereCondition)
        {

            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "select * from ProductDiscount where   " + whereCondition;
            SqlCommand cmd = new SqlCommand(strsql, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable _dt = new DataTable();
            sda.Fill(_dt);
            List<M_ProductDiscount> list = null;
            if (_dt.Rows.Count > 0)
            {
                list = new List<M_ProductDiscount>();
                foreach (DataRow sqldr in _dt.Rows)
                {
                    var fields = new M_ProductDiscount();
                   // fields.ID = int.Parse(sqldr["ID"].ToString());
                    fields.Code =  Convert.ToInt64( sqldr["Code"].ToString());
                    fields.Name = sqldr["Name"].ToString();
                    fields.Locked = bool.Parse(sqldr["Locked"].ToString());
                 
                    list.Add(fields);

                }
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return list;


        }
        public M_ProductDiscount GetSingle(string whereCondition)
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            decimal? rate = 0;
            strsql = "select * from ProductDiscount where   " + whereCondition;
            sqldr = dp.ExecuteReader(con, strsql);
            M_ProductDiscount fields = null;
            if (sqldr.Read())
            {
                fields = new M_ProductDiscount();
                //    fields.ID = int.Parse(sqldr["ID"].ToString());
                fields.Code = Convert.ToInt64(sqldr["Code"].ToString());
                fields.Name = sqldr["Name"].ToString();
                fields.Locked = bool.Parse(sqldr["Locked"].ToString());
                fields.Amount = Convert.ToDecimal(sqldr["Amount"]);
                rate = fields.Amount;



            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }


            if (rate <= 0 )
            {
                strsql = "select * from ProductDiscount where DiscountCategory='A'   ";
                sqldr = dp.ExecuteReader(con, strsql);
                M_ProductDiscount field = null;
                if (sqldr.Read())
                {
                    field = new M_ProductDiscount();
                    //    fields.ID = int.Parse(sqldr["ID"].ToString());
                    field.Code = Convert.ToInt64(sqldr["Code"].ToString());
                    field.Name = sqldr["Name"].ToString();
                    field.Locked = bool.Parse(sqldr["Locked"].ToString());
                    field.Amount = Convert.ToDecimal(sqldr["Amount"]);
                    field.discType= sqldr["discType"].ToString();

                }
                return field;
            }




            return fields;
        }

        public M_ProductDiscount GetDiscountRate(string brandid, string category, string subcategory, string product)
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            decimal? rate = 0;
            strsql = "select * from ProductDiscount where  DiscountCategory='A'"; 
            sqldr = dp.ExecuteReader(con, strsql);
            M_ProductDiscount fields = null;
            if (sqldr.Read())
            {
                fields = new M_ProductDiscount();
                //    fields.ID = int.Parse(sqldr["ID"].ToString());
                fields.Code = Convert.ToInt64(sqldr["Code"].ToString());
                fields.Name = sqldr["Name"].ToString();
                fields.Locked = bool.Parse(sqldr["Locked"].ToString());
                fields.Amount = Convert.ToDecimal(sqldr["Amount"]);
                rate = fields.Amount;



            }
            if (rate <= 0)
            {
                strsql = "select * from ProductDiscount where DiscountCategory='B' and brandid='" + brandid.Trim() + "'";
                sqldr.Close();
                sqldr = dp.ExecuteReader(con, strsql);
                M_ProductDiscount field = null;
                if (sqldr.Read())
                {
                    field = new M_ProductDiscount();
                    //    fields.ID = int.Parse(sqldr["ID"].ToString());
                    field.Code = Convert.ToInt64(sqldr["Code"].ToString());
                    field.Name = sqldr["Name"].ToString();
                    field.Locked = bool.Parse(sqldr["Locked"].ToString());
                    field.Amount = Convert.ToDecimal(sqldr["Amount"]);
                    field.discType = sqldr["discType"].ToString();
                    rate = field.Amount;
                }
                return field;
            }
            if (rate <= 0)
            {
                strsql = "select * from ProductDiscount where DiscountCategory='C' and categoryid='" + category.Trim()+"'";
                sqldr.Close();
                   sqldr = dp.ExecuteReader(con, strsql);
                M_ProductDiscount field = null;
                if (sqldr.Read())
                {
                    field = new M_ProductDiscount();
                    //    fields.ID = int.Parse(sqldr["ID"].ToString());
                    field.Code = Convert.ToInt64(sqldr["Code"].ToString());
                    field.Name = sqldr["Name"].ToString();
                    field.Locked = bool.Parse(sqldr["Locked"].ToString());
                    field.Amount = Convert.ToDecimal(sqldr["Amount"]);
                    field.discType = sqldr["discType"].ToString();
                    rate = field.Amount;
                }
                return field;
            }
            if (rate <= 0)
            {
                strsql = "select * from ProductDiscount where DiscountCategory='S' and subcategoryid='" + brandid.Trim() + "'";
                sqldr.Close();
                sqldr = dp.ExecuteReader(con, strsql);
                M_ProductDiscount field = null;
                if (sqldr.Read())
                {
                    field = new M_ProductDiscount();
                    //    fields.ID = int.Parse(sqldr["ID"].ToString());
                    field.Code = Convert.ToInt64(sqldr["Code"].ToString());
                    field.Name = sqldr["Name"].ToString();
                    field.Locked = bool.Parse(sqldr["Locked"].ToString());
                    field.Amount = Convert.ToDecimal(sqldr["Amount"]);
                    field.discType = sqldr["discType"].ToString();
                    rate = field.Amount;
                }
                return field;
            }
            if (rate <= 0)
            {
                strsql = "select * from ProductDiscount where DiscountCategory='P' and productid='" + product.Trim() + "'";
                sqldr.Close();
                sqldr = dp.ExecuteReader(con, strsql);
                M_ProductDiscount field = null;
                if (sqldr.Read())
                {
                    field = new M_ProductDiscount();
                    //    fields.ID = int.Parse(sqldr["ID"].ToString());
                    field.Code = Convert.ToInt64(sqldr["Code"].ToString());
                    field.Name = sqldr["Name"].ToString();
                    field.Locked = bool.Parse(sqldr["Locked"].ToString());
                    field.Amount = Convert.ToDecimal(sqldr["Amount"]);
                    field.discType = sqldr["discType"].ToString();
                    rate = field.Amount;
                }
                return field;
            }



            return fields;
        }
        public long getUpdateMasterCount()
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            long no = 0;
            //MAX( CAST(CODE AS BIGINT  ))
            strsql = @"SELECT ISNULL(MAX( CAST(CODE AS BIGINT  ))  ,0) +1  AS code FROM ProductDiscount";

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            sqldr = dp.ExecuteReader(con, strsql);
            if (sqldr.Read())
            {
                no = Convert.ToInt64( sqldr["code"]);
            }
            sqldr.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (no ==0) no = 1;
            return no;
        }


      
        #endregion

      
    }
}
