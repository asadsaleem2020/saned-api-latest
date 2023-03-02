using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreInfrastructure.Accounts.Transaction;
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

namespace MechSuitsApi.Areas.Accounts
{
    //    [Authorize]
    [Route("api/Opening")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class OpeningController : ControllerBase
    {
        private readonly IUriService uriService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AppDBContext _context;
        private SqlConnection con;
        string companycode = "C001"; string user = "";
        Connection c = new Connection();
        public OpeningController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            //   companycode = currentUser.FindFirst("Company").Value.ToString(); user = currentUser.FindFirst("User").Value.ToString();
            con = c.V_connection;
        }

        // GET: api/Level2
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_Opening>>> Get()
        {
            return await _context.AS_Opening_Balances.ToListAsync();
        }



        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_Opening>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.AS_Opening_Balances
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.AS_Opening_Balances.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Opening>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }


        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_Opening>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            // return await _context.Area.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.AS_Opening_Balances.Where(m => m.AccountCode.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.AS_Opening_Balances.Where(m => m.AccountCode.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Opening>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }

 
        // GET: api/Level2/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_Opening>> Get(int id)
        {
            var m = await _context.AS_Opening_Balances.FindAsync(id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }



        // PUT: api/Level2/5

        //  [HttpPut("{id}")]
        [HttpPut]
        [Route("update")]

        //   public IHttpActionResult PutEmaployeeMaster(EmployeeDetail employee)
        public async Task<IActionResult> update(M_Opening m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_Opening obj = new M_Opening();
                obj = await _context.AS_Opening_Balances.FindAsync(m.YearId);

                if (obj != null)
                {
                    obj.Company_Code = companycode;



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
        public async Task<ActionResult<M_Opening>> create(M_Opening m)
        {
            m.YearId = getNext(m.YearId);
            _context.AS_Opening_Balances.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = m.YearId }, m);
        }
        public string getNext(string groupcode)
        {
            Console.WriteLine(groupcode);
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = @"
 select top 1 case when len(cast(code + 1 as integer)) < 3  
then  replicate('0',3-len(cast((code+1) as varchar) ))+ cast(( cast(code as integer) +1) as varchar) else
  cast(( cast(code as integer) + 1) as varchar) end as code
 from AS_Acclevel2 where Level1Code = '" + groupcode + "'    order by id desc";
            //strsql = "select inull(code) + 1 as code from tblAccountsL2_L  where level1code = '" + groupcode +"'";   
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
            if (no.Trim() == "") no = "001";
            return no;
        }


        // DELETE: api/Level2/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_Opening>> Delete(string id)
        {
            var m = await _context.AS_Opening_Balances.FindAsync(companycode,id);
            if (m == null)
            {
                return NotFound();
            }

            _context.AS_Opening_Balances.Remove(m);
            await _context.SaveChangesAsync();

            return m;
        }


    }
}
