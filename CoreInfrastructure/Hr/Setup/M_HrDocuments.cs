using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreInfrastructure.Hr.Setup
{
    public class M_HrDocuments
    {
        public string CompanyCode { get; set; } = "C001";
        public string Code { get; set; }
        public string DocumentType { get; set; }
        public string IssueDate { get; set; }
        public string ExpiryDate { get; set; }
         
        public string File { get; set; }
        public string Status { get; set; }
        public string IsActive { get; set; }
        public string IsDeleted { get; set; }
        public string Locked { get; set; }
    }
}
