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
    [Route("api/HrShiftDetails")]
    [ApiController]
    public class HrShiftDetailsController : ControllerBase
    {
        private readonly IUriService uriService;
        private readonly AppDBContext _context;
        private SqlConnection con;
        string companycode = ""; string user = "";
        private IHttpContextAccessor _httpContextAccessor;
        Connection c = new Connection();
        public HrShiftDetailsController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
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
        public async Task<ActionResult<IEnumerable<M_HrShiftDetails>>> GetAS_Acclevel2()
        {
            return await _context.HrShiftDetails.ToListAsync();
        }
        [HttpGet]
        [Route("chunks/{shiftCode}")]
        public async Task<ActionResult<IEnumerable<M_HrShiftDetails>>> Get1(string shiftCode,[FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.HrShiftDetails.Where(m=>m.ShiftCode==shiftCode)
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.HrShiftDetails.Where(m => m.ShiftCode == shiftCode).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_HrShiftDetails>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("{shiftCode}/search")]
        public async Task<ActionResult<IEnumerable<M_HrShiftDetails>>> Get3(string shiftCode,[FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];

            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);


            var pagedData = await _context.HrShiftDetails.Where(m => (m.Address.Contains(title))&& m.ShiftCode==shiftCode)
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.HrShiftDetails.Where(m => (m.Address.Contains(title)) && m.ShiftCode == shiftCode).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_HrShiftDetails>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_HrShiftDetails>> GetM_Level2(string id)
        {
            var m_Level2 = await _context.HrShiftDetails.FindAsync(id);

            if (m_Level2 == null)
            {
                return NotFound();
            }

            return m_Level2;
        }
        [HttpGet]
        [Route("GetbyShiftCode/{id}")]
        public async Task<ActionResult<IEnumerable<M_HrShiftDetails>>> GetLevel2WithLevel1(string id)
        {

            var m_Level2 = await _context.HrShiftDetails.Where(x => x.ShiftCode == id).ToListAsync();

            if (m_Level2 == null)
            {
                return NotFound();
            }

            return m_Level2;
        }

       
        [HttpPut]
        [Route("update")]
  public async Task<IActionResult> update(M_HrShiftDetails m_Level2)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var obj = new M_HrShiftDetails();
                obj = await _context.HrShiftDetails.FindAsync(m_Level2.Code);

                if (obj != null)
                {
                    
                    obj.StartTime = m_Level2.StartTime;
                    obj.Address= m_Level2.Address;

                    obj.EndTime= m_Level2.EndTime;  
                    obj.Status=m_Level2.Status??"0"; 
                    obj.ShiftCode = m_Level2.ShiftCode;
                    obj.ShiftName = m_Level2.ShiftName;
                    
                }
               
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
        public async Task<ActionResult<M_HrShiftDetails>> create(M_HrShiftDetails m_Level2)
        {
            m_Level2.Code = getUpdateMasterCount();
            m_Level2.DateAdded = DateTime.Now.ToString();
            _context.HrShiftDetails.Add(m_Level2);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetM_Level2", new { id = m_Level2.Code }, m_Level2);
        }
        public string getUpdateMasterCount()
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            //MAX( CAST(CODE AS BIGINT  ))
            strsql = @"SELECT ISNULL(MAX( CAST(CODE AS BIGINT  ))  ,0) +1  AS code FROM HrShiftDetails";

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
        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_HrShiftDetails>> DeleteM_Level2(string id)
        {
            var m_Level2 = await _context.HrShiftDetails.FindAsync(id);
            if (m_Level2 == null)
            {
                return NotFound();
            }

            _context.HrShiftDetails.Remove(m_Level2);
            await _context.SaveChangesAsync();

            return m_Level2;
        }

        //private bool M_Level2Exists(int id)
        //{
        //    return _context.AS_Acclevel2.Any(e => e.ID == id);
        //}
    }
}
