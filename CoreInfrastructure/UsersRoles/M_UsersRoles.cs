using System;
using System.Collections.Generic;
using System.Text;

namespace CoreInfrastructure.UsersRoles
{
  public  class M_UsersRoles
    {
        
        public string COMPANY_CODE { get; set; }
        public long ROLE_ID { get; set; }        
        public long MENU_ID { get; set; }
        public bool ACTIVE { get; set; }
        public bool NEW { get; set; }
        public bool EDIT { get; set; }
        public bool DEL { get; set; }
        public bool APPROVE { get; set; }
        public bool CHK { get; set; }
        public bool UNCHK { get; set; }
        public bool PRNT { get; set; }
        public bool GETLIST { get; set; }

        //public static implicit operator M_UsersRoles(M_UsersRoles v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
