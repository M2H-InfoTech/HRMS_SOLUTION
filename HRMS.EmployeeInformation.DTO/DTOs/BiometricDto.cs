using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class BiometricDto
    {
        public int InstId { get; set; }
        public int EmployeeID { get; set; }
        public int BranchBiometricId { get; set; }
        public string? BiometricId { get; set; }
        public string? BiometricIdEdit { get; set; }
        public int EntryBy { get; set; }
        public DateTime EntryDt { get; set; }
        public bool MarkAttn { get; set; }
    }

}
