using CoreInfrastructure.GeneralSetting.Age;
using MechSuitsApi.Classes;
using MechSuitsApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Cors;
using CoreInfrastructure.GeneralSetting.Office;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace MechSuitsApi.Areas.GeneralSetting.Controllers
{
    // [Authorize]
    [Route("api/office")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class OfficeController : ControllerBase
    {

        private readonly IUriService uriService;
        private readonly AppDBContext _context;
         private SqlConnection con;
        D_DB dbset = null;

        private IHttpContextAccessor _httpContextAccessor;
        Connection c = new Connection();
        string companycode = ""; string user = "";



        public OfficeController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            _context = context;
            con = c.V_connection;
            this.uriService = uriService;
            dbset = new D_DB(c.V_connection);
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            //  companycode = currentUser.FindFirst("Company").Value.ToString();
            //  user = currentUser.FindFirst("User").Value.ToString();


        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_Office>>> Get()
        {


            return await _context.office.ToListAsync();

        }



        [HttpGet("{id}")]
        public async Task<ActionResult<M_Office>> Get(string id)
        {

            var m = await _context.office.FindAsync("C001", id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }


        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_Office m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_Office obj = new M_Office();
                obj = await _context.office.FindAsync("C001", m.Code);

                if (obj != null)
                {

                    obj.DName = m.DName;
                    obj.DName_AR = m.DName_AR;
                    obj.Address = m.Address;
                    obj.Rlease_Date = m.Rlease_Date;
                    obj.Place_Of_Issue_AR = m.Place_Of_Issue_AR;
                    obj.Place_Of_Issue = m.Place_Of_Issue;
                    obj.Mobile = m.Mobile;
                    obj.Passport = m.Passport;



                }

                _context.Entry(obj).State = EntityState.Modified;

                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {
                throw;
            }
            return Ok(m);
        }
    }

}

