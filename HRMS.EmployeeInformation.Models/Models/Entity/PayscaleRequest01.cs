using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class PayscaleRequest01
{
    public long PayRequest01Id { get; set; }

    public long? PayRequestId { get; set; }

    public int? BatchId { get; set; }

    public int? EmployeeId { get; set; }

    public double? TotalEarnings { get; set; }

    public double? TotalDeductions { get; set; }

    public double? TotalPay { get; set; }

    public string? EmployeeStatus { get; set; }

    public DateTime? EffectiveDate { get; set; }

    public int? IsRevision { get; set; }

    public DateTime? RevisionFrom { get; set; }

    public double? ActualGrossFixed { get; set; }

    public int? Payrolltype { get; set; }

    public int? TotalHours { get; set; }

    public double? HourlyAmount { get; set; }

    public int? PayscaleSlab { get; set; }

    public int? OverrideStatus { get; set; }

    public int? CalculateArrear { get; set; }

    public int? OverrideId { get; set; }

    public string? PayscaleEmpRemarks { get; set; }
}
