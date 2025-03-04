using MPLOYEE_INFORMATION.DTO.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
    {
    public class CareerHistoryResultDto
        {
        public IEnumerable<CareerHistoryDto>? Table { get; set; }
        public List<Dictionary<string, object>> Table1 { get; set; }
        }
    }
