using MechSuitsApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using CoreInfrastructure.Auth;
using CoreInfrastructure.Auth.User;
using System.Data.SqlClient;
using System.Data;

namespace MechSuitsApi.Classes.Auth
{
    public class JwtAuthManager : IJwtAuthManager
    {
        private readonly string key;
        private   AppDBContext _context;
        Connection c = new Connection();
        SqlConnection con;
        public JwtAuthManager(string key)
        {
            this.key = key;
            con = c.V_connection;


        }
        public string Authenticate(string username, string password,string year)
        {
            string ls_query = @"SELECT    COMPANY_CODE		  ,CODE	,Name	  ,ROLE_CODE   ,USER_PASSWORD ,ACTIVE ,FIRST_NAME ,LAST_NAME FROM dbo.APP_USERS where  NAME='" + username+"' and USER_PASSWORD='"+password+"'";

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlDataAdapter sda = new SqlDataAdapter(ls_query, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            M_Users m = null;
            if (dt.Rows.Count > 0)
            {
                m = new M_Users(); 
            //  m.ID = int.Parse(sqldr["ID"].ToString());
                m.COMPANY_CODE = dt.Rows[0].ItemArray[0].ToString();
                m.USER_CODE = dt.Rows[0].ItemArray[1].ToString();
                m.USER_NAME = dt.Rows[0].ItemArray[2].ToString();
                m.ROLE_CODE = dt.Rows[0].ItemArray[3].ToString();
                m.USER_PASSWORD = dt.Rows[0].ItemArray[4].ToString(); 
                m.FIRST_NAME = dt.Rows[0].ItemArray[6].ToString();
                m.LAST_NAME = dt.Rows[0].ItemArray[7].ToString(); 

            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (m == null)
            {
                return "Token is expired";
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {

                    new Claim("Company", m.COMPANY_CODE),
                    new Claim("User", m.USER_CODE),
                    new Claim("Year", year),
                     new Claim("Role",  m.ROLE_CODE),


                    // new Claim(ClaimTypes.Role, m. )
                }
                ),
                  Expires = DateTime.UtcNow.AddHours(5),
              //  Expires = DateTime.UtcNow.AddMinutes(5),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
           
            var token = tokenHandler.CreateToken(tokenDescriptor);
            /**Console.WriteLine(token.Id);
            Console.WriteLine(token.Issuer);
            Console.WriteLine(token.SecurityKey);
            Console.WriteLine(token.SigningKey);
            Console.WriteLine(token.ValidFrom);*/
           /// Console.WriteLine(token.ValidTo);
          

            return tokenHandler.WriteToken(token);
        }
    }

}
