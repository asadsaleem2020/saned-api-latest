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
using System.Data;
using Microsoft.AspNetCore.Cors;

namespace MechSuitsApi.Areas.Recruitement.RecruitmentOrder
{
    [Route("api/Recruitement[controller]")]
    [ApiController]
    // [Authorize]
    [EnableCors("AllowOrigin")]
    public class AgentController : ControllerBase
    {
        private readonly IUriService uriService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AppDBContext _context;
        private SqlConnection con;
        string companycode = "";
        string user = "";
        Connection c = new Connection();

        public AgentController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
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
        public async Task<ActionResult<IEnumerable<M_Agent>>> Get1(string orderNumber, [FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var m = _context.RecruitementOrder_Agent.
         FromSqlRaw($"SELECT CompanyCode, Code, OrderNumber, AgentID, (Select Name From Agents Where Code = AgentID) as AgentName, WorkerID, (Select Name From Candidates Where Code = WorkerID) as WorkerName, WorkerRecruitmentCost, AmountPaid, Notes, AddedOn, AddedBy, ModifiedOn, ModifiedBy, DeletedOn, DeletedBy, Status, IsActive, Sort, Locked FROM RecruitementOrder_Agent where [OrderNumber]={orderNumber}");

            var pagedData = m.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();
            var totalRecords = await m.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Agent>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_Agent>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.RecruitementOrder_Agent.Where(m => m.Code.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.RecruitementOrder_Agent.Where(m => m.Code.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Agent>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);

        }
        // GET: api/Agent
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_Agent>>> GetRecruitementOrder_Agent()
        {
            return await _context.RecruitementOrder_Agent.ToListAsync();
        }

        // GET: api/Agent/5
        [HttpGet("{orderNumber}/{Code}")]
        public async Task<ActionResult<IEnumerable<M_Agent>>> GetM_Agent(string orderNumber, string Code)
        {
            var m = _context.RecruitementOrder_Agent.FromSqlRaw($"SELECT CompanyCode, Code, OrderNumber, AgentID, (Select Name From Agents Where Code = AgentID) as AgentName, WorkerID, (Select Name From Candidates Where Code = WorkerID) as WorkerName, WorkerRecruitmentCost, AmountPaid, Notes, AddedOn, AddedBy, ModifiedOn, ModifiedBy, DeletedOn, DeletedBy, Status, IsActive, Sort, Locked FROM RecruitementOrder_Agent where OrderNumber={orderNumber} AND Code={Code}");

            return await m.ToListAsync();
        }

        // PUT: api/Agent/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutM_Agent(string id, M_Agent M_Agent)
        {
            if (id != M_Agent.Code)
            {
                return BadRequest();
            }

            _context.Entry(M_Agent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!M_AgentExists(id))
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
        [HttpPut]
        [Route("paidamount/update")]
        public async Task<IActionResult> updateOrderStatus(M_Agent m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {

                M_Agent obj = new M_Agent();
                obj = await _context.RecruitementOrder_Agent.FindAsync(m.Code);
                if (obj != null)
                {
                    obj.AmountPaid = (int.Parse(obj.AmountPaid) + int.Parse(m.AmountPaid)).ToString();

                }
                _context.RecruitementOrder_Agent.Attach(obj).Property(x => x.AmountPaid).IsModified = true;
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            return Ok(m);
        }

        // POST: api/Agent
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<M_Agent>> PostM_Agent(M_Agent M_Agent)
        {
            int maxValue = int.Parse(_context.RecruitementOrder_Agent.Max(m => m.Code) ?? "10000");
            Console.WriteLine(maxValue);
            M_Agent.Code = (maxValue + 1).ToString();
            _context.RecruitementOrder_Agent.Add(M_Agent);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (M_AgentExists(M_Agent.Code))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetM_Agent", new { Code = M_Agent.Code, OrderNumber = M_Agent.OrderNumber }, M_Agent);
        }

        // DELETE: api/Agent/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteM_Agent(string id)
        {
            var M_Agent = await _context.RecruitementOrder_Agent.FindAsync(id);
            if (M_Agent == null)
            {
                return NotFound();
            }

            _context.RecruitementOrder_Agent.Remove(M_Agent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool M_AgentExists(string id)
        {
            return _context.RecruitementOrder_Agent.Any(e => e.Code == id);
        }
        public void UpdateAllToOne()
        {
            SqlDataReader sqldr;
            string strsql;
            strsql = "update RecruitementOrder_Agent set CancellationStatus='1'";
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(strsql, con);
            sqldr = cmd.ExecuteReader();
            sqldr.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }
}

