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
using CoreInfrastructure.Recruitement.Philippine;
using D_DB = CoreInfrastructure.Recruitement.Philippine.D_DB;
using Microsoft.OpenApi.Any;
using CoreInfrastructure.Recruitement.Musaned;
using Microsoft.Graph;

namespace MechSuitsApi.Areas.Recruitement.Philippine
{
    [Route("api/transmittal")]
    [ApiController]
    // [Authorize]
    [EnableCors("AllowOrigin")]
    public class RecruitmentTransmittalController : ControllerBase
    {
        private readonly IUriService uriService;
        string companycode = ""; string user = "";
        private readonly AppDBContext _context;
        D_DB dbset = null;

        private SqlConnection con;
        Connection c = new Connection();
        private IHttpContextAccessor _httpContextAccessor;
        public RecruitmentTransmittalController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
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
            var querySQL = "SELECT Code, CompanyCode, (Select Name from Agents where Code = Agent) as Agent, TransmittalDate, Notes, Status, Sort, Locked FROM RecruitementTransmittal_Header"; 
            
            //var pagedData = await _context.RecruitementTransmittal_Header
            //        .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            //        .Take(validFilter.PageSize)
            //        .ToListAsync();
            var m = _context.RecruitementTransmittal_Header.FromSqlRaw(querySQL);
            var pagedData = await m.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToListAsync();
            var totalRecords = m.Count();
            if (pagedData != null)
            {
                for (int i = 0; i < pagedData.Count; i++)
                {
                    pagedData[i].DetailRows = dbset.GetDetailsrowsforEdit(pagedData[i].Code);
                }
            }
            //var totalRecords = await _context.RecruitementTransmittal_Header.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<AnyType>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            var route = Request.Path.Value + "?title=" + title;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.RecruitementTransmittal_Header.Where(m => m.Code.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            if (pagedData != null)
            {
                for (int i = 0; i < pagedData.Count; i++)
                {
                    pagedData[i].DetailRows = dbset.GetDetailsrowsforEdit(pagedData[i].Code);
                }
            }
            var totalRecords = await _context.RecruitementTransmittal_Header.Where(m => m.Code.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);

        }
        [HttpGet]
        [Route("printData/{id}")]
        public List<List<string>> GetMx()
        {
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "SELECT * FROM [SanedDatabase].[dbo].[RecruitementTransmittal_Detail] where Code = '10002'";
            SqlCommand cmd = new SqlCommand(strsql, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable _dt = new DataTable();
            sda.Fill(_dt);
            //List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            //List<string> fields = new List<string>();
            //List<KeyValuePair<string, string>> kvpList = new List<KeyValuePair<string, string>>();            
           
            List<List<string>> list2 = new List<List<string>>();
            if (_dt.Rows.Count > 0)
            {
                
                foreach (DataRow row in _dt.Rows)
                {
                    List<string> list = new List<string>();
                    for (int i = 0; i < _dt.Columns.Count; i++)
                    {
                        list.Add(row[i].ToString());
                    }
                    list2.Add(list);
                }
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return list2;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_RecruitementTransmittalHeader>>> GetList()
        {
            var m = await _context.RecruitementTransmittal_Header.ToListAsync();
            if (m != null)
            {
                for (int i = 0; i < m.Count; i++)
                {
                    m[i].DetailRows = dbset.GetDetailsrowsforEdit(m[i].Code);
                }
            }
            return m;
        }

        [HttpGet("{Code}")]
        public async Task<ActionResult<M_RecruitementTransmittalHeader>> Getxm(string Code)
        {
            var m = await _context.RecruitementTransmittal_Header.FindAsync(Code);
            if (m == null)
            {
                return NotFound();
            }
            m.DetailRows = dbset.GetDetailsrowsforEdit(Code);
            return m;
        }
        [HttpPost]
        public async Task<M_RecruitementTransmittalHeader> create(M_RecruitementTransmittalHeader H)
        {
            H.Code = dbset.getUpdateMasterCount();
            //Console.WriteLine(H.CODE);

            if (H.DetailRows != null && H.DetailRows.Count > 0)
            {
                int i = 0;
                foreach (var item in H.DetailRows)
                {
                    i++;
                    M_RecruitementTransmittalDetail m = new M_RecruitementTransmittalDetail
                    {
                        Code = H.Code,
                        CompanyCode = H.CompanyCode,
                        SeqNo = i,
                        OrderID = item.OrderID,
                        Remarks = item.Remarks,
                        Status = item.Status
                    };
                    dbset.Set_DetailRows(1, m);

                }
            }
            
            M_RecruitementTransmittalHeader H1 = new M_RecruitementTransmittalHeader();
            H1.Code = H.Code;
            H1.CompanyCode = H.CompanyCode;
            H1.Agent = H.Agent;
            H1.TransmittalDate = H.TransmittalDate;
            H1.Notes = H.Notes;
            H1.Status = H.Status;
            H1.Sort = H.Sort;
            H1.Locked = H.Locked;

            dbset.Set_Header(1, H1);
            return H;


        }
        [HttpPut]
        [Route("update")]

        public async Task<M_RecruitementTransmittalHeader> update(M_RecruitementTransmittalHeader H)
        {

            Console.WriteLine("first--" + H.Code);
            if (H.DetailRows != null && H.DetailRows.Count > 0)
            {
                Console.WriteLine("2nd--" + H.Code);
                dbset.deleteData("detailrows", H.Code);
                int i = 0;
                foreach (var item in H.DetailRows)
                {
                    i++;
                    M_RecruitementTransmittalDetail m = new M_RecruitementTransmittalDetail
                    {
                        Code = H.Code,
                        CompanyCode = H.CompanyCode,
                        SeqNo = i,
                        OrderID = item.OrderID,
                        Remarks = item.Remarks,
                        Status = item.Status
                    };
                    dbset.Set_DetailRows(1, m);

                }
            }
            M_RecruitementTransmittalHeader H1 = new M_RecruitementTransmittalHeader();
            H1.Code = H.Code;
            H1.CompanyCode = H.CompanyCode;
            H1.Agent = H.Agent;
            H1.TransmittalDate = H.TransmittalDate;
            H1.Notes = H.Notes;
            H1.Status = H.Status;
            H1.Sort = H.Sort;
            H1.Locked = H.Locked;
            dbset.deleteData("header", H.Code);
            dbset.Set_Header(1, H1);
            return H;


        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> Deletem(string id)
        {
            dbset.deleteData("detailrows", id);
            dbset.deleteData("header", id);

            return id;
        }


    }
}
