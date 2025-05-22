using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class BranchVisit00
{
    public long BranchVisitId { get; set; }

    public string? BatchCode { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? FlowStatus { get; set; }

    public int? ResubmitCount { get; set; }
}
