namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class QualificationTableDto
    {
        public int Qlficationid { get; set; }
        public int? EmpId { get; set; }
        public string? Course { get; set; }
        public string? University { get; set; }
        public string? InstName { get; set; }
        public string? DurFrm { get; set; }
        public string? DurTo { get; set; }
        public string? YearPass { get; set; }
        public string? MarkPer { get; set; }
        public string? Class { get; set; }
        public string? Subjects { get; set; }
        public string? Status { get; set; }
        public int InstId { get; set; }
        public int Entryby { get; set; }
        public DateTime EntryDate { get; set; }
        public int? CourseId { get; set; }
        public int? UniversityId { get; set; }
        public int? InstitutId { get; set; }
        public int? SpecialId { get; set; }
    }
}
