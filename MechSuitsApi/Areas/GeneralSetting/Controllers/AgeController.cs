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
using CoreInfrastructure.GeneralSetting.Age;


namespace MechSuitsApi.Areas.GeneralSetting.Controllers
{
   // [Authorize]
    [Route("api/age")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class AgeController : ControllerBase
    {
        private readonly IUriService uriService;
        private readonly AppDBContext _context;
        private SqlConnection con;
        D_DB dbset = null;

        private IHttpContextAccessor _httpContextAccessor;
        Connection c = new Connection();
        string companycode = ""; string user = "";



        public AgeController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
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
        public async Task<ActionResult<IEnumerable<M_Age>>> Get()
        {


            return await _context.Age.ToListAsync();

        }

        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_Age>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.Age
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.Age.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Age>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<M_Age>> Get(string id)
        {

            var m = await _context.Age.FindAsync(companycode, id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_Age>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
   
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
      

            var pagedData = await _context.Age.Where(m => m.Name.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.Age.Where(m => m.Name.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Age>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_Age m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_Age obj = new M_Age();
                obj = await _context.Age.FindAsync(companycode, m.Code);

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

       

        [HttpPost]
        public async Task<ActionResult<M_Age>> create(M_Age m)
        {


            m.COMPANY_CODE = companycode;
            m.Code = dbset.getUpdateMasterCount();
            _context.Age.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { code = m.Code }, m);
        }

        // DELETE: api/VendorType/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_Age>> Delete(string id)
        {

            var m = await _context.Age.FindAsync(companycode, id);
            if (m == null)
            {
                return NotFound();
            }

            _context.Age.Remove(m);
            await _context.SaveChangesAsync();

            return m;
        }
 
    }

}

