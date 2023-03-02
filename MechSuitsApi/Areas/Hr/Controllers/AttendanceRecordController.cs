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

using Microsoft.OpenApi.Any;
using CoreInfrastructure.Hr.
    Setup.Attendance;
using System.Collections.Immutable;
using ServiceStack.Script;


namespace MechSuitsApi.Areas.Hr.Controllers
{
    [Route("api/AttendanceRecord")]
    [ApiController]
    public class AttendanceRecordController : ControllerBase
    {

        private readonly IUriService uriService;
        string companycode = ""; string user = "";
        private readonly AppDBContext _context;
        D_DB dbset = null;

        private SqlConnection con;
        Connection c = new Connection();
        private IHttpContextAccessor _httpContextAccessor;
        public AttendanceRecordController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
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
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.HrAttendance_Header
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
            var totalRecords = await _context.HrAttendance_Header.CountAsync();
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
            var pagedData = await _context.HrAttendance_Header.Where(m => m.Code.Contains(title))
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
            var totalRecords = await _context.HrAttendance_Header.Where(m => m.Code.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_HrAttendance_Header>>> GetList()
        {
            var m = await _context.HrAttendance_Header.ToListAsync();
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
        public async Task<ActionResult<M_HrAttendance_Header>> Getxm(string Code)
        {
            var m = await _context.HrAttendance_Header.FindAsync(Code);
            if (m == null)
            {
                return NotFound();
            }
            m.DetailRows = dbset.GetDetailsrowsforEdit(Code);
            return m;
        }
        [HttpPost]
        public async Task<M_HrAttendance_Header> create(M_HrAttendance_Header H)
        {
            H.Code = dbset.getUpdateMasterCount();
            H.ShiftID = dbset.GetShift(H.EmployeeID);
            var currentTime = TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss"));
            M_HrAttendance_Header H1 = new M_HrAttendance_Header();
            H1.Code = H.Code;
            H1.CompanyCode = H.CompanyCode ?? "C001";
            H1.EmployeeID = H.EmployeeID ?? "";
            H1.ShiftID = H.ShiftID ?? "";
            H1.LastExitTime = H.LastExitTime ?? TimeSpan.Parse("00:00:00:000");
            H1.LastEntryTime = H.LastEntryTime ?? currentTime;
            H1.AttendanceDate = H.AttendanceDate ?? DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            H1.Notes = H.Notes ?? "NO Notes";
            H1.Status = H.Status ?? "0";
            H1.Sort = H.Sort ?? "0";
            H1.Locked = H.Locked ?? "0";
            int i = 1;
            M_HrAttendance_Detail d = new M_HrAttendance_Detail
            {
                CompanyCode = H1.CompanyCode,
                Code = H.Code,
                SeqNo = i.ToString(),
                EmployeeID = H.EmployeeID,
                AttendanceDate = H1.AttendanceDate,
                EntryTime = currentTime,
                 ExitTime = TimeSpan.Parse("00:00:00:000"),
                LateTime = TimeSpan.Parse("00:00:00:000"),
                ExtraTime = TimeSpan.Parse("00:00:00:000"),
                EarlyDismissalTime = TimeSpan.Parse("00:00:00:000"),
                Notes = "NO NOTES",
                Status = "1",
                Sort = "0",
                Locked = "0"
            };
            DateTime date = DateTime.Now;
            string fdate = date.ToString("yyyy-MM-dd 00:00:00.000");
            Console.WriteLine(fdate);
            var newObj = _context.HrAttendance_Header
                .Where(m => m.EmployeeID == H.EmployeeID && m.AttendanceDate == DateTime.Parse(fdate)).FirstOrDefault();
            var shiftDetails = dbset.GetShiftDetails(H.ShiftID);
           
            if (newObj == null)
            {
                var StartTime = TimeSpan.Parse(shiftDetails.StartTime);
                var lateTime = StartTime - currentTime;
                //Console.WriteLine(late);
                if (lateTime.TotalMinutes < 0)
                {
                    d.LateTime = lateTime.Duration();
                }
                dbset.Set_Header(1, H1);
                d.SeqNo = dbset.getUpdateSeqNoCount(H.Code);
                dbset.Set_DetailRows(1, d);
            }
            else
            {
                await updateHeaderAttendence(newObj.Code);
                var detailobject = dbset.FindRow(H.EmployeeID, fdate);
                if (detailobject.Status == "1")
                {
                    //yahan pr hona hy
                    var EndTime = TimeSpan.Parse(shiftDetails.EndTime);
                    var dynaimcTime = EndTime - currentTime;
                    if (dynaimcTime.TotalMinutes < 0)
                    {
                        d.ExtraTime = dynaimcTime.Duration();
                    }
                    else
                    {
                        d.EarlyDismissalTime = dynaimcTime.Duration();
                    }
                    dbset.UpdateDetailAttendence(detailobject.Code, detailobject.SeqNo,d.ExtraTime, d.EarlyDismissalTime);
                }
                else
                {
                    d.Code = detailobject.Code;
                    d.SeqNo = dbset.getUpdateSeqNoCount(d.Code);
                    dbset.Set_DetailRows(1, d);
                } 
            }

            return H;
        }

        [HttpPut]
        [Route("update/Attendance")]


        public async Task<IActionResult> updateHeaderAttendence(string Code)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var obj = new M_HrAttendance_Header();
                obj = await _context.HrAttendance_Header.FindAsync(Code);

                if (obj != null)
                {

                    obj.LastExitTime = TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss"));

                }
                _context.Entry(obj).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return Ok();
        }
        [HttpPut]
        [Route("update")]
        public async Task<M_HrAttendance_Header> update(M_HrAttendance_Header H)
        {
            if (H.DetailRows != null && H.DetailRows.Count > 0)
            {
                dbset.deleteData("detailrows", H.Code);
                int i = 0;
                foreach (var item in H.DetailRows)
                {
                    i++;
                    M_HrAttendance_Detail m = new M_HrAttendance_Detail
                    {
                        CompanyCode = H.CompanyCode,
                        Code = H.Code,
                        //  SEQNO = i,
                        EmployeeID = H.EmployeeID,
                        AttendanceDate = H.AttendanceDate,
                        EntryTime = item.EntryTime,
                        ExitTime = item.ExitTime,
                        LateTime = item.LateTime,
                        ExtraTime = item.ExtraTime,
                        EarlyDismissalTime = item.EarlyDismissalTime,
                        Notes = item.Notes,
                        Status = item.Status,
                        Sort = item.Sort,
                        Locked = item.Locked
                    };
                    dbset.Set_DetailRows(1, m);

                }

                M_HrAttendance_Header H1 = new M_HrAttendance_Header();
                H1.Code = H.Code;
                H1.CompanyCode = H.CompanyCode;
                H1.EmployeeID = H.EmployeeID;

                dbset.deleteData("header", H.Code);
                dbset.Set_Header(1, H1);
                return H;


            }
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
