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
using CoreInfrastructure.SpnoserShip;

namespace MechSuitsApi.Areas.sponsership
{  // [Authorize]
    [Route("api/SponsorshipTransferRefund")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class SponsorshipTransferRefundController : ControllerBase
    {
        private readonly IUriService uriService;
        string companycode = "C001"; string user = "";
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        private IHttpContextAccessor _httpContextAccessor;
        public SponsorshipTransferRefundController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
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
        public async Task<ActionResult<IEnumerable<M_SponsorshipTransferRefund>>> GetList()
        {
            return await _context.SponsershipTransferRefund.ToListAsync();
        }
        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_SponsorshipTransferRefund>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.SponsershipTransferRefund
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.SponsershipTransferRefund.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_SponsorshipTransferRefund>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_SponsorshipTransferRefund>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];

            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);


            var pagedData = await _context.SponsershipTransferRefund.Where(m => m.Code.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.SponsershipTransferRefund.Where(m => m.Code.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_SponsorshipTransferRefund>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }
        [HttpGet]
        [Route("topCardCounter")]
        public List<Dictionary<string, string>> topCardCounter()
        {
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "SELECT Distinct (SELECT COUNT(*) FROM [SponsershipTransferRequest]) AS [all], (SELECT COUNT(*) FROM [SponsershipTransferRefund]) AS [refund], (SELECT COUNT(*) FROM [SponsershipTransferRequest] Where [Status]='2') AS [completed] FROM [SponsershipTransferRequest]";
            SqlCommand cmd = new SqlCommand(strsql, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable _dt = new DataTable();
            sda.Fill(_dt);
            Dictionary<string, string> kvpList = new Dictionary<string, string>();
            List<Dictionary<string, string>> l = new List<Dictionary<string, string>>();
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow sqldr in _dt.Rows)
                {
                    kvpList.Add("all", sqldr["all"].ToString());
                    kvpList.Add("refund", sqldr["refund"].ToString());
                    kvpList.Add("completed", sqldr["completed"].ToString());
                    l.Add(kvpList);
                }
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return l;
        }
        // GET: api/Level2/5
        [HttpGet("{Code}")]
        public async Task<ActionResult<M_SponsorshipTransferRefund>> Getm(string Code)
        {
            var m = await _context.SponsershipTransferRefund.FindAsync(Code);
            if (m == null)
            {
                return NotFound();
            }
            return m;
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_SponsorshipTransferRefund m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                M_SponsorshipTransferRefund obj = new M_SponsorshipTransferRefund();
                obj = await _context.SponsershipTransferRefund.FindAsync(m.Code);

                if (obj != null)
                {
                    obj.Locked = m.Locked;
                    obj.Sort = m.Sort;
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
        public async Task<ActionResult<M_SponsorshipTransferRefund>> create(M_SponsorshipTransferRefund data)
        {
            data.Code = getNext(companycode);

            _context.SponsershipTransferRefund.Add(data);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getm", new { Code = data.Code }, data);
        }
        public string getNext(string Company_Code)
        {
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = "select max(cast(Code as bigint)) +1 as Code from SponsershipTransferRefund where Company_Code='" + Company_Code + "' ";

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
            if (no.Trim() == "") no = "1";
            return no;
        }
        // DELETE: api/Level2/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_SponsorshipTransferRefund>> Deletem(Int64 id)
        {
            var m = await _context.SponsershipTransferRefund.FindAsync(id);
            if (m == null)
            {
                return NotFound();
            }
            _context.SponsershipTransferRefund.Remove(m);
            await _context.SaveChangesAsync();
            return m;
        }
    }
}
