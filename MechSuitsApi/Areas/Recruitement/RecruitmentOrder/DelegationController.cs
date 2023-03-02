using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreInfrastructure.Recruitement.RecruitmentOrder;
using MechSuitsApi.Classes;
using CoreInfrastructure.Hr.Setup;
using MechSuitsApi.Interfaces;
using System.Security.Claims;
using System.Data.SqlClient;

namespace MechSuitsApi.Areas.Recruitement.RecruitmentOrder
{
    [Route("api/[controller]")]
    [ApiController]
    public class DelegationController : ControllerBase
    {
        private readonly IUriService uriService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AppDBContext _context;
        private SqlConnection con;
        string companycode = "";
        string user = "";
        Connection c = new Connection();

        public DelegationController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            con = c.V_connection;
        }
        // GET: api/StaffImprint/chunks
        [HttpGet]
        [Route("chunks/{orderNumber}")]
        public async Task<ActionResult<IEnumerable<M_Delegation>>> Get1(string orderNumber,[FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var m = _context.RecruitementOrder_delegation.
         FromSqlRaw($"SELECT * FROM [SanedDatabase].[dbo].[RecruitementOrder_delegation] where [OrderNumber]={orderNumber}");

            var pagedData = m.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();
            var totalRecords = await m.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Delegation>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_Delegation>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.RecruitementOrder_delegation.Where(m => m.Code.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.RecruitementOrder_delegation.Where(m => m.Code.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Delegation>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);

        }
        // GET: api/Delegation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_Delegation>>> GetRecruitementOrder_delegation()
        {
            return await _context.RecruitementOrder_delegation.ToListAsync();
        }

        // GET: api/Delegation/5
        [HttpGet("{Code}")]
        public async Task<ActionResult<M_Delegation>> GetM_Delegation(string Code)
        {
            var m_Delegation = await _context.RecruitementOrder_delegation.FindAsync(Code);

            if (m_Delegation == null)
            {
                return NotFound();
            }

            return m_Delegation;
        }

        // PUT: api/Delegation/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutM_Delegation(string id, M_Delegation m_Delegation)
        {
            if (id != m_Delegation.Code)
            {
                return BadRequest();
            }

            _context.Entry(m_Delegation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!M_DelegationExists(id))
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

        // POST: api/Delegation
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<M_Delegation>> PostM_Delegation(M_Delegation m_Delegation)
        {
            int maxValue = int.Parse(_context.RecruitementOrder_delegation.Max(m => m.Code) ?? "10000");
            Console.WriteLine(maxValue);
            m_Delegation.Code = (maxValue + 1).ToString();
            _context.RecruitementOrder_delegation.Add(m_Delegation);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (M_DelegationExists(m_Delegation.Code))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetM_Delegation", new { Code = m_Delegation.Code }, m_Delegation);
        }

        // DELETE: api/Delegation/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteM_Delegation(string id)
        {
            var m_Delegation = await _context.RecruitementOrder_delegation.FindAsync(id);
            if (m_Delegation == null)
            {
                return NotFound();
            }

            _context.RecruitementOrder_delegation.Remove(m_Delegation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool M_DelegationExists(string id)
        {
            return _context.RecruitementOrder_delegation.Any(e => e.Code == id);
        }
    }
}
