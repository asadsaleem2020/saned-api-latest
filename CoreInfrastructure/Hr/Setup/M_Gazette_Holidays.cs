using System;
using System.Collections.Generic;
using System.Text;

namespace CoreInfrastructure.Hr.Setup
{
   public  class M_Gazette_Holidays
    {

		//public int ID { get; set; }
		public string COMPANY_CODE { get; set; }
		public string Code { get; set; }

		public string Name { get; set; }
		public string Abbreviation { get; set; }
		public DateTime DATE { get; set; }
        public bool Locked { get; set; }
		public int Sort { get; set; }
	}
}
