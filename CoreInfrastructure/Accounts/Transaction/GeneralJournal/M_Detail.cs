using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreInfrastructure.Accounts.Transaction.GeneralJournal
{
    [Keyless]
    public class M_Detail
    {
        public long ID { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
     
        public string Remarks { get; set; }
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> Credit { get; set; } 
    }
}
