using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models;

public partial class Payroll01
{
    public int PayrollPeriodSubId { get; set; }

    public int? PayrollPeriodId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Description { get; set; }

    public DateTime? DeadlineDate { get; set; }

    public DateTime? StartMidDate { get; set; }

    public DateTime? EndMidDate { get; set; }

    public int? PayRollDaysFrom { get; set; }

    public int? PayRollDaysFixedFrom { get; set; }

    public int? PayRollDaysTo { get; set; }

    public int? PayRollDaysFixedTo { get; set; }

    public DateTime? ShowFromDate { get; set; }

    public DateTime? ShowToDate { get; set; }

    public bool? IsClose { get; set; }
}
