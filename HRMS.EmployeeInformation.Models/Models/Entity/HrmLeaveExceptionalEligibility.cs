using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models;

public partial class HrmLeaveExceptionalEligibility
{
    public int EligibilityRegId { get; set; }

    public int? SettingsDetailsHeadId { get; set; }

    public int? Year { get; set; }

    public int? Month { get; set; }

    public decimal? Count { get; set; }
}
