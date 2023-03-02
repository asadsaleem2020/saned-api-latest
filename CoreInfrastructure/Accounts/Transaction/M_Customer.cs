using System;
using System.Collections.Generic;
using System.Text;

namespace CoreInfrastructure.Accounts.Transaction
{
   public class M_Customer
    {
		public Int64 ID { get; set; }
		public string Company_Code { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public bool Locked { get; set; }
		public string ADDRESS { get; set; }
		public string CITY { get; set; }
		public string COUNTRY { get; set; } 
		public string CNIC { get; set; } 
		public string TEL1 { get; set; } 
		public string EMAIL { get; set; }  
		public Int64 Sort { get; set; }


    }
}
