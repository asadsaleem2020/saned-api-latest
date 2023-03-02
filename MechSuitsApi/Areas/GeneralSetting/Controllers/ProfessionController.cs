using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MechSuitsApi.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MechSuitsApi.Interfaces;
using CoreInfrastructure.GeneralSetting.Professions;

//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using MechSuitsApi.Classes;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Cors;
//using System.Data.SqlClient;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using System.Security.Claims;
//using MechSuitsApi.Interfaces; 
//using CoreInfrastructure.GeneralSetting.Professions;
//using System.Data.Entity;
//using System.Linq;
//using EntityState = Microsoft.EntityFrameworkCore.EntityState;

namespace MechSuitsApi.Areas.GeneralSetting.Controllers
{
    // [Authorize]
    [Route("api/Profession")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ProfessionController : ControllerBase
    {
        private readonly IUriService uriService;
        private readonly AppDBContext _context;
        private SqlConnection con;
        D_DB dbset = null;

        private IHttpContextAccessor _httpContextAccessor;
        Connection c = new Connection();
        string companycode = ""; string user = "";



        public ProfessionController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
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
        public async Task<ActionResult<IEnumerable<M_Profession>>> Get()
        {


            return await _context.professions.ToListAsync();

        }

        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_Profession>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.professions
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.professions.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Profession>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<M_Profession>> Get(string id)
        {

            var m = await _context.professions.FindAsync(companycode, id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_Profession>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];

            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);


            var pagedData = await _context.professions.Where(m => m.Name.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.professions.Where(m => m.Name.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Profession>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_Profession m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_Profession obj = new M_Profession();
                obj = await _context.professions.FindAsync(companycode, m.Code);

                if (obj != null)
                {

                    obj.Name = m.Name;
                    obj.AName = m.AName;
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

       

        [HttpPost]
        public async Task<ActionResult<M_Profession>> create(M_Profession m)
        {


            m.COMPANY_CODE = companycode;
            m.Code = dbset.getUpdateMasterCount();
            _context.professions.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { code = m.Code }, m);
        }

        // DELETE: api/VendorType/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_Profession>> Delete(string id)
        {

            var m = await _context.professions.FindAsync(companycode, id);
            if (m == null)
            {
                return NotFound();
            }

            _context.professions.Remove(m);
            await _context.SaveChangesAsync();

            return m;
        }

    }

}
 
