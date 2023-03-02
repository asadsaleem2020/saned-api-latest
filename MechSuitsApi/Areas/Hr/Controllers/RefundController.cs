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
    [Route("api/refund")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class RefundController : Controller
    {
        private readonly IUriService uriService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        string companycode = "";   string user = "";
        public RefundController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context;
            con = c.V_connection;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
       //   companycode = currentUser.FindFirst("Company").Value.ToString();    user = currentUser.FindFirst("User").Value.ToString();
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_Refund>>> GetAS_Acclevel2()
        {
            return await _context.HR_REFUND.ToListAsync();
        }


        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_Refund>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.HR_REFUND
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.HR_REFUND.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Refund>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }


        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_Refund>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            // return await _context.Area.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.HR_REFUND.Where(m => m.EMPLOYEE_CODE.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.HR_REFUND.Where(m => m.EMPLOYEE_CODE.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Refund>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }






        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_Refund>> GetM_Level2(string id)
        {
            var m_Level2 = await _context.HR_REFUND.FindAsync(id);

            if (m_Level2 == null)
            {
                return NotFound();
            }

            return m_Level2;
        }
        [HttpGet]
        [Route("GetLevel2WithLevel1")]
        public async Task<ActionResult<IEnumerable<M_Refund>>> GetLevel2WithLevel1(string id)
        {

            var m_Level2 = await _context.HR_REFUND.Where(x => x.DOCUMENT_NO == id).ToListAsync();

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
        public async Task<IActionResult> update(M_Refund m_Level2)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var obj = new M_Refund();
                obj = await _context.HR_REFUND.FindAsync(m_Level2.DOCUMENT_NO);

                if (obj != null)
                {
                    // obj.COMPANY_CODE = m_Level2.COMPANY_CODE;
                    // obj.DOCUMENT_NO = m_Level2.DOCUMENT_NO;
                    obj.EMPLOYEE_CODE = m_Level2.EMPLOYEE_CODE;
                    obj.NAME = m_Level2.NAME;
                    obj.GL_ACCOUNT_CODE = m_Level2.GL_ACCOUNT_CODE;
                    obj.GL_ACCOUNT_NAME = m_Level2.GL_ACCOUNT_NAME;
                    obj.DOCUMENT_DATE = m_Level2.DOCUMENT_DATE;
                    obj.AMOUNT = m_Level2.AMOUNT;
                    obj.REMARKS = m_Level2.REMARKS;
                    obj.STATUS = m_Level2.STATUS;
                    obj.CHECKED = m_Level2.CHECKED;
                    obj.DOC = m_Level2.DOC;
                    obj.ISDELETED = m_Level2.ISDELETED;
                    obj.CREATED_ON = m_Level2.CREATED_ON;
                    obj.CREATED_BY = m_Level2.CREATED_BY;
                    obj.UPDATED_ON = m_Level2.UPDATED_ON;
                    obj.UPDATED_BY = m_Level2.UPDATED_BY;
                    obj.APPROVED_ON = m_Level2.APPROVED_ON;
                    obj.APPROVED_BY = m_Level2.APPROVED_BY;
                    obj.DELETED_ON = m_Level2.DELETED_ON;
                    obj.DELETED_BY = m_Level2.DELETED_BY;
                    obj.TYPE = m_Level2.TYPE;

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
        public async Task<ActionResult<M_Refund>> create(M_Refund m_Level2)
        {
            m_Level2.DOCUMENT_NO = getUpdateMasterCount();
            _context.HR_REFUND.Add(m_Level2);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetM_Level2", new { id = m_Level2.DOCUMENT_NO }, m_Level2);
        }
        public string getUpdateMasterCount()
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            //MAX( CAST(CODE AS BIGINT  ))
            strsql = @"SELECT ISNULL(MAX( CAST(DOCUMENT_NO AS BIGINT  ))  ,0) +1  AS code FROM HR_REFUND";

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
        public async Task<ActionResult<M_Refund>> DeleteM_Level2(string id)
        {
            var m_Level2 = await _context.HR_REFUND.FindAsync(id);
            if (m_Level2 == null)
            {
                return NotFound();
            }

            _context.HR_REFUND.Remove(m_Level2);
            await _context.SaveChangesAsync();

            return m_Level2;
        }

        //private bool M_Level2Exists(int id)
        //{
        //    return _context.AS_Acclevel2.Any(e => e.ID == id);
        //}
    }
}

