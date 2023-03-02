using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreInfrastructure.Auth.Menu
{
  public  class M_MainModel
    {
        [Key]
        public int MODULE_ID { get; set; }
        public int HAS_MENU_LEVEL { get; set; }
        public bool ACTIVE { get; set; }
        public int SORT { get; set; }
        public string MODULE_NAME { get; set; }

        public string Module_Icon { get; set; }
        public virtual ICollection<M_DetailModel> DetailRows { get; set; }
    }
}
