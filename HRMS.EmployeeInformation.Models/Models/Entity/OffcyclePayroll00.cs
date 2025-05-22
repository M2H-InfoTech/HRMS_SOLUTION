using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class OffcyclePayroll00
{
    public long OffcyclePayrollId { get; set; }

    public string? BatchCode { get; set; }

    public int? PayRollPeriodId { get; set; }

    public int? PayRollPeriodSubId { get; set; }

    public int? BatchId { get; set; }

    public string? BatchDescription { get; set; }

    public int? EmployeeCount { get; set; }

    public double? TotalEarnings { get; set; }

    public double? TotalDeductions { get; set; }

    public string? Status { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? FlowStatus { get; set; }

    public int? CategoryType { get; set; }

    public string? Remarks { get; set; }

    public DateTime? FinalApprovalDate { get; set; }

    public string? RejectReason { get; set; }

    public long? OldProcessOffcycleId { get; set; }
}
