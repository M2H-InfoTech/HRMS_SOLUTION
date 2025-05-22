using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class CompoOff00
{
    public int CompoId { get; set; }

    public string? CompoRequestId { get; set; }

    public int? EmpId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? FlowStatus { get; set; }

    public string? ApprovalStatus { get; set; }

    public int? ProxyId { get; set; }

    public int? EntryBy { get; set; }

    public int? CompoLeave { get; set; }

    public double? NeededHours { get; set; }

    public decimal? CompCaryfrwrd { get; set; }

    public double? AvailableHours { get; set; }

    public int? ShiftId { get; set; }

    public double? Days { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? EntryFrom { get; set; }

    public string? Remarks { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public int? CompoType { get; set; }

    public int? DayType { get; set; }

    public int? FirstHalf { get; set; }

    public int? Secondhalf { get; set; }

    public string? Purpose { get; set; }

    public string? CancelRemarks { get; set; }

    public int? CompensationType { get; set; }

    public int? CompoFlagStatus { get; set; }

    public int? ProcessPayrollId { get; set; }
}
