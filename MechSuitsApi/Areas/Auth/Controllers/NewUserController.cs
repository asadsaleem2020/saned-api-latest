using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MechSuitsApi.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using CoreInfrastructure.Auth.NewUsers;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MechSuitsApi.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MechSuitsApi.Areas.Auth.Controllers
{
   // [Authorize]
    [Route("api/newusers")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class NewUserController : ControllerBase
    {
        private readonly IUriService uriService;
        string companycode = "";   string user = "";
        private readonly AppDBContext _context;
        private SqlConnection con;
        D_DB dbset = null;
        private IHttpContextAccessor _httpContextAccessor;
        Connection c = new Connection();
        public NewUserController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
      // companycode = currentUser.FindFirst("Company").Value.ToString();    user = currentUser.FindFirst("User").Value.ToString();
            con = c.V_connection;
            dbset = new D_DB(c.V_connection);
        }

        // GET: api/VendorType
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_NewUser>>> Get()
        {
            return await _context.APP_USERS.ToListAsync();
        }

        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_NewUser>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.APP_USERS
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.APP_USERS.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_NewUser>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }


        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_NewUser>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            // return await _context.Area.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.APP_USERS.Where(m => m.NAME.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.APP_USERS.Where(m => m.NAME.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_NewUser>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }




        // GET: api/VendorType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_NewUser>> Get(string id)
        {
            
            var m = await _context.APP_USERS.FindAsync(companycode, id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_NewUser m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_NewUser obj = new M_NewUser();
                obj = await _context.APP_USERS.FindAsync(companycode, m.CODE,m.ROLE_CODE);

                if (obj != null)
                {

                    obj.NAME = m.NAME;
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

     

        [HttpPost]
        public async Task<ActionResult<M_NewUser>> create(M_NewUser m)
        {
            m.CODE = dbset.getUpdateMasterCount();
            _context.APP_USERS.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { code = m.CODE }, m);
        }

        // DELETE: api/VendorType/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_NewUser>> Delete(Int64 id)
        {
            
            var m = await _context.APP_USERS.FindAsync(companycode, id);
            if (m == null)
            {
                return NotFound();
            }

            _context.APP_USERS.Remove(m);
            await _context.SaveChangesAsync();

            return m;
        }






    }
}
