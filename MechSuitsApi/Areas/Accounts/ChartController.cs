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
  //  [Authorize]
    [Route("api/Chart")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ChartController : ControllerBase
    {
        private readonly IUriService uriService;
        string companycode = "C001";   string user = "";
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
       private IHttpContextAccessor _httpContextAccessor;
        public ChartController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context;
            con = c.V_connection;
            _httpContextAccessor = httpContextAccessor;
             var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
        //    companycode = currentUser.FindFirst("Company").Value.ToString();
        //    user = currentUser.FindFirst("User").Value.ToString();
            // companycode = "C001";
            // user = "Tester";

        }
        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_Chart>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.Chart_of_Accounts
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.Chart_of_Accounts.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Chart>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("page/search")]
        public async Task<ActionResult<IEnumerable<M_Chart>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            // return await _context.Area.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.Chart_of_Accounts.Where(m => m.Name.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.Chart_of_Accounts.Where(m => m.Name.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Chart>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }


        // GET: api/Level2
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_Chart>>> GetChart_of_Accounts()
        {
            return await _context.Chart_of_Accounts.ToListAsync();
        }


        [HttpGet("{type}/{category}/GetAccount")]
        public async Task<ActionResult<IEnumerable<M_Chart>>> GetAccount(string type, string category)
        {
            string where = "1=1";
            
            if (type == "Main" && category == "Expense")
            {
                return await _context.Chart_of_Accounts.Where(m => m.Level1Name == category).Take(10).ToListAsync();
            }
            else if (type == "MainAsset" && category == "BA")
            {
                return await _context.Chart_of_Accounts.Where(m => m.Level1Name == "Asset" && m.AccountCategory == category).Take(10).ToListAsync();
            }
            else if (type == "Sub" && category == "Expense")
            {
                return await _context.Chart_of_Accounts.Where(m => m.AccountCategory == "CA" || m.AccountCategory == "BA").Take(10).ToListAsync();
            }
            else if (type == "SubAsset" && category == "BA")
            {
                return await _context.Chart_of_Accounts.Where(m => m.AccountCategory == "CU" || m.AccountCategory == "VE").Take(10).ToListAsync();
            }
            else if (type == "MainAsset" && category == "CA")
            {
                return await _context.Chart_of_Accounts.Where(m => m.Level1Name == "Asset" && m.AccountCategory == category).Take(10).ToListAsync();
            }
            else
            {
                return await _context.Chart_of_Accounts.Where(m => m.AccountCategory == "CU" || m.AccountCategory == "VE").Take(10).ToListAsync();
            }

            //return await _context.Chart_of_Accounts.Where(m => m.AccountCategory == "CU" || m.AccountCategory == "VE").Take(10).ToListAsync();
        }


            [HttpGet("{type}/{category}/Search")]
        public async Task<ActionResult<IEnumerable<M_Chart>>> GetSearch(string type,string category)
        {
            string where = "1=1";
            string title = HttpContext.Request.Query["title"];


            if (type == "Main" && category =="Expense")
            {
                return await _context.Chart_of_Accounts
            .Where(m => m.Level1Name == category
                         && m.Name.Contains(title)).Take(10).ToListAsync();
            }
           else if (type == "MainAsset" && category == "BA")
            {
                return await _context.Chart_of_Accounts
            .Where(m => m.Level1Name == "Asset" && m.AccountCategory == category
                         && m.Name.Contains(title)).Take(10).ToListAsync();
            }
           else if (type == "MainAsset" && category == "CA")
            {
                return await _context.Chart_of_Accounts
            .Where(m => m.Level1Name == "Asset" && m.AccountCategory == category
                         && m.Name.Contains(title)).Take(10).ToListAsync();
            }
          else  if (type == "Sub" && category == "Expense")
            {
                return await _context.Chart_of_Accounts
            .Where(m => (m.AccountCategory == "CA" || m.AccountCategory == "BA")
                         && m.Name.Contains(title)).Take(10).ToListAsync();
            }
           else if (type == "SubAsset" && category == "BA")
            {
                return await _context.Chart_of_Accounts
            .Where(m => (m.AccountCategory == "CU" || m.AccountCategory == "VE")
                         && m.Name.Contains(title)).Take(10).ToListAsync();
            }

            
            else
            {
                return await _context.Chart_of_Accounts
            .Where(m => (m.AccountCategory == "CU" || m.AccountCategory == "VE")
                         && m.Name.Contains(title)).Take(10).ToListAsync();
            }
            



        }


        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_Chart>>> Get1()
        {
            string where = "1=1";
            string title = HttpContext.Request.Query["title"];
                return await _context.Chart_of_Accounts
            .Where(m =>  m.Name.Contains(title)).Take(10).ToListAsync();

        }

        // GET: api/Level2/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_Chart>> GetM_Chart(Int64 id)
        {
            var M_Chart = await _context.Chart_of_Accounts.FindAsync(id);

            if (M_Chart == null)
            {
                return NotFound();
            }

            return M_Chart;
        }
        [HttpGet]
        [Route("GetLevel2WithLevel1")]
        public async Task<ActionResult<IEnumerable<M_Chart>>> GetLevel2WithLevel1(string id)
        {

            var M_Chart = await _context.Chart_of_Accounts.Where(x => x.Level1Code == id).ToListAsync();

            //  var M_Chart = await _context.Chart_of_Accounts.FindAsync().ToList();// Include(i => i.Code).FirstOrDefaultAsync(i => i.Code == id);
            //    var M_Chart= await _context.Chart_of_Accounts.Where(s => s.Code.Contains(id));

            if (M_Chart == null)
            {
                return NotFound();
            }

            return M_Chart;
        }

      
        [HttpPut]
        [Route("update")]        
        public async Task<IActionResult> update(M_Chart M_Chart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_Chart obj = new M_Chart();
                obj = await _context.Chart_of_Accounts.FindAsync(M_Chart.ID);

                if (obj != null)
                {
                   // obj.Company_Code = M_Chart.Company_Code;
                    obj.Level1Code = M_Chart.Level1Code;
                    obj.Level1Name = M_Chart.Level1Name;
                    obj.Level2Code = M_Chart.Level2Code;
                    obj.Level2Name = M_Chart.Level2Name;
                    obj.Level3Code = M_Chart.Level3Code;
                   obj.Level3Name = M_Chart.Level3Name;
                 //   obj.Code = M_Chart.Code;
                    obj.Name = M_Chart.Name;
                   obj.AccountType = M_Chart.AccountType;
                   obj.AccountCategory = M_Chart.AccountCategory;
                    obj.RegisterType = M_Chart.RegisterType;
                    obj.Locked = M_Chart.Locked;
                    obj.ADDRESS = M_Chart.ADDRESS;
                    obj.CITY = M_Chart.CITY;
                    obj.COUNTRY = M_Chart.COUNTRY;


                    obj.NTN = M_Chart.NTN;
                    obj.CNIC = M_Chart.CNIC;
                    obj.SALETAXNO = M_Chart.SALETAXNO;
                    obj.TEL1 = M_Chart.TEL1;
                    obj.TEL2 = M_Chart.TEL2;
                    obj.EMAIL = M_Chart.EMAIL;
                    obj.FAXNO = M_Chart.FAXNO;
                    obj.WEBSITE = M_Chart.WEBSITE;
                    obj.Remarks = M_Chart.Remarks;
                    obj.Zone = M_Chart.Zone;
                    obj.Officer = M_Chart.Officer;
                    obj.CONTACT_PERSON = M_Chart.CONTACT_PERSON;
                    obj.CREDITLIMIT = M_Chart.CREDITLIMIT;






                }
                // int i = this.obj.SaveChanges();
                _context.Entry(obj).State = EntityState.Modified;

                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {
                throw;
            }
            return Ok(M_Chart);
        }

        
      
        [HttpPost]
        public async Task<ActionResult<M_Chart>> create(M_Chart M_Chart)
        {
            M_Chart.Code = (getNext(companycode, M_Chart.Level1Code, M_Chart.Level2Code, M_Chart.Level3Code));
            _context.Chart_of_Accounts.Add(M_Chart);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetM_Chart", new { id = M_Chart.ID }, M_Chart);
        }
        public string getNext(string Company_Code, string level1, string level2, string level3)
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = "select  max(  cast(code as bigint)) +1  as code from Chart_of_Accounts where level1code='" + level1 + "' and level2code='" + level2 + "' and level3Code='" + level3 + "' and Company_Code='" + Company_Code + "' ";

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
            if (no.Trim() == "") no = level1.Trim() + level2.Trim() + level3.Trim() + "10001";
            return no;
        }


        // DELETE: api/Level2/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_Chart>> DeleteM_Chart(Int64 id)
        {
            var M_Chart = await _context.Chart_of_Accounts.FindAsync(id);
            if (M_Chart == null)
            {
                return NotFound();
            }

            _context.Chart_of_Accounts.Remove(M_Chart);
            await _context.SaveChangesAsync();

            return M_Chart;
        }

        
    }
}
