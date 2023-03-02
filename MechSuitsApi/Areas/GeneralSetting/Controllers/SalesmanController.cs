using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MechSuitsApi.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using CoreInfrastructure.GeneralSetting.Salesman;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MechSuitsApi.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MechSuitsApi.Areas.GeneralSetting.Controllers
{
    [Authorize]
    [Route("api/salesman")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class SalesmanController : ControllerBase
    {
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IUriService uriService;
        private readonly AppDBContext _context;
        private SqlConnection con;
        D_DB dbset = null;
        string companycode = "";   string user = "";
        Connection c = new Connection();
        public SalesmanController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context;
            con = c.V_connection;
            dbset = new D_DB(c.V_connection);
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
         companycode = currentUser.FindFirst("Company").Value.ToString();    user = currentUser.FindFirst("User").Value.ToString();
        }

        // GET: api/VendorType
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_Salesman>>> Get()
        {
            return await _context.Salesman.ToListAsync();
        }
        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_Salesman>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.Salesman
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.Salesman.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Salesman>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        } 
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_Salesman>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            // return await _context.Area.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.Salesman.Where(m => m.Name.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.Salesman.Where(m => m.Name.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Salesman>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }

        // GET: api/VendorType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_Salesman>> Get(string id)
        {
            
            var m = await _context.Salesman.FindAsync(companycode, id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }

        //[HttpGet]
        //[Route("search")]
        //public async Task<ActionResult<IEnumerable<M_Salesman>>> Get1()
        //{
        //    string where = "1=1";
        //    string title = HttpContext.Request.Query["title"];
        //    return await _context.Salesman
        //.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();

        //}


        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_Salesman m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_Salesman obj = new M_Salesman();
                obj = await _context.Salesman.FindAsync(companycode, m.Code);

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
        public async Task<ActionResult<M_Salesman>> create(M_Salesman m)
        {
            m.Code = dbset.getUpdateMasterCount();
            _context.Salesman.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { code = m.Code }, m);
        }

        // DELETE: api/VendorType/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_Salesman>> Delete(string id)
        {
            
            var m = await _context.Salesman.FindAsync(companycode, id);
            if (m == null)
            {
                return NotFound();
            }

            _context.Salesman.Remove(m);
            await _context.SaveChangesAsync();

            return m;
        }




    }

}

