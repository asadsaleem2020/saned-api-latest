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
using CoreInfrastructure.ToolbarItems;

namespace MechSuitsApi.Areas.Hr.Controllers
{
 //   [Authorize]
    [Route("api/Employee_Profile")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class EmployeeProfileController : Controller
    {
        private readonly IUriService uriService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AppDBContext _context;
        private SqlConnection con;
        string companycode = "";   
        string user = "";
        Connection c = new Connection();
        public EmployeeProfileController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;


         //   companycode = currentUser.FindFirst("Company").Value.ToString();
         //   user = currentUser.FindFirst("User").Value.ToString();


           


            con = c.V_connection;
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
                    return Ok(new { filename });
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
        public void updatepicturepath( string id , string imgname)
        {
            SqlCommand dp = new SqlCommand();
            
            string strsql;
            string no = "";
            //MAX( CAST(CODE AS BIGINT  ))
            strsql = @"UPDATE HR_EMPLOYEE SET Image='" +imgname+ "' where EMPLOYEE_CODE='"+id+"'";

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            dp = new SqlCommand(strsql, con);
            dp.ExecuteNonQuery();

            
            
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
           
           
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_EmployeeProfile>>> GetAS_Acclevel2()
        {
       //     Console.WriteLine("Employees profile get method");
            
            return await _context.HR_EMPLOYEE.ToListAsync();
        }




        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_EmployeeProfile>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.HR_EMPLOYEE
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.HR_EMPLOYEE.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_EmployeeProfile>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_EmployeeProfile>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            // return await _context.Area.Where(m => m.Name.Contains(title)).Take(10).ToListAsync();
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.HR_EMPLOYEE.Where(m => m.Name.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.HR_EMPLOYEE.Where(m => m.Name.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_EmployeeProfile>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);

        }





        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_EmployeeProfile>> GetM_Level2(String id)
        {
            var m_Level2 = await _context.HR_EMPLOYEE.FindAsync(id);

            if (m_Level2 == null)
            {
                return NotFound();
            }

            return m_Level2;
        }
        [HttpGet]
        [Route("GetLevel2WithLevel1")]
        public async Task<ActionResult<IEnumerable<M_EmployeeProfile>>> GetLevel2WithLevel1(string id)
        {

            var m_Level2 = await _context.HR_EMPLOYEE.Where(x => x.Code == id).ToListAsync();

            if (m_Level2 == null)
            {
                return NotFound();
            }

            return m_Level2;
        }

        // PUT: api/Departments/5

        //  [HttpPut("{id}")]
        [HttpPut]
        [Route("update")]

        //   public IHttpActionResult PutEmaployeeMaster(EmployeeDetail employee)
        public async Task<IActionResult> update(M_EmployeeProfile m_Level2)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var obj = new M_EmployeeProfile();
                obj = await _context.HR_EMPLOYEE.FindAsync(m_Level2.Code);

                if (obj != null)
                {
                    obj.Company_Code = m_Level2.Company_Code;
                    obj.Code = m_Level2.Code;
                    obj.Name = m_Level2.Name;
                    obj.FH_Name = m_Level2.FH_Name;
                    obj.Gender = m_Level2.Gender;
                    obj.NIC = m_Level2.NIC;
                    obj.DOB = m_Level2.DOB;
                    obj.MartialStatus = m_Level2.MartialStatus;
                    obj.Caste = m_Level2.Caste;
                    obj.Religion = m_Level2.Religion;
                    obj.Blood = m_Level2.Blood;
                    obj.Mobile = m_Level2.Mobile;
                    obj.Email = m_Level2.Email;
                    obj.Country = m_Level2.Country;
                    obj.P_Address = m_Level2.P_Address;
                    obj.C_Address = m_Level2.C_Address;
                    obj.Designation = m_Level2.Designation;
                    obj.Experties = m_Level2.Experties;
                    obj.Office = m_Level2.Office;
                    obj.Department = m_Level2.Department;
                    obj.ShiftId=m_Level2.ShiftId;   
                    obj.DOJ = m_Level2.DOJ;
                    obj.Contract_Duration = m_Level2.Contract_Duration;
                    obj.Type_ID = m_Level2.Type_ID;
                    obj.Emloyee_Status = m_Level2.Emloyee_Status;
                    obj.Working_Period = m_Level2.Working_Period;
                    obj.Ref_By = m_Level2.Ref_By;
                    obj.Salary = m_Level2.Salary;
                    obj.Allowance = m_Level2.Allowance;
                    obj.Insurance = m_Level2.Insurance;
                    obj.DiscountRate = m_Level2.DiscountRate;
                    obj.House_Rent = m_Level2.House_Rent;
                    obj.Residence_Expiry = m_Level2.Residence_Expiry;
                    obj.Med_Allowance = m_Level2.Med_Allowance;
                    obj.Med_Expiry = m_Level2.Med_Expiry;
                    obj.Other_Deduction = m_Level2.Other_Deduction;
                    obj.Passport_Number = m_Level2.Passport_Number;
                    obj.Passport_Expiry = m_Level2.Passport_Expiry;
                    obj.IqamaNumber = m_Level2.IqamaNumber;
                    obj.Frontier_Number = m_Level2.Frontier_Number;
                    obj.VacationStart = m_Level2.VacationStart;
                    obj.VacationEnd = m_Level2.VacationEnd;
                    obj.ArrivalDate = m_Level2.ArrivalDate;
                    obj.Emerg_Name = m_Level2.Emerg_Name;
                    obj.Emerg_Phone = m_Level2.Emerg_Phone;
                    obj.Emerg_Address = m_Level2.Emerg_Address;
                    obj.Attendance_Password = m_Level2.Attendance_Password;
                    obj.CreatedOn = m_Level2.CreatedOn;
                    obj.CreatedBy = m_Level2.CreatedBy;
                    obj.DeletedOn = m_Level2.DeletedOn;
                    obj.DeletedBy = m_Level2.DeletedBy;
                    obj.UpdatedOn = m_Level2.UpdatedOn;
                    obj.UpdatedBy = m_Level2.UpdatedBy;
                    obj.Image = m_Level2.Image;
                    obj.Notes = m_Level2.Notes;
                    obj.Status = m_Level2.Status;
                    obj.Sort = m_Level2.Sort;
                    obj.Locked = m_Level2.Locked;

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

        [HttpPut("{id}")]
        //public async Task<IActionResult> PutM_Level2(string id, M_Level2 m_Level2)
        //{
        //    if (id != m_Level2.Code)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(m_Level2).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        //if (!M_Level2Exists(id))
        //        //{
        //        //    return NotFound();
        //        //}
        //        //else
        //        //{
        //        //    throw;
        //        //}
        //    }

        //    return NoContent();
        //}

        // POST: api/Departments
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpGet("view/{id}")]
        public ActionResult<M_EmployeeProfile> Getv(String id)
        {
            var sql = "SELECT ID, Company_Code, Code,ShiftId, Name, FH_Name, Gender, NIC, DOB, " +
                "MartialStatus, Caste, Religion, Blood, Mobile, Email, (Select Name From Country where Code=Country) as Country, P_Address, " +
                "C_Address, Designation, Experties, Office, Department, DOJ, Contract_Duration, " +
                "Type_ID, Emloyee_Status, Working_Period, Ref_By, Salary, Allowance, Insurance, " +
                "DiscountRate, House_Rent, Residence_Expiry, Med_Allowance, Med_Expiry, " +
                "Other_Deduction, Passport_Number, Passport_Expiry, IqamaNumber, Frontier_Number, " +
                "VacationStart, VacationEnd, ArrivalDate, Emerg_Name, Emerg_Phone, Emerg_Address, " +
                "Attendance_Password, CreatedOn, CreatedBy, DeletedOn, DeletedBy, UpdatedOn, " +
                "UpdatedBy, Image, Notes, Status, Sort, Locked FROM HR_EMPLOYEE where Code=" + id;
            var m = _context.HR_EMPLOYEE.FromSqlRaw(sql);

            //if (m == null)
            //{
            //    return NotFound();
            //}

            //return m.First();
            return m == null ? NotFound() : m.First();
        }
        
        [HttpPost]
        public async Task<ActionResult<M_EmployeeProfile>> create(M_EmployeeProfile m_Level2)
        {
         //   Console.WriteLine(m_Level2.ROLE_ID + "is ROLE ID");
            m_Level2.Code = getUpdateMasterCount();
            m_Level2.ID = int.Parse(m_Level2.Code)-1000;
            _context.HR_EMPLOYEE.Add(m_Level2);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetM_Level2", new { id = m_Level2.Code }, m_Level2);
        }
        public string getUpdateMasterCount()
        {
            DataProcess dp = new DataProcess();
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            //MAX( CAST(CODE AS BIGINT  ))
            strsql = @"SELECT ISNULL(MAX( CAST(CODE AS BIGINT  ))  ,1000) +1  AS code FROM HR_EMPLOYEE";

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            sqldr = dp.ExecuteReader(con, strsql);
            if (sqldr.Read())
            {
                no = sqldr["code"].ToString().Trim();
            }
            sqldr.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (no.Trim() == "") no = "1000";
            return no;
        }
        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_EmployeeProfile>> Deletem(String id)
        {
            var m = await _context.HR_EMPLOYEE.FindAsync(id);
            if (m == null)
            {
                return NotFound();
            }
            _context.HR_EMPLOYEE.Remove(m);
            await _context.SaveChangesAsync();
            return m;
        }

        //private bool M_Level2Exists(int id)
        //{
        //    return _context.AS_Acclevel2.Any(e => e.ID == id);
        //}
    }
}
