using System;

namespace CoreInfrastructure.Recruitement.Philippine
{
    public class M_RecruitementAnnex
    {
        public string Code { get; set; }
        public string CompanyCode { get; set; }
        public string OrderID { get; set; }
        public string Date { get; set; }
        public Int32 Cleaning { get; set; }
        public Int32 Washing { get; set; }
        public Int32 BabySitting { get; set; }
        public Int32 ElderCare { get; set; }
        public Int32 Cooking { get; set; }
        public string Tasks { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public string Locked { get; set; }
        public string Sort { get; set; }
    }
}
