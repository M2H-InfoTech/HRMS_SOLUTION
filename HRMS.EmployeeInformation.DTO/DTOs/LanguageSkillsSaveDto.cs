namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class LanguageSkillsSaveDto
    {

        public int EmpID { get; set; }
        public List<LanguageSkillDto>? Lilanguage { get; set; } // Ensure this is a list
    }

}
