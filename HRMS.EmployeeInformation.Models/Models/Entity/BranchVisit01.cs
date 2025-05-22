using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class BranchVisit01
{
    public long BranchVisit01Id { get; set; }

    public long? BranchVisitId { get; set; }

    public int? EmployeeId { get; set; }

    public int? FranchiseId { get; set; }

    public int? BranchId { get; set; }

    public int? Area { get; set; }

    public int? Region { get; set; }

    public DateTime? VisitDate { get; set; }

    public decimal? Time { get; set; }

    public int? ManagerEmpId { get; set; }

    public int? NoofStaffonDuty { get; set; }

    public int? BudgetedHeadCount { get; set; }

    public int? ActualHeadCount { get; set; }

    public string? Suggestion { get; set; }

    public string? ActionPlan { get; set; }

    public double? Timeline { get; set; }

    public int? Meridian { get; set; }
}
