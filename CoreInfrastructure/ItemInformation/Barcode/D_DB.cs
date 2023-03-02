using System;
using System.Data;
using System.Data.SqlClient;
using Executer;

using System.Collections.Generic;

namespace CoreInfrastructure.ItemInformation.Barcode

{
    public class D_DB
    {
        private SqlConnection con;

        public D_DB(SqlConnection V_connection)
        {
            con = V_connection;

        }

        #region Get Members Populated
        public List<M_BarcodeGenerate> GetList(string whereCondition)
        {

            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "select * from BarcodeGenerate where   " + whereCondition;
            SqlCommand cmd = new SqlCommand(strsql, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable _dt = new DataTable();
            sda.Fill(_dt);
            List<M_BarcodeGenerate> list = null;
            if (_dt.Rows.Count > 0)
            {
                list = new List<M_BarcodeGenerate>();
                foreach (DataRow sqldr in _dt.Rows)
                {
                    var fields = new M_BarcodeGenerate();
                   // fields.ID = int.Parse(sqldr["ID"].ToString());
                    fields.Code = sqldr["Code"].ToString();
                   
                   
                    

                    list.Add(fields);

                }
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return list;


        }
        public M_BarcodeGenerate GetSingle(string whereCondition)
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "select * from BarcodeGenerate where   " + whereCondition;
            sqldr = dp.ExecuteReader(con, strsql);
            M_BarcodeGenerate fields = null;
            if (sqldr.Read())
            {
                fields = new M_BarcodeGenerate();
            //    fields.ID = int.Parse(sqldr["ID"].ToString());
                fields.Code = sqldr["Code"].ToString(); 
            
                
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return fields;
        }


       
 
        #endregion

       
    }
}
