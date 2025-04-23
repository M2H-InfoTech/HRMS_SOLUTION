using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs.Documents
{
    //Created By Shan Lal 
    public class EmployeeDocumentListDto
    {
        public int? DocID { get; set; }
        public int? EmpID { get; set; }
        public string? Emp_Code { get; set; }
        public string? FileName { get; set; }
        public string? DocName { get; set; }
        public string? FolderName { get; set; }
    }
    public class DocumentDetailDto
    {
        public int? DocID { get; set; } 
        public int? DetailID { get; set; }
        public string? DocName { get; set; }
        public string? FileName { get; set; }
    }
}
