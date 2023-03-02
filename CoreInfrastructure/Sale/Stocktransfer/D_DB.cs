using System;
using System.Data;
using System.Data.SqlClient;
using Executer;

using System.Collections.Generic;

namespace CoreInfrastructure.Sale.Stocktransfer

{
    public class D_DB
    {
        private SqlConnection con;

        public D_DB(SqlConnection V_connection)
        {
            con = V_connection;

        }

        #region Get Members Populated
        public List<M_StockTransfer> GetList(string whereCondition)
        {

            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "select * from StockTransfer where   " + whereCondition;
            SqlCommand cmd = new SqlCommand(strsql, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable _dt = new DataTable();
            sda.Fill(_dt);
            List<M_StockTransfer> list = null;
            if (_dt.Rows.Count > 0)
            {
                list = new List<M_StockTransfer>();
                foreach (DataRow sqldr in _dt.Rows)
                {
                    var fields = new M_StockTransfer();
                   // fields.ID = int.Parse(sqldr["ID"].ToString());
                    fields.Code = sqldr["Code"].ToString();
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
        public M_StockTransfer GetSingle(string whereCondition)
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "select * from StockTransfer where   " + whereCondition;
            sqldr = dp.ExecuteReader(con, strsql);
            M_StockTransfer fields = null;
            if (sqldr.Read())
            {
                fields = new M_StockTransfer();
            //    fields.ID = int.Parse(sqldr["ID"].ToString());
                fields.Code = sqldr["Code"].ToString();
                fields.Name = sqldr["Name"].ToString();
                fields.Locked = bool.Parse(sqldr["Locked"].ToString());
                
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return fields;
        }


        public string getUpdateMasterCount()
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            //MAX( CAST(CODE AS BIGINT  ))
            strsql = @"SELECT ISNULL(MAX( CAST(documentno AS BIGINT  ))  ,0) +1  AS code FROM StockTransfer";

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

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
            if (no.Trim() == "") no = "1";
            return no;
        }
 
        #endregion

       
    }
}
