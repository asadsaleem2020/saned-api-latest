using System;
using System.Collections.Generic;
using System.Text;

namespace CoreInfrastructure.Auth.Fiscalyear
{
  public  class M_Fiscalyear
    {
        public string CompanyCode { get; set; }
        public string Year { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        
    }
}
