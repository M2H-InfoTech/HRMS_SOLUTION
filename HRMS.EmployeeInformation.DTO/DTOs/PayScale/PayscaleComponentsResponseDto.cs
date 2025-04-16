using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs.PayScale
{
    public class PayscaleComponentsResponseDto
    {
        public List<PayscaleHourlyDto>? HourlyComponents { get; set; }
        public List<PayscaleComponentDto>? EarningsComponents { get; set; }
        public List<PayscaleComponentDto>? DeductionComponents { get; set; }
        public PayscaleEmployeeInfoDto? EmployeeInfo { get; set; }
    }

}
