using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreInfrastructure.Accounts.Transaction.ShiftClose
{
    public class M_Header
    {

        
        public string COMPANY_CODE { get; set; }

        public string VoucherId { get; set; }

        public string VoucherType { get; set; }
        public Nullable<System.DateTime> DocumentDate { get; set; }
        public string Receiver { get; set; }
        public string Payee { get; set; }
        public Nullable<decimal> Total { get; set; }
        public string CounterNumber { get; set; }
        public string Checked { get; set; }
        public string Status { get; set; }
        public string Rejected { get; set; }
        public string Remarks { get; set; }
        
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> CREATED_ON { get; set; }
        public string DELETED_BY { get; set; }
        public Nullable<System.DateTime> DELETED_ON { get; set; }
        public string IS_DELETED { get; set; }
        public Nullable<System.DateTime> UPDATED_ON { get; set; }
        public string UPDATED_BY { get; set; }
        public string APPROVED_BY { get; set; }
        public Nullable<System.DateTime> APPROVED_ON { get; set; }
        
        public string IMAGE { get; set; }
       
    }
}
