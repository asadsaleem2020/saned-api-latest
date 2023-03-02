using System;
using System.Collections.Generic;
using System.Text;

namespace CoreInfrastructure.Hr.Setup
{
  public  class M_Attendance
    {
        
     
        public string COMPANY_CODE { get; set; }
        public string Default_Status { get; set; }
        public string EMPLOYEE_CODE { get; set; }
        public string DEPARTMENT { get; set; }
        public string DESIGNATION { get; set; }
        public string SECTION { get; set; }
        public DateTime Date_To { get; set; }
        public DateTime Date_From { get; set; }
         
        // public string COMPANY_CODE { get; set; }
        // public DateTime ATTENDANCE_DATE { get; set; }
        // public Int64 ATTENDANCE_DAY { get; set; }
        // public string DEPARTMENT { get; set; }
        // public string DESIGNATION { get; set; }
        //public string SECTION { get; set; }
        // public string EMPLOYEE_CODE { get; set; }
        // public string EMPLOYEE_NAME { get; set; }
        // public string SHIFT_ID { get; set; }
        // public string STATUS { get; set; }
        // public string REMARKS { get; set; }
        // public DateTime TIME_IN { get; set; }
        // public DateTime TIME_OUT { get; set; }
        // public decimal WORKING_HOURS { get; set; }
        // public DateTime OVERTIME { get; set; }

    }
}
