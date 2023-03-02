using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreInfrastructure.Hr.Setup
{
    public class M_HrShiftDetails
    {
        public string CompanyCode { get; set; }
        public string Code { get; set; }
        public string ShiftCode { get; set; }
        public string ShiftName { get; set; }
        public string Address { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string LateAllowed { get; set; }
        public string DateAdded { get; set; }
        public string Status { get; set; }
    }
}
