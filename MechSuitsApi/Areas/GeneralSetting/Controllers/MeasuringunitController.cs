using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MechSuitsApi.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using CoreInfrastructure.GeneralSetting.MeasuringUnit;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MechSuitsApi.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MechSuitsApi.Areas.GeneralSetting.Controllers
{
    [Authorize]
    [Route("api/measuringunit")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class MeasuringunitController : ControllerBase
    {
        private readonly IUriService uriService;
        string companycode = "";   string user = "";
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AppDBContext _context;
        private SqlConnection con;
        D_DB dbset = null;
        Connection c = new Connection();
        public MeasuringunitController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
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
        public async Task<ActionResult<IEnumerable<M_MeasuringUnit>>> Get()
        {
            return await _context.MeasuringUnit.ToListAsync();
        }
        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_MeasuringUnit>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.MeasuringUnit
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.MeasuringUnit.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_MeasuringUnit>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_MeasuringUnit>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            // return await _context.Area.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.MeasuringUnit.Where(m => m.Name.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.MeasuringUnit.Where(m => m.Name.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_MeasuringUnit>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }


        // GET: api/VendorType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_MeasuringUnit>> Get(string id)
        {
            
            var m = await _context.MeasuringUnit.FindAsync(companycode, id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_MeasuringUnit m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_MeasuringUnit obj = new M_MeasuringUnit();
                obj = await _context.MeasuringUnit.FindAsync(companycode, m.Code);

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
        public async Task<ActionResult<M_MeasuringUnit>> create(M_MeasuringUnit m)
        {
            m.Code = dbset.getUpdateMasterCount();
            _context.MeasuringUnit.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { code = m.Code }, m);
        }

        // DELETE: api/VendorType/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_MeasuringUnit>> Delete(string id)
        {
            
            var m = await _context.MeasuringUnit.FindAsync(companycode, id);
            if (m == null)
            {
                return NotFound();
            }

            _context.MeasuringUnit.Remove(m);
            await _context.SaveChangesAsync();

            return m;
        }




    }

}

