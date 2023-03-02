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
using Microsoft.Graph;

namespace MechSuitsApi.Areas.Hr.Controllers
{
    [Route("api/HrEmployers")]
    [ApiController]
    public class HrEmployersController : ControllerBase
    {
        private readonly IUriService uriService;
        private readonly AppDBContext _context;
        private SqlConnection con;
        private IHttpContextAccessor _httpContextAccessor;
        Connection c = new Connection();
        string companycode = ""; string user = "";
        public HrEmployersController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;

            _context = context;
            con = c.V_connection;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            //companycode = currentUser.FindFirst("Company").Value.ToString();    user = currentUser.FindFirst("User").Value.ToString();
        }
        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_HrEmployers>>> GetAS_Acclevel2()
        {
            return await _context.HrEmployers.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<M_HrEmployers>> GetM_Level2(string id)
        {
            var m_Level2 = await _context.HrEmployers.FindAsync(id);

            if (m_Level2 == null)
            {
                return NotFound();
            }

            return m_Level2;
        }

        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_HrEmployers>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.HrEmployers
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.HrEmployers.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_HrEmployers>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_HrEmployers>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            // return await _context.Area.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.HrEmployers.Where(m => m.Code.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.HrEmployers.Where(m => m.Code.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_HrEmployers>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);

        }



        [HttpPut]
        [Route("update")]

        //   public IHttpActionResult PutEmaployeeMaster(EmployeeDetail employee)
        public async Task<IActionResult> update(M_HrEmployers m_Level2)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var obj = new M_HrEmployers();
                obj = await _context.HrEmployers.FindAsync(m_Level2.Code);

                if (obj != null)
                {
                    //obj.COMPANY_CODE = m_Level2.COMPANY_CODE;
                    //  obj.Code = m_Level2.Code;
                    obj.Longitude = m_Level2.Longitude;
                    obj.Lattitude = m_Level2.Lattitude ?? "";
                    obj.Status = obj.Status;
                    obj.Areainkm = m_Level2.Areainkm;
                    obj.Description = m_Level2.Description;
                    obj.DateModified = DateTime.Now.ToString();
                    obj.Permissions = m_Level2.Permissions;
                    obj.BusinessAddress = m_Level2.BusinessAddress;

                }
                // int i = this.obj.SaveChanges();
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
        public async Task<ActionResult<M_HrEmployers>> create(M_HrEmployers m_Level2)
        {
            m_Level2.Code = getUpdateMasterCount();
            m_Level2.DateAdded = DateTime.Now.ToString();
            _context.HrEmployers.Add(m_Level2);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetM_Level2", new { id = m_Level2.Code }, m_Level2);
        }
        public string getUpdateMasterCount()
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            //MAX( CAST(CODE AS BIGINT  ))
            strsql = @"SELECT ISNULL(MAX( CAST(Code AS BIGINT  ))  ,10000) +1  AS Code FROM HrEmployers";

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
        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_HrEmployers>> DeleteM_Level2(string id)
        {
            var m_Level2 = await _context.HrEmployers.FindAsync(id);
            if (m_Level2 == null)
            {
                return NotFound();
            }

            _context.HrEmployers.Remove(m_Level2);
            await _context.SaveChangesAsync();

            return m_Level2;
        }

    }
}