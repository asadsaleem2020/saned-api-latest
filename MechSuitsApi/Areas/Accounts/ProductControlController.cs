using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MechSuitsApi.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MechSuitsApi.Interfaces;
using CoreInfrastructure.Accounts.Setup;

namespace MechSuitsApi.Areas.Accounts
{
    [Route("api/ProductControl")]
    [ApiController]
    public class ProductControlController : ControllerBase
    {
        private readonly IUriService uriService;
        private readonly AppDBContext _context;
        private SqlConnection con;


        private IHttpContextAccessor _httpContextAccessor;
        Connection c = new Connection();
        string companycode = ""; string user = "";



        public ProductControlController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            _context = context;
            con = c.V_connection;
            this.uriService = uriService;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            //  companycode = currentUser.FindFirst("Company").Value.ToString();
            //  user = currentUser.FindFirst("User").Value.ToString();


        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_ProductControl>>> Get()
        {


            return await _context.PRODUCT_CONTROL.ToListAsync();

        }



        [HttpGet("{id}")]
        public async Task<ActionResult<M_ProductControl>> Get(string id)
        {

            var m = await _context.PRODUCT_CONTROL.FindAsync(id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }


        [Route("update")]
        public async Task<IActionResult> update(M_ProductControl m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_ProductControl obj = new M_ProductControl();
                obj = await _context.PRODUCT_CONTROL.FindAsync(m.Code);

                if (obj != null)
                {

                    obj.Code = m.Code;
                    obj.SaleAccountCode = m.SaleAccountCode;
                    obj.PurchaseAccountCode = m.PurchaseAccountCode;
                    obj.SrAccountCode = m.SrAccountCode;
                    obj.PrAccountCode = m.PrAccountCode;
                    obj.InventoryAccountCode = m.InventoryAccountCode;
                    obj.SaleAccountName = m.SaleAccountName;
                    obj.PurchaseAccountName = m.PurchaseAccountName;
                    obj.PrAccountName = m.PrAccountName;
                    obj.SrAccountName = m.SrAccountName;
                    obj.inventroyaccountname = m.inventroyaccountname;
                    obj.CashAccount = m.CashAccount;
                    obj.CashName = m.CashName;
                    obj.StPayableCode = m.StPayableCode;
                    obj.StPayableName = m.StPayableName;
                    obj.StAdvanceCode = m.StAdvanceCode;
                    obj.StAdvanceName = m.StAdvanceName;
                    obj.POSCustomerAccountCode = m.POSCustomerAccountCode;
                    obj.POSCustomerAccountName = m.POSCustomerAccountName;
                    obj.BankCode = m.BankCode;
                    obj.BankName = m.BankName;
                    obj.SupplierAccount = m.SupplierAccount;
                    obj.CashBookAccount = m.CashBookAccount;
                    obj.AdvanceRevenueAccount = m.AdvanceRevenueAccount;
                    obj.EmployeeReceivablesAccount = m.EmployeeReceivablesAccount;
                    obj.AdministrativeSalriesAccount = m.AdministrativeSalriesAccount;
                    obj.SponsorshipTransferRevenueAccount = m.SponsorshipTransferRevenueAccount;
                    obj.ContractCancellationAccount = m.ContractCancellationAccount;
                    obj.AuthorizationExpenseAccount = m.AuthorizationExpenseAccount;
                    obj.GeneralExpenseAccount = m.GeneralExpenseAccount;
                    obj.SupportCompanyAccount = m.SupportCompanyAccount;
                    obj.CustomerAccount = m.CustomerAccount;
                    obj.BankBookAccount = m.BankBookAccount;
                    obj.PurcahseAccount = m.PurcahseAccount;
                    obj.VatAccount = m.VatAccount;
                    obj.SalesAccount = m.SalesAccount;
                    obj.RentalRevenueAccount = m.RentalRevenueAccount;
                    obj.LateFeeAccount = m.LateFeeAccount;
                    obj.TicketAccount = m.TicketAccount;
                    obj.ContractAuthenticationAccount = m.ContractAuthenticationAccount;
                    obj.SupportingExpenseAccount = m.SupportingExpenseAccount;
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



        //[HttpPost]
        //public async Task<ActionResult<M_Age>> create(M_Age m)
        //{


        //    m.COMPANY_CODE = companycode;
        //    m.Code = dbset.getUpdateMasterCount();
        //    _context.Age.Add(m);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("Get", new { code = m.Code }, m);
        //}

        // DELETE: api/VendorType/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<M_Age>> Delete(string id)
        //{

        //    var m = await _context.Age.FindAsync(companycode, id);
        //    if (m == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Age.Remove(m);
        //    await _context.SaveChangesAsync();

        //    return m;
        //}

    }
}
