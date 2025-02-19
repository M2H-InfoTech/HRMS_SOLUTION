namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class PersonalDetailsUpdateDto
    {
        public int? EmpId { get; set; }
        public string? GuardiansName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PersonalEMail { get; set; }
        public int? CountryID { get; set; }
        public int? NationalityID { get; set; }
        public int? country2ID { get; set; }
        public string? BloodGrp { get; set; }
        public int? ReligionID { get; set; }
        public string? IdentificationMark { get; set; }
        public string? Height { get; set; }
        public string? Weight { get; set; }
        public DateTime? WeddingDate { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Gender { get; set; }
        public int EntryBy { get; set; }


    }
}
