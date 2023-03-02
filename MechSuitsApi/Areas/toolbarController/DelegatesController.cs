using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreInfrastructure.toolbarItems;
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

namespace MechSuitsApi.Areas.toolbarController
{// [Authorize]
    [Route("api/Delegates")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class DelegatesController : ControllerBase
    {
        private readonly IUriService uriService;
        string companycode = ""; string user = "";
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        private IHttpContextAccessor _httpContextAccessor;
        public DelegatesController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
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
        public async Task<ActionResult<IEnumerable<M_Delegates>>> GetList()
        {
            return await _context.Delegates.ToListAsync();
        }
        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_Delegates>>> Get1([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {
           
            var route = Request.Path.Value;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.Delegates
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.Delegates.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Delegates>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_Delegates>>> Get3([FromQuery] CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            var route = Request.Path.Value + "?title=" + title;
            var validFilter = new CoreInfrastructure.ItemInformation.ItemInformation.PaginationFilter(filter.PageNumber, filter.PageSize);


            var pagedData = await _context.Delegates.Where(m => m.Name.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.Delegates.Where(m => m.Name.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_Delegates>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<M_Delegates>> Getm(Int64 id)
        {
            var m = await _context.Delegates.FindAsync(id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_Delegates m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_Delegates obj = new M_Delegates();
                obj = await _context.Delegates.FindAsync(m.ID);

                if (obj != null)
                {
                    obj.Name = m.Name;
                    obj.mobile = m.mobile;
                    obj.ID_Number = m.ID_Number;
                    obj.EMAIL = m.EMAIL;
                    obj.Phone = m.Phone;
                    obj.Photo_ID = m.Photo_ID;
                    obj.Sort = m.Sort;
                    obj.Status = m.Status;
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
        public async Task<ActionResult<M_Delegates>> create(M_Delegates m)
        {
            m.Code = getNext(companycode);
            _context.Delegates.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getm", new { id = m.ID }, m);
        }
        public string getNext(string Company_Code)
        {
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = @"SELECT ISNULL(MAX( CAST(CODE AS BIGINT  ))  ,1000) +1  AS code FROM Delegates";
           // strsql = "select  max(  cast(code as bigint)) +1  as code from Delegates where    Company_Code='" + Company_Code + "' ";

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
        public async Task<ActionResult<M_Delegates>> Deletem(Int64 id)
        {
            var m = await _context.Delegates.FindAsync(id);
            if (m == null)
            {
                return NotFound();
            }
            _context.Delegates.Remove(m);
            await _context.SaveChangesAsync();
            return m;
        }


    }
}
