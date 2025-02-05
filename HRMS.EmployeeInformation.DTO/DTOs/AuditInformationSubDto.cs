using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class AuditInformationSubDto
    {
        public int? InfoID { get; set; }
        public int? Info01ID { get; set; }
        public int EmpID { get; set; }
        public string? Information { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
