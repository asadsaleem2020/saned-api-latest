using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreInfrastructure.Accounts.Transaction.Expense
{
    [Keyless]
    public   class M_Detail
    {
        public long ID { get; set; }
        public string COMPANY_CODE { get; set; }
        public string VoucherId { get; set; }
        public long SeqNo { get; set; }
        public string Vouchertype { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string Remarks { get; set; }
        
    }
}
