namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class HraDetailsDto
    {
        public string EmpCode { get; set; }
        public string Name { get; set; }
        public bool IsHRA { get; set; }
        public string HRAStatus { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Remarks { get; set; }
    }

    public class EmployeeHraDto
    {
        public List<HraDetailsDto> HraHistory { get; set; } = new List<HraDetailsDto>();
        public bool? IsHRA { get; set; }
    }
}

