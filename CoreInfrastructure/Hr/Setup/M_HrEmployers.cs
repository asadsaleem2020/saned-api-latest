using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreInfrastructure.Hr.Setup
{
    public  class M_HrEmployers
    {
        public string Code { get; set; }
        public string CompanyCode { get; set; }
        public string Lattitude { get; set; }
        public string Longitude { get; set; }
        public decimal Areainkm { get; set; }
        public string Permissions { get; set; }
        public string BusinessAddress { get; set; }
        public string Description { get; set; }
        public string DateAdded { get; set; }
        public string DateModified { get; set; }


        public string Status { get; set; } = "Active";
    }
}
