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
using Microsoft.AspNetCore.Authorization;
using MechSuitsApi.Interfaces;
using CoreInfrastructure.ToolbarItems;
using Microsoft.AspNetCore.Routing.Matching;

namespace MechSuitsApi.Areas.toolbarController
{
    // [Authorize]
    [Route("api/Candidates")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class CandidatesController : ControllerBase
    {

        private readonly IUriService uriService;
        string companycode = ""; string user = "";
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        private IHttpContextAccessor _httpContextAccessor;
        public CandidatesController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
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
        public async Task<ActionResult<IEnumerable<M_Candidates>>> GetList()
        {
            return await _context.Candidates.ToListAsync();
        }
        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_Candidates>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var q = "SELECT ID ,Company_Code,Code,Name,RName, Passport, PlaceofIssue, " +
                "ReleaseDate,ExpirayDate,  Type, DOB,Experience,Religion, MaritalStatus, " +
                "(Select Name From professions where Code=Profession) as  Profession, WorkerAddress, " +
                " ContactNumber,Relativesname, RelativeContact, " +
                "(Select Name From Country where Code=country) as country, " +
                " (Select Name From Agents where Code=Agent) as Agent,PassportID,CV, Notes," +
                " Status, Sort, Locked  FROM  Candidates where Status=0 ";


            var m = _context.Candidates.
                FromSqlRaw(q);
            var pagedData = m
                      .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                      .Take(validFilter.PageSize)
                      .ToList();


            var totalRecords = await m.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Candidates>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }

        private string getAgent(string agent)
        {
            // m[0].Agent = "Select * from Agents where  Name='" + agent + "'";


            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = @"Select * from Agents where  Code='" + agent + "'";
            // strsql = "select  max(  cast(code as bigint)) +1  as code from Candidates where    Company_Code='" + Company_Code + "' ";
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(strsql, con);
            sqldr = cmd.ExecuteReader();
            if (sqldr.Read())
            {
                no = sqldr["Name"].ToString().Trim();
            }
            sqldr.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (no.Trim() == "") no = "name";
            return no;

            //throw new NotImplementedException();
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_Candidates>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            var route = Request.Path.Value + "?title=" + title;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.Candidates.Where(m => m.Name.Contains(title) )
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.Candidates.Where(m => m.Name.Contains(title) ).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Candidates>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }
        // GET: api/Level2/5
        [HttpGet("{Code}")]
        public async Task<ActionResult<M_Candidates>> Getm(string Code)
        {

            var m = await _context.Candidates.FindAsync(Code);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }
        //[HttpGet("{id}")]
        //public async Task<ActionResult<M_Candidates>> Getm(Int64 id)
        //{

        //    var m = await _context.Candidates.FindAsync(id);

        //    if (m == null)
        //    {
        //        return NotFound();
        //    }

        //    return m;
        //}
        [HttpGet("view/{Code}")]
        public ActionResult<M_Candidates> Getv(string Code)
        {
            var sql = "SELECT ID ,Company_Code,Code,Name,RName, Passport, PlaceofIssue, " +
                "ReleaseDate,ExpirayDate,  Type, DOB,Experience,Religion, MaritalStatus," +
                "(Select Name From professions where Code=Profession) as  Profession, WorkerAddress," +
                " ContactNumber,Relativesname, RelativeContact," +
                "(Select Name From Country where Code=country) as country, " +
                "(Select Name From Agents where Code=Agent) as Agent,PassportID,CV, " +
                "Notes, Status, Sort, Locked  FROM  Candidates where Code=" + Code;
            var m = _context.Candidates.FromSqlRaw(sql);

            if (m == null)
            {
                return NotFound();
            }

            return m.First();
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_Candidates m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_Candidates obj = new M_Candidates();
                obj = await _context.Candidates.FindAsync(m.Code);

                if (obj != null)
                {
                    obj.Name = m.Name;
                    obj.RName = m.RName;
                    obj.Passport = m.Passport;
                    obj.PlaceofIssue = m.PlaceofIssue;
                    obj.ReleaseDate = m.ReleaseDate;
                    obj.ExpirayDate = m.ExpirayDate;
                    obj.Type = m.Type;
                    obj.DOB = m.DOB;
                    obj.Experience = m.Experience;
                    obj.Religion = m.Religion;
                    obj.MaritalStatus = m.MaritalStatus;
                    obj.Profession = m.Profession;
                    obj.WorkerAddress = m.WorkerAddress;
                    obj.ContactNumber = m.ContactNumber;
                    obj.Relativesname = m.Relativesname;
                    obj.RelativeContact = m.RelativeContact;
                    obj.country = m.country;
                    obj.Agent = m.Agent;
                    obj.PassportID = m.PassportID;
                    obj.CV = m.CV;
                    obj.Notes = m.Notes;
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
        public async Task<ActionResult<M_Candidates>> create(M_Candidates m)
        {
            m.Code = getNext(companycode);
            _context.Candidates.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getm", new { Code = m.Code }, m);
        }
        public string getNext(string Company_Code)
        {
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = @"SELECT ISNULL(MAX( CAST(CODE AS BIGINT  ))  ,10000) +1  AS code FROM Candidates";
            // strsql = "select  max(  cast(code as bigint)) +1  as code from Candidates where    Company_Code='" + Company_Code + "' ";
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
        [HttpDelete("{Code}")]
        public async Task<ActionResult<M_Candidates>> Deletem(string Code)
        {
            var m = await _context.Candidates.FindAsync(Code);
            if (m == null)
            {
                return NotFound();
            }
            _context.Candidates.Remove(m);
            await _context.SaveChangesAsync();
            return m;
        }
    }
}
