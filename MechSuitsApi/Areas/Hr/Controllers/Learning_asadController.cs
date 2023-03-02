//using System;
//using System.Data;
//using System.Data.SqlClient;
//using Executer;
//using System.Collections.Generic;

//namespace NuGetSDB.HR.HREMS.AutoAttendance
//{
//    public class D_DB
//    {
//        private SqlConnection con;
//        private Connection GetConnection;

//        public D_DB()
//        {
//            GetConnection = new Connection();
//            con = GetConnection.V_connection;
//        }
//        //    #region Get Members Populated
//        public List<M_Modal> GetList(string whereCondition)
//        {
//            string strsql;
//            if (con.State == ConnectionState.Closed)
//            {
//                con.Open();
//            }
//            strsql = "select * from HR_DAILY_ATTENDENCE_DETAIL where   " + whereCondition;
//            SqlCommand cmd = new SqlCommand(strsql, con);
//            SqlDataAdapter sda = new SqlDataAdapter(cmd);
//            DataTable _dt = new DataTable();
//            sda.Fill(_dt);
//            List<M_Modal> list = new List<M_Modal>();
//            if (_dt.Rows.Count > 0)
//            {
//                list = new List<M_Modal>();
//                foreach (DataRow sqldr in _dt.Rows)
//                {
//                    M_Modal fields = new M_Modal
//                    {
//                        COMPANY_CODE = sqldr["COMPANY_CODE"].ToString(),
//                        ATTENDANCE_DATE = DateTime.Parse(sqldr["ATTENDANCE_DATE"].ToString()),
//                        ATTENDANCE_DAY = int.Parse(sqldr["ATTENDANCE_DAY"].ToString()),
//                        EMPLOYEE_CODE = sqldr["EMPLOYEE_CODE"].ToString(),
//                        EMPLOYEE_NAME = sqldr["EMPLOYEE_NAME"].ToString(),
//                        DEPARTMENT = sqldr["DEPARTMENT"].ToString(),
//                        DESIGNATION = sqldr["DESIGNATION"].ToString(),
//                        SECTION = sqldr["SECTION"].ToString(),
//                        SHIFT_ID = sqldr["SHIFT_ID"].ToString(),
//                        REMARKS = sqldr["REMARKS"].ToString(),
//                        STATUS = sqldr["STATUS"].ToString(),
//                        TIME_IN = DateTime.Parse(sqldr["TIME_IN"].ToString()),

//                        TIME_OUT = DateTime.Parse(sqldr["TIME_OUT"].ToString()),
//                        WORKING_HOURS = decimal.Parse(sqldr["WORKING_HOURS"].ToString()),
//                        OVERTIME = Convert.ToDateTime(sqldr["OVERTIME"].ToString())
//                    };
//                    list.Add(fields);


//                }
//            }
//            return list;
//        }

//        //public M_Modal GetSingle(string whereCondition)
//        //{
//        //    DataProcess dp = new DataProcess();
//        //    SqlDataReader sqldr;
//        //    string strsql;
//        //    if (con.State == ConnectionState.Closed)
//        //    {
//        //        con.Open();
//        //    }
//        //    strsql = "select * from HR_DESIGNATION where   " + whereCondition;
//        //    sqldr = dp.ExecuteReader(con, strsql);
//        //    M_Modal fields = null;
//        //    if (sqldr.Read())
//        //    {
//        //        fields = new M_Modal();
//        //        fields.ID = int.Parse(sqldr["ID"].ToString());
//        //        fields.Code = sqldr["Code"].ToString();
//        //        fields.Name = sqldr["Name"].ToString();
//        //        fields.Locked = bool.Parse(sqldr["Locked"].ToString());
//        //        fields.Sort = int.Parse(sqldr["Sort"].ToString());
//        //    }
//        //    return fields;
//        //}
//        //public string getUpdateMasterCount()
//        //{
//        //    DataProcess dp = new DataProcess();
//        //    SqlDataReader sqldr;
//        //    string strsql;
//        //    string no = "";
//        //    strsql = @"SELECT ISNULL(CAST( MAX(CODE) AS BIGINT) ,0) +1  AS code FROM HR_DESIGNATION";

//        //    if (con.State == ConnectionState.Closed)
//        //    {
//        //        con.Open();
//        //    }

//        //    sqldr = dp.ExecuteReader(con, strsql);
//        //    if (sqldr.Read())
//        //    {
//        //        no = sqldr["code"].ToString().Trim();
//        //    }
//        //    sqldr.Close();
//        //    if (con.State == ConnectionState.Open)
//        //    {
//        //        con.Close();
//        //    }
//        //    if (no.Trim() == "") no = "1";
//        //    return no;
//        //}
//        //public long check_is_new_exists(string WhereCondition)
//        //{
//        //    Executer.DataProcess dp = new Executer.DataProcess();
//        //    DataSet ds = new DataSet();
//        //    string strSQL;
//        //    strSQL = @"   SELECT isnull(COunt(1),0)  FROM a_accounts_level_3 where  " + WhereCondition;
//        //    if (con.State == ConnectionState.Closed)
//        //    {
//        //        con.Open();
//        //    }
//        //    SqlCommand cmd = new SqlCommand(strSQL, con);
//        //    long count = Convert.ToInt64(cmd.ExecuteScalar());


//        //    if (con.State == ConnectionState.Open)
//        //    {
//        //        con.Close();
//        //    }

//        //    return count;
//        //}
//        //#endregion

//        #region Set Method for Members
//        public void Set_b(int Mode, M_Modal m)
//        {
//            DataProcess DataProcess = new DataProcess();
//            string strsql;
//            if (con.State == ConnectionState.Closed)
//            {
//                con.Open();
//            }
//            strsql = "Execute DBO.SP_INSERT_ATTENDANCE " + Mode.ToString() + ",";

//            strsql += "'" + m.COMPANY_CODE.Replace("'", "''") + "',";
//            strsql += "'" + m.DEPARTMENT.Replace("'", "''") + "',";
//            strsql += "'" + m.SECTION.Replace("'", "''") + "',";
//            strsql += "'" + m.EMPLOYEE_CODE.Replace("'", "''") + "',";
//            strsql += "'" + m.TIME_IN + "',";
//            strsql += "'" + m.STATUS.Replace("'", "''") + "'";



//            DataProcess.ExecuteTransaction(con, strsql);
//            if (con.State == ConnectionState.Open)
//            {
//                con.Close();
//            }
//        }
//        public void Set_machine(string[] as_array)
//        {
//            DataProcess DataProcess = new DataProcess();
//            string strsql;
//            if (con.State == ConnectionState.Closed)
//            {
//                con.Open();
//            }
//            strsql = "Execute DBO.[SP_machineatt] ";

//            strsql += "'" + as_array[0].Trim().Replace("'", "''") + "',";
//            strsql += "'" + as_array[1].Trim().Replace("'", "''") + "',";
//            strsql += "'" + as_array[2].Trim().Replace("'", "''") + "',";
//            strsql += "'" + as_array[3].Trim().Replace("'", "''") + "',";
//            strsql += "'" + as_array[4].Trim() + "',";
//            strsql += "'" + as_array[5].Trim().Replace("'", "''") + "',";
//            strsql += "'" + as_array[6].Trim().Replace("'", "''") + "',";
//            strsql += "'" + as_array[7].Trim().Replace("'", "''") + "'";
//            DataProcess.ExecuteTransaction(con, strsql);
//            if (con.State == ConnectionState.Open)
//            {
//                con.Close();
//            }
//        }
//        #endregion
//    }
//}