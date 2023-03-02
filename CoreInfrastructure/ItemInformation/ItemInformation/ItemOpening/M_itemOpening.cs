using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreInfrastructure.ItemInformation.ItemOpening
{
    public class M_itemOpening
    {
      
        public string COMPANY_CODE { get; set; }

        public string Code { get; set; }

        public string ITEMCODE { get; set; }
        public string ITEMNAME { get; set; }

        public decimal QTY { get; set; }
        public decimal RATE { get; set; } 
    }
}
