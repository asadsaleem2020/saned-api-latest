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
//using CoreInfrastructure.GeneralSetting.Age;
using CoreInfrastructure.GeneralSetting.Experience;
 
namespace MechSuitsApi.Areas.GeneralSetting.Controllers
{  // [Authorize]
    [Route("api/Experience")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ExperienceController : ControllerBase
    {


        private readonly IUriService uriService;
        private readonly AppDBContext _context;
        private SqlConnection con;
        D_DB dbset = null;
        private IHttpContextAccessor _httpContextAccessor;
        Connection c = new Connection();
        string companycode = ""; string user = "";



        public ExperienceController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
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
        public async Task<ActionResult<IEnumerable<M_Experience>>> Get()
        {


            return await _context.Experience.ToListAsync();

        }

        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_Experience>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.Experience
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.Experience.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Experience>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<M_Experience>> Get(string id)
        {

            var m = await _context.Experience.FindAsync(companycode, id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_Experience>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];

            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);


            var pagedData = await _context.Experience.Where(m => m.Name.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.Experience.Where(m => m.Name.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Experience>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_Experience m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_Experience obj = new M_Experience();
                obj = await _context.Experience.FindAsync(companycode, m.Code);

                if (obj != null)
                {

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
        public async Task<ActionResult<M_Experience>> create(M_Experience m)
        {


            m.COMPANY_CODE = companycode;
            m.Code = dbset.getUpdateMasterCount();
            _context.Experience.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { code = m.Code }, m);
        }

        // DELETE: api/VendorType/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_Experience>> Delete(string id)
        {

            var m = await _context.Experience.FindAsync(companycode, id);
            if (m == null)
            {
                return NotFound();
            }

            _context.Experience.Remove(m);
            await _context.SaveChangesAsync();

            return m;
        }

    }

}

