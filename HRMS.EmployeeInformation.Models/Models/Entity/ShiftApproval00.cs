using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class ShiftApproval00
{
    public int Id { get; set; }

    public string? RequestId { get; set; }

    public int? EmployeeId { get; set; }

    public int? ShiftType { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? FlowStatus { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }
}
