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


using CoreInfrastructure.VisaRequest;
namespace MechSuitsApi.Areas
{
    [Route("api/VisaRequest")]
    [ApiController]
    public class VisaRequestController : ControllerBase
    {

        private readonly IUriService uriService;
        string companyCODE = "C001"; string user = "";
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        private IHttpContextAccessor _httpContextAccessor;
        public VisaRequestController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context;
            con = c.V_connection;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            //   companyCODE = currentUser.FindFirst("Company").Value.ToString();
            //   user = currentUser.FindFirst("User").Value.ToString();
        }

        // GET: api/Level2
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<M_VisaRequest>>> GetList()
        //{
        //    return await _context.VisaRequest.ToListAsync();
        //}
        public async Task<dynamic> GetList()
        {
            return await _context.VisaRequest.ToListAsync();

            //var query = from visa in _context.VisaRequest
            //            select new
            //            {
            //                visa.COMPANY_CODE,
            //                visa.CODE,
            //                CLIENT =   _context.RCustomer.FirstOrDefault(c => c.Code == visa.CLIENT).Name,
            //                visa.DATE,
            //                COUNTRY = _context.Country.FirstOrDefault(c => c.Code == visa.COUNTRY).Name,
            //                visa.RELIGION,
            //                visa.PROFESSION,
            //                visa.GENDER,
            //                visa.JOB,
            //                visa.WORK,
            //                visa.SALARY,
            //                visa.MOBILE,
            //                visa.APPLICATION_STATUS,
            //                visa.GOVT_FEE,
            //                visa.COMPANY_FEE,
            //                visa.VISA_PAID_STATUS,
            //                visa.FAMILIY_ID,
            //                visa.PHOTO,
            //                visa.BANK_STATEMENT,
            //                visa.NOTES
            //            };
            //var result = await  query.ToListAsync();
            //return result;
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
            strsql = "SELECT Distinct (SELECT COUNT(*) FROM RCustomer) AS Customer,   (SELECT COUNT(*) FROM VisaRequest where VisaStatus=0) AS Resquests, (SELECT COUNT(*) FROM VisaRequest where VisaStatus=1) AS Completed ";
            //strsql = "SELECT Distinct (SELECT COUNT(*) FROM RCustomer) AS Customer," +
            //    "(SELECT COUNT(*) FROM VisaRequest where VisaStatus=0) AS Resquests," +
            //    "(SELECT COUNT(*) FROM [RecruitementOrder] where ApplicationStatus = '2') " +
            //    "AS Agency,(SELECT COUNT(*) FROM [RecruitmentPackages]) AS Completed,(SELECT COUNT(*) FROM [RecruitementOrder] where ApplicationStatus = '2') AS Alternate FROM RecruitementOrder";
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
                    kvpList.Add("Customer", sqldr["Customer"].ToString());
                    kvpList.Add("Resquests", sqldr["Resquests"].ToString());
                    kvpList.Add("Completed", sqldr["Completed"].ToString());
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
        [Route("chunks/{status}")]
        public async Task<ActionResult<IEnumerable<M_VisaRequest>>> Get1(String Status, [FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var query = "SELECT COMPANY_CODE,CODE,(Select Name From RCustomer where Code=CLIENT ) as CLIENT , DATE,(Select Name From Country where Code=COUNTRY)as COUNTRY, RELIGION, PROFESSION, GENDER, JOB , WORK , SALARY , MOBILE , APPLICATION_STATUS , GOVT_FEE , COMPANY_FEE , VISA_PAID_STATUS , FAMILIY_ID , PHOTO , BANK_STATEMENT, NOTES ,VisaStatus,Status,Sort,Locked FROM VisaRequest where VisaStatus=" + Status;

            var pagedData = await _context.VisaRequest.FromSqlRaw(query)
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.VisaRequest.FromSqlRaw(query).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_VisaRequest>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_VisaRequest>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];

            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);


            var pagedData = await _context.VisaRequest.Where(m => m.CODE.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.VisaRequest.Where(m => m.CODE.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_VisaRequest>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }
        // GET: api/Level2/5
        [HttpGet("{CODE}")]
        public async Task<ActionResult<M_VisaRequest>> Getm(string CODE)
        {
            var m = await _context.VisaRequest.FindAsync(CODE);
            if (m == null)
            {
                return NotFound();
            }
            return m;
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_VisaRequest m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                M_VisaRequest obj = new M_VisaRequest();
                obj = await _context.VisaRequest.FindAsync(m.CODE);

                if (obj != null)
                {
                    obj.COMPANY_CODE = m.COMPANY_CODE;
                    obj.CODE = m.CODE;
                    obj.CLIENT = m.CLIENT;
                    obj.DATE = m.DATE;
                    obj.COUNTRY = m.COUNTRY;
                    obj.RELIGION = m.RELIGION;
                    obj.PROFESSION = m.PROFESSION;
                    obj.GENDER = m.GENDER;
                    obj.JOB = m.JOB;
                    obj.WORK = m.WORK;
                    obj.SALARY = m.SALARY;
                    obj.MOBILE = m.MOBILE;
                    obj.APPLICATION_STATUS = m.APPLICATION_STATUS;
                    obj.GOVT_FEE = m.GOVT_FEE;
                    obj.COMPANY_FEE = m.COMPANY_FEE;
                    obj.VISA_PAID_STATUS = m.VISA_PAID_STATUS;
                    obj.FAMILIY_ID = m.FAMILIY_ID;
                    obj.PHOTO = m.PHOTO;
                    obj.BANK_STATEMENT = m.BANK_STATEMENT;
                    obj.NOTES = m.NOTES;

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
        public async Task<ActionResult<M_VisaRequest>> create(M_VisaRequest data)
        {
            data.CODE = getNext(companyCODE);

            _context.VisaRequest.Add(data);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getm", new { CODE = data.CODE }, data);
        }
        public string getNext(string Company_CODE)
        {
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = "select max(cast(CODE as bigint)) +1 as CODE from VisaRequest  ";

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(strsql, con);
            sqldr = cmd.ExecuteReader();
            if (sqldr.Read())
            {
                no = sqldr["CODE"].ToString().Trim();
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
        public async Task<ActionResult<M_VisaRequest>> Deletem(Int64 id)
        {
            var m = await _context.VisaRequest.FindAsync(id);
            if (m == null)
            {
                return NotFound();
            }
            _context.VisaRequest.Remove(m);
            await _context.SaveChangesAsync();
            return m;
        }
    }
}