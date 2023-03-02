//using Microsoft.AspNetCore.Cors;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using CoreInfrastructure.Purchase.PurchaseInvoice;
//using System.Security.Claims;
//using MechSuitsApi.Classes;
//using Microsoft.EntityFrameworkCore;
//using CoreInfrastructure.Generals;
//using MechSuitsApi.Interfaces;
//using Microsoft.EntityFrameworkCore;
//using CoreInfrastructure.Recruitement.Musaned;
//using D_DB = CoreInfrastructure.Recruitement.Musaned.D_DB;
//using System.Data.SqlClient;

//namespace MechSuitsApi.Areas.Recruitement
//{
//    [Route("api/musaned")]
//    [ApiController]
//    // [Authorize]
//    [EnableCors("AllowOrigin")] 
//    public class RecruitmentMusanedController : ControllerBase
//    {
//        //private readonly IUriService uriService;
//        //private IHttpContextAccessor _httpContextAccessor;

//        //string companycode = "";
//        //string user = "";
//        //Connection c = null;
//        //GeneralFunctions general = null;
//        //private readonly AppDBContext _context;

//        //public RecruitmentMusanedController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
//        //{
//        //    this.uriService = uriService;
//        //    c = new Connection();
//        //   // dbset = new D_DB(c.V_connection);
//        //    _httpContextAccessor = httpContextAccessor;
//        //    var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
//        //    //companycode = currentUser.FindFirst("Company").Value.ToString();
//        //    //user = currentUser.FindFirst("User").Value.ToString();
//        //    _context = context;
//        //    //companycode = "C001";
//        //    //user = "Tester";
//        //}
//        private readonly IUriService uriService;
//        string companycode = ""; string user = "";
//        private readonly AppDBContext _context;
//        private SqlConnection con;
//        Connection c = new Connection();
//        private IHttpContextAccessor _httpContextAccessor;
//        public RecruitmentMusanedController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
//        {
//            this.uriService = uriService;
//            _context = context;
//            con = c.V_connection;
//            _httpContextAccessor = httpContextAccessor;
//            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
//            //   companycode = currentUser.FindFirst("Company").Value.ToString();
//            //   user = currentUser.FindFirst("User").Value.ToString();
//        }

//        [HttpGet]
//     //public async Task<ActionResult<IEnumerable<M_RecruitementMusanedHeader>>> Get()

//     //   {


//     //       return await _context.RecruitementMusaned_Header.ToListAsync();
//     //     // return Ok(  await _context.RecruitementMusaned_Header.ToListAsync());

//     //   }
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<M_RecruitementMusanedHeader>>> GetList()
//        {
//            return await _context.RecruitementMusaned_Header.ToListAsync();
//        }

//        [HttpGet("{id}")]
//        public async Task<ActionResult<M_RecruitementMusanedHeader>> Get(string id)
//        {

//            var m = await _context.RecruitementMusaned_Header.FindAsync(  id);

//            if (m == null)
//            {
//                return NotFound();
//            }

//            return m;
//        }
//    }
//}


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
using CoreInfrastructure.Recruitement;
using System.Globalization;
using System.Security.Cryptography;
using CoreInfrastructure.Hr.Setup; 
using Microsoft.CodeAnalysis;
using Microsoft.Graph;
using static Azure.Core.HttpHeader;
using static ServiceStack.Diagnostics.Events;
using System.Diagnostics.Tracing;
using System.Net.Sockets;
using CoreInfrastructure.Recruitement.Musaned;
using Microsoft.OpenApi.Any;

namespace MechSuitsApi.Areas.Recruitement
{
    [Route("api/musaned")]
    [ApiController]
    // [Authorize]
    [EnableCors("AllowOrigin")]
    public class RecruitmentMusanedController : ControllerBase
    {
        private readonly IUriService uriService;
        string companycode = ""; string user = "";
        private readonly AppDBContext _context;
        D_DB dbset = null;
      
        private SqlConnection con;
        Connection c = new Connection();
        private IHttpContextAccessor _httpContextAccessor;
        public RecruitmentMusanedController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context; dbset = new D_DB(c.V_connection);
            con = c.V_connection;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            //   companycode = currentUser.FindFirst("Company").Value.ToString();
            //   user = currentUser.FindFirst("User").Value.ToString();
        }
        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<AnyType>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.RecruitementMusaned_Header
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            if (pagedData != null)
            {
                for (int i = 0; i < pagedData.Count; i++)
                {
                    pagedData[i].DETAILROWS = dbset.GetDetailsrowsforEdit(pagedData[i].CODE);
                }
            }
            var totalRecords = await _context.RecruitementMusaned_Header.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<AnyType>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            // return await _context.RecruitementMusaned_Header.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.RecruitementMusaned_Header.ToListAsync();

