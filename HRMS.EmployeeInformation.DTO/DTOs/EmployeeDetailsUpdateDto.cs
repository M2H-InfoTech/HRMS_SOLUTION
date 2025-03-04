namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class EmployeeDetailsUpdateDto
    {
        public int InstId { get; set; }

        public int EmpId { get; set; }

        public string? EmpCode { get; set; }

        public string FirstName { get; set; } = null!;

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }
        public string? EmailId { get; set; }
        public string? PersonalEmail { get; set; }

        public DateTime? DateOfBirth { get; set; } = DateTime.Now;

        public string? Gender { get; set; }

        public string? GuardiansName { get; set; }

        public DateTime? JoinDt { get; set; } = DateTime.Now;

        public string? NaturePost { get; set; }

        public int? EmpStatus { get; set; }

        public DateTime? StatusChangeDate { get; set; } = DateTime.Now;

        public DateTime? ReviewDt { get; set; } = DateTime.Now;

        public DateTime? BonusDt { get; set; } = DateTime.Now;

        public DateTime? ProbationDt { get; set; } = DateTime.Now;

        public bool? IsProbation { get; set; }

        public string? NationalIdNo { get; set; }

        public string? PassportNo { get; set; }

        public int? NoticePeriod { get; set; }

        public int? BranchId { get; set; }

        public int? DepId { get; set; }

        public int? BandId { get; set; }

        public int? GradeId { get; set; }

        public int? DesigId { get; set; }

        public int EntryBy { get; set; }

        public DateTime EntryDt { get; set; } = DateTime.Now;

        public string? RejoinRemarks { get; set; }

        public int? CompanyId { get; set; }

        public int? LastEntity { get; set; }

        public int? CurrentStatus { get; set; }

        public string? EmpFirstEntity { get; set; }

        public string? EmpEntity { get; set; }

        public DateTime? RelievingDate { get; set; }

        public int? IsVerified { get; set; }

        public int? SeperationStatus { get; set; }

        public int? IsExperienced { get; set; }

        public int? ProbationNoticePeriod { get; set; }

        public bool? Ishra { get; set; }

        public int? CountryOfBirth { get; set; }

        public DateTime? GratuityStrtDate { get; set; }

        public DateTime? FirstEntryDate { get; set; } = DateTime.Now;

        public bool? IsMarkAttn { get; set; }

        public bool? PublicHoliday { get; set; }

        public int? IsExpat { get; set; }

        public string? UserType { get; set; }

        public bool? CompanyConveyance { get; set; }

        public bool? CompanyVehicle { get; set; }

        public DateTime? InitialDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; } = DateTime.Now;

        public bool? MealAllowanceDeduct { get; set; }

        public bool? InitialPaymentPending { get; set; }

        public bool? IsDelete { get; set; }

        public int? DeletedBy { get; set; }

        public DateTime? DeletedDate { get; set; } = DateTime.Now;

        public int? IsSave { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; } = DateTime.Now;

        public int? DisableGratuity { get; set; }

        public int? GrtExperianceFrmFirstentryDate { get; set; }

        public int? GrtstartFrmFirstentryDate { get; set; }

        public string? EmpFileNumber { get; set; }

        public int? DailyRateTypeId { get; set; }

        public int? PayrollMode { get; set; }

        public bool? CanteenRequest { get; set; }

        public DateTime? WeddingDate { get; set; } = DateTime.Now;
        public string? Phone { get; set; }
        public string? HomeCountryPhone { get; set; }

        public PersonalDetailsDto? PersonalDetailsDto { get; set; }
        public int UserRole { get; set; }
    }
}
