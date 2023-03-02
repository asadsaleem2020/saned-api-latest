using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MechSuitsApi.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using CoreInfrastructure.ItemInformation.ItemSubCategory;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MechSuitsApi.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MechSuitsApi.Areas.Items.Controllers
{
    [Authorize]
    [Route("api/itemsubcategory")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ItemSubCategoryController : ControllerBase
    {
        private readonly IUriService uriService; 
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AppDBContext _context;
        private SqlConnection con;
        D_DB dbset = null;
        string companycode = "";   string user = "";
        Connection c = new Connection();
        public ItemSubCategoryController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context;
            con = c.V_connection;
            dbset = new D_DB(c.V_connection);
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
         companycode = currentUser.FindFirst("Company").Value.ToString();    user = currentUser.FindFirst("User").Value.ToString();
        }



        [HttpGet]
        [Route("top10")]

        public async Task<ActionResult<IEnumerable<M_SubCategory>>> Get2(string id)
        {
            // string title = HttpContext.Request.Query["title"];
            string strsql;
            strsql = "select Top(10) * from ItemCategory";
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlDataAdapter sda = new SqlDataAdapter(strsql, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            List<M_SubCategory> list = null;
            list = new List<M_SubCategory>();

            if (dt.Rows.Count > 0)
            {

                foreach (DataRow sqldr in dt.Rows)
                {
                    var m = new M_SubCategory();
                    //  m.ID = int.Parse(sqldr["ID"].ToString());
                    m.COMPANY_CODE = sqldr["COMPANY_CODE"].ToString();
                    m.Code = sqldr["Code"].ToString();
                    m.Name = sqldr["Name"].ToString();
                    m.Locked = bool.Parse(sqldr["Locked"].ToString());
                    m.Sort = int.Parse(sqldr["Sort"].ToString());
                    list.Add(m);

                }
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return list;

        }

        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_SubCategory>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.Item_SubCategory.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.Item_SubCategory.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_SubCategory>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_SubCategory>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            // return await _context.Area.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.Item_SubCategory.Where(m => m.Name.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.Item_SubCategory.Where(m => m.Name.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_SubCategory>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }






        //[HttpGet]
        //[Route("search")]
        //public async Task<ActionResult<IEnumerable<M_SubCategory>>> Get1(string id)
        //{
        //    string title = HttpContext.Request.Query["title"];
        //    string strsql;
        //    strsql = "select Top(10) * from ItemCategory where  Name Like '%" + title + "%' ";
        //    if (con.State == ConnectionState.Closed)
        //    {
        //        con.Open();
        //    }
        //    SqlDataAdapter sda = new SqlDataAdapter(strsql, con);
        //    DataTable dt = new DataTable();
        //    sda.Fill(dt);
        //    List<M_SubCategory> list = null;
        //    list = new List<M_SubCategory>();

        //    if (dt.Rows.Count > 0)
        //    {

        //        foreach (DataRow sqldr in dt.Rows)
        //        {
        //            var m = new M_SubCategory();
        //            //  m.ID = int.Parse(sqldr["ID"].ToString());
        //            m.COMPANY_CODE = sqldr["COMPANY_CODE"].ToString();
        //            m.Code = sqldr["Code"].ToString();
        //            m.Name = sqldr["Name"].ToString();
        //            m.Locked = bool.Parse(sqldr["Locked"].ToString());
        //            m.Sort = int.Parse(sqldr["Sort"].ToString());
        //            list.Add(m);

        //        }
        //    }
        //    if (con.State == ConnectionState.Open)
        //    {
        //        con.Close();
        //    }
        //    return list;

        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_SubCategory>>> Get()
        {
            return await _context.Item_SubCategory.ToListAsync();
        }
        [HttpGet("GetList_isusedNext")]

        public List<Models.M_SubCategory> GetList_isusedNext()
        {
            Executer.DataProcess dp = new Executer.DataProcess();
            DataSet ds = new DataSet();
            string strSQL;
            strSQL = @" select *,(select isnull(COunt(1),0) FROM Item_SubCategoryDetail where level1code=a.level1code and level2code=a.code) as used from Item_SubCategory  as a ";
            SqlCommand cmd = new SqlCommand(strSQL, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable _dt = new DataTable();
            sda.Fill(_dt);
            List<Models.M_SubCategory> list = null;
            if (_dt.Rows.Count > 0)
            {
                list = new List<Models.M_SubCategory>();
                foreach (DataRow sqldr in _dt.Rows)
                {
                    var fields = new Models.M_SubCategory();
                    //  fields.ID = int.Parse(sqldr["ID"].ToString());
                    fields.Code = sqldr["Code"].ToString();
                    fields.COMPANY_CODE = sqldr["Company_Code"].ToString();
                    fields.Level1Code = sqldr["Level1Code"].ToString();
                    fields.Level1Name = sqldr["Level1Name"].ToString();
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
        // GET: api/VendorType/5
        [HttpGet("{level1code}/{code}")]
        public async Task<ActionResult<M_SubCategory>> Get(string level1code, string code)
        {
            
            var m = await _context.Item_SubCategory.FindAsync(companycode, level1code, code);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_SubCategory m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_SubCategory obj = new M_SubCategory();
                
                obj = await _context.Item_SubCategory.FindAsync(companycode, m.Level1Code, m.Code);

                if (obj != null)
                {
                 //   obj.COMPANY_CODE = m.COMPANY_CODE;


                    obj.Name = m.Name;
                    obj.Locked = m.Locked;
                    obj.Sort = m.Sort;

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
        public async Task<ActionResult<M_SubCategory>> create(M_SubCategory m)
        {

            m.Code = dbset.getUpdateMasterCount(m.Level1Code);
           
            m.Level1Name= dbset.getbrandname(m.Level1Code);
            _context.Item_SubCategory.Add(m);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Get", new { level1code = m.Level1Code, code = m.Code }, m);
        }

        // DELETE: api/VendorType/5
        [HttpDelete("{level1code}/{code}")]
        public async Task<ActionResult<M_SubCategory>> Delete(string level1code, string code)
        {
            
            var m = await _context.Item_SubCategory.FindAsync(companycode, level1code, code);
            if (m == null)
            {
                return NotFound();
            }

            _context.Item_SubCategory.Remove(m);
            await _context.SaveChangesAsync();

            return m;
        }

        [HttpGet]
        [Route("GetLevel2WithLevel1")]
        public async Task<ActionResult<IEnumerable<M_SubCategory>>> GetLevel2WithLevel1(string id)
        {

            var m_Level2 = await _context.Item_SubCategory.Where(x => x.Level1Code == id).ToListAsync();

            if (m_Level2 == null)
            {
                return NotFound();
            }

            return m_Level2;
        }

    }
}
