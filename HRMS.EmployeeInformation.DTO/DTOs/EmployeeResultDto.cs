using EMPLOYEE_INFORMATION.Models.EnumFolder;

namespace MPLOYEE_INFORMATION.DTO.DTOs
{
    public class EmployeeResultDto
    {

        public int? EmpId { get; set; }
        public string? EmpCode { get; set; }
        public string? Name { get; set; }
        public string? GuardiansName { get; set; }
        public string? DateOfBirth { get; set; }
        public string? JoinDate { get; set; }
        public string? DataDate { get; set; }
        public string? Gender { get; set; }
        public string? MaritalStatus { get; set; }
        public int? SeperationStatus { get; set; }
        public string? ProbationDt { get; set; }
        public bool? IsProbation { get; set; }
        public int? LastEntity { get; set; }
        public int? CurrentStatus { get; set; }
        public string? EmpStatus { get; set; }
        public string? EmpFileNumber { get; set; }
        public int? IsSave { get; set; }
        public string? OfficialEmail { get; set; }
        public string? PersonalEmail { get; set; }
        public string? Phone { get; set; }
        public string? ProbationStatus { get; set; }
        public string? Nationality { get; set; }
        public string? ResignationDate { get; set; }
        public string? RelievingDate { get; set; }
        public string? LevelOneDescription { get; set; }
        public string? LevelTwoDescription { get; set; }
        public string? LevelThreeDescription { get; set; }
        public string? LevelFourDescription { get; set; }

        public string? LevelFiveDescription { get; set; }

        public string? LevelSixDescription { get; set; }

        public string? LevelSevenDescription { get; set; }

        public string? LevelEightDescription { get; set; }

        public string? LevelNineDescription { get; set; }
        public string? LevelTenDescription { get; set; }

        public string? LevelElevenDescription { get; set; }

        public string? Age { get;set; }
        public int? DailyRateTypeId { get;  set; }
        public int? PayrollMode { get;  set; }
        public string? SalaryType { get;  set; }
        public string? ReportingEmployeeCode { get;  set; }
        public string? ReportingEmployeeName { get;  set; }
        public string? ImageUrl { get;  set; }
        public double PayScale { get;  set; }
        public string? EmpStatusDesc { get;  set; }
        public string? Description { get;  set; }
        public string? ResignationType { get;  set; }
        public string? ReligionName { get;  set; }
        public string? BloodGroup { get;  set; }
        public string? WorkingStatus { get; set; }
        public double? GrossPay { get;  set; }
        public DateTime? WeddingDate { get;  set; }
        public string? StatusDesc { get;  set; }
        public string? ResignationReason { get;  set; }
        public ProbationStatus Probation { get; set; }
    }
}
