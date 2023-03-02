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
using CoreInfrastructure.Customers;
using CoreInfrastructure.GeneralSetting.City;
using CoreInfrastructure.ToolbarItems;

namespace MechSuitsApi.Areas.AccomodationSystem
{
    // [Authorize]
    [Route("api/WorkersData")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class WorkerDataController : ControllerBase
    {

        private readonly IUriService uriService;
        string companycode = "C001"; string user = "";
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        private IHttpContextAccessor _httpContextAccessor;
        public WorkerDataController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
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
        public async Task<ActionResult<IEnumerable<M_WorkerData>>> GetList()
        {
            return await _context.WorkerData.Where(m=>m.Status=="0").ToListAsync();
        }
        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_WorkerData>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            //SELECT  Company_Code       ,Code       ,Name      ,RName       ,Passport       ,PlaceofIssue       ,type       ,DOB       ,Experience       ,Religion       ,MaritalStatus       ,Country       ,(Select Name from professions where Code= Profession)as   Profession       ,Salary       ,address       ,Mobile       ,RelativeName       ,RelativeContact       ,DateReceived       ,arrivalStation       ,WorkStatus       ,ProfilePhoto       ,Proxy       ,sponsor       ,ResidenceNO       ,ResidenceExpiry       ,AdditonInfo       ,Status       ,Sort       ,Locked       ,ID   FROM   WorkerData 
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var m = _context.WorkerData.
          FromSqlRaw("SELECT  Company_Code       ,Code       ,Name      ,RName       ,Passport       ,PlaceofIssue       " +
          ",type       ,DOB       ,Experience       ,Religion       ,MaritalStatus       ,(Select Name from Country where Code= Country) as   Country       ," +
          "(Select Name from professions where Code= Profession)as   Profession       ,Salary       ," +
          "address       ,Mobile       ," +          "RelativeName       ,RelativeContact       ," +
          "DateReceived       ,arrivalStation       ,(Select Name from WorkerStatus where Code= WorkStatus) as   WorkStatus       ," +          "ProfilePhoto       ," +
          "Proxy       ,sponsor       ,ResidenceNO       ,ResidenceExpiry       ,AdditonInfo       ," +
          "Status       ,Sort       ,Locked       ,ID   FROM   WorkerData where status=0  \r\n").ToList();

            var pagedData = m


             
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList ();
            var totalRecords = await _context.WorkerData.Where(m => m.Status == "0").CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_WorkerData>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_WorkerData>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];

            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);


            var pagedData = await _context.WorkerData.Where(m => m.Name.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.WorkerData.Where(m => m.Name.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_WorkerData>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }
        // GET: api/Level2/5
        // GET: api/VendorType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_WorkerData>> Getm(Int64 id)
        {

            var m = await _context.WorkerData.FindAsync(id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }
        [HttpGet("view/{id}")]
        public ActionResult<M_WorkerData> Getv(Int64 id)
        {
            var sql = "SELECT  Company_Code       ,Code       ,Name      ,RName       ,Passport       ,PlaceofIssue       " +
          ",type       ,DOB       ,Experience       ,Religion       ,MaritalStatus       ,(Select Name from Country where Code= Country) as   Country       ," +
          "(Select Name from professions where Code= Profession)as   Profession       ,Salary       ," +
          "address       ,Mobile       ," + "RelativeName       ,RelativeContact       ," +
          "DateReceived       ,(Select Name from Arrivals where Code= arrivalStation) as arrivalStation       ,(Select Name from WorkerStatus where Code= WorkStatus) as   WorkStatus       ," + "ProfilePhoto       ," +
          "(Select Name from Agents where Code = Proxy) as Proxy       ,(Select Name from RCustomer where Code = sponsor) as sponsor       ,ResidenceNO       ,ResidenceExpiry       ,AdditonInfo       ," +
          "Status       ,Sort       ,Locked       ,ID   FROM   WorkerData where ID=" + id;
            var m = _context.WorkerData.FromSqlRaw(sql);

            if (m == null)
            {
                return NotFound();
            }

            return m.First();
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_WorkerData m)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_WorkerData obj = new M_WorkerData();
                obj = await _context.WorkerData.FindAsync(m.ID);
                Console.WriteLine("I AM HERE");
                if (obj != null)
                {
                    Console.WriteLine("I AM HERE");
                    // obj.ID=m.ID;
                    obj.Company_Code = m.Company_Code;
                    // obj.Code = m.Code;
                    obj.Name = m.Name;
                    obj.RName = m.RName;
                    obj.Passport = m.Passport;
                    obj.PlaceofIssue = m.PlaceofIssue;
                    obj.type=m.type;
                    obj.DOB = m.DOB;
                    obj.Experience= m.Experience;
                    obj.Religion=m.Religion;
                    obj.MaritalStatus = m.MaritalStatus;
                    obj.Country = m.Country;
                    obj.Profession = m.Profession;
                    obj.Salary = m.Salary;
                    obj.address = m.address;
                    obj.Mobile = m.Mobile;
                    obj.RelativeName = m.RelativeName;
                    obj.RelativeContact = m.RelativeContact;
                    obj.DateReceived = m.DateReceived;
                    obj.arrivalStation = m.arrivalStation;
                    obj.WorkStatus = m.WorkStatus;
                    obj.ProfilePhoto = m.ProfilePhoto;
                    obj.Proxy = m.Proxy;
                    obj.sponsor = m.sponsor;
                    obj.ResidenceNO = m.ResidenceNO;
                    obj.ResidenceExpiry=m.ResidenceExpiry;
                    obj.AdditonInfo = m.AdditonInfo;
                    obj.Status = m.Status;
                    obj.Sort = m.Sort;
                    obj.Locked = m.Locked;

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
        public async Task<ActionResult<M_WorkerData>> create(M_WorkerData m)
        {

            m.Code = getNext(companycode);
            m.Status = "0";
            _context.WorkerData.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getm", new { id = m.ID }, m);
        }
        public string getNext(string Company_Code)
        {
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = "select max(cast(code as bigint)) +1  as code from WorkerData where Company_Code='" + Company_Code + "' ";

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
            if (no.Trim() == "") no = "20001";
            return no;
        }
        // DELETE: api/Level2/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_WorkerData>> Deletem(Int64 id)
        {
            var m = await _context.WorkerData.FindAsync(id);
            if (m == null)
            {
                return NotFound();
            }
            _context.WorkerData.Remove(m);
            await _context.SaveChangesAsync();
            return m;
        }
    }
}
