using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs.Documents
{
    public class FilesDto
    {
        public int? DocID { get; set; }
        public int? DetailID { get; set; }
        public string? FileName { get; set; }
        public string? Status { get; set; }
    }
}
