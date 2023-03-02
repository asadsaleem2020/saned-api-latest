
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
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MechSuitsApi.Interfaces;
using Microsoft.CodeAnalysis;
using CoreInfrastructure.Recruitement.RecruitmentOrder;
using Microsoft.Graph;

namespace MechSuitsApi.Areas.Recruitement.RecruitmentOrder
{
    [Route("api/RecruitementOrder")]
    [ApiController]
    // [Authorize]
    [EnableCors("AllowOrigin")]
    public class RecruitementOrderController : ControllerBase
    {
        private readonly IUriService uriService;
        string companycode = ""; string user = "";
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        private IHttpContextAccessor _httpContextAccessor;
        public RecruitementOrderController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
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
        public async Task<ActionResult<IEnumerable<M_RecruitementOrder>>> GetList()
        {
            return await _context.RecruitementOrder.FromSqlRaw("SELECT (Select Name from RCustomer where Code= Client) as Client,OrderStatus,TimelineStatus,PaymentStatus, PaidAmount, TotalAmount, (Select Name from Country where Code= Country)as Country, (Select Name from professions where Code= Profession)as Profession, (select Name from Agents where Code = IDNumber) as IDNumber, Company_Code,  Date, VisaType, VisaNumber, VisaDate, Religion, Type, Experience, Age, Appearance, MaritalSatus, CustomerTerms, Workplace, Rworkplace, ArrivalStation, Salary, ApplicationStatus, OrderPackage, ArrivalStationID, DelegateID, AgentName, Agentsource, AgencyNumber, Agencysource, Agencydate, ContractNo, DateofContract, RecruitmentFee, AdditionsalRecruitementFee, ValueAddedFee, VisaPhoto, BackingContractPhoto, AgencyPhoto, AgentIDPhoto, Notes, SMS, Status, Sort, Locked, OrderNumber, ID, WorkerName, PassportNumber, PassportExpiry, ContactNumber, DelegatePhoto FROM RecruitementOrder").ToListAsync();
        }


