using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
    {
    public class GetDependentDetailsDto
        {
        public int EmpId { get; set; }
        public string? EmpCode { get; set; }
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public string? DateOfBirth { get; set; }
               

        }
    }
