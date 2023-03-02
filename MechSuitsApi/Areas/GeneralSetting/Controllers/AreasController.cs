using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MechSuitsApi.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using CoreInfrastructure.GeneralSetting.Area;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MechSuitsApi.Interfaces;
using ServiceStack.Configuration;

namespace MechSuitsApi.Areas.GeneralSetting.Controllers
{ //  [Authorize]
    [Route("api/area")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class AreasController : ControllerBase
    {

        private readonly IUriService uriService;
        private readonly AppDBContext _context;
        private SqlConnection con;
        D_DB dbset = null;
         
        private IHttpContextAccessor _httpContextAccessor;
        Connection c = new Connection();
        string companycode = "";   string user = ""; string Role = "";
         


        public AreasController(AppDBContext context , IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            _context = context;
            con = c.V_connection;
            this.uriService = uriService;
            dbset = new D_DB(c.V_connection);
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
          //  companycode = currentUser.FindFirst("Company").Value.ToString();  
          //  user = currentUser.FindFirst("User").Value.ToString();
          //  Role = currentUser.FindFirst("Role").Value.ToString(); getname();
            
          // Role.Append(',')

        }
      
      // GET: api/VendorType
     //   [Authorize( Roles= "user")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_Area>>> Get()
        {
            Console.WriteLine(RoleNames.Admin);
            Console.WriteLine(Role);

            Console.WriteLine(user);
            return await _context.Area.ToListAsync();
           
        }
        //Pagination
      //  [Authorize(Policy = "superadmin,admin")]
      //  [Authorize(Roles. = RoleNames.AllowAnyUser)]
        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_Area>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
           // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.Area
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.Area.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Area>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }


        // GET: api/VendorType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_Area>> Get(string id)
        {
           
            var m = await _context.Area.FindAsync(companycode, id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_Area>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
             
            string title = HttpContext.Request.Query["title"];
           // return await _context.Area.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.Area.Where(m => m.Name.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.Area.Where(m => m.Name.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Area>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_Area m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_Area obj = new M_Area();
                obj = await _context.Area.FindAsync(companycode, m.Code);

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
        public async Task<ActionResult<M_Area>> create(M_Area m)
        {
            

            m.COMPANY_CODE = companycode;
            m.Code = dbset.getUpdateMasterCount();
            _context.Area.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { code = m.Code }, m);
        }

        // DELETE: api/VendorType/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_Area>> Delete(string id)
        {
            
            var m = await _context.Area.FindAsync(companycode, id);
            if (m == null)
            {
                return NotFound();
            }

            _context.Area.Remove(m);
            await _context.SaveChangesAsync();

            return m;
        }




    }

}

