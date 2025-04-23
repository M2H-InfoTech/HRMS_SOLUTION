using EMPLOYEE_INFORMATION.Models.Entity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class PayscaleResultDto
    {
        public int NewFormat { get; set; }

        public List<PayComponentDto> Earnings { get; set; } = new();
        public List<PayComponentDto> Deductions { get; set; } = new();

        public List<HrmValueType> ProposedPayTypes { get; set; } = new();
        public List<HrmValueType> IncrementTypes { get; set; } = new();

        public PayscaleEffectiveDateDto EffectiveDate { get; set; }
        
    }
}
