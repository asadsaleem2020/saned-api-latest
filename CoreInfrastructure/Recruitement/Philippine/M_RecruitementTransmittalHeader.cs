 
using System;
using System.Collections.Generic;

namespace CoreInfrastructure.Recruitement.Philippine
{
    public class M_RecruitementTransmittalHeader
    {
        public string Code { get; set; }
        public string CompanyCode { get; set; }
        public string Agent { get; set; }
        public DateTime? TransmittalDate { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public string Sort { get; set; }
        public string Locked { get; set; }
        
        public virtual ICollection<M_RecruitementTransmittalDetail> DetailRows { get; set; }
    }
}
