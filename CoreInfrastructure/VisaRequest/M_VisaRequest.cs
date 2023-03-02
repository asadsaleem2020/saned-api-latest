using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreInfrastructure.VisaRequest
{
    public class M_VisaRequest
    {
        public string COMPANY_CODE { get; set; } = "C001";
        public string CODE { get; set; }
        public string CLIENT { get; set; }
        public string DATE { get; set; }
        public string COUNTRY { get; set; }
        public string RELIGION { get; set; }
        public string PROFESSION { get; set; }
        public string GENDER { get; set; }
        public string JOB { get; set; }
        public string WORK { get; set; }
        public string SALARY { get; set; }
        public string MOBILE { get; set; }
        public string APPLICATION_STATUS { get; set; }
        public string GOVT_FEE { get; set; }
        public string COMPANY_FEE { get; set; }
        public string VISA_PAID_STATUS { get; set; }
        public string FAMILIY_ID { get; set; }
        public string PHOTO { get; set; }
        public string BANK_STATEMENT { get; set; }
        public string NOTES { get; set; }
        public string VisaStatus { get; set; } = "0";
        public string Status { get; set; } = "0";
        public string Sort { get; set; } = "0";
        public string Locked { get; set; } = "0";
    }
}
