using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreInfrastructure.Auth.User;
using Microsoft.AspNetCore.Cors;
using MechSuitsApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using MechSuitsApi.Classes;
using Executer;
using CoreInfrastructure.AccomodationSystem;

namespace MechSuitsApi.Areas.Auth.Controllers
{
    [Authorize]
    [Route("api/chlynkya")]
    [ApiController]
    [EnableCors("AllowOrigin")]

    public class UsersController : ControllerBase
    {
        Connection c = new Connection();
        SqlConnection con;

        string companycode = "";   string user = "";
        private readonly IJwtAuthManager _jwtAuthManager;
        public UsersController(IJwtAuthManager jwtAuthManager)
        {
            _jwtAuthManager = jwtAuthManager;
            con = c.V_connection;
        }
        [HttpGet]
        
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(M_LoginUser m)
        {
            var token = _jwtAuthManager.Authenticate(m.USER_NAME, m.USER_PASSWORD, m.Year);
            string ls_query = @"SELECT    COMPANY_CODE		  ,CODE	,Name	  ,ROLE_CODE   ,USER_PASSWORD ,ACTIVE ,
            FIRST_NAME ,LAST_NAME 
            FROM dbo.APP_USERS where  NAME='" + m.USER_NAME + "' and USER_PASSWORD='" + m.USER_PASSWORD + "'";

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlDataAdapter sda = new SqlDataAdapter(ls_query, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            M_Users u = null;
            if (dt.Rows.Count > 0)
            {
                u = new M_Users();
                //  m.ID = int.Parse(sqldr["ID"].ToString());
                u.COMPANY_CODE = dt.Rows[0].ItemArray[0].ToString();
                u.USER_CODE = dt.Rows[0].ItemArray[1].ToString();
                u.USER_NAME = dt.Rows[0].ItemArray[2].ToString();
                u.ROLE_CODE = dt.Rows[0].ItemArray[3].ToString();
                u.USER_PASSWORD = dt.Rows[0].ItemArray[4].ToString();
                u.FIRST_NAME = dt.Rows[0].ItemArray[6].ToString();
                u.LAST_NAME = dt.Rows[0].ItemArray[7].ToString();

            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }




            Console.WriteLine("///////////////////////////////////////////////////////////////////");
            Console.WriteLine(m.Year);
            Console.WriteLine("////////////////////////////////////////////////////////////////////////////////////");
            var response = new
            {
                
                 token,u.ROLE_CODE,u.FIRST_NAME,u.USER_CODE
            };

            var json = "";
            ////////////////////////
           
            if (token == null|| response.ROLE_CODE==null    )
            {
                return Unauthorized();
            }
            ///////////////////
            if (token == null)
            {
                return Unauthorized();
            }
            else
            {
                
                  json = JsonConvert.SerializeObject(response);

            }
            return new OkObjectResult(json);
        }

       public string getroles(string ROLE_CODE,string id)
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            string sql = " select * from APP_USERS as U inner join  APP_USERS_ROLES as R  on   U.ROLE_CODE=R.ROLE_ID  where U.CODE = '" + id + "' and R.ACTIVE = 1";
            return "ok";
        }
        //[AllowAnonymous]
        //[HttpPost("authenticates")]
        //public IActionResult Authenticate(string USER_NAME,string USER_PASSWORD)
        //{
        //    var token = _jwtAuthManager.Authenticate(USER_NAME, USER_PASSWORD,"101");
        //    var response = new
        //    {
        //        auth_token = token
        //    };

        //    var json = "";
        //    if (token == null)
        //    {
        //        return Unauthorized();
        //    }
        //    else
        //    {
        //        Console.WriteLine(JsonConvert.SerializeObject(response));
        //        json = JsonConvert.SerializeObject(response);
        //        Console.WriteLine(json);
        //    }
        //    return new OkObjectResult(json);
        //}
    }

}
