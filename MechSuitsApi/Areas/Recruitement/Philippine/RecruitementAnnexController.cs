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
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MechSuitsApi.Interfaces;
using Microsoft.CodeAnalysis;
using CoreInfrastructure.Recruitement.Philippine;
using CoreInfrastructure.Recruitement;
using static ServiceStack.Diagnostics.Events;

namespace MechSuitsApi.Areas.Recruitement
{// [Authorize]
    [Route("api/RecruitementAnnex")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class RecruitementAnnexController : ControllerBase
    {
        private readonly IUriService uriService;
        string companycode = ""; string user = "";
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        private IHttpContextAccessor _httpContextAccessor;
        public RecruitementAnnexController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
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
        public async Task<ActionResult<IEnumerable<M_RecruitementAnnex>>> GetList()
        {
            return await _context.RecruitementAnnex.ToListAsync();
        }

        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_RecruitementAnnex>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
           
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var querySQL = "SELECT Code, CompanyCode, OrderID, Date, Cleaning, Washing, BabySitting, ElderCare, Cooking, Tasks, Notes, Status, (SELECT WorkerName  FROM RecruitementOrder where OrderNumber = OrderID) as Locked ,(SELECT Name FROM RCustomer where Code = (SELECT Client FROM RecruitementOrder where OrderNumber = OrderID)) as Sort FROM RecruitementAnnex";
            var m = _context.RecruitementAnnex.FromSqlRaw(querySQL).ToList();
            var pagedData = m.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();
            var totalRecords = await _context.RecruitementAnnex.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_RecruitementAnnex>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_RecruitementAnnex>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];

            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);


            var pagedData = await _context.RecruitementAnnex.Where(m => m.Code.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.RecruitementAnnex.Where(m => m.Code.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_RecruitementAnnex>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }

        // GET: api/Level2/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_RecruitementAnnex>> Getm(string id)
        {
            var m = await _context.RecruitementAnnex.FindAsync(id);
            if (m == null)
            {
                return NotFound();
            }
            return m;
        }
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_RecruitementAnnex m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                M_RecruitementAnnex obj = new M_RecruitementAnnex();
                obj = await _context.RecruitementAnnex.FindAsync(m.Code);
                if (obj == null)
                {
                    return BadRequest(ModelState);
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
        public async Task<ActionResult<M_RecruitementAnnex>> create (M_RecruitementAnnex m)
        {
            m.Code = getNext();
           // m.Date = DateTime.Now.ToString();
            _context.RecruitementAnnex.Add(m);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Getm", new { id = m.Code }, m);
        }
        public string getNext()
        {
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = "select max(cast (Code as bigint)) +1 as Code from RecruitementAnnex";

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
        public async Task<ActionResult<M_RecruitementAnnex>> Deletem(Int64 id)
        {
            var m = await _context.RecruitementAnnex.FindAsync(id);
            if (m == null)
            {
                return NotFound();
            }
            _context.RecruitementAnnex.Remove(m);
            await _context.SaveChangesAsync();
            return m;
        }
    }
}
