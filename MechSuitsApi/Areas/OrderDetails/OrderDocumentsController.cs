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
    [Route("api/OrderDocuments")]
    [ApiController]
    // [Authorize]
    [EnableCors("AllowOrigin")]
    public class OrderDocumentsController : ControllerBase
    {
        private readonly IUriService uriService;
        string companycode = ""; string user = "";
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        private IHttpContextAccessor _httpContextAccessor;
        public OrderDocumentsController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
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
        public async Task<ActionResult<IEnumerable<M_OrderDocuments>>> Get1(string orderNumber, [FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var m = _context.OrderDetails_OrderDocuments.
         FromSqlRaw($"Select CompanyCode, Code, OrderNumber,Type, Title, AttachedDocument, AddedOn, AddedBy, Status, Sort, Locked from OrderDetails_OrderDocuments where [OrderNumber]={orderNumber}");

            var pagedData = m.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();
            var totalRecords = await m.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("chunks/type/{type}")]
        public async Task<ActionResult<IEnumerable<M_OrderDocuments>>> Get111(string type, [FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var m = _context.OrderDetails_OrderDocuments.
         FromSqlRaw($"Select CompanyCode, Code, OrderNumber,Type, Title, AttachedDocument, AddedOn, AddedBy, Status, Sort, Locked from OrderDetails_OrderDocuments where [Type]='" + type + "'");

            var pagedData = m.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();
            var totalRecords = await m.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_OrderDocuments>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.OrderDetails_OrderDocuments.Where(m => m.Code.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.OrderDetails_OrderDocuments.Where(m => m.Code.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);

        }
        // GET: api/OrderUpdate
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_OrderDocuments>>> GetOrderDetails_OrderDocuments()
        {
            return await _context.OrderDetails_OrderDocuments.ToListAsync();
        }
        [HttpGet("{type}/ByOrderNumber/{orderNumber}")]
        public async Task<ActionResult<IEnumerable<M_OrderDocuments>>> GetRecruitementOrder_byOrderAndType(string type, string orderNumber)
        {
            var m = _context.OrderDetails_OrderDocuments.
        FromSqlRaw($"Select CompanyCode, Code, OrderNumber, Type, Title, AttachedDocument, AddedOn, (SELECT Name From APP_USERS Where Code=AddedBy) as AddedBy, Status, Sort, Locked from OrderDetails_OrderDocuments where [OrderNumber]='" + orderNumber + "' AND [Type]='" + type + "' ORDER BY Code DESC");

            // return await _context.OrderDetails_OrderUpdate.Where(m => m.OrderNumber == orderNumber).OrderByDescending(s => s.Code).ToListAsync();
            return await m.ToListAsync();
        }
        [HttpGet("ByOrderNumber/{orderNumber}")]
        public async Task<ActionResult<IEnumerable<M_OrderDocuments>>> GetRecruitementOrder_byOrder(string orderNumber)
        {
            var m = _context.OrderDetails_OrderDocuments.
        FromSqlRaw($"Select CompanyCode, Code, OrderNumber, Type, Title, AttachedDocument, AddedOn, (SELECT Name From APP_USERS Where Code=AddedBy) as AddedBy, Status, Sort, Locked from OrderDetails_OrderDocuments where [OrderNumber]={orderNumber} ORDER BY Code DESC");

            // return await _context.OrderDetails_OrderDocuments.Where(m => m.OrderNumber == orderNumber).OrderByDescending(s => s.Code).ToListAsync();
            return await m.ToListAsync();
        }

        // GET: api/OrderUpdate/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_OrderDocuments>> GetM_OrderDocuments(string id)
        {
            var M_OrderDocuments = await _context.OrderDetails_OrderDocuments.FindAsync(id);

            if (M_OrderDocuments == null)
            {
                return NotFound();
            }

            return M_OrderDocuments;
        }

        // PUT: api/OrderUpdate/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutM_OrderDocuments(string id, M_OrderDocuments M_OrderDocuments)
        {
            if (id != M_OrderDocuments.Code)
            {
                return BadRequest();
            }

            _context.Entry(M_OrderDocuments).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!M_OrderDocumentsExists(id))
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
        public async Task<ActionResult<M_OrderDocuments>> PostM_OrderDocuments(M_OrderDocuments M_OrderDocuments)
        {
            int maxValue = int.Parse(_context.OrderDetails_OrderDocuments.Max(m => m.Code) ?? "10000");
            //Console.WriteLine(maxValue);
            M_OrderDocuments.Code = (maxValue + 1).ToString();
            M_OrderDocuments.AddedOn = DateTime.Now.ToString();
            _context.OrderDetails_OrderDocuments.Add(M_OrderDocuments);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (M_OrderDocumentsExists(M_OrderDocuments.Code))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetM_OrderDocuments", new { id = M_OrderDocuments.Code }, M_OrderDocuments);
        }

        // DELETE: api/OrderUpdate/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteM_OrderDocuments(string id)
        {
            var M_OrderDocuments = await _context.OrderDetails_OrderDocuments.FindAsync(id);
            if (M_OrderDocuments == null)
            {
                return NotFound();
            }

            _context.OrderDetails_OrderDocuments.Remove(M_OrderDocuments);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool M_OrderDocumentsExists(string id)
        {
            return _context.OrderDetails_OrderDocuments.Any(e => e.Code == id);
        }
    }
}
