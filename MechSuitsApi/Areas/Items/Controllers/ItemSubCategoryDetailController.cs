using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MechSuitsApi.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using CoreInfrastructure.ItemInformation.ItemSubCategoryDetail;
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
    [Route("api/itemsubcategorydetail")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ItemSubCategoryDetailController : ControllerBase
    {
        private readonly IUriService uriService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AppDBContext _context;
        private SqlConnection con;
        D_DB dbset = null;
        string companycode = "";   string user = "";
        Connection c = new Connection();
        public ItemSubCategoryDetailController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
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
        public async Task<ActionResult<IEnumerable<M_SubCategoryDetail>>> Get()
        {
            return await _context.Item_SubCategoryDetail.ToListAsync();
        }



        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_SubCategoryDetail>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.Item_SubCategoryDetail
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.Item_SubCategoryDetail.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_SubCategoryDetail>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_SubCategoryDetail>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            // return await _context.Area.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.Item_SubCategoryDetail.Where(m => m.Name.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.Item_SubCategoryDetail.Where(m => m.Name.Contains(title) ).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_SubCategoryDetail>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);

        }



        [HttpGet("GetList_isusedNext")]
        public List<Models.M_SubCategoryDetail> GetList_isusedNext()
        {
            Executer.DataProcess dp = new Executer.DataProcess();
            DataSet ds = new DataSet();
            string strSQL;
            strSQL = @" select *,(select isnull(COunt(1),0) FROM ITEM_INFORMATION
where CATEGORY_ID=a.level1code and SUB_CATEGORY_ID=a.Level2Code and SUB_CATEGORY_DETAIL_ID=a.Code) as used from Item_SubCategoryDetail  as a ";
            SqlCommand cmd = new SqlCommand(strSQL, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable _dt = new DataTable();
            sda.Fill(_dt);
            List<Models.M_SubCategoryDetail> list = null;
            if (_dt.Rows.Count > 0)
            {
                list = new List<Models.M_SubCategoryDetail>();
                foreach (DataRow sqldr in _dt.Rows)
                {
                    var fields = new Models.M_SubCategoryDetail();
                    //  fields.ID = int.Parse(sqldr["ID"].ToString());
                    fields.Code = sqldr["Code"].ToString();
                    fields.Company_Code = sqldr["COMPANY_CODE"].ToString();
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
        // GET: api/VendorType/5
        [HttpGet("{level1code}/{level2code}/{code}")]
        public async Task<ActionResult<M_SubCategoryDetail>> Get(string level1code, string level2code, string code)
        {
            
            var m = await _context.Item_SubCategoryDetail.FindAsync(companycode, level1code, level2code, code);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_SubCategoryDetail m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                
                M_SubCategoryDetail obj = new M_SubCategoryDetail();
                obj = await _context.Item_SubCategoryDetail.FindAsync(companycode, m.Level1Code, m.Level2Code, m.Code);

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

        [HttpPut("{id}")]

        [HttpPost]
        public async Task<ActionResult<M_SubCategoryDetail>> create(M_SubCategoryDetail m)
        {

            m.Code = dbset.getUpdateMasterCount(m.Level2Code, m.Level1Code);
            _context.Item_SubCategoryDetail.Add(m);
            await _context.SaveChangesAsync();

            //  return CreatedAtAction("Get", new { code = m.Code }, m);
            return CreatedAtAction("Get", new { level1code = m.Level1Code, level2code = m.Level2Code, code = m.Code }, m);
        }
        [HttpGet]
        [Route("GetLevel3WithLevel1n2")]
        public async Task<ActionResult<IEnumerable<M_SubCategoryDetail>>> GetLevel3WithLevel1n2(string l1, string l2)
        {

            var m = await _context.Item_SubCategoryDetail.Where(x => x.Level1Code == l1 && x.Level2Code == l2).ToListAsync();

            //  var M_Level3 = await _context.AS_Acclevel3.FindAsync().ToList();// Include(i => i.Code).FirstOrDefaultAsync(i => i.Code == id);
            //  var M_Level3= await _context.AS_Acclevel3.Where(s => s.Code.Contains(id));

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }

        // DELETE: api/VendorType/5
        [HttpDelete("{level1code}/{level2code}/{code}")]
        public async Task<ActionResult<M_SubCategoryDetail>> Delete(string level1code, string level2code, string code)
        {
            
            var m = await _context.Item_SubCategoryDetail.FindAsync(companycode, level1code, level2code, code);
            if (m == null)
            {
                return NotFound();
            }

            _context.Item_SubCategoryDetail.Remove(m);
            await _context.SaveChangesAsync();

            return m;
        }
    }
}
