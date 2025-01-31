using EMPLOYEE_INFORMATION.DTO.DTOs;

namespace MPLOYEE_INFORMATION.DTO.DTOs
{
    public class EmployeeStatusResultDto
    {
        public IEnumerable<EmployeeStatusDto>? EmployeeStatuses { get; set; }
        public IEnumerable<SystemStatusDto>? SystemStatuses { get; set; }
        public IEnumerable<string>? CompanyParameterCodes { get; set; }
        public IEnumerable<ActiveStatusDto>? ActiveStatus { get; set; }
    }
}
