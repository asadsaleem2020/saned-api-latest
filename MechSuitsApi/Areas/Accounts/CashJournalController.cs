using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreInfrastructure.Accounts.Transaction.CashJournal;
using System.Data.SqlClient;
using Executer;
using MechSuitsApi.Classes;
using Microsoft.EntityFrameworkCore;
using CoreInfrastructure.Generals;
using System.Data;
using System.Net.Http.Headers;
using System.IO;
using System.Security.Claims;
using MechSuitsApi.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MechSuitsApi.Areas.Accounts
{
   // [Authorize]
    [Route("api/CashJournal")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class CashJournalController : ControllerBase
    {

        private readonly IUriService uriService; 
        string companycode = "C001";   string user = "1001";
        D_DB dbset = null;
        GeneralFunctions general = null;
       private IHttpContextAccessor _httpContextAccessor;
        Connection c = null;
        private readonly AppDBContext _context;
        public CashJournalController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            c = new Connection();
            dbset = new D_DB(c.V_connection);
            _context = context;
             _httpContextAccessor = httpContextAccessor;
             var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            // companycode = currentUser.FindFirst("Company").Value.ToString();    user = currentUser.FindFirst("User").Value.ToString();
            //var currentUser = "";
        

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_Header>>> Get()
        {

            return await _context.CashJournal_Header.ToListAsync();
        }
        [HttpGet]
        [Route("UNPOSTED/chunks")]
        public async Task<ActionResult<IEnumerable<M_Header>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.CashJournal_Header.Where(m => (m.Checked == "0" && m.VoucherType == "CRV"))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.CashJournal_Header.Where(m => m.Checked == "0" && m.VoucherType == "CRV").CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Header>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_Header>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            // return await _context.Area.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.CashJournal_Header.Where(m => (m.VoucherId.Contains(title) && (m.Checked == "0" && m.VoucherType == "CRV")))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.CashJournal_Header.Where(m => (m.VoucherId.Contains(title) && (m.Checked == "0" && m.VoucherType == "CRV"))).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Header>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);

        }
        [HttpGet]
        [Route("POSTED/chunks")]
        public async Task<ActionResult<IEnumerable<M_Header>>> Get2([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.CashJournal_Header.Where(m => (m.Checked == "1" && m.VoucherType == "CRV"))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.CashJournal_Header.Where(m => (m.Checked == "1" && m.VoucherType == "CRV")).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Header>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("posted/search")]
        public async Task<ActionResult<IEnumerable<M_Header>>> Get6([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            // return await _context.Area.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.CashJournal_Header.Where(m => (m.VoucherId.Contains(title) && (m.Checked == "1" && m.VoucherType == "CRV")))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.CashJournal_Header.Where(m => (m.VoucherId.Contains(title) && (m.Checked == "1" && m.VoucherType == "CRV"))).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Header>(pagedData, validFilter, totalRecords, uriService, route);
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
                   // updatepicturepath(filedata[1].Trim('"'), fileName.Trim());
                    var filename = fileName.Trim();
                   // var DB_PATH = dbPath;

                    //return Ok(new { dbPath });
                    return Ok(new { filename});
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
            strsql = @"UPDATE CashJournal_Header SET IMAGE='" + imgname + "' where VoucherId='" + id + "'";

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
        public async Task<ActionResult<M_Header>> Get(Int64 id)
        {
            var m = await _context.CashJournal_Header.FindAsync(id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }



        [HttpGet("{voucherid}/GetDetail")]
        public ActionResult<IEnumerable<M_Detail>> GetDetail(string voucherid)
        {


            var m = dbset.GetDetailRowForEdit("voucherId='" + voucherid + "'").ToList();

            //  var M_Level3 = await _context.AS_Acclevel3.FindAsync().ToList();// Include(i => i.Code).FirstOrDefaultAsync(i => i.Code == id);
            //    var M_Level3= await _context.AS_Acclevel3.Where(s => s.Code.Contains(id));

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }

        [HttpPost]
        public string create(M_Header b)
        {
            b.COMPANY_CODE = companycode;
           b.CREATED_BY = user;
            //  b.COSTCENTER_CODE = "";
            b.CREATED_ON = DateTime.Now;
            b.IS_DELETED = "0";
            b.DELETED_BY = user;
            b.DELETED_ON = DateTime.Now;
            b.Total = 0;
            foreach (var item in b.DetailRows)
            {
                b.Total += item.Amount;
            }
            b.Remarks = b.Remarks == null ? "" : b.Remarks;



            string journalcode = "";
            string year, month, yearmonth;
            year = DateTime.Parse(b.DocumentDate.ToString()).Year.ToString();
            month = DateTime.Parse(b.DocumentDate.ToString()).Month.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            year = year.Substring(2, 2);
            yearmonth = year + month;

            journalcode = dbset.getUpdateMasterCount(b.VoucherType, yearmonth, b);



            b.VoucherId = journalcode;
            string triggerStatus = b.Checked;
            if (b.Checked == "1")
            {
                b.Checked = "0";
                b.UPDATED_BY = user; 
                b.UPDATED_ON = DateTime.Now;
            }
            else
            {
                b.UPDATED_BY = user;
                b.UPDATED_ON = DateTime.Now;
            }
            if (b.Status == "1")
            {
                b.Checked = "0";
                triggerStatus = "1";
                b.UPDATED_BY = user;
                b.UPDATED_ON = DateTime.Now;
                b.APPROVED_BY = user;
                b.APPROVED_ON = DateTime.Now;
            }
            else
            {

                b.APPROVED_BY = user;
                b.APPROVED_ON = DateTime.Now;
            }
            if (b.DetailRows != null && b.DetailRows.Count > 0)
            {
                dbset.Set_a(1, b);
                int i = 0;
                foreach (var item in b.DetailRows)
                {
                    i++;
                    M_Detail m = new M_Detail
                    {
                        AccountCode = item.AccountCode,
                        AccountName = item.AccountName ?? "",
                        ID = 0,

                        COMPANY_CODE = companycode,
                        SeqNo = i,
                        Amount = item.Amount,
                        Remarks = item.Remarks ?? "",
                        Vouchertype = b.VoucherType,
                        VoucherId = b.VoucherId

                    };
                    dbset.Set_detail(1, m);
                }
            }


            return journalcode;

        }

        [HttpPut]
        [Route("update")]
        public void update(M_Header m)
        {
            
            m.COMPANY_CODE = companycode;
            m.CREATED_BY = user;
            m.CREATED_ON = DateTime.Now;

            m.IS_DELETED = "0";
            m.DELETED_BY = user;
            m.DELETED_ON = DateTime.Now;
            m.Total = 0;
            m.Remarks = m.Remarks == null ? "" : m.Remarks;
            foreach (var item in m.DetailRows)
            {
                m.Total += item.Amount;
            }
            string journalcode = "";
            journalcode = m.VoucherId;
            int DELETED_COUNT = 0;
            string triggerStatus = m.Status;
            if (m.Checked == "1")
            {
                //m.Checked = "0";
                m.Status = "0";
                m.UPDATED_BY = user;
                m.UPDATED_ON = DateTime.Now;
            }
            else
            {
                m.UPDATED_BY = "";
                m.UPDATED_ON = DateTime.Now;
            }
            if (m.Status == "1")
            {
                m.Checked = "1";
                m.Status = "0";
                triggerStatus = "1";
                m.UPDATED_BY = user;
                m.UPDATED_ON = DateTime.Now;
                m.APPROVED_BY = "";
                m.APPROVED_ON = DateTime.Now;
            }
            else
            {

                m.APPROVED_BY = user;
                m.APPROVED_ON = DateTime.Now;
            }
            if (m.DetailRows != null && m.DetailRows.Count > 0)
            {
                dbset.Set_a(2, m);
                string where = "COMPANY_CODE='"+companycode+"' AND VOUCHERID='" + m.VoucherId + "' AND VOUCHERTYPE='" + m.VoucherType + "'";
                DELETED_COUNT = dbset.Delete_Voucher_for_Edit_mode(where);
                if (DELETED_COUNT >= 0)
                {

                    int i = 0;
                    foreach (var item in m.DetailRows)
                    {
                        i++;
                        M_Detail d = new M_Detail
                        {
                            AccountCode = item.AccountCode,
                            AccountName = item.AccountName ?? "",
                            ID = 0,
                            COMPANY_CODE = companycode,
                            SeqNo = i,
                            Amount = item.Amount,
                            Remarks = item.Remarks == null ? "" : item.Remarks,
                            Vouchertype = m.VoucherType,
                            VoucherId = m.VoucherId

                        };
                        dbset.Set_detail(1, d);
                    }
                }

            }




        }

        // Get List according to type

        [HttpGet("{purpose}/{type}/GetListsType")]
        public async Task<ActionResult<IEnumerable<M_Header>>> GetLists(string purpose, string type)
        {
            string where = "1=1";

            return await _context.CashJournal_Header.Where(m => m.Checked == purpose && m.VoucherType == type).ToListAsync();


        }


        [HttpGet("{purpose}/GetLists")]
        public async Task<ActionResult<IEnumerable<M_Header>>> GetLists(string purpose)
        {
            string where = "1=1";
            
            return await _context.CashJournal_Header.Where(m =>  m.Checked == purpose).ToListAsync();


        }
        [HttpGet("{purpose}/postunpost")]
        public string postunpost(string purpose)
        {

            string[] ls_val = purpose.Split(',');
            string ls_purpose = ls_val[0].ToString().Trim();
            string ls_voucherid = ls_val[1].ToString().Trim();

            string ls_vouchertype = ls_val[2].ToString().Trim();
            string ls_value = ls_val[3].ToString().Trim();
            string WhereCondition = "Company_Code='"+companycode+"' and VoucherId='" + ls_voucherid.Trim() + "' and voucherType='" + ls_vouchertype.Trim() + "'";
            //if (ls_val[2].Trim() == "Checked")
            //{
            general = new GeneralFunctions(c.V_connection);
            general.UpdateTable("CashJournal_Header", "Checked", ls_value, WhereCondition);
            //general = new GeneralFunctions(c.V_connection);
            //general.UpdateTable("GeneralJournal", "Checked", "1", WhereCondition);
            return "Done";
            //string updatequery = "";
            //updatequery = " Updated_on='" + DateTime.Now + "' , updated_by='" + Session["User_id"].ToString() + "'";
            //general = new GeneralFunctions(c.V_connection);
            //general.UpdateTable("GeneralJournal", updatequery, WhereCondition);
            //}
            //if (ls_val[2].Trim() == "Approved" || ls_val[2].Trim() == "EApproved" || ls_val[2].Trim() == "CApproved")
            //{
            //    general = new GeneralFunctions(c.V_connection);
            //    general.UpdateTable("GeneralJournal", "Checked", "1", WhereCondition);
            //    general = new GeneralFunctions(c.V_connection);
            //    general.UpdateTable("GeneralJournal", "Status", "1", WhereCondition);
            //    string updatequery = "";
            //    updatequery = " Approved_On='" + DateTime.Now + "' , Approved_by='" + Session["User_id"].ToString() + "' , Updated_on='" + DateTime.Now + "' , updated_by='" + Session["User_id"].ToString() + "'";
            //    general = new GeneralFunctions(c.V_connection);
            //    general.UpdateTable("GeneralJournal", updatequery, WhereCondition);

            //}
            //if (ls_val[2].Trim() == "UnChecked" || ls_val[2].Trim() == "AUnChecked")
            //{
            //    general = new GeneralFunctions(c.V_connection);
            //    general.UpdateTable("GeneralJournal", "Status", "0", WhereCondition);
            //    general = new GeneralFunctions(c.V_connection);
            //    general.UpdateTable("GeneralJournal", "Checked", "0", WhereCondition);
            //    string updatequery = "";
            //    updatequery = " Approved_On='" + DateTime.Now + "' , Approved_by='' , Updated_on='" + DateTime.Now + "' , updated_by=''";
            //    general = new GeneralFunctions(c.V_connection);
            //    general.UpdateTable("GeneralJournal", updatequery, WhereCondition);
            //}
            //string returns = "";

            //if (ls_val[2].Trim() == "Checked" || ls_val[2].Trim() == "EApproved")
            //{
            //    returns = "UnPosted";
            //}
            //if (ls_val[2].Trim() == "UnChecked" || ls_val[2].Trim() == "CApproved")
            //{
            //    returns = "Checked";
            //}
            //if (ls_val[2].Trim() == "Approved" || ls_val[2].Trim() == "AUnChecked")
            //{
            //    returns = "Approved";
            //}

            //return RedirectToAction(returns, "JournalGeneral");
        }

    }
}

