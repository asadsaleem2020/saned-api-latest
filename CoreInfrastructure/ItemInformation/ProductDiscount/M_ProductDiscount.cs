using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CoreInfrastructure.ItemInformation.ProductDiscount
{
   public class M_ProductDiscount
    {

        public string COMPANY_CODE { get; set; }
        public Int64 Code { get; set; }
        public string Name { get; set; }
        public string discType { get; set; }
        public string DiscountCategory { get; set; }
        public string SelectedDays { get; set; }
        public bool DayandDate { get; set; }
        public bool Locked { get; set; }
        public Nullable<DateTime> startdate { get; set; }
        public Nullable<DateTime> enddate { get; set; }

        public string SingleProduct { get; set; }
        public string Level1Code { get; set; }
        public string Level2Code { get; set; }
        public string Level3Code { get; set; }
        public Nullable<decimal> Amount { get; set; }







    }
}
