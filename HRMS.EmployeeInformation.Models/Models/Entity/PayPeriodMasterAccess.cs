using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models;

public partial class PayPeriodMasterAccess
{
    public long PayPeriodMasterAccessId { get; set; }

    public int? EmployeeId { get; set; }

    public int? PayrollPeriodId { get; set; }

    public int? BasicSettingsId { get; set; }

    public int? IsCompanyLevel { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Active { get; set; }

    public DateTime? ValidDateFrom { get; set; }

    public DateTime? ValidDateTo { get; set; }
}
