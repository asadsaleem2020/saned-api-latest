using System;
using System.Data;
using System.Data.SqlClient;
using Executer;
using System.Collections.Generic;
namespace CoreInfrastructure.ItemInformation.ItemSubCategoryDetail
{
  public  class D_DB 
    {
        private SqlConnection con;

        public D_DB(SqlConnection V_connection)
        {
            con = V_connection;

        }

        #region Get Members Populated
        public string getUpdateMasterCount(string groupcode, string level1)
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = @"select top 1 case when code + 1 <100 then replicate('0',2)+ cast((code+1) as varchar)else cast((code + 1) as varchar)end as code
 from Item_SubCategoryDetail where Level2Code = '" + groupcode + "' and level1Code = '" + level1 + "'   order by id desc";
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
            if (no.Trim() == "") no = "001";
            return no;
        }
        public List<M_SubCategoryDetail> GetList(string whereCondition)
        {
            
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "select * from Item_SubCategoryDetail where   " + whereCondition;
            SqlCommand cmd = new SqlCommand(strsql, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable _dt = new DataTable();
            sda.Fill(_dt);
            List<M_SubCategoryDetail> list = null;
            if (_dt.Rows.Count > 0)
            {
                list = new List<M_SubCategoryDetail>();
                foreach (DataRow sqldr in _dt.Rows)
                {
                    var fields = new M_SubCategoryDetail(); 
                   
                    fields.Level1Code = sqldr["Level1Code"].ToString();
                    fields.Level1Name = sqldr["Level1Name"].ToString();
                    fields.Level2Code = sqldr["Level2Code"].ToString();
                    fields.Level2Name = sqldr["Level2Name"].ToString();
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
        public M_SubCategoryDetail GetSingle(string whereCondition)
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "select * from Item_SubCategoryDetail where   " + whereCondition;
            sqldr = dp.ExecuteReader(con, strsql);
            M_SubCategoryDetail fields = null;
            if (sqldr.Read())
            { 
                fields = new M_SubCategoryDetail();

                
                fields.Level1Code = sqldr["Level1Code"].ToString();
                fields.Level1Name = sqldr["Level1Name"].ToString();
                fields.Level2Code = sqldr["Level2Code"].ToString();
                fields.Level2Name = sqldr["Level2Name"].ToString();
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

        public long check_is_new_exists(string WhereCondition)
        {
            Executer.DataProcess dp = new Executer.DataProcess();
            DataSet ds = new DataSet();
            string strSQL;
            strSQL = @"   SELECT isnull(COunt(1),0)  FROM Chart_of_Accounts where  " + WhereCondition;
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

        public void Set_b(int Mode, M_SubCategoryDetail m)
        {
            DataProcess DataProcess = new DataProcess();
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "Execute sp_Item_SubCategoryDetail " + Mode.ToString() + ",";
             
            strsql += "'" + m.Company_Code.Replace("'", "''") + "',";
            strsql += "'" + m.Level1Code.Replace("'", "''") + "',";
            strsql += "'" + m.Level1Name.Replace("'", "''") + "',";
            strsql += "'" + m.Level2Code.Replace("'", "''") + "',";
            strsql += "'" + m.Level2Name.Replace("'", "''") + "',";
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
