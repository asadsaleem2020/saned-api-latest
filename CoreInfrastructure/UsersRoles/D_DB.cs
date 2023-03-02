using System;
using System.Data;
using System.Data.SqlClient;
using Executer;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace CoreInfrastructure.UsersRoles
{
    public class D_DB 
    {
        private SqlConnection con;

       

        public D_DB(SqlConnection V_connection)
        {
            con = V_connection;

        }
        public List<M_ModalDetail> GetDetailRowForEdit(string ROLE_ID, string company_code)
        {
            DataProcess dp = new DataProcess();

            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            try
            {
                Console.WriteLine(ROLE_ID);
                Console.WriteLine(company_code);


                strsql = @" select * from (
                    SELECT U.COMPANY_CODE ,U.ROLE_ID ,H.MODULE_ID,H.MODULE_NAME,U.MENU_ID,D.MENU_NAME,D.MODULE_MENU_SORT,
                    U.ACTIVE,
                   U.NEW,U.EDIT,U.DEL,
                    (case d.approve when 'N'  then 1 else U.APPROVE end) as APPROVE,
                    U.CHK,
                    (case d.unchk when 'N'  then 1 else U.UNCHK end) as UNCHK,U.PRNT,U.GETLIST
                    FROM APP_USERS_ROLES AS U INNER JOIN APP_MODULE_MENU AS D 
                    ON D.MENU_ID = U.MENU_ID INNER JOIN APP_MODULE AS H ON H.MODULE_ID=D.MODULE_ID
                    WHERE  h.ACTIVE=1 and d.ACTIVE=1 and ROLE_ID= " + ROLE_ID + " AND COMPANY_CODE='" + company_code + @"'

                    UNION ALL 
                    SELECT COMPANY_CODE='' ,ROLE_ID='' ,H.MODULE_ID,H.MODULE_NAME,D.MENU_ID,D.MENU_NAME,D.MODULE_MENU_SORT,
                    ACTIVE=0,
                    (case new when 'N'  then 1 else 0 end) as new,
                    (case new when 'N'  then 1 else 0 end) as EDIT,
                    (case new when 'N'  then 1 else 0 end) as DEL,
                    (case approve when 'N'  then 1 else 0 end) as approve,
                    (case chk when 'N'  then 1 else 0 end) as chk,
                    (case unchk when 'N'  then 1 else 0 end) as unchk,PRNT=0,GETLIST=1
                    FROM APP_MODULE_MENU AS D  INNER JOIN APP_MODULE AS H ON H.MODULE_ID=D.MODULE_ID
                    WHERE d.ACTIVE=1 and h.ACTIVE=1 and D.MENU_ID NOT IN  
                    (SELECT MENU_ID FROM APP_USERS_ROLES WHERE ROLE_ID=" + ROLE_ID + "" +
                    " AND COMPANY_CODE='" + company_code + "')  )  as t  order by t.role_id, t.module_id,t.menu_id,t.module_menu_sort";
                //strsql = @" select * from (
                //    SELECT U.COMPANY_CODE ,U.ROLE_ID ,H.MODULE_ID,H.MODULE_NAME,U.MENU_ID,D.MENU_NAME,D.MODULE_MENU_SORT,
                //    U.ACTIVE,
                //    (case d.new when 'N'  then 1 else U.NEW end) as NEW,U.EDIT,U.DEL,
                //    (case d.approve when 'N'  then 1 else U.APPROVE end) as APPROVE,
                //    (case d.chk when 'N'  then 1 else U.CHK end) as CHK,
                //    (case d.unchk when 'N'  then 1 else U.UNCHK end) as UNCHK,U.PRNT,U.GETLIST
                //    FROM APP_USERS_ROLES AS U INNER JOIN APP_MODULE_MENU AS D 
                //    ON D.MENU_ID = U.MENU_ID INNER JOIN APP_MODULE AS H ON H.MODULE_ID=D.MODULE_ID
                //    WHERE  h.ACTIVE=1 and d.ACTIVE=1 and ROLE_ID= " + ROLE_ID + " AND COMPANY_CODE='" + company_code + @"'

                //    UNION ALL 
                //    SELECT COMPANY_CODE='' ,ROLE_ID='' ,H.MODULE_ID,H.MODULE_NAME,D.MENU_ID,D.MENU_NAME,D.MODULE_MENU_SORT,
                //    ACTIVE=0,
                //    (case new when 'N'  then 1 else 0 end) as new,
                //    (case new when 'N'  then 1 else 0 end) as EDIT,
                //    (case new when 'N'  then 1 else 0 end) as DEL,
                //    (case approve when 'N'  then 1 else 0 end) as approve,
                //    (case chk when 'N'  then 1 else 0 end) as chk,
                //    (case unchk when 'N'  then 1 else 0 end) as unchk,PRNT=0,GETLIST=1
                //    FROM APP_MODULE_MENU AS D  INNER JOIN APP_MODULE AS H ON H.MODULE_ID=D.MODULE_ID
                //    WHERE d.ACTIVE=1 and h.ACTIVE=1 and D.MENU_ID NOT IN  
                //    (SELECT MENU_ID FROM APP_USERS_ROLES WHERE ROLE_ID=" + ROLE_ID + "" +
                //    " AND COMPANY_CODE='" + company_code + "')  )  as t  order by t.role_id, t.module_id,t.menu_id,t.module_menu_sort";

                SqlCommand cmd = new SqlCommand(strsql, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                Console.WriteLine("Helllo execution");

                List<M_ModalDetail> list = null;
                if (dt.Rows.Count > 0)
                {
                    list = new List<M_ModalDetail>();
                    foreach (DataRow row in dt.Rows)
                    {
                        M_ModalDetail fields = new M_ModalDetail
                        {

                            Company_Code = row["COMPANY_CODE"].ToString(),
                            ROLE_ID = int.Parse(row["ROLE_ID"].ToString()),
                            MODULE_ID = int.Parse(row["MODULE_ID"].ToString()),
                            MODULE_NAME = row["MODULE_NAME"].ToString(),
                            MENU_ID = int.Parse(row["MENU_ID"].ToString()),
                            MENU_NAME = row["MENU_NAME"].ToString(),
                            MODULE_MENU_SORT = row["MODULE_MENU_SORT"].ToString(),
                            NEW = row["NEW"].ToString(),
                            EDIT = row["EDIT"].ToString(),
                            DEL = row["DEL"].ToString(),
                            APPROVE = row["APPROVE"].ToString(),
                            CHK = row["CHK"].ToString(),
                            UNCHK = row["UNCHK"].ToString(),
                            PRNT = row["PRNT"].ToString(),
                            GETLIST = row["GETLIST"].ToString(),
                            ACTIVE = Convert.ToBoolean(row["ACTIVE"])

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
            catch (Exception)
            {
                return null;
            }
            }
       
        public string getUpdateMasterCount(string yearmonth, M_ModalHeader M)
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            string no = "";


            strsql = @"
	 select  isnull(max(cast(right(Invoice_Code, 4) as bigint)),1000)	  as code 
		  from PO_HEADER 
		  where isnumeric(Invoice_Code) = 1 and     left(Invoice_Code ,4 ) ='" + yearmonth + "' and company_Code='" + M.Id + "'";

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
        public int Delete_Voucher_for_Edit_mode(string where)
        {

            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "delete  from app_users_roles where " + where;
            SqlCommand cmd = new SqlCommand(strsql, con);
            int i = cmd.ExecuteNonQuery();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return i;
        }

        public void Set_d(int Mode, M_ModalDetail m)
        {

            DataProcess DataProcess = new DataProcess();
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            strsql = "Execute sp_app_users_roles " + Mode.ToString() + ",";
            strsql += "'" + m.Company_Code.Replace("'", "''") + "',";
            strsql += m.ROLE_ID + ",";
            strsql += m.MENU_ID + ",";
           
                strsql += m.ACTIVE + ",";
            

            if (m.NEW == "on")
            {
                strsql += 1 + ",";
            }
            else
            {
                strsql += 0 + ",";
            }
            strsql += m.EDIT + ",";
            strsql += m.DEL + ",";

            if (m.APPROVE == "on")
            {
                strsql += 1 + ",";
            }
            else
            {
                strsql += 0 + ",";
            }

            if (m.CHK == "on")
            {
                strsql += 1 + ",";
            }
            else
            {
                strsql += 0 + ",";
            }

            if (m.UNCHK == "on")
            {
                strsql += 1 + ",";
            }
            else
            {
                strsql += 0 + ",";
            }
            strsql += m.PRNT + ",";
            strsql += m.GETLIST;
            DataProcess.ExecuteTransaction(con, strsql);




            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }
}

