using System;
using System.Collections.Generic;
using System.Text;

namespace CoreInfrastructure.Recruitement
{
   public class M_RecruitmentRefund
    {
        public Int64 ID { get; set; }
       // public string Code { get; set; }
        public string Company_Code { get; set; }
        public string OrderNumber { get; set; }
        public string Demand { get; set; }
        public string ContractCost { get; set; }
        public string Fine { get; set; }
        public string DiscountBear { get; set; }
        public string DiscountAmount { get; set; }
        public string rafund { get; set; }
        public string HijriDate { get; set; }
        public string Date { get; set; }
        public string PaymentType { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public string Sort { get; set; }
        public string Locked { get; set; }
    }
}
