using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
    {
    public class SetEmpDocumentDetailsDto
        {
        public int? EmpID { get; set; }
        public int? ProxyID { get; set; }
        public int EntryBy { get; set; }
        public int DocumentID { get; set; }
        public string? FlowStatus { get; set; }


        }
    }
