using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreInfrastructure.Accounts.Transaction;
using MechSuitsApi.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using System.Data.SqlClient;
using System.Data;
using Executer;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MechSuitsApi.Areas.Accounts
{
   // [Authorize]
    [Route("api/Customer")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class Customer : ControllerBase
    {
        string companycode = "";   string user = "";
        private readonly AppDBContext _context;
        private SqlConnection con;
        Connection c = new Connection();
        private IHttpContextAccessor _httpContextAccessor;
        public Customer(AppDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            con = c.V_connection;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
       //     companycode = currentUser.FindFirst("Company").Value.ToString();
       //       user = currentUser.FindFirst("User").Value.ToString();
        }

        // GET: api/Level2
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_Customer>>> GetList()
        {
            return await _context.Customer.ToListAsync();
        }

        // GET: api/Level2/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_Customer>> Getm(Int64 id)
        {
            var m = await _context.Customer.FindAsync(id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }
       
       


        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_Customer m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_Customer obj = new M_Customer();
                obj = await _context.Customer.FindAsync(m.ID);

                if (obj != null)
                {                    
                    obj.Name = m.Name;
                    obj.Locked = m.Locked;
                    obj.ADDRESS = m.ADDRESS;
                    obj.CITY = m.CITY;
                    obj.COUNTRY = m.COUNTRY;
                    obj.CNIC = m.CNIC;
                    obj.TEL1 = m.TEL1;
                    obj.EMAIL = m.EMAIL;
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
        public async Task<ActionResult<M_Customer>> create(M_Customer m)
        {
            m.Code = getNext(companycode);
            _context.Customer.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getm", new { id = m.ID }, m);
        }
        public string getNext(string Company_Code)
        {          
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = "select  max(  cast(code as bigint)) +1  as code from Customer where    Company_Code='" + Company_Code + "' ";

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
            if (no.Trim() == "") no = "1001";
            return no;
        }
        // DELETE: api/Level2/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_Customer>> Deletem(Int64 id)
        {
            var m = await _context.Customer.FindAsync(id);
            if (m == null)
            {
                return NotFound();
            }
            _context.Customer.Remove(m);
            await _context.SaveChangesAsync();
            return m;
        }
    }
}
