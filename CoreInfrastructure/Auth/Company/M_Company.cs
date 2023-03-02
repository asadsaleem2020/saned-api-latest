using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace CoreInfrastructure.Auth.Company
{
    
    public class M_Company
    {
        
        public string COMPANY_CODE { get; set; } 
        public string COMPANY_NAME { get; set; }
        public string ADDRESS { get; set; }
        public bool ACTIVE { get; set; }
        public Nullable<DateTime> START_DATE { get; set; }
        public Nullable<DateTime> END_DATE { get; set; }

        public string POSTAL { get; set; }
        public string PHONE { get; set; }
        public string FAX { get; set; }
        public string E_MAIL { get; set; }
        public string SALES_TAX_NO { get; set; }
        public string WEBSITE { get; set; }
        public string COUNTRY { get; set; }
        public string CITY { get; set; }


      


    }
}
