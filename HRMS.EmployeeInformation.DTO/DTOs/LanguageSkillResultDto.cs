 

namespace MPLOYEE_INFORMATION.DTO.DTOs
{
    public class LanguageSkillResultDto
    {
        public int Emp_LangId { get; set; }

        public string? Name { get; set; }
        public byte? LanguageId { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }

        public byte? Read { get; set; }
        public byte? Write { get; set; }
        public byte? Speak { get; set; }
        public byte? Comprehend { get; set; }
        public byte? MotherTongue { get; set; }

    }
}
