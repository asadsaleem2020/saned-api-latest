using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MechSuitsApi.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using CoreInfrastructure.Sale.Stocktransfer;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
namespace MechSuitsApi.Areas.Sales.Controllers
{
    [Authorize]
    [Route("api/stocktransfer")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class StockTransferController : ControllerBase
    {
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AppDBContext _context;
        private SqlConnection con;
        D_DB dbset = null;
        string companycode = "";   string user = "";
        Connection c = new Connection();
        public StockTransferController(AppDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            con = c.V_connection;
            dbset = new D_DB(c.V_connection);
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
          companycode = currentUser.FindFirst("Company").Value.ToString();    user = currentUser.FindFirst("User").Value.ToString();
        }

        // GET: api/VendorType
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_StockTransfer>>> Get()
        {
            return await _context.StockTransfer.ToListAsync();
        }

        // GET: api/VendorType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_StockTransfer>> Get(long id)
        {
            
            var m = await _context.StockTransfer.FindAsync(companycode, id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_StockTransfer m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_StockTransfer obj = new M_StockTransfer();
                obj = await _context.StockTransfer.FindAsync(m.COMPANY_CODE, m.documentno);

                if (obj != null)
                {
                     
                    obj.Name = m.Name;
                    obj.Code = m.Code;
                    obj.Qty = m.Qty;
                    obj.Rackno = m.Rackno;
                    obj.Locked = m.Locked;
                   

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

       

        [HttpPost]
        public async Task<ActionResult<M_StockTransfer>> create(M_StockTransfer m)
        {
            m.documentno = Convert.ToInt64(  dbset.getUpdateMasterCount());
            _context.StockTransfer.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { code = m.Code }, m);
        }

        // DELETE: api/VendorType/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_StockTransfer>> Delete(string id)
        {
            
            var m = await _context.StockTransfer.FindAsync(companycode, id);
            if (m == null)
            {
                return NotFound();
            }

            _context.StockTransfer.Remove(m);
            await _context.SaveChangesAsync();

            return m;
        }





    }
}
