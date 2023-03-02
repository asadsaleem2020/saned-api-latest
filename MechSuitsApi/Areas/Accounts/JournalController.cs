using MechSuitsApi.Classes;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreInfrastructure.Accounts.Transaction.GeneralJournal;
using Microsoft.EntityFrameworkCore;
using CoreInfrastructure.Generals;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MechSuitsApi.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MechSuitsApi.Areas.Accounts
{
    //[Authorize]
    [Route("api/Journal")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class JournalController : ControllerBase
    {
        private readonly IUriService uriService;
        string companycode = "C001"; string user = "1001";

        D_DB dbset = null;

        private IHttpContextAccessor _httpContextAccessor;
        GeneralFunctions general = null;
        
        
        

        private readonly AppDBContext _context;
        Connection c = null;
        public JournalController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            c = new Connection();
            dbset = new D_DB(c.V_connection);
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
          
            //companycode = currentUser.FindFirst("Company").Value.ToString();    user = currentUser.FindFirst("User").Value.ToString();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_Header>>> Get()
        {

            return await _context.GeneralJournal.Where(m => m.SeqNo == 1).ToListAsync();
        }

        [HttpGet]
        [Route("UNPOSTED/chunks")]
        public async Task<ActionResult<IEnumerable<M_Header>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.GeneralJournal.Where(m => (m.Checked == "0"&& m.SeqNo==1))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.GeneralJournal.Where(m => m.Checked == "0").CountAsync();
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

            var pagedData = await _context.GeneralJournal.Where(m => (m.VoucherId.Contains(title) && (m.Checked == "0" && m.SeqNo==1)))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.GeneralJournal.Where(m => (m.VoucherId.Contains(title) && (m.Checked == "0" && m.SeqNo == 1))).CountAsync();
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
            var pagedData = await _context.GeneralJournal.Where(m => (m.Checked == "1" && m.SeqNo == 1))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.GeneralJournal.Where(m => (m.Checked == "1" && m.SeqNo == 1)).CountAsync();
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

            var pagedData = await _context.GeneralJournal.Where(m => (m.VoucherId.Contains(title) && (m.Checked == "1" && m.SeqNo == 1)))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.GeneralJournal.Where(m => (m.VoucherId.Contains(title) && (m.Checked == "1" && m.SeqNo == 1))).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Header>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<M_Header>> Get(Int64 id)
        {
            var m = await _context.GeneralJournal.FindAsync(id);

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
        public void create(M_Header j)


        {
            string journalcode = "";
            string year, month, yearmonth;
            year = DateTime.Parse(j.DocumentDate.ToString()).Year.ToString();
            month = DateTime.Parse(j.DocumentDate.ToString()).Month.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            year = year.Substring(2, 2);
            yearmonth = year + month;
            j.COMPANY_CODE = companycode;
            j.CREATED_BY = user;
            //  b.COSTCENTER_CODE = "";
            j.CREATED_ON = DateTime.Now;

            j.IS_DELETED = "0";
            j.DELETED_BY = user;
            j.DELETED_ON = DateTime.Now;

            j.DELETED_ON = DateTime.Now;
            journalcode = dbset.getUpdateMasterCount(j.VoucherType, yearmonth, j);
            j.VoucherId = journalcode;
            string triggerStatus = j.Status;
            if (j.Checked == "1")
            {
                //j.Checked = "0";
                j.Status = "0";
                j.UPDATED_BY = user;
                j.UPDATED_ON = DateTime.Now;
            }
            else
            {
                j.UPDATED_BY = "";
                j.UPDATED_ON = DateTime.Now;
            }
            if (j.Status == "1")
            {
                j.Checked = "1";
                triggerStatus = "1";
                j.UPDATED_BY = user;
                j.UPDATED_ON = DateTime.Now;
                j.APPROVED_BY = "";
                j.APPROVED_ON = DateTime.Now;
            }
            else
            {

                j.APPROVED_BY = user;
                j.APPROVED_ON = DateTime.Now;
            }
            decimal? ldec_credit, ldec_debit;
            ldec_credit = 0;
            ldec_debit = 0;
            foreach (var item in j.DetailRows)
            {
                ldec_credit += item.Credit == null ? 0 : item.Credit;
                ldec_debit += item.Debit == null ? 0 : item.Debit;
            }
            if (ldec_credit != ldec_debit)
            {
                ModelState.AddModelError(string.Empty, "Debit and Credit Amount Should be Equal");


            }
            if (j.DetailRows != null && j.DetailRows.Count > 0)
            {
                int i = 0;
                foreach (var item in j.DetailRows)
                {
                    i++;
                    j.SeqNo = i;
                    j.AccountCode = item.AccountCode;
                    j.AccountName = item.AccountName ?? "";
                    j.Debit = item.Debit;
                    j.Credit = item.Credit;
                    j.Remarks = item.Remarks == null ? "" : item.Remarks;
                    dbset.Set_a(1, j);
                }
            }


            string Goto = journalcode + "," + j.VoucherType + ",C";
            Console.WriteLine(Goto);



        }


        [HttpPut]
        [Route("update")]
        public void update(M_Header j)
        {
            j.COMPANY_CODE = companycode;
            j.CREATED_BY = "";
            j.CREATED_ON = DateTime.Now;


            j.IS_DELETED = "0";
            j.DELETED_BY = user;
            j.DELETED_ON = DateTime.Now;
            string journalcode = "";
            journalcode = j.VoucherId;
            int DELETED_COUNT = 0;
            string triggerStatus = j.Status;
            if (j.Checked == "1")
            {
                //j.Checked = "0";
                j.Status = "0";
                j.UPDATED_BY = "";
                j.UPDATED_ON = DateTime.Now;
            }
            else
            {
                j.UPDATED_BY = user;
                j.UPDATED_ON = DateTime.Now;
            }
            if (j.Status == "1")
            {
                j.Checked = "1";
                j.Status = "0";
                triggerStatus = "1";
                j.UPDATED_BY = user;
                j.UPDATED_ON = DateTime.Now;
                j.APPROVED_BY = user;
                j.APPROVED_ON = DateTime.Now;
            }
            else
            {

                j.APPROVED_BY = user;
                j.APPROVED_ON = DateTime.Now;
            }
            decimal? ldec_credit, ldec_debit;
            ldec_credit = 0;
            ldec_debit = 0;
            foreach (var item in j.DetailRows)
            {
                ldec_credit += j.Debit == null ? 0 : j.Debit;
                ldec_debit += j.Credit == null ? 0 : j.Credit;
            }

            if (j.DetailRows != null && j.DetailRows.Count > 0)
            {
                string where = "COMPANY_CODE='"+companycode+"' AND VOUCHERID='" + j.VoucherId + "' AND VOUCHERTYPE='" + j.VoucherType + "'";
                DELETED_COUNT = dbset.Delete_Voucher_for_Edit_mode(where);
                if (DELETED_COUNT >= 0)
                {
                    int i = 0;
                    foreach (var item in j.DetailRows)
                    {
                        i++;
                        j.SeqNo = i;
                        j.AccountCode = item.AccountCode;
                        j.AccountName = item.AccountName ?? "";
                        j.Debit = item.Debit;
                        j.Credit = item.Credit;
                        j.Remarks = item.Remarks == null ? "" : item.Remarks;
                        dbset.Set_a(1, j);
                    }

                }

            }


        }

        [HttpGet("{purpose}/GetLists")] 
        public async Task<ActionResult<IEnumerable<M_Header>>> GetLists(string purpose)
        {
            string where = "1=1";
            //if (purpose == "A")
            //{
            //    where = "status=1 and checked=1 and seqno=1 order by VoucherId desc";
            //}
            //if (purpose == "A") // checked 
            //{
            //    where = "1";
            //}
            //if (purpose == "U") //  unchecked
            //{
            //    where = "checked=0";
            //}
            //if (purpose == "Rejected")
            //{
            //    where = "status=0 and Rejected=1 and seqno=1 order by VoucherId desc";
            //}
            return await _context.GeneralJournal.Where(m => m.SeqNo == 1 && m.Checked == purpose).ToListAsync();

           
        }
        [HttpGet("{purpose}/postunpost")]
        public string postunpost(string purpose)
        {
            Console.Write("Updating check status");
            string[] ls_val = purpose.Split(',');
            string ls_purpose = ls_val[0].ToString().Trim();
            string ls_voucherid = ls_val[1].ToString().Trim();
           
            string ls_vouchertype = ls_val[2].ToString().Trim();
            string ls_value = ls_val[3].ToString().Trim();
            string WhereCondition = "Company_Code='" + companycode + "' and VoucherId='" + ls_voucherid.Trim() + "' and voucherType='" + ls_vouchertype.Trim() + "'";
            //if (ls_val[2].Trim() == "Checked")
            //{
                general = new GeneralFunctions(c.V_connection);
                general.UpdateTable("GeneralJournal", "Checked", ls_value, WhereCondition);
                 
            return "Done";
                //string updatequery = "";
                //updatequery = " Updated_on='" + DateTime.Now + "' , updated_by='" + Session["User_id"].ToString() + "'";
                //general = new GeneralFunctions(c.V_connection);
                //general.UpdateTable("GeneralJournal", updatequery, WhereCondition);
            //}
            
          

            
        }
    }
}
