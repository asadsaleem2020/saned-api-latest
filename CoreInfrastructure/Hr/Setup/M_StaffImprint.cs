using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreInfrastructure.Hr.Setup
{
    public class M_StaffImprint
    {
        public string CompanyCode { get; set; }
        public string Code { get; set; }
        public string EmployeeID { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ActivateStatus { get; set; }
        public string FingerPrintID { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public string Locked { get; set; }
        public string Sort { get; set; }
    }
}
