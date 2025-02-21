using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class TmpFileUpDto
    {
        public int DetailID { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public Byte[]? FileData { get; set; }
    }
}
 