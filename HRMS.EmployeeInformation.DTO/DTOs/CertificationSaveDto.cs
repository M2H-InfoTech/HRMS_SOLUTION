using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
    {
    public class CertificationSaveDto
    {
            public int EmpId { get; set; }
            public int? CertificationID { get; set; } // Nullable for new inserts
            public int CertificationName { get; set; }
            public int CertificationField { get; set; }
            public int YearOfCompletion { get; set; }
            public int IssuingAuthority { get; set; }
            public int EntryBy { get; set; }
    }
}
