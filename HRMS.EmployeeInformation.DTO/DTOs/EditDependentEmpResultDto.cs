using MPLOYEE_INFORMATION.DTO.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
    {
    public class EditDependentEmpResultDto
        {
        public IEnumerable<EditDependentEmpDto>? EditDependentEmp { get; set; }
        public IEnumerable<EditDependentEducationDto>? EditDependentEducation { get; set; }

        }
    }
