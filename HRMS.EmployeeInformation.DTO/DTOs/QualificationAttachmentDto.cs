namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class QualificationAttachmentDto
    {
        public int EmployeeId { get; set; }
        public int EntryBy { get; set; }
        public List<string>? FileNames { get; set; } // Stores only file names or paths
        public int? QualificationId { get; set; }
    }
}
