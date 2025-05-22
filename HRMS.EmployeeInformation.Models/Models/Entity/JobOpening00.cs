using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class JobOpening00
{
    public int JobOpeningMasterId { get; set; }

    public string? RequestId { get; set; }

    public int? EmployeeId { get; set; }

    public string? JobTitle { get; set; }

    public string? Location { get; set; }

    public string? Qualification { get; set; }

    public int? NoOfPost { get; set; }

    public DateTime? PostedOn { get; set; }

    public DateTime? TargetDate { get; set; }

    public int? JobOpeningStatus { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? FlowStatus { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? FunctionalAreaId { get; set; }

    public string? YearsOfExprce { get; set; }

    public DateTime? JobExpire { get; set; }

    public string? Priority { get; set; }

    public string? Purpose { get; set; }

    public string? JobDesc { get; set; }

    public string? HiringType { get; set; }

    public string? RolesAndResponse { get; set; }

    public DateTime? DateFrom { get; set; }

    public int? EntryBy { get; set; }
}
