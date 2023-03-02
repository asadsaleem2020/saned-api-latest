using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreInfrastructure.UsersRoles
{ 
      public class M_ModalDetail
      { 
  
        public long ID { get; set; }
        public string Company_Code { get; set; }
        public long ROLE_ID { get; set; }
        public long MODULE_ID { get; set; }
        public long MENU_ID { get; set; }
        public string MODULE_NAME { get; set; }
        public string MENU_NAME { get; set; }
        public string MODULE_MENU_SORT { get; set; }
        public bool ACTIVE { get; set; }
        public string NEW { get; set; }
        public string EDIT { get; set; }
        public string DEL { get; set; }
        public string APPROVE { get; set; }
        public string CHK { get; set; }
        public string UNCHK { get; set; }
        public string PRNT { get; set; }
        public string GETLIST { get; set; }

    }
}
