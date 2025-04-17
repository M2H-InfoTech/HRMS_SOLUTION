using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models;

public partial class HrmPayscaleMasterAccess
{
    public long IdPayscaleMasterAccess { get; set; }

    public int? EmployeeId { get; set; }

    public int? PayscaleMasterId { get; set; }

    public int? BasicSettingsId { get; set; }

    public int? IsCompanyLevel { get; set; }

    public string? Active { get; set; }

    public DateTime? ValidDatefrom { get; set; }

    public DateTime? ValidDateTo { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }
}
