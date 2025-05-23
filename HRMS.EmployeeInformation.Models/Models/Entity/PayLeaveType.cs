using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class PayLeaveType
{
    public int InstId { get; set; }

    public int LeaveTypeId { get; set; }

    public string LeaveDesc { get; set; } = null!;

    public string? Descriptions { get; set; }

    public string? Active { get; set; }
}
