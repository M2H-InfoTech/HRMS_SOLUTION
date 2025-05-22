using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class LeaveEnhancementRequest00
{
    public int Id { get; set; }

    public int? EmployeeId { get; set; }

    public string? Description { get; set; }

    public double? Enhancement { get; set; }

    public string? Reason { get; set; }

    public string? SequenceId { get; set; }

    public int? LeaveType { get; set; }

    public string? FlowStatus { get; set; }

    public int? ProxyId { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDt { get; set; }

    public string? Status { get; set; }
}
