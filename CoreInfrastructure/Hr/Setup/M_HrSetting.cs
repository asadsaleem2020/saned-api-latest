using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreInfrastructure.Hr.Setup
{
    public class M_HrSetting
    {
        public string CompanyCode { get; set; } = "C001";
        public string Code { get; set; }
        public string IpAddress { get; set; }
        public string WorkingHours { get; set; }
        public string WorkPeriod { get; set; }
        public string WorkingDays { get; set; }
        public string DaysOfAbsense { get; set; }
        public string ExtraHours { get; set; }
        public string HoursOfAbsense { get; set; }
        public string Status { get; set; }
        public string IsActive { get; set; }
        public string IsDeleted { get; set; }
        public string Locked { get; set; }
    }
}
