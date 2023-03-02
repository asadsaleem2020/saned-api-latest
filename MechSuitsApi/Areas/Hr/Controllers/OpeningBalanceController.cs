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
using Microsoft.AspNetCore.Authorization;

namespace MechSuitsApi.Areas.Hr.Controllers
{

   // [Authorize]
    [Route("api/Opening_Balance")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class OpeningBalanceController : Controller
    {
        private readonly IUriService uriService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AppDBContext _context;
        string companycode = "";   string user = "";
        private SqlConnection con;
        Connection c = new Connection();
        public OpeningBalanceController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context;
            con = c.V_connection;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
       //     companycode = currentUser.FindFirst("Company").Value.ToString(); user = currentUser.FindFirst("User").Value.ToString();
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_OpeningBalance>>> GetAS_Acclevel2()
        {
            return await _context.HR_Opening_Balances.ToListAsync();
        }



        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_OpeningBalance>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.HR_Opening_Balances
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.HR_Opening_Balances.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_OpeningBalance>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }


        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_OpeningBalance>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            // return await _context.Area.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.HR_Opening_Balances.Where(m => m.AccountCode.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.HR_Opening_Balances.Where(m => m.AccountCode.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_OpeningBalance>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }





        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_OpeningBalance>> GetM_Level2(string id)
        {
            var m_Level2 = await _context.HR_Opening_Balances.FindAsync(id);

            if (m_Level2 == null)
            {
                return NotFound();
            }

            return m_Level2;
        }
        [HttpGet]
        [Route("GetLevel2WithLevel1")]
        public async Task<ActionResult<IEnumerable<M_OpeningBalance>>> GetLevel2WithLevel1(string id)
        {

            var m_Level2 = await _context.HR_Opening_Balances.Where(x => x.PeriodID == id).ToListAsync();

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
        public async Task<IActionResult> update(M_OpeningBalance m_Level2)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var obj = new M_OpeningBalance();
                obj = await _context.HR_Opening_Balances.FindAsync(m_Level2.PeriodID);

                if (obj != null)
                {
                    obj.AccountCode = m_Level2.AccountCode;
                   // obj.PeriodID = m_Level2.PeriodID;
                    obj.Company_Code = m_Level2.Company_Code;
                    obj.OpeningDr = m_Level2.OpeningDr;

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

        [HttpPut("{id}")]
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
        public async Task<ActionResult<M_OpeningBalance>> create(M_OpeningBalance m_Level2)
        {
            m_Level2.PeriodID = getUpdateMasterCount();
            _context.HR_Opening_Balances.Add(m_Level2);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetM_Level2", new { id = m_Level2.PeriodID }, m_Level2);
        }
        public string getUpdateMasterCount()
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            //MAX( CAST(CODE AS BIGINT  ))
            strsql = @"SELECT ISNULL(MAX( CAST(PeriodID AS BIGINT  ))  ,0) +1  AS code FROM HR_Opening_Balances";

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
        public async Task<ActionResult<M_OpeningBalance>> DeleteM_Level2(string id)
        {
            var m_Level2 = await _context.HR_Opening_Balances.FindAsync(id);
            if (m_Level2 == null)
            {
                return NotFound();
            }

            _context.HR_Opening_Balances.Remove(m_Level2);
            await _context.SaveChangesAsync();

            return m_Level2;
        }

        //private bool M_Level2Exists(int id)
        //{
        //    return _context.AS_Acclevel2.Any(e => e.ID == id);
        //}
    }
}
