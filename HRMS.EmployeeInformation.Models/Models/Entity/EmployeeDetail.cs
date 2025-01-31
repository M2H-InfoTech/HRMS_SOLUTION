using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class EmployeeDetail
{
    public int InstId { get; set; }

    public int EmpId { get; set; }

    public string? EmpCode { get; set; }

    public string? Name { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public int? BranchId { get; set; }

    public int? GradeId { get; set; }

    public int? BandId { get; set; }

    public int? DepId { get; set; }

    public int? DesigId { get; set; }

    public int? LastEntity { get; set; }

    public DateTime? JoinDt { get; set; }

    public DateTime? ProbationDt { get; set; }

    public string? GuardiansName { get; set; }

    public string? Gender { get; set; }

    public int? EmpStatus { get; set; }

    public int? CurrentStatus { get; set; }

    public string? EmpFirstEntity { get; set; }

    public string? EmpEntity { get; set; }

    public int? SeperationStatus { get; set; }

    public int? IsExperienced { get; set; }

    public int? NoticePeriod { get; set; }

    public int? ProbationNoticePeriod { get; set; }

    public DateTime? GratuityStrtDate { get; set; }

    public DateTime? FirstEntryDate { get; set; }

    public bool? Ishra { get; set; }

    public int? IsExpat { get; set; }

    public bool? CompanyConveyance { get; set; }

    public bool? CompanyVehicle { get; set; }

    public bool IsDelete { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedDate { get; set; }

    public int IsSave { get; set; }
}
