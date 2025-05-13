using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class HrShiftseason00
{
    public int ShiftSeason01Id { get; set; }

    public int? ShiftId { get; set; }

    public int? ShiftStartType { get; set; }

    public decimal? StartTime { get; set; }

    public int? ShiftEndType { get; set; }

    public decimal? EndTime { get; set; }

    public decimal? TotalHours { get; set; }

    public DateTime? EffectiveFrom { get; set; }

    public decimal? MinimumWorkHours { get; set; }
}
