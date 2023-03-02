using System;
using System.Data;
using System.Data.SqlClient;
using Executer;

using System.Collections.Generic;

namespace CoreInfrastructure.GeneralSetting.department
{
    public class D_DB
    {
        private SqlConnection con;

        public D_DB(SqlConnection V_connection)
        {
            con = V_connection;

        }

        #region Get Members Populated
        public List<M_Department> GetList(string whereCondition)
        {

            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "select * from Department where   " + whereCondition;
            SqlCommand cmd = new SqlCommand(strsql, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable _dt = new DataTable();
            sda.Fill(_dt);
            List<M_Department> list = null;
            if (_dt.Rows.Count > 0)
            {
                list = new List<M_Department>();
                foreach (DataRow sqldr in _dt.Rows)
                {
                    var fields = new M_Department();
                    // fields.ID = int.Parse(sqldr["ID"].ToString());
                    fields.Code = sqldr["Code"].ToString();
                    fields.Name = sqldr["Name"].ToString();
                    fields.Locked = bool.Parse(sqldr["Locked"].ToString());
                    fields.Sort = int.Parse(sqldr["Sort"].ToString());

                    list.Add(fields);

                }
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return list;


        }
        public M_Department GetSingle(string whereCondition)
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "select * from Department where   " + whereCondition;
            sqldr = dp.ExecuteReader(con, strsql);
            M_Department fields = null;
            if (sqldr.Read())
            {
                fields = new M_Department();
                //    fields.ID = int.Parse(sqldr["ID"].ToString());
                fields.Code = sqldr["Code"].ToString();
                fields.Name = sqldr["Name"].ToString();
                fields.Locked = bool.Parse(sqldr["Locked"].ToString());
                fields.Sort = int.Parse(sqldr["Sort"].ToString());
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
            strsql = @"SELECT ISNULL(MAX( CAST(CODE AS BIGINT  ))  ,0) +1  AS code FROM Department";

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

        #region Set Method for Members
        public void Set_b(int Mode, M_Department m)
        {
            DataProcess DataProcess = new DataProcess();
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "Execute SP_Department " + Mode.ToString() + ",";

            strsql += "'" + m.COMPANY_CODE.Replace("'", "''") + "',";
            strsql += "'" + m.Code.Replace("'", "''") + "',";
            strsql += "'" + m.Name.Replace("'", "''") + "',";
            if (m.Locked == true)
                strsql += "1,";
            else
                strsql += "0,";
            strsql += m.Sort;


            DataProcess.ExecuteTransaction(con, strsql);
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        #endregion
    }
}
