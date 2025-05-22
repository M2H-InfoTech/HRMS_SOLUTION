using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class SalaryOnHoldNew00
{
    public long SalaryOnHoldId { get; set; }

    public string? RequestCode { get; set; }

    public int? BatchType { get; set; }

    public long? PayRollPeriodId { get; set; }

    public long? PayRollPeriodSubId { get; set; }

    public string? Remark { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Status { get; set; }

    public string? FlowStatus { get; set; }

    public bool? IsDirect { get; set; }

    public int? FromPayroll { get; set; }
}
