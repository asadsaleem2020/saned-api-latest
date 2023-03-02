using CoreInfrastructure.AccomodationSystem;
using CoreInfrastructure.GeneralSetting.Age;
using CoreInfrastructure.GeneralSetting.SystemConfiguration;
using MechSuitsApi.Classes;
using MechSuitsApi.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
 
using System.Data.SqlClient;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MechSuitsApi.Areas.GeneralSetting.Controllers
{  
    // [Authorize]
      
    [Route("api/systemconfig")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class SystemConfigurationController : ControllerBase
    {
        private readonly IUriService uriService;
        private readonly AppDBContext _context;
        private SqlConnection con;
        D_DB dbset = null;

        private IHttpContextAccessor _httpContextAccessor;
        Connection c = new Connection();
        string companycode = "C001"; string user = "";



        public SystemConfigurationController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            _context = context;
            con = c.V_connection;
            this.uriService = uriService;
            dbset = new D_DB(c.V_connection);
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            //  companycode = currentUser.FindFirst("Company").Value.ToString();
            //  user = currentUser.FindFirst("User").Value.ToString();


        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_SystemConfiguration>>> Get()
        {


            return await _context.SystemConfiguration.ToListAsync();

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<M_SystemConfiguration>> Getm(string id)
        {
            var m = await _context.SystemConfiguration.FindAsync("C001",id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_SystemConfiguration m)
        {
            Console.WriteLine("HEllo i am here");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_SystemConfiguration obj = new M_SystemConfiguration();

                obj = await _context.SystemConfiguration.FindAsync(companycode, m.Code);

                if (obj != null)
                {

                    obj.COMPANY_CODE = m.COMPANY_CODE;
                    obj.Code = m.Code;
                    obj.OfficeName = m.OfficeName;
                    obj.OfficeNameAr = m.OfficeNameAr;
                    obj.Email = m.Email;
                    obj.RegionAr = m.RegionAr;
                    obj.CityAr = m.CityAr;
                    obj.StreetAr = m.StreetAr;
                    obj.NeighborhoodAr = m.NeighborhoodAr;
                    obj.BuildingNumberAr = m.BuildingNumberAr;
                    obj.Region = m.Region;
                    obj.City = m.City;
                    obj.Address = m.Address;
                    obj.CitySubdivision = m.CitySubdivision;
                    obj.BuildingNumber = m.BuildingNumber;
                    obj.Tel = m.Tel;
                    obj.Mob = m.Mob;
                    obj.Fax = m.Fax;
                    obj.License = m.License;
                    obj.CommercialRegister = m.CommercialRegister;
                    obj.PostalBox = m.PostalBox;
                    obj.PostalCode = m.PostalCode;
                    obj.SMS = m.SMS;
                    obj.SMSPassword = m.SMSPassword;
                    obj.SenderName = m.SenderName;
                    obj.TaxNumber = m.TaxNumber;
                    obj.VAT = m.VAT;
                    obj.Logo = m.Logo;
                    obj.Status = m.Status;
                    obj.Locked = m.Locked;
                    obj.Sort = m.Sort;

                }

                _context.Entry(obj).State = EntityState.Modified;

                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {
                throw;
            }
            return Ok(m);
        }


    }
}
