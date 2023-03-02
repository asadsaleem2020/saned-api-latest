using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechSuitsApi.Models
{
    public class M_Report
    {
        public string ProcedureName { get; set; }
        public string CompanyCode { get; set; }
        public string StartCode { get; set; }
        public string EndCode { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string User { get; set; }
        public string Department { get; set; }

        public string Salesman { get; set; }

        public string Area { get; set; }

        public string Type { get; set; }
        public string ItemStartCode { get; set; }
        public string ItemEndCode { get; set; }

         
    }
}
