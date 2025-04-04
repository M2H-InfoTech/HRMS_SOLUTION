namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class UpdateEmployeeRequestDto
    {
        public int EmpID { get; set; }
        public string GuardiansName { get; set; } = "";
        public string NationalID { get; set; } = "";
        public string PassportNo { get; set; } = "";
        public int NoticePeriod { get; set; } = 0;
        public int Country2ID { get; set; } = 0;
        public DateTime? GraStrtDate { get; set; }
        public DateTime? FrstEntryDate { get; set; }
        public bool ISHRA { get; set; } = false;
        public int IsExpat { get; set; } = 0;
        public bool MarkAttn { get; set; } = false;
        public bool CompanyConveyance { get; set; } = false;
        public bool CompanyVehicle { get; set; } = false;
        public bool MealAllowanceDeduct { get; set; } = false;
        public string EmpFileNumber { get; set; } = "";
        public string MaritalStatus { get; set; } = "";
        public string BloodGroup { get; set; } = "";
        public int ReligionID { get; set; } = 0;
        public string IdentificationMark { get; set; } = "";
        public int EntryBy { get; set; } = 0;
        public DateTime? EntryDate { get; set; }
        public string Height { get; set; } = "";
        public string Weight { get; set; } = "";
        public DateTime? WeddingDate { get; set; }
        public string EmailId { get; set; } = "";
        public string PersonalEMail { get; set; } = "";
    }
}
