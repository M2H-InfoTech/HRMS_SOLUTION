using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class ReportingRequest
{
    public int ReportingId { get; set; }

    public string? RequestId { get; set; }

    public string? EmpId { get; set; }

    public int? ReportingTo { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? EntryBy { get; set; }

    public string? Remarks { get; set; }

    public string? FlowStatus { get; set; }

    public string? ApprovalStatus { get; set; }

    public int? ProxyId { get; set; }
}
