using System;
using System.Collections.Generic;
using System.Text;

namespace CoreInfrastructure.ToolbarItems
{
   public class M_Agents
    {
       
        public string ID { get; set; }
        public string Company_Code { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }
        public string RName { get; set; }
        public string EMAIL { get; set; }
        public string ID_Number { get; set; }
        public string Country { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }

        //[RAddress] 
        //[city]  
        //[Rcity]  
        //[License]  
        // [mobile]  
        //[Phone]  
        //[fax]  
        //[SendingBank]  
        //[responsibleName]  
        //[AccountNumber]
        public string RAddress { get; set; }
        public string city { get; set; }
        public string Rcity { get; set; }
        public string License { get; set; }
        public string mobile { get; set; }
        public string Phone { get; set; }
        public string fax { get; set; }
        public string SendingBank { get; set; }
        public string responsibleName { get; set; }
        public string AccountNumber { get; set; }
        //[CellPhone]  
        //[licenseHolder]  
        //[Homephone]  
        //[Notes]  
        //[Status]  
        //[Sort]  
        //[Locked]

        public string CellPhone { get; set; }
        public string licenseHolder { get; set; }
        public string Homephone { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public string Sort { get; set; }
        public string Locked { get; set; }

    }
}
