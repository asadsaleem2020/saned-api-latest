using CoreInfrastructure.GeneralSetting;
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
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using MechSuitsApi.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MechSuitsApi.Areas.GeneralSetting.Controllers 
{
    [Authorize]
    [Route("api/producttype")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ProductTypeController : ControllerBase
    {
        private readonly IUriService uriService;
        private readonly AppDBContext _context;
        private SqlConnection con;
        string companycode = ""; string user = "";
        private IHttpContextAccessor _httpContextAccessor;
        Connection c = new Connection();
        public ProductTypeController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;

            _context = context;
            con = c.V_connection;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
           companycode = currentUser.FindFirst("Company").Value.ToString(); user = currentUser.FindFirst("User").Value.ToString();
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_ProductType>>> GetAS_Acclevel2()
        {
            return await _context.ProductType.ToListAsync();
        }
        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_ProductType>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.ProductType
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.ProductType.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_ProductType>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_ProductType>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            // return await _context.Area.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.ProductType.Where(m => m.Name.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.ProductType.Where(m => m.Name.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_ProductType>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_ProductType>> GetM_Level2(string id)
        {
            var m_Level2 = await _context.ProductType.FindAsync(id);

            if (m_Level2 == null)
            {
                return NotFound();
            }

            return m_Level2;
        }
        [HttpGet]
        [Route("GetLevel2WithLevel1")]
        public async Task<ActionResult<IEnumerable<M_ProductType>>> GetLevel2WithLevel1(string id)
        {

            var m_Level2 = await _context.ProductType.Where(x => x.Code == id).ToListAsync();

            if (m_Level2 == null)
            {
                return NotFound();
            }

            return m_Level2;
        }

        // PUT: api/Departments/5

        //  [HttpPut("{id}")]
        [HttpPut]
        [Route("update")]

        //   public IHttpActionResult PutEmaployeeMaster(EmployeeDetail employee)
        public async Task<IActionResult> update(M_ProductType m_Level2)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var obj = new M_ProductType();
                obj = await _context.ProductType.FindAsync(m_Level2.Code);

                if (obj != null)
                {
                    //obj.COMPANY_CODE = m_Level2.COMPANY_CODE;
                    //  obj.Code = m_Level2.Code;
                    obj.Name = m_Level2.Name;
                    obj.Locked = m_Level2.Locked;
                    obj.Sort = m_Level2.Sort;

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
        public async Task<ActionResult<M_ProductType>> create(M_ProductType m_Level2)
        {
            m_Level2.Code = getUpdateMasterCount();
            _context.ProductType.Add(m_Level2);
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
            strsql = @"SELECT ISNULL(MAX( CAST(CODE AS BIGINT  ))  ,0) +1  AS code FROM ProductType";

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
        public async Task<ActionResult<M_ProductType>> DeleteM_Level2(string id)
        {
            var m_Level2 = await _context.ProductType.FindAsync(id);
            if (m_Level2 == null)
            {
                return NotFound();
            }

            _context.ProductType.Remove(m_Level2);
            await _context.SaveChangesAsync();

            return m_Level2;
        }

        //private bool M_Level2Exists(int id)
        //{
        //    return _context.AS_Acclevel2.Any(e => e.ID == id);
        //}
    }
}
