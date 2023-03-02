using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MechSuitsApi.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using CoreInfrastructure.Auth.Roles;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MechSuitsApi.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MechSuitsApi.Areas.Auth.Controllers
{
   // [Authorize]
    [Route("api/roles")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class RolesController : ControllerBase
    {
        private readonly IUriService uriService;
        string companycode = "C001";   string user = "";
        private readonly AppDBContext _context;
        private SqlConnection con;
        D_DB dbset = null;
        private IHttpContextAccessor _httpContextAccessor;
        Connection c = new Connection();
        public RolesController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context;
            con = c.V_connection;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
        //    companycode = currentUser.FindFirst("Company").Value.ToString();
        //    user = currentUser.FindFirst("User").Value.ToString();
            dbset = new D_DB(c.V_connection);
        }

        // GET: api/VendorType
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_Roles>>> Get()
        {
          //  Console.WriteLine(_context.Roles);
            return await _context.Roles.ToListAsync();
        }

        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_Roles>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.Roles
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.Roles.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Roles>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }


        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_Roles>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            // return await _context.Area.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.Roles.Where(m => m.Name.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.Roles.Where(m => m.Name.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Roles>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }







        // GET: api/VendorType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_Roles>> Get(long id)
        {
            
            var m = await _context.Roles.FindAsync(companycode, id);

            if (m == null)
            {
              //  Console.WriteLine("Nulldata");
                return NotFound();
            }

           Console.WriteLine(m.ToString());
            return m;
        }
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_Roles m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_Roles obj = new M_Roles();
                obj = await _context.Roles.FindAsync(companycode, m.code);

                if (obj != null)
                {

                    obj.Name = m.Name;
                    obj.ACTIVE = m.ACTIVE; 
                    obj.DESCRIPTION=m.DESCRIPTION;

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
        public async Task<ActionResult<M_Roles>> create(M_Roles m)
        {
            m.code = dbset.getUpdateMasterCount();
            _context.Roles.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { code = m.code }, m);
        }

        // DELETE: api/VendorType/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_Roles>> Delete(Int64 id)
        {
            
            var m = await _context.Roles.FindAsync(companycode, id);
            if (m == null)
            {
                return NotFound();
            }

            _context.Roles.Remove(m);
            await _context.SaveChangesAsync();

            return m;
        }





    }
}
