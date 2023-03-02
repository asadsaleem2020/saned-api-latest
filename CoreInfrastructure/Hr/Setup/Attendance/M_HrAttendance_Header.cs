using CoreInfrastructure.Recruitement.Musaned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreInfrastructure.Hr.Setup.Attendance
{
    public class M_HrAttendance_Header
    {
        public string Code { get; set; }
        public string CompanyCode { get; set; }
        public string EmployeeID { get; set; }
        public DateTime? AttendanceDate { get; set; }
        public TimeSpan? LastEntryTime { get; set; }
        public TimeSpan? LastExitTime { get; set; }
        public string ShiftID { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public string Sort { get; set; }
        public string Locked { get; set; }
         public virtual ICollection<M_HrAttendance_Detail> DetailRows { get; set; }
    }
}
