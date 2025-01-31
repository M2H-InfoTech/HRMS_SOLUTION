 

namespace MPLOYEE_INFORMATION.DTO.DTOs
{
    public class LanguageSkillResultDto
    {
        public int Emp_LangId { get; set; }

        public string? Name { get; set; }
        public int LanguageId { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }

        public int Read { get; set; }
        public int Write { get; set; }
        public int Speak { get; set; }
        public int Comprehend { get; set; }
        public int MotherTongue { get; set; }

    }
}
