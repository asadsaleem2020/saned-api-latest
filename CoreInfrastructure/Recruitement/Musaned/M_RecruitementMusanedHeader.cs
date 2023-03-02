 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CoreInfrastructure.Recruitement.Musaned
{
    public class M_RecruitementMusanedHeader
    {
        public string CODE { get; set; }
        public string COMPANY_CODE { get; set; }
        public string BANK { get; set; }
        public string AMOUNT { get; set; }
        public DateTime? PAYMENTDATE { get; set; }
        public string NOTES { get; set; }
        public string STATUS { get; set; }
        public string SORT { get; set; }
        public string LOCKED { get; set; }
        public virtual ICollection<M_RecruitementMusanedDetails> DETAILROWS { get; set; }
    }
}
