using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CoreInfrastructure.ItemInformation.Barcode
{
   public class M_BarcodeGenerate
    {
       
        public string COMPANY_CODE { get; set; }        
        public string Code { get; set; } 
        public decimal Weight { get; set; }
        public string Barcode { get; set; }

        public string ITEM_NAME { get; set; }




    }
}
