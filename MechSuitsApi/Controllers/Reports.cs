using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreInfrastructure.Accounts.Transaction.CashJournal;
using System.Data.SqlClient;
using Executer;
using MechSuitsApi.Classes;
using Microsoft.EntityFrameworkCore;
using CoreInfrastructure.Generals;
using System.Data;
using System.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MechSuitsApi.Models;

namespace MechSuitsApi.Controllers
{
    [Route("api/reports")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ReportsController : ControllerBase
    {
        string companycode = ""; string user = "";
        D_DB dbset = null;
        GeneralFunctions general = null;
        private IHttpContextAccessor _httpContextAccessor;
        Connection c = null;
        string ls_companyCode = "", ls_start_date = "", ls_end_code = "", ls_start_code = "", ls_end_date = "", ls_user = "", ls_department = "";
        string ls_saleman = "", ls_area = "", ls_type = "", ls_Item_start_code = "", ls_item_end_code = "";


        private readonly AppDBContext _context;
        public ReportsController(AppDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            c = new Connection();
            dbset = new D_DB(c.V_connection);



        }
        [HttpGet("saleinvoicedetail")]
        public object updatepicturepath(string startcode, string endcode)
        {
            DataSet ds = new DataSet();
            if (c.V_connection.State == ConnectionState.Closed)
            {
                c.V_connection.Open();
            }
            using (SqlCommand cmd = new SqlCommand("R_SALEINVOICE", c.V_connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;



                cmd.Parameters.Add(new SqlParameter("STARTCODE", SqlDbType.VarChar)).Value = startcode;
                cmd.Parameters.Add(new SqlParameter("ENDCODE", SqlDbType.VarChar)).Value = endcode;
                cmd.Parameters.Add(new SqlParameter("COMPANYCODE", SqlDbType.VarChar)).Value = "C001";
                cmd.CommandTimeout = 300;
                cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                Object obj = ds;
                return obj;
            }




        }


        [HttpPost("GetReport")]
        public DataSet return_dataset_of_report(M_Report m)
        {
            DataSet ds = new DataSet();
            Connection cl = new Connection();


            SqlConnection con = cl.V_connection;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            using (SqlCommand cmd = new SqlCommand(m.ProcedureName, con))
            {
                ls_saleman = m.Salesman.Trim();
                ls_area = m.Area.Trim();
                ls_companyCode = "C001";
                ls_department = m.Department.Trim();
                ls_end_code = m.EndCode.Trim();
                ls_end_date = m.EndDate.ToShortDateString();
                ls_item_end_code = m.ItemEndCode.Trim();
                ls_Item_start_code = m.ItemStartCode.Trim();
                ls_start_code = m.StartCode.Trim();
                ls_start_date = m.StartDate.ToShortDateString();
                ls_type = m.Type.Trim();
                ls_user = m.User.Trim();

                cmd.CommandType = CommandType.StoredProcedure;
                switch (m.ProcedureName)
                {

                    case "R_PURCHASEINVOICE":
                    case "R_PURCHASERETURN":
                    case "R_SALERETURN":
                    case "R_SALEINVOICE":
                    case "R_GatePass":
                    //case "r_bankreceipt_detail":
                    case "r_cashreceipt_detail":
                    case "r_item_list":
                        cmd.Parameters.Add(new SqlParameter("CompanyCode", SqlDbType.VarChar)).Value = ls_companyCode;
                        cmd.Parameters.Add(new SqlParameter("STARTCODE", SqlDbType.VarChar)).Value = ls_start_code;
                        cmd.Parameters.Add(new SqlParameter("ENDCODE", SqlDbType.VarChar)).Value = ls_end_code;

                        break;
                    case "r_bankreceipt_detail":
                        cmd.Parameters.Add(new SqlParameter("Company_Code", SqlDbType.VarChar)).Value = ls_companyCode;
                        cmd.Parameters.Add(new SqlParameter("START_CODE", SqlDbType.VarChar)).Value = ls_start_code;
                        cmd.Parameters.Add(new SqlParameter("END_CODE", SqlDbType.VarChar)).Value = ls_end_code;

                        break;

                    case "r_daily_cash":
                    case "R_ACCOUNTS_RECEIVABLES_ByCompany":
                    case "R_ACCOUNTS_PAYABLE_ByCompany":
                    case "R_GatePass_SUMMARY":
                    case "R_Purchase_SUMMARY_ByCompany":
                    case "R_PurchaseReturn_SUMMARY":
                    case "R_PurchaseReturn_SUMMARY_ByCompany":
                    case "R_PURCHASESUMMARY":
                    case "R_SALEINVOICE_SUMMARY":
                    case "R_SaleReturn_SUMMARY":
                    case "R_SALERETURN_SUMMARY_ByCompany":
                    case "R_SALEINVOICE_SUMMARY_ByCompany":
                        cmd.Parameters.Add(new SqlParameter("CompanyCode", SqlDbType.VarChar)).Value = ls_companyCode;
                        cmd.Parameters.Add(new SqlParameter("STARTDATE", SqlDbType.Date)).Value = ls_start_date;
                        cmd.Parameters.Add(new SqlParameter("ENDDATE", SqlDbType.Date)).Value = ls_end_date;
                        break;
                    case "R_SALEINVOICE_SUMMARY_Department":
                        cmd.Parameters.Add(new SqlParameter("CompanyCode", SqlDbType.VarChar)).Value = ls_companyCode;
                        cmd.Parameters.Add(new SqlParameter("STARTDATE", SqlDbType.Date)).Value = ls_start_date;
                        cmd.Parameters.Add(new SqlParameter("ENDDATE", SqlDbType.Date)).Value = ls_end_date;
                        cmd.Parameters.Add(new SqlParameter("dept", SqlDbType.VarChar)).Value = ls_department;
                        break;
                    case "R_UserWISE_pURCHASESummary":
                    case "R_UserWISE_saleSummary":
                        cmd.Parameters.Add(new SqlParameter("CompanyCode", SqlDbType.VarChar)).Value = ls_companyCode;
                        cmd.Parameters.Add(new SqlParameter("STARTDATE", SqlDbType.Date)).Value = ls_start_date;
                        cmd.Parameters.Add(new SqlParameter("ENDDATE", SqlDbType.Date)).Value = ls_end_date;
                        cmd.Parameters.Add(new SqlParameter("usercode", SqlDbType.VarChar)).Value = ls_user;
                        break;
                    case "R_SALEMANWISE_SALE_AND_ITEMSALE":
                        cmd.Parameters.Add(new SqlParameter("CompanyCode", SqlDbType.VarChar)).Value = ls_companyCode;
                        cmd.Parameters.Add(new SqlParameter("STARTDATE", SqlDbType.Date)).Value = ls_start_date;
                        cmd.Parameters.Add(new SqlParameter("ENDDATE", SqlDbType.Date)).Value = ls_end_date;
                        cmd.Parameters.Add(new SqlParameter("saleman", SqlDbType.VarChar)).Value = ls_saleman;
                        cmd.Parameters.Add(new SqlParameter("STARTCODE", SqlDbType.VarChar)).Value = ls_start_code;
                        cmd.Parameters.Add(new SqlParameter("ENDCODE", SqlDbType.VarChar)).Value = ls_end_code;
                        break;
                    case "R_DAILY_SALE":
                        cmd.Parameters.Add(new SqlParameter("CompanyCode", SqlDbType.VarChar)).Value = ls_companyCode;
                        cmd.Parameters.Add(new SqlParameter("CURRENT_DATE", SqlDbType.VarChar)).Value = ls_start_date;
                        break;
                    case "R_DAILY_STOCK":
                        cmd.Parameters.Add(new SqlParameter("CompanyCode", SqlDbType.VarChar)).Value = ls_companyCode;

                        cmd.Parameters.Add(new SqlParameter("ENDDATE", SqlDbType.Date)).Value = ls_end_date;
                        cmd.Parameters.Add(new SqlParameter("STARTCODE", SqlDbType.VarChar)).Value = ls_start_code;
                        cmd.Parameters.Add(new SqlParameter("ENDCODE", SqlDbType.VarChar)).Value = ls_end_code;
                        break;
                    case "ledger":

                        cmd.Parameters.Add(new SqlParameter("CompanyCode", SqlDbType.VarChar)).Value = ls_companyCode;
                        cmd.Parameters.Add(new SqlParameter("STARTDATE", SqlDbType.Date)).Value = ls_start_date;
                        cmd.Parameters.Add(new SqlParameter("ENDDATE", SqlDbType.Date)).Value = ls_end_date;
                        cmd.Parameters.Add(new SqlParameter("STARTCODE", SqlDbType.VarChar)).Value = ls_start_code;
                        cmd.Parameters.Add(new SqlParameter("ENDCODE", SqlDbType.VarChar)).Value = ls_end_code;
                        break;
                    case "R_ACCOUNTS_PAYABLE":
                    case "R_ACCOUNTS_RECEIVABLES":
                        cmd.Parameters.Add(new SqlParameter("CompanyCode", SqlDbType.VarChar)).Value = ls_companyCode;
                        cmd.Parameters.Add(new SqlParameter("STARTDATE", SqlDbType.Date)).Value = ls_start_date;
                        cmd.Parameters.Add(new SqlParameter("ENDDATE", SqlDbType.Date)).Value = ls_end_date;
                        cmd.Parameters.Add(new SqlParameter("PartyCode", SqlDbType.VarChar)).Value = ls_start_code;

                        break;
                    case "R_ACCOUNTS_PAYABLE_AREAWISE":
                    case "R_ACCOUNTS_RECEIVABLES_AREAWISE":


                        cmd.Parameters.Add(new SqlParameter("CompanyCode", SqlDbType.VarChar)).Value = ls_companyCode;
                        cmd.Parameters.Add(new SqlParameter("STARTDATE", SqlDbType.Date)).Value = ls_start_date;
                        cmd.Parameters.Add(new SqlParameter("ENDDATE", SqlDbType.Date)).Value = ls_end_date;
                        cmd.Parameters.Add(new SqlParameter("Area", SqlDbType.VarChar)).Value = ls_area;

                        break;
                    case "R_BANKBOOK":
                        cmd.Parameters.Add(new SqlParameter("CompanyCode", SqlDbType.VarChar)).Value = ls_companyCode;
                        cmd.Parameters.Add(new SqlParameter("STARTDATE", SqlDbType.Date)).Value = ls_start_date;
                        cmd.Parameters.Add(new SqlParameter("ENDDATE", SqlDbType.Date)).Value = ls_end_date;
                        cmd.Parameters.Add(new SqlParameter("ACCOUNTCODE", SqlDbType.VarChar)).Value = ls_start_code;

                        break;
                    case "r_bankpayment_summary_ByCompany":
                    case "r_cashreceipt_summary_ByCompany":
                        cmd.Parameters.Add(new SqlParameter("CompanyCode", SqlDbType.VarChar)).Value = ls_companyCode;
                        cmd.Parameters.Add(new SqlParameter("STARTDATE", SqlDbType.Date)).Value = ls_start_date;
                        cmd.Parameters.Add(new SqlParameter("ENDDATE", SqlDbType.Date)).Value = ls_end_date;
                        cmd.Parameters.Add(new SqlParameter("type", SqlDbType.VarChar)).Value = ls_type;

                        break;
                    case "R_CUSTOMERWISE_SALES":
                    case "R_ITEM_LEDGER_Carton_PCS":
                    case "r_item_wise_purchase":
                    case "r_itemwise_sale":
                    case "R_ITEM_MOVEMENT":
                        cmd.Parameters.Add(new SqlParameter("CompanyCode", SqlDbType.VarChar)).Value = ls_companyCode;
                        cmd.Parameters.Add(new SqlParameter("STARTDATE", SqlDbType.Date)).Value = ls_start_date;
                        cmd.Parameters.Add(new SqlParameter("ENDDATE", SqlDbType.Date)).Value = ls_end_date;
                        cmd.Parameters.Add(new SqlParameter("STARTCODE", SqlDbType.VarChar)).Value = ls_start_code;
                        cmd.Parameters.Add(new SqlParameter("ENDCODE", SqlDbType.VarChar)).Value = ls_end_code;
                        break;
                    case "r_Customer_item_sale":
                        cmd.Parameters.Add(new SqlParameter("CompanyCode", SqlDbType.VarChar)).Value = ls_companyCode;
                        cmd.Parameters.Add(new SqlParameter("STARTDATE", SqlDbType.Date)).Value = ls_start_date;
                        cmd.Parameters.Add(new SqlParameter("ENDDATE", SqlDbType.Date)).Value = ls_end_date;
                        cmd.Parameters.Add(new SqlParameter("STARTCODE", SqlDbType.VarChar)).Value = ls_start_code;
                        cmd.Parameters.Add(new SqlParameter("ENDCODE", SqlDbType.VarChar)).Value = ls_end_code;
                        cmd.Parameters.Add(new SqlParameter("ITEMSTARTCODE", SqlDbType.VarChar)).Value = ls_Item_start_code;
                        cmd.Parameters.Add(new SqlParameter("ITEMENDCODE", SqlDbType.VarChar)).Value = ls_item_end_code;
                        break;
                    case "R_INVENTORY_ALL":
                        cmd.Parameters.Add(new SqlParameter("CompanyCode", SqlDbType.VarChar)).Value = ls_companyCode;
                        cmd.Parameters.Add(new SqlParameter("STARTDATE", SqlDbType.Date)).Value = ls_start_date;
                        cmd.Parameters.Add(new SqlParameter("ENDDATE", SqlDbType.Date)).Value = ls_end_date;

                        break;
                    case "r_item_label":
                        cmd.Parameters.Add(new SqlParameter("CompanyCode", SqlDbType.VarChar)).Value = ls_companyCode;
                        cmd.Parameters.Add(new SqlParameter("STARTCODE", SqlDbType.VarChar)).Value = ls_start_code;
                        break;
                }

                cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }





        }
    }
}
