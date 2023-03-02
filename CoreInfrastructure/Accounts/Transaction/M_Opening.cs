using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreInfrastructure.Accounts.Transaction
{
    [Keyless]
    public class M_Opening
    {
        [Key]
        public string YearId { get; set; }
        public string Company_Code { get; set; }
      
        public string AccountCode { get; set; }

        public decimal OpeningDr { get; set; }
        public decimal OpeningCr { get; set; }




    }
}
