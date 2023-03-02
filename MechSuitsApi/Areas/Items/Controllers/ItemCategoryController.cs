using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MechSuitsApi.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using CoreInfrastructure.ItemInformation.ItemCategory;
using System.Data.SqlClient;
using System.Data;
using System.Security.Claims;
using MechSuitsApi.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MechSuitsApi.Areas.Items.Controllers
{
    [Authorize]
    [Route("api/ItemCategory")]
    [ApiController]
    [EnableCors("AllowOrigin")]

    public class ItemCategoryController : ControllerBase
    {
        private IHttpContextAccessor _httpContextAccessor;

        private readonly AppDBContext _context; private readonly IUriService uriService;
        private SqlConnection con;
        D_DB dbset = null;
        string companycode = "";   string user = "";
        Connection c = new Connection();
        public ItemCategoryController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {  
            this.uriService = uriService;
            _context = context;
            con = c.V_connection;
            dbset = new D_DB(c.V_connection);
            _httpContextAccessor = httpContextAccessor;
             var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            //Console.WriteLine("Almost near..................................................////NEar");
            //Console.WriteLine("Almost near..................................................////NEar");
            //  Console.WriteLine(currentUser.AuthenticationType);
            companycode = currentUser.FindFirst("Company").Value.ToString();
            user = currentUser.FindFirst("User").Value.ToString();
            //var user = "Testing";
            //companycode = "C001";


        }



        [HttpGet]
        [Route("top10")]

        public async Task<ActionResult<IEnumerable<M_ItemCategory>>> Get2(string id)
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
            List<M_ItemCategory> list = null;
            list = new List<M_ItemCategory>();

            if (dt.Rows.Count > 0)
            {

                foreach (DataRow sqldr in dt.Rows)
                {
                    var m = new M_ItemCategory();
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
        public async Task<ActionResult<IEnumerable<M_ItemCategory>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.ItemCategory
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.ItemCategory.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_ItemCategory>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_ItemCategory>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            // return await _context.Area.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.ItemCategory.Where(m => m.Name.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.ItemCategory.Where(m => m.Name.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_ItemCategory>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }



        //[HttpGet]
        //[Route("search")]
        //public async Task<ActionResult<IEnumerable<M_ItemCategory>>> Get1(string id)
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
        //    List<M_ItemCategory> list = null;
        //    list = new List<M_ItemCategory>();

        //    if (dt.Rows.Count > 0)
        //    {

        //        foreach (DataRow sqldr in dt.Rows)
        //        {
        //            var m = new M_ItemCategory();
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
        public async Task<ActionResult<IEnumerable<M_ItemCategory>>> Get()
        {
            return await _context.ItemCategory.ToListAsync();
        }
        [HttpGet("GetList_isusedNext")] 
        public List<Models.M_ItemCategory> GetList_isusedNext()
        {
            Executer.DataProcess dp = new Executer.DataProcess();
            DataSet ds = new DataSet();
            string strSQL;
            strSQL = @" select *,(select isnull(COunt(1),0) FROM Item_SubCategory where level1code=a.code) as used from ItemCategory  as a ";
            SqlCommand cmd = new SqlCommand(strSQL, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable _dt = new DataTable();
            sda.Fill(_dt);
            List<Models.M_ItemCategory> list = null;
            if (_dt.Rows.Count > 0)
            {
                list = new List<Models.M_ItemCategory>();
                foreach (DataRow sqldr in _dt.Rows)
                {
                    var fields = new Models.M_ItemCategory();
                    //  fields.ID = int.Parse(sqldr["ID"].ToString());
                    fields.Code = sqldr["Code"].ToString();
                    fields.COMPANY_CODE = sqldr["Company_Code"].ToString();
                 
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

        
        [HttpGet("{code}")]
        public async Task<ActionResult<M_ItemCategory>> Get(string code)
        {
            
            var m = await _context.ItemCategory.FindAsync(companycode, code);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_ItemCategory m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_ItemCategory obj = new M_ItemCategory();
                                obj = await _context.ItemCategory.FindAsync(companycode,m.Code);

                if (obj != null)
                {
                    obj.COMPANY_CODE = m.COMPANY_CODE;


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

        [HttpPut("{id}")]

        [HttpPost]
        public async Task<ActionResult<M_ItemCategory>> create(M_ItemCategory m)
        {
            m.Code = dbset.getUpdateMasterCount();
            _context.ItemCategory.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { code = m.Code }, m);
        }

        // DELETE: api/VendorType/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_ItemCategory>> Delete(string id)
        {
            
            var m = await _context.ItemCategory.FindAsync(companycode, id);
            if (m == null)
            {
                return NotFound();
            }

            _context.ItemCategory.Remove(m);
            await _context.SaveChangesAsync();

            return m;
        }




    }
}
