using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreInfrastructure.Hr.Setup
{
    public class M_HrSalary
    {

        public string CompanyCode { get; set; } = "C001";
        public string Code { get; set; }
        public string EmployeeCode { get; set; }
        public DateTime? Date { get; set; }
        public decimal Salary { get; set; }
        public decimal Allowance { get; set; }
        public decimal Advance { get; set; }
        public decimal Discount { get; set; }
        public decimal InsuranceDiscount { get; set; }
        public decimal TotalSalary { get; set; }
        public string Notes { get; set; }
    }
}
