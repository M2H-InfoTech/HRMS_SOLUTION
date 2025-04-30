using HRMS.EmployeeInformation.DTO.DTOs;

namespace LEAVE.Dto
{
    public class AccessMetadataDto
    {
        public int TransactionId { get; set; }
        public int LinkLevel { get; set; }
        public bool HasAccessRights { get; set; }
        public AccessCheckResultDto? accessCheckResultDto { get; set; }
    }
}
