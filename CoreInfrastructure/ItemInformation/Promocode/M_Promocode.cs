using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CoreInfrastructure.ItemInformation.Promocode
{
   public class M_Promocode
    {
       
        public string COMPANY_CODE { get; set; }
        public long documentno { get; set; }
        public string Code { get; set; } 
        public string Name { get; set; }
        public string ContactId { get; set; }
        public long DiscountRate { get; set; }
        public bool printname { get; set; }

        public bool printvariation { get; set; }
        public bool printprice { get; set; }
        public bool printbusinessname { get; set; }
        public long noofLable { get; set; }
        public string showprice { get; set; }
      


    }
}