            var pagedData = await _context.RecruitementMusaned_Header.Where(m => m.CODE.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            if (pagedData != null)
            {
                for (int i = 0; i < pagedData.Count; i++)
                {
                    pagedData[i].DETAILROWS = dbset.GetDetailsrowsforEdit(pagedData[i].CODE);
                }
            }
            var totalRecords = await _context.RecruitementMusaned_Header.Where(m => m.CODE.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);

        }
        // GET: api/Level2
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_RecruitementMusanedHeader>>> GetList()
        {
            var m= await _context.RecruitementMusaned_Header.ToListAsync();
            if (m != null)
            {
                for(int i = 0; i < m.Count; i++)
                {
                    m[i].DETAILROWS = dbset.GetDetailsrowsforEdit(m[i].CODE);
                }
            }
            return m;
        }

        [HttpGet("{CODE}")]
        public async Task<ActionResult<M_RecruitementMusanedHeader>> Getxm(string CODE)
        {
            var m = await _context.RecruitementMusaned_Header.FindAsync(CODE);
            if (m == null)
            {
                return NotFound();
            }
            m.DETAILROWS = dbset.GetDetailsrowsforEdit(CODE);
            return m;
        }
        [HttpPost]
        public async Task<M_RecruitementMusanedHeader> create(M_RecruitementMusanedHeader H)
        {
            H.CODE = dbset.getUpdateMasterCount();
            //Console.WriteLine(H.CODE);

            if (H.DETAILROWS != null && H.DETAILROWS.Count > 0)
            {
                int i = 0;
                foreach (var item in H.DETAILROWS)
                {
                    i++;
                    M_RecruitementMusanedDetails m = new M_RecruitementMusanedDetails
                    {
                        COMPANY_CODE = H.COMPANY_CODE,
                        CODE = H.CODE,
                        SEQNO = i,
                        CONTRACT = item.CONTRACT,
                        STATUS = item.STATUS,
                        REMARKS = item.REMARKS
                    };
                    dbset.Set_DetailRows(1, m);

                }
            }
            M_RecruitementMusanedHeader H1 = new M_RecruitementMusanedHeader();
            H1.CODE = H.CODE;
            H1.COMPANY_CODE = H.COMPANY_CODE;
            H1.BANK = H.BANK;
            H1.AMOUNT = H.AMOUNT;
            H1.PAYMENTDATE = H.PAYMENTDATE;
            H1.NOTES = H.NOTES;
            H1.STATUS = H.STATUS;
            H1.SORT = H.SORT;
            H1.LOCKED = H.LOCKED;

            dbset.Set_Header(1, H1);
            return H;


        }
        [HttpPut]
        [Route("update")]

        public async Task<M_RecruitementMusanedHeader> update(M_RecruitementMusanedHeader H)
        {
            
            if (H.DETAILROWS != null && H.DETAILROWS.Count > 0)
            {
                dbset.deleteData("detailrows", H.CODE);
                int i = 0;
                foreach (var item in H.DETAILROWS)
                {
                    i++;
                    M_RecruitementMusanedDetails m = new M_RecruitementMusanedDetails
                    {
                        COMPANY_CODE = H.COMPANY_CODE,
                        CODE = H.CODE,
                        SEQNO = i,
                        CONTRACT = item.CONTRACT,
                        STATUS = item.STATUS,
                        REMARKS = item.REMARKS
                    };
                    dbset.Set_DetailRows(1, m);

                }
            }
            M_RecruitementMusanedHeader H1 = new M_RecruitementMusanedHeader();
            H1.CODE = H.CODE;
            H1.COMPANY_CODE = H.COMPANY_CODE;
            H1.BANK = H.BANK;
            H1.AMOUNT = H.AMOUNT;
            H1.PAYMENTDATE = H.PAYMENTDATE;
            H1.NOTES = H.NOTES;
            H1.STATUS = H.STATUS;
            H1.SORT = H.SORT;
            H1.LOCKED = H.LOCKED;
            dbset.deleteData("header", H.CODE);
            dbset.Set_Header(1, H1);
            return H;


        }
        //// DELETE: api/Level2/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> Deletem(string id)
        {
            dbset.deleteData("detailrows", id);
            dbset.deleteData("header", id);

            return id;
        }
    }
}

