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

namespace MechSuitsApi.Areas.toolbarController
{
    // [Authorize]
    [Route("api/FollowUps")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class FollowUpsController : ControllerBase
    {



        private readonly IUriService uriService;
        string companycode = ""; string user = "";
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        private IHttpContextAccessor _httpContextAccessor;
        public FollowUpsController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context;
            con = c.V_connection;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            //companycode = currentUser.FindFirst("Company").Value.ToString();
         //   user = currentUser.FindFirst("User").Value.ToString();
        }

        // GET: api/Level2
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_FollowUps>>> GetList()
        {
            return await _context.FollowUps.ToListAsync();
        }
        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_FollowUps>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var m = _context.FollowUps.
          FromSqlRaw("SELECT ID,Company_Code,OrderNumber,RequestTypeID,Date, " +
          "(select Name from APP_USERS where CODE=EmployeeID) as EmployeeID     " +
          "   ,(select Name from RCustomer where CODE= client) as client     " +
          "   ,mobile        ,OrderDetails        ,Photo        ,Status    " +
          "    ,Sort        ,Locked    FROM  FollowUps").ToList();

            var pagedData = m
                        .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                        .Take(validFilter.PageSize)
                        .ToList();

            var totalRecords = await _context.FollowUps.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_FollowUps>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_FollowUps>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];

            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);


            var pagedData = await _context.FollowUps.Where(m => m.OrderNumber.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.FollowUps.Where(m => m.OrderNumber.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_FollowUps>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }
        // GET: api/Level2/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_FollowUps>> Getm(Int64 id)
        {
            var m = await _context.FollowUps.FindAsync(id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_FollowUps m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_FollowUps obj = new M_FollowUps();
                obj = await _context.FollowUps.FindAsync(m.ID);

                if (obj != null)
                {
                    // obj.Name = m.Name;
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
        [HttpPost]
        public async Task<ActionResult<M_FollowUps>> create(M_FollowUps m)
        {
            m.OrderNumber = getNext(companycode);

            _context.FollowUps.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getm", new { id = m.ID }, m);
        }
        public string getNext(string Company_Code)
        {
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = "select  max(  cast(OrderNumber as bigint)) +1  as code from FollowUps ";

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
        public async Task<ActionResult<M_FollowUps>> Deletem(Int64 id)
        {
            var m = await _context.FollowUps.FindAsync(id);
            if (m == null)
            {
                return NotFound();
            }
            _context.FollowUps.Remove(m);
            await _context.SaveChangesAsync();
            return m;
        }
    }
}
