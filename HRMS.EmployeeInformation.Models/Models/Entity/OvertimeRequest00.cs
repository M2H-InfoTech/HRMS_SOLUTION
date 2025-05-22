using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class OvertimeRequest00
{
    public int OverTimeId { get; set; }

    public string OvertimeRequest { get; set; } = null!;

    public int EmpId { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public DateTime FromTime { get; set; }

    public DateTime ToTime { get; set; }

    public double? TotalTime { get; set; }

    public string? Reason { get; set; }

    public string? FlowStatus { get; set; }

    public string? ApprovalStatus { get; set; }

    public int? ProxyId { get; set; }

    public int? EntryBy { get; set; }

    public int? ShiftId { get; set; }

    public DateTime EntryDate { get; set; }

    public string? EntryFrom { get; set; }

    public int? EndNextDay { get; set; }

    public int? Type { get; set; }

    public double? HoursApproved { get; set; }
}
