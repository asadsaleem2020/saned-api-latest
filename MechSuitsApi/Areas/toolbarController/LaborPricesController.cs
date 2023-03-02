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
using MechSuitsApi.Interfaces;
using CoreInfrastructure.ToolbarItems;
using CoreInfrastructure.AccomodationSystem;
using CoreInfrastructure.GeneralSetting.Chat;

namespace MechSuitsApi.Areas.toolbarController
{
    [Route("api/laborprices")]
    // [Authorize]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class LaborPricesController : ControllerBase
    {

        private readonly IUriService uriService;
        string companycode = "C001"; string user = "";
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        private IHttpContextAccessor _httpContextAccessor;
        public LaborPricesController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
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
        public async Task<ActionResult<IEnumerable<M_LaborPrices>>> GetList()
        {
            // return await _context.Order.Where(m => m.Status == "0").ToListAsync();
            return _context.LaborPrices.FromSqlRaw("SELECT ID, Company_Code, Code, Agent, Occupation, Religion, Experience, PriceDollar, " +
                "PriceRiyal, ContratctPeriod, LeaveInDays, SalaryInRiyal, ArrivalDuration, Notes, Status, Sort, Locked FROM  [LaborPrices] Where Status=0").ToList();


        }
        [HttpGet]
        [Route("{agentid}/chunks")]
        public async Task<ActionResult<IEnumerable<M_LaborPrices>>> Get1(String agentid,[FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);

            var m = _context.LaborPrices.
          FromSqlRaw("SELECT ID, Company_Code, Code, (Select Name from Agents where ID = Agent) as Agent, " +
                "(Select Name from professions where Code = Occupation) as Occupation, Religion, Experience, PriceDollar, " +
                   "PriceRiyal, ContratctPeriod, LeaveInDays, SalaryInRiyal, ArrivalDuration, Notes, Status, " +
                   "Sort, Locked FROM  [LaborPrices] Where Status=0 AND Agent = " + agentid).ToList();
            var pagedData = m



                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
            var totalRecords = await _context.LaborPrices.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_LaborPrices>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        //[HttpGet]
        //[Route("search")]
        //public async Task<ActionResult<IEnumerable<M_LaborPrices>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        //{

        //    string title = HttpContext.Request.Query["title"];

        //    var route = Request.Path.Value + "?title=" + title;

        //    var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);


        //    var pagedData = await _context.LaborPrices.Where(m => m.Code.Contains(title))
        //            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
        //            .Take(validFilter.PageSize)
        //            .ToListAsync();
        //    var totalRecords = await _context.LaborPrices.Where(m => m.Code.Contains(title)).CountAsync();
        //    var pagedReponse = PaginationHelper.CreatePagedReponse<M_LaborPrices>(pagedData, validFilter, totalRecords, uriService, route);
        //    return Ok(pagedReponse);


        //}
        // GET: api/Level2/5
        [HttpGet("{agentid}")]
        public ActionResult<IEnumerable<M_LaborPrices>> Getm(String agentid)
        {
            Console.WriteLine(agentid);
            var sql = "SELECT ID, Company_Code, Code, (Select Name from Agents where ID = Agent) as Agent, " +
                "(Select Name from professions where Code = Occupation) as Occupation, Religion, Experience, PriceDollar, " +
                   "PriceRiyal, ContratctPeriod, LeaveInDays, SalaryInRiyal, ArrivalDuration, Notes, Status, " +
                   "Sort, Locked FROM  [LaborPrices] Where Agent = '"+ agentid + "'"; //ORDER BY CONVERT(DATETIME,SendingTime) DESC

            var m = _context.LaborPrices.FromSqlRaw(sql).ToList();
            if (m == null)
            {
                // return NotFound();
            }

            return m;
        }

        [HttpGet("{agentID}/view/{id}")]
        public ActionResult<IEnumerable<M_LaborPrices>> GetView(string agentID, string id)
        {
            if (agentID != null && agentID != "" && id != null && id != "")
            {
                var sql = "SELECT ID, Company_Code, Code, Agent,Occupation, Religion, Experience, PriceDollar, " +
                   "PriceRiyal, ContratctPeriod, LeaveInDays, SalaryInRiyal, ArrivalDuration, Notes, Status, " +
                   "Sort, Locked FROM  [LaborPrices] Where Agent=" + agentID + " AND ID=" + id;

                var m = _context.LaborPrices.FromSqlRaw(sql).ToList();

                if (m == null)
                {
                    // return NotFound();
                }

                return m;
            }
            return null;
            
        }
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_LaborPrices m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_LaborPrices obj = new M_LaborPrices();
                obj = await _context.LaborPrices.FindAsync(m.Code);

                if (obj != null)
                {
                    obj.Company_Code = m.Company_Code;
                    obj.Code = m.Code;
                    obj.Agent = m.Agent;
                    obj.Occupation = m.Occupation;
                    obj.Religion = m.Religion;
                    obj.Experience = m.Experience;
                    obj.PriceDollar = m.PriceDollar;
                    obj.PriceRiyal = m.PriceRiyal;
                    obj.ContratctPeriod = m.ContratctPeriod;
                    obj.LeaveInDays = m.LeaveInDays;
                    obj.SalaryInRiyal = m.SalaryInRiyal;
                    obj.ArrivalDuration = m.ArrivalDuration;
                    obj.Notes = m.Notes;
                    obj.Status = m.Status;
                    obj.Sort = m.Sort;
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
        public async Task<ActionResult<M_LaborPrices>> create(M_LaborPrices m)
        {
            Console.WriteLine("Hello creating Record For LaborPrice");
            m.Code = getNext(companycode);
            Console.WriteLine(m.Code);

            _context.LaborPrices.Add(m);

            await _context.SaveChangesAsync();
            return CreatedAtAction("Getm", new { agentid = m.Agent }, m);
        }
        public string getNext(string Company_Code)
        {
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = @"SELECT ISNULL(MAX( CAST(Code as bigint)),0) +1  as Code from [LaborPrices]";

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(strsql, con);
            sqldr = cmd.ExecuteReader();
            if (sqldr.Read())
            {
                no = sqldr["Code"].ToString().Trim();
            }
            sqldr.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (no.Trim() == "") no = "1";
            return no;
        }


    }
}
