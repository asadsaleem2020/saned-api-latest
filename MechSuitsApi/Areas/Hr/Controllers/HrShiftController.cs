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
using Microsoft.AspNetCore.Authorization;
using MechSuitsApi.Interfaces;

namespace MechSuitsApi.Areas.Hr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HrShiftController : ControllerBase
    {

        private readonly IUriService uriService;
        private readonly AppDBContext _context;
        private SqlConnection con;
        string companycode = ""; string user = "";
        private IHttpContextAccessor _httpContextAccessor;
        Connection c = new Connection();
        public HrShiftController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context;
            con = c.V_connection;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;


            //  companycode = currentUser.FindFirst("Company").Value.ToString();
            Console.WriteLine(companycode);
            // user = currentUser.FindFirst("User").Value.ToString();
            Console.WriteLine(user);
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_HrShift>>> GetAS_Acclevel2()
        {
            return await _context.HrShift.ToListAsync();
        }
        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_HrShift>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.HrShift
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.HrShift.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_HrShift>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_HrShift>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];

            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);


            var pagedData = await _context.HrShift.Where(m => m.Title.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.HrShift.Where(m => m.Title.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_HrShift>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_HrShift>> GetM_Level2(string id)
        {
            var m_Level2 = await _context.HrShift.FindAsync(id);

            if (m_Level2 == null)
            {
                return NotFound();
            }

            return m_Level2;
        }
        [HttpGet]
        [Route("GetLevel2WithLevel1")]
        public async Task<ActionResult<IEnumerable<M_HrShift>>> GetLevel2WithLevel1(string id)
        {

            var m_Level2 = await _context.HrShift.Where(x => x.Code == id).ToListAsync();

            if (m_Level2 == null)
            {
                return NotFound();
            }

            return m_Level2;
        }

        // PUT: api/Departments/5

        //  [HttpPut("{id}")]
        [HttpPut]
        [Route("update")]

        //   public IHttpActionResult PutEmaployeeMaster(EmployeeDetail employee)
        public async Task<IActionResult> update(M_HrShift m_Level2)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var obj = new M_HrShift();
                obj = await _context.HrShift.FindAsync(m_Level2.Code);

                if (obj != null)
                {

                    
                    obj.Status = m_Level2.Status ?? "0";
                    obj.Code= m_Level2.Code;    
                    obj.Title = m_Level2.Title; 
                    obj.DateAdded = m_Level2.DateAdded; 

                }
                // int i = this.obj.SaveChanges();
                _context.Entry(obj).State = EntityState.Modified;

                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {
                throw;
            }
            return Ok(m_Level2);
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutM_Level2(string id, M_Level2 m_Level2)
        //{
        //    if (id != m_Level2.Code)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(m_Level2).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        //if (!M_Level2Exists(id))
        //        //{
        //        //    return NotFound();
        //        //}
        //        //else
        //        //{
        //        //    throw;
        //        //}
        //    }

        //    return NoContent();
        //}

        // POST: api/Departments
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<M_HrShift>> create(M_HrShift m_Level2)
        {
            m_Level2.Code = getUpdateMasterCount();
            m_Level2.DateAdded = DateTime.Now.ToString();
            _context.HrShift.Add(m_Level2);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetM_Level2", new { id = m_Level2.Code }, m_Level2);
        }
        public string getUpdateMasterCount()
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            string no = "";
             
            strsql = @"SELECT ISNULL(MAX( CAST(CODE AS BIGINT  ))  ,10000) +1  AS code FROM HrShift";

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            sqldr = dp.ExecuteReader(con, strsql);
            if (sqldr.Read())
            {
                no = sqldr["code"].ToString().Trim();
            }
            sqldr.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (no.Trim() == "") no = "1";
            return no;
        }
    
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_HrShift>> DeleteM_Level2(string id)
        {
            var m_Level2 = await _context.HrShift.FindAsync(id);
            if (m_Level2 == null)
            {
                return NotFound();
            }

            _context.HrShift.Remove(m_Level2);
            await _context.SaveChangesAsync();

            return m_Level2;
        }

        
    }
}
