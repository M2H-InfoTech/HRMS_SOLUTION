using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class HrShift02
{
    public int Shift02Id { get; set; }

    public int? ShiftId { get; set; }

    public int? BreakStartType { get; set; }

    public decimal? BreakStartTime { get; set; }

    public int? BreakEndType { get; set; }

    public decimal? BreakEndTime { get; set; }

    public decimal? TotalBreakHours { get; set; }

    public DateTime? EffectiveFrom { get; set; }

    public string? IsPaid { get; set; }

    public int? Shift01Id { get; set; }

    public double? TotalBreakMinutes { get; set; }
}
