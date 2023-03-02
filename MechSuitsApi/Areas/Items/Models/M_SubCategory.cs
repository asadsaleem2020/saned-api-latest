using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechSuitsApi.Areas.Items.Models
{
    public class M_SubCategory
    {

        public string COMPANY_CODE { get; set; }

        public string Level1Code { get; set; }

        public string Level1Name { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
        public bool Locked { get; set; }
        public int Sort { get; set; }
        public int used { get; set; }

    }
}
