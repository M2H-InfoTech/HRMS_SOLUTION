using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class TmpEmpEmeDto
    {
        public string EmployeeCode { get; set; }
        public string?  EmergencyContactName { get; set; }
        public string? Address { get; set; }
        public string? PinNo { get; set; }
        public string? Country { get; set; }
        public string? PhoneNo { get; set; }
        public string? AlternatePhoneNo { get; set; }
        public string? MobileNo { get; set; }
        public string? Relation { get; set; }
    }

}
