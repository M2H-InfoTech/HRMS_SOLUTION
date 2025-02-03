using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class DocumentFillDto
    {
        public int? ReasonId { get; set; }
        public int? CategoryFieldId { get; set; }
        public int? FieldValues { get; set; }
        public string? FieldDescription { get; set; }
        public int? DataTypeId { get; set; }
    }
}
