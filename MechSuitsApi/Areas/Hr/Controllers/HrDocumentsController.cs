using CoreInfrastructure.Hr.Setup;
using Executer;
using MechSuitsApi.Classes;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MechSuitsApi.Interfaces;

namespace MechSuitsApi.Areas.Hr.Controllers
{
    [Route("api/HrDocuments")]
    [ApiController]
    public class HrDocumentsController : ControllerBase
    {

        private readonly IUriService uriService;
        private readonly AppDBContext _context;
        private SqlConnection con;
        string companycode = ""; string user = "";
        private IHttpContextAccessor _httpContextAccessor;
        Connection c = new Connection();
        public HrDocumentsController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context;
            con = c.V_connection;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;


            //  companycode = currentUser.FindFirst("Company").Value.ToString();
            Console.WriteLine(companycode);
            // user = currentUser.FindFirst("User").Value.ToString();
            Console.WriteLine(user);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_HrDocuments>>> GetAS_Acclevel2()
        {
            return await _context.HrDocuments.ToListAsync();
        }
        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_HrDocuments>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.HrDocuments
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.HrDocuments.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_HrDocuments>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_HrDocuments>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];

            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);


            var pagedData = await _context.HrDocuments.Where(m => m.Code.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.HrDocuments.Where(m => m.Code.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_HrDocuments>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }

        [HttpGet("{id}")]
        public async Task<ActionResult<M_HrDocuments>> GetM_Level2(string id)
        {
            var m_Level2 = await _context.HrDocuments.FindAsync(id);

            if (m_Level2 == null)
            {
                return NotFound();
            }

            return m_Level2;
        }



        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_HrDocuments m_Level2)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var obj = new M_HrDocuments();
                obj = await _context.HrDocuments.FindAsync(m_Level2.Code);

                if (obj != null)
                {

                    obj.DocumentType = m_Level2.DocumentType;
                    obj.IssueDate = m_Level2.IssueDate;
                    obj.ExpiryDate = m_Level2.ExpiryDate;

                    obj.File = m_Level2.File;
                    obj.Status = m_Level2.Status;
                    obj.IsActive = m_Level2.IsActive;
                    obj.IsDeleted = m_Level2.IsDeleted;
                    obj.Locked = m_Level2.Locked;


                }

                _context.Entry(obj).State = EntityState.Modified;

                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {
                throw;
            }
            return Ok(m_Level2);
        }


        [HttpPost]
        public async Task<ActionResult<M_HrDocuments>> create(M_HrDocuments m_Level2)
        {
            m_Level2.Code = getUpdateMasterCount();
            _context.HrDocuments.Add(m_Level2);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetM_Level2", new { id = m_Level2.Code }, m_Level2);
        }
        public string getUpdateMasterCount()
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            string no = "";

            strsql = @"SELECT ISNULL(MAX( CAST(Code AS BIGINT  ))  ,10000) +1  AS Code FROM HrDocuments";

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            sqldr = dp.ExecuteReader(con, strsql);
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

        [HttpDelete("{id}")]
        public async Task<ActionResult<M_HrDocuments>> DeleteM_Level2(string id)
        {
            var m_Level2 = await _context.HrDocuments.FindAsync(id);
            if (m_Level2 == null)
            {
                return NotFound();
            }
            _context.HrDocuments.Remove(m_Level2);
            await _context.SaveChangesAsync();

            return m_Level2;
        }


    }
}
