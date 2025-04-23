using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs.PayScale
{
    public class PayscaleComponentDto
    {
        public long ComponentId { get; set; }
        public string? Component { get; set; } // Format: "PayCode - Description"
        public decimal Amount { get; set; }
        public double? TotalEarnings { get; set; }
        public double? TotalDeductions { get; set; }
    }
}
