using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechSuitsApi.Areas.Items.Models
{
    public class M_SubCategoryDetail
    {
        #region Public Properties

        public string Company_Code { get; set; }
        public string Level1Code { get; set; }
        public string Level1Name { get; set; }
        public string Level2Code { get; set; }
        public string Level2Name { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool Locked { get; set; }
        public int Sort { get; set; }
        public int used { get; set; }
        #endregion
    }
}
