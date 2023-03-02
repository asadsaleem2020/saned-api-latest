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
using CoreInfrastructure.AccomodationSystem;
using CoreInfrastructure.GeneralSetting.Chat;
using CoreInfrastructure.Recruitement;
using CoreInfrastructure.Recruitement.RecruitmentOrder;

namespace MechSuitsApi.Areas.AccomodationSystem
{
    // [Authorize]
    [Route("api/Orders")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class OrderController : ControllerBase
    {

        private readonly IUriService uriService;
        string companycode = "C001"; string user = "";
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        private IHttpContextAccessor _httpContextAccessor;
        public OrderController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context;
            con = c.V_connection;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            //   companycode = currentUser.FindFirst("Company").Value.ToString();
            //   user = currentUser.FindFirst("User").Value.ToString();
        }

        // GET: api/Level2
        [HttpGet]
        public async Task<IEnumerable<M_Order>> GetList()
        {
            return await _context.Order.FromSqlRaw("SELECT ID, Company_Code, OrderNumber, Date, (Select Name from RCustomer where Code= ClientID) as ClientID, (Select Name from WorkerData where Code= WorkerName) as WorkerName, RequestTypeID, ContractDuration, RentalStartDate, RentalCost, ExperienceAllowed, ProbationaryStart, ProbationaryEnd, TrailDays, CostofDayAfterTrail, SponsorshipFee, ValueAddedFee, PaymentStatus, PaidAmount, TotalAmount, OrderStatus, Notes, Message, Status, Sort, Locked FROM [Order]").ToListAsync();
        }
        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_Order>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);

