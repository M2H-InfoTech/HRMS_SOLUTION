using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class BreakPermission00
{
    public int BreakPermissionId { get; set; }

    public string? BreakPermissionRequestId { get; set; }

    public int? EmpId { get; set; }

    public DateTime? AppliedDate { get; set; }

    public TimeOnly? FromTime { get; set; }

    public TimeOnly? ToTime { get; set; }

    public string? Reason { get; set; }

    public string? FlowStatus { get; set; }

    public string? ApprovalStatus { get; set; }

    public int? ProxyId { get; set; }

    public int? EntryBy { get; set; }

    public double? TotalTime { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? EntryFrom { get; set; }

    public int? IsCompo { get; set; }

    public string? UpdatedFrom { get; set; }

    public string? CancelRemarks { get; set; }
}
