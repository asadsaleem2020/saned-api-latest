using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreInfrastructure.Accounts.Setup
{
	public class M_Level1
	{
		[Key]
		public int ID { get; set; }
		public string Company_Code { get; set; }
		public string Code { get; set; }

		public string Name { get; set; }
		public bool Locked { get; set; }
		public string Nature { get; set; }

	}   
}
