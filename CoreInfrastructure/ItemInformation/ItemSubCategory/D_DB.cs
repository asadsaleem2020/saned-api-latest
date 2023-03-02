using System;
using System.Data;
using System.Data.SqlClient;
using Executer;
using System.Collections.Generic;

namespace CoreInfrastructure.ItemInformation.ItemSubCategory
{
    public class D_DB
    {
        private SqlConnection con;

        public D_DB(SqlConnection V_connection)
        {
            con = V_connection;

        }
        #region Get Members Populated
        public List<M_SubCategory> GetList(string whereCondition)
        {

            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "select * from Item_SubCategory where   " + whereCondition;
            SqlCommand cmd = new SqlCommand(strsql, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable _dt = new DataTable();
            sda.Fill(_dt);
            List<M_SubCategory> list = null;
            if (_dt.Rows.Count > 0)
            {
                list = new List<M_SubCategory>();
                foreach (DataRow sqldr in _dt.Rows)
                {
                    var fields = new M_SubCategory();


                    fields.Level1Code = sqldr["Level1Code"].ToString();
                    fields.Level1Name = sqldr["Level1Name"].ToString();
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
        public M_SubCategory GetSingle(string whereCondition)
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "select * from Item_SubCategory where   " + whereCondition;
            sqldr = dp.ExecuteReader(con, strsql);
            M_SubCategory fields = null;
            if (sqldr.Read())
            {
                fields = new M_SubCategory();



                fields.Level1Code = sqldr["Level1Code"].ToString();
                fields.Level1Name = sqldr["Level1Name"].ToString();
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

        //public void Get_b(int _ID)
        //{

        //    DataProcess dp = new DataProcess();
        //    SqlDataReader sqldr;
        //    string strsql;
        //    if (con.State == ConnectionState.Closed)
        //    {
        //        con.Open();
        //    }
        //    strsql = "select * from A_Accounts_level_2 where ID = " + _ID;
        //    sqldr = dp.ExecuteReader(con, strsql);
        //    if (sqldr.Read())
        //    {
        //        _Level1Code = sqldr["Level1Code"].ToString();
        //        _Level1Name = sqldr["Level1Name"].ToString();
        //        _Code = sqldr["Code"].ToString();
        //        _Name = sqldr["Name"].ToString();
        //        _Locked = bool.Parse(sqldr["Locked"].ToString());
        //        _Sort = int.Parse(sqldr["Sort"].ToString());
        //        _EntryDate = sqldr["EntryDate"].ToString();
        //        _EntryUser = sqldr["EntryUser"].ToString();
        //    }
        //    sqldr.Close();
        //    if (con.State == ConnectionState.Open)
        //    {
        //        con.Close();
        //    }
        //}
        public string getUpdateMasterCount(string groupcode)
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = @"select top 1 case when code + 1 <10 then replicate('0',1)+ cast((code+1) as varchar)else cast((code + 1) as varchar)end as code
 from Item_SubCategory where Level1Code = '" + groupcode + "' order by id desc";
            //strsql = "select inull(code) + 1 as code from tblAccountsL2_L  where level1code = '" + groupcode +"'";   
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
            if (no.Trim() == "") no = "01";
            return no;
        }
        public string getbrandname(string code)
        {
           
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = @"select Name    from ItemCategory where Code = '" + code + "'";
          
            //strsql = "select inull(code) + 1 as code from tblAccountsL2_L  where level1code = '" + groupcode +"'";   
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            sqldr = dp.ExecuteReader(con, strsql);
            if (sqldr.Read())
            {
                no = sqldr["Name"].ToString();
                Console.WriteLine("inside controller");
            }
            sqldr.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (no.Trim() == "") no = "nobrand";
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
        public void Set_b(int Mode, M_SubCategory m)
        {
            DataProcess DataProcess = new DataProcess();
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "Execute sp_ItemSubCategory " + Mode.ToString() + ",";

            strsql += "'" + m.COMPANY_CODE.Replace("'", "''") + "',";
            strsql += "'" + m.Level1Code.Replace("'", "''") + "',";
            strsql += "'" + m.Level1Name.Replace("'", "''") + "',";
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
