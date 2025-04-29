using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class HrmLeaveBasicSetting
{
    public int SettingsId { get; set; }

    public string? SettingsName { get; set; }

    public string? SettingsDescription { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? LeaveMasterId { get; set; }

    public int? DaysOrHours { get; set; }

    public int? RejoinWarningShow { get; set; }

    public double? RejoinWarningShowDaysMax { get; set; }
}
