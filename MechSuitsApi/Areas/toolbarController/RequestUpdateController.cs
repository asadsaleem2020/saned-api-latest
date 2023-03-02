using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MechSuitsApi.Classes;
using MechSuitsApi.Interfaces;
using System.Security.Claims;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Cors;
using CoreInfrastructure.ToolbarItems;

namespace MechSuitsApi.Areas.toolbarController
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    [EnableCors("AllowOrigin")]
    public class RequestUpdateController : ControllerBase
    {
        private readonly IUriService uriService;
        string companycode = ""; string user = "";
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        private IHttpContextAccessor _httpContextAccessor;
        public RequestUpdateController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context;
            con = c.V_connection;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            //   companycode = currentUser.FindFirst("Company").Value.ToString();
            //   user = currentUser.FindFirst("User").Value.ToString();
        }
        // GET: api/StaffImprint/chunks
        [HttpGet]
        [Route("chunks/{personID}")]
        public async Task<ActionResult<IEnumerable<M_AgentCustomer_RequestUpdate>>> Get11(string personID, [FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var m = _context.AgentCustomer_RequestUpdate.
         FromSqlRaw("Select CompanyCode, Code, PersonID, Reply, [Type], AttachedFile, AddedOn, (SELECT Name From APP_USERS Where Code=AddedBy) as AddedBy, Status, Sort, Locked from AgentCustomer_RequestUpdate where [PersonID]='" + personID + "'");

            var pagedData = m.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();
            var totalRecords = await m.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("chunks/type/{type}")]
        public async Task<ActionResult<IEnumerable<M_AgentCustomer_RequestUpdate>>> Get1(string type, [FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var m = _context.AgentCustomer_RequestUpdate.
         FromSqlRaw("Select CompanyCode, Code, PersonID, Reply, [Type], AttachedFile, AddedOn, (SELECT Name From APP_USERS Where Code=AddedBy) as AddedBy, Status, Sort, Locked from AgentCustomer_RequestUpdate where [Type]='" + type + "'");

            var pagedData = m.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();
            var totalRecords = await m.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_AgentCustomer_RequestUpdate>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.AgentCustomer_RequestUpdate.Where(m => m.Code.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.AgentCustomer_RequestUpdate.Where(m => m.Code.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);

        }
        // GET: api/OrderUpdate
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_AgentCustomer_RequestUpdate>>> GetAgentCustomer_RequestUpdate()
        {
            return await _context.AgentCustomer_RequestUpdate.ToListAsync();
        }

        [HttpGet("{type}/ByPersonID/{personID}")]
        public async Task<ActionResult<IEnumerable<M_AgentCustomer_RequestUpdate>>> GetRecruitementOrder_byOrderAndType(string type, string personID)
        {
            var m = _context.AgentCustomer_RequestUpdate.
        FromSqlRaw("Select CompanyCode, Code, PersonID, Reply, [Type], AttachedFile, AddedOn, (SELECT Name From APP_USERS Where Code=AddedBy) as AddedBy, Status, Sort, Locked from AgentCustomer_RequestUpdate where [PersonID]='" + personID + "' AND [Type]='" + type + "' ORDER BY Code DESC");

            // return await _context.AgentCustomer_RequestUpdate.Where(m => m.PersonID == personID).OrderByDescending(s => s.Code).ToListAsync();
            return await m.ToListAsync();
        }
        [HttpGet("ByPersonID/{personID}")]
        public async Task<ActionResult<IEnumerable<M_AgentCustomer_RequestUpdate>>> GetRecruitementOrder_byOrder(string personID)
        {
            var m = _context.AgentCustomer_RequestUpdate.
        FromSqlRaw("Select CompanyCode, Code, PersonID, Reply, [Type], AttachedFile, AddedOn, (SELECT Name From APP_USERS Where Code=AddedBy) as AddedBy, Status, Sort, Locked from AgentCustomer_RequestUpdate where [PersonID]='" + personID + "' ORDER BY Code DESC");

            // return await _context.AgentCustomer_RequestUpdate.Where(m => m.PersonID == personID).OrderByDescending(s => s.Code).ToListAsync();
            return await m.ToListAsync();
        }

        // GET: api/OrderUpdate/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_AgentCustomer_RequestUpdate>> GetM_AgentCustomer_RequestUpdate(string id)
        {
            var M_AgentCustomer_RequestUpdate = await _context.AgentCustomer_RequestUpdate.FindAsync(id);

            if (M_AgentCustomer_RequestUpdate == null)
            {
                return NotFound();
            }

            return M_AgentCustomer_RequestUpdate;
        }

        // PUT: api/OrderUpdate/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutM_AgentCustomer_RequestUpdate(string id, M_AgentCustomer_RequestUpdate M_AgentCustomer_RequestUpdate)
        {
            if (id != M_AgentCustomer_RequestUpdate.Code)
            {
                return BadRequest();
            }

            _context.Entry(M_AgentCustomer_RequestUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!M_AgentCustomer_RequestUpdateExists(id))
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

        // POST: api/OrderUpdate
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<M_AgentCustomer_RequestUpdate>> PostM_AgentCustomer_RequestUpdate(M_AgentCustomer_RequestUpdate M_AgentCustomer_RequestUpdate)
        {
            int maxValue = int.Parse(_context.AgentCustomer_RequestUpdate.Max(m => m.Code) ?? "10000");
            //Console.WriteLine(maxValue);
            M_AgentCustomer_RequestUpdate.Code = (maxValue + 1).ToString();
            M_AgentCustomer_RequestUpdate.AddedOn = DateTime.Now.ToString();
            _context.AgentCustomer_RequestUpdate.Add(M_AgentCustomer_RequestUpdate);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (M_AgentCustomer_RequestUpdateExists(M_AgentCustomer_RequestUpdate.Code))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetM_AgentCustomer_RequestUpdate", new { id = M_AgentCustomer_RequestUpdate.Code }, M_AgentCustomer_RequestUpdate);
        }

        // DELETE: api/OrderUpdate/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteM_AgentCustomer_RequestUpdate(string id)
        {
            var M_AgentCustomer_RequestUpdate = await _context.AgentCustomer_RequestUpdate.FindAsync(id);
            if (M_AgentCustomer_RequestUpdate == null)
            {
                return NotFound();
            }

            _context.AgentCustomer_RequestUpdate.Remove(M_AgentCustomer_RequestUpdate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool M_AgentCustomer_RequestUpdateExists(string id)
        {
            return _context.AgentCustomer_RequestUpdate.Any(e => e.Code == id);
        }
    }
}
