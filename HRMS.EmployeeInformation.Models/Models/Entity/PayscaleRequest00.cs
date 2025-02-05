using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class PayscaleRequest00
{
    public long PayRequestId { get; set; }

    public int? BatchId { get; set; }

    public int? CurrencyId { get; set; }

    public double? TotalEarnings { get; set; }

    public double? TotalDeductions { get; set; }

    public double? TotalPay { get; set; }

    public string? EmployeeIds { get; set; }

    public string? BatchStatus { get; set; }

    public string? RejectStatus { get; set; }

    public DateTime? EffectiveDate { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? FlowStatus { get; set; }

    public string? RejectReason { get; set; }

    public int? Type { get; set; }

    public string? PayReqCode { get; set; }

    public double? ActualGrossFixed { get; set; }

    public int? Payrolltype { get; set; }

    public int? TotalHours { get; set; }

    public double? HourlyAmount { get; set; }

    public string? PayscaleRemarks { get; set; }
}
