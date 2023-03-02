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
using MechSuitsApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Graph;
using CoreInfrastructure.GeneralSetting.City;
using System.Reflection.Emit;

namespace MechSuitsApi.Areas.Hr.Controllers
{
  //  [Authorize]
    [Route("api/staff_advance")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class AdvanceController : Controller
    {
        private readonly IUriService uriService;
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        string companycode = "";   string user = "";
        private IHttpContextAccessor _httpContextAccessor;
        public AdvanceController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context;
            con = c.V_connection;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
        //    companycode = currentUser.FindFirst("Company").Value.ToString(); user = currentUser.FindFirst("User").Value.ToString();
        }
       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_Advance>>> GetA()
        {
            return await _context.HR_ADVANCE.ToListAsync();
        }

        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_Advance>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var m = _context.HR_ADVANCE.
 FromSqlRaw("SELECT ID, COMPANY_CODE, CODE,MONTHS,PAYMENTMETHOD, (Select Name From HR_EMPLOYEE where Code=EMPLOYEE) as EMPLOYEE, AMOUNT, CREATED_ON, CREATED_BY, APPROVED_ON," +
                " APPROVED_BY, DELETED_ON, PAYMENTMETHODDELETED_BY, NOTES, STATUS, SORT, LOCKED FROM HR_ADVANCE").ToList();
            var pagedData = m
                   .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                   .Take(validFilter.PageSize)
                   .ToList();
            var totalRecords = await _context.HR_ADVANCE.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Advance>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }


        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_Advance>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            // return await _context.Area.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.HR_ADVANCE.Where(m => m.EMPLOYEE.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.HR_ADVANCE.Where(m => m.EMPLOYEE.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Advance>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }

