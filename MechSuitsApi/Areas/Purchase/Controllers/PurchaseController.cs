using System.Data;
using System.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreInfrastructure.Purchase.PurchaseOrder;
using System.Data.SqlClient;
using Executer;
using MechSuitsApi.Classes;
using Microsoft.EntityFrameworkCore;
using CoreInfrastructure.Generals;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MechSuitsApi.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MechSuitsApi.Areas.Purchase.Controllers
{
    [Authorize]
    [Route("api/purchaseorder")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class PurchaseController : ControllerBase
    {
        private readonly IUriService uriService;
        private IHttpContextAccessor _httpContextAccessor;
        D_DB dbset = null;
        Connection c = null;
        string companycode = "";   
        string user = "";
        GeneralFunctions general = null;
        private readonly AppDBContext _context;
        
        public PurchaseController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            c = new Connection();
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            companycode = currentUser.FindFirst("Company").Value.ToString();
            user = currentUser.FindFirst("User").Value.ToString();
            dbset = new D_DB(c.V_connection);
            _context = context;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_POHeader>>> Get()
        {

            return await _context.POHEADER.ToListAsync();
        }

        [HttpGet]
        [Route("UNPOSTED/chunks")]
        public async Task<ActionResult<IEnumerable<M_POHeader>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.POHEADER.Where(m => m.STATUS == false)
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.POHEADER.Where(m => m.STATUS == false).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_POHeader>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_POHeader>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            // return await _context.Area.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.POHEADER.Where(m => (m.INVOICECODE.Contains(title)&&m.STATUS==false))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.POHEADER.Where(m => (m.INVOICECODE.Contains(title) && m.STATUS == false)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_POHeader>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);

        }
        [HttpGet]
        [Route("POSTED/chunks")]
        public async Task<ActionResult<IEnumerable<M_POHeader>>> Get2([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.POHEADER.Where(m => m.STATUS == true)
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.POHEADER.Where(m => m.STATUS == true).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_POHeader>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("posted/search")]
        public async Task<ActionResult<IEnumerable<M_POHeader>>> Get6([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            // return await _context.Area.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.POHEADER.Where(m => (m.INVOICECODE.Contains(title) && m.STATUS == true))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.POHEADER.Where(m => (m.INVOICECODE.Contains(title) && m.STATUS == true)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_POHeader>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);

        }




        [HttpPost, DisableRequestSizeLimit]
        [Route("Upload")]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                //var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var filedata = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Split(',');
                    var fileName = DateTime.Now.TimeOfDay.Milliseconds + "" + filedata[0].Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    updatepicturepath(filedata[1].Trim('"'), fileName.Trim());
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        public void updatepicturepath(string id, string imgname)
        {
            SqlCommand dp = new SqlCommand();

            string strsql;
            string no = "";
            //MAX( CAST(CODE AS BIGINT  ))
            strsql = @"UPDATE POHEADER SET IMAGE='" + imgname + "' where VoucherId='" + id + "'";

            if (c.V_connection.State == ConnectionState.Closed)
            {
                c.V_connection.Open();
            }
            dp = new SqlCommand(strsql, c.V_connection);
            dp.ExecuteNonQuery();



            if (c.V_connection.State == ConnectionState.Open)
            {
                c.V_connection.Close();
            }


        }
        [HttpGet("{id}")]
        public async Task<ActionResult<M_POHeader>> Get(string id)
        {

            var m = await _context.POHEADER.FindAsync(companycode, id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }



        [HttpGet("{voucherid}/GetDetail")]
        public ActionResult<IEnumerable<M_PODetail>> GetDetail(string voucherid)
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
        public void create(M_POHeader H)
        {
            H.COMPANY_CODE =  companycode;
            H.CREATED_BY = user;
            H.CREATED_ON = DateTime.Now;
            H.IS_DELETED = "0";
            H.DELETED_BY = user;
            H.DELETED_ON = DateTime.Now;
            H.REMARKS = H.REMARKS ?? "";

            H.TOTAL_AMOUNT = H.TOTAL_AMOUNT ?? 0;
            H.INVOICECODE = H.INVOICECODE ?? "";
            H.NET_AMOUNT = H.NET_AMOUNT ?? 0;
            H.CASH_AMOUNT = H.CASH_AMOUNT ?? 0;
            H.CREDIT_AMOUNT = H.CREDIT_AMOUNT ?? 0;
            //H.TOTAL_AMOUNT = 0;
            //H.NET_AMOUNT = 0;
            //foreach (var item in H.DetailRows)
            //{

            //    H.NET_AMOUNT += item.TotalAmount;
            //}
            string journalcode = "";

            string year, month, yearmonth;
            year = DateTime.Parse(H.INVOICEDATE.ToString()).Year.ToString();
            month = DateTime.Parse(H.INVOICEDATE.ToString()).Month.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            year = year.Substring(2, 2);
            yearmonth = year + month;
            journalcode = dbset.getUpdateMasterCount(yearmonth, H);
            H.INVOICECODE = journalcode;
            H.DOCUMENT_NO = journalcode;
            Boolean triggerStatus = H.STATUS;
            if (H.STATUS == true)
            {
                H.STATUS = false;
                H.UPDATED_BY = user;
                H.UPDATED_ON = DateTime.Now;
            }
            else
            {
                H.UPDATED_BY = user;
                H.UPDATED_ON = DateTime.Now;
            }
            if (H.DetailRows != null && H.DetailRows.Count > 0)
            {
                dbset.Set_c(1, H);
                int i = 0;
                foreach (var item in H.DetailRows)
                {
                    i++;
                    M_PODetail m = new M_PODetail
                    {

                        COMPANY_CODE = H.COMPANY_CODE,
                        INVOICECODE = H.INVOICECODE,
                        SEQNO = i,
                        ItemCode = item.ItemCode,
                        ItemName = item.ItemName ?? "",
                        Amount = item.Amount ?? 0,
                        TotalAmount = item.TotalAmount,
                        DiscountAmount = item.DiscountAmount,
                        DiscountRate = item.DiscountRate,
                        MeasuringUnit = item.MeasuringUnit ?? "",
                        Quantity = item.Quantity ?? 0,
                        Rate = item.Rate ?? 0,
                        AmountAfterDiscount = item.AmountAfterDiscount ?? 0,
                        Ctn = item.Ctn ?? 0,
                        Packing = item.Packing ?? 0,
                        TaxAmount = item.TaxAmount ?? 0,
                        TaxRate = item.TaxRate ?? 0,
                        TotalQty = item.TotalQty ?? 0,
                        USERCODE = H.CREATED_BY



                    };
                    dbset.Set_d(1, m);
                }
            }

        }

        [HttpPut]
        [Route("update")]
        public void update(M_POHeader H)
        {
            H.COMPANY_CODE = companycode;
            H.CREATED_BY = user;
            H.CREATED_ON = DateTime.Now;
            H.IS_DELETED = "0";
            H.DELETED_BY = user;
            H.DELETED_ON = DateTime.Now;
            H.REMARKS = H.REMARKS ?? "";

            H.TOTAL_AMOUNT = H.TOTAL_AMOUNT ?? 0;
            H.DISCOUNT_AMOUNT = H.DISCOUNT_AMOUNT ?? 0;
            H.DISCOUNT_RATE = H.DISCOUNT_RATE ?? 0;
            H.NET_AMOUNT = H.NET_AMOUNT ?? 0;
            H.CASH_AMOUNT = H.CASH_AMOUNT ?? 0;
            H.CREDIT_AMOUNT = H.CREDIT_AMOUNT ?? 0;
            //H.TOTAL_AMOUNT = 0;
            //H.NET_AMOUNT = 0;
            //foreach (var item in H.DetailRows)
            //{

            //    H.NET_AMOUNT += item.TotalAmount;
            //}
            string journalcode = "";

            string year, month, yearmonth;
            year = DateTime.Parse(H.INVOICEDATE.ToString()).Year.ToString();
            month = DateTime.Parse(H.INVOICEDATE.ToString()).Month.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            year = year.Substring(2, 2);
            yearmonth = year + month;


            Boolean triggerStatus = H.STATUS;
            if (H.STATUS == true)
            {
                H.STATUS = false;
                H.UPDATED_BY = user;
                H.UPDATED_ON = DateTime.Now;
            }
            else
            {
                H.UPDATED_BY = user;
                H.UPDATED_ON = DateTime.Now;
            }


            int DELETED_COUNT = 0;
            if (H.DetailRows != null && H.DetailRows.Count > 0)
            {
                dbset.Set_c(2, H);
                string where = "COMPANY_CODE='" + companycode + "' AND INVOICECODE='" + H.INVOICECODE + "'";
                DELETED_COUNT = dbset.Delete_Voucher_for_Edit_mode(where);
                if (DELETED_COUNT >= 0)
                {

                    int i = 0;
                    foreach (var item in H.DetailRows)
                    {
                        i++;
                        M_PODetail d = new M_PODetail
                        {
                            COMPANY_CODE = H.COMPANY_CODE,
                            INVOICECODE = H.INVOICECODE,
                            SEQNO = i,
                            ItemCode = item.ItemCode,
                            ItemName = item.ItemName ?? "",
                            Amount = item.Amount ?? 0,
                            TotalAmount = item.TotalAmount,
                            DiscountAmount = item.DiscountAmount,
                            DiscountRate = item.DiscountRate,
                            MeasuringUnit = item.MeasuringUnit ?? "",
                            Quantity = item.Quantity ?? 0,
                            Rate = item.Rate ?? 0,
                            AmountAfterDiscount = item.AmountAfterDiscount ?? 0,
                            Ctn = item.Ctn ?? 0,
                            Packing = item.Packing ?? 0,
                            TaxAmount = item.TaxAmount ?? 0,
                            TaxRate = item.TaxRate ?? 0,
                            TotalQty = item.TotalQty ?? 0,
                            USERCODE = H.CREATED_BY

                        };
                        dbset.Set_d(1, d);
                    }
                }

            }




        }


        [HttpGet("{purpose}/GetLists")]
        public async Task<ActionResult<IEnumerable<M_POHeader>>> GetLists(string purpose)
        {
            string where = "1=1";
            var flag = false;
            if (purpose == "1")
            {
                flag = true;
            }
            return await _context.POHEADER.Where(m => m.STATUS == flag).ToListAsync();


        }
        [HttpGet("{purpose}/postunpost")]
        public string postunpost(string purpose)
        {

            string[] ls_val = purpose.Split(',');
            string ls_purpose = ls_val[0].ToString().Trim();
            string ls_voucherid = ls_val[1].ToString().Trim();
            string ls_value = ls_val[2].ToString().Trim();
            string WhereCondition = "Company_Code='" + companycode + "' and INVOICECODE='" + ls_voucherid.Trim() + "'";
            //if (ls_val[2].Trim() == "Checked")
            //{
            general = new GeneralFunctions(c.V_connection);
            general.UpdateTable("POHEADER", "STATUS", ls_value, WhereCondition);

            return "Done";


        }




    }
}
