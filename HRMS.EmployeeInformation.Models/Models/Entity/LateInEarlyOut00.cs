using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class LateInEarlyOut00
{
    public int LateEarlyId { get; set; }

    public string? LateSequenceId { get; set; }

    public int? CompanyId { get; set; }

    public int? EmployeeId { get; set; }

    public DateOnly? AppliedOn { get; set; }

    public DateOnly? Date { get; set; }

    public decimal? BreakStart { get; set; }

    public decimal? BreakEnd { get; set; }

    public string? ApprovalStatus { get; set; }

    public int? ProxyEmployeeId { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? EntryFrom { get; set; }

    public string? FlowStatus { get; set; }

    public int? Type { get; set; }

    public string? Time { get; set; }

    public int? IsCompo { get; set; }

    public string? UpdatedFrom { get; set; }

    public string? CancelRemarks { get; set; }

    public int? PersonalOrOfficial { get; set; }

    public string? CancelStatus { get; set; }
}
