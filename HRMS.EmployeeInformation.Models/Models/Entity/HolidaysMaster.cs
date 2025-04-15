using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class HolidaysMaster
{
    public int InstId { get; set; }

    public int HolidayMasterId { get; set; }

    public DateTime? HolidayFromDate { get; set; }

    public DateTime? HolidayToDate { get; set; }

    public string? HolidayName { get; set; }

    public string? Location { get; set; }

    public string? CurYear { get; set; }

    public int? RestrictedHoliday { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int? CreatedBy { get; set; }

    public bool? PayType { get; set; }

    public int? SpecialHoliday { get; set; }

    public int? LegalHoliday { get; set; }

    public int? ExcludeCasualHoliday { get; set; }

    public int? Enablereligion { get; set; }

    public int? Religion { get; set; }

    public int? ExcludeFromPayrollDays { get; set; }
}
