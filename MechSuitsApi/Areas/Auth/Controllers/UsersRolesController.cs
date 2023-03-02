using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using MechSuitsApi.Classes;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CoreInfrastructure.UsersRoles;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MechSuitsApi.Areas.Auth.Controllers
{
    //[Authorize]
    [Route("api/userroles")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class UsersRolesController : ControllerBase
    {
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        D_DB dbset = null;
        private IHttpContextAccessor _httpContextAccessor;
        string companycode = ""; string user = "";
        public UsersRolesController(AppDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            companycode = currentUser.FindFirst("Company").Value.ToString();
            user = currentUser.FindFirst("User").Value.ToString();
            con = c.V_connection;
            dbset = new D_DB(c.V_connection);
        }
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<M_ModalDetail>> GetRoleList(string id)
        {
            List<M_ModalDetail> rst = dbset.GetDetailRowForEdit(id.Trim(),  companycode );
             Console.WriteLine(id);
            return rst;

        }
        /// <summary>
        /// //////////////////////
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>

        //[HttpPut]
        //[Route("update")]
        //public async Task<IActionResult> update(M_Roles m)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    try
        //    {
        //        M_Roles obj = new M_Roles();
        //        obj = await _context.Roles.FindAsync(companycode, m.code);

        //        if (obj != null)
        //        {

        //            obj.Name = m.Name;
        //            obj.ACTIVE = m.ACTIVE;

        //        }

        //        _context.Entry(obj).State = EntityState.Modified;

        //        await _context.SaveChangesAsync();

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return Ok(m);
        //}


        [HttpPut("{id}")]

        public async Task<ActionResult<string>> Put(IList<M_ModalDetail> m)
        {
            bool d,c,e,n;
            if (m != null)
            {

                M_UsersRoles u = null;
                Console.WriteLine(m[0].CHK + m[0].NEW + " " +  " " + m[0].EDIT + " " + m[0].DEL);
                foreach (var item in m)
                {
                    if (item.CHK == "0") { c = false; } else { c = true; }
                    if (item.NEW == "0") { n = false; } else { n = true; }
                    if (item.EDIT == "0") { e = false; } else { e = true; }
                    if (item.DEL == "0") { d = false; } else { d = true; }
                    
                   
                    u = new M_UsersRoles
                    {
                        ACTIVE = item.ACTIVE,
                        APPROVE = true,
                        CHK = c,
                        COMPANY_CODE = companycode,
                        DEL = d,
                        EDIT = e,
                        GETLIST = true,
                        MENU_ID = item.MENU_ID,
                        NEW = n,
                        PRNT = true,
                        ROLE_ID = item.ROLE_ID,
                        UNCHK = true
                    };
                    _context.APP_USERS_ROLES.Update(u);
                }

            }

            await _context.SaveChangesAsync();

            return "Updated";
        }

        /////////////////////
        ///////////////
        [HttpPost]
        public async Task<ActionResult<string>> Post(IList<M_ModalDetail> m)
        {
            if (m != null)
            {
                Console.WriteLine("Condition is executing");
                M_UsersRoles u = null;
                Console.WriteLine("ROLE ID  is"+m[0].ROLE_ID);
                //DeleteMenu(m[0].ROLE_ID);
                foreach (var item in m)
                {
                    u = new M_UsersRoles
                    {
                        //  ACTIVE = item.ACTIVE,
                        ACTIVE = true,
                        APPROVE = false,
                        CHK = false,
                        COMPANY_CODE = companycode,
                        DEL = true,
                        EDIT = true,
                        GETLIST = false,
                        MENU_ID = item.MENU_ID,
                        NEW = true,
                        PRNT = false,
                        ROLE_ID = item.ROLE_ID,
                        UNCHK = false
                    };
                    _context.APP_USERS_ROLES.Add(u);
                }

            }
          
            await _context.SaveChangesAsync();

            return "Posted";
        }

        private void DeleteMenu(long id)
        {
            
            try
            {
                string ls_query = "DELETE FROM APP_USERS_ROLES WHERE role_id="+id;
                SqlCommand cmd = new SqlCommand(ls_query, con);
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
    
        }
    }
}
