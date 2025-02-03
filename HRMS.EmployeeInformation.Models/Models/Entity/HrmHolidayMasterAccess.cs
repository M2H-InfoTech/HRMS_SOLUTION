using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class HrmHolidayMasterAccess
{
    public long IdHolidayMasterAccess { get; set; }

    public int? EmployeeId { get; set; }

    public int? HolidayMasterId { get; set; }

    public int? BasicSettingsId { get; set; }

    public int? IsCompanyLevel { get; set; }

    public string? Active { get; set; }

    public DateTime? ValidDatefrom { get; set; }

    public DateTime? ValidDateTo { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }
}
