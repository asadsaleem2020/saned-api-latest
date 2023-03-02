using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MechSuitsApi.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using System.Data.SqlClient;
using System.Data;
using Executer;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MechSuitsApi.Interfaces;
using CoreInfrastructure.AccomodationSystem;
using CoreInfrastructure.Recruitement;

namespace MechSuitsApi.Areas.AccomodationSystem
{
    // [Authorize]
    [Route("api/Refunding")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class RefundController : ControllerBase
    {

        private readonly IUriService uriService;
        string companycode = "C001"; string user = "";
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        private IHttpContextAccessor _httpContextAccessor;
        public RefundController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context;
            con = c.V_connection;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            //   companycode = currentUser.FindFirst("Company").Value.ToString();
            //   user = currentUser.FindFirst("User").Value.ToString();
        }

        // GET: api/Level2
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_Refunds>>> GetList()
        {
            Console.WriteLine("HELLO WORLD");
            return await _context.Refund.ToListAsync();
        }
        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_Refunds>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            //
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var m = _context.Refund.
         FromSqlRaw("SELECT ID, Company_Code, Date, CompanyID, ContractAmount, tax, rafund, taxamount, PaymentType, Status, Sort, " +
         "Locked, Code,(Select Name from WorkerData Where Code=(Select WorkerID from [Order] where OrderNumber= ClientCode)) as WorkerID, " +
         "(Select Name from RCustomer Where Code=(Select ClientID from [Order] where OrderNumber= ClientCode)) as ClientCode, " +
         "Note FROM Refund");
            //(Select Name from RCustomer where Code= ClientCode) as 
            var pagedData = m
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList ();
            var totalRecords = await m.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Refunds>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("chunksType")]
        public async Task<ActionResult<IEnumerable<M_Refunds>>> Gettype([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            string title = HttpContext.Request.Query["title"];
            var route = Request.Path.Value + "?title=" + title;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);

            var d = title;
            if (title == "3") { d = "1 OR PaymentType = 2"; }
            Console.WriteLine(d);
            int totalRecords;

            var m = _context.Refund.FromSqlRaw("SELECT ID, Company_Code, Date, CompanyID, ContractAmount, tax, rafund, taxamount, PaymentType, Status, Sort, " +
         "Locked, Code,(Select Name from WorkerData Where Code=(Select WorkerID from [Order] where OrderNumber= ClientCode)) as WorkerID, " +
         "(Select Name from RCustomer Where Code=(Select ClientID from [Order] where OrderNumber= ClientCode)) as ClientCode, " +
         "Note FROM Refund where PaymentType = " + d);

            var pagedData = m.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
             .Take(validFilter.PageSize)
             .ToList();
            totalRecords = await m.CountAsync();
            //totalRecords = await _context.Refund.Where(m => (m.Date < d)).CountAsync();

            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Refunds>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_Refunds>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];

            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);


            var pagedData = await _context.Refund.Where(m => m.Code.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.Refund.Where(m => m.Code.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Refunds>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }
        // GET: api/Level2/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_Refunds>> Getm(Int64 id)
        {
            var m = await _context.Refund.FindAsync(id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_Refunds m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_Refunds obj = new M_Refunds();
                obj = await _context.Refund.FindAsync(m.ID);

                if (obj != null)
                {
                   // obj.ID = m.ID;
                    obj.Company_Code = m.Company_Code;
                   // obj.Code = m.Code;
                    obj.Date = m.Date;
                    obj.CompanyID = m.CompanyID;

                    obj.ContractAmount = m.ContractAmount;
                    obj.tax = m.tax;
                    obj.rafund = m.rafund;
                    obj.taxamount = m.taxamount;
                    obj.PaymentType = m.PaymentType;

                    obj.Status = m.Status;
                    obj.Sort = m.Sort;
                    obj.Locked = m.Locked;
                    obj.ClientCode = m.ClientCode;
                    obj.Note = m.Note;
                }
                // int i = this.obj.SaveChanges();
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
        public async Task<ActionResult<M_Refunds>> create(M_Refunds m)
        {
            m.Code = getNext(companycode);
            _context.Refund.Add(m);

            await _context.SaveChangesAsync();
            UpdateOrderStatus(m.ClientCode);
            return CreatedAtAction("Getm", new { id = m.ID }, m);
        }
        
        public string UpdateOrderStatus(string Code)
        {
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = @"update  [Order] set Status='1' where OrderNumber=" + Code;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(strsql, con);
            sqldr = cmd.ExecuteReader();
            if (sqldr.Read())
            {
                no = sqldr["OrderNumber"].ToString().Trim();
            }
            sqldr.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (no.Trim() == "") no = "10001";
            return no;
        }
        public string getNext(string Company_Code)
        {
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = "select  max(  cast(code as bigint)) +1  as code from Refund where    Company_Code='" + Company_Code + "' ";

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(strsql, con);
            sqldr = cmd.ExecuteReader();
            if (sqldr.Read())
            {
                no = sqldr["code"].ToString().Trim();
            }
            sqldr.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (no.Trim() == "") no = "10001";
            return no;
        }
        // DELETE: api/Level2/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_Refunds>> Deletem(Int64 id)
        {
            var m = await _context.Refund.FindAsync(id);
            if (m == null)
            {
                return NotFound();
            }
            _context.Refund.Remove(m);
            await _context.SaveChangesAsync();
            return m;
        }
    }
}
