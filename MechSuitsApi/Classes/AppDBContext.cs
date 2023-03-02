
using Microsoft.EntityFrameworkCore;
using CoreInfrastructure.Accounts.Setup;
using CoreInfrastructure.GeneralSetting;
using CoreInfrastructure.Accounts.Transaction;
using CoreInfrastructure.ItemInformation.ItemCategory;
using CoreInfrastructure.GeneralSetting.BusinessLocation;
using CoreInfrastructure.GeneralSetting.StatusName;
using CoreInfrastructure.GeneralSetting.M_BarCodeType;
using CoreInfrastructure.Auth.Menu;
using CoreInfrastructure.ItemInformation.ItemSubCategory;
using CoreInfrastructure.ItemInformation.ItemSubCategoryDetail;
using CoreInfrastructure.ItemInformation.ItemInformation;
using CoreInfrastructure.GeneralSetting.Area;
using CoreInfrastructure.GeneralSetting.department;
using CoreInfrastructure.GeneralSetting.City;
using CoreInfrastructure.GeneralSetting.Country;
using CoreInfrastructure.GeneralSetting.Salesman;
using CoreInfrastructure.GeneralSetting.MeasuringUnit;
using CoreInfrastructure.ItemInformation.ItemOpening;
using CoreInfrastructure.ItemInformation.productlabel;
using CoreInfrastructure.ItemInformation.Promocode;
using CoreInfrastructure.Sale.SaleInvoice;
using CoreInfrastructure.Sale.SaleOrder;
using CoreInfrastructure.Auth.User;
using CoreInfrastructure.Auth.NewUsers;
using CoreInfrastructure.Purchase.PurchaseInvoice;
using CoreInfrastructure.Purchase.PurchaseReturn;
using CoreInfrastructure.Sale.SaleReturn;
using CoreInfrastructure.Sale.Stocktransfer;
using CoreInfrastructure.Auth.Fiscalyear;
using CoreInfrastructure.Hr.Setup;
using CoreInfrastructure.Purchase.PurchaseOrder;
using CoreInfrastructure.ItemInformation.ItemComposition;
using CoreInfrastructure.ItemInformation.ProductDiscount;
using CoreInfrastructure.Auth.Roles;
using CoreInfrastructure.Auth.Company;
using CoreInfrastructure.UsersRoles;

using CoreInfrastructure.GeneralSetting.CurrentOffer;
using CoreInfrastructure.GeneralSetting.LabelFormate;
using CoreInfrastructure.GeneralSetting.ProductTypeId;
using CoreInfrastructure.GeneralSetting.Color;
using CoreInfrastructure.ItemInformation.Barcode;
using CoreInfrastructure.Sale;
using CoreInfrastructure.Sale.Holdsales;
using CoreInfrastructure.GeneralSetting.Age;
using CoreInfrastructure.GeneralSetting.Appearance;
using CoreInfrastructure.GeneralSetting.WorkStatus;
using CoreInfrastructure.GeneralSetting.Visas;
using CoreInfrastructure.GeneralSetting.Arrivals;
using CoreInfrastructure.GeneralSetting.Experience;
using CoreInfrastructure.GeneralSetting.MaritalCondition;
using CoreInfrastructure.GeneralSetting.RequestStatus;
using CoreInfrastructure.GeneralSetting.Terminals;
using CoreInfrastructure.Customers;
using CoreInfrastructure.toolbarItems;
using CoreInfrastructure.ToolbarItems;
using CoreInfrastructure.AccomodationSystem;
using CoreInfrastructure.Recruitement;
using CoreInfrastructure.SpnoserShip;
using CoreInfrastructure.Accounts.Transaction.BankJournal;
using System;
using CoreInfrastructure.Accounts.Transaction.CashJournal;
using CoreInfrastructure.Accounts.Transaction.Expense;
using CoreInfrastructure.GeneralSetting.Professions;
using CoreInfrastructure.GeneralSetting.SystemConfiguration;
using CoreInfrastructure.GeneralSetting.Office;
using CoreInfrastructure.GeneralSetting.Chat;
using CoreInfrastructure.Recruitement.Musaned;
using CoreInfrastructure.VisaRequest;
using CoreInfrastructure.Hr.Setup.Attendance;
using CoreInfrastructure.Recruitement.RecruitmentOrder;
//using M_RecruitementOrder = CoreInfrastructure.Recruitement.M_RecruitementOrder;
using CoreInfrastructure.Recruitement.Philippine;
using M_RecruitementOrder = CoreInfrastructure.Recruitement.RecruitmentOrder.M_RecruitementOrder;


