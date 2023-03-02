using System;
using System.Collections.Generic;
using System.Text;

namespace CoreInfrastructure.Accounts.Transaction
{
    public class M_Chart
    {

        public Int64 ID { get; set; }
        public string Company_Code { get; set; }
        public string Level1Code { get; set; }
        public string Level1Name { get; set; }
        public string Level2Code { get; set; }
        public string Level2Name { get; set; }
        public string Level3Code { get; set; }
        public string Level3Name { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string AccountType { get; set; }
        public string AccountCategory { get; set; }
        public string RegisterType { get; set; }
        public bool Locked { get; set; }
        public string ADDRESS { get; set; }
        public string CITY { get; set; }
        public string COUNTRY { get; set; }
        public string NTN { get; set; }
        public string CNIC { get; set; }
        public string SALETAXNO { get; set; }
        public string TEL1 { get; set; }
        public string TEL2 { get; set; }
        public string EMAIL { get; set; }
        public string FAXNO { get; set; }
        public string WEBSITE { get; set; }
        public string Remarks { get; set; }
        public string Zone { get; set; }
        public string Officer { get; set; }

        public string CONTACT_PERSON { get; set; }
        public Nullable<decimal> CREDITLIMIT { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public string EntryUser { get; set; }





    }
}
