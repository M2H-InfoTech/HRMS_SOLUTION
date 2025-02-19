namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class HrEmpMasterDto
    {


        public int EmpId { get; set; }

        public string? EmpCode { get; set; }

        public string FirstName { get; set; } = null!;

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        public string? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? GuardiansName { get; set; }

        public string? JoinDt { get; set; }

        public int? EmpStatus { get; set; }
        public string? ReviewDt { get; set; }
        public string? ProbationDt { get; set; }

        public bool? IsProbation { get; set; }

        public string? NationalIdNo { get; set; }

        public string? PassportNo { get; set; }

        public int? NoticePeriod { get; set; }

        public int? BranchId { get; set; }

        public int? DepId { get; set; }

        public int? DesigId { get; set; }

        public int EntryBy { get; set; }

        public string? EntryDt { get; set; }

        public int? CurrentStatus { get; set; }

        public bool? Ishra { get; set; }

        public int? CountryOfBirth { get; set; }

        public string? GratuityStrtDate { get; set; }

        public string? FirstEntryDate { get; set; }

        public bool? PublicHoliday { get; set; }

        public int? IsExpat { get; set; }

        public bool? CompanyConveyance { get; set; }

        public bool? CompanyVehicle { get; set; }

        public bool? MealAllowanceDeduct { get; set; }
        public string? EmpFileNumber { get; set; }

        public int? DailyRateTypeId { get; set; }

        public int? PayrollMode { get; set; }

        public int? CanteenRequest { get; set; }

        public string? WeddingDate { get; set; }
        public int? Religion { get; set; }
        public string? BloodGrp { get; set; }
        public string? StatusDesc { get; set; }
        public int? Nationality { get; set; }
        public string? IdentMark { get; set; }
        public int? Country { get; set; }
        public string? Height { get; set; }
        public string? Weight { get; set; }
        public string? OfficialEmail { get; set; }
        public string? PersonalEmail { get; set; }
        public string? Phone { get; set; }
        public string? HomeCountryPhone { get; set; }
        public string? MaritalStatus { get; set; }
        public int? ReprotToWhome { get; set; }
        public string? EffectDate { get; set; }
        public int RoleId { get; set; }
        public bool? NeedApp { get; set; }

        public int? LevelOneId { get; set; }
        public int? LevelTwoId { get; set; }
        public int? LevelThreeId { get; set; }
        public int? LevelFourId { get; set; }
        public int? LevelFiveId { get; set; }
        public int? LevelSixId { get; set; }
        public int? LevelSevenId { get; set; }
        public int? LevelEightId { get; set; }
        public int? LevelNineId { get; set; }
        public int? LevelTenId { get; set; }
        public int? LevelElevenId { get; set; }
        public string? ReportingEmp { get; set; }
    }
}
