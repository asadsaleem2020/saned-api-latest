using System;
using System.Collections.Generic;
using System.Text;

namespace CoreInfrastructure.ToolbarItems
{
   public class M_LaborPrices
    {
       
        public Int64 ID { get; set; }
        public string Company_Code { get; set; }
        public string Code { get; set; }

        //Declare Here
        public string Agent { get; set; }
        public string Occupation { get; set; }
        public string Religion { get; set; }
        public string Experience { get; set; }
        public string PriceDollar { get; set; }
        public string PriceRiyal { get; set; }
        public string ContratctPeriod { get; set; }
        public string LeaveInDays { get; set; }
        public string SalaryInRiyal { get; set; }
        public string ArrivalDuration { get; set; }
        


        //Declare above
        public string Notes { get; set; }
        public string Status { get; set; }
        public string Sort { get; set; }
        public string Locked { get; set; }

    }
}
