using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreInfrastructure.Hr.Setup.Attendance
{
    public class M_HrAttendance_Detail
    {
        public string CompanyCode { get; set; }
        public string Code { get; set; }
        public string SeqNo { get; set; }
          public string EmployeeID { get; set; }
        public DateTime? AttendanceDate { get; set; }
        public TimeSpan? EntryTime { get; set; }
        public TimeSpan? ExitTime { get; set; }
        public TimeSpan? LateTime { get; set; }
        public TimeSpan? ExtraTime { get; set; }
        public TimeSpan? EarlyDismissalTime { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public string Sort { get; set; }
        public string Locked { get; set; }
    }
}
