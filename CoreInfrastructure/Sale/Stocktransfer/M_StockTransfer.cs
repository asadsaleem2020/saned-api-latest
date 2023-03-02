using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CoreInfrastructure.Sale.Stocktransfer
{
   public class M_StockTransfer
    {
       
        public string COMPANY_CODE { get; set; }
        public long documentno { get; set; }
        public string Code { get; set; } 
        public string Name { get; set; }
        public string Rackno { get; set; }
        public Nullable<decimal> Qty { get; set; }
        public bool Locked { get; set; }
       
      
    }
}
