using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreInfrastructure.Accounts.Transaction.ShiftClose;
using MechSuitsApi.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using System.Data.SqlClient;
using System.Data;
using Executer;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using CoreInfrastructure.Generals;
using MechSuitsApi.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MechSuitsApi.Areas.Accounts
{
  //  [Authorize]
    [Route("api/shiftclose")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ShiftClose : ControllerBase
    {
        private readonly IUriService uriService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AppDBContext _context;
        private SqlConnection con;
        string companycode = ""; string user = "";
        Connection c = new Connection();
        D_DB dbset = null;
        GeneralFunctions general = null;
        public ShiftClose(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            dbset = new D_DB(c.V_connection);
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
       //     companycode = currentUser.FindFirst("Company").Value.ToString(); user = currentUser.FindFirst("User").Value.ToString();
            con = c.V_connection;
        }

        // GET: api/Level2
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_Header>>> Get()
        {
            return await _context.ShiftClose.ToListAsync();
        }
        [HttpGet]
        [Route("UNPOSTED/chunks")]
        public async Task<ActionResult<IEnumerable<M_Header>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.ShiftClose.Where(m => m.Checked == "0")
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.ShiftClose.Where(m => m.Checked == "0").CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Header>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_Header>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            // return await _context.Area.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.ShiftClose.Where(m => (m.VoucherId.Contains(title) && m.Checked == "0"))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.ShiftClose.Where(m => (m.VoucherId.Contains(title) && m.Checked == "0")).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Header>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);

        }
        [HttpGet]
        [Route("POSTED/chunks")]
        public async Task<ActionResult<IEnumerable<M_Header>>> Get2([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.ShiftClose.Where(m => m.Checked == "1")
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.ShiftClose.Where(m => m.Checked == "1").CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Header>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("posted/search")]
        public async Task<ActionResult<IEnumerable<M_Header>>> Get6([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            // return await _context.Area.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.ShiftClose.Where(m => (m.VoucherId.Contains(title) && m.Checked == "1"))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.ShiftClose.Where(m => (m.VoucherId.Contains(title) && m.Checked == "1")).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Header>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);

        }







        // GET: api/Level2/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_Header>> Get(string id)
        {
            var m = await _context.ShiftClose.FindAsync(companycode,id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }



        // PUT: api/Level2/5

        //  [HttpPut("{id}")]
        [HttpPut]
        //   public IHttpActionResult PutEmaployeeMaster(EmployeeDetail employee)
        public async Task<IActionResult> Put(M_Header m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_Header obj = new M_Header();
                obj = await _context.ShiftClose.FindAsync(companycode, m.VoucherId);

                if (obj != null)
                {

                    obj.CounterNumber = m.CounterNumber;
                    obj.DocumentDate = m.DocumentDate;
                    obj.Payee = m.Payee;
                    obj.Remarks = m.Remarks;
                    obj.Total = m.Total;
                    


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
        public async Task<ActionResult<M_Header>> create(M_Header m)
        {
            string year, month, yearmonth;
            year = DateTime.Parse(m.DocumentDate.ToString()).Year.ToString();
            month = DateTime.Parse(m.DocumentDate.ToString()).Month.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            year = year.Substring(2, 2);
            yearmonth = year + month;
            m.VoucherId = dbset.getUpdateMasterCount(m.VoucherType, yearmonth, m);
            m.Receiver = user;
            m.CREATED_BY = user;
            _context.ShiftClose.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = m.VoucherId }, m);
        }
     

        // DELETE: api/Level2/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_Header>> Delete(string id)
        {
            var m = await _context.ShiftClose.FindAsync(id);
            if (m == null)
            {
                return NotFound();
            }

            _context.ShiftClose.Remove(m);
            await _context.SaveChangesAsync();

            return m;
        }
        [HttpGet("{purpose}/GetLists")]
        public async Task<ActionResult<IEnumerable<M_Header>>> GetLists(string purpose)
        {
            string where = "1=1";

            return await _context.ShiftClose.Where(m => m.Checked == purpose).ToListAsync();


        }
        [HttpGet("{purpose}/postunpost")]
        public string postunpost(string purpose)
        {

            string[] ls_val = purpose.Split(',');
            string ls_purpose = ls_val[0].ToString().Trim();
            string ls_voucherid = ls_val[1].ToString().Trim();

            string ls_vouchertype = ls_val[2].ToString().Trim();
            string ls_value = ls_val[3].ToString().Trim();
            string WhereCondition = "Company_Code='" + companycode + "' and VoucherId='" + ls_voucherid.Trim() + "' and voucherType='" + ls_vouchertype.Trim() + "'";
            //if (ls_val[2].Trim() == "Checked")
            //{
            general = new GeneralFunctions(c.V_connection);
            general.UpdateTable("ShiftClose", "Checked", ls_value, WhereCondition);
            //general = new GeneralFunctions(c.V_connection);
            //general.UpdateTable("GeneralJournal", "Checked", "1", WhereCondition);
            return "Done";
           
            
        }


    }
}
