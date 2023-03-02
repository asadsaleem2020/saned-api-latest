using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MechSuitsApi.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using CoreInfrastructure.ItemInformation.Barcode;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MechSuitsApi.Areas.Items.Controllers
{
    [Authorize]
    [Route("api/Barcodegenerate")]
    [ApiController]
    [EnableCors("AllowOrigin")] 
    public class BarcodegenerateController : Controller
    {
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AppDBContext _context;
        private SqlConnection con;
        string companycode = "";   string user = "";
        D_DB dbset = null;
        Connection c = new Connection();
        public BarcodegenerateController(AppDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            con = c.V_connection;
            dbset = new D_DB(c.V_connection);
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
           companycode = currentUser.FindFirst("Company").Value.ToString();    user = currentUser.FindFirst("User").Value.ToString();
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_BarcodeGenerate>>> Get()
        {
            return await _context.BarcodeGenerate.ToListAsync();
        }
        


        [HttpGet("{code}")]
        public async Task<ActionResult<M_BarcodeGenerate>> Get(string code)
        {
            
            var m = await _context.BarcodeGenerate.FindAsync(companycode, code);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_BarcodeGenerate m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_BarcodeGenerate obj = new M_BarcodeGenerate();
                
                obj = await _context.BarcodeGenerate.FindAsync(companycode, m.Barcode);

                if (obj != null)
                {
                    

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

        [HttpPut("{id}")]

        [HttpPost]
        public async Task<ActionResult<M_BarcodeGenerate>> create(M_BarcodeGenerate m)
        {
            Console.WriteLine("Inserting Barcode");
            m.Barcode = m.Code + "#" +m.Weight.ToString();
         
            _context.BarcodeGenerate.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { code = m.Barcode }, m);
        }

        // DELETE: api/VendorType/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_BarcodeGenerate>> Delete(string id)
        {
            
            var m = await _context.BarcodeGenerate.FindAsync(companycode, id);
            if (m == null)
            {
                return NotFound();
            }

            _context.BarcodeGenerate.Remove(m);
            await _context.SaveChangesAsync();

            return m;
        }




    }
}
