using System;
using System.Collections.Generic;
using System.Text;

namespace CoreInfrastructure.Sale.Holdsales
{
    public class M_SHOLDHEADER
    {
        public string COMPANY_CODE { get; set; }
        public string INVOICECODE { get; set; }
        public string REF_NO { get; set; }
        public Nullable<System.DateTime> INVOICEDATE { get; set; }
        public string PARTYCODE { get; set; }
        public string NAME { get; set; }
        public string TYPE { get; set; }
        public string SO_NO { get; set; }
        public Boolean STATUS { get; set; }
        public string AREA_CODE { get; set; }

        public string DEPT_ID { get; set; }
        public string DESPATCH { get; set; }
        public string REMARKS { get; set; }

        public string DOCUMENT_NO { get; set; }
        public string SALESMAN { get; set; }
        public string PAYMENT_TYPE { get; set; }

        public Nullable<decimal> TOTAL_AMOUNT { get; set; }
        public Nullable<decimal> DISCOUNT_RATE { get; set; }
        public Nullable<decimal> DISCOUNT_AMOUNT { get; set; }
        public Nullable<decimal> NET_AMOUNT { get; set; }
        public Nullable<decimal> CASH_AMOUNT { get; set; }
        public Nullable<decimal> CREDIT_AMOUNT { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> CREATED_ON { get; set; }
        public string DELETED_BY { get; set; }
        public Nullable<System.DateTime> DELETED_ON { get; set; }
        public string IS_DELETED { get; set; }
        public Nullable<System.DateTime> UPDATED_ON { get; set; }
        public string UPDATED_BY { get; set; }
        public virtual ICollection<M_SHOLDDETAIL> DetailRows { get; set; }









    }
}
