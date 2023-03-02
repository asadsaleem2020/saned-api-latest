using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreInfrastructure.UsersRoles
{
      public class M_ModalHeader
      {
        public long Id { get; set; }
        public string Role_Id { get; set; }        
        public virtual ICollection<M_ModalDetail> DetailRows { get; set; }
    }
}
