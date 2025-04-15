using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class EmployeeWithHolidayAccessDto
    {
        public EmployeeDetailsDto Employee { get; set; }
        public bool HasHolidayAccess { get; set; }
    }
}
