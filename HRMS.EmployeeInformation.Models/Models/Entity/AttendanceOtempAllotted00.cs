using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class AttendanceOtempAllotted00
{
    public int OtreqId { get; set; }

    public string? BatchCode { get; set; }

    public int? EmpCount { get; set; }

    public DateTime? Otdate { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? FlowStatus { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? Status { get; set; }
}