        [HttpGet("data")]
        public List<M_Advance> GetMx()
        {
            //var sql = "SELECT ID, COMPANY_CODE, CODE,EMPLOYEE, AMOUNT, CREATED_ON, CREATED_BY, APPROVED_ON," +
            //    " APPROVED_BY, DELETED_ON, DELETED_BY, NOTES, STATUS, SORT, LOCKED FROM HR_ADVANCE";

           // sql =   var m = _context.HR_ADVANCE.FromSqlRaw(sql).ToList();

            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            // strsql = "SELECT    Adv.EMPLOYEE,  cast(Sum(CAST(Adv.AMOUNT AS INT)) AS varchar(max)) as AMOUNT  , SORT=''  FROM [dbo].[HR_EMPLOYEE] as EMP LEFT JOIN HR_ADVANCE as Adv ON EMP.Code = Adv.EMPLOYEE  GROUP BY Adv.EMPLOYEE";
            strsql = "SELECT EMPLOYEE,(Select Name From HR_EMPLOYEE WHERE Code=EMPLOYEE) as EMPLOYEE_NAME,cast(Sum(CAST(AMOUNT AS INT)) AS varchar) as AMOUNT  , CAST(count(*) AS varchar) as SORT FROM HR_ADVANCE WHERE EMPLOYEE is not null GROUP BY EMPLOYEE";
            SqlCommand cmd = new SqlCommand(strsql, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable _dt = new DataTable();
            sda.Fill(_dt);
            List<M_Advance> list = null;
            if (_dt.Rows.Count > 0)
            {
                list = new List<M_Advance>();
                foreach (DataRow sqldr in _dt.Rows)
                {
                    var fields = new M_Advance();
                    // fields.ID = int.Parse(sqldr["ID"].ToString());
                    fields.EMPLOYEE = sqldr["EMPLOYEE"].ToString();
                    fields.LOCKED = sqldr["EMPLOYEE_NAME"].ToString();
                    fields.AMOUNT = sqldr["AMOUNT"].ToString();
                    fields.SORT =  sqldr["SORT"].ToString();

                    list.Add(fields);

                }
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return list;
        }
        //// GET: api/Departments/5
        //[HttpGet("{empCode}")]
        //public Task<List<M_Advance>> GetM(String empCode)
        //{
        //    var sql = "SELECT ID, COMPANY_CODE, CODE, (Select Name From HR_EMPLOYEE where Code=EMPLOYEE) as EMPLOYEE, AMOUNT, CREATED_ON, CREATED_BY, APPROVED_ON," +
        //        " APPROVED_BY, DELETED_ON, DELETED_BY, NOTES, STATUS, SORT, LOCKED FROM HR_ADVANCE where EMPLOYEE=" + empCode;
        //    var m = _context.HR_ADVANCE.FromSqlRaw(sql).ToListAsync();

        //    if (m == null)
        //    {
        //        //return NotFound();
        //    }

        //    return m;
        //}

        [HttpPut]
        [Route("update")]

        //   public IHttpActionResult PutEmaployeeMaster(EmployeeDetail employee)
        public async Task<IActionResult> update(M_Advance m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var obj = new M_Advance();
                obj = await _context.HR_ADVANCE.FindAsync(m.CODE);

                if (obj != null)
                {
                    //obj.ID = m.ID;
                    obj.COMPANY_CODE = m.COMPANY_CODE;
                    obj.CODE = m.CODE;
                    obj.EMPLOYEE = m.EMPLOYEE;
                    obj.AMOUNT = m.AMOUNT;
                    obj.CREATED_ON = m.CREATED_ON;
                    obj.CREATED_BY = m.CREATED_BY;
                    obj.APPROVED_ON = m.APPROVED_ON;
                    obj.APPROVED_BY = m.APPROVED_BY;
                    obj.DELETED_ON = m.DELETED_ON;
                    obj.DELETED_BY = m.DELETED_BY;
                    obj.NOTES = m.NOTES;
                    obj.STATUS = m.STATUS;
                    obj.SORT = m.SORT;
                    obj.LOCKED = m.LOCKED;


                }
                //int i = this.obj.SaveChanges();
                _context.Entry(obj).State = EntityState.Modified;

                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {
                throw;
            }
            return Ok(m);
        }
        [HttpGet("view/{id}")]
        public Task<List<M_Advance>> GettingRecordbyEmpID(String id)
        {
            var sql = "SELECT ID, COMPANY_CODE, CODE,MONTHS,PAYMENTMETHOD, (Select Name From HR_EMPLOYEE where Code=EMPLOYEE) as EMPLOYEE, AMOUNT, CREATED_ON, CREATED_BY, APPROVED_ON," +
                " APPROVED_BY, DELETED_ON, DELETED_BY, NOTES, STATUS, SORT, LOCKED FROM HR_ADVANCE where EMPLOYEE=" + id;
            var m = _context.HR_ADVANCE.FromSqlRaw(sql).ToListAsync();

            if (m == null)
            {
                //return NotFound();
            }

            return m;
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutM_Level2(string id, M_Level2 m_Level2)
        //{
        //    if (id != m_Level2.Code)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(m_Level2).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        //if (!M_Level2Exists(id))
        //        //{
        //        //    return NotFound();
        //        //}
        //        //else
        //        //{
        //        //    throw;
        //        //}
        //    }

        //    return NoContent();
        //}

        // POST: api/Departments
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<M_Advance>> create(M_Advance m)
        {
            //   Console.WriteLine(m_Level2.ROLE_ID + "is ROLE ID");
            m.CODE = getUpdateMasterCount();
            m.ID = int.Parse(m.CODE);
            _context.HR_ADVANCE.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GettingRecordbyEmpID", new { id = m.EMPLOYEE }, m);
        }
        public string getUpdateMasterCount()
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            //MAX( CAST(CODE AS BIGINT  ))
            strsql = @"SELECT ISNULL(MAX(CAST(CODE AS BIGINT)),0) +1  AS code FROM HR_ADVANCE";

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            sqldr = dp.ExecuteReader(con, strsql);
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
        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_Advance>> DeleteM_Level2(string id)
        {
            var m_Level2 = await _context.HR_ADVANCE.FindAsync(id);
            if (m_Level2 == null)
            {
                return NotFound();
            }

            _context.HR_ADVANCE.Remove(m_Level2);
            await _context.SaveChangesAsync();

            return m_Level2;
        }

        //private bool M_Level2Exists(int id)
        //{
        //    return _context.AS_Acclevel2.Any(e => e.ID == id);
        //}
    }
}