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
using CoreInfrastructure.Recruitement;

namespace MechSuitsApi.Areas.Recruitement
{  // [Authorize]
    [Route("api/RecruitmentRefund")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class RecruitmentRefundController : ControllerBase
    {
        private readonly IUriService uriService;
        string companycode = "C001"; string user = "";
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        private IHttpContextAccessor _httpContextAccessor;
        public RecruitmentRefundController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
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
        public async Task<ActionResult<IEnumerable<M_RecruitmentRefund>>> GetList()
        {
            return await _context.RecruitmentRefund.ToListAsync();
        }
        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_RecruitmentRefund>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var m = _context.RecruitmentRefund.
           FromSqlRaw("SELECT Company_Code        ,OrderNumber        ,ContractCost        ,Fine        ,DiscountBear        ,DiscountAmount        ,rafund        ,HijriDate        ,Date       , PaymentType       , Notes        ,Status       , Sort       , Locked       , ID        ,(Select Name from RCustomer where Code= Demand)as  Demand   FROM  RecruitmentRefund ").ToList();

            var pagedData = m


      
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList ();
            var totalRecords = await _context.RecruitmentRefund.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_RecruitmentRefund>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("topCardCounter")]
        public List<Dictionary<string, string>> GetMx()
        {
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "SELECT Distinct (SELECT COUNT(*) FROM [RecruitementOrder]) AS Order1, (SELECT COUNT(*) FROM [RecruitmentPackages]) AS Package, (SELECT COUNT(*) FROM [RecruitementOrder] where ApplicationStatus = '2') AS Agency, (SELECT COUNT(*) FROM [RecruitmentPackages]) AS Completed, (SELECT COUNT(*) FROM [RecruitementOrder] where ApplicationStatus = '2') AS Alternate FROM RecruitementOrder";
            SqlCommand cmd = new SqlCommand(strsql, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable _dt = new DataTable();
            sda.Fill(_dt);
            //List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            //List<string> fields = new List<string>();
            //List<KeyValuePair<string, string>> kvpList = new List<KeyValuePair<string, string>>();            
            Dictionary<string, string> kvpList = new Dictionary<string, string>();
            List<Dictionary<string, string>> l = new List<Dictionary<string, string>>();
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow sqldr in _dt.Rows)
                {
                    kvpList.Add("Order", sqldr["Order1"].ToString());
                    kvpList.Add("Package", sqldr["Package"].ToString());
                    kvpList.Add("Agency", sqldr["Agency"].ToString());
                    kvpList.Add("Completed", sqldr["Completed"].ToString());
                    kvpList.Add("Alternate", sqldr["Alternate"].ToString());
                    l.Add(kvpList);
                }
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return l;
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_RecruitmentRefund>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];

            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);


            var pagedData = await _context.RecruitmentRefund.Where(m => m.OrderNumber.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.RecruitmentRefund.Where(m => m.OrderNumber.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_RecruitmentRefund>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }
        // GET: api/Level2/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_RecruitmentRefund>> Getm(Int64 id)
        {
            var m = await _context.RecruitmentRefund.FindAsync(id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_RecruitmentRefund m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_RecruitmentRefund obj = new M_RecruitmentRefund();
                obj = await _context.RecruitmentRefund.FindAsync(m.OrderNumber);

                if (obj != null)
                {
                    // obj.Name = m.Name;
                    obj.Locked = m.Locked;


                    //   obj.EMAIL = m.EMAIL;
                    obj.Sort = m.Sort;
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
        public async Task<ActionResult<M_RecruitmentRefund>> create(M_RecruitmentRefund m)
        {
            m.OrderNumber = getNext(companycode);
            _context.RecruitmentRefund.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getm", new { id = m.OrderNumber }, m);
        }
        public string getNext(string Company_Code)
        {
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = "select  max(  cast(OrderNumber as bigint)) +1  as code from RecruitmentRefund where    Company_Code='" + Company_Code + "' ";

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
        public async Task<ActionResult<M_RecruitmentRefund>> Deletem(Int64 id)
        {
            var m = await _context.RecruitmentRefund.FindAsync(id);
            if (m == null)
            {
                return NotFound();
            }
            _context.RecruitmentRefund.Remove(m);
            await _context.SaveChangesAsync();
            return m;
        }
    }
}
