using System;
using System.Collections.Generic;
using System.Text;

namespace CoreInfrastructure.SpnoserShip
{
   public class M_SponsorshipTransferRefund
	{
		public string Code { get; set; }
        public string Company_Code { get; set; }
		public string OrderNumber { get; set; }
		public string Amount { get; set; }
		public string Date { get; set; }
		public string PaymentMethod { get; set; }
		public string Notes { get; set; }
		public string Status { get; set; }
		public string Sort { get; set; }
		public string Locked { get; set; }

	}
}
