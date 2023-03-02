using System;
using System.Collections.Generic;
using System.Text;

namespace CoreInfrastructure.GeneralSetting
{
	public class M_GS_Zone
	{

		public Int64 ID { get; set; }
		public string Company_Code { get; set; } 
		public string Name { get; set; }
		public bool Locked { get; set; }
		public Int64 Sort { get; set; }

	}   
}
