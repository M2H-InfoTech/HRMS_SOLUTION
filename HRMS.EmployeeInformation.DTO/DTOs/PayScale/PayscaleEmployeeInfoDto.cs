using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs.PayScale
{
    public class PayscaleEmployeeInfoDto
    {
        public int? EmpId { get; set; }
        public string? EmpCode { get; set; }
        public string? Name { get; set; }
        public string? JoinDate { get; set; } // Format: dd/MM/yyyy
        public string? Designation { get; set; }
        public string? EffectiveDate { get; set; } // Format: dd/MM/yyyy
        public string? CurrencyCode { get; set; }
    }
}
