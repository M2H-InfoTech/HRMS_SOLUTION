using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class Payroll00
{
    public int PayrollPeriodId { get; set; }

    public string? PeriodCode { get; set; }

    public string? Description { get; set; }

    public int? PeriodType { get; set; }

    public bool? IsExcludePayroll { get; set; }

    public DateTime? StartDate { get; set; }

    public int? FixedDays { get; set; }

    public int? AddNextYear { get; set; }

    public int? MonthlyFixedDays { get; set; }

    public int? DeadlineDays { get; set; }

    public int? EndDay { get; set; }

    public int? EndDayValue { get; set; }

    public int? PayRollDays { get; set; }

    public bool? IsSplitePayRoll { get; set; }

    public int? EndDayValueSecond { get; set; }

    public int? EnableSandwich { get; set; }

    public int? Companyworkingdayscalc { get; set; }

    public int? EnableLeaveencashment { get; set; }

    public int? IsIndia { get; set; }
}
