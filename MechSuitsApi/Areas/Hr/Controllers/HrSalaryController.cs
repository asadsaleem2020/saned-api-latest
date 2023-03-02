using CoreInfrastructure.Hr.Setup;
using Executer;
using MechSuitsApi.Classes;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MechSuitsApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
 

namespace MechSuitsApi.Areas.Hr.Controllers
{
    [Route("api/HrSalary")]
    [ApiController]
    public class HrSalaryController : ControllerBase
    {

        private readonly IUriService uriService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AppDBContext _context;
        private SqlConnection con;
        string companycode = "";
        string user = "";
        Connection c = new Connection();
        public HrSalaryController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            //   companycode = currentUser.FindFirst("Company").Value.ToString();
            //   user = currentUser.FindFirst("User").Value.ToString();

             con = c.V_connection;
        }


       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_HrSalary>>> GetAS_Acclevel2()
        {
            

            return await _context.HrSalary.ToListAsync();
        }




        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_HrSalary>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
             
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.HrSalary
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.HrSalary.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_HrSalary>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_HrSalary>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            var route = Request.Path.Value + "?title=" + title;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.HrSalary.Where(m => m.Code.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.HrSalary.Where(m => m.Code.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_HrSalary>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);

        }





      
        [HttpGet("{id}")]
        public async Task<ActionResult<M_HrSalary>> GetM_Level2(String id)
        {
            var m_HrSalary = await _context.HrSalary.FindAsync(id);

            if (m_HrSalary == null)
            {
                return NotFound();
            }

            return m_HrSalary;
        }
        [HttpGet]
        [Route("GetByEmployee/{id}")]
        public async Task<ActionResult<IEnumerable<M_HrSalary>>> GetByEmployee(string id)
        {

            var m_Level2 = await _context.HrSalary.Where(x => x.EmployeeCode == id).ToListAsync();

            if (m_Level2 == null)
            {
                return NotFound();
            }

            return m_Level2;
        }

       
        [HttpPut]
        [Route("update")]

       
        public async Task<IActionResult> update(M_HrSalary m_Level2)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var obj = new M_HrSalary();
                obj = await _context.HrSalary.FindAsync(m_Level2.Code);

                if (obj != null)
                {
                    //obj.Company_Code = m_Level2.Company_Code;
                    //obj.Code = m_Level2.Code;
                    //obj.Name = m_Level2.Name;
                    //obj.FH_Name = m_Level2.FH_Name;
                    //obj.Gender = m_Level2.Gender;
                    //obj.NIC = m_Level2.NIC;
                    //obj.DOB = m_Level2.DOB;
                    //obj.MartialStatus = m_Level2.MartialStatus;
                    //obj.Caste = m_Level2.Caste;
                    //obj.Religion = m_Level2.Religion;
                    //obj.Blood = m_Level2.Blood;
                    //obj.Mobile = m_Level2.Mobile;
                    //obj.Email = m_Level2.Email;
                    //obj.Country = m_Level2.Country;
                    //obj.P_Address = m_Level2.P_Address;
                    //obj.C_Address = m_Level2.C_Address;
                    //obj.Designation = m_Level2.Designation;
                    //obj.Experties = m_Level2.Experties;
                    //obj.Office = m_Level2.Office;
                    //obj.Department = m_Level2.Department;
                    //obj.DOJ = m_Level2.DOJ;
                    //obj.Contract_Duration = m_Level2.Contract_Duration;
                    //obj.Type_ID = m_Level2.Type_ID;
                    //obj.Emloyee_Status = m_Level2.Emloyee_Status;
                    //obj.Working_Period = m_Level2.Working_Period;
                    //obj.Ref_By = m_Level2.Ref_By;
                    //obj.Salary = m_Level2.Salary;
                    //obj.Allowance = m_Level2.Allowance;
                    //obj.Insurance = m_Level2.Insurance;
                    //obj.DiscountRate = m_Level2.DiscountRate;
                    //obj.House_Rent = m_Level2.House_Rent;
                    //obj.Residence_Expiry = m_Level2.Residence_Expiry;
                    //obj.Med_Allowance = m_Level2.Med_Allowance;
                    //obj.Med_Expiry = m_Level2.Med_Expiry;
                    //obj.Other_Deduction = m_Level2.Other_Deduction;
                    //obj.Passport_Number = m_Level2.Passport_Number;
                    //obj.Passport_Expiry = m_Level2.Passport_Expiry;
                    //obj.IqamaNumber = m_Level2.IqamaNumber;
                    //obj.Frontier_Number = m_Level2.Frontier_Number;
                    //obj.VacationStart = m_Level2.VacationStart;
                    //obj.VacationEnd = m_Level2.VacationEnd;
                    //obj.ArrivalDate = m_Level2.ArrivalDate;
                    //obj.Emerg_Name = m_Level2.Emerg_Name;
                    //obj.Emerg_Phone = m_Level2.Emerg_Phone;
                    //obj.Emerg_Address = m_Level2.Emerg_Address;
                    //obj.Attendance_Password = m_Level2.Attendance_Password;
                    //obj.CreatedOn = m_Level2.CreatedOn;
                    //obj.CreatedBy = m_Level2.CreatedBy;
                    //obj.DeletedOn = m_Level2.DeletedOn;
                    //obj.DeletedBy = m_Level2.DeletedBy;
                    //obj.UpdatedOn = m_Level2.UpdatedOn;
                    //obj.UpdatedBy = m_Level2.UpdatedBy;
                    //obj.Image = m_Level2.Image;
                    //obj.Notes = m_Level2.Notes;
                    //obj.Status = m_Level2.Status;
                    //obj.Sort = m_Level2.Sort;
                    //obj.Locked = m_Level2.Locked;

                }
                // int i = this.obj.SaveChanges();
                _context.Entry(obj).State = EntityState.Modified;

                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {
                throw;
            }
            return Ok(m_Level2);
        }
         
    
       

        [HttpPost]
        public async Task<ActionResult<M_HrSalary>> create(M_HrSalary SalaryModel)
        {
            
            SalaryModel.Code = getUpdateMasterCount();
            SalaryModel.Date = SalaryModel.Date ?? DateTime.Now;
            _context.HrSalary.Add(SalaryModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetM_Level2", new { id = SalaryModel.Code }, SalaryModel);
        }
        public string getUpdateMasterCount()
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            string no = "";
             
            strsql = @"SELECT ISNULL(MAX( CAST(Code AS BIGINT  ))  ,1000) +1  AS Code FROM HrSalary";

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            sqldr = dp.ExecuteReader(con, strsql);
            if (sqldr.Read())
            {
                no = sqldr["Code"].ToString().Trim();
            }
            sqldr.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (no.Trim() == "") no = "1000";
            return no;
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_HrSalary>> Deletem(String id)
        {
            var m = await _context.HrSalary.FindAsync(id);
            if (m == null)
            {
                return NotFound();
            }
            _context.HrSalary.Remove(m);
            await _context.SaveChangesAsync();
            return m;
        }

       
    }
}