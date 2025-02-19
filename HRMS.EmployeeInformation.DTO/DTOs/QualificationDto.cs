


using HRMS.EmployeeInformation.DTO.DTOs;

namespace MPLOYEE_INFORMATION.DTO.DTOs
{
    public class QualificationDto
    {
        public IEnumerable<QualificationTableDto>? QualificationTable { get; set; }
        public IEnumerable<QualificationFileDto>? QualificationFile { get; set; }
    }

}
