using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    internal class SaveUpdateEmployeeRequestDto
    {
        public int EmpID { get; set; }
        public string EntryBy { get; set; }
    }
}
