using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MechSuitsApi.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using CoreInfrastructure.GeneralSetting.City;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MechSuitsApi.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MechSuitsApi.Areas.GeneralSetting.Controllers
{
    [Authorize]
    [Route("api/city")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class CityController : ControllerBase
    {


        private readonly AppDBContext _context;
        private SqlConnection con;
        D_DB dbset = null;
        private readonly IUriService uriService;
        Connection c = new Connection();
        private IHttpContextAccessor _httpContextAccessor;
        string companycode = "";   string user = "";
        public CityController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            _context = context;
            this.uriService = uriService;
            con = c.V_connection;
            dbset = new D_DB(c.V_connection);
       
        _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            companycode = currentUser.FindFirst("Company").Value.ToString();
            user = currentUser.FindFirst("User").Value.ToString();
        }

        // GET: api/VendorType
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_City>>> Get()
        {
            return await _context.City.ToListAsync();
        }
        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_City>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
           // return await _context.City.ToListAsync();
            var route = Request.Path.Value;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.City
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.City.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_City>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_City>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            // return await _context.Area.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.City.Where(m => m.Name.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.City.Where(m => m.Name.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_City>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }
        // GET: api/VendorType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_City>> Get(string id)
        {
            
            var m = await _context.City.FindAsync(companycode, id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_City m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_City obj = new M_City();
                obj = await _context.City.FindAsync(companycode, m.Code);

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
        public async Task<ActionResult<M_City>> create(M_City m)
        {
            m.Code = dbset.getUpdateMasterCount();
            _context.City.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { code = m.Code }, m);
        }

        // DELETE: api/VendorType/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_City>> Delete(string id)
        {
            
            var m = await _context.City.FindAsync(companycode, id);
            if (m == null)
            {
                return NotFound();
            }

            _context.City.Remove(m);
            await _context.SaveChangesAsync();

            return m;
        }




    }

}

