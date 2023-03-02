using MechSuitsApi.Classes;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CoreInfrastructure.Auth.Menu;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MechSuitsApi.Areas.Auth.Controllers
{
    //[Authorize]
    [Route("api/Menu")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class MenuController : ControllerBase
    {
        string companycode = "";   string user = "";
        private readonly AppDBContext _context;
        private SqlConnection con;
        private IHttpContextAccessor _httpContextAccessor;
        Connection c = new Connection();
        D_DB dbset = null;
        public MenuController(AppDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            con = c.V_connection;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
          //  companycode = currentUser.FindFirst("Company").Value.ToString(); user = currentUser.FindFirst("User").Value.ToString();
            dbset = new D_DB(c.V_connection);
        }
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<M_MainModel>> GetMenu(string id)
        {
            try
            {
                  Console.WriteLine( id);
               //10001 to id args
                var result = dbset.GetMainMenuList(id);
                return result;

            }
            catch (Exception ex)
            {
                var error = ex.Message.ToString();
                return Content("Error");
            }
        }
        [HttpGet("{moduleId}/GetDetailMenu")]
        public ActionResult<IEnumerable<M_DetailModel>> GetDetailMenu(string moduleId)
        {
            try
            {
                var result = dbset.GetMainMenuDetailList("MODULE_ID=" + moduleId);
                return result;

            }
            catch (Exception ex)
            {
                var error = ex.Message.ToString();
                return Content("Error");
            }




        }

    }
}
