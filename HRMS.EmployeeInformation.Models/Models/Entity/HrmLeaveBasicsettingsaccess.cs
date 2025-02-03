using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class HrmLeaveBasicsettingsaccess
{
    public long IdEmployeeSettinsAccess { get; set; }

    public int? EmployeeId { get; set; }

    public int? SettingsId { get; set; }

    public int? LeaveMasterId { get; set; }

    public int? IsCompanyLevel { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? LinkLevel { get; set; }

    public int? AssignPeriodBs { get; set; }

    public DateTime? FromDateBs { get; set; }

    public DateTime? ValidToBs { get; set; }

    public decimal? Laps { get; set; }
}
