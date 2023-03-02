using System;
using System.Data;
using System.Data.SqlClient;
using Executer;
using System.Collections.Generic;

namespace CoreInfrastructure.Auth.Company
{
  public  class D_DB 
    {
        public string Rols_id = "";
        private SqlConnection con;

        public D_DB(SqlConnection V_connection)
        {
            con = V_connection;

        }
        #region Get Members Populated
        public List<M_Company> GetList(string whereCondition)
        {
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "select * from COMPANY where   " + whereCondition;
            SqlCommand cmd = new SqlCommand(strsql, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable _dt = new DataTable();
            sda.Fill(_dt);
            List<M_Company> list = null;
            if (_dt.Rows.Count > 0)
            {
                list = new List<M_Company>();
                foreach (DataRow sqldr in _dt.Rows)
                {
                    var fields = new M_Company();
                    //  fields.ID = int.Parse(sqldr["ID"].ToString());
                    fields.COMPANY_CODE =  sqldr["COMPANY_CODE"].ToString();
                    fields.COMPANY_NAME= sqldr["COMPANY_NAME"].ToString();
                    fields.PHONE = sqldr["PHONE"].ToString();
                    fields.ACTIVE= bool.Parse(sqldr["Active"].ToString());
                    
                    list.Add(fields);
                }
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return list;
        }
        public M_Company GetSingle(string whereCondition)
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "select * from ROLES where   " + whereCondition;
            sqldr = dp.ExecuteReader(con, strsql);
            M_Company fields = null;
            if (sqldr.Read())
            {
                fields = new M_Company();
                fields.COMPANY_CODE = sqldr["COMPANY_CODE"].ToString();
                fields.COMPANY_NAME = sqldr["COMPANY_NAME"].ToString();
                fields.PHONE = sqldr["PHONE"].ToString();
                fields.ACTIVE = bool.Parse(sqldr["Active"].ToString());

            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return fields;
        }     
        public long getUpdateMasterCount()
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            long no = 0;
            strsql = @"SELECT ISNULL(CAST( MAX(Code) AS BIGINT) ,0) +1  AS code FROM ROLES";

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            sqldr = dp.ExecuteReader(con, strsql);
            if (sqldr.Read())
            {
                no = Convert.ToInt64(sqldr["code"]);
            }
            sqldr.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (no  == 0) no = 1;
            return no;
        }       
        public long check_is_new_exists(string WhereCondition)
        {
            Executer.DataProcess dp = new Executer.DataProcess();
            DataSet ds = new DataSet();
            string strSQL;
            strSQL = @"   SELECT isnull(COunt(1),0)  FROM a_accounts_level_3 where  " + WhereCondition;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(strSQL, con);
            long count = Convert.ToInt64(cmd.ExecuteScalar());


            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            return count;
        }
        #endregion

   
    }
}
