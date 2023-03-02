using System;
using System.Collections.Generic;
using System.Text;

namespace CoreInfrastructure.Hr.Setup
{
    public class M_Advance
    {
        public Int64 ID { get; set; }
        public string COMPANY_CODE { get; set; }
        public string CODE { get; set; }
        public string EMPLOYEE { get; set; }
        public string AMOUNT { get; set; }
        public string MONTHS { get; set; }
        public string PAYMENTMETHOD { get; set; }
        public string CREATED_ON { get; set; }
        public string CREATED_BY { get; set; }
        public string APPROVED_ON { get; set; }
        public string APPROVED_BY { get; set; }
        public string DELETED_ON { get; set; }
        public string DELETED_BY { get; set; }
        //Declare above
        public string NOTES { get; set; }
        public string STATUS { get; set; }
        public string SORT { get; set; }
        public string LOCKED { get; set; }

    }
    public class M_Advance1
    {
        
        public string EMPLOYEE { get; set; }
        public string AMOUNT { get; set; }
        public string STATUS { get; set; }
        public string CREATED_ON { get; set; }


    }
}
