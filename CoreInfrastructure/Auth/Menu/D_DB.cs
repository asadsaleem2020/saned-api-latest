using Executer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CoreInfrastructure.Auth.Menu
{
    public class D_DB
    {

        public string Rols_id = "";
        private SqlConnection con;

        public D_DB(SqlConnection V_connection)
        {
            con = V_connection;

        }
        #region Get Members Populated
        public List<M_MainModel> GetMainMenuList(string whereCondition)
        {
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open(); 
            }

            //strsql = "select * from  app_module  where active =1 and module_id<>99 order by sort";
            strsql = @"select * from  app_module  where active =1 and module_id<>99 
            and module_id in (select MODULE_ID from app_module_menu where menu_id in (
            select  menu_id from app_users_roles where ACTIVE=1 and ROLE_ID='" + whereCondition + "')) order by sort";
            SqlCommand cmd = new SqlCommand(strsql, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable _dt = new DataTable();
            sda.Fill(_dt);
            List<M_MainModel> list = null;
            if (_dt.Rows.Count > 0)
            {
                list = new List<M_MainModel>();
                foreach (DataRow sqldr in _dt.Rows)
                {
                    var fields = new M_MainModel();
                    //  fields.ID = int.Parse(sqldr["ID"].ToString());
                    fields.MODULE_ID = int.Parse(sqldr["MODULE_ID"].ToString());
                    fields.MODULE_NAME = sqldr["MODULE_NAME"].ToString();
                    fields.Module_Icon = sqldr["Module_Icon"].ToString();
                    fields.HAS_MENU_LEVEL = int.Parse(sqldr["HAS_MENU_LEVEL"].ToString());
                    fields.ACTIVE = bool.Parse(sqldr["ACTIVE"].ToString());
                    fields.SORT = int.Parse(sqldr["SORT"].ToString());
                    fields.DetailRows = GetMainMenuDetailList("MODULE_ID=" + int.Parse(sqldr["MODULE_ID"].ToString()) + " and r.role_id=" + whereCondition);

                    list.Add(fields);
                }
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return list;
        }
        public List<M_DetailModel> GetMainMenuDetailList(string whereCondition)
        {
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = @"   select MENU_NAME, MENU_AREA, CONTROLLER, ACTION from APP_USERS_ROLES as
            R INNER JOIN APP_MODULE_MENU AS M ON M.MENU_ID = R.MENU_ID
            where r.active=1 and    " + whereCondition + " ORDER BY MODULE_MENU_SORT";
            SqlCommand cmd = new SqlCommand(strsql, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable _dt = new DataTable();
            sda.Fill(_dt);
            List<M_DetailModel> list = null;
            if (_dt.Rows.Count > 0)
            {
                list = new List<M_DetailModel>();
                foreach (DataRow sqldr in _dt.Rows)
                {
                    var fields = new M_DetailModel();
                    //  fields.ID = int.Parse(sqldr["ID"].ToString());
                    fields.MENU_AREA = sqldr["MENU_AREA"].ToString();
                    fields.MENU_NAME = sqldr["MENU_NAME"].ToString();
                    fields.CONTROLLER = sqldr["CONTROLLER"].ToString();
                    fields.ACTION = sqldr["ACTION"].ToString();


                    list.Add(fields);
                }
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            /////////////////////////////////////////////
            Console.WriteLine(list.ToString());
            return list;
        }
        public long GetDashBoardAvailbility(string whereCondition)
        {
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = @"  select Count(1) count from APP_USERS_ROLES as R
            INNER JOIN APP_MODULE_MENU AS M ON M.MENU_ID = R.MENU_ID
            where r.active=1 and m.menu_id=1001 and m.active=1 and     " + whereCondition;
            SqlCommand cmd = new SqlCommand(strsql, con);
            long l = Convert.ToInt64(cmd.ExecuteScalar());
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return l;
        }
        //       public M_ModalReports GetUserReportList(string whereCondition)
        //       {
        //           string strsql;
        //           if (con.State == ConnectionState.Closed)
        //           {
        //               con.Open();
        //           }
        //           strsql = @"  select MENU_NAME, MENU_AREA, CONTROLLER, upper(ACTION) from APP_USERS_ROLES as R INNER JOIN APP_MODULE_MENU AS M ON M.MENU_ID = R.MENU_ID
        //WHERE M.MODULE_ID=99 AND " + whereCondition + " ORDER BY MODULE_MENU_SORT";
        //           SqlCommand cmd = new SqlCommand(strsql, con);
        //           SqlDataAdapter sda = new SqlDataAdapter(cmd);
        //           DataTable _dt = new DataTable();
        //           sda.Fill(_dt);
        //           M_ModalReports m = new M_ModalReports();

        //           foreach (DataRow sqldr in _dt.Rows)
        //           {
        //               m.LEDGER = bool.Parse(sqldr["LEDGER"].ToString());
        //               m.R_DAILY_VOUCHER_LIST = bool.Parse(sqldr["R_DAILY_VOUCHER_LIST"].ToString());
        //               m.R_TRAIL_BALANCE_ALL_LEVEL4 = bool.Parse(sqldr["R_TRAIL_BALANCE_ALL_LEVEL4"].ToString());
        //               m.R_ACCOUNTS_PAYABLE = bool.Parse(sqldr["R_ACCOUNTS_PAYABLE"].ToString());
        //               m.FC_LEDGER = bool.Parse(sqldr["FC_LEDGER"].ToString());
        //               m.TRAIL_BALANCE = bool.Parse(sqldr["TRAIL_BALANCE"].ToString());
        //               m.R_ACCOUNTS_PAYABLE_BALANCE = bool.Parse(sqldr["R_ACCOUNTS_PAYABLE_BALANCE"].ToString());
        //               m.R_CHEQUE_STATUS = bool.Parse(sqldr["R_CHEQUE_STATUS"].ToString());
        //               m.R_ACCOUNTS_RECEIVABLE = bool.Parse(sqldr["R_ACCOUNTS_RECEIVABLE"].ToString());





        //               m.ACCOUNT_LEVEL_3 = bool.Parse(sqldr["ACCOUNT_LEVEL_3"].ToString());
        //               m.R_ACCOUNTSLEVEL2_LIST = bool.Parse(sqldr["R_ACCOUNTSLEVEL2_LIST"].ToString());
        //               m.R_ACCOUNTSLEVEL3_LIST = bool.Parse(sqldr["R_ACCOUNTSLEVEL3_LIST"].ToString());
        //               m.R_ACCOUNTS_LIST = bool.Parse(sqldr["R_ACCOUNTS_LIST"].ToString());
        //               m.R_ITEMCATEGORY_LIST = bool.Parse(sqldr["R_ITEMCATEGORY_LIST"].ToString());
        //               m.R_ITEM_SUBCATEGORY_LIST = bool.Parse(sqldr["R_ITEM_SUBCATEGORY_LIST"].ToString());
        //               m.R_ITEM_SUBCATEGORYDETAIL_LIST = bool.Parse(sqldr["R_ITEM_SUBCATEGORYDETAIL_LIST"].ToString());
        //               m.R_LOCATION_LIST = bool.Parse(sqldr["R_LOCATION_LIST"].ToString());
        //               m.R_ITEM_LIST = bool.Parse(sqldr["R_ITEM_LIST"].ToString());

        //               m.R_CROPYEAR_LIST = bool.Parse(sqldr["R_CROPYEAR_LIST"].ToString());
        //               m.R_MANDI_LIST = bool.Parse(sqldr["R_MANDI_LIST"].ToString());
        //               m.R_QUALITYPARAMETERS_LIST = bool.Parse(sqldr["R_QUALITYPARAMETERS_LIST"].ToString());
        //               m.R_BAG_TYPES_LIST = bool.Parse(sqldr["R_BAG_TYPES_LIST"].ToString());
        //               m.R_PROCESSES_LIST = bool.Parse(sqldr["R_PROCESSES_LIST"].ToString());
        //               m.R_CURRENCY_LIST = bool.Parse(sqldr["R_CURRENCY_LIST"].ToString());
        //               m.R_COUNTRYCITY_LIST = bool.Parse(sqldr["R_COUNTRYCITY_LIST"].ToString());
        //               m.R_CHARGES_SCHEDULING_LIST = bool.Parse(sqldr["R_CHARGES_SCHEDULING_LIST"].ToString());
        //               m.R_LOCATION_LIST = bool.Parse(sqldr["R_LOCATION_LIST"].ToString());



        //               m.R_PURCHASE_CONTRACT = bool.Parse(sqldr["R_PURCHASE_CONTRACT"].ToString());
        //               m.R_PURCHASE_INVOICE = bool.Parse(sqldr["R_PURCHASE_INVOICE"].ToString());
        //               m.R_GRN_NOTE = bool.Parse(sqldr["R_GRN_NOTE"].ToString());
        //               m.R_PARTYWISE_PURCHASE_HISTORY_TOUCH_RATE = bool.Parse(sqldr["R_PARTYWISE_PURCHASE_HISTORY_TOUCH_RATE"].ToString());
        //               m.R_PARTYWISE_PURCHASE_CONTRACT_STATUS = bool.Parse(sqldr["R_PARTYWISE_PURCHASE_CONTRACT_STATUS"].ToString());
        //               m.R_VARIETYWISE_PURCHASE_COST = bool.Parse(sqldr["R_VARIETYWISE_PURCHASE_COST"].ToString());
        //               m.R_PARTYWISE_PURCHASE_HISTORY = bool.Parse(sqldr["R_PARTYWISE_PURCHASE_HISTORY"].ToString());



        //               m.R_SALE_CONTRACT = bool.Parse(sqldr["R_SALE_CONTRACT"].ToString());
        //               m.R_SALE_INVOICE = bool.Parse(sqldr["R_SALE_INVOICE"].ToString());

        //               m.R_SALES_RECOVERY = bool.Parse(sqldr["R_SALES_RECOVERY"].ToString());
        //               m.R_DATEWISE_SALE = bool.Parse(sqldr["R_DATEWISE_SALE"].ToString());
        //               m.R_PARTYWISE_SALE_CONTRACT_STATUS = bool.Parse(sqldr["R_PARTYWISE_SALE_CONTRACT_STATUS"].ToString());
        //               m.R_PROCESSING = bool.Parse(sqldr["R_PROCESSING"].ToString());
        //               m.R_VARIETIES_STOCK_SUMMARY = bool.Parse(sqldr["R_VARIETIES_STOCK_SUMMARY"].ToString());
        //               m.R_UNIT_WISE_STOCK_TRANSFER_SUMMARY = bool.Parse(sqldr["R_UNIT_WISE_STOCK_TRANSFER_SUMMARY"].ToString());
        //               m.R_UNIT_WISE_STOCK_TRANSFER = bool.Parse(sqldr["R_UNIT_WISE_STOCK_TRANSFER"].ToString());
        //               m.R_LOCATIONWISE_STOCK = bool.Parse(sqldr["R_LOCATIONWISE_STOCK"].ToString());
        //               m.R_ITEMWISE_QUANTITATIVE_STOCK = bool.Parse(sqldr["R_ITEMWISE_QUANTITATIVE_STOCK"].ToString());
        //               m.R_VARIETYWISE_SALE_CONTRACT_STATUS = bool.Parse(sqldr["R_VARIETYWISE_SALE_CONTRACT_STATUS"].ToString());
        //               m.R_EMPLOYEE_LEDGER = bool.Parse(sqldr["R_EMPLOYEE_LEDGER"].ToString());
        //               m.R_HR_DESIGNATION_LIST = bool.Parse(sqldr["R_HR_DESIGNATION_LIST"].ToString());
        //               m.R_HR_DEPARTMENT_LIST = bool.Parse(sqldr["R_HR_DEPARTMENT_LIST"].ToString());

        //           }

        //           if (con.State == ConnectionState.Open)
        //           {
        //               con.Close();
        //           }
        //           return m;
        //       }

        public string IsReportVisible(string Rpt_name)
        {


            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            strsql = "  select (case count(1)   when 1 then '' else 'none' end)  from APP_USERS_ROLES as R INNER JOIN APP_MODULE_MENU AS M ON M.MENU_ID = R.MENU_ID WHERE M.MODULE_ID = 99 and R.active = 1   AND ROLE_ID = '" + Rols_id + "' and action = '" + Rpt_name + "' ";
            SqlCommand cmd = new SqlCommand(strsql, con);

            string count = cmd.ExecuteScalar().ToString();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            return count;
        }

        //public long isValid(string username, string password)
        //{


        //    string strsql;
        //    if (con.State == ConnectionState.Closed)
        //    {
        //        con.Open();
        //    }
        //    password = c_encrypt_decrypt.f_encrypt(password.ToUpper());
        //    strsql = "select Count(1) from App_users where   USER_NAME='" + username + "' AND USER_PASSWORD='" + password + "'";
        //    SqlCommand cmd = new SqlCommand(strsql, con);

        //    long count = Convert.ToInt64(cmd.ExecuteScalar());
        //    if (con.State == ConnectionState.Open)
        //    {
        //        con.Close();
        //    }

        //    return count;
        //}
        public long IsAuthorizedRequestFromUserUrl(string whereCondition)
        {


            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            strsql = @"select Count(1) from app_users_roles as r inner join 
            APP_MODULE_MENU as m on r.MENU_ID=m.MENU_ID
            where r.active = 1 and " + whereCondition;
            SqlCommand cmd = new SqlCommand(strsql, con);

            long count = Convert.ToInt64(cmd.ExecuteScalar());
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            return count;
        }
        public string ChkUnChkUserRoles(string role_id, string controller)
        {


            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            strsql = @"select cast(r.unchk as char(1))+','+cast(r.Chk as char(1))+','+cast(r.APPROVE
                        as char(1)) from APP_USERS_ROLES  as r inner join APP_MODULE_MENU as  m
                        on m.MENU_ID=r.MENU_ID where ROLE_ID='" + role_id + "'  and controller='" + controller + "'";
            SqlCommand cmd = new SqlCommand(strsql, con);

            string count = cmd.ExecuteScalar().ToString();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            return count;
        }

        //public bool isValidUserProfile(string username, string password)
        //{
        //    DataProcess dp = new DataProcess();
        //    SqlDataReader sqldr;
        //    bool valid = false;
        //    string strsql;

        //    if (con.State == ConnectionState.Closed)
        //    {
        //        con.Open();
        //    }
        //    password = c_encrypt_decrypt.f_encrypt(password.ToUpper());
        //    strsql = "select * from app_users where user_name= '" + username + "' and user_password = '" + password + "'";
        //    sqldr = dp.ExecuteReader(con, strsql);
        //    //if (sqldr.Read())
        //    //{
        //    //    USER_CODE1 = sqldr["USER_CODE"].ToString();
        //    //    ROLE_CODE1 = sqldr["ROLE_CODE"].ToString();
        //    //    USER_NAME1 = sqldr["USER_NAME"].ToString();
        //    //    ACTIVE1 = sqldr["ACTIVE"].ToString();

        //    //    valid = true;
        //    //}

        //    sqldr.Close();
        //    if (con.State == ConnectionState.Open)
        //    {
        //        con.Close();
        //    }
        //    return valid;
        //}
        public M_MainModel GetSingle(string whereCondition)
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
            M_MainModel fields = null;
            if (sqldr.Read())
            {
                fields = new M_MainModel();
                //    fields.ROLE_ID = sqldr["Role_id"].ToString();

                fields.ACTIVE = bool.Parse(sqldr["Active"].ToString());

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
            strsql = @"SELECT ISNULL(CAST( MAX(ROLE_ID) AS BIGINT) ,0) +1  AS code FROM ROLES";

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
        public void Set_b(int Mode, M_MainModel m)
        {
            DataProcess DataProcess = new DataProcess();
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "Execute sp_roles " + Mode.ToString() + ",";
            //strsql += m.ID + ",";
            //strsql += "'" + m.COMPANY_CODE.Replace("'", "''") + "',";

            if (m.ACTIVE == true)
                strsql += "1,";
            else
                strsql += "0,";

            //   strsql += "'" + m.CREATED_BY.Replace("'", "''") + "'";
            DataProcess.ExecuteTransaction(con, strsql);
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

        }
        #endregion
    }
}
