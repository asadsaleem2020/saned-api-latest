using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreInfrastructure.Accounts.Setup;
using MechSuitsApi.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MechSuitsApi.Controllers
{
    //[Authorize]
    [Route("api/Level1")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class Level1Controller : ControllerBase
    {
      
        private readonly AppDBContext _context;
        private IHttpContextAccessor _httpContextAccessor;
        string companycode = "";   string user = "";
        public Level1Controller(AppDBContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
        //  companycode = currentUser.FindFirst("Company").Value.ToString(); 
         //   user = currentUser.FindFirst("User").Value.ToString();
        }

    // GET: api/Level1
    [HttpGet]
    public async Task<ActionResult<IEnumerable<M_Level1>>> GetAS_Acclevel1()
    {
        return await _context.AS_Acclevel1.ToListAsync();
    }

    // GET: api/Level1/5
    [HttpGet("{id}")]
    public async Task<ActionResult<M_Level1>> GetM_model(int id)
    {
        var m_model = await _context.AS_Acclevel1.FindAsync(id);

        if (m_model == null)
        {
            return NotFound();
        }

        return m_model;
    }

    // PUT: api/Level1/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPut("{id}")]
    public async Task<IActionResult> PutM_model(int id, M_Level1 m_model)
    {
        if (id != m_model.ID)
        {
            return BadRequest();
        }

        _context.Entry(m_model).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!M_modelExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Level1
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPost]
    public async Task<ActionResult<M_Level1>> PostM_model(M_Level1 m_model)
    {
        _context.AS_Acclevel1.Add(m_model);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetM_model", new { id = m_model.ID }, m_model);
    }

    // DELETE: api/Level1/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<M_Level1>> DeleteM_model(int id)
    {
        var m_model = await _context.AS_Acclevel1.FindAsync(id);
        if (m_model == null)
        {
            return NotFound();
        }

        _context.AS_Acclevel1.Remove(m_model);
        await _context.SaveChangesAsync();

        return m_model;
    }

    private bool M_modelExists(int id)
    {
        return _context.AS_Acclevel1.Any(e => e.ID == id);
    }
}

}
