namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class AddEmployeeDto
    {
        public string? empCode { get; set; }

        public int InstId { get; set; }
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? EmailId { get; set; }
        public string? PersonalEmail { get; set; }
        public DateTime? DateOfBirth { get; set; } = DateTime.Now;
        public string? Gender { get; set; }
        public string? GuardiansName { get; set; }
        public DateTime? JoinDt { get; set; } = DateTime.Now;
        public int? EmpStatus { get; set; }
        public DateTime? ReviewDt { get; set; } = DateTime.Now;
        public DateTime? ProbationDt { get; set; } = DateTime.Now;
        public bool? IsProbation { get; set; }
        public string? NationalIdNo { get; set; }
        public string? PassportNo { get; set; }
        public int? NoticePeriod { get; set; }
        public int EntryBy { get; set; }
        public DateTime EntryDt { get; set; } = DateTime.Now;
        public int? LastEntity { get; set; }
        public int? CurrentStatus { get; set; }
        public int? SeperationStatus { get; set; }
        public bool? Ishra { get; set; }
        public int? CountryOfBirth { get; set; }
        public DateTime? GratuityStrtDate { get; set; }
        public DateTime? FirstEntryDate { get; set; } = DateTime.Now;
        public int? IsExpat { get; set; }
        public bool? CompanyConveyance { get; set; }
        public bool? CompanyVehicle { get; set; }
        public DateTime? InitialDate { get; set; } = DateTime.Now;
        public bool? MealAllowanceDeduct { get; set; }
        public bool? InitialPaymentPending { get; set; }
        public string? IsAutoCode { get; set; }
        public string? EmpFileNumber { get; set; }
        public int? DailyRateTypeId { get; set; }
        public int? PayrollMode { get; set; }
        public bool? CanteenRequest { get; set; }
        public DateTime? WeddingDate { get; set; } = DateTime.Now;
        public string? Phone { get; set; }
        public string? HomeCountryPhone { get; set; }
        public string? MaritalStatus { get; set; }
        public int? ReligionID { get; set; }
        public string? BloodGrp { get; set; }
        public int? NationalityID { get; set; }
        public int? CountryID { get; set; }
        public string? IdentMark { get; set; }
        public string? Height { get; set; }
        public string? Weight { get; set; }
        public int? ReprotToWhome { get; set; }
        public DateTime? EffectDate { get; set; }
        public string? Active { get; set; }
        public int? empFirstEntiry { get; set; }
        public string? Add1 { get; set; }
        public string? PBNo { get; set; }
        public string? Mobile { get; set; }
        public string? OfficePhone { get; set; }
        public int MobileAppNeeded { get; set; }
        public int UserRole { get; set; }

    }
}
