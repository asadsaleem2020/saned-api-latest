using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreInfrastructure.Auth.Fiscalyear;
using MechSuitsApi.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using System.Data.SqlClient;
using System.Data;
using Executer;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MechSuitsApi.Areas.Auth.Controllers
{
    [Route("api/fiscalyear")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class FiscalYearController : ControllerBase
    {
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AppDBContext _context;
        private SqlConnection con;
        string companycode = ""; string user = "";
        Connection c = new Connection();
        public FiscalYearController(AppDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            // var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            //  companycode = currentUser.FindFirst("Company").Value.ToString();
            //  user = currentUser.FindFirst("User").Value.ToString();
            con = c.V_connection;
        }

        // GET: api/Level2
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_Fiscalyear>>> Get()
        {
            return await _context.Fiscalyears.ToListAsync();
        }

        // GET: api/Level2/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_Fiscalyear>> Get(int id)
        {
            var m = await _context.Fiscalyears.FindAsync(id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }



        // PUT: api/Level2/5

        //  [HttpPut("{id}")]
        [HttpPut]
        [Route("update")]

        //   public IHttpActionResult PutEmaployeeMaster(EmployeeDetail employee)
        public async Task<IActionResult> update(M_Fiscalyear m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_Fiscalyear obj = new M_Fiscalyear();
                obj = await _context.Fiscalyears.FindAsync(m.Year);

                if (obj != null)
                {
                    obj.CompanyCode = companycode;



                }
                // int i = this.obj.SaveChanges();
                _context.Entry(obj).State = EntityState.Modified;

                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {
                throw;
            }
            return Ok(m);
        }


        [HttpPost]
        public async Task<ActionResult<M_Fiscalyear>> create(M_Fiscalyear m)
        {
           
            _context.Fiscalyears.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = m.Year }, m);
        }
    


        // DELETE: api/Level2/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_Fiscalyear>> Delete(string id)
        {
            var m = await _context.Fiscalyears.FindAsync(id);
            if (m == null)
            {
                return NotFound();
            }

            _context.Fiscalyears.Remove(m);
            await _context.SaveChangesAsync();

            return m;
        }


    }
}
