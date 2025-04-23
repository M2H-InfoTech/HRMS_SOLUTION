using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class Attendancepolicy00
{
    public int AttendancePolicyId { get; set; }

    public string? PolicyName { get; set; }

    public string? Criteria { get; set; }

    public string? CheckDirection { get; set; }

    public decimal? LateIn { get; set; }

    public decimal? LateOut { get; set; }

    public decimal? EarlyIn { get; set; }

    public decimal? EarlyOut { get; set; }

    public int? RoundOf { get; set; }

    public int? SpeacialSeasonId { get; set; }

    public bool? StrictShiftTime { get; set; }

    public bool? OverTimeInclude { get; set; }

    public bool? CkhOtconsiderInShortage { get; set; }

    public bool? EnableOtonRequest { get; set; }

    public int? StatusOnAbsentShortage { get; set; }

    public bool? PresentForMinimumWorkHrs { get; set; }

    public decimal? MinimumWorkHrsForPrsnt { get; set; }

    public int? SortOrder { get; set; }

    public int? SpecialOtenabled { get; set; }

    public double? ShortageFreeMinutes { get; set; }

    public int? ConsiderApprovedHours { get; set; }

    public int? EnableLateinPolicy { get; set; }

    public int? CountForLateIn { get; set; }

    public int? TimeFrom { get; set; }

    public int? TimeTo { get; set; }
}
