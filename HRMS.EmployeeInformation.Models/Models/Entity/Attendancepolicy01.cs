using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class Attendancepolicy01
{
    public int AttendancePolicy01Id { get; set; }

    public int AttendancePolicyId { get; set; }

    public string? MaxLateComingLimitNo { get; set; }

    public string? MaxEarlyOutLimitNo { get; set; }

    public string? MaxLateComingLimitMin { get; set; }

    public string? MaxEarlyOutLimitMin { get; set; }

    public string? EarlyGapLimitNo { get; set; }

    public string? LateGapLimitNo { get; set; }

    public int? PolicyConId { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int? CreatedBy { get; set; }
}
