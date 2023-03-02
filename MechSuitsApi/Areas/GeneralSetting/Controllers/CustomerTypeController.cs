using System.Threading.Tasks;
using CoreInfrastructure.GeneralSetting;
using MechSuitsApi.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MechSuitsApi.Areas.GeneralSetting.Controllers
{
    [Authorize]
    [Route("api/CustomerType")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class CustomerTypeController : ControllerBase
    {
        string companycode = "";   string user = "";
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        public CustomerTypeController(AppDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            con = c.V_connection;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
          companycode = currentUser.FindFirst("Company").Value.ToString();    user = currentUser.FindFirst("User").Value.ToString();
        }

        // GET: api/VendorType
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_GS_CustomerType>>> Get()
        {
            return await _context.GS_CustomerType.ToListAsync();
        }

        // GET: api/VendorType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_GS_CustomerType>> Get(Int64 id)
        {
            var m = await _context.GS_CustomerType.FindAsync(id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }



        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_GS_CustomerType m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_GS_CustomerType obj = new M_GS_CustomerType();
                obj = await _context.GS_CustomerType.FindAsync(m.ID);

                if (obj != null)
                {
                    obj.Company_Code = companycode;


                    obj.Name = m.Name;
                    obj.Locked = m.Locked;
                    obj.Sort = m.Sort;

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

        [HttpPut("{id}")]



        [HttpPost]
        public async Task<ActionResult<M_GS_CustomerType>> create(M_GS_CustomerType m)
        {

            _context.GS_CustomerType.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = m.ID }, m);
        }



        // DELETE: api/VendorType/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_GS_CustomerType>> Delete(Int64 id)
        {
            var m = await _context.GS_CustomerType.FindAsync(id);
            if (m == null)
            {
                return NotFound();
            }

            _context.GS_CustomerType.Remove(m);
            await _context.SaveChangesAsync();

            return m;
        }


    }
}
