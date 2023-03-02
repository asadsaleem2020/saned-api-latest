using CoreInfrastructure.Hr.Setup;
using Executer;
using MechSuitsApi.Classes;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MechSuitsApi.Interfaces;
using CoreInfrastructure.AccomodationSystem;
using CoreInfrastructure.ToolbarItems;

namespace MechSuitsApi.Areas.Hr.Controllers
{
    //[Authorize]
    [Route("api/vehicles")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class VehiclesController : ControllerBase
    {
        private readonly IUriService uriService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        string companycode = ""; string user = "";
        public VehiclesController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context;
            con = c.V_connection;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            //   companycode = currentUser.FindFirst("Company").Value.ToString();    user = currentUser.FindFirst("User").Value.ToString();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_Vehicles>>> GetAS_Acclevel2()
        {
            return await _context.HR_VEHICLES.ToListAsync();
        }



        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_Vehicles>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.HR_VEHICLES
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.HR_VEHICLES.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Vehicles>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }


        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_Vehicles>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            // return await _context.Area.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.HR_VEHICLES.Where(m => (m.CODE.Contains(title) || m.PLATENO.Contains(title)))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.HR_VEHICLES.Where(m => (m.CODE.Contains(title) || m.PLATENO.Contains(title))).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Vehicles>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }

        [HttpGet("{id}")]
        public async Task<ActionResult<M_Vehicles>> Getm(string id)
        {
            var m = await _context.HR_VEHICLES.FindAsync(id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }
        [HttpPost]
        public async Task<ActionResult<M_Vehicles>> create(M_Vehicles m)
        {
            Console.WriteLine("Hello creating Record For Vehicles");
            m.CODE = getNext();
            _context.HR_VEHICLES.Add(m);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Getm", new { id = m.CODE }, m);
        }
        public string getNext()
        {
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = @"SELECT ISNULL(MAX( CAST(CODE as bigint)),1000) +1  as CODE from HR_VEHICLES";

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(strsql, con);
            sqldr = cmd.ExecuteReader();
            if (sqldr.Read())
            {
                no = sqldr["CODE"].ToString().Trim();
            }
            sqldr.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (no.Trim() == "") no = "1001";
            return no;
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_Vehicles m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_Vehicles obj = new M_Vehicles();
                obj = await _context.HR_VEHICLES.FindAsync(m.CODE);

                if (obj != null)
                {
                    obj.VTYPE = m.VTYPE;
                    obj.VMODEL = m.VMODEL;
                    obj.PLATENO = m.PLATENO;
                    obj.SERIALNO = m.SERIALNO;
                    obj.APPEXPIRY = m.APPEXPIRY;
                    obj.EXAMEXPIRY = m.EXAMEXPIRY;
                    obj.INSURANCEEXPIRY = m.INSURANCEEXPIRY;
                    obj.PHOTO = m.PHOTO;
                    obj.NOTES = m.NOTES;
                }
                // int i = this.obj.SaveChanges();
                _context.Entry(obj).State = EntityState.Modified;

                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {
                throw;
            }
            return Ok(m);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_Vehicles>> Deletem(string id)
        {
            var m = await _context.HR_VEHICLES.FindAsync(id);
            if (m == null)
            {
                return NotFound();
            }
            _context.HR_VEHICLES.Remove(m);
            await _context.SaveChangesAsync();
            return m;
        }

    }
}
