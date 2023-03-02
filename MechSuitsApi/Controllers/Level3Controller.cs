using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreInfrastructure.Accounts.Setup;
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

namespace MechSuitsApi.Controllers
{
   //[Authorize]
    [Route("api/Level3")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class Level3Controller : ControllerBase
    {
        private readonly IUriService uriService;
        string companycode = "C001";   string user = "";
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        public Level3Controller(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context;
            con = c.V_connection;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
         //  companycode = currentUser.FindFirst("Company").Value.ToString();    user = currentUser.FindFirst("User").Value.ToString();
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_Level3>>> GetAS_Acclevel3()
        {
            return await _context.AS_Acclevel3.ToListAsync();
        }
        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<Models.M_Level3>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = pagedfunction()
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize).ToList();
                 //   .ToListAsync();
            var totalRecords = pagedfunction().Count();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Models.M_Level3>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }


        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_Level3>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            // return await _context.Area.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.AS_Acclevel3.Where(m => m.Name.Contains(title)  )
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.AS_Acclevel3.Where(m =>  m.Name.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Level3>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);

        }
        

        [HttpGet("GetList_isusedNext")]
        public List<Models.M_Level3> GetList_isusedNext()
        {
            Executer.DataProcess dp = new Executer.DataProcess();
            DataSet ds = new DataSet();
            string strSQL;
            strSQL = @" select *,(select isnull(COunt(1),0) FROM chart_of_accounts
where level1code=a.level1code and level2code=a.Level2Code and Level3Code=a.Code) as used from AS_Acclevel3  as a ";
            SqlCommand cmd = new SqlCommand(strSQL, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable _dt = new DataTable();
            sda.Fill(_dt);
            List<Models.M_Level3> list = null;
            if (_dt.Rows.Count > 0)
            {
                list = new List<Models.M_Level3>();
                foreach (DataRow sqldr in _dt.Rows)
                {
                    var fields = new Models.M_Level3();
                    //  fields.ID = int.Parse(sqldr["ID"].ToString());
                    fields.Code = sqldr["Code"].ToString();
                    fields.Company_Code = sqldr["Company_Code"].ToString();
                    fields.Level1Code = sqldr["Level1Code"].ToString();
                    fields.Level1Name = sqldr["Level1Name"].ToString();
                    fields.Level2Code = sqldr["Level2Code"].ToString();
                    fields.Level2Name = sqldr["Level2Name"].ToString();
                    fields.Locked = Convert.ToBoolean(sqldr["Locked"]);
                    fields.Name = sqldr["Name"].ToString();
                    fields.Sort = Convert.ToInt32(sqldr["Sort"]);

                    fields.used = Convert.ToInt32(sqldr["used"]);
                    list.Add(fields);
                }
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return list;
        }
        // GET: api/Level2/5
        [HttpGet("{level1code}/{level2code}/{code}")]
        public async Task<ActionResult<M_Level3>> GetM_Level3(string level1code,string level2code, string code)
        {
            
            var m = await _context.AS_Acclevel3.FindAsync(companycode, level1code,level2code, code);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }
        [HttpGet]
        [Route("GetLevel3WithLevel1n2")]
        public async Task<ActionResult<IEnumerable<M_Level3>>> GetLevel3WithLevel1n2(string l1,string l2)
        {

            var M_Level3 = await _context.AS_Acclevel3.Where(x => x.Level1Code == l1 && x.Level2Code==l2).ToListAsync();

            //  var M_Level3 = await _context.AS_Acclevel3.FindAsync().ToList();// Include(i => i.Code).FirstOrDefaultAsync(i => i.Code == id);
            //    var M_Level3= await _context.AS_Acclevel3.Where(s => s.Code.Contains(id));

            if (M_Level3 == null)
            {
                return NotFound();
            }

            return M_Level3;
        }

        // PUT: api/Level2/5

        //  [HttpPut("{id}")]
        [HttpPut]
        [Route("update")]

        //   public IHttpActionResult PutEmaployeeMaster(EmployeeDetail employee)
        public async Task<IActionResult> update(M_Level3 m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_Level3 obj = new M_Level3();
                string COMPANY_CODE = companycode;
                obj = await _context.AS_Acclevel3.FindAsync(COMPANY_CODE, m.Level1Code,m.Level2Code, m.Code);

                if (obj != null)
                {
                    //obj.Company_Code = m.Company_Code;
                    //obj.Level1Code = m.Level1Code;
                    //obj.Level1Name = m.Level1Name;
                    //obj.Code = m.Code;
                    obj.Name = m.Name;
                    obj.Locked = m.Locked;
                    obj.Sort = m.Sort;

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
        public async Task<ActionResult<M_Level3>> create(M_Level3 m)
        {
            m.Code = (getNext(m.Level1Code, m.Level2Code));
            _context.AS_Acclevel3.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetM_Level3", new { level1code = m.Level1Code,level2code=m.Level2Code, code = m.Code }, m);
        }
        public string getNext(string level1code, string level2code)
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = @" 
 select top 1 case when len(cast(code + 1 as integer)) < 4    then  replicate('0',4-len(cast((code+1) as varchar) ))+ cast(( cast(code as integer) +1) as varchar) else
  cast(( cast(code as integer) + 1) as varchar) end as code
 from AS_Acclevel3 where Level2Code = '" + level2code + "' and level1Code = '" + level1code + "'   order by id desc";
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
            if (no.Trim() == "") no = "0001";
            return no;
        }


        // DELETE: api/Level2/5
        [HttpDelete("{level1code}/{level2code}/{code}")]
        public async Task<ActionResult<M_Level3>> DeleteM_Level3(string level1code,string level2code, string code)
        {
            
            var M_Level3 = await _context.AS_Acclevel3.FindAsync(companycode, level1code,level2code, code);
            if (M_Level3 == null)
            {
                return NotFound();
            }

            _context.AS_Acclevel3.Remove(M_Level3);
            await _context.SaveChangesAsync();

            return M_Level3;
        }

        //private bool M_Level3Exists(int id)
        //{
        //    return _context.AS_Acclevel3.Any(e => e.ID == id);
        //}
        public List<Models.M_Level3> pagedfunction()
        {
            Executer.DataProcess dp = new Executer.DataProcess();
            DataSet ds = new DataSet();
            string strSQL;
            strSQL = @" select *,(select isnull(COunt(1),0) FROM chart_of_accounts
where level1code=a.level1code and level2code=a.Level2Code and Level3Code=a.Code) as used from AS_Acclevel3  as a ";
            SqlCommand cmd = new SqlCommand(strSQL, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable _dt = new DataTable();
            sda.Fill(_dt);
            List<Models.M_Level3> list = null;
            if (_dt.Rows.Count > 0)
            {
                list = new List<Models.M_Level3>();
                foreach (DataRow sqldr in _dt.Rows)
                {
                    var fields = new Models.M_Level3();
                    //  fields.ID = int.Parse(sqldr["ID"].ToString());
                    fields.Code = sqldr["Code"].ToString();
                    fields.Company_Code = sqldr["Company_Code"].ToString();
                    fields.Level1Code = sqldr["Level1Code"].ToString();
                    fields.Level1Name = sqldr["Level1Name"].ToString();
                    fields.Level2Code = sqldr["Level2Code"].ToString();
                    fields.Level2Name = sqldr["Level2Name"].ToString();
                    fields.Locked = Convert.ToBoolean(sqldr["Locked"]);
                    fields.Name = sqldr["Name"].ToString();
                    fields.Sort = Convert.ToInt32(sqldr["Sort"]);

                    fields.used = Convert.ToInt32(sqldr["used"]);
                    list.Add(fields);
                }
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return list;
        }
    }
  


}
