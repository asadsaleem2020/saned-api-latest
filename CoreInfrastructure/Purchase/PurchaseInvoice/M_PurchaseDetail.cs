using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreInfrastructure.Purchase.PurchaseInvoice
{ 
      public class M_PurchaseDetail
      {

        public string COMPANY_CODE { get; set; }
        public string INVOICECODE { get; set; }
        public long SEQNO { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public Nullable<decimal> Packing { get; set; }
        public Nullable<decimal> Ctn { get; set; }
        public string MeasuringUnit { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> TotalQty { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> DiscountRate { get; set; }
        public Nullable<decimal> DiscountAmount { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<decimal> AmountAfterDiscount { get; set; }
        public Nullable<decimal> TaxRate { get; set; }
        public Nullable<decimal> TaxAmount { get; set; }
        public string USERCODE { get; set; }






    }
}
