using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MechSuitsApi.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using System.Data.SqlClient;
using System.Data;
using Executer;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MechSuitsApi.Interfaces;
using CoreInfrastructure.SpnoserShip;
using System.Drawing.Printing;

namespace MechSuitsApi.Areas.sponsership
{
    [Route("api/SponsorshipTransferRequest")]
    [ApiController]
    public class SponsorshipTransferRequestController : ControllerBase
    {
        private readonly IUriService uriService;
        string companycode = "C001"; string user = "";
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        private IHttpContextAccessor _httpContextAccessor;

        public SponsorshipTransferRequestController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context;
            con = c.V_connection;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            //   companycode = currentUser.FindFirst("Company").Value.ToString();
            //   user = currentUser.FindFirst("User").Value.ToString();erwrewrew
        }



        [HttpGet]
        [Route("chunks/{type}")]
        public async Task<ActionResult<IEnumerable<M_SponsorshipTransferRequest>>> Get1(string type,[FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);

            var m = _context.SponsershipTransferRequest.
           FromSqlRaw("SELECT Code, Company_Code,RefundDate,RefundStatus, OrderNumber, (select name from RCustomer where code = CurrentSponsor) as CurrentSponsor, Date, Experience, TrialStartDate, TrialEndDate, TrialDays, CostAfterTrial, (select name from RCustomer where code = FormerSponsor) as FormerSponsor, (select name from Candidates where code = WorkerID) as WorkerID, (select name from professions where code = (select Profession from Candidates where code = WorkerID)) as job, AccomodationProvider, MedicalCheck, SalaryCheck, AgencyCheck, Salary, SalariesReceived, PaidAmountToPreviousSponsor, ReceivedWorkerDate, CostOfTransfer, Notes, Status, Sort, Locked from [SponsershipTransferRequest] where RefundStatus = " + type);
            var pagedData = m.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();
            //totalRecords = await _context.RecruitementOrder.Where(m => (m.Date < d) && (m.ApplicationStatus == "2")).CountAsync();
            var totalRecords = await m.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_SponsorshipTransferRequest>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("chunksCompleted")]
        public async Task<ActionResult<IEnumerable<M_SponsorshipTransferRequest>>> Get2([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);

            var m = _context.SponsershipTransferRequest.
           FromSqlRaw("SELECT Code, Company_Code,RefundDate,RefundStatus, OrderNumber, (select name from RCustomer where code = CurrentSponsor) as CurrentSponsor, Date, Experience, TrialStartDate, TrialEndDate, TrialDays, CostAfterTrial, (select name from RCustomer where code = CurrentSponsor) as FormerSponsor, (select name from Candidates where code = WorkerID) as WorkerID, (select name from professions where code = (select Profession from Candidates where code = WorkerID)) as job, AccomodationProvider, MedicalCheck, SalaryCheck, AgencyCheck, Salary, SalariesReceived, PaidAmountToPreviousSponsor, ReceivedWorkerDate, CostOfTransfer, Notes, Status, Sort, Locked from [SponsershipTransferRequest] where status='2'");
            var pagedData = m.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();
            //totalRecords = await _context.RecruitementOrder.Where(m => (m.Date < d) && (m.ApplicationStatus == "2")).CountAsync();
            var totalRecords = await m.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_SponsorshipTransferRequest>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("topCardCounter")]
        public List<Dictionary<string, string>> topCardCounter()
        {
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "SELECT Distinct (SELECT COUNT(*) FROM [SponsershipTransferRequest] where RefundStatus = '0') AS [all], (SELECT COUNT(*) FROM [SponsershipTransferRefund]) AS [refund], (SELECT COUNT(*) FROM [SponsershipTransferRequest] Where [Status]='2') AS [completed] FROM [SponsershipTransferRequest]";
            SqlCommand cmd = new SqlCommand(strsql, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable _dt = new DataTable();
            sda.Fill(_dt);
            Dictionary<string, string> kvpList = new Dictionary<string, string>();
            List<Dictionary<string, string>> l = new List<Dictionary<string, string>>();
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow sqldr in _dt.Rows)
                {
                    kvpList.Add("all", sqldr["all"].ToString());
                    kvpList.Add("refund", sqldr["refund"].ToString());
                    kvpList.Add("completed", sqldr["completed"].ToString());
                    l.Add(kvpList);
                }
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return l;
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_SponsorshipTransferRequest>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];

            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);


            var pagedData = await _context.SponsershipTransferRequest.Where(m => m.OrderNumber.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.SponsershipTransferRequest.Where(m => m.OrderNumber.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_SponsorshipTransferRequest>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }

        // GET: api/SponsorshipTransferRequest
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_SponsorshipTransferRequest>>> GetSponsershipTransferRequest()
        {
            return await _context.SponsershipTransferRequest.ToListAsync();
        }

        [HttpGet]
        [Route("RefundList")]
        public async Task<ActionResult<IEnumerable<M_SponsorshipTransferRequest>>> GetOnlyRefundRequests()
        {
            return await _context.SponsershipTransferRequest.
           FromSqlRaw("SELECT Code, Company_Code,RefundDate,RefundStatus, OrderNumber, (select name from RCustomer where code = CurrentSponsor) as CurrentSponsor, Date, Experience, TrialStartDate, TrialEndDate, TrialDays, CostAfterTrial, (select name from RCustomer where code = CurrentSponsor) as FormerSponsor, (select name from Candidates where code = WorkerID) as WorkerID, (select name from professions where code = (select Profession from Candidates where code = WorkerID)) as job, AccomodationProvider, MedicalCheck, SalaryCheck, AgencyCheck, Salary, SalariesReceived, PaidAmountToPreviousSponsor, ReceivedWorkerDate, CostOfTransfer, Notes, Status, Sort, Locked from [SponsershipTransferRequest] where RefundStatus='0'").ToListAsync();
        }


        // GET: api/SponsorshipTransferRequest/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_SponsorshipTransferRequest>> GetM_SponsorshipTransferRequest(string id)
        {
            var m_SponsorshipTransferRequest = await _context.SponsershipTransferRequest.FindAsync(id);

            if (m_SponsorshipTransferRequest == null)
            {
                return NotFound();
            }

            return m_SponsorshipTransferRequest;
        }

        // PUT: api/SponsorshipTransferRequest/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutM_SponsorshipTransferRequest(string id, M_SponsorshipTransferRequest m_SponsorshipTransferRequest)
        {
            if (id != m_SponsorshipTransferRequest.Code)
            {
                return BadRequest();
            }

            _context.Entry(m_SponsorshipTransferRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!M_SponsorshipTransferRequestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/SponsorshipTransferRequest
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<M_SponsorshipTransferRequest>> PostM_SponsorshipTransferRequest(M_SponsorshipTransferRequest m_SponsorshipTransferRequest)
        {
            m_SponsorshipTransferRequest.Code = getNext();
            m_SponsorshipTransferRequest.OrderNumber = (int.Parse(m_SponsorshipTransferRequest.Code) + 10000).ToString();
            _context.SponsershipTransferRequest.Add(m_SponsorshipTransferRequest);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (M_SponsorshipTransferRequestExists(m_SponsorshipTransferRequest.Code))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetM_SponsorshipTransferRequest", new { id = m_SponsorshipTransferRequest.Code }, m_SponsorshipTransferRequest);
        }

        public string getNext()
        {
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = "select max(cast(Code as bigint)) +1 as Code from SponsershipTransferRequest";

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
            if (no.Trim() == "") no = "1";
            return no;
        }

        // DELETE: api/SponsorshipTransferRequest/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteM_SponsorshipTransferRequest(string id)
        {
            var m_SponsorshipTransferRequest = await _context.SponsershipTransferRequest.FindAsync(id);
            if (m_SponsorshipTransferRequest == null)
            {
                return NotFound();
            }

            _context.SponsershipTransferRequest.Remove(m_SponsorshipTransferRequest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool M_SponsorshipTransferRequestExists(string id)
        {
            return _context.SponsershipTransferRequest.Any(e => e.Code == id);
        }
    }
}
