using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MechSuitsApi.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using CoreInfrastructure.ItemInformation.productlabel;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using CoreInfrastructure.ItemInformation.ItemInformation;
using D_DB = CoreInfrastructure.ItemInformation.productlabel.D_DB;
using MechSuitsApi.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using ExcelDataReader;

namespace MechSuitsApi.Areas.Items.Controllers
{
    [Authorize]
    [Route("api/printlabel")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class productlabelController : ControllerBase
    {
        private IHttpContextAccessor _httpContextAccessor;

        private readonly AppDBContext _context;
        private SqlConnection con;
        D_DB dbset = null; 

        private readonly IUriService uriService;
        string companycode = "C001";   string user = "";
        Connection c = new Connection();
        public productlabelController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            this.uriService = uriService;
            _context = context;
            con = c.V_connection;
            dbset = new D_DB(c.V_connection);
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
          companycode = currentUser.FindFirst("Company").Value.ToString();    user = currentUser.FindFirst("User").Value.ToString();
        }

        // GET: api/VendorType
        [HttpGet]
        [Route("chunking")]
        public async Task<ActionResult<IEnumerable<M_productlabel>>> Get()
        {
            return await _context.ProductLabel.ToListAsync();

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_productlabel>>> Get([FromQuery] PaginationFilter filter)
        {
            // return await _context.ProductLabel.ToListAsync();
            var route = Request.Path.Value;

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.ProductLabel
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.ProductLabel.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_productlabel>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);

        }

        // GET: api/VendorType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_productlabel>> Get(String id)
        {
            
            var m = await _context.ProductLabel.FindAsync(companycode, id);

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_productlabel m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_productlabel obj = new M_productlabel();
                obj = await _context.ProductLabel.FindAsync(m.COMPANY_CODE, m.documentno);

                if (obj != null)
                {

                    obj.Name = m.Name;
                    obj.Code = m.Code;
                    obj.ContactId = m.ContactId;
                    obj.noofLable = m.noofLable;
                    obj.printbusinessname = m.printbusinessname;
                    obj.printname = m.printname;
                    obj.printprice = m.printprice;
                    obj.printvariation = m.printvariation;
                    obj.showprice = m.showprice;

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
        [HttpPost, DisableRequestSizeLimit]
        [Route("Upload/barcodes")]
        public async Task<IActionResult> Upload1()
        {
            Console.WriteLine("Nothing HAppening");
            var formCollection = await Request.ReadFormAsync();
            var file = formCollection.Files.First();
            //var file = Request.Form.Files[0];
            var folderName = Path.Combine("Resources", "Images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            string filePath = string.Empty;

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
                using (var stream = System.IO.File.Open(fullPath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // M_ItemInformation m = new M_ItemInformation();
                        M_productlabel m;
                        int jojo = 0;
                        Console.WriteLine("Uploading Barcodes");
                        while (reader.Read()) //Each row of the file
                        {
                            if (jojo == 1)
                            {
                               
                                Console.WriteLine(reader.GetValue(0).ToString());
                                Console.WriteLine(reader.GetValue(1).ToString());
                                Console.WriteLine(reader.GetValue(2));
                                Console.WriteLine(reader.GetValue(3));
                                m = new M_productlabel
                                {
                                    COMPANY_CODE = companycode,
                                    Code = reader.GetValue(0).ToString(),
                                    Name = reader.GetValue(1).ToString(),
                                    noofLable = (long)Convert.ToDouble(reader.GetValue(2)),
                                    ITEM_BARCODE = reader.GetValue(3).ToString(),
                                    documentno = dbset.getUpdateMasterCount(),
                                    ContactId = reader.GetValue(3).ToString(),
                                    printname = false,
                                    printvariation=false,
                                    printprice=false,
                                    printbusinessname=false,
                                    showprice="0"


    };

                               // var obj = await _context.ProductLabel.FindAsync(companycode, m.Code);

                                //if (obj == null)
                                //{

                                    //obj.Code = reader.GetValue(0).ToString();
                                    //obj.Name = reader.GetValue(1).ToString();
                                    //obj.noofLable = (long)reader.GetValue(2);
                                    //obj.ITEM_BARCODE = reader.GetValue(3).ToString();
                                    //_context.Entry(obj).State = EntityState.Modified;
                                    //await _context.SaveChangesAsync();
                                    _context.ProductLabel.Add(m);
                                    await _context.SaveChangesAsync();

                               // }
                            }
                            else
                            {
                                jojo = 1;
                            }

                        }
                        jojo = 0;
                    }

                }
            }
            return Ok("Done");

        }


        [HttpPost]
        public async Task<ActionResult<M_productlabel>> create(M_productlabel m)
        {
            Console.WriteLine(m);
            Console.WriteLine(m.ITEM_BARCODE);
           // m.documentno = Convert.ToInt64(dbset.getUpdateMasterCount());
            m.documentno = dbset.getUpdateMasterCount();
            Console.WriteLine(m.ContactId);
            m.ContactId = m.documentno+11;
           // m.ContactId = dbset.getUpdateMasterCountid();
           // Console.WriteLine(m.ContactId);
            _context.ProductLabel.Add(m);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { code = m.Code }, m);
        }

        // DELETE: api/VendorType/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_productlabel>> Delete(long id)
        {
            
            var m = await _context.ProductLabel.FindAsync(companycode, id);
            if (m == null)
            {
                return NotFound();
            }

            _context.ProductLabel.Remove(m);
            await _context.SaveChangesAsync();

            return m;
        }






    }
}
