using System;
using System.Collections.Generic;
using System.Text;

namespace CoreInfrastructure.AccomodationSystem
{
  public  class M_Order
    {
	 
		public Int64 ID { get; set; }
		public string Company_Code { get; set; }
		public string OrderNumber { get; set; }
		public string Date { get; set; }
		public string DateHijri { get; set; }
		public string ClientID { get; set; }
		public string workerID { get; set; }
		public string RequestTypeID { get; set; }

		public string ContractDuration { get; set; }
		public string RentalStartDate { get; set; }
		public string RentalCost { get; set; }
		public string ExperienceAllowed { get; set; }
		public string ProbationaryStart { get; set; }
		public string ProbationaryEnd { get; set; }
		public string TrailDays { get; set; }
		public string CostofDayAfterTrail { get; set; }
		public string SponsorshipFee { get; set; }
		public string ValueAddedFee { get; set; }
		public string Notes { get; set; }
		public string Message { get; set; }
		public string Status { get; set; }
		public string Sort { get; set; }
        public string Locked { get; set; }

	}
}