//using CoreInfrastructure.ItemInformation.ProductInformation;

namespace MechSuitsApi.Classes
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<CoreInfrastructure.Accounts.Transaction.CashJournal.M_Header>().HasBaseType((Type)null);
            //modelBuilder.Entity<CoreInfrastructure.Accounts.Transaction.Expense.M_Header>().HasBaseType((Type)null);
            //modelBuilder.Entity<CoreInfrastructure.Accounts.Transaction.GeneralJournal.M_Header>().HasBaseType((Type)null);
            //modelBuilder.Entity<CoreInfrastructure.Accounts.Transaction.BankJournal.M_Header>().HasBaseType((Type)null);
            modelBuilder.Entity<M_ProductControl>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_Opening>()
                    .HasKey(o => new {
                        o.Company_Code,
                        o.AccountCode
                    });
            modelBuilder.Entity<M_Opening>()
                    .HasKey(o => new {
                        o.Company_Code,
                        o.AccountCode
                    });
            ////////////////
            modelBuilder.Entity<M_Level2>()
                    .HasKey(o => new {
                        o.Company_Code,
                        o.Level1Code,
                        o.Code
                    });
            modelBuilder.Entity<M_Company>()
                    .HasKey(o => new {
                        o.COMPANY_CODE
                    });
            modelBuilder.Entity<M_Level3>()
                    .HasKey(o => new {
                        o.Company_Code,
                        o.Level1Code,
                        o.Level2Code,
                        o.Code
                    });
            // items
            modelBuilder.Entity<M_ItemCategory>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.Code
                    });
            modelBuilder.Entity<M_StatusName>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.Code
                    });
            modelBuilder.Entity<M_BarCodeType>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.Code
                    });
            modelBuilder.Entity<M_BusinessLocation>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.Code
                    });
            modelBuilder.Entity<M_CurrentOffer>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.Code
                    });
            modelBuilder.Entity<M_LabelFormate>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.Code
                    });
            modelBuilder.Entity<M_BarcodeGenerate>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.Barcode
                    });
            modelBuilder.Entity<M_ProductTypeId>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.Code
                    });
            // items
            modelBuilder.Entity<M_ProductDiscount>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.Code
                    });
            modelBuilder.Entity<M_Delegation>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_SubCategory>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.Level1Code,
                        o.Code
                    });
            modelBuilder.Entity<M_SubCategoryDetail>()
                    .HasKey(o => new {
                        o.Company_Code,
                        o.Level1Code,
                        o.Level2Code,
                        o.Code
                    });
            modelBuilder.Entity<M_ItemInformation>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.ITEM_CODE
                    });
            modelBuilder.Entity<M_Area>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.Code
                    });
            //start rabita
            modelBuilder.Entity<M_VisaRequest>()
                    .HasKey(o => new {
                        o.CODE
                    });
            // modelBuilder.Entity<M_Agents>()
            //.HasKey(o => new { o.ID }); 
            modelBuilder.Entity<M_LaborPrices>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_Age>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.Code
                    });
            modelBuilder.Entity<M_SystemConfiguration>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.Code
                    });
            modelBuilder.Entity<M_Office>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.Code
                    });
            modelBuilder.Entity<M_Chat>()
                    .HasKey(o => new {
                        o.Company_Code,
                        o.Code
                    });
            modelBuilder.Entity<M_Appearance>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.Code
                    });
            modelBuilder.Entity<M_Arrivals>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.Code
                    });
            modelBuilder.Entity<M_Experience>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.Code
                    });
            modelBuilder.Entity<M_MaritalCondition>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.Code
                    });
            modelBuilder.Entity<M_RequestStatus>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.Code
                    });
            modelBuilder.Entity<M_Terminals>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.Code
                    });
            modelBuilder.Entity<M_Visas>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.Code
                    });
            modelBuilder.Entity<M_WorkStatus>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.Code
                    });
            modelBuilder.Entity<M_Profession>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.Code
                    });
            // Recruitement 
            //end rabita
            modelBuilder.Entity<M_Color>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.Code
                    });
            modelBuilder.Entity<M_Roles>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.code
                    });
            modelBuilder.Entity<M_Department>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.Code
                    });
            modelBuilder.Entity<M_Country>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.Code
                    });
            modelBuilder.Entity<M_City>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.Code
                    });
            modelBuilder.Entity<M_Salesman>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.Code
                    });
            modelBuilder.Entity<M_MeasuringUnit>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.Code
                    });
            modelBuilder.Entity<M_itemOpening>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.ITEMCODE
                    });
            modelBuilder.Entity<M_SaleHeader>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.INVOICECODE
                    });
            modelBuilder.Entity<M_SaleDetail>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.INVOICECODE,
                        o.SEQNO
                    });
            modelBuilder.Entity<M_SHOLDHEADER>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.INVOICECODE
                    });
            modelBuilder.Entity<M_SHOLDDETAIL>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.INVOICECODE,
                        o.SEQNO
                    });
            modelBuilder.Entity<M_Fiscalyear>()
                    .HasKey(o => new {
                        o.CompanyCode,
                        o.Year
                    });
            modelBuilder.Entity<M_RecruitementAnnex>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_OrderHeader>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.INVOICECODE
                    });
            modelBuilder.Entity<M_OrderDetail>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.INVOICECODE,
                        o.SEQNO
                    });
            modelBuilder.Entity<M_POHeader>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.INVOICECODE
                    });
            modelBuilder.Entity<M_PODetail>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.INVOICECODE,
                        o.SEQNO
                    });
            modelBuilder.Entity<M_Users>()
                    .HasKey(o => new {
                        o.ID,
                        o.USER_CODE,
                        o.ROLE_CODE
                    });
            modelBuilder.Entity<M_NewUser>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.CODE,
                        o.ROLE_CODE
                    });
            modelBuilder.Entity<M_PurchaseHeader>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.INVOICECODE
                    });
            modelBuilder.Entity<M_PurchaseDetail>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.INVOICECODE,
                        o.SEQNO
                    });
            modelBuilder.Entity<M_SponsorshipTransferRefund>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_SponsorshipTransferRequest>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_RecruitementMusanedHeader>()
                    .HasKey(o => new {
                        o.CODE
                    });
            modelBuilder.Entity<M_RecruitementMusanedDetails>()
                    .HasKey(o => new {
                        o.CODE,
                        o.SEQNO
                    });
            modelBuilder.Entity<M_HrAttendance_Header>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_HrAttendance_Detail>()
                    .HasKey(o => new {
                        o.Code,
                        o.SeqNo
                    });
            modelBuilder.Entity<M_StaffImprint>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_Rack>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_PRHeader>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.INVOICECODE
                    });
            modelBuilder.Entity<M_PRDetail>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.INVOICECODE,
                        o.SEQNO
                    });
            modelBuilder.Entity<M_SRHeader>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.INVOICECODE
                    });
            modelBuilder.Entity<M_SRDetail>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.INVOICECODE,
                        o.SEQNO
                    });
            modelBuilder.Entity<M_ExcelSheet>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.ITEM_CODE
                    });
            modelBuilder.Entity<M_StockTransfer>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.documentno
                    });
            modelBuilder.Entity<M_productlabel>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.documentno
                    });
            modelBuilder.Entity<M_Promocode>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.documentno
                    });
            modelBuilder.Entity<M_Departments>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_HrEmployers>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_HrShiftDetails>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_HrShift>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_HrDocuments>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_HrSetting>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_Designation>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_Sections>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_Gazette_Holidays>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_Shiftsetup>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_EmployeeProfile>()
                    .HasKey(o => new {
                        o.Code,
                    });
            modelBuilder.Entity<M_HrSalary>()
                    .HasKey(o => new {
                        o.Code,
                    });
            modelBuilder.Entity<M_LeaveApplication>()
                    .HasKey(o => new {
                        o.DOCUMENT_NO,
                    });
            modelBuilder.Entity<M_Advance>()
                    .HasKey(o => new {
                        o.ID,
                    });
            modelBuilder.Entity<M_Loan>()
                    .HasKey(o => new {
                        o.DOCUMENT_NO,
                    });
            modelBuilder.Entity<M_Refund>()
                    .HasKey(o => new {
                        o.DOCUMENT_NO,
                    });
            modelBuilder.Entity<M_Vehicles>()
                    .HasKey(o => new {
                        o.CODE,
                    });
            modelBuilder.Entity<M_OpeningBalance>()
                    .HasKey(o => new {
                        o.PeriodID,
                    });
            modelBuilder.Entity<M_Positions>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_AttendanceTable>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.ATTENDANCE_DATE,
                        o.EMPLOYEE_CODE
                    });
            modelBuilder.Entity<M_ItemCompositionHeader>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.ITEMCODE
                    });
            modelBuilder.Entity<M_ItemCompositionDetail>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.ItemCode,
                        o.SEQNO,
                        o.PARENTCODE
                    });
            modelBuilder.Entity<M_ModalDetail>()
                    .HasKey(o => new {
                        o.Company_Code,
                        o.ROLE_ID,
                        o.MENU_ID,
                    });
            modelBuilder.Entity<M_UsersRoles>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.ROLE_ID,
                        o.MENU_ID,
                    });
            modelBuilder.Entity<CoreInfrastructure.Accounts.Transaction.ShiftClose.M_Header>()
                    .HasKey(o => new {
                        o.COMPANY_CODE,
                        o.VoucherId
                    });
            modelBuilder.Entity<M_RecruitementTransmittalHeader>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_RecruitementTransmittalDetail>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_Main_Account>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_ProductTax>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_RecruitementOrder>()
                    .HasKey(o => new {
                        o.OrderNumber
                    });
            modelBuilder.Entity<M_ProductType>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_ProductTags>()
                    .HasKey(o => new {
                        o.Code
                    });
            //modelBuilder.Entity<M_ProductInformation>()
            //.HasKey(o => new { o.COMPANY_CODE, o.ITEM_CODE });
            modelBuilder.Entity<M_CandidateReservation>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_OrderUpdate>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_ClientOrderUpdate>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_OrderDocuments>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_Agent>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_Order>()
                    .HasKey(o => new {
                        o.OrderNumber
                    });
            modelBuilder.Entity<M_RCustomer>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_Candidates>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_WorkerData>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_Agents>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_AgentCustomer_RequestUpdate>()
                    .HasKey(o => new {
                        o.Code
                    });
            modelBuilder.Entity<M_Delegates>()
                    .HasKey(o => new {
                        o.Code
                    });

        }
        //AS Account Setting
        public DbSet<M_Level1> AS_Acclevel1 { get; set; }
        public DbSet<M_Level2> AS_Acclevel2 { get; set; }
        public DbSet<M_Level3> AS_Acclevel3 { get; set; }
        public DbSet<M_Chart> Chart_of_Accounts { get; set; }
        public DbSet<M_ProductControl> PRODUCT_CONTROL { get; set; }
        public DbSet<M_Customer> Customer { get; set; }
        //Rabita start
        public DbSet<M_RCustomer> RCustomer { get; set; }
        public DbSet<M_Delegates> Delegates { get; set; }
        public DbSet<M_Agents> Agents { get; set; }
        public DbSet<M_LaborPrices> LaborPrices { get; set; }
        public DbSet<M_Candidates> Candidates { get; set; }
        public DbSet<M_FollowUps> FollowUps { get; set; }
        public DbSet<M_EmployementProblems> EmployementProblems { get; set; }
        public DbSet<M_Refunds> Refund { get; set; }
        public DbSet<M_Vehicles> HR_VEHICLES { get; set; }
        public DbSet<M_Order> Order { get; set; }
        public DbSet<M_WorkerData> WorkerData { get; set; }
        //Rabita end
        public DbSet<M_Opening> AS_Opening_Balances { get; set; }
        public DbSet<CoreInfrastructure.Accounts.Transaction.BankJournal.M_Header> BankJournal_Header { get; set; }
        public DbSet<CoreInfrastructure.Accounts.Transaction.OrderJournal.M_Header> OrderJournal_Header { get; set; }
        public DbSet<CoreInfrastructure.Accounts.Transaction.OrderJournal.M_Detail> OrderJournal_Detail { get; set; }
        public DbSet<CoreInfrastructure.Accounts.Transaction.CashJournal.M_Header> CashJournal_Header { get; set; }
        public DbSet<CoreInfrastructure.Accounts.Transaction.CashJournal.M_Detail> CashJournal_Detail { get; set; }
        public DbSet<CoreInfrastructure.Accounts.Transaction.GeneralJournal.M_Header> GeneralJournal { get; set; }
        public DbSet<CoreInfrastructure.Accounts.Transaction.Expense.M_Header> Expense_Header { get; set; }
        public DbSet<CoreInfrastructure.Accounts.Transaction.Expense.M_Detail> Expense_Detail { get; set; }
        public DbSet<CoreInfrastructure.Accounts.Transaction.ShiftClose.M_Header> ShiftClose { get; set; }

        //GS = General Setting
        public DbSet<M_GS_VendorType> GS_VendorType { get; set; }
        public DbSet<M_BarcodeGenerate> BarcodeGenerate { get; set; }
        public DbSet<M_GS_CustomerType> GS_CustomerType { get; set; }
        public DbSet<M_Company> COMPANY { get; set; }
        public DbSet<M_GS_Officer> GS_Officer { get; set; }
        public DbSet<M_GS_Zone> GS_Zone { get; set; }
        public DbSet<M_StockTransfer> StockTransfer { get; set; }
        public DbSet<M_productlabel> ProductLabel { get; set; }
        public DbSet<M_Promocode> Promocode { get; set; }
        public DbSet<M_Area> Area { get; set; }
        //start rabita
        public DbSet<M_Age> Age { get; set; }
        public DbSet<M_VisaRequest> VisaRequest { get; set; }
        public DbSet<M_SystemConfiguration> SystemConfiguration { get; set; }
        public DbSet<M_Office> office { get; set; }
        public DbSet<M_Chat> chat { get; set; }
        public DbSet<M_Appearance> Appearance { get; set; }
        public DbSet<M_Arrivals> Arrivals { get; set; }
        public DbSet<M_Experience> Experience { get; set; }
        public DbSet<M_MaritalCondition> MaritalCondition { get; set; }
        public DbSet<M_RequestStatus> RequestStatus { get; set; }
        public DbSet<M_Terminals> Terminals { get; set; }
        public DbSet<M_Visas> Visas { get; set; }
        public DbSet<M_WorkStatus> WorkerStatus { get; set; }
        public DbSet<M_Profession> professions { get; set; }
        public DbSet<M_RecruitementOrder> RecruitementOrder { get; set; }
        public DbSet<M_Delegation> RecruitementOrder_delegation { get; set; }

        public DbSet<M_RecruitementAnnex> RecruitementAnnex { get; set; }
        public DbSet<M_RecruitementTransmittalHeader> RecruitementTransmittal_Header { get; set; }
        public DbSet<M_RecruitementTransmittalDetail> RecruitementTransmittal_Detail { get; set; }
        public DbSet<M_StaffImprint> HrStaffImprint { get; set; }
        public DbSet<M_RecruitementMusanedHeader> RecruitementMusaned_Header { get; set; }
        public DbSet<M_RecruitementMusanedDetails> RecruitementMusaned_Detail { get; set; }

        public DbSet<M_HrAttendance_Header> HrAttendance_Header   { get; set; }
        public DbSet<M_HrAttendance_Detail> HrAttendance_Detail { get; set; }

        public DbSet<M_RecruitmentEmplyementReceipt> RecruitmentEmplyementReceipt { get; set; }
        public DbSet<M_RecruitmentPackages> RecruitmentPackages { get; set; }
        public DbSet<M_RecruitmentRefund> RecruitmentRefund { get; set; }
        public DbSet<M_SponsorshipTransferRefund> SponsershipTransferRefund { get; set; }
        public DbSet<M_SponsorshipTransferRequest> SponsershipTransferRequest { get; set; }

        //end rabita
        public DbSet<M_Department> Department { get; set; }
        public DbSet<M_Country> Country { get; set; }
        public DbSet<M_City> City { get; set; }
        public DbSet<M_Salesman> Salesman { get; set; }
        public DbSet<M_MeasuringUnit> MeasuringUnit { get; set; }
        public DbSet<M_Color> Color { get; set; }
        public DbSet<M_ExcelSheet> Product_Information { get; set; }

        // menu
        public DbSet<M_MainModel> APP_MODULE { get; set; }
        public DbSet<M_Roles> Roles { get; set; }
        public DbSet<M_UsersRoles> APP_USERS_ROLES { get; set; }
        public DbSet<M_Fiscalyear> Fiscalyears { get; set; }
        // items
        public DbSet<M_BarCodeType> BarCodeType { get; set; }
        public DbSet<M_LabelFormate> LabelFormate { get; set; }
        public DbSet<M_ProductTypeId> ProductTypeId { get; set; }
        public DbSet<M_CurrentOffer> CurrentOffer { get; set; }
        public DbSet<M_StatusName> StatusName { get; set; }
        public DbSet<M_BusinessLocation> BusinessLocation { get; set; }
        public DbSet<M_ItemCategory> ItemCategory { get; set; }
        public DbSet<M_SubCategory> Item_SubCategory { get; set; }
        public DbSet<M_SubCategoryDetail> Item_SubCategoryDetail { get; set; }
        //    public DbSet<M_ItemInformation> Product_Information { get; set; }
        public DbSet<M_CandidateReservation> Candidates_Reservation { get; set; }
        public DbSet<M_itemOpening> ITEMOPENING { get; set; }
        public DbSet<M_ItemCompositionHeader> ITEMCOMPOSITIONHEADER { get; set; }

        public DbSet<M_ItemCompositionDetail> ITEMCOMPOSITIONDETAIL { get; set; }
        public DbSet<M_ProductDiscount> ProductDiscount { get; set; }

        //SALE 
        public DbSet<M_SaleHeader> SALEINVOICEHEADER { get; set; }
        public DbSet<M_SaleDetail> SALEINVOICEDETAIL { get; set; }

        public DbSet<M_SHOLDHEADER> SALEHOLDHEADER { get; set; }
        public DbSet<M_SHOLDDETAIL> SALEHOLDDETAIL { get; set; }
        public DbSet<M_OrderHeader> SOHEADER { get; set; }
        public DbSet<M_OrderDetail> SODETAIL { get; set; }
        public DbSet<M_SRHeader> SRHEADER { get; set; }
        public DbSet<M_SRDetail> SRDETAIL { get; set; }
        //purchase 
        public DbSet<M_PurchaseHeader> PURCHASEHEADER { get; set; }
        public DbSet<M_PurchaseDetail> PURCHASEDETAIL { get; set; }
        //purchase 
        public DbSet<M_PRHeader> PRHEADER { get; set; }
        public DbSet<M_PRDetail> PRDETAIL { get; set; }
        public DbSet<M_POHeader> POHEADER { get; set; }
        public DbSet<M_PODetail> PODETAIL { get; set; }
        //user

        public DbSet<M_NewUser> APP_USERS { get; set; }

        // By Me
        public DbSet<M_Main_Account> Main_Account { get; set; }
        public DbSet<M_ProductTax> ProductTax { get; set; }

        public DbSet<M_ProductTags> ProductTags { get; set; }
        public DbSet<M_ProductType> ProductType { get; set; }
        // public DbSet<M_ProductInformation> Product_Information { get; set; }

        public DbSet<M_Rack> Rack { get; set; }

        //customer





        // hr 
        public DbSet<M_Departments> HR_DEPARTMENT { get; set; }
        public DbSet<M_HrEmployers> HrEmployers { get; set; }
        
        public DbSet<M_HrShiftDetails> HrShiftDetails { get; set; }
        public DbSet<M_HrShift>  HrShift { get; set; }
        
        public DbSet<M_HrDocuments> HrDocuments { get; set; }
        public DbSet<M_HrSetting> HrSetting { get; set; }
        // designation
        public DbSet<M_Designation> HR_DESIGNATION { get; set; }
        // section
        public DbSet<M_Sections> HR_SECTION { get; set; }
        // Gazette_Holidays
        public DbSet<M_Gazette_Holidays> HR_GAZATTED_HOLIDAY_SETUP { get; set; }
        // Shift Setup
        public DbSet<M_Shiftsetup> HR_SHIFTSETUP { get; set; }
        public DbSet<M_HrSalary> HrSalary { get; set; }
        // Shift Setup
        public DbSet<M_EmployeeProfile> HR_EMPLOYEE { get; set; }
        public DbSet<M_LeaveApplication> HR_LEAVES { get; set; }
        public DbSet<M_Advance> HR_ADVANCE { get; set; }
        //public DbSet<M_Advance1> HR_ADVANCE1 { get; set; }
        public DbSet<M_Loan> HR_LOAN { get; set; }
        public DbSet<M_OpeningBalance> HR_Opening_Balances { get; set; }
        public DbSet<M_Refund> HR_REFUND { get; set; }
        public DbSet<M_Positions> HR_POSITION { get; set; }
        public DbSet<M_AttendanceTable> HR_DAILY_ATTENDENCE_DETAIL { get; set; }
        public DbSet<M_OrderUpdate> OrderDetails_OrderUpdate { get; set; }
        public DbSet<M_ClientOrderUpdate> OrderDetails_ClientOrderUpdate { get; set; }
        public DbSet<M_OrderDocuments> OrderDetails_OrderDocuments { get; set; }
        public DbSet<M_Agent> RecruitementOrder_Agent { get; set; }
        public DbSet<M_AgentCustomer_RequestUpdate> AgentCustomer_RequestUpdate { get; set; }


    }
}
