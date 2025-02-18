

namespace MPLOYEE_INFORMATION.DTO.DTOs
{
    public class QualificationDto
    {
        public IEnumerable<QualificationTableDto>? QualificationTable { get; set; }
        public IEnumerable<QualificationFileDto>? QualificationFile { get; set; }
    }

    public class QualificationTableDto
    {
        public int Qlf_id { get; set; }
        public int? Emp_Id { get; set; }
        public int InstId { get; set; }
        public string? Course { get; set; }
        public string? University { get; set; }
        public string? Inst_Name { get; set; }
        public string? Dur_Frm { get; set; }
        public string? Dur_To { get; set; }
        public string? Year_Pass { get; set; }
        public string? Mark_Per { get; set; }
        public string? Class { get; set; }
        public string? Subjects { get; set; }
        public string? Status { get; set; }
        public int? Entryby { get; set; }
        public DateTime? EntryDate { get; set; }
        public int CourseId { get; set; }
        public int UniversityId { get; set; }
        public int InstitutId { get; set; }
        public int SpecialId { get; set; }




        }
    public class QualificationFileDto
    {
        public int QualAttachId { get; set; }
        public int? QualificationId { get; set; }
        public string? QualFileName { get; set; }
        public string? DocStatus { get; set; }
    }

}
