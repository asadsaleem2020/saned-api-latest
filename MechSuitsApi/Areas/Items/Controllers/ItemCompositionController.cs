using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreInfrastructure.ItemInformation.ItemComposition;
using MechSuitsApi.Classes;
using Microsoft.EntityFrameworkCore;
using CoreInfrastructure.Generals;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MechSuitsApi.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MechSuitsApi.Areas.Items.Controllers
{
    [Authorize]
    [Route("api/itemcomposition")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ItemCompositionController : ControllerBase
    {
        private readonly IUriService uriService;
        private IHttpContextAccessor _httpContextAccessor;
        D_DB dbset = null;
        Connection c = null;
        string companycode = "";   string user = "";
        GeneralFunctions general = null;
        private readonly AppDBContext _context;
        
        public ItemCompositionController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            c = new Connection();
        dbset = new D_DB(c.V_connection);
        _httpContextAccessor = httpContextAccessor;
        var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
         companycode = currentUser.FindFirst("Company").Value.ToString();    user = currentUser.FindFirst("User").Value.ToString();
        _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_ItemCompositionHeader>>> Get()
        {

            return await _context.ITEMCOMPOSITIONHEADER.ToListAsync();
        }




        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_ItemCompositionHeader>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.ITEMCOMPOSITIONHEADER
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.ITEMCOMPOSITIONHEADER.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_ItemCompositionHeader>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }


        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_ItemCompositionHeader>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            // return await _context.Area.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.ITEMCOMPOSITIONHEADER.Where(m => m.ITEMCODE.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.ITEMCOMPOSITIONHEADER.Where(m => m.ITEMCODE.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_ItemCompositionHeader>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }







        [HttpGet("{id}")]
        public async Task<ActionResult<M_ItemCompositionHeader>> Get(string id)
        {

            var m = await _context.ITEMCOMPOSITIONHEADER.FindAsync(companycode, id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }



        [HttpGet("{voucherid}/GetDetail")]
        public ActionResult<IEnumerable<M_ItemCompositionDetail>> GetDetail(string voucherid)
        {


            var m = dbset.GetDetailRowForEdit("invoicecode='" + voucherid + "'").ToList();

            //  var M_Level3 = await _context.AS_Acclevel3.FindAsync().ToList();// Include(i => i.Code).FirstOrDefaultAsync(i => i.Code == id);
            //    var M_Level3= await _context.AS_Acclevel3.Where(s => s.Code.Contains(id));

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }

        [HttpPost]
        public void create(M_ItemCompositionHeader H)
        {
            H.COMPANY_CODE = companycode;
            H.CREATED_BY = "";
            H.CREATED_ON = DateTime.Now;
           
            H.REMARKS = H.REMARKS ?? "";
           
           
           
           


            if (H.DetailRows != null && H.DetailRows.Count > 0)
            {
                dbset.Set_c(1, H);
                int i = 0;
                foreach (var item in H.DetailRows)
                {
                    i++;
                    M_ItemCompositionDetail m = new M_ItemCompositionDetail
                    {

                        COMPANY_CODE = H.COMPANY_CODE,
                         PARENTCODE = H.ITEMCODE,
                        SEQNO = i,
                        ItemCode = item.ItemCode,
                        ItemName = item.ItemName ?? "",
                        Amount = item.Amount ?? 0,
                       
                        Quantity = item.Quantity ?? 0,
                        Rate = item.Rate ?? 0,
                       



                    };
                    dbset.Set_d(1, m);
                }
            }

        }

        [HttpPut]
        [Route("update")]
        public void update(M_ItemCompositionHeader H)
        {
            H.COMPANY_CODE = companycode;
            H.CREATED_BY = "";
            H.CREATED_ON = DateTime.Now;
            
            H.REMARKS = H.REMARKS ?? "";

            

            int DELETED_COUNT = 0;
            if (H.DetailRows != null && H.DetailRows.Count > 0)
            {
                dbset.Set_c(2, H);
                string where = "COMPANY_CODE='" + companycode + "' AND ITEMCODE='" + H.ITEMCODE + "'";
                DELETED_COUNT = dbset.Delete_Voucher_for_Edit_mode(where);
                if (DELETED_COUNT >= 0)
                {

                    int i = 0;
                    foreach (var item in H.DetailRows)
                    {
                        i++;
                        M_ItemCompositionDetail d = new M_ItemCompositionDetail
                        {
                            COMPANY_CODE = H.COMPANY_CODE,
                             PARENTCODE = H.ITEMCODE,
                            SEQNO = i,
                            ItemCode = item.ItemCode,
                            ItemName = item.ItemName ?? "",
                            Amount = item.Amount ?? 0,                           
                            Quantity = item.Quantity ?? 0,
                            Rate = item.Rate ?? 0,
                          
                          

                        };
                        dbset.Set_d(1, d);
                    }
                }

            }




        }


        [HttpGet("{purpose}/GetLists")]
        public async Task<ActionResult<IEnumerable<M_ItemCompositionHeader>>> GetLists(string purpose)
        {
            string where = "1=1";
            var flag = false;
            if (purpose == "1")
            {
                flag = true;
            }
            return await _context.ITEMCOMPOSITIONHEADER.Where(m => m.STATUS == flag).ToListAsync();


        }
       




    }
}
