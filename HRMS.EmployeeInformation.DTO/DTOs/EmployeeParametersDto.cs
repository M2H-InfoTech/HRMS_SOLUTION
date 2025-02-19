namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class EmployeeParametersDto
    {
        public string Mode { get; set; } = string.Empty;
        public int InstId { get; set; } = 0;

        public string EmpCode { get; set; } = string.Empty;
        public int TransactionID { get; set; } = 0;
        public string TransactionType { get; set; } = string.Empty;
        public int EmpID { get; set; } = 0;
        public string EmpIDs { get; set; } = string.Empty;
        public int DeptID { get; set; } = 0;
        public int DesigID { get; set; } = 0;
        public int BandID { get; set; } = 0;
        public int GradeID { get; set; } = 0;
        public int BranchID { get; set; } = 0;
        public int CompanyID { get; set; } = 0;
        public int Country2ID { get; set; } = 0;
        public int CountryID { get; set; } = 0;
        public int LastEntity { get; set; } = 0;
        public int JobType { get; set; } = 0;
        public string Status { get; set; } = "0";
        public string SystemStatus { get; set; } = "0";
        public int ReportingTo { get; set; } = 0;
        public string TypeID { get; set; } = string.Empty;
        public string EmployeeCode { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddeleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string GuardiansName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string MaritalStatus { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; } = null;
        public string NationalID { get; set; } = string.Empty;
        public string PassportNo { get; set; } = string.Empty;
        public string EmailId { get; set; } = string.Empty;
        public bool IsMobileApp { get; set; } = false;
        public bool IsHRA { get; set; } = false;
        public bool MealAllowanceDeduct { get; set; } = false;
        public string PersonalEMail { get; set; } = string.Empty;
        public DateTime? JoiningDate { get; set; } = null;
        public DateTime? ReviewDate { get; set; } = null;
        public int NoticePeriod { get; set; } = 0;
        public bool IsProbation { get; set; } = false;
        public DateTime? ProbationDate { get; set; } = null;
        public DateTime? EffectDate { get; set; } = null;
        public string? ContactNo { get; set; }
        public string? HomeCountryPhone { get; set; }
        public int ReligionID { get; set; }
        public int NationalityID { get; set; }
        public string BloodGroup { get; set; } = string.Empty;
        public string Height { get; set; } = string.Empty;
        public string Weight { get; set; } = string.Empty;
        public string EmpEntityIds { get; set; } = string.Empty;
        public int FirstEntityID { get; set; } = 0;
        public string IsAutoCode { get; set; } = string.Empty;
        public int IsExpat { get; set; } = 0;
        public bool MarkAttn { get; set; } = false;
        public string SeqNumber { get; set; } = string.Empty;
        public int UserID { get; set; } = 0;
        public string UserIds { get; set; } = string.Empty;
        public int UserRole { get; set; } = 0;
        public int EntryBy { get; set; } = 0;
        public DateTime? EntryDate { get; set; } = null;
        public string IdentificationMark { get; set; } = string.Empty;
        public string DeviceID { get; set; } = string.Empty;
        public DateTime? GraStrtDate { get; set; } = null;
        public DateTime? FrstEntryDate { get; set; } = null;
        public bool CompanyConveyance { get; set; } = false;
        public bool CompanyVehicle { get; set; } = false;
        public int NewEmpID { get; set; } = 0;
        public int DetailID { get; set; } = 0;
        public string EmpFileNumber { get; set; } = string.Empty;
        public int PayrollMethod { get; set; } = 0;
        public int DailyRateID { get; set; } = 0;
        public int CanteenRequest { get; set; } = 0;
        public DateTime? WeddingDate { get; set; } = null;
        public DateTime? FirstEntryDate { get; set; }
    }
}
