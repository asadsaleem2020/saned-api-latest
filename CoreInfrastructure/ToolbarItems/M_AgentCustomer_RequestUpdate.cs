using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreInfrastructure.ToolbarItems
{
    public class M_AgentCustomer_RequestUpdate
    {
        public string CompanyCode { get; set; }
        public string Code { get; set; }
        public string PersonID { get; set; }
        public string Type { get; set; }
        public string Reply { get; set; }
        public string AttachedFile { get; set; }
        public string AddedOn { get; set; }
        public string AddedBy { get; set; }
        public string Status { get; set; }
        public string Sort { get; set; }
        public string Locked { get; set; }
    }
}
