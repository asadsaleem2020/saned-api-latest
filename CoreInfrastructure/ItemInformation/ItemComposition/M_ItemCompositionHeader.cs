using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreInfrastructure.ItemInformation.ItemComposition
{
    public class M_ItemCompositionHeader
    {
        public string COMPANY_CODE { get; set; }
        public string ITEMCODE { get; set; }
        public Boolean STATUS { get; set; }
        public string REMARKS { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> CREATED_ON { get; set; }
        
        public virtual ICollection<M_ItemCompositionDetail> DetailRows { get; set; }










    }
}
