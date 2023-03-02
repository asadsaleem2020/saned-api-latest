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
    [Route("api/salary")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class SalaryController : Controller
    {
        private IHttpContextAccessor _httpContextAccessor;

        private readonly AppDBContext _context;
        private SqlConnection con;
        string companycode = "";   string user = "";
        Connection c = new Connection();
        public SalaryController(AppDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            con = c.V_connection;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
        //  companycode = currentUser.FindFirst("Company").Value.ToString();    user = currentUser.FindFirst("User").Value.ToString();
        }
        //   GET: api/Departments
        //[HttpGet("{employeecode}/{dept}/{sec}/{startdate}/{enddate}")]
        //public async Task<ActionResult<IEnumerable<M_AttendanceTable>>> GetAttendace(string employeecode, string dept, string sec, string startdate, string enddate)
        //{



        //    string where = "";

        //    string ls_emp = "";
        //    if (employeecode == "0")
        //    {
        //        ls_emp = " 1=1 ";
        //    }
        //    else
        //    {
        //        ls_emp = "EMPLOYEE_CODE ='" + employeecode + "' ";
        //    }
        //    string ls_dpt = "0";
        //    if (dept == "0")
        //    {
        //        ls_dpt = " 1=1";
        //    }
        //    else
        //    {
        //        ls_dpt = " DEPARTMENT = '" + dept + "' ";
        //    }
        //    string ls_sec = "";
        //    if (sec == "0")
        //    {
        //        ls_sec = "   1=1";
        //    }
        //    else
        //    {
        //        ls_sec = " SECTION='" + sec + "' ";
        //    }

        //    where = ls_emp + " AND " + ls_dpt + "  and  " + ls_sec + "  and  ATTENDANCE_DATE between '" + startdate + "'  and '" + enddate + "'";
        //    List<M_AttendanceTable> result = GetList(where);
        //    return result;
        //}
        [HttpPost]
        [Route("Generate")]
        public List<M_SalaryTable> Generate(int Mode, M_Salary m)
        {



            DateTime ldt_startmonth, ldt_endmonth, ldt_date;


            ldt_date = Convert.ToDateTime(m.Date_From);
            ldt_startmonth = new DateTime(ldt_date.Year, ldt_date.Month, 1).Date;
            ldt_endmonth = new DateTime(ldt_date.Year, ldt_date.Month, 1).AddMonths(1).AddDays(-1).Date;

            int dateend = ldt_endmonth.Day;


            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }



            DataProcess DataProcess = new DataProcess();
            string strsql;

            strsql = "Execute DBO.SP_GENERATE_SALARY";
            strsql += "'" + ldt_startmonth + "',";
            strsql += "'" + ldt_endmonth + "',";
            strsql += "'0',";
            strsql += dateend + ",";
            strsql += "'" + companycode + "'";


            DataProcess.ExecuteTransaction(con, strsql);




            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
         return    GetList(ldt_endmonth);


        }

        public List<M_SalaryTable> GetList(DateTime ldt_endmonth)
        {
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = @"select *,(select NAME from hr_employee WHERE hr_employee.EMPLOYEE_CODE= EMP_ID) AS ENAME from HR_SALARY_DETAIL " +
                " where  Period_id =cast(Month('" + ldt_endmonth+"') as varchar)+'-'+cast(Year('"+ldt_endmonth+"') as varchar)";
            SqlCommand cmd = new SqlCommand(strsql, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable _dt = new DataTable();
            sda.Fill(_dt);
            List<M_SalaryTable> list = new List<M_SalaryTable>();
            if (_dt.Rows.Count > 0)
            {
                list = new List<M_SalaryTable>();
                foreach (DataRow sqldr in _dt.Rows)
                {
                    M_SalaryTable fields = new M_SalaryTable
                    {
                        COMPANY_CODE = sqldr["COMPANY_CODE"].ToString(),
                        PERIOD_ID = sqldr["PERIOD_ID"].ToString(),
                        EMP_ID = sqldr["EMP_ID"].ToString(),
                        EMPLOYEE_NAME = sqldr["ENAME"].ToString(),
                        PAYMENT_DATE = DateTime.Parse(sqldr["PAYMENT_DATE"].ToString()),
                        PRESENT_DAYS = int.Parse(sqldr["PRESENT_DAYS"].ToString()),
                        ABSENTS = int.Parse(sqldr["ABSENTS"].ToString()),
                        LEAVES = int.Parse(sqldr["LEAVES"].ToString()),
                        HOLIDAYS = int.Parse(sqldr["HOLIDAYS"].ToString()),
                        GAZTTED_HOLIDAYS = int.Parse(sqldr["GAZTTED_HOLIDAYS"].ToString()),
                        PAIDDAYS = int.Parse(sqldr["PAIDDAYS"].ToString()),
                        UNPIADDAYS = int.Parse(sqldr["UNPIADDAYS"].ToString()),
                        BASIC_SALARY = decimal.Parse(sqldr["BASIC_SALARY"].ToString()),
                        ALLOWANCE = decimal.Parse(sqldr["ALLOWANCE"].ToString()),
                        MEAL_ALLOWANCE = decimal.Parse(sqldr["MEAL_ALLOWANCE"].ToString()),
                        MEDICAL_ALLOWANCE = decimal.Parse(sqldr["MEDICAL_ALLOWANCE"].ToString()),
                        TRAVEL_CAR_ALLOWANCE = decimal.Parse(sqldr["TRAVEL_CAR_ALLOWANCE"].ToString()),
                        MOBILE_ALLOWANCE = decimal.Parse(sqldr["MOBILE_ALLOWANCE"].ToString()),
                        PA = decimal.Parse(sqldr["PA"].ToString()),
                        MESS = decimal.Parse(sqldr["MESS"].ToString()),
                        EOBI = decimal.Parse(sqldr["EOBI"].ToString()),
                        QTR = decimal.Parse(sqldr["QTR"].ToString()),
                        Other_DEDUCTION = decimal.Parse(sqldr["Other_DEDUCTION"].ToString()),
                        ADVANCE = decimal.Parse(sqldr["ADVANCE"].ToString()),
                        STL_AMOUNT = decimal.Parse(sqldr["STL_AMOUNT"].ToString()),
                        INSTALLMENT = decimal.Parse(sqldr["INSTALLMENT"].ToString()),
                        STATUS = sqldr["STATUS"].ToString(),
                        IS_LOCKED = sqldr["IS_LOCKED"].ToString(),
                        OverTimeHrs = decimal.Parse(sqldr["OverTimeHrs"].ToString()),
                        OverTimeAmount = decimal.Parse(sqldr["OverTimeAmount"].ToString()),
                        GROSS_SALARY = decimal.Parse(sqldr["GROSS_SALARY"].ToString()),
                        NET_SALARY = decimal.Parse(sqldr["NET_SALARY"].ToString()),



                    };
                    list.Add(fields);


                }
            }
            return list;
        }
        [HttpGet("{employeecode}/{startdate}/{status}")]
        public string UpdateStatus(string employeecode, string startdate, string status)
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
