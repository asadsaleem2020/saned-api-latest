using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreInfrastructure.ItemInformation.ItemOpening;
using MechSuitsApi.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using System.Data.SqlClient;
using System.Data;
using Executer;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MechSuitsApi.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MechSuitsApi.Areas.Items.Controllers
{
    [Authorize]
    [Route("api/itemopening")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ItemOpeningController : ControllerBase
    {
        private readonly IUriService uriService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AppDBContext _context;
        private SqlConnection con;
        string companycode = "";   string user = "";
        Connection c = new Connection();
        public ItemOpeningController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context;
            con = c.V_connection;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
          companycode = currentUser.FindFirst("Company").Value.ToString();    user = currentUser.FindFirst("User").Value.ToString();
        }

        // GET: api/Level2
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_itemOpening>>> Get()
        {
            return await _context.ITEMOPENING.ToListAsync();
        }


        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_itemOpening>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.ITEMOPENING
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.ITEMOPENING.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_itemOpening>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }


        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_itemOpening>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            // return await _context.Area.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.ITEMOPENING.Where(m => m.ITEMNAME.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.ITEMOPENING.Where(m => m.ITEMNAME.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_itemOpening>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }


        // GET: api/Level2/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_itemOpening>> Get(string id)
        {

            var m = await _context.ITEMOPENING.FindAsync(companycode, id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }

        [HttpGet]
        [Route("GetLevel2WithLevel1")]
        public async Task<ActionResult<IEnumerable<M_itemOpening>>> GetLevel2WithLevel1(string id)
        {

            var m = await _context.ITEMOPENING.Where(x => x.Code == id).ToListAsync();

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }


        [HttpPut]
        [Route("update")]

        //   public IHttpActionResult PutEmaployeeMaster(EmployeeDetail employee)
        public async Task<IActionResult> update(M_itemOpening m)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                M_itemOpening obj = new M_itemOpening();
                obj = await _context.ITEMOPENING.FindAsync(m.Code);

                if (obj != null)
                {
                    //obj.COMPANY_CODE = m.COMPANY_CODE;
                   // obj.Code = m.Code;
                    obj.ITEMCODE = m.ITEMCODE;
                    obj.ITEMNAME = m.ITEMNAME;
                    obj.QTY = m.QTY;
                    obj.RATE = m.RATE;

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
        public async Task<ActionResult<M_itemOpening>> create(M_itemOpening m)
        {
            m.Code = getUpdateMasterCount();
            _context.ITEMOPENING.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = m.ITEMCODE }, m);
        }

        public string getUpdateMasterCount()
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            //MAX( CAST(CODE AS BIGINT  ))
            strsql = @"SELECT ISNULL(MAX( CAST(Code AS BIGINT  ))  ,0) +1  AS code FROM ITEMOPENING";

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
        

        // DELETE: api/Level2/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_itemOpening>> Delete(string id)
        {
            
            var m = await _context.ITEMOPENING.FindAsync(companycode ,id);
            if (m == null)
            {
                return NotFound();
            }

            _context.ITEMOPENING.Remove(m);
            await _context.SaveChangesAsync();

            return m;
        }



    }
}
