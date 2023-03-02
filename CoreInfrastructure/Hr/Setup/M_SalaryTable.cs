using System;
using System.Collections.Generic;
using System.Text;

namespace CoreInfrastructure.Hr.Setup
{
   public class M_SalaryTable
    {
        public int ID { get; set; }
        public string COMPANY_CODE { get; set; }
        public string PERIOD_ID { get; set; }
        public string EMP_ID { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public DateTime PAYMENT_DATE { get; set; }
        public int PRESENT_DAYS { get; set; }
        public int ABSENTS { get; set; }
        public int LEAVES { get; set; }
        public int HOLIDAYS { get; set; }
        public int GAZTTED_HOLIDAYS { get; set; }
        public int PAIDDAYS { get; set; }
        public int UNPIADDAYS { get; set; }
        public decimal BASIC_SALARY { get; set; }
        public decimal ALLOWANCE { get; set; }
        public decimal MEAL_ALLOWANCE { get; set; }
        public decimal MEDICAL_ALLOWANCE { get; set; }
        public decimal TRAVEL_CAR_ALLOWANCE { get; set; }
        public decimal MOBILE_ALLOWANCE { get; set; }
        public decimal PA { get; set; }
        public decimal MESS { get; set; }
        public decimal EOBI { get; set; }
        public decimal QTR { get; set; }
        public decimal Other_DEDUCTION { get; set; }
        public decimal ADVANCE { get; set; }
        public decimal STL_AMOUNT { get; set; }
        public decimal INSTALLMENT { get; set; }

        public string STATUS { get; set; }
        public string IS_LOCKED { get; set; }
        public decimal OverTimeHrs { get; set; }
        public decimal OverTimeAmount { get; set; }
        public decimal GROSS_SALARY { get; set; }
        public decimal NET_SALARY { get; set; }
    }
}
