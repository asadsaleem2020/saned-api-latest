using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreInfrastructure.Recruitement.RecruitmentOrder;
using MechSuitsApi.Classes;
using MechSuitsApi.Interfaces;
using System.Security.Claims;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Cors;

namespace MechSuitsApi.Areas.OrderDetails
{
    [Route("api/ClientOrderUpdate")]
    [ApiController]
    // [Authorize]
    [EnableCors("AllowOrigin")]
    public class ClientOrderUpdateController : ControllerBase
    {
        private readonly IUriService uriService;
        string companycode = ""; string user = "";
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        private IHttpContextAccessor _httpContextAccessor;
        public ClientOrderUpdateController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
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
        [Route("chunks/{orderNumber}")]
        public async Task<ActionResult<IEnumerable<M_OrderUpdate>>> Get11(string orderNumber, [FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var m = _context.OrderDetails_ClientOrderUpdate.
         FromSqlRaw("Select CompanyCode, Code, OrderNumber, Reply, [Type], AttachedFile, AddedOn, (SELECT Name From APP_USERS Where Code=AddedBy) as AddedBy, Status, Sort, Locked from OrderDetails_ClientOrderUpdate where [OrderNumber]='" + orderNumber + "'");

            var pagedData = m.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();
            var totalRecords = await m.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("chunks/type/{type}")]
        public async Task<ActionResult<IEnumerable<M_OrderUpdate>>> Get1(string type, [FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var m = _context.OrderDetails_ClientOrderUpdate.
         FromSqlRaw("Select CompanyCode, Code, OrderNumber, Reply, [Type], AttachedFile, AddedOn, (SELECT Name From APP_USERS Where Code=AddedBy) as AddedBy, Status, Sort, Locked from OrderDetails_ClientOrderUpdate where [Type]='" + type + "'");

            var pagedData = m.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();
            var totalRecords = await m.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_ClientOrderUpdate>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.OrderDetails_ClientOrderUpdate.Where(m => m.Code.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.OrderDetails_ClientOrderUpdate.Where(m => m.Code.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);

        }
        // GET: api/OrderUpdate
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_ClientOrderUpdate>>> GetRecruitementOrder_OrderUpdate()
        {
            return await _context.OrderDetails_ClientOrderUpdate.ToListAsync();
        }

        [HttpGet("{type}/ByOrderNumber/{orderNumber}")]
        public async Task<ActionResult<IEnumerable<M_ClientOrderUpdate>>> GetRecruitementOrder_byOrderAndType(string type, string orderNumber)
        {
            var m = _context.OrderDetails_ClientOrderUpdate.
        FromSqlRaw("Select CompanyCode, Code, OrderNumber, Reply, [Type], AttachedFile, AddedOn, (SELECT Name From APP_USERS Where Code=AddedBy) as AddedBy, Status, Sort, Locked from OrderDetails_ClientOrderUpdate where [OrderNumber]='" + orderNumber + "' AND [Type]='" + type + "' ORDER BY Code DESC");

            // return await _context.OrderDetails_OrderUpdate.Where(m => m.OrderNumber == orderNumber).OrderByDescending(s => s.Code).ToListAsync();
            return await m.ToListAsync();
        }
        [HttpGet("ByOrderNumber/{orderNumber}")]
        public async Task<ActionResult<IEnumerable<M_OrderUpdate>>> GetRecruitementOrder_byOrder(string orderNumber)
        {
            var m = _context.OrderDetails_OrderUpdate.
        FromSqlRaw("Select CompanyCode, Code, OrderNumber, Reply, [Type], AttachedFile, AddedOn, (SELECT Name From APP_USERS Where Code=AddedBy) as AddedBy, Status, Sort, Locked from OrderDetails_OrderUpdate where [OrderNumber]='" + orderNumber + "' ORDER BY Code DESC");

            // return await _context.OrderDetails_OrderUpdate.Where(m => m.OrderNumber == orderNumber).OrderByDescending(s => s.Code).ToListAsync();
            return await m.ToListAsync();
        }

        // GET: api/OrderUpdate/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_ClientOrderUpdate>> GetM_ClientOrderUpdate(string id)
        {
            var M_ClientOrderUpdate = await _context.OrderDetails_ClientOrderUpdate.FindAsync(id);

            if (M_ClientOrderUpdate == null)
            {
                return NotFound();
            }

            return M_ClientOrderUpdate;
        }

        // PUT: api/OrderUpdate/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutM_ClientOrderUpdate(string id, M_ClientOrderUpdate M_ClientOrderUpdate)
        {
            if (id != M_ClientOrderUpdate.Code)
            {
                return BadRequest();
            }

            _context.Entry(M_ClientOrderUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!M_ClientOrderUpdateExists(id))
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
        public async Task<ActionResult<M_ClientOrderUpdate>> PostM_ClientOrderUpdate(M_ClientOrderUpdate M_ClientOrderUpdate)
        {
            int maxValue = int.Parse(_context.OrderDetails_ClientOrderUpdate.Max(m => m.Code) ?? "10000");
            //Console.WriteLine(maxValue);
            M_ClientOrderUpdate.Code = (maxValue + 1).ToString();
            _context.OrderDetails_ClientOrderUpdate.Add(M_ClientOrderUpdate);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (M_ClientOrderUpdateExists(M_ClientOrderUpdate.Code))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetM_ClientOrderUpdate", new { id = M_ClientOrderUpdate.Code }, M_ClientOrderUpdate);
        }

        // DELETE: api/OrderUpdate/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteM_ClientOrderUpdate(string id)
        {
            var M_ClientOrderUpdate = await _context.OrderDetails_ClientOrderUpdate.FindAsync(id);
            if (M_ClientOrderUpdate == null)
            {
                return NotFound();
            }

            _context.OrderDetails_ClientOrderUpdate.Remove(M_ClientOrderUpdate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool M_ClientOrderUpdateExists(string id)
        {
            return _context.OrderDetails_ClientOrderUpdate.Any(e => e.Code == id);
        }
    }
}
