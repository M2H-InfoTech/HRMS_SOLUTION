using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class DailyRatePolicy00
{
    public int RateId { get; set; }

    public string? Name { get; set; }

    public string? ExcludedWeaklyHoliday { get; set; }

    public string? ExcludedPublicHoliday { get; set; }

    public DateTime EntryDate { get; set; }

    public int EntryBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdatedBy { get; set; }

    public int? GratuityFormulaId { get; set; }

    public int? FixedDays { get; set; }

    public string? Days { get; set; }

    public int? MonthlyFlag { get; set; }
}
