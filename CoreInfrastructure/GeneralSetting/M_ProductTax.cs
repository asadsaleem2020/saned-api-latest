using System;
using System.Collections.Generic;
using System.Text;

namespace CoreInfrastructure.GeneralSetting
{
     public class M_ProductTax
    {

        public string COMPANY_CODE { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Percentage { get; set; }
        public bool Locked { get; set; }
        public int Sort { get; set; }
    }
}