            var m = _context.Order.FromSqlRaw("SELECT ID, Company_Code, OrderNumber, Date, (Select Name from RCustomer where Code= ClientID) as ClientID, (Select Name from WorkerData where Code= WorkerName) as WorkerName, RequestTypeID, ContractDuration, RentalStartDate, RentalCost, ExperienceAllowed, ProbationaryStart, ProbationaryEnd, TrailDays, CostofDayAfterTrail, SponsorshipFee, ValueAddedFee, PaymentStatus, PaidAmount, TotalAmount, OrderStatus, Notes, Message, Status, Sort, Locked FROM [Order]").ToList();
            var pagedData = m
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList ();
            var totalRecords = await _context.Order.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Order>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("chunksType")]
        public async Task<ActionResult<IEnumerable<M_Order>>> Gettype([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            string title = HttpContext.Request.Query["title"];
            var route = Request.Path.Value + "?title=" + title;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);

            var d = title;
            if (title == "3") { d = "1 OR RequestTypeID = 2"; }
            Console.WriteLine(d);
            int totalRecords;

                var m = _context.Order.FromSqlRaw("SELECT ID, Company_Code, OrderNumber, Date, (Select Name from RCustomer where Code= ClientID) as ClientID, (Select Name from WorkerData where Code= WorkerName) as WorkerName, RequestTypeID, ContractDuration, RentalStartDate, RentalCost, ExperienceAllowed, ProbationaryStart, ProbationaryEnd, TrailDays, CostofDayAfterTrail, SponsorshipFee, ValueAddedFee, PaymentStatus, PaidAmount, TotalAmount, OrderStatus, Notes, Message, Status, Sort, Locked FROM [Order] where RequestTypeID = " + d);

               var pagedData = m.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
                totalRecords = await m.CountAsync();

            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Order>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_Order>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];

            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);


            var pagedData = await _context.Order.Where(m => m.OrderNumber.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.Order.Where(m => m.OrderNumber.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Order>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }
        // GET: api/Level2/5
        [HttpGet("{OrderNumber}")]
        public async Task<ActionResult<M_Order>> Getm(string OrderNumber)
        {
            var m = await _context.Order.FindAsync(OrderNumber);
            if (m == null)
            {
                return NotFound();
            }
            return m;
        }
        //public ActionResult<IEnumerable<M_Order>> Getm(string id)
        //{
        //    var sql = "Select ID, Company_Code, OrderNumber, Date, ClientID, WorkerName, RequestTypeID, ContractDuration, RentalStartDate, RentalCost, ExperienceAllowed, ProbationaryStart, ProbationaryEnd, TrailDays, CostofDayAfterTrail, SponsorshipFee , ValueAddedFee, Notes, Message, Status, Sort, Locked FROM [Order] where ID =" + id;

        //    var m = _context.Order.FromSqlRaw(sql).ToList();

        //    if (m == null)
        //    {
        //        // return NotFound();
        //    }

        //    return m;
        //}

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_Order m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_Order obj = new M_Order();
                //string str = m.OrderNumber;
                //long OrderNumber = long.Parse(str);

                obj = await _context.Order.FindAsync(m.ID);

                if (obj != null)
                {
                   // obj.OrderNumber = m.OrderNumber;
                   obj.ClientID=m.ClientID;
                    obj.ValueAddedFee = m.ValueAddedFee;
                    obj.Company_Code = m.Company_Code;
                    obj.ProbationaryEnd = m.ProbationaryEnd;
                    obj.ProbationaryStart = m.ProbationaryStart;
                    obj.ContractDuration = m.ContractDuration;
                    obj.CostofDayAfterTrail = m.CostofDayAfterTrail;
                    obj.Date = m.Date;
                 //   obj.ID = m.ID;
                    obj.Message = m.Message;
                    obj.PaidAmount = m.PaidAmount;
                    obj.TotalAmount = m.TotalAmount;
                    obj.OrderStatus = m.OrderStatus;
                    obj.PaymentStatus = m.PaymentStatus;
                    obj.Notes = m.Notes;
                    obj.RentalCost = m.RentalCost;
                    obj.RentalStartDate = m.RentalStartDate;
                    obj.RequestTypeID = m.RequestTypeID;
                    obj.SponsorshipFee = m.SponsorshipFee;
                    obj.ExperienceAllowed = m.ExperienceAllowed;
                    obj.TrailDays = m.TrailDays;
                    obj.workerName = m.workerName;
                    obj.Locked = m.Locked;


                    //    obj.EMAIL = m.EMAIL;
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
        [HttpPut]
        [Route("orderStatus/update")]
        public async Task<IActionResult> updateOrderStatus(M_Order m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                M_Order obj = new M_Order();
                obj = await _context.Order.FindAsync(m.OrderNumber);
                if (obj != null)
                {
                    obj.OrderStatus = m.OrderStatus;

                }
                _context.Order.Attach(obj).Property(x => x.OrderStatus).IsModified = true;
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            return Ok(m);
        }
        [HttpPut]
        [Route("orderNotes/update")]
        public async Task<IActionResult> updateOrderNotes(M_Order m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                M_Order obj = new M_Order();
                obj = await _context.Order.FindAsync(m.OrderNumber);
                if (obj != null)
                {
                    obj.Notes = m.Notes;

                }
                _context.Order.Attach(obj).Property(x => x.Notes).IsModified = true;
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            return Ok(m);
        }
        [HttpPost]
        public async Task<ActionResult<M_Order>> create(M_Order m)
        {
            m.OrderNumber = getNext(companycode);
            _context.Order.Add(m);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Getm", new { OrderNumber = m.OrderNumber }, m);
        }
        //public async Task<ActionResult<M_Order>> create(M_Order m)
        //{
        //     Console.WriteLine("Hello creating MAX");
        //    m.OrderNumber = getNext(companycode);
        //    Console.WriteLine(m.OrderNumber);

        //    _context.Order.Add(m);

        //    await _context.SaveChangesAsync();
        //     UpdateWorkerStatus(m.WorkerName);
        //    return CreatedAtAction("Getm", new { OrderNumber = m.OrderNumber }, m);
        //}
        public string getNext(string Company_Code)
        {
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = @"SELECT ISNULL(MAX( CAST(OrderNumber as bigint)),10000) +1  as OrderNumber from [Order]";

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(strsql, con);
            sqldr = cmd.ExecuteReader();
            if (sqldr.Read())
            {
                no = sqldr["OrderNumber"].ToString().Trim();
            }
            sqldr.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (no.Trim() == "") no = "10001";
            return no;
        }
        public string UpdateWorkerStatus(string workerCode)
        {
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = @"update WorkerData set status='1' where Code="+ workerCode;
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
            if (no.Trim() == "") no = "10001";
            return no;
        }
        // DELETE: api/Level2/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_Order>> Deletem(string id)
        {
            var m = await _context.Order.FindAsync(id);
            if (m == null)
            {
                return NotFound();
            }
            _context.Order.Remove(m);
            await _context.SaveChangesAsync();
            return m;
        }
    }
}
