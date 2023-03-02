using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace CoreInfrastructure.Auth.NewUsers
{
   public class M_NewUser
    { 
        public string COMPANY_CODE { get; set; }
        public Int64 CODE { get; set; }
        public string NAME { get; set; }
        public string USER_PASSWORD { get; set; }
        
        public Int64 ROLE_CODE { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; } 
        public Boolean ACTIVE { get; set; }
        


    }
}
