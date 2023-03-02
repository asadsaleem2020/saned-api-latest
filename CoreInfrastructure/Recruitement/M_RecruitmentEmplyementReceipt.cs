using System;
using System.Collections.Generic;
using System.Text;

namespace CoreInfrastructure.Recruitement
{
  public  class M_RecruitmentEmplyementReceipt
    {
		public Int64 ID { get; set; }
		public string Company_Code { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string Passport { get; set; }
		public string ClientID { get; set; }
		public string TrialExpiryDate { get; set; }
		public string ContractExpiryDate { get; set; }
		public string DateReceived { get; set; }
	 
		public string Status { get; set; }
		public string Sort { get; set; }
		public string Locked { get; set; }
	}
}
