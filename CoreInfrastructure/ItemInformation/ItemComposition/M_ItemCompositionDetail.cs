using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreInfrastructure.ItemInformation.ItemComposition
{ 
      public class M_ItemCompositionDetail
    {
        public string COMPANY_CODE { get; set; }
        public string PARENTCODE { get; set; }
        public long SEQNO { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public Nullable<decimal> Quantity { get; set; }        
        public Nullable<decimal> Rate { get; set; }
        public Nullable<decimal> Amount { get; set; }
        

    }
}
