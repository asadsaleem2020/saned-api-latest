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
using CoreInfrastructure.ToolbarItems;
using CoreInfrastructure.Customers;

namespace MechSuitsApi.Areas.toolbarController
{
    //[Authorize]
    [Route("api/Agents")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class AgentsController : ControllerBase
    {


        private readonly IUriService uriService;
        string companycode = ""; string user = "";
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        private IHttpContextAccessor _httpContextAccessor;
        public AgentsController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context;
            con = c.V_connection;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            //companycode = currentUser.FindFirst("Company").Value.ToString();
           // user = currentUser.FindFirst("User").Value.ToString();
        }

        // GET: api/Level2
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_Agents>>> GetList()
        {
            return await _context.Agents.ToListAsync();
        }
        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_Agents>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
            // return await _context.Area.ToListAsync();
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.Agents
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.Agents.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Agents>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_Agents>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];

            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);


            var pagedData = await _context.Agents.Where(m => m.Name.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.Agents.Where(m => m.Name.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Agents>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }
        // GET: api/Level2/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_Agents>> Getm(Int64 id)
        {

            var m = await _context.Agents.FindAsync(id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }
        [HttpGet("view/{id}")]
        public ActionResult<M_Agents> Getv(Int64 id)
        {
            var sql = "SELECT ID, Company_Code, Code, Name, RName, EMAIL, ID_Number, (Select Name From Country where Code=Country) as Country, Password, Address, " +
                "RAddress, city, Rcity, License, mobile, Phone, fax, SendingBank, responsibleName, AccountNumber, " +
                "CellPhone, licenseHolder, Homephone, Notes, Status, Sort, Locked FROM Agents where ID=" + id;
            var m = _context.Agents.FromSqlRaw(sql);

            if (m == null)
            {
                return NotFound();
            }

            return m.First();
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_Agents m)

        {
            Console.WriteLine(m.ID);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_Agents obj = new M_Agents();
                obj = await _context.Agents.FindAsync(m.ID);

                if (obj != null)
                {
                    obj.Name = m.Name;
                    obj.EMAIL = m.EMAIL;
                    obj.RName = m.RName;
                    obj.Address = m.Address;
                    obj.RAddress = m.RAddress;
                    obj.Country= m.Country;
                    obj.Phone = m.Phone;
                    obj.fax = m.fax;
                    obj.License = m.License;
                    obj.Homephone = m.Homephone;
                    obj.city = m.city;
                    obj.Rcity= m.city;  
                    obj.Password=m.Password;
                    obj.SendingBank = m.SendingBank;
                    obj.mobile = m.mobile;  
                    obj.CellPhone = m.CellPhone;
                    obj.Homephone = m.Homephone;
                    obj.licenseHolder = m.licenseHolder;
                    obj.ID_Number = m.ID_Number;    
                    obj.Status = m.Status;
                    obj.Locked = m.Locked;
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
        [HttpPost]
        public async Task<ActionResult<M_Agents>> create(M_Agents m)
        {
            m.Code = getNext(companycode);
            _context.Agents.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getm", new { id = m.ID }, m);
        }
        public string getNext(string Company_Code)
        { 
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = @"SELECT ISNULL(MAX( CAST(CODE AS BIGINT  ))  ,1000) +1  AS code FROM Agents";
            //strsql = "select  max(  cast(code as bigint)) +1  as code from Agents where    Company_Code='" + Company_Code + "' ";

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
        public async Task<ActionResult<M_Agents>> Deletem(Int64 id)
        {
            var m = await _context.Agents.FindAsync(id);
            if (m == null)
            {
                return NotFound();
            }
            _context.Agents.Remove(m);
            await _context.SaveChangesAsync();
            return m;
        }

    }
}
