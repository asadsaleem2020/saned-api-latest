using System;
using System.Collections.Generic;
using System.Text;

namespace CoreInfrastructure.Auth.User
{
  public  class M_Users
    {
        public long ID { get; set; }
        public string COMPANY_CODE { get; set; }
      
        public string USER_CODE { get; set; }
        public string USER_NAME { get; set; }
        public string USER_PASSWORD { get; set; }
        public string USER_DESCRIPTION { get; set; }
        public string ROLE_CODE { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<DateTime> CREATED_ON { get; set; }
        public Boolean ACTIVE { get; set; }
        public Boolean ENABLED { get; set; }
       
   
     
      
     
      
    }
}
