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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MechSuitsApi.Interfaces;

namespace MechSuitsApi.Controllers
{
     // [Authorize]
    [Route("api/Level2")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class Level2Controller : ControllerBase
    {
        private readonly IUriService uriService;
        string companycode = "C001";   string user = "";
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        public Level2Controller(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context;
            con = c.V_connection;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
         // companycode = currentUser.FindFirst("Company").Value.ToString();    user = currentUser.FindFirst("User").Value.ToString();
        }

        // GET: api/Level2
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_Level2>>> GetAS_Acclevel2()
        {
            return await _context.AS_Acclevel2.ToListAsync();
        }
        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_Level2>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.AS_Acclevel2
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.AS_Acclevel2.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Level2>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_Level2>>> Getx([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            // return await _context.Area.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.AS_Acclevel2.Where(m => m.Name.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.AS_Acclevel2.Where(m => m.Name.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Level2>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);

        }
        [HttpGet("GetList_isusedNext")] 
        
        public List<Models.M_Level2> GetList_isusedNext()
        {
            Executer.DataProcess dp = new Executer.DataProcess();
            DataSet ds = new DataSet();
            string strSQL;
            strSQL = @" select *,(select isnull(COunt(1),0) FROM AS_Acclevel3 where level1code=a.level1code and level2code=a.code) as used from AS_Acclevel2  as a ";
            SqlCommand cmd = new SqlCommand(strSQL, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable _dt = new DataTable();
            sda.Fill(_dt);
            List<Models.M_Level2> list = null;
            if (_dt.Rows.Count > 0)
            {
                list = new List<Models.M_Level2>();
                foreach (DataRow sqldr in _dt.Rows)
                {
                    var fields = new Models.M_Level2();
                    //  fields.ID = int.Parse(sqldr["ID"].ToString());
                    fields.Code = sqldr["Code"].ToString();
                    fields.Company_Code= sqldr["Company_Code"].ToString();
                    fields.Level1Code = sqldr["Level1Code"].ToString();
                    fields.Level1Name= sqldr["Level1Name"].ToString();
                    fields.Locked =  Convert.ToBoolean( sqldr["Locked"]);
                    fields.Name= sqldr["Name"].ToString();
                    fields.Sort= Convert.ToInt32( sqldr["Sort"]);

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
        [HttpGet("{level1code}/{code}")]
        public async Task<ActionResult<M_Level2>> GetM_Level2(string level1code,string code)
        {
            Console.WriteLine("I am here to see YOU");
            var m_Level2 = await _context.AS_Acclevel2.FindAsync(companycode, level1code,code);

            if (m_Level2 == null)
            {
                return NotFound();
            }

            return m_Level2;
        }
        [HttpGet]
        [Route("GetLevel2WithLevel1")]
        public async Task<ActionResult<IEnumerable<M_Level2>>> GetLevel2WithLevel1(string id)
        {

            var m_Level2 = await _context.AS_Acclevel2.Where(x => x.Level1Code == id).ToListAsync();

            if (m_Level2 == null)
            {
                return NotFound();
            }

            return m_Level2;
        }

       
        [HttpPut]
        [Route("update")]

       
        public async Task<IActionResult> update(M_Level2 m_Level2)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_Level2 obj = new M_Level2();
                
                obj = await _context.AS_Acclevel2.FindAsync(companycode,m_Level2.Level1Code, m_Level2.Code);

                if (obj != null)
                {
                    obj.Company_Code = m_Level2.Company_Code;
                  //  obj.Level1Code = m_Level2.Level1Code;
                 //   obj.Level1Name = m_Level2.Level1Name;
                 //   obj.Code = m_Level2.Code;
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

        [HttpPut("{id}")]
        

        
        [HttpPost]
        public async Task<ActionResult<M_Level2>> create(M_Level2 m_Level2)
        {
            m_Level2.Code = (getNext(m_Level2.Level1Code));
            _context.AS_Acclevel2.Add(m_Level2);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetM_Level2", new { level1code = m_Level2.Level1Code,code=m_Level2.Code }, m_Level2);
        }
        public string getNext(string groupcode)
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = @"
 select top 1 case when len(cast(code + 1 as integer)) < 3    then  replicate('0',3-len(cast((code+1) as varchar) ))+ cast(( cast(code as integer) +1) as varchar) else
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
        [HttpDelete("{level1code}/{code}")]
        public async Task<ActionResult<M_Level2>> DeleteM_Level2(string level1code, string code)
        {
            
            var m_Level2 = await _context.AS_Acclevel2.FindAsync(companycode, level1code, code);
            if (m_Level2 == null)
            {
                return NotFound();
            }

            _context.AS_Acclevel2.Remove(m_Level2);
            await _context.SaveChangesAsync();

            return m_Level2;
        }
        [HttpGet("{level1code}/{code}/exists")]
        public long exists(string level1code, string code)
        {
            Executer.DataProcess dp = new Executer.DataProcess();
            DataSet ds = new DataSet();
            string strSQL;
            strSQL = @"   SELECT isnull(COunt(1),0) as exist  FROM AS_Acclevel3 where level1code='" + level1code + "'and level2code='"+code+"'";
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(strSQL, con);
            long count = Convert.ToInt64(cmd.ExecuteScalar());


            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            return count;
        }
    }

}
