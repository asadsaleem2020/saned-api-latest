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
    [Route("api/OrderUpdate")]
    [ApiController]
    // [Authorize]
    [EnableCors("AllowOrigin")]
    public class OrderUpdateController : ControllerBase
    {
        private readonly IUriService uriService;
        string companycode = ""; string user = "";
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        private IHttpContextAccessor _httpContextAccessor;
        public OrderUpdateController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
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
         FromSqlRaw("Select CompanyCode, Code, OrderNumber, Reply, [Type], AttachedFile, AddedOn, (SELECT Name From APP_USERS Where Code=AddedBy) as AddedBy, Status, Sort, Locked from OrderDetails_OrderUpdate where [OrderNumber]='" + orderNumber + "'");

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
         FromSqlRaw("Select CompanyCode, Code, OrderNumber, Reply, [Type], AttachedFile, AddedOn, (SELECT Name From APP_USERS Where Code=AddedBy) as AddedBy, Status, Sort, Locked from OrderDetails_OrderUpdate where [Type]='" + type + "'");

            var pagedData = m.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();
            var totalRecords = await m.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_OrderUpdate>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.OrderDetails_OrderUpdate.Where(m => m.Code.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.OrderDetails_OrderUpdate.Where(m => m.Code.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);

        }
        // GET: api/OrderUpdate
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_OrderUpdate>>> GetOrderDetails_OrderUpdate()
        {
            return await _context.OrderDetails_OrderUpdate.ToListAsync();
        }

        [HttpGet("{type}/ByOrderNumber/{orderNumber}")]
        public async Task<ActionResult<IEnumerable<M_OrderUpdate>>> GetRecruitementOrder_byOrderAndType(string type,string orderNumber)
        {
            var m = _context.OrderDetails_OrderUpdate.
        FromSqlRaw("Select CompanyCode, Code, OrderNumber, Reply, [Type], AttachedFile, AddedOn, (SELECT Name From APP_USERS Where Code=AddedBy) as AddedBy, Status, Sort, Locked from OrderDetails_OrderUpdate where [OrderNumber]='"+ orderNumber + "' AND [Type]='"+ type + "' ORDER BY Code DESC");

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
        public async Task<ActionResult<M_OrderUpdate>> GetM_OrderUpdate(string id)
        {
            var m_OrderUpdate = await _context.OrderDetails_OrderUpdate.FindAsync(id);

            if (m_OrderUpdate == null)
            {
                return NotFound();
            }

            return m_OrderUpdate;
        }

        // PUT: api/OrderUpdate/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutM_OrderUpdate(string id, M_OrderUpdate m_OrderUpdate)
        {
            if (id != m_OrderUpdate.Code)
            {
                return BadRequest();
            }

            _context.Entry(m_OrderUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!M_OrderUpdateExists(id))
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
        public async Task<ActionResult<M_OrderUpdate>> PostM_OrderUpdate(M_OrderUpdate m_OrderUpdate)
        {
            int maxValue = int.Parse(_context.OrderDetails_OrderUpdate.Max(m => m.Code) ?? "10000");
            //Console.WriteLine(maxValue);
            m_OrderUpdate.Code = (maxValue + 1).ToString();
            m_OrderUpdate.AddedOn = DateTime.Now.ToString();
            _context.OrderDetails_OrderUpdate.Add(m_OrderUpdate);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (M_OrderUpdateExists(m_OrderUpdate.Code))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetM_OrderUpdate", new { id = m_OrderUpdate.Code }, m_OrderUpdate);
        }

        // DELETE: api/OrderUpdate/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteM_OrderUpdate(string id)
        {
            var m_OrderUpdate = await _context.OrderDetails_OrderUpdate.FindAsync(id);
            if (m_OrderUpdate == null)
            {
                return NotFound();
            }

            _context.OrderDetails_OrderUpdate.Remove(m_OrderUpdate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool M_OrderUpdateExists(string id)
        {
            return _context.OrderDetails_OrderUpdate.Any(e => e.Code == id);
        }
    }
}
