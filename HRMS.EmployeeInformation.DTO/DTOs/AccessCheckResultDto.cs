namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class AccessCheckResultDto
    {
        public List<HighLevelTableDto>? AccessLevel { get; set; }
        public List<string>? Levels { get; set; }
        public List<EmployeeDto>? Employees { get; set; }
    }
}
