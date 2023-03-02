
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MechSuitsApi.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using CoreInfrastructure.ItemInformation.ProductDiscount;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Data;
using MechSuitsApi.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MechSuitsApi.Areas.Items.Controllers
{
    [Authorize]
    [Route("api/productdiscount")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ProductDiscountController : ControllerBase
    {

        private readonly IUriService uriService;
        private readonly AppDBContext _context;
        private SqlConnection con;
       
        D_DB dbset = null;
        string companycode = ""; string user = "";
        private IHttpContextAccessor _httpContextAccessor;
        Connection c = new Connection();
        public ProductDiscountController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context;
            con = c.V_connection;
            dbset = new D_DB(c.V_connection);
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            companycode = currentUser.FindFirst("Company").Value.ToString(); user = currentUser.FindFirst("User").Value.ToString();
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_ProductDiscount>>> Get()
        {
            return await _context.ProductDiscount.ToListAsync();
        }

        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_ProductDiscount>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.ProductDiscount
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.ProductDiscount.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_ProductDiscount>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_ProductDiscount>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            // return await _context.Area.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.ProductDiscount.Where(m => m.Name.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.ProductDiscount.Where(m => m.Name.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_ProductDiscount>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }


        [HttpGet("{code}")]
        public async Task<ActionResult<M_ProductDiscount>> Get(long code)
        {
            string COMPANY_CODE = "C001";
            var m = await _context.ProductDiscount.FindAsync(COMPANY_CODE, code);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }
        [HttpPut]
        public async Task<IActionResult> Put(M_ProductDiscount m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_ProductDiscount obj = new M_ProductDiscount();
                string COMPANY_CODE = "C001";
                obj = await _context.ProductDiscount.FindAsync(COMPANY_CODE, m.Code);

                if (obj != null)
                {
                    obj.COMPANY_CODE = m.COMPANY_CODE;


                    obj.Name = m.Name;
                    obj.DayandDate = m.DayandDate;
                    obj.DiscountCategory = m.DiscountCategory;
                    obj.discType = m.discType;
                    obj.enddate = m.enddate;
                    obj.startdate = m.startdate;
                    obj.SelectedDays = m.SelectedDays;
                    obj.Locked = m.Locked;
                    obj.SingleProduct = m.SingleProduct;
                    obj.Level1Code = m.Level1Code;
                    obj.Level2Code = m.Level2Code;
                    obj.Level3Code = m.Level3Code;
                    obj.Amount = m.Amount;



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
        public async Task<ActionResult<M_ProductDiscount>> create(M_ProductDiscount m)
        {
            m.Code = dbset.getUpdateMasterCount();
            _context.ProductDiscount.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { code = m.Code }, m);
        }

        // DELETE: api/VendorType/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_ProductDiscount>> Delete(long id)
        {
            string COMPANY_CODE = "C001";
            var m = await _context.ProductDiscount.FindAsync(COMPANY_CODE, id);
            if (m == null)
            {
                return NotFound();
            }

            _context.ProductDiscount.Remove(m);
            await _context.SaveChangesAsync();

            return m;
        }





    }
}
