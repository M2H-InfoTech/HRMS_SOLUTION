namespace MPLOYEE_INFORMATION.DTO.DTOs
{
    public class EmployeeInformationParameters
    {
        public int empId { get; set; }
        public int roleId { get; set; }
        public int userId { get; set; }
        public string? empIds { get; set; }
        public string? empStatus { get; set; }
        public string? systemStatus { get; set; }
        public string? filterType { get; set; }
        public DateTime? durationFrom { get; set; }
        public DateTime? durationTo { get; set; }
        public int probationStatus { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }

        public int draw { get; set; }

        //public bool linkLevelExists { get; set; }

        //public string? ageFormat { get; set; }
        //public string? currentStatusDesc { get; set; }

        //public bool existsEmployee { get; set; }
    }
}
