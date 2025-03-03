using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
    {
    public class VisaFilesPendingDto
        {
        public int? DocID {  get; set; }
        public int? DetailID { get; set; }
        public string? FolderName { get; set; }
        public string? Status { get; set; }
        }
    }
