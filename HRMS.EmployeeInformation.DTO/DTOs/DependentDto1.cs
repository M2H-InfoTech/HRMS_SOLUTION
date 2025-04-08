using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class DependentDto1
    {
        public int DepId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DateOfBirth { get; set; }
        public int InterEmpId { get; set; }
        public string Type { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
    }
}
