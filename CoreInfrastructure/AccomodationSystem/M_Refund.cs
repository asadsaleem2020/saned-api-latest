using System;
using System.Collections.Generic;
using System.Text;

namespace CoreInfrastructure.AccomodationSystem
{
   public class M_Refunds
    {
		public Int64 ID { get; set; }
		public string Company_Code { get; set; }
		public string Code { get; set; }
		public string Date { get; set; }
		public string CompanyID { get; set; }
		public string ContractAmount { get; set; }
		public string tax { get; set; }
		public string rafund { get; set; }
	 
		public string taxamount { get; set; }
		public string PaymentType { get; set; }
		public string Status { get; set; }
		public string Sort { get; set; }
		public string Locked { get; set; }
        public string ClientCode { get; set; }
        public string WorkerID { get; set; }
        public string Note { get; set; }


    }
}
