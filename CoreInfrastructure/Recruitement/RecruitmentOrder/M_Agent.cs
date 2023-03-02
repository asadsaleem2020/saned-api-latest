using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreInfrastructure.Recruitement.RecruitmentOrder
{
    public class M_Agent
    {
        public string CompanyCode { get; set; }
        public string Code { get; set; }
        public string OrderNumber { get; set; }
        public string AgentID { get; set; }
        public string AgentName { get; set; }
        public string WorkerID { get; set; }
        public string WorkerName { get; set; }
        public string WorkerRecruitmentCost { get; set; }
        public string AmountPaid { get; set; }
        public string Notes { get; set; }
        public string AddedOn { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string DeletedOn { get; set; }
        public string DeletedBy { get; set; }
        public string Status { get; set; }
        public string IsActive { get; set; }
        public string Sort { get; set; }
        public string Locked { get; set; }
    }
}
