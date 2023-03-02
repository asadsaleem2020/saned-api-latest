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
using CoreInfrastructure;
using CoreInfrastructure.ToolbarItems;
using CoreInfrastructure.Recruitement;

namespace MechSuitsApi.Areas.toolbarController
{
    [Route("api/EmployementProblems")]
    [ApiController]
    public class EmployementProblemsController : ControllerBase
    {

        private readonly IUriService uriService;
        string companycode = "C001"; string user = "";
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        private IHttpContextAccessor _httpContextAccessor;
        public EmployementProblemsController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
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
        public async Task<ActionResult<IEnumerable<M_EmployementProblems>>> GetList()
        {
            return await _context.EmployementProblems.ToListAsync();
        }
        [HttpGet]
        [Route("chunks/{type}")]
        public async Task<ActionResult<IEnumerable<M_EmployementProblems>>> Get1(string type, [FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = new List<M_EmployementProblems>();
            int totalRecords;
            if (type == "active")
            {
               var m = _context.EmployementProblems.FromSqlRaw("SELECT ID, Company_Code, OrderNumber,Date, (Select Name from APP_USERS where Code= EmployeeID) as EmployeeID, (Select Name from RCustomer where Code=client) as client,ProblemDetails, Photo,Status ,Sort  ,Locked ,(Select Name From Candidates where Code=Factor) as Factor FROM EmployementProblems Where [Status]='0'");
                pagedData = m.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();
               totalRecords = await m.CountAsync();
            }else if(type == "completed")
            {
                var m = _context.EmployementProblems.
               FromSqlRaw("SELECT  ID,Company_Code,OrderNumber,Date," +
               "(Select Name from APP_USERS where Code= EmployeeID) as EmployeeID," +
               "(Select Name from RCustomer where Code=client) as client,ProblemDetails," +
               "Photo,Status ,Sort  ,Locked ,(Select Name From Candidates where Code=Factor) as Factor FROM EmployementProblems Where [Status]='1'");pagedData = m.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();totalRecords = await m.CountAsync();
            }else
            {
                var m = _context.EmployementProblems.
              FromSqlRaw("SELECT  ID,Company_Code,OrderNumber,Date," +
              "(Select Name from APP_USERS where Code= EmployeeID) as EmployeeID," +
              "(Select Name from RCustomer where Code=client) as client,ProblemDetails," +
              "Photo,Status ,Sort  ,Locked ,(Select Name From Candidates where Code=Factor) as Factor FROM EmployementProblems");
                pagedData = m.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
                //totalRecords = await _context.RecruitementOrder.Where(m => (m.Date < d) && (m.ApplicationStatus == "2")).CountAsync();
                totalRecords = await m.CountAsync();
            }

            var pagedReponse = PaginationHelper.CreatePagedReponse<M_EmployementProblems>(pagedData, validFilter, totalRecords, uriService, route);
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
            strsql = "SELECT Distinct (SELECT COUNT(*) FROM [EmployementProblems]) AS [all], (SELECT COUNT(*) FROM [EmployementProblems] Where [Status]='0') AS [active],(SELECT COUNT(*) FROM [EmployementProblems] Where [Status]='1') AS [completed] FROM EmployementProblems";
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
                    kvpList.Add("active", sqldr["active"].ToString());
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
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_EmployementProblems>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];

            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);


            var pagedData = await _context.EmployementProblems.Where(m => m.OrderNumber.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.EmployementProblems.Where(m => m.OrderNumber.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_EmployementProblems>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }
        // GET: api/Level2/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_EmployementProblems>> Getm(Int64 id)
        {
            var m = await _context.EmployementProblems.FindAsync(id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_EmployementProblems m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_EmployementProblems obj = new M_EmployementProblems();
                obj = await _context.EmployementProblems.FindAsync(m.OrderNumber);

                if (obj != null)
                {
                   // obj.Name = m.Name;
                    obj.Locked = m.Locked;


                    //obj.EMAIL = m.EMAIL;
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
        public async Task<ActionResult<M_EmployementProblems>> create(M_EmployementProblems m)
        {
            m.OrderNumber = getNext(companycode);
            _context.EmployementProblems.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getm", new { id = m.ID }, m);
        }
        public string getNext(string Company_Code)
        {
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = "select  max(  cast(OrderNumber as bigint)) +1  as code from EmployementProblems where    Company_Code='" + Company_Code + "' ";

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
        public async Task<ActionResult<M_EmployementProblems>> Deletem(Int64 id)
        {
            var m = await _context.EmployementProblems.FindAsync(id);
            if (m == null)
            {
                return NotFound();
            }
            _context.EmployementProblems.Remove(m);
            await _context.SaveChangesAsync();
            return m;
        }

    }
}
