using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models;

public partial class Payscale00
{
    public long PayScaleId { get; set; }

    public long? PayRequest01Id { get; set; }

    public long? PayRequestId { get; set; }

    public int? CurrencyId { get; set; }

    public int? BatchId { get; set; }

    public int? EmployeeId { get; set; }

    public double? TotalEarnings { get; set; }

    public double? TotalDeductions { get; set; }

    public double? TotalPay { get; set; }

    public string? EmployeeStatus { get; set; }

    public DateTime? EffectiveFrom { get; set; }

    public DateTime? EffectiveTo { get; set; }

    public int? IsRevision { get; set; }

    public DateTime? RevisionFrom { get; set; }

    public double? ActualGrossFixed { get; set; }

    public int? Payrolltype { get; set; }

    public double? HourlyAmount { get; set; }

    public int? PayscaleSlab { get; set; }

    public int? OverrideStatus { get; set; }

    public int? CalculateArrear { get; set; }

    public long? ArrearCalculatedprocesspayroll { get; set; }
}
