using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class AirTicketAllowance
{
    public int AirTicketAllowanceId { get; set; }

    public string? Batchcode { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? FlowStatus { get; set; }

    public int? EmpId { get; set; }

    public int? ProxyEmpId { get; set; }

    public int? RecoveryMode { get; set; }

    public double? Amount { get; set; }

    public string? Status { get; set; }

    public int? ProcesspayRollId { get; set; }

    public int? ProcessPayRoll01Id { get; set; }

    public int? PayRollPeriodSubId { get; set; }
}
