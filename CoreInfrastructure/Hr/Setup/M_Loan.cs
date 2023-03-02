using System;
using System.Collections.Generic;
using System.Text;

namespace CoreInfrastructure.Hr.Setup
{
    public class M_Loan
    {
        public string COMPANY_CODE { get; set; }
        public string DOCUMENT_NO { get; set; }
        public string EMPLOYEE_CODE { get; set; }
        public string NAME { get; set; }
        public string GL_ACCOUNT_CODE { get; set; }
        public string GL_ACCOUNT_NAME { get; set; }
        public DateTime DOCUMENT_DATE { get; set; }
        public decimal AMOUNT { get; set; }

        public string REMARKS { get; set; }
        public string STATUS { get; set; }
        public string CHECKED { get; set; }
        public DateTime DOC { get; set; }
        public bool ISDELETED { get; set; }
        public decimal INSTALLMENT { get; set; }
        public DateTime Start_Date { get; set; }

        public DateTime CREATED_ON { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime UPDATED_ON { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime APPROVED_ON { get; set; }
        public string APPROVED_BY { get; set; }

        public DateTime DELETED_ON { get; set; }
        public string DELETED_BY { get; set; }

    }
}
