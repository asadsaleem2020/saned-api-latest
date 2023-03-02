using System;
using System.Collections.Generic;
using System.Text;

namespace CoreInfrastructure.Hr.Setup
{
    public class M_LeaveApplication
    {
		//public long ID { get; set; }
		public string COMPANY_CODE { get; set; }
		public string DOCUMENT_NO { get; set; }

		public DateTime DOCUMENT_DATE { get; set; }
		public DateTime DATE_FROM { get; set; }
		public DateTime DATE_TO { get; set; }
		public long LeaveDays { get; set; }
		public string EMPLOYEE_CODE { get; set; }
		public string CHECKED { get; set; }
		public string STATUS { get; set; }
		public string LeaveType { get; set; }
		public string ValType { get; set; }
		public string REMARKS { get; set; }
		public string Reference { get; set; }
		public string Leave_Approved_by { get; set; }
		public DateTime Leave_Approved_on { get; set; }
		public string CREATED_BY { get; set; }
		public DateTime CREATED_ON { get; set; }
		public string DELETED_BY { get; set; }
		public DateTime DELETED_ON { get; set; }
		public string IS_DELETED { get; set; }
		public DateTime UPDATED_ON { get; set; }
		public string UPDATED_BY { get; set; }
		public DateTime APPROVED_ON { get; set; }

		public string APPROVED_BY { get; set; }
		
	}
}

