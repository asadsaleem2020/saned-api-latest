using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreInfrastructure.SpnoserShip
{
    public class M_SponsorshipTransferRequest
    {
        public string Code { get; set; }
        public string Company_Code { get; set; }
        public string OrderNumber { get; set; }
        public string CurrentSponsor { get; set; }
        public string Date { get; set; }
        public string Experience { get; set; }
        public string TrialStartDate { get; set; }
        public string TrialEndDate { get; set; }
        public string TrialDays { get; set; }
        public string CostAfterTrial { get; set; }
        public string FormerSponsor { get; set; }
        public string WorkerID { get; set; }
        public string Job { get; set; }
        public string AccomodationProvider { get; set; }
        public string MedicalCheck { get; set; }
        public string SalaryCheck { get; set; }
        public string AgencyCheck { get; set; }
        public string Salary { get; set; }
        public string SalariesReceived { get; set; }
        public string PaidAmountToPreviousSponsor { get; set; }
        public string ReceivedWorkerDate { get; set; }
        public string CostOfTransfer { get; set; }
        public string RefundStatus { get; set; }
        public string RefundDate { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public string Sort { get; set; }
        public string Locked { get; set; }
    }
}
