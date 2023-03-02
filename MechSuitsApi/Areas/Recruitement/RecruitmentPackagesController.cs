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
using System.Runtime.Remoting;

namespace MechSuitsApi.Areas.Recruitement
{ 
    // [Authorize]
    [Route("api/RecruitmentPackages")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class RecruitmentPackagesController : ControllerBase
    {
        private readonly IUriService uriService;
        string companycode = "C001"; string user = "";
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        private IHttpContextAccessor _httpContextAccessor;
        public RecruitmentPackagesController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
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
        public async Task<ActionResult<IEnumerable<M_RecruitmentPackages>>> GetList()
        {
            return await _context.RecruitmentPackages.ToListAsync();
        }
        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_RecruitmentPackages>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var m = _context.RecruitmentPackages.
         FromSqlRaw("SELECT ID  ,Company_Code  ,Code , (Select Name from Country where Code= Country)as Country  ,Name ,RName ,(Select Name from professions where Code= ProfessionID)as    ProfessionID  ,Value  ,duration    ,Status ,Sort  ,Locked   FROM    RecruitmentPackages ").ToList();

            var pagedData = m
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();
            var totalRecords = await _context.RecruitmentPackages.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_RecruitmentPackages>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_RecruitmentPackages>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];

            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);


            var pagedData = await _context.RecruitmentPackages.Where(m => m.Code.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.RecruitmentPackages.Where(m => m.Code.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_RecruitmentPackages>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }
        // GET: api/Level2/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_RecruitmentPackages>> Getm(Int64 id)
        {
            var m = await _context.RecruitmentPackages.FindAsync(id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_RecruitmentPackages m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_RecruitmentPackages obj = new M_RecruitmentPackages();
                obj = await _context.RecruitmentPackages.FindAsync(m.ID);

                if (obj != null)
                {
                    obj.Company_Code = m.Company_Code;
                    obj.Country = m.Country;
                    obj.Name = m.Name;
                    obj.RName = m.RName;
                    obj.ProfessionID=m.ProfessionID;
                    obj.Value = m.Value;
                    obj.duration = m.duration;
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
        public async Task<ActionResult<M_RecruitmentPackages>> create(M_RecruitmentPackages m)
        {
            m.Code = getNext(companycode);
            _context.RecruitmentPackages.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getm", new { id = m.ID }, m);
        }
        public string getNext(string Company_Code)
        {
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = "select  max(  cast(Code as bigint)) +1  as code from RecruitmentPackages";

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
        public async Task<ActionResult<M_RecruitmentPackages>> Deletem(Int64 id)
        {
            var m = await _context.RecruitmentPackages.FindAsync(id);
            if (m == null)
            {
                return NotFound();
            }
            _context.RecruitmentPackages.Remove(m);
            await _context.SaveChangesAsync();
            return m;
        }
    }
}
