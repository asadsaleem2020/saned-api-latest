using CoreInfrastructure.GeneralSetting.BusinessLocation;
using CoreInfrastructure.GeneralSetting.Color;
using CoreInfrastructure.GeneralSetting.CurrentOffer;
using CoreInfrastructure.GeneralSetting.department;
using CoreInfrastructure.GeneralSetting.LabelFormate;
using CoreInfrastructure.GeneralSetting.M_BarCodeType;
using CoreInfrastructure.GeneralSetting.MeasuringUnit;
using CoreInfrastructure.GeneralSetting.ProductTypeId;
using CoreInfrastructure.GeneralSetting.StatusName;
using CoreInfrastructure.ItemInformation.ItemCategory;
using CoreInfrastructure.ItemInformation.ItemInformation;
using CoreInfrastructure.ItemInformation.ItemSubCategory;
using CoreInfrastructure.ItemInformation.ItemSubCategoryDetail;
using ExcelDataReader;
using MechSuitsApi.Classes;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
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
using System.Security.Claims;
using MechSuitsApi.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MechSuitsApi.Areas.Items.Controllers
{
    [Authorize]
    [Route("api/iteminformation")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ItemInformationController : ControllerBase
    {
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AppDBContext _context;
        private readonly IUriService uriService;
        private SqlConnection con;
        string companycode = "";
        string user = "";
        CoreInfrastructure.ItemInformation.ItemInformation.D_DB dbset = null;
        D_DBExcel dbexcel = null;
        Connection c = new Connection();
        public ItemInformationController(AppDBContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService)
        {
            _context = context;
            this.uriService = uriService;
            _httpContextAccessor = httpContextAccessor;
            var currentUser = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            companycode = currentUser.FindFirst("Company").Value.ToString(); user = currentUser.FindFirst("User").Value.ToString();
            con = c.V_connection;
            dbset = new CoreInfrastructure.ItemInformation.ItemInformation.D_DB(c.V_connection);
            dbexcel = new D_DBExcel(c.V_connection);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_ExcelSheet>>> Get()
        {
            return await _context.Product_Information.ToListAsync();

        }

        [HttpGet]
        [Route("exportitems")]
        public async Task<ActionResult<IEnumerable<M_ExcelSheet>>> Get3()
        {
            string title = "";
            return await _context.Product_Information.Where(m => (m.ITEM_NAME).Contains(title) || m.ITEM_CODE.Contains(title)).Take(10).ToListAsync();
            // return await _context.Product_Information.ToListAsync();

        }


        [HttpGet]
        [Route("chunks")]
        public async Task<ActionResult<IEnumerable<M_ExcelSheet>>> Get([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.Product_Information
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.Product_Information.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_ExcelSheet>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);



        }




        //[HttpGet]
        //[Route("top10")]

        //public async Task<ActionResult<IEnumerable<M_ItemInformation>>> Get2(string id)
        //{
        //    // string title = HttpContext.Request.Query["title"];
        //    string strsql;
        //    strsql = "select Top(10) * from Product_Information";
        //    if (con.State == ConnectionState.Closed)
        //    {
        //        con.Open();
        //    }
        //    SqlDataAdapter sda = new SqlDataAdapter(strsql, con);
        //    DataTable dt = new DataTable();
        //    sda.Fill(dt);
        //    List<M_ItemInformation> list = null;
        //    list = new List<M_ItemInformation>();

        //    if (dt.Rows.Count > 0)
        //    {

        //        foreach (DataRow sqldr in dt.Rows)
        //        {
        //            var m = new M_ItemInformation();
        //            m.COMPANY_CODE = sqldr["COMPANY_CODE"].ToString();
        //            m.ITEM_CODE = sqldr["ITEM_CODE"].ToString();
        //            m.ITEM_NAME = sqldr["ITEM_NAME"].ToString();
        //            m.SHORT_NAME = sqldr["SHORT_NAME"].ToString();
        //            m.ITEM_BARCODE = sqldr["ITEM_BARCODE"].ToString();
        //            //m.LOCKED = bool.Parse(sqldr["Locked"].ToString());
        //            m.CATEGORY_ID = sqldr["CATEGORY_ID"].ToString();
        //            //m.CATEGORY_NAME = sqldr["CATEGORY_NAME"].ToString();
        //            m.SUB_CATEGORY_ID = sqldr["SUB_CATEGORY_ID"].ToString();
        //            //m.SUB_CATEGORY_NAME = sqldr["SUB_CATEGORY_NAME"].ToString();
        //            m.SUB_CATEGORY_DETAIL_ID = sqldr["SUB_CATEGORY_DETAIL_ID"].ToString();
        //            //m.SUB_CATEGORY_DETAIL_NAME= sqldr["SUB_CATEGORY_DETAIL_NAME"].ToString();
        //            m.MEASURING_UNIT_ID = sqldr["MEASURING_UNIT_ID"].ToString();
        //            //m.MEASURING_UNIT_NAME = sqldr["MEASURING_UNIT_NAME"].ToString();
        //            //m.LOCAL_SALE_ACCOUNT_CODE = sqldr["LOCAL_SALE_ACCOUNT_CODE"].ToString();
        //            //m.LOCAL_SALE_ACCOUNT_NAME = sqldr["LOCAL_SALE_ACCOUNT_NAME"].ToString(); 
        //            //m.EXPORT_SALE_ACCOUNT_CODE = sqldr["EXPORT_SALE_ACCOUNT_CODE"].ToString();
        //            //m.EXPORT_SALE_ACCOUNT_NAME = sqldr["EXPORT_SALE_ACCOUNT_NAME"].ToString();
        //            //m.PURCHASE_ACCOUNT_CODE = sqldr["PURCHASE_ACCOUNT_CODE"].ToString();
        //            //m.PURCHASE_ACCOUNT_NAME = sqldr["PURCHASE_ACCOUNT_NAME"].ToString(); 
        //            //m.SALES_CONSUMPTION_ACCOUNT_CODE = sqldr["SALES_CONSUMPTION_ACCOUNT_CODE"].ToString();
        //            //m.SALES_CONSUMPTION_ACCOUNT_NAME = sqldr["SALES_CONSUMPTION_ACCOUNT_NAME"].ToString(); 
        //            //m.FINISHED_STOCK_ACCOUNT_CODE = sqldr["FINISHED_STOCK_ACCOUNT_CODE"].ToString();
        //            //m.FINISHED_STOCK_ACCOUNT_NAME = sqldr["FINISHED_STOCK_ACCOUNT_NAME"].ToString();
        //            //m.WIP_CONSUMPTION_ACCOUNT_CODE = sqldr["WIP_CONSUMPTION_ACCOUNT_CODE"].ToString();
        //            //m.WIP_CONSUMPTION_ACCOUNT_NAME = sqldr["WIP_CONSUMPTION_ACCOUNT_NAME"].ToString();
        //            m.PURCHASE_RATE = Decimal.Parse(sqldr["PURCHASE_RATE"].ToString());
        //            m.SALE_RATE = Decimal.Parse(sqldr["SALE_RATE"].ToString());
        //            m.Whole_Sale_Rate = Decimal.Parse(sqldr["Whole_Sale_Rate"].ToString());
        //            m.Retail_Rate = Decimal.Parse(sqldr["Retail_Rate"].ToString());
        //            m.Colour = sqldr["Colour"].ToString();
        //            m.Packing_Qty = Decimal.Parse(sqldr["Packing_Qty"].ToString());
        //            m.MIN_LEVEL = Decimal.Parse(sqldr["MIN_LEVEL"].ToString());
        //            m.MAX_LEVEL = Decimal.Parse(sqldr["MAX_LEVEL"].ToString());
        //            list.Add(m);

        //        }
        //    }
        //    if (con.State == ConnectionState.Open)
        //    {
        //        con.Close();
        //    }
        //    return list;

        //}



        //// GET: api/Level2/5
        //[HttpGet]
        //[Route("search")]
        //public async Task<ActionResult<M_ExcelSheet>> Get1(string id)
        //{
        //    string title = HttpContext.Request.Query["title"];

        //    var m = await _context.Product_Information.FindAsync(companycode, id);

        //    if (m == null)
        //    {
        //        return NotFound();
        //    }

        //    return m;
        //}
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<M_ExcelSheet>>> Get1([FromQuery] PaginationFilter filter)
        {

            string title = HttpContext.Request.Query["title"];
            //if(title.Length > 0)
            //{
            //  return await _context.Product_Information.Where(m => (m.ITEM_NAME).Contains(title) || m.ITEM_CODE.Contains(title)).Take(10).ToListAsync();

            //}
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            // var response = await _context.Product_Information.ToListAsync();

            var pagedData = await _context.Product_Information.Where(m => (m.ITEM_NAME).Contains(title) || m.ITEM_CODE.Contains(title))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.Product_Information.Where(m => (m.ITEM_NAME).Contains(title) || m.ITEM_CODE.Contains(title)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_ExcelSheet>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }
        [HttpGet("{brand}/{category}/{subcategory}")]
      
        public async Task<ActionResult<IEnumerable<M_ExcelSheet>>> Get1([FromQuery] PaginationFilter filter, string brand = "", string category = "", string subcategory = "")
        {
           
            string title = HttpContext.Request.Query["title"];
            //if(title.Length > 0)
            //{
            //  return await _context.Product_Information.Where(m => (m.ITEM_NAME).Contains(title) || m.ITEM_CODE.Contains(title)).Take(10).ToListAsync();

            //}
            var route = Request.Path.Value + "?title=" + title;

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
           var  pagedData = await _context.Product_Information.Where(m =>
           (m.ITEM_NAME).Contains(title) || (m.ITEM_CODE).Contains(title))
           .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
           .Take(validFilter.PageSize)
           .ToListAsync();
            var totalRecords = await _context.Product_Information.Where(m => (m.ITEM_NAME).Contains(title) || m.ITEM_CODE.Contains(title) || (m.CATEGORY_ID).Equals(brand) ||
          (m.SUB_CATEGORY_ID).Equals(category) ||
          (m.SUB_CATEGORY_DETAIL_ID).Equals(subcategory)).CountAsync();
            // var response = await _context.Product_Information.ToListAsync();
            if (brand == "0")
            {
                Console.WriteLine("Brand 0");
                pagedData = await _context.Product_Information.Where(m =>
            (m.ITEM_NAME).Contains(title) || (m.ITEM_CODE).Contains(title))
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToListAsync();

                totalRecords = await _context.Product_Information.Where(m => (m.ITEM_NAME).Contains(title) || m.ITEM_CODE.Contains(title)).CountAsync();
            }
            else if (category == "00")
            {
                Console.WriteLine("00");
                pagedData = await _context.Product_Information.Where(m =>
              (m.ITEM_NAME).Contains(title) ||
              (m.ITEM_CODE).Contains(title) ||
              (m.CATEGORY_ID).Equals(brand))
              .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
              .Take(validFilter.PageSize)
              .ToListAsync();

                totalRecords = await _context.Product_Information.Where(m => 
                (m.ITEM_NAME).Contains(title) ||
                (m.ITEM_CODE).Contains(title) ||
                  (m.CATEGORY_ID).Equals(brand)).CountAsync();
            }
            else if (subcategory == "000")

            {
                Console.WriteLine("000");
                pagedData = await _context.Product_Information.Where(m =>
                (m.ITEM_NAME).Contains(title) ||
                (m.ITEM_CODE).Contains(title) ||
                 ((m.CATEGORY_ID).Equals(brand) && (m.SUB_CATEGORY_ID).Equals(category)))
                 .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                 .Take(validFilter.PageSize)
                 .ToListAsync();

                totalRecords = await _context.Product_Information.Where(m =>
                   (m.ITEM_NAME).Contains(title) ||
                   (m.ITEM_CODE).Contains(title) ||
                   ((m.CATEGORY_ID).Equals(brand) &&
                   (m.SUB_CATEGORY_ID).Equals(category))).CountAsync();
            }
            else
            {
                Console.WriteLine("else");
                pagedData = await _context.Product_Information.Where(m =>
               (m.ITEM_NAME).Contains(title) ||
               (m.ITEM_CODE).Contains(title) ||
              ((m.CATEGORY_ID).Equals(brand) && (m.SUB_CATEGORY_ID).Equals(category) &&(m.SUB_CATEGORY_DETAIL_ID).Equals(subcategory)))
               .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
               .Take(validFilter.PageSize)
               .ToListAsync();
           totalRecords = await _context.Product_Information.Where(m => 
               (m.ITEM_NAME).Contains(title) ||
               (m.ITEM_CODE).Contains(title) ||
               ((m.CATEGORY_ID).Equals(brand) && (m.SUB_CATEGORY_ID).Equals(category) && (m.SUB_CATEGORY_DETAIL_ID).Equals(subcategory))).CountAsync();
            }



            //var totalRecords = await _context.Product_Information.Where(m => (m.ITEM_NAME).Contains(title) || m.ITEM_CODE.Contains(title) || (m.CATEGORY_ID).Equals(brand) ||
            //(m.SUB_CATEGORY_ID).Equals(category) ||
            //(m.SUB_CATEGORY_DETAIL_ID).Equals(subcategory)).CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<M_ExcelSheet>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);


        }



        [HttpGet("{id}")]
        public async Task<ActionResult<M_ExcelSheet>> Get(string id)
        {

            var m = await _context.Product_Information.FindAsync(companycode, id);

            if (m == null)
            {
                return NotFound();
            }
            //Convert.ChangeType(m, M_ExcelSheet);
            return m;
        }


        [HttpGet("{scan}/{id}")]
        //[Route("scan")]
        public async Task<ActionResult<M_ExcelSheet>> Scanning(string scan, string id)
        {
            Console.WriteLine("HEllo");

            var m = await _context.Product_Information.Where(x => x.ITEM_BARCODE == id).FirstAsync();  

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }

        [HttpGet]
        [Route("barcode")]
        public async Task<ActionResult<M_ExcelSheet>> barcode(string id)
        {
           // Console.WriteLine("HEllo");

            var m = await _context.Product_Information.Where(x => x.ITEM_BARCODE == id ||  x.ITEM_CODE == id || x.ITEM_NAME == id).FirstAsync();

            if (m == null)
            {
                return NotFound();
            }

            return m;
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("Upload")]
        public async Task<IActionResult> Upload()
        {
           //Console.WriteLine("Data is Ready to insert");
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
                         M_ExcelSheet m;
                        int jojo = 0;
                        while (reader.Read()) //Each row of the file
                        {
                            if (jojo == 1)
                            {
                                string stringDate = DateTime.Now.ToString("yyyy-MM-dd");
                                Console.WriteLine(stringDate);
                                Console.WriteLine(reader.GetValue(65).ToString() ?? "Defualt");
                                Console.WriteLine(reader.GetValue(0));
                                Console.WriteLine(reader.GetValue(1));
                            
                            //try
                            //{
                                m = new M_ExcelSheet
                                {


                                    //COMPANY_CODE = (reader.GetValue(0).ToString()) ?? "C001",
                                    COMPANY_CODE ="C001",
                                    ITEM_CODE = reader.GetValue(1).ToString(),
                                    ITEM_NAME = reader.GetValue(2).ToString(),
                                    ITEM_BARCODE = (reader.GetValue(3).ToString()) ?? "",
                                    CATEGORY_ID = (reader.GetValue(4).ToString()) ?? "",
                                   SUB_CATEGORY_ID = (reader.GetValue(5).ToString()) ?? "",
                                    SUB_CATEGORY_DETAIL_ID = reader.GetValue(6).ToString() ?? "",
                                    BARCODE_TYPE_ID = reader.GetValue(7).ToString() ?? "",
                                    STATUS_ID = reader.GetValue(8).ToString() ?? "",
                                    BUSSINESS_LOCATION_ID = reader.GetValue(9).ToString() ?? "",
                                    Alter_Qty = Convert.ToDecimal(reader.GetValue(10) ?? 0),
                                    PRODUCT_TYPE_ID = reader.GetValue(11).ToString() ?? "",
                                    WAREHOUSE_ID = reader.GetValue(12).ToString() ?? "",
                                    BATCH_NO = reader.GetValue(13).ToString() ?? "",
                                    Colour = reader.GetValue(14).ToString() ?? "",
                                    MEASURING_UNIT_ID = reader.GetValue(15).ToString(),
                                    PURCHASE_RATE = Convert.ToDecimal(reader.GetValue(16) ?? 0),
                                    SALE_RATE = Convert.ToDecimal(reader.GetValue(17) ?? 0),
                                    Packing_Qty = Convert.ToDecimal(reader.GetValue(18) ?? 0),
                                    Whole_Sale_Rate = Convert.ToDecimal(reader.GetValue(19) ?? 0),
                                    Retail_Rate = Convert.ToDecimal(reader.GetValue(20) ?? 0),
                                    MAX_LEVEL = Convert.ToDecimal(reader.GetValue(21) ?? 0),
                                    MIN_LEVEL = Convert.ToDecimal(reader.GetValue(22) ?? 0),
                                    UpperBox = Convert.ToDecimal(reader.GetValue(23) ?? 0),
                                    InnerBox = Convert.ToDecimal(reader.GetValue(24) ?? 0),
                                    PacketBox = Convert.ToDecimal(reader.GetValue(25) ?? 0),
                                    UpperPrice = Convert.ToDecimal(reader.GetValue(26) ?? 0),
                                    InnerPrice = Convert.ToDecimal(reader.GetValue(27) ?? 0),
                                    PacketPrice = Convert.ToDecimal(reader.GetValue(28) ?? 0),
                                    SHORT_NAME = reader.GetValue(29).ToString() ?? "",
                                    SKU = reader.GetValue(30).ToString() ?? "",
                                    EXPIRAY_DATE = Convert.ToDateTime(stringDate),
                                    EXPIRAY_ALERT_ID = reader.GetValue(32).ToString() ?? "",
                                    EXPIRAY_ALERT = Convert.ToDecimal(reader.GetValue(33) ?? 0),
                                    MP_NUMBER = reader.GetValue(34).ToString() ?? "",
                                    SP_NUMBER = reader.GetValue(35).ToString() ?? "",
                                    WARRENTY_DAYS = Convert.ToDecimal(reader.GetValue(36) ?? 0),
                                    MODEL_NUMBER = reader.GetValue(37).ToString() ?? "",
                                    WEIGHT = Convert.ToDecimal(reader.GetValue(38) ?? 0),
                                    DESCRIPTION = reader.GetValue(39).ToString() ?? "",
                                    IMG_PATH = reader.GetValue(40).ToString() ?? "",
                                    //40 clear

                                    CURRENT_STOCK = Convert.ToDecimal(reader.GetValue(41) ?? 0),
                                    REORDER_QUANTITY = Convert.ToDecimal(reader.GetValue(42) ?? 0),
                                    LABEL_FORMATE = reader.GetValue(43).ToString() ?? "",
                                    PACKED_DATE = Convert.ToDateTime(stringDate),
                                    USED_DATE = Convert.ToDateTime(stringDate),
                                    SELL_DATE = Convert.ToDateTime(stringDate),
                                    NOTSALE_QUANTITY = Convert.ToDecimal(reader.GetValue(47) ?? 0),
                                    DISCOUNT_RATE = Convert.ToDecimal(reader.GetValue(48) ?? 0),
                                    CURRENT_OFFER = reader.GetValue(49).ToString() ?? "",
                                    OFFERDAY_ID = reader.GetValue(50).ToString() ?? "",
                                    OFFER_START_DATE = Convert.ToDateTime(stringDate),
                                    OFFER_END_DATE = Convert.ToDateTime(stringDate),
                                    RACK_NO = reader.GetValue(53).ToString() ?? "",
                                    ROW_NO = reader.GetValue(54).ToString() ?? "",
                                    POSITION = reader.GetValue(55).ToString() ?? "",
                                    FRONT_LOCATION_ID = reader.GetValue(56).ToString() ?? "",
                                    BACK_LOCATION_ID = reader.GetValue(57).ToString() ?? "",
                                    MULTI_BARCODE_ID = reader.GetValue(58).ToString() ?? "",
                                    CREATED_ON = Convert.ToDateTime(stringDate),
                                    CREATED_BY = reader.GetValue(60).ToString() ?? "",
                                    IS_DELETED = Convert.ToBoolean(reader.GetValue(61) ?? false),
                                    DELETED_BY = reader.GetValue(62).ToString() ?? "",
                                    DELETEED_ON = Convert.ToDateTime(stringDate),
                                    UPDATE_BY = reader.GetValue(64).ToString() ?? "",
                                    UPDATE_ON = Convert.ToDateTime(stringDate),


                                };
                            //}
                            //catch
                            //{
                            //    Console.WriteLine("Skipping data row");
                            //    continue;
                            //}

                            

                                var obj = await _context.Product_Information.FindAsync(companycode, m.ITEM_CODE);
                                if (obj != null)
                                {
                                    Console.WriteLine("Entered In IF Condition!");
                                    obj.COMPANY_CODE = reader.GetValue(0).ToString();
                                    obj.ITEM_CODE = reader.GetValue(1).ToString();
                                    if (reader.GetValue(2).ToString() != "")
                                    {
                                        obj.ITEM_NAME = reader.GetValue(2).ToString();
                                    }
                                    if (reader.GetValue(3).ToString() != "")
                                    {
                                        obj.ITEM_BARCODE = reader.GetValue(3).ToString();
                                    }

                                    if (dbexcel.ifexists("ItemCategory", "code", "Name", reader.GetValue(65).ToString()).Trim().Length > 0)
                                    {
                                        obj.CATEGORY_ID = dbexcel.ifexists("ItemCategory", "code", "Name", reader.GetValue(65).ToString()).Trim();
                                    }
                                    else
                                    {
                                        obj.CATEGORY_ID = dbexcel.getNextIdCategory();
                                        M_ItemCategory c = new M_ItemCategory
                                        {
                                            COMPANY_CODE = companycode,
                                            Code = obj.CATEGORY_ID,
                                            Locked = false,
                                            Name = reader.GetValue(65).ToString(),
                                            Sort = 0
                                        };
                                        _context.ItemCategory.Add(c);
                                        await _context.SaveChangesAsync();
                                    }



                                    if (
                                      dbexcel.ifexists("Item_SubCategory", "code", "Name", reader.GetValue(66).ToString()).Trim().Length > 0
                                      )
                                    {
                                        obj.SUB_CATEGORY_ID = dbexcel.ifexists("Item_SubCategory", "code", "Name", reader.GetValue(66).ToString()).Trim();
                                    }
                                    else
                                    {
                                        obj.SUB_CATEGORY_ID = dbexcel.getNextIdSubCategory(obj.CATEGORY_ID);
                                        M_SubCategory c = new M_SubCategory
                                        {
                                            COMPANY_CODE = companycode,
                                            Code = obj.SUB_CATEGORY_ID,
                                            Locked = false,
                                            Name = reader.GetValue(66).ToString(),
                                            Sort = 0,
                                            Level1Code = obj.CATEGORY_ID,
                                            Level1Name = reader.GetValue(65).ToString(),


                                        };
                                        _context.Item_SubCategory.Add(c);
                                        await _context.SaveChangesAsync();
                                    }

                                    if (
                                   dbexcel.ifexists("Item_SubCategoryDetail", "code", "Name", reader.GetValue(6).ToString()).Trim().Length > 0
                                   )
                                    {
                                        obj.SUB_CATEGORY_DETAIL_ID = dbexcel.ifexists("Item_SubCategoryDetail", "code", "Name", reader.GetValue(6).ToString()).Trim();
                                    }
                                    else
                                    {
                                        obj.SUB_CATEGORY_DETAIL_ID = dbexcel.getNextIdSubCategoryDetail(obj.SUB_CATEGORY_DETAIL_ID, obj.CATEGORY_ID);
                                        M_SubCategoryDetail c = new M_SubCategoryDetail
                                        {
                                            Company_Code = companycode,
                                            Code = obj.SUB_CATEGORY_DETAIL_ID,
                                            Locked = false,
                                            Name = reader.GetValue(6).ToString(),
                                            Sort = 0,
                                            Level1Code = obj.CATEGORY_ID,
                                            Level1Name = reader.GetValue(4).ToString(),
                                            Level2Code = obj.SUB_CATEGORY_ID,
                                            Level2Name = reader.GetValue(5).ToString(),


                                        };
                                        _context.Item_SubCategoryDetail.Add(c);
                                        await _context.SaveChangesAsync();
                                    }




                                    if (
                                       dbexcel.ifexists("BarCodeType", "code", "Name", reader.GetValue(7).ToString()).Trim().Length > 0
                                       )
                                    {
                                        obj.BARCODE_TYPE_ID = dbexcel.ifexists("BarCodeType", "code", "Name", reader.GetValue(7).ToString()).Trim();
                                    }
                                    else
                                    {
                                        obj.BARCODE_TYPE_ID = dbexcel.getNextIdBarcodeType();
                                        M_BarCodeType c = new M_BarCodeType
                                        {
                                            COMPANY_CODE = companycode,
                                            Code = obj.BARCODE_TYPE_ID,
                                            Locked = false,
                                            Name = reader.GetValue(7).ToString(),
                                            Sort = 0


                                        };
                                        _context.BarCodeType.Add(c);
                                        await _context.SaveChangesAsync();
                                    }




                                    if (
                                     dbexcel.ifexists("StatusName", "code", "Name", reader.GetValue(8).ToString()).Trim().Length > 0
                                     )
                                    {
                                        obj.STATUS_ID = dbexcel.ifexists("StatusName", "code", "Name", reader.GetValue(8).ToString()).Trim();
                                    }
                                    else
                                    {
                                        obj.STATUS_ID = dbexcel.getNextIdStatusName();
                                        M_StatusName c = new M_StatusName
                                        {
                                            COMPANY_CODE = companycode,
                                            Code = obj.STATUS_ID,
                                            Locked = false,
                                            Name = reader.GetValue(8).ToString(),
                                            Sort = 0


                                        };
                                        _context.StatusName.Add(c);
                                        await _context.SaveChangesAsync();
                                    }



                                    if (
                                   dbexcel.ifexists("BusinessLocation", "code", "Name", reader.GetValue(9).ToString()).Trim().Length > 0
                                   )
                                    {
                                        obj.BUSSINESS_LOCATION_ID = dbexcel.ifexists("BusinessLocation", "code", "Name", reader.GetValue(9).ToString()).Trim();
                                    }
                                    else
                                    {
                                        obj.BUSSINESS_LOCATION_ID = dbexcel.getNextIdCategory();
                                        M_BusinessLocation c = new M_BusinessLocation
                                        {
                                            COMPANY_CODE = companycode,
                                            Code = obj.BUSSINESS_LOCATION_ID,
                                            Locked = false,
                                            Name = reader.GetValue(9).ToString(),
                                            Sort = 0


                                        };
                                        _context.BusinessLocation.Add(c);
                                        await _context.SaveChangesAsync();
                                    }

                                    if (Convert.ToDecimal(reader.GetValue(10)) != 0)
                                    {
                                        obj.Alter_Qty = Convert.ToDecimal(reader.GetValue(10));
                                    }



                                    if (
                                 dbexcel.ifexists("ProductTypeId", "code", "Name", reader.GetValue(11).ToString()).Trim().Length > 0
                                 )
                                    {
                                        obj.BUSSINESS_LOCATION_ID = dbexcel.ifexists("ProductTypeId", "code", "Name", reader.GetValue(11).ToString()).Trim();
                                    }
                                    else
                                    {
                                        obj.BUSSINESS_LOCATION_ID = dbexcel.getNextIdProductTypeId();
                                        M_ProductTypeId c = new M_ProductTypeId
                                        {
                                            COMPANY_CODE = companycode,
                                            Code = obj.BUSSINESS_LOCATION_ID,
                                            Locked = false,
                                            Name = reader.GetValue(11).ToString(),
                                            Sort = 0


                                        };
                                        _context.ProductTypeId.Add(c);
                                        await _context.SaveChangesAsync();
                                    }


                                    if (
                               dbexcel.ifexists("Department", "code", "Name", reader.GetValue(12).ToString()).Trim().Length > 0
                               )
                                    {
                                        obj.WAREHOUSE_ID = dbexcel.ifexists("Department", "code", "Name", reader.GetValue(12).ToString()).Trim();
                                    }
                                    else
                                    {
                                        obj.WAREHOUSE_ID = dbexcel.getNextIdDepartment();
                                        M_Department c = new M_Department
                                        {
                                            COMPANY_CODE = companycode,
                                            Code = obj.WAREHOUSE_ID,
                                            Locked = false,
                                            Name = reader.GetValue(12).ToString(),
                                            Sort = 0


                                        };
                                        _context.Department.Add(c);
                                        await _context.SaveChangesAsync();
                                    }
                                    if (reader.GetValue(13).ToString() != "")
                                        obj.BATCH_NO = reader.GetValue(13).ToString();


                                    if (
                              dbexcel.ifexists("color", "code", "Name", reader.GetValue(14).ToString()).Trim().Length > 0
                              )
                                    {
                                        obj.Colour = dbexcel.ifexists("color", "code", "Name", reader.GetValue(14).ToString()).Trim();
                                    }
                                    else
                                    {
                                        obj.Colour = dbexcel.getNextIdDepartment();
                                        M_Color c = new M_Color
                                        {
                                            COMPANY_CODE = companycode,
                                            Code = obj.Colour,
                                            Locked = false,
                                            Name = reader.GetValue(14).ToString(),
                                            Sort = 0


                                        };
                                        _context.Color.Add(c);
                                        await _context.SaveChangesAsync();
                                    }

                                    if (reader.GetValue(15).ToString() != "")
                                        obj.MEASURING_UNIT_ID = reader.GetValue(15).ToString();

                                    if (
                         dbexcel.ifexists("MeasuringUnit", "code", "Name", reader.GetValue(15).ToString()).Trim().Length > 0
                         )
                                    {
                                        obj.MEASURING_UNIT_ID = dbexcel.ifexists("MeasuringUnit", "code", "Name", reader.GetValue(14).ToString()).Trim();
                                    }
                                    else
                                    {
                                        obj.MEASURING_UNIT_ID = dbexcel.getNextIdMeasuringUnit();
                                        M_MeasuringUnit c = new M_MeasuringUnit
                                        {
                                            COMPANY_CODE = companycode,
                                            Code = obj.MEASURING_UNIT_ID,
                                            Locked = false,
                                            Name = reader.GetValue(15).ToString(),
                                            Sort = 0


                                        };
                                        _context.MeasuringUnit.Add(c);
                                        await _context.SaveChangesAsync();
                                    }
                                    if (Convert.ToDecimal(reader.GetValue(16)) != 0)
                                        obj.PURCHASE_RATE = Convert.ToDecimal(reader.GetValue(16));
                                    if (Convert.ToDecimal(reader.GetValue(17)) != 0)
                                        obj.SALE_RATE = Convert.ToDecimal(reader.GetValue(17));

                                    if (Convert.ToDecimal(reader.GetValue(18)) != 0)
                                        obj.Packing_Qty = Convert.ToDecimal(reader.GetValue(18));
                                    if (Convert.ToDecimal(reader.GetValue(19)) != 0)
                                        obj.Whole_Sale_Rate = Convert.ToDecimal(reader.GetValue(19));
                                    if (Convert.ToDecimal(reader.GetValue(20)) != 0)
                                        obj.Retail_Rate = Convert.ToDecimal(reader.GetValue(20));

                                    if (Convert.ToDecimal(reader.GetValue(21)) != 0)
                                        obj.MAX_LEVEL = Convert.ToDecimal(reader.GetValue(21));
                                    if (Convert.ToDecimal(reader.GetValue(22)) != 0)
                                        obj.MIN_LEVEL = Convert.ToDecimal(reader.GetValue(22));
                                    if (Convert.ToDecimal(reader.GetValue(23)) != 0)
                                        obj.UpperBox = Convert.ToDecimal(reader.GetValue(23));
                                    if (Convert.ToDecimal(reader.GetValue(24)) != 0)
                                        obj.InnerBox = Convert.ToDecimal(reader.GetValue(24));
                                    if (Convert.ToDecimal(reader.GetValue(25)) != 0)
                                        obj.PacketBox = Convert.ToDecimal(reader.GetValue(25));
                                    if (Convert.ToDecimal(reader.GetValue(26)) != 0)
                                        obj.UpperPrice = Convert.ToDecimal(reader.GetValue(26));

                                    if (Convert.ToDecimal(reader.GetValue(27)) != 0)
                                        obj.InnerPrice = Convert.ToDecimal(reader.GetValue(27));
                                    if (Convert.ToDecimal(reader.GetValue(28)) != 0)
                                        obj.PacketPrice = Convert.ToDecimal(reader.GetValue(28));
                                    if (reader.GetValue(29).ToString() != "")
                                    {
                                        obj.SHORT_NAME = reader.GetValue(29).ToString();
                                    }
                                    if (reader.GetValue(30).ToString() != "")
                                    {
                                        obj.SKU = reader.GetValue(30).ToString();
                                    }
                                    obj.EXPIRAY_DATE = Convert.ToDateTime(DateTime.Now.ToString());
                                    if (reader.GetValue(32).ToString() != "")
                                    {
                                        obj.EXPIRAY_ALERT_ID = reader.GetValue(32).ToString();
                                    }
                                    if (Convert.ToDecimal(reader.GetValue(33)) != 0)
                                        obj.EXPIRAY_ALERT = Convert.ToDecimal(reader.GetValue(33));
                                    if (reader.GetValue(34).ToString() != "")
                                    {
                                        obj.MP_NUMBER = reader.GetValue(34).ToString();
                                    }
                                    if (reader.GetValue(35).ToString() != "")
                                    {
                                        obj.SP_NUMBER = reader.GetValue(35).ToString();
                                    }

                                    if (Convert.ToDecimal(reader.GetValue(36)) != 0)
                                        obj.WARRENTY_DAYS = Convert.ToDecimal(reader.GetValue(36));
                                    if (reader.GetValue(37).ToString() != "")
                                    {
                                        obj.MODEL_NUMBER = reader.GetValue(37).ToString();
                                    }
                                    if (Convert.ToDecimal(reader.GetValue(38)) != 0)
                                        obj.WEIGHT = Convert.ToDecimal(reader.GetValue(38));
                                    if (reader.GetValue(39).ToString() != "")
                                    {
                                        obj.DESCRIPTION = reader.GetValue(39).ToString();
                                    }
                                    if (reader.GetValue(40).ToString() != "")
                                    {
                                        obj.IMG_PATH = reader.GetValue(40).ToString();
                                    }
                                    if (Convert.ToDecimal(reader.GetValue(41)) != 0)
                                        obj.CURRENT_STOCK = Convert.ToDecimal(reader.GetValue(41));
                                    if (Convert.ToDecimal(reader.GetValue(42)) != 0)
                                        obj.REORDER_QUANTITY = Convert.ToDecimal(reader.GetValue(42));

                                    if (
                                 dbexcel.ifexists("LabelFormate", "code", "Name", reader.GetValue(43).ToString()).Trim().Length > 0
                                 )
                                    {
                                        obj.LABEL_FORMATE = dbexcel.ifexists("LabelFormate", "code", "Name", reader.GetValue(43).ToString()).Trim();
                                    }
                                    else
                                    {
                                        obj.LABEL_FORMATE = dbexcel.getNextIdLabelFormate();
                                        M_LabelFormate c = new M_LabelFormate
                                        {
                                            COMPANY_CODE = companycode,
                                            Code = obj.LABEL_FORMATE,
                                            Locked = false,
                                            Name = reader.GetValue(43).ToString(),
                                            Sort = 0


                                        };
                                        _context.LabelFormate.Add(c);
                                        await _context.SaveChangesAsync();
                                    }




                                    // obj.PACKED_DATE = Convert.ToDateTime(reader.GetValue(44));
                                    obj.PACKED_DATE = Convert.ToDateTime(stringDate);

                                    // obj.USED_DATE = Convert.ToDateTime(reader.GetValue(45));
                                    obj.USED_DATE = Convert.ToDateTime(stringDate);
                                    //  obj.SELL_DATE = Convert.ToDateTime(reader.GetValue(46));
                                    obj.SELL_DATE = Convert.ToDateTime(stringDate);
                                    if (Convert.ToDecimal(reader.GetValue(47)) != 0)
                                    {
                                        obj.NOTSALE_QUANTITY = Convert.ToDecimal(reader.GetValue(47));
                                    }
                                    if (Convert.ToDecimal(reader.GetValue(48)) != 0)
                                        obj.DISCOUNT_RATE = Convert.ToDecimal(reader.GetValue(48));
                                    if (reader.GetValue(49).ToString() != "")
                                    {
                                        obj.CURRENT_OFFER = reader.GetValue(49).ToString();
                                    }
                                    if (
                               dbexcel.ifexists("CurrentOffer", "code", "Name", reader.GetValue(49).ToString()).Trim().Length > 0
                               )
                                    {
                                        obj.CURRENT_OFFER = dbexcel.ifexists("CurrentOffer", "code", "Name", reader.GetValue(49).ToString()).Trim();
                                    }
                                    else
                                    {
                                        obj.CURRENT_OFFER = dbexcel.getNextIdCurrentOffer();
                                        M_CurrentOffer c = new M_CurrentOffer
                                        {
                                            COMPANY_CODE = companycode,
                                            Code = obj.CURRENT_OFFER,
                                            Locked = false,
                                            Name = reader.GetValue(49).ToString(),
                                            Sort = 0


                                        };
                                        _context.CurrentOffer.Add(c);
                                        await _context.SaveChangesAsync();
                                    }
                                    if (reader.GetValue(50).ToString() != "")
                                    {
                                        obj.OFFERDAY_ID = reader.GetValue(50).ToString();
                                    }
                                    // obj.OFFER_START_DATE = Convert.ToDateTime(reader.GetValue(51));
                                    obj.OFFER_START_DATE = Convert.ToDateTime(stringDate);
                                    //   obj.OFFER_END_DATE = Convert.ToDateTime(reader.GetValue(52));
                                    obj.OFFER_END_DATE = Convert.ToDateTime(stringDate);
                                    if (reader.GetValue(53).ToString() != "")
                                    {
                                        obj.RACK_NO = reader.GetValue(53).ToString();
                                    }

                                    if (reader.GetValue(54).ToString() != "")
                                    {
                                        obj.ROW_NO = reader.GetValue(54).ToString();
                                    }
                                    if (reader.GetValue(55).ToString() != "")
                                    {
                                        obj.POSITION = reader.GetValue(55).ToString();
                                    }
                                    if (reader.GetValue(56).ToString() != "")
                                    {
                                        obj.FRONT_LOCATION_ID = reader.GetValue(56).ToString();
                                    }
                                    if (reader.GetValue(57).ToString() != "")
                                    {
                                        obj.BACK_LOCATION_ID = reader.GetValue(57).ToString();
                                    }
                                    if (reader.GetValue(58).ToString() != "")
                                    {
                                        obj.MULTI_BARCODE_ID = reader.GetValue(58).ToString();
                                    }
                                    //obj.CREATED_ON = Convert.ToDateTime(reader.GetValue(59));
                                    obj.CREATED_ON = Convert.ToDateTime(stringDate);
                                    if (reader.GetValue(60).ToString() != "")
                                    {
                                        obj.CREATED_BY = reader.GetValue(60).ToString();
                                    }
                                    obj.IS_DELETED = Convert.ToBoolean(reader.GetValue(61));
                                    if (reader.GetValue(62).ToString() != "")
                                    {
                                        obj.DELETED_BY = reader.GetValue(62).ToString();
                                    }
                                    //obj.DELETEED_ON = Convert.ToDateTime(reader.GetValue(63));
                                    obj.DELETEED_ON = Convert.ToDateTime(stringDate);
                                    if (reader.GetValue(64).ToString() != "")
                                    {
                                        obj.UPDATE_BY = reader.GetValue(64).ToString();
                                        // obj.UPDATE_BY = "qasim";
                                    }

                                    //obj.UPDATE_ON = Convert.ToDateTime(reader.GetValue(65));
                                    obj.UPDATE_ON = Convert.ToDateTime(stringDate);


                                    _context.Entry(obj).State = EntityState.Modified;
                                    await _context.SaveChangesAsync();

                                }
                                else
                                {
                                    Console.WriteLine("Entered In Else Condition!");
                                    m.COMPANY_CODE = reader.GetValue(0).ToString();
                                    m.ITEM_CODE = reader.GetValue(1).ToString();
                                    m.ITEM_NAME = reader.GetValue(2).ToString();
                                    m.ITEM_BARCODE = reader.GetValue(3).ToString() ?? m.ITEM_CODE;

                                    if (
                                            dbexcel.ifexists("ItemCategory", "code", "Name", reader.GetValue(65).ToString()).Trim().Length > 0
                                            )
                                    {
                                        m.CATEGORY_ID = dbexcel.ifexists("ItemCategory", "code", "Name", reader.GetValue(65).ToString()).Trim();
                                    }
                                    else
                                    {
                                        m.CATEGORY_ID = dbexcel.getNextIdCategory();
                                        M_ItemCategory c = new M_ItemCategory
                                        {
                                            COMPANY_CODE = companycode,
                                            Code = m.CATEGORY_ID,
                                            Locked = false,
                                            Name = reader.GetValue(65).ToString(),
                                            Sort = 0


                                        };
                                        _context.ItemCategory.Add(c);
                                        await _context.SaveChangesAsync();
                                    }



                                    if (
                                      dbexcel.ifexists("Item_SubCategory", "code", "Name", reader.GetValue(66).ToString()).Trim().Length > 0
                                      )
                                    {
                                        m.SUB_CATEGORY_ID = dbexcel.ifexists("Item_SubCategory", "code", "Name", reader.GetValue(66).ToString()).Trim();
                                    }
                                    else
                                    {
                                        m.SUB_CATEGORY_ID = dbexcel.getNextIdSubCategory(m.CATEGORY_ID);
                                        M_SubCategory c = new M_SubCategory
                                        {
                                            COMPANY_CODE = companycode,
                                            Code = m.SUB_CATEGORY_ID,
                                            Locked = false,
                                            Name = reader.GetValue(66).ToString(),
                                            Sort = 0,
                                            Level1Code = m.CATEGORY_ID,
                                            Level1Name = reader.GetValue(65).ToString(),


                                        };
                                        _context.Item_SubCategory.Add(c);
                                        await _context.SaveChangesAsync();
                                    }

                                    if (
                                   dbexcel.ifexists("Item_SubCategoryDetail", "code", "Name", reader.GetValue(67).ToString()).Trim().Length > 0
                                   )
                                    {
                                        m.SUB_CATEGORY_DETAIL_ID = dbexcel.ifexists("Item_SubCategoryDetail", "code", "Name", reader.GetValue(67).ToString()).Trim();
                                    }
                                    else
                                    {
                                        m.SUB_CATEGORY_DETAIL_ID = dbexcel.getNextIdSubCategoryDetail(m.SUB_CATEGORY_DETAIL_ID, m.CATEGORY_ID);
                                        M_SubCategoryDetail c = new M_SubCategoryDetail
                                        {
                                            Company_Code = companycode,
                                            Code = m.SUB_CATEGORY_DETAIL_ID,
                                            Locked = false,
                                            Name = reader.GetValue(67).ToString(),
                                            Sort = 0,
                                            Level1Code = m.CATEGORY_ID,
                                            Level1Name = reader.GetValue(65).ToString(),
                                            Level2Code = m.SUB_CATEGORY_ID,
                                            Level2Name = reader.GetValue(66).ToString(),
                                        };
                                        _context.Item_SubCategoryDetail.Add(c);
                                        await _context.SaveChangesAsync();
                                    }




                                    if (
                                       dbexcel.ifexists("BarCodeType", "code", "Name", reader.GetValue(7).ToString()).Trim().Length > 0
                                       )
                                    {
                                        m.BARCODE_TYPE_ID = dbexcel.ifexists("BarCodeType", "code", "Name", reader.GetValue(7).ToString()).Trim();
                                    }
                                    else
                                    {
                                        m.BARCODE_TYPE_ID = dbexcel.getNextIdBarcodeType();
                                        M_BarCodeType c = new M_BarCodeType
                                        {
                                            COMPANY_CODE = companycode,
                                            Code = m.BARCODE_TYPE_ID,
                                            Locked = false,
                                            Name = reader.GetValue(7).ToString(),
                                            Sort = 0


                                        };
                                        _context.BarCodeType.Add(c);
                                        await _context.SaveChangesAsync();
                                    }




                                    if (
                                     dbexcel.ifexists("StatusName", "code", "Name", reader.GetValue(8).ToString()).Trim().Length > 0
                                     )
                                    {
                                        m.STATUS_ID = dbexcel.ifexists("StatusName", "code", "Name", reader.GetValue(8).ToString()).Trim();
                                    }
                                    else
                                    {
                                        m.STATUS_ID = dbexcel.getNextIdStatusName();
                                        M_StatusName c = new M_StatusName
                                        {
                                            COMPANY_CODE = companycode,
                                            Code = m.STATUS_ID,
                                            Locked = false,
                                            Name = reader.GetValue(8).ToString(),
                                            Sort = 0


                                        };
                                        _context.StatusName.Add(c);
                                        await _context.SaveChangesAsync();
                                    }



                                    if (
                                   dbexcel.ifexists("BusinessLocation", "code", "Name", reader.GetValue(9).ToString()).Trim().Length > 0
                                   )
                                    {
                                        m.BUSSINESS_LOCATION_ID = dbexcel.ifexists("BusinessLocation", "code", "Name", reader.GetValue(9).ToString()).Trim();
                                    }
                                    else
                                    {
                                        m.BUSSINESS_LOCATION_ID = dbexcel.getNextIdCategory();
                                        M_BusinessLocation c = new M_BusinessLocation
                                        {
                                            COMPANY_CODE = companycode,
                                            Code = m.BUSSINESS_LOCATION_ID,
                                            Locked = false,
                                            Name = reader.GetValue(9).ToString(),
                                            Sort = 0


                                        };
                                        _context.BusinessLocation.Add(c);
                                        await _context.SaveChangesAsync();
                                    }


                                    m.Alter_Qty = Convert.ToDecimal(reader.GetValue(10));



                                    if (
                                 dbexcel.ifexists("ProductTypeId", "code", "Name", reader.GetValue(11).ToString()).Trim().Length > 0
                                 )
                                    {
                                        m.BUSSINESS_LOCATION_ID = dbexcel.ifexists("ProductTypeId", "code", "Name", reader.GetValue(11).ToString()).Trim();
                                    }
                                    else
                                    {
                                        m.BUSSINESS_LOCATION_ID = dbexcel.getNextIdProductTypeId();
                                        M_ProductTypeId c = new M_ProductTypeId
                                        {
                                            COMPANY_CODE = companycode,
                                            Code = m.BUSSINESS_LOCATION_ID,
                                            Locked = false,
                                            Name = reader.GetValue(11).ToString(),
                                            Sort = 0


                                        };
                                        _context.ProductTypeId.Add(c);
                                        await _context.SaveChangesAsync();
                                    }


                                    if (
                               dbexcel.ifexists("Department", "code", "Name", reader.GetValue(12).ToString()).Trim().Length > 0
                               )
                                    {
                                        m.WAREHOUSE_ID = dbexcel.ifexists("Department", "code", "Name", reader.GetValue(12).ToString()).Trim();
                                    }
                                    else
                                    {
                                        m.WAREHOUSE_ID = dbexcel.getNextIdDepartment();
                                        M_Department c = new M_Department
                                        {
                                            COMPANY_CODE = companycode,
                                            Code = m.WAREHOUSE_ID,
                                            Locked = false,
                                            Name = reader.GetValue(12).ToString(),
                                            Sort = 0


                                        };
                                        _context.Department.Add(c);
                                        await _context.SaveChangesAsync();
                                    }
                                    m.BATCH_NO = reader.GetValue(13).ToString();


                                    if (
                              dbexcel.ifexists("color", "code", "Name", reader.GetValue(14).ToString()).Trim().Length > 0
                              )
                                    {
                                        m.Colour = dbexcel.ifexists("color", "code", "Name", reader.GetValue(14).ToString()).Trim();
                                    }
                                    else
                                    {
                                        m.Colour = dbexcel.getNextIdDepartment();
                                        M_Color c = new M_Color
                                        {
                                            COMPANY_CODE = companycode,
                                            Code = m.Colour,
                                            Locked = false,
                                            Name = reader.GetValue(14).ToString(),
                                            Sort = 0


                                        };
                                        _context.Color.Add(c);
                                        await _context.SaveChangesAsync();
                                    }


                                    m.MEASURING_UNIT_ID = reader.GetValue(15).ToString();

                                    if (
                         dbexcel.ifexists("MeasuringUnit", "code", "Name", reader.GetValue(15).ToString()).Trim().Length > 0
                         )
                                    {
                                        m.MEASURING_UNIT_ID = dbexcel.ifexists("MeasuringUnit", "code", "Name", reader.GetValue(14).ToString()).Trim();
                                    }
                                    else
                                    {
                                        m.MEASURING_UNIT_ID = dbexcel.getNextIdMeasuringUnit();
                                        M_MeasuringUnit c = new M_MeasuringUnit
                                        {
                                            COMPANY_CODE = companycode,
                                            Code = m.MEASURING_UNIT_ID,
                                            Locked = false,
                                            Name = reader.GetValue(15).ToString(),
                                            Sort = 0


                                        };
                                        _context.MeasuringUnit.Add(c);
                                        await _context.SaveChangesAsync();
                                    }

                                    m.PURCHASE_RATE = Convert.ToDecimal(reader.GetValue(16));
                                    m.SALE_RATE = Convert.ToDecimal(reader.GetValue(17));


                                    m.Packing_Qty = Convert.ToDecimal(reader.GetValue(18));
                                    m.Whole_Sale_Rate = Convert.ToDecimal(reader.GetValue(19));
                                    m.Retail_Rate = Convert.ToDecimal(reader.GetValue(20));
                                    m.MAX_LEVEL = Convert.ToDecimal(reader.GetValue(21));
                                    m.MIN_LEVEL = Convert.ToDecimal(reader.GetValue(22));
                                    m.UpperBox = Convert.ToDecimal(reader.GetValue(23));
                                    m.InnerBox = Convert.ToDecimal(reader.GetValue(24));
                                    m.PacketBox = Convert.ToDecimal(reader.GetValue(25));
                                    m.UpperPrice = Convert.ToDecimal(reader.GetValue(26));
                                    m.InnerPrice = Convert.ToDecimal(reader.GetValue(27));
                                    m.PacketPrice = Convert.ToDecimal(reader.GetValue(28));
                                    m.SHORT_NAME = reader.GetValue(29).ToString();
                                    m.SKU = reader.GetValue(30).ToString();
                                    m.EXPIRAY_DATE = Convert.ToDateTime(stringDate);
                                    m.EXPIRAY_ALERT_ID = reader.GetValue(32).ToString();
                                    m.EXPIRAY_ALERT = Convert.ToDecimal(reader.GetValue(33));
                                    m.MP_NUMBER = reader.GetValue(34).ToString();
                                    m.SP_NUMBER = reader.GetValue(35).ToString();
                                    m.WARRENTY_DAYS = Convert.ToDecimal(reader.GetValue(36));
                                    m.MODEL_NUMBER = reader.GetValue(37).ToString();
                                    m.WEIGHT = Convert.ToDecimal(reader.GetValue(38));
                                    m.DESCRIPTION = reader.GetValue(39).ToString();
                                    m.IMG_PATH = reader.GetValue(40).ToString();
                                    m.CURRENT_STOCK = Convert.ToDecimal(reader.GetValue(41));
                                    m.REORDER_QUANTITY = Convert.ToDecimal(reader.GetValue(42));

                                    if (
                                 dbexcel.ifexists("LabelFormate", "code", "Name", reader.GetValue(43).ToString()).Trim().Length > 0
                                 )
                                    {
                                        m.LABEL_FORMATE = dbexcel.ifexists("LabelFormate", "code", "Name", reader.GetValue(43).ToString()).Trim();
                                    }
                                    else
                                    {
                                        m.LABEL_FORMATE = dbexcel.getNextIdLabelFormate();
                                        M_LabelFormate c = new M_LabelFormate
                                        {
                                            COMPANY_CODE = companycode,
                                            Code = m.LABEL_FORMATE,
                                            Locked = false,
                                            Name = reader.GetValue(43).ToString(),
                                            Sort = 0


                                        };
                                        _context.LabelFormate.Add(c);
                                        await _context.SaveChangesAsync();
                                    }




                                    m.PACKED_DATE = Convert.ToDateTime(stringDate);
                                    m.USED_DATE = Convert.ToDateTime(stringDate);
                                    m.SELL_DATE = Convert.ToDateTime(stringDate);
                                    m.NOTSALE_QUANTITY = Convert.ToDecimal(reader.GetValue(47));
                                    m.DISCOUNT_RATE = Convert.ToDecimal(reader.GetValue(48));
                                    m.CURRENT_OFFER = reader.GetValue(49).ToString();
                                    if (
                               dbexcel.ifexists("CurrentOffer", "code", "Name", reader.GetValue(49).ToString()).Trim().Length > 0
                               )
                                    {
                                        m.CURRENT_OFFER = dbexcel.ifexists("CurrentOffer", "code", "Name", reader.GetValue(49).ToString()).Trim();
                                    }
                                    else
                                    {
                                        m.CURRENT_OFFER = dbexcel.getNextIdCurrentOffer();
                                        M_CurrentOffer c = new M_CurrentOffer
                                        {
                                            COMPANY_CODE = companycode,
                                            Code = m.CURRENT_OFFER,
                                            Locked = false,
                                            Name = reader.GetValue(49).ToString(),
                                            Sort = 0


                                        };
                                        _context.CurrentOffer.Add(c);
                                        await _context.SaveChangesAsync();
                                    }
                                    m.OFFERDAY_ID = reader.GetValue(50).ToString();
                                    m.OFFER_START_DATE = Convert.ToDateTime(stringDate);
                                    m.OFFER_END_DATE = Convert.ToDateTime(stringDate);
                                    m.RACK_NO = reader.GetValue(53).ToString();
                                    m.ROW_NO = reader.GetValue(54).ToString();
                                    m.POSITION = reader.GetValue(55).ToString();
                                    m.FRONT_LOCATION_ID = reader.GetValue(56).ToString();
                                    m.BACK_LOCATION_ID = reader.GetValue(57).ToString();
                                    m.MULTI_BARCODE_ID = reader.GetValue(58).ToString();
                                    m.CREATED_ON = Convert.ToDateTime(stringDate);
                                    m.CREATED_BY = reader.GetValue(60).ToString();
                                    m.IS_DELETED = Convert.ToBoolean(reader.GetValue(61));
                                    m.DELETED_BY = reader.GetValue(62).ToString();
                                    m.DELETEED_ON = Convert.ToDateTime(stringDate);
                                    m.UPDATE_BY = reader.GetValue(64).ToString();
                                    m.UPDATE_ON = Convert.ToDateTime(stringDate);


                                    m.ITEM_CODE = dbset.getUpdateMasterCount(m.COMPANY_CODE, m.CATEGORY_ID, m.SUB_CATEGORY_ID, m.SUB_CATEGORY_DETAIL_ID);
                                    if (m.ITEM_BARCODE == "0")
                                    {
                                        m.ITEM_BARCODE = m.ITEM_CODE;
                                    }
                                    _context.Product_Information.Add(m);
                                    await _context.SaveChangesAsync();
                                }
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
        [HttpPost, DisableRequestSizeLimit]
        [Route("Upload/rates")]
        public async Task<IActionResult> Upload1()
        {
          
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
                        M_ExcelSheet m;
                        int jojo = 0;
                        while (reader.Read()) //Each row of the file
                        {
                            if (jojo == 1)
                            {
                                Console.WriteLine("Upload Rates is Ready to Update");
                            Console.WriteLine(reader.GetValue(0).ToString());
                            Console.WriteLine(reader.GetValue(1).ToString());
                          //  Console.WriteLine(Convert.ToDecimal(reader.GetValue(2)));
                           // Console.WriteLine(Convert.ToDecimal(reader.GetValue(4)));
                            m = new M_ExcelSheet
                            {

                                ITEM_CODE = reader.GetValue(0).ToString(),
                                ITEM_NAME = reader.GetValue(1).ToString(),
                                PURCHASE_RATE = Convert.ToDecimal(reader.GetValue(2) ?? 0),
                                SALE_RATE = Convert.ToDecimal(reader.GetValue(3) ?? 0),
                             
                            };
                           
                            var obj = await _context.Product_Information.FindAsync(companycode, m.ITEM_CODE);
                          
                                            if (obj != null)
                                        {

                                            obj.ITEM_CODE = reader.GetValue(0).ToString();
                                            obj.ITEM_NAME = reader.GetValue(1).ToString();
                                            obj.PURCHASE_RATE = Convert.ToDecimal(reader.GetValue(2) ?? 0);
                                            obj.SALE_RATE = Convert.ToDecimal(reader.GetValue(3) ?? 0);
                              

                                            _context.Entry(obj).State = EntityState.Modified;
                                            await _context.SaveChangesAsync();

                                        }
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

        //[HttpGet]
        //[Route("GetLevel2WithLevel1")]
        //public async Task<ActionResult<IEnumerable<M_ItemInformation>>> GetLevel2WithLevel1(string id)
        //{

        //    var m = await _context.Product_Information.Where(x => x.Level1Code == id).ToListAsync();

        //    //  var m = await _context.Chart_of_Accounts.FindAsync().ToList();// Include(i => i.Code).FirstOrDefaultAsync(i => i.Code == id);
        //    //    var m= await _context.Chart_of_Accounts.Where(s => s.Code.Contains(id));

        //    if (m == null)
        //    {
        //        return NotFound();
        //    }

        //    return m;
        //}


        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> update(M_ItemInformation m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                M_ExcelSheet obj = new M_ExcelSheet();
                obj = await _context.Product_Information.FindAsync(companycode, m.ITEM_CODE);

                if (obj != null)
                {
                    obj.COMPANY_CODE = m.COMPANY_CODE;
                    obj.ITEM_CODE = m.ITEM_CODE;
                    obj.ITEM_NAME = m.ITEM_NAME;
                    obj.ITEM_BARCODE = m.ITEM_BARCODE;
                    obj.CATEGORY_ID = m.CATEGORY_ID;
                    obj.SUB_CATEGORY_ID = m.SUB_CATEGORY_ID;
                    obj.SUB_CATEGORY_DETAIL_ID = m.SUB_CATEGORY_DETAIL_ID;
                    obj.BARCODE_TYPE_ID = m.BARCODE_TYPE_ID;
                    obj.STATUS_ID = m.STATUS_ID;
                    obj.BUSSINESS_LOCATION_ID = m.BUSSINESS_LOCATION_ID;
                    obj.Alter_Qty = m.Alter_Qty;
                    obj.PRODUCT_TYPE_ID = m.PRODUCT_TYPE_ID;
                    obj.WAREHOUSE_ID = m.WAREHOUSE_ID;
                    obj.BATCH_NO = m.BATCH_NO;
                    obj.Colour = m.Colour;
                    obj.MEASURING_UNIT_ID = m.MEASURING_UNIT_ID;
                    obj.PURCHASE_RATE = m.PURCHASE_RATE;
                    obj.SALE_RATE = m.SALE_RATE;
                    obj.Packing_Qty = m.Packing_Qty;
                    obj.Whole_Sale_Rate = m.Whole_Sale_Rate;
                    obj.Retail_Rate = m.Retail_Rate;
                    obj.MAX_LEVEL = m.MAX_LEVEL;
                    obj.MIN_LEVEL = m.MIN_LEVEL;
                    obj.UpperBox = m.UpperBox;
                    obj.InnerBox = m.InnerBox;
                    obj.PacketBox = m.PacketBox;
                    obj.UpperPrice = m.UpperPrice;
                    obj.InnerPrice = m.InnerPrice;
                    obj.PacketPrice = m.PacketPrice;
                    obj.SHORT_NAME = m.SHORT_NAME;
                    obj.SKU = m.SKU;
                    obj.EXPIRAY_DATE = m.EXPIRAY_DATE;
                    obj.EXPIRAY_ALERT_ID = m.EXPIRAY_ALERT_ID;
                    obj.EXPIRAY_ALERT = m.EXPIRAY_ALERT;
                    obj.MP_NUMBER = m.MP_NUMBER;
                    obj.SP_NUMBER = m.SP_NUMBER;
                    obj.WARRENTY_DAYS = m.WARRENTY_DAYS;
                    obj.MODEL_NUMBER = m.MODEL_NUMBER;
                    obj.WEIGHT = m.WEIGHT;
                    obj.DESCRIPTION = m.DESCRIPTION;
                    obj.IMG_PATH = m.IMG_PATH;
                    obj.CURRENT_STOCK = m.CURRENT_STOCK;
                    obj.REORDER_QUANTITY = m.REORDER_QUANTITY;
                    obj.LABEL_FORMATE = m.LABEL_FORMATE;
                    obj.PACKED_DATE = m.PACKED_DATE;
                    obj.USED_DATE = m.USED_DATE;
                    obj.SELL_DATE = m.SELL_DATE;
                    obj.NOTSALE_QUANTITY = m.NOTSALE_QUANTITY;
                    obj.CURRENT_OFFER = m.CURRENT_OFFER;
                    obj.DISCOUNT_RATE = m.DISCOUNT_RATE;
                    obj.OFFERDAY_ID = m.OFFERDAY_ID;
                    obj.OFFER_START_DATE = m.OFFER_START_DATE;
                    obj.OFFER_END_DATE = m.OFFER_END_DATE;
                    obj.RACK_NO = m.RACK_NO;
                    obj.ROW_NO = m.ROW_NO;
                    obj.POSITION = m.POSITION;
                    obj.FRONT_LOCATION_ID = m.FRONT_LOCATION_ID;
                    obj.BACK_LOCATION_ID = m.BACK_LOCATION_ID;
                    obj.MULTI_BARCODE_ID = m.MULTI_BARCODE_ID;
                    obj.CREATED_ON = m.CREATED_ON;
                    obj.CREATED_BY = m.CREATED_BY;
                    obj.IS_DELETED = m.IS_DELETED;
                    obj.DELETED_BY = m.DELETED_BY;
                    obj.DELETEED_ON = m.DELETEED_ON;
                    obj.UPDATE_BY = m.UPDATE_BY;
                    obj.UPDATE_ON = m.UPDATE_ON;


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

        [HttpPut("{id}")]
        [Route("update/rates")]
        public async Task<IActionResult> update(M_ExcelSheet m, string id)
        {

           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            try
            {

                M_ExcelSheet obj = new M_ExcelSheet();
                //Console.WriteLine("UPating rates");

                obj = await _context.Product_Information.FindAsync(companycode, m.ITEM_CODE);
                if (obj != null)
                {
                    Console.WriteLine("UPating rates");

                    obj.ITEM_CODE = m.ITEM_CODE;
                    obj.ITEM_NAME = m.ITEM_NAME;
                    obj.PURCHASE_RATE = m.PURCHASE_RATE;
                    obj.SALE_RATE = m.SALE_RATE;
                }
                // int i = this.obj.SaveChanges();
                _context.Entry(obj).State = EntityState.Modified;
                Console.WriteLine(obj.ITEM_NAME);
                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {
                throw;
            }
            return Ok(m);
        }

        [HttpPost]
        public async Task<ActionResult<M_ExcelSheet>> create(M_ExcelSheet m)
        {
            m.ITEM_CODE = dbset.getUpdateMasterCount(m.COMPANY_CODE, m.CATEGORY_ID, m.SUB_CATEGORY_ID, m.SUB_CATEGORY_DETAIL_ID);
            //m.ITEM_BARCODE = dbset.getUpdateMasterCount(m.COMPANY_CODE, m.CATEGORY_ID, m.SUB_CATEGORY_ID, m.SUB_CATEGORY_DETAIL_ID);
            //   Console.WriteLine(typeof(ITEM_BARCODE));
            Console.WriteLine(m.ITEM_CODE);
            if (m.ITEM_BARCODE == null)
            {
                m.ITEM_BARCODE = m.ITEM_CODE.ToString();
            }
            else if (m.ITEM_BARCODE == "")
            {
                m.ITEM_BARCODE = m.ITEM_CODE.ToString();
            }
            else
            {
                m.ITEM_BARCODE = m.ITEM_BARCODE;
            }


            _context.Product_Information.Add(m);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Get", new { id = m.ITEM_CODE }, m);
        }



        // DELETE: api/Level2/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<M_ExcelSheet>> Deletem(string id)
        {
            var m = await _context.Product_Information.FindAsync(companycode, id);
            if (m == null)
            {
                return NotFound();
            }

            _context.Product_Information.Remove(m);
            await _context.SaveChangesAsync();

            return m;
        }

    }
}
