using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class HourlyPayscaleResult
    {
        public double HourlyAmount { get; set; }
        public int? TotalHours { get; set; }
        public double TotalAmount { get; set; }
    }
}
