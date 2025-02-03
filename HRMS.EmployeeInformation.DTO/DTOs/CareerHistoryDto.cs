using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class CareerHistoryDto
    {
        public string? Category {  get; set; }
        public string? Relevant {  get; set; }
        public string? NonRelevent { get; set; }
        public string? Total { get; set; }
    }
}
