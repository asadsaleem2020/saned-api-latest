using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreInfrastructure.Auth.Menu
{
    public class M_DetailModel
    {
        [Key]
        public int id { get; set; }
        public string MENU_NAME { get; set; }
        public string MENU_AREA { get; set; }

        public string ACTION { get; set; }

        public string CONTROLLER { get; set; }
    }
}
