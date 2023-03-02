using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreInfrastructure.Customers;
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
using CoreInfrastructure.ToolbarItems;
using System.IO;
using System.Net.Http.Headers;

namespace MechSuitsApi.Areas.GeneralSetting.Controllers
{
    // [Authorize]
    [Route("api/RabitaCustomers")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class RCustomerController : ControllerBase
    {
        private readonly IUriService uriService;
        string companycode = "C001"; string user = "";
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        private IHttpContextAccessor _httpContextAccessor;
        public RCustomerController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
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
        public async Task<ActionResult<IEnumerable<M_RCustomer>>> GetList()
        {
            return await _context.RCustomer.ToListAsync();
        }
        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_RCustomer>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.RCustomer
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.RCustomer.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_RCustomer>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_RCustomer>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];

            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);


            var pagedData = await _context.RCustomer.Where(m => m.Name.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.RCustomer.Where(m => m.Name.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_RCustomer>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }
        // GET: api/Level2/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_RCustomer>> Getm(Int64 id)
        {

            var m = await _context.RCustomer.FindAsync(id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }
        [HttpGet("view/{id}")]
        public ActionResult<M_RCustomer> Getv(Int64 id)
        {
            var sql = "SELECT ID,Company_Code,Code,Name,RName,ID_Number,mobile,mobile2,CITY," +
                "RCITY,STATUS,STREET,RSTREET,Neighborhood,District,EMAIL,DOB,Category,FamilyMemebers," +
                "ChildUnderFiveYEars,LivingType,Rooms,Photo_ID,OtherDocument,Source,Office_Know_how," +
                "Locked,Sort,ADDRESS,AgentCode,MARITAL_STATUS,(Select Name From Delegates where Code=DelegateID) as DelegateID FROM RCustomer where ID=" + id;
            var m = _context.RCustomer.FromSqlRaw(sql);

            if (m == null)
            {
                return NotFound();
            }

            return m.First();
        }
        

        [HttpPost, DisableRequestSizeLimit]
        [Route("upload")]
        public async Task<IActionResult> Upload()
        {
            Console.WriteLine("Always be much pretty");
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();

               
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    long time = DateTime.Now.Ticks;
                    // DateTime dateValue = DateTime.Parse(time);

                    var fileName = time.ToString() +  ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');;
                    //ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    Console.WriteLine(dbPath);
                    return Ok(new { fileName });
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

        [HttpPost]
        public async Task<ActionResult<M_RCustomer>> create(M_RCustomer m)
        {
            Console.WriteLine("Entering in Create");

            m.Code = getNext(companycode);
            //Console.WriteLine("ALL is WELL"+m.Code);
            _context.RCustomer.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getm", new { id = m.ID }, m);
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_RCustomer m)
        {
            Console.WriteLine("Entering in  Update Function");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_RCustomer obj = new M_RCustomer();
                obj = await _context.RCustomer.FindAsync(m.ID);

                if (obj != null)
                {

                    obj.Name = m.Name;
                    obj.RName = m.RName;

                    obj.AgentCode = m.AgentCode;
                    obj.ID_Number = m.ID_Number;
                    obj.mobile = m.mobile;
                    obj.mobile2 = m.mobile2;
                    obj.MARITAL_STATUS = m.MARITAL_STATUS;
                    obj.DelegateID = m.DelegateID;
                    obj.CITY = m.CITY;
                    obj.RCITY = m.CITY;
                    obj.STATUS = m.STATUS;
                    obj.STREET = m.STREET;
                    obj.RSTREET = m.RSTREET;
                    obj.Neighborhood = m.Neighborhood;
                    obj.District = m.District;
                    obj.EMAIL = m.EMAIL;
                    obj.DOB = m.DOB;
                    obj.Category = m.Category;
                    obj.FamilyMemebers = m.FamilyMemebers;
                    obj.ChildUnderFiveYEars = m.ChildUnderFiveYEars;
                    obj.LivingType = m.LivingType;
                    obj.Rooms = m.Rooms;
                    obj.Photo_ID = m.Photo_ID;
                    obj.OtherDocument = m.OtherDocument;
                    obj.Source = m.Source;
                    Console.WriteLine(obj.Source);
                    obj.Office_Know_how = m.Office_Know_how;
                    obj.Locked = m.Locked;
                    obj.Sort = m.Sort;
                    obj.ADDRESS = m.ADDRESS;
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

        //[HttpPost]
        //public async Task<ActionResult<M_Agents>> create(M_Agents m)
        //{
        //    m.Code = getNext(companycode);
        //    _context.Agents.Add(m);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("Getm", new { id = m.ID }, m);
        //}


        public string getNext(string Company_Code)
        {
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = @"SELECT ISNULL(MAX( CAST(CODE AS BIGINT  ))  ,1000) +1  AS code FROM RCustomer";
            //  strsql = "select  max(  cast(code as bigint)) +1  as code from RCustomer where    Company_Code='" + Company_Code + "' ";

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
        public async Task<ActionResult<M_RCustomer>> Deletem(Int64 id)
        {
            var m = await _context.RCustomer.FindAsync(id);
            if (m == null)
            {
                return NotFound();
            }
            _context.RCustomer.Remove(m);
            await _context.SaveChangesAsync();
            return m;
        }

    }
}
