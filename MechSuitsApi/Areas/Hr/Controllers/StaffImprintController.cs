using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreInfrastructure.Hr.Setup;
using MechSuitsApi.Classes;
using MechSuitsApi.Interfaces;
using System.Data.SqlClient;
using System.Security.Claims;
using CoreInfrastructure.Recruitement;

namespace MechSuitsApi.Areas.Hr.Controllers
{
    [Route("api/StaffImprint")]
    [ApiController]
    public class StaffImprintController : ControllerBase
    {
        private readonly IUriService uriService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AppDBContext _context;
        private SqlConnection con;
        string companycode = "";
        string user = "";
        Connection c = new Connection();

        public StaffImprintController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            con = c.V_connection;
        }

        // GET: api/StaffImprint/chunks
        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_StaffImprint>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var m = _context.HrStaffImprint.
         FromSqlRaw("SELECT CompanyCode, Code, (Select Name from HR_EMPLOYEE where Code = EmployeeID) as EmployeeID, AddedDate, ModifiedDate, ActivateStatus, FingerPrintID, Notes, Status, Locked, Sort FROM [SanedDatabase].[dbo].[HrStaffImprint]");

            var pagedData = m.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();
            var totalRecords = await m.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_StaffImprint>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_StaffImprint>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.HrStaffImprint.Where(m => m.Code.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.HrStaffImprint.Where(m => m.Code.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_StaffImprint>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);

        }
        // GET: api/StaffImprint
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_StaffImprint>>> GetHrStaffImprint()
        {
            return await _context.HrStaffImprint.ToListAsync();
        }

        // GET: api/StaffImprint/5
        [HttpGet("{Code}")]
        public async Task<ActionResult<M_StaffImprint>> GetM_StaffImprint(string Code)
        {
            var m_StaffImprint = await _context.HrStaffImprint.FindAsync(Code);

            if (m_StaffImprint == null)
            {
                return NotFound();
            }

            return m_StaffImprint;
        }

        // PUT: api/StaffImprint/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutM_StaffImprint(string id, M_StaffImprint m_StaffImprint)
        {
            if (id != m_StaffImprint.Code)
            {
                return BadRequest();
            }

            _context.Entry(m_StaffImprint).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!M_StaffImprintExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/StaffImprint
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<M_StaffImprint>> PostM_StaffImprint(M_StaffImprint m_StaffImprint)
        {
            int maxValue = int.Parse(_context.HrStaffImprint.Max(m => m.Code) ?? "10000");
            //Console.WriteLine(maxValue);
            m_StaffImprint.Code = (maxValue + 1).ToString();
            _context.HrStaffImprint.Add(m_StaffImprint);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (M_StaffImprintExists(m_StaffImprint.Code))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetM_StaffImprint", new { Code = m_StaffImprint.Code }, m_StaffImprint);
        }
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_StaffImprint m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                M_StaffImprint obj = new M_StaffImprint();
                obj = await _context.HrStaffImprint.FindAsync(m.Code);
                if (obj == null)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    obj.ModifiedDate = DateTime.Now;
                    obj.ActivateStatus = m.ActivateStatus;
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
        // DELETE: api/StaffImprint/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteM_StaffImprint(string id)
        {
            var m_StaffImprint = await _context.HrStaffImprint.FindAsync(id);
            if (m_StaffImprint == null)
            {
                return NotFound();
            }

            _context.HrStaffImprint.Remove(m_StaffImprint);
            await _context.SaveChangesAsync();

            return Ok("Deleted SuccessFully !!!");
        }

        private bool M_StaffImprintExists(string id)
        {
            return _context.HrStaffImprint.Any(e => e.Code == id);
        }
    }
}
