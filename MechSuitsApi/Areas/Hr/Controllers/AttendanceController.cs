using Executer;
using MechSuitsApi.Classes;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CoreInfrastructure.Hr.Setup;
using System.Data;
using Microsoft.EntityFrameworkCore;
using CoreInfrastructure.Generals;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MechSuitsApi.Areas.Hr.Controllers
{
   // [Authorize]
    [Route("api/attendance")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class AttendanceController : Controller
    {
        private readonly AppDBContext _context;
        private SqlConnection con;
        string companycode = "";   string user = "";
        private IHttpContextAccessor _httpContextAccessor;
        Connection c = new Connection();
        public AttendanceController(AppDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            con = c.V_connection;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
          //companycode = currentUser.FindFirst("Company").Value.ToString();    user = currentUser.FindFirst("User").Value.ToString();
        }
     //   GET: api/Departments
       [HttpGet("{employeecode}/{dept}/{sec}/{startdate}/{enddate}")]
        public async Task<ActionResult<IEnumerable<M_AttendanceTable>>> GetAttendace(string employeecode, string dept, string sec ,string startdate,string enddate)
        {
          
           

            string where = "";

            string ls_emp = "";
            if (employeecode == "0")
            {
                ls_emp = " 1=1 ";
            }
            else
            {
                ls_emp = "EMPLOYEE_CODE ='" +employeecode+"' ";
            }
            string ls_dpt = "0";
            if (dept == "0")
            {
                ls_dpt = " 1=1";
            }
            else
            {
                ls_dpt = " DEPARTMENT = '"+ dept+"' ";
            }
            string ls_sec = "";
            if (sec == "0")
            {
                ls_sec = "   1=1";
            }
            else
            {
                ls_sec = " SECTION='"+ sec +"' ";
            }
           
            where =     ls_emp + " AND " + ls_dpt  + "  and  " + ls_sec +  "  and  ATTENDANCE_DATE between '" + startdate + "'  and '" + enddate + "'";
            List<M_AttendanceTable> result = GetList(where);
            return   result;
        }
        [HttpPost]
        [Route("GenerateAttendance")]
        public void GenerateAttendance(int Mode, M_Attendance m)
        {
           
          

            DateTime ldt_startdate, ldt_enddate;
            ldt_startdate = Convert.ToDateTime(m.Date_From);
            ldt_enddate = Convert.ToDateTime(m.Date_To);
            int datefrom = ldt_startdate.Day;
            int dateend = ldt_enddate.Day;


            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            for (int i = datefrom; i <= dateend; i++)
                {
                    DateTime att_date = new DateTime(ldt_startdate.Year, ldt_startdate.Month, i);
                   

                   
                      
                    DataProcess DataProcess = new DataProcess();
                string strsql;

                strsql = "Execute DBO.SP_INSERT_ATTENDANCE " + Mode.ToString() + ",";         

                strsql += "'" + m.COMPANY_CODE.Replace("'", "''") + "',";
                strsql += "'" + m.DEPARTMENT.Replace("'", "''") + "',";
                strsql += "'" + m.SECTION.Replace("'", "''") + "',";
                strsql += "'" + m.EMPLOYEE_CODE.Replace("'", "''") + "',";
                strsql += "'" + att_date + "',";
                strsql += "'" + m.Default_Status.Replace("'", "''") + "'";

                DataProcess.ExecuteTransaction(con, strsql);


            }

            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }



        }
        
        private List<M_AttendanceTable> GetList(string whereCondition)
        {
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "select * from HR_DAILY_ATTENDENCE_DETAIL where   " + whereCondition;
            SqlCommand cmd = new SqlCommand(strsql, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable _dt = new DataTable();
            sda.Fill(_dt);
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            List<M_AttendanceTable> list = new List<M_AttendanceTable>();
            if (_dt.Rows.Count > 0)
            {
                list = new List<M_AttendanceTable>();
                foreach (DataRow sqldr in _dt.Rows)
                {
                    M_AttendanceTable fields = new M_AttendanceTable
                    {
                        COMPANY_CODE = sqldr["COMPANY_CODE"].ToString(),
                        ATTENDANCE_DATE = DateTime.Parse(sqldr["ATTENDANCE_DATE"].ToString()),
                        ATTENDANCE_DAY = int.Parse(sqldr["ATTENDANCE_DAY"].ToString()),
                        EMPLOYEE_CODE = sqldr["EMPLOYEE_CODE"].ToString(),
                        EMPLOYEE_NAME = sqldr["EMPLOYEE_NAME"].ToString(),
                        DEPARTMENT = sqldr["DEPARTMENT"].ToString(),
                        DESIGNATION = sqldr["DESIGNATION"].ToString(),
                        SECTION = sqldr["SECTION"].ToString(),
                        SHIFT_ID = sqldr["SHIFT_ID"].ToString(),
                        REMARKS = sqldr["REMARKS"].ToString(),
                        STATUS = sqldr["STATUS"].ToString(),
                        TIME_IN = DateTime.Parse(sqldr["TIME_IN"].ToString()),

                        TIME_OUT = DateTime.Parse(sqldr["TIME_OUT"].ToString()),
                        WORKING_HOURS = decimal.Parse(sqldr["WORKING_HOURS"].ToString()),
                        OVERTIME = Convert.ToDateTime(sqldr["OVERTIME"].ToString())
                    };
                    list.Add(fields);


                }
            }
            return list;
        }
        [HttpGet("{employeecode}/{startdate}/{status}")]
        public string UpdateStatus(string employeecode , string startdate,string status)
        {
           
            string WhereCondition = "";
            WhereCondition = "";
            string updatequery = "";
           
            
                updatequery = "STATUS='" + status.Trim() + "'";
                WhereCondition = " ATTENDANCE_DATE='" + startdate + "' AND EMPLOYEE_CODE='" + employeecode + "' AND   COMPANY_CODE='" + companycode + "'";

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            string query = "Update HR_DAILY_ATTENDENCE_DETAIL Set " + updatequery + " where " + WhereCondition;

            SqlCommand cmd = new SqlCommand(query, con);
            long l = cmd.ExecuteNonQuery();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return "Done";


        }
        public void MarkAttendance(int Mode, M_Attendance m)
        {
            
           
        }
    }

}