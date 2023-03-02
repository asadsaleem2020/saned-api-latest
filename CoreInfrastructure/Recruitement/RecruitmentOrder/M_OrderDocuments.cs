using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreInfrastructure.Recruitement.RecruitmentOrder
{
    public class M_OrderDocuments
    {
        public string CompanyCode { get; set; }
        public string Code { get; set; }
        public string OrderNumber { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string AttachedDocument { get; set; }
        public string AddedOn { get; set; }
        public string AddedBy { get; set; }
        public string Status { get; set; }
        public string Sort { get; set; }
        public string Locked { get; set; }
    }
}
