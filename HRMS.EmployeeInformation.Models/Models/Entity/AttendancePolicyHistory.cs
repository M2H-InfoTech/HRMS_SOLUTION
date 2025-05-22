using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class AttendancePolicyHistory
{
    public long AttendancePolicyHistoryId { get; set; }

    public int? AttendancePolicyId { get; set; }

    public int? EmployeeId { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? Ipaddress { get; set; }
}
