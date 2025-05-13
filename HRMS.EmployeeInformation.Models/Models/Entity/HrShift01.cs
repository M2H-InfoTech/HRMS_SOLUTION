using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class HrShift01
{
    public int Shift01Id { get; set; }

    public int? ShiftId { get; set; }

    public int? ShiftStartType { get; set; }

    public decimal? StartTime { get; set; }

    public int? ShiftEndType { get; set; }

    public decimal? EndTime { get; set; }

    public decimal? TotalHours { get; set; }

    public DateTime? EffectiveFrom { get; set; }

    public decimal? MinimumWorkHours { get; set; }

    public double? FirstHalf { get; set; }

    public double? SecondHalf { get; set; }

    public double? StartTimeMinutes { get; set; }

    public double? EndTimeMinutes { get; set; }

    public double? TotalMinutes { get; set; }

    public double? MinimumWorkMinutes { get; set; }

    public double? FirstHalfMinutes { get; set; }

    public double? SecondHalfMinutes { get; set; }
}
