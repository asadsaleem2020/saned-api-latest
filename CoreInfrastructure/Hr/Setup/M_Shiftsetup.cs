using System;
using System.Collections.Generic;
using System.Text;

namespace CoreInfrastructure.Hr.Setup
{
    public class M_Shiftsetup
    {
		//public int ID { get; set; }
		public string COMPANY_CODE { get; set; }
		public string Code { get; set; }

		public string Name { get; set; }
		
		public DateTime TimeIn { get; set; }
		public decimal DutyHours { get; set; }
		public decimal MinDutyHours { get; set; }
		public decimal RelaxMinutes { get; set; }
		public bool Locked { get; set; }
		public int Sort { get; set; }
	}
}
