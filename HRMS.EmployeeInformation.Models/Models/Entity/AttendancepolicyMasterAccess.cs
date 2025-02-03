using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class AttendancepolicyMasterAccess
{
    public long AttendanceAccessId { get; set; }

    public int? EmployeeId { get; set; }

    public int? PolicyId { get; set; }

    public int? IsCompanyLevel { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Active { get; set; }

    public DateTime? ValidDatefrom { get; set; }

    public DateTime? ValidDateTo { get; set; }

    public int? IsExcludeBreakHours { get; set; }
}
