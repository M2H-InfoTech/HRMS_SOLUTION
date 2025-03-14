namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class LanguageSkillDto
    {
        public string? LanguageId { get; set; }
        public bool Read { get; set; }
        public bool Write { get; set; }
        public bool Speak { get; set; }
        public bool Comprehend { get; set; }
        public bool MotherTongue { get; set; }
    }
}
