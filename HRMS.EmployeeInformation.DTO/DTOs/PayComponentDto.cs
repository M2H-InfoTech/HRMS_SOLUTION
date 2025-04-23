using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class PayComponentDto
    {
        public string? EarnPayCode { get; set; }            // Maps to PayCode
        public string? PayCodeDescription { get; set; }     // Maps to PayCodeDescription
        public int? PayCodeId { get; set; }                 // Maps to PayCodeId
        public int? Type { get; set; }                      // 1 = Earnings, 2 = Deductions
        public double? Amount { get; set; }    // Default to 0.00
    }

}
