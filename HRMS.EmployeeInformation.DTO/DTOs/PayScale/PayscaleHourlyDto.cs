using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs.PayScale
{
    public class PayscaleHourlyDto
    {
        public int? EmployeeId { get; set; }
        public int? TotalHours { get; set; }
        public double? HourlyAmount { get; set; }
    }
}