        [HttpGet]
        [Route("searchfilters/{type}/{condition}")]
        public async Task<ActionResult<IEnumerable<M_RecruitementOrder>>> Get3(string type, string condition, [FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var querySQL = "SELECT (Select Name from RCustomer where Code= Client) as Client,OrderStatus,TimelineStatus,PaymentStatus, PaidAmount, TotalAmount, (Select Name from Country where Code= Country)as Country, (Select Name from professions where Code= Profession)as Profession, (select Name from Agents where Code = IDNumber) as IDNumber, Company_Code,  Date, VisaType, VisaNumber, VisaDate, Religion, Type, Experience, Age, Appearance, MaritalSatus, CustomerTerms, Workplace, Rworkplace, ArrivalStation, Salary, ApplicationStatus, OrderPackage, ArrivalStationID, DelegateID, AgentName, Agentsource, AgencyNumber, Agencysource, Agencydate, ContractNo, DateofContract, RecruitmentFee, AdditionsalRecruitementFee, ValueAddedFee, VisaPhoto, BackingContractPhoto, AgencyPhoto, AgentIDPhoto, Notes, SMS, Status, Sort, Locked, OrderNumber, ID, WorkerName, PassportNumber, PassportExpiry, ContactNumber, DelegatePhoto FROM RecruitementOrder where  " + condition;
            if (type == "agency")
            {
                querySQL = "SELECT (Select Name from RCustomer where Code= Client) as Client,OrderStatus,TimelineStatus,PaymentStatus, PaidAmount, TotalAmount, (Select Name from Country where Code= Country)as Country, (Select Name from professions where Code= Profession)as Profession, (select Name from Agents where Code = IDNumber) as IDNumber, Company_Code,  Date, VisaType, VisaNumber, VisaDate, Religion, Type, Experience, Age, Appearance, MaritalSatus, CustomerTerms, Workplace, Rworkplace, ArrivalStation, Salary, ApplicationStatus, OrderPackage, ArrivalStationID, DelegateID, AgentName, Agentsource, AgencyNumber, Agencysource, Agencydate, ContractNo, DateofContract, RecruitmentFee, AdditionsalRecruitementFee, ValueAddedFee, VisaPhoto, BackingContractPhoto, AgencyPhoto, AgentIDPhoto, Notes, SMS, Status, Sort, Locked, OrderNumber, ID, WorkerName, PassportNumber, PassportExpiry, ContactNumber, DelegatePhoto FROM RecruitementOrder where ApplicationStatus = '2' and " + condition;

            }
            else if (type == "alternate")
            {
                querySQL = "SELECT (Select Name from RCustomer where Code= Client) as Client,OrderStatus,TimelineStatus,PaymentStatus, PaidAmount, TotalAmount, (Select Name from Country where Code= Country)as Country, (Select Name from professions where Code= Profession)as Profession, (select Name from Agents where Code = IDNumber) as IDNumber, Company_Code,  Date, VisaType, VisaNumber, VisaDate, Religion, Type, Experience, Age, Appearance, MaritalSatus, CustomerTerms, Workplace, Rworkplace, ArrivalStation, Salary, ApplicationStatus, OrderPackage, ArrivalStationID, DelegateID, AgentName, Agentsource, AgencyNumber, Agencysource, Agencydate, ContractNo, DateofContract, RecruitmentFee, AdditionsalRecruitementFee, ValueAddedFee, VisaPhoto, BackingContractPhoto, AgencyPhoto, AgentIDPhoto, Notes, SMS, Status, Sort, Locked, OrderNumber, ID, WorkerName, PassportNumber, PassportExpiry, ContactNumber, DelegatePhoto FROM RecruitementOrder where ApplicationStatus = '2' and " + condition;

            }
            else if (type == "philippines")
            {
                querySQL = "SELECT (Select Name from RCustomer where Code= Client) as Client,OrderStatus,TimelineStatus,PaymentStatus, PaidAmount, TotalAmount, (Select Name from Country where Code= Country)as Country, (Select Name from professions where Code= Profession)as Profession, (select Name from Agents where Code = IDNumber) as IDNumber, Company_Code,  Date, VisaType, VisaNumber, VisaDate, Religion, Type, Experience, Age, Appearance, MaritalSatus, CustomerTerms, Workplace, Rworkplace, ArrivalStation, Salary, ApplicationStatus, OrderPackage, ArrivalStationID, DelegateID, AgentName, Agentsource, AgencyNumber, Agencysource, Agencydate, ContractNo, DateofContract, RecruitmentFee, AdditionsalRecruitementFee, ValueAddedFee, VisaPhoto, BackingContractPhoto, AgencyPhoto, AgentIDPhoto, Notes, SMS, Status, Sort, Locked, OrderNumber, ID, WorkerName, PassportNumber, PassportExpiry, ContactNumber, DelegatePhoto FROM RecruitementOrder where " + condition;
                Console.WriteLine(querySQL);
            }


            var m = _context.RecruitementOrder.FromSqlRaw(querySQL).ToList();
            var pagedData = m.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();
            var totalRecords = m.Count();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_RecruitementOrder>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }

        [HttpGet]
        [Route("chunks/{type}")]
        public async Task<ActionResult<IEnumerable<M_RecruitementOrder>>> Get1(string type, [FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var querySQL = "SELECT (Select Name from RCustomer where Code= Client) as Client,OrderStatus,TimelineStatus,PaymentStatus, PaidAmount, TotalAmount, (Select Name from Country where Code= Country)as Country, (Select Name from professions where Code= Profession)as Profession, (select Name from Agents where Code = IDNumber) as IDNumber, Company_Code,  Date, VisaType, VisaNumber, VisaDate, Religion, Type, Experience, Age, Appearance, MaritalSatus, CustomerTerms, Workplace, Rworkplace, ArrivalStation, Salary, ApplicationStatus, OrderPackage, ArrivalStationID, DelegateID, AgentName, Agentsource, AgencyNumber, Agencysource, Agencydate, ContractNo, DateofContract, RecruitmentFee, AdditionsalRecruitementFee, ValueAddedFee, VisaPhoto, BackingContractPhoto, AgencyPhoto, AgentIDPhoto, Notes, SMS, Status, Sort, Locked, OrderNumber, ID, WorkerName, PassportNumber, PassportExpiry, ContactNumber, DelegatePhoto FROM RecruitementOrder";
            if (type == "agency")
            {
                querySQL = "SELECT (Select Name from RCustomer where Code= Client) as Client,OrderStatus,TimelineStatus,PaymentStatus, PaidAmount, TotalAmount, (Select Name from Country where Code= Country)as Country, (Select Name from professions where Code= Profession)as Profession, (select Name from Agents where Code = IDNumber) as IDNumber, Company_Code,  Date, VisaType, VisaNumber, VisaDate, Religion, Type, Experience, Age, Appearance, MaritalSatus, CustomerTerms, Workplace, Rworkplace, ArrivalStation, Salary, ApplicationStatus, OrderPackage, ArrivalStationID, DelegateID, AgentName, Agentsource, AgencyNumber, Agencysource, Agencydate, ContractNo, DateofContract, RecruitmentFee, AdditionsalRecruitementFee, ValueAddedFee, VisaPhoto, BackingContractPhoto, AgencyPhoto, AgentIDPhoto, Notes, SMS, Status, Sort, Locked, OrderNumber, ID, WorkerName, PassportNumber, PassportExpiry, ContactNumber, DelegatePhoto FROM RecruitementOrder where ApplicationStatus = '2'";

            }
            else if (type == "alternate")
            {
                querySQL = "SELECT (Select Name from RCustomer where Code= Client) as Client,OrderStatus,TimelineStatus,PaymentStatus, PaidAmount, TotalAmount, (Select Name from Country where Code= Country)as Country, (Select Name from professions where Code= Profession)as Profession, (select Name from Agents where Code = IDNumber) as IDNumber, Company_Code,  Date, VisaType, VisaNumber, VisaDate, Religion, Type, Experience, Age, Appearance, MaritalSatus, CustomerTerms, Workplace, Rworkplace, ArrivalStation, Salary, ApplicationStatus, OrderPackage, ArrivalStationID, DelegateID, AgentName, Agentsource, AgencyNumber, Agencysource, Agencydate, ContractNo, DateofContract, RecruitmentFee, AdditionsalRecruitementFee, ValueAddedFee, VisaPhoto, BackingContractPhoto, AgencyPhoto, AgentIDPhoto, Notes, SMS, Status, Sort, Locked, OrderNumber, ID, WorkerName, PassportNumber, PassportExpiry, ContactNumber, DelegatePhoto FROM RecruitementOrder where ApplicationStatus = '2'";

            }
            else if (type == "philippines")
            {
                querySQL = "SELECT (Select Name from RCustomer where Code= Client) as Client,OrderStatus,TimelineStatus,PaymentStatus, PaidAmount, TotalAmount, (Select Name from Country where Code= Country)as Country, (Select Name from professions where Code= Profession)as Profession, (select Name from Agents where Code = IDNumber) as IDNumber, Company_Code,  Date, VisaType, VisaNumber, VisaDate, Religion, Type, Experience, Age, Appearance, MaritalSatus, CustomerTerms, Workplace, Rworkplace, ArrivalStation, Salary, ApplicationStatus, OrderPackage, ArrivalStationID, DelegateID, AgentName, Agentsource, AgencyNumber, Agencysource, Agencydate, ContractNo, DateofContract, RecruitmentFee, AdditionsalRecruitementFee, ValueAddedFee, VisaPhoto, BackingContractPhoto, AgencyPhoto, AgentIDPhoto, Notes, SMS, Status, Sort, Locked, OrderNumber, ID, WorkerName, PassportNumber, PassportExpiry, ContactNumber, DelegatePhoto FROM RecruitementOrder where Country = '16'";

            }


            var m = _context.RecruitementOrder.FromSqlRaw(querySQL).ToList();
            var pagedData = m.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();
            var totalRecords = await _context.RecruitementOrder.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_RecruitementOrder>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }

        //[HttpGet]
        //[Route("agencychunks")]
        //public async Task<ActionResult<IEnumerable<M_RecruitementOrder>>> GetAgency([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        //{

        //    var route = Request.Path.Value;
        //    var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
        //    var m = _context.RecruitementOrder.
        //    FromSqlRaw("SELECT (Select Name from RCustomer where Code= Client) as Client, " +
        //     "(Select Name from Country where Code= Country)as Country, " +
        //     "(Select Name from professions where Code= Profession)as Profession, " +
        //     "(select Name from Agents where Code = IDNumber) as IDNumber, " +
        //     "Company_Code,  Date, VisaType, VisaNumber, VisaDate, Religion, Type, Experience, Age, Appearance, MaritalSatus, CustomerTerms, Workplace, Rworkplace, ArrivalStation, Salary, ApplicationStatus, OrderPackage, ArrivalStationID, DelegateID, AgentName, Agentsource, AgencyNumber, Agencysource, Agencydate, ContractNo, DateofContract, RecruitmentFee, AdditionsalRecruitementFee, ValueAddedFee, VisaPhoto, BackingContractPhoto, AgencyPhoto, AgentIDPhoto, Notes, SMS, Status, Sort, Locked, OrderNumber, ID, WorkerName, PassportNumber, PassportExpiry, ContactNumber, DelegatePhoto " +
        //     "FROM RecruitementOrder where ApplicationStatus = '2'");

        //    var pagedData = m
        //            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
        //            .Take(validFilter.PageSize)
        //            .ToList();
        //    //var totalRecords = await _context.RecruitementOrder.CountAsync();
        //    var totalRecords = await m.CountAsync();

        //    var pagedReponse = PaginationHelper.CreatePagedReponse<M_RecruitementOrder>(pagedData, validFilter, totalRecords, uriService, route);
        //    return Ok(pagedReponse);
        //}
        //[HttpGet]
        //[Route("alternatechunks")]
        //public async Task<ActionResult<IEnumerable<M_RecruitementOrder>>> GetAlternate([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        //{

        //    var route = Request.Path.Value;
        //    var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
        //    var m = _context.RecruitementOrder.
        //    FromSqlRaw("SELECT (Select Name from RCustomer where Code= Client) as Client, " +
        //     "(Select Name from Country where Code= Country)as Country, " +
        //     "(Select Name from professions where Code= Profession)as Profession, " +
        //     "(select Name from Agents where Code = IDNumber) as IDNumber, " +
        //     "Company_Code,  Date, VisaType, VisaNumber, VisaDate, Religion, Type, Experience, Age, Appearance, MaritalSatus, CustomerTerms, Workplace, Rworkplace, ArrivalStation, Salary, ApplicationStatus, OrderPackage, ArrivalStationID, DelegateID, AgentName, Agentsource, AgencyNumber, Agencysource, Agencydate, ContractNo, DateofContract, RecruitmentFee, AdditionsalRecruitementFee, ValueAddedFee, VisaPhoto, BackingContractPhoto, AgencyPhoto, AgentIDPhoto, Notes, SMS, Status, Sort, Locked, OrderNumber, ID, WorkerName, PassportNumber, PassportExpiry, ContactNumber, DelegatePhoto " +
        //     "FROM RecruitementOrder where ApplicationStatus = '2'");

        //    var pagedData = m
        //            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
        //            .Take(validFilter.PageSize)
        //            .ToList();
        //    //var totalRecords = await _context.RecruitementOrder.CountAsync();
        //    var totalRecords = await m.CountAsync();

        //    var pagedReponse = PaginationHelper.CreatePagedReponse<M_RecruitementOrder>(pagedData, validFilter, totalRecords, uriService, route);
        //    return Ok(pagedReponse);
        //}

        [HttpGet]
        [Route("chunks150/{type}")]
        public async Task<ActionResult<IEnumerable<M_RecruitementOrder>>> Get150(string type, [FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            string title = HttpContext.Request.Query["title"];
            var route = Request.Path.Value + "?title=" + title;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var d = DateTime.Parse(title);
            int totalRecords;
            var querySQL = "SELECT (Select Name from RCustomer where Code = Client) as Client,  (Select Name from Country where Code = Country) as Country,  (Select Name from professions where Code = Profession)as Profession,  (select Name from Agents where Code = IDNumber) as IDNumber, Company_Code,   Date, VisaType, VisaNumber, VisaDate, Religion, Type, Experience, Age,  Appearance, MaritalSatus, CustomerTerms, Workplace, Rworkplace, ArrivalStation, Salary,  ApplicationStatus, OrderPackage, ArrivalStationID, DelegateID, AgentName, Agentsource, AgencyNumber,  Agencysource, Agencydate, ContractNo, DateofContract, RecruitmentFee, AdditionsalRecruitementFee,  ValueAddedFee, VisaPhoto, BackingContractPhoto, AgencyPhoto, AgentIDPhoto, Notes, SMS, Status, Sort,  Locked, OrderNumber, ID, WorkerName, PassportNumber, PassportExpiry, ContactNumber, DelegatePhoto FROM  RecruitementOrder where Date<'" + d + "'";
            if (type == "agency")
            {
                querySQL = "SELECT (Select Name from RCustomer where Code = Client) as Client, (Select Name from Country where Code = Country) as Country, (Select Name from professions where Code = Profession)as Profession, (select Name from Agents where Code = IDNumber) as IDNumber, Company_Code,  Date, VisaType, VisaNumber, VisaDate, Religion, Type, Experience, Age, Appearance, MaritalSatus, CustomerTerms, Workplace, Rworkplace, ArrivalStation, Salary, ApplicationStatus, OrderPackage, ArrivalStationID, DelegateID, AgentName, Agentsource, AgencyNumber, Agencysource, Agencydate, ContractNo, DateofContract, RecruitmentFee, AdditionsalRecruitementFee, ValueAddedFee, VisaPhoto, BackingContractPhoto, AgencyPhoto, AgentIDPhoto, Notes, SMS, Status, Sort, Locked, OrderNumber, ID, WorkerName, PassportNumber, PassportExpiry, ContactNumber, DelegatePhoto FROM RecruitementOrder where ApplicationStatus = '2' AND Date<'" + d + "'";
            }
            else if (type == "alternate")
            {
                querySQL = "SELECT (Select Name from RCustomer where Code = Client) as Client, (Select Name from Country where Code = Country) as Country, (Select Name from professions where Code = Profession)as Profession, (select Name from Agents where Code = IDNumber) as IDNumber, Company_Code,  Date, VisaType, VisaNumber, VisaDate, Religion, Type, Experience, Age, Appearance, MaritalSatus, CustomerTerms, Workplace, Rworkplace, ArrivalStation, Salary, ApplicationStatus, OrderPackage, ArrivalStationID, DelegateID, AgentName, Agentsource, AgencyNumber, Agencysource, Agencydate, ContractNo, DateofContract, RecruitmentFee, AdditionsalRecruitementFee, ValueAddedFee, VisaPhoto, BackingContractPhoto, AgencyPhoto, AgentIDPhoto, Notes, SMS, Status, Sort, Locked, OrderNumber, ID, WorkerName, PassportNumber, PassportExpiry, ContactNumber, DelegatePhoto FROM RecruitementOrder where ApplicationStatus = '2' AND Date<'" + d + "'";
            }
            else if (type == "philippines")
            {
                querySQL = "SELECT (Select Name from RCustomer where Code = Client) as Client, (Select Name from Country where Code = Country) as Country, (Select Name from professions where Code = Profession)as Profession, (select Name from Agents where Code = IDNumber) as IDNumber, Company_Code,  Date, VisaType, VisaNumber, VisaDate, Religion, Type, Experience, Age, Appearance, MaritalSatus, CustomerTerms, Workplace, Rworkplace, ArrivalStation, Salary, ApplicationStatus, OrderPackage, ArrivalStationID, DelegateID, AgentName, Agentsource, AgencyNumber, Agencysource, Agencydate, ContractNo, DateofContract, RecruitmentFee, AdditionsalRecruitementFee, ValueAddedFee, VisaPhoto, BackingContractPhoto, AgencyPhoto, AgentIDPhoto, Notes, SMS, Status, Sort, Locked, OrderNumber, ID, WorkerName, PassportNumber, PassportExpiry, ContactNumber, DelegatePhoto FROM RecruitementOrder where Country = '16'AND Date<'" + d + "'";
            }
            var m = _context.RecruitementOrder.FromSqlRaw(querySQL);
            var pagedData = m.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();
            totalRecords = await m.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_RecruitementOrder>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }

        [HttpGet]
        [Route("chunksCountry/{type}")]
        public async Task<ActionResult<IEnumerable<M_RecruitementOrder>>> GetData_WRT_Country(string type, [FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            string title = HttpContext.Request.Query["title"];
            var route = Request.Path.Value + "?title=" + title;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);

            int totalRecords;
            var querySQL = "";
            if (title == "countrychanged")
            {
                querySQL = "SELECT (Select Name from RCustomer where Code = Client) as Client,OrderStatus,TimelineStatus,PaymentStatus, PaidAmount, TotalAmount, (Select Name from Country where Code = Country) as Country, (Select Name from professions where Code = Profession)as Profession, (select Name from Agents where Code = IDNumber) as IDNumber, Company_Code,  Date, VisaType, VisaNumber, VisaDate, Religion, Type, Experience, Age, Appearance, MaritalSatus, CustomerTerms, Workplace, Rworkplace, ArrivalStation, Salary, ApplicationStatus, OrderPackage, ArrivalStationID, DelegateID, AgentName, Agentsource, AgencyNumber, Agencysource, Agencydate, ContractNo, DateofContract, RecruitmentFee, AdditionsalRecruitementFee, ValueAddedFee, VisaPhoto, BackingContractPhoto, AgencyPhoto, AgentIDPhoto, Notes, SMS, Status, Sort, Locked, OrderNumber, ID, WorkerName, PassportNumber, PassportExpiry, ContactNumber, DelegatePhoto FROM RecruitementOrder where Country = '" + type + "'";
            }
            else
            {
                querySQL = "SELECT (Select Name from RCustomer where Code = Client) as Client,OrderStatus,TimelineStatus,PaymentStatus, PaidAmount, TotalAmount, (Select Name from Country where Code = Country) as Country, (Select Name from professions where Code = Profession)as Profession, (select Name from Agents where Code = IDNumber) as IDNumber, Company_Code,  Date, VisaType, VisaNumber, VisaDate, Religion, Type, Experience, Age, Appearance, MaritalSatus, CustomerTerms, Workplace, Rworkplace, ArrivalStation, Salary, ApplicationStatus, OrderPackage, ArrivalStationID, DelegateID, AgentName, Agentsource, AgencyNumber, Agencysource, Agencydate, ContractNo, DateofContract, RecruitmentFee, AdditionsalRecruitementFee, ValueAddedFee, VisaPhoto, BackingContractPhoto, AgencyPhoto, AgentIDPhoto, Notes, SMS, Status, Sort, Locked, OrderNumber, ID, WorkerName, PassportNumber, PassportExpiry, ContactNumber, DelegatePhoto FROM RecruitementOrder";

            }
            var m = _context.RecruitementOrder.FromSqlRaw(querySQL);
            var pagedData = m.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();
            totalRecords = await m.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_RecruitementOrder>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("{type}/search")]
        public async Task<ActionResult<IEnumerable<M_RecruitementOrder>>> Get3(string type, [FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            string title = HttpContext.Request.Query["title"];
            var route = Request.Path.Value + "?title=" + title;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var d = title;
            var pagedData = new List<M_RecruitementOrder>();
            int totalRecords;
            if (type == "agency")
            {
                var m = _context.RecruitementOrder.
                FromSqlRaw("SELECT (Select Name from RCustomer where Code = Client) as Client, " +
                "(Select Name from Country where Code = Country) as Country, " +
                "(Select Name from professions where Code = Profession)as Profession, " +
                "(select Name from Agents where Code = IDNumber) as IDNumber, Company_Code, " +
                " Date, VisaType, VisaNumber, VisaDate, Religion, Type, Experience, Age, " +
                "Appearance, MaritalSatus, CustomerTerms, Workplace, Rworkplace, ArrivalStation, Salary, " +
                "ApplicationStatus, OrderPackage, ArrivalStationID, DelegateID, AgentName, Agentsource, AgencyNumber," +
                " Agencysource, Agencydate, ContractNo, DateofContract, RecruitmentFee, AdditionsalRecruitementFee, " +
                "ValueAddedFee, VisaPhoto, BackingContractPhoto, AgencyPhoto, AgentIDPhoto, Notes, SMS, Status, Sort," +
                " Locked, OrderNumber, ID, WorkerName, PassportNumber, PassportExpiry, ContactNumber, DelegatePhoto FROM " +
                "RecruitementOrder where ApplicationStatus = '2' AND OrderNumber LIKE '%" + d + "%'");

                pagedData = m.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
                totalRecords = await m.CountAsync();
            }
            else if (type == "alternate")
            {
                //pagedData = await _context.RecruitementOrder.Where(m => ((m.Date < d) && (m.ApplicationStatus == "2")))
                //        .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                //        .Take(validFilter.PageSize)
                //        .ToListAsync();
                var m = _context.RecruitementOrder.
                FromSqlRaw("SELECT (Select Name from RCustomer where Code = Client) as Client, " +
                "(Select Name from Country where Code = Country) as Country, " +
                "(Select Name from professions where Code = Profession)as Profession, " +
                "(select Name from Agents where Code = IDNumber) as IDNumber, Company_Code, " +
                " Date, VisaType, VisaNumber, VisaDate, Religion, Type, Experience, Age, " +
                "Appearance, MaritalSatus, CustomerTerms, Workplace, Rworkplace, ArrivalStation, Salary, " +
                "ApplicationStatus, OrderPackage, ArrivalStationID, DelegateID, AgentName, Agentsource, AgencyNumber," +
                " Agencysource, Agencydate, ContractNo, DateofContract, RecruitmentFee, AdditionsalRecruitementFee, " +
                "ValueAddedFee, VisaPhoto, BackingContractPhoto, AgencyPhoto, AgentIDPhoto, Notes, SMS, Status, Sort," +
                " Locked, OrderNumber, ID, WorkerName, PassportNumber, PassportExpiry, ContactNumber, DelegatePhoto FROM " +
                "RecruitementOrder where ApplicationStatus = '2' AND OrderNumber LIKE '%" + d + "%'");

                pagedData = m.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
                //totalRecords = await _context.RecruitementOrder.Where(m => (m.Date < d) && (m.ApplicationStatus == "2")).CountAsync();
                totalRecords = await m.CountAsync();
            }
            else
            {
                //pagedData = await _context.RecruitementOrder.Where(m => (m.Date < d))
                //.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                //.Take(validFilter.PageSize)
                //.ToListAsync();
                var m = _context.RecruitementOrder.
               FromSqlRaw("SELECT (Select Name from RCustomer where Code = Client) as Client, " +
               "(Select Name from Country where Code = Country) as Country, " +
               "(Select Name from professions where Code = Profession)as Profession, " +
               "(select Name from Agents where Code = IDNumber) as IDNumber, Company_Code, " +
               " Date, VisaType, VisaNumber, VisaDate, Religion, Type, Experience, Age, " +
               "Appearance, MaritalSatus, CustomerTerms, Workplace, Rworkplace, ArrivalStation, Salary, " +
               "ApplicationStatus, OrderPackage, ArrivalStationID, DelegateID, AgentName, Agentsource, AgencyNumber," +
               " Agencysource, Agencydate, ContractNo, DateofContract, RecruitmentFee, AdditionsalRecruitementFee, " +
               "ValueAddedFee, VisaPhoto, BackingContractPhoto, AgencyPhoto, AgentIDPhoto, Notes, SMS, Status, Sort," +
               " Locked, OrderNumber, ID, WorkerName, PassportNumber, PassportExpiry, ContactNumber, DelegatePhoto FROM " +
               "RecruitementOrder where OrderNumber LIKE '%" + d + "%'");

                pagedData = m.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
                totalRecords = await m.CountAsync();
                //totalRecords = await _context.RecruitementOrder.Where(m => (m.Date < d)).CountAsync();
            }

            var pagedReponse = PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("topCardCounter")]
        public List<Dictionary<string, string>> GetMx()
        {
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "SELECT Distinct (SELECT COUNT(*) FROM [RecruitmentRefund]) AS Refund,(SELECT COUNT(*) FROM [RecruitmentPackages]) AS Package,(SELECT COUNT(*) FROM [RecruitementOrder] where ApplicationStatus = '2') AS Agency,(SELECT COUNT(*) FROM [RecruitmentPackages]) AS Completed,(SELECT COUNT(*) FROM [RecruitementOrder] where ApplicationStatus = '2') AS Alternate FROM RecruitementOrder";
            SqlCommand cmd = new SqlCommand(strsql, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable _dt = new DataTable();
            sda.Fill(_dt);
            //List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            //List<string> fields = new List<string>();
            //List<KeyValuePair<string, string>> kvpList = new List<KeyValuePair<string, string>>();            
            Dictionary<string, string> kvpList = new Dictionary<string, string>();
            List<Dictionary<string, string>> l = new List<Dictionary<string, string>>();
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow sqldr in _dt.Rows)
                {
                    kvpList.Add("Refund", sqldr["Refund"].ToString());
                    kvpList.Add("Package", sqldr["Package"].ToString());
                    kvpList.Add("Agency", sqldr["Agency"].ToString());
                    kvpList.Add("Completed", sqldr["Completed"].ToString());
                    kvpList.Add("Alternate", sqldr["Alternate"].ToString());
                    l.Add(kvpList);
                }
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return l;
        }


        // GET: api/Level2/5
        [HttpGet("{OrderNumber}")]
        public async Task<ActionResult<M_RecruitementOrder>> Getm(string OrderNumber)
        {
            var m = await _context.RecruitementOrder.FindAsync(OrderNumber);
            if (m == null)
            {
                return NotFound();
            }
            return m;
        }
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_RecruitementOrder m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                M_RecruitementOrder obj = new M_RecruitementOrder();
                obj = await _context.RecruitementOrder.FindAsync(m.OrderNumber);
                if (obj != null)
                {
                    // obj.Name = m.Name;

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
        public async Task<IActionResult> updateOrderStatus(M_RecruitementOrder m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                M_RecruitementOrder obj = new M_RecruitementOrder();
                obj = await _context.RecruitementOrder.FindAsync(m.OrderNumber);
                if (obj != null)
                {
                    obj.OrderStatus = m.OrderStatus;

                }
                _context.RecruitementOrder.Attach(obj).Property(x => x.OrderStatus).IsModified = true;
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
        public async Task<IActionResult> updateOrderNotes(M_RecruitementOrder m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                M_RecruitementOrder obj = new M_RecruitementOrder();
                obj = await _context.RecruitementOrder.FindAsync(m.OrderNumber);
                if (obj != null)
                {
                    obj.Notes = m.Notes;

                }
                _context.RecruitementOrder.Attach(obj).Property(x => x.Notes).IsModified = true;
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            return Ok(m);
        }
        [HttpPost]
        public async Task<ActionResult<M_RecruitementOrder>> create(M_RecruitementOrder m)
        {
            m.OrderNumber = getNext(companycode);
            m.Date = DateTime.Now;
            _context.RecruitementOrder.Add(m);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Getm", new { OrderNumber = m.OrderNumber }, m);
        }
        public string getNext(string Company_Code)
        {
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = "select  max(  cast(OrderNumber as bigint)) +1  as code from RecruitementOrder  ";

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
        public async Task<ActionResult<M_RecruitementOrder>> Deletem(long id)
        {
            var m = await _context.RecruitementOrder.FindAsync(id);
            if (m == null)
            {
                return NotFound();
            }
            _context.RecruitementOrder.Remove(m);
            await _context.SaveChangesAsync();
            return m;
        }
    }
}

