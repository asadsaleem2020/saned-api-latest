using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MechSuitsApi.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using CoreInfrastructure.Auth.Company;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MechSuitsApi.Areas.GeneralSetting.Controllers
{
    [Authorize]
    [Route("api/company")]
    [ApiController]
    public class CompanyController : ControllerBase
    {

        string companycode = "";   string user = "";

        private readonly AppDBContext _context;
        private SqlConnection con;
        private IHttpContextAccessor _httpContextAccessor;
        Connection c = new Connection();
        public CompanyController(AppDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            con = c.V_connection;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
          companycode = currentUser.FindFirst("Company").Value.ToString();    user = currentUser.FindFirst("User").Value.ToString();
        }

        // GET: api/VendorType
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_Company>>> Get()
        {
            return await _context.COMPANY.ToListAsync();
        }

        // GET: api/VendorType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_Company>> Get(string id)
        {
            
            var m = await _context.COMPANY.FindAsync(companycode);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_Company m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_Company obj = new M_Company();
                obj = await _context.COMPANY.FindAsync(companycode);

                if (obj != null)
                {

                    obj.COMPANY_NAME = m.COMPANY_NAME;
                    obj.ADDRESS = m.ADDRESS;
                    obj.CITY= m.CITY;
                    obj.COUNTRY = m.COUNTRY;
                    obj.FAX= m.FAX;
                    obj.E_MAIL= m.E_MAIL;
                    obj.WEBSITE = m.WEBSITE;
                    obj.POSTAL = m.POSTAL;
                    obj.PHONE = m.PHONE;
                    obj.SALES_TAX_NO = m.SALES_TAX_NO;
                    obj.ACTIVE = m.ACTIVE;
                    


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
        public async Task<ActionResult<M_Company>> create(M_Company m)
        {
            
            _context.COMPANY.Add(m);
             await _context.SaveChangesAsync();

          return CreatedAtAction("Get", new { code = companycode }, m);
        }

        // DELETE: api/VendorType/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_Company>> Delete(string id)
        {
            
            var m = await _context.COMPANY.FindAsync(companycode, id);
            if (m == null)
            {
                return NotFound();
            }

            _context.COMPANY.Remove(m);
            await _context.SaveChangesAsync();

            return m;
        }





    }
}
