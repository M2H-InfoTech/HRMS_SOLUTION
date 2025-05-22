using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class Attendancepolicy02
{
    public int AttendancePolicy02Id { get; set; }

    public int AttendancePolicyId { get; set; }

    public int? OverTimeTypeId { get; set; }

    public decimal? Maximum { get; set; }

    public decimal? Minimum { get; set; }

    public string? WeekDay { get; set; }

    public bool? OthoursAfterConsider { get; set; }

    public decimal? StartTime { get; set; }

    public decimal? EndTime { get; set; }

    public int? PolicyDayType { get; set; }
}
