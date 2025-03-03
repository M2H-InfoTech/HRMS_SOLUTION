using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
    {
    public class DocumentGetFolderNameDto
        {
        public long DocID { get; set; }
        public string? DocName { get; set; }
        public string? FolderName { get; set; }
        }
    }
