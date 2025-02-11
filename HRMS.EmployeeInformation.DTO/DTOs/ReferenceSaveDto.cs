using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class ReferenceSaveDto
    {
        public int EmpID { get; set; }
        public string? RefType { get; set; }
        public string? RefMethod { get; set; }
        public int RefEmpID { get; set; }
        public int ConsultantID { get; set; }
        public string? RefName { get; set; }
        public string? PhoneNo { get; set; }
        public string? Address { get; set; }
        public string? Status { get; set; }
        public int EntryBy { get; set; }
        public int DetailID { get; set; }

    }
}
