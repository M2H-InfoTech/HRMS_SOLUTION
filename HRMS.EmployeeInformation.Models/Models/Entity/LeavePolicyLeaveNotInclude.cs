using System;
using System.Collections.Generic;
namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class LeavePolicyLeaveNotInclude
{
    public int LeaveNotIncludeId { get; set; }

    public int? LeavePolicyInstanceLimitId { get; set; }

    public int? LeavePolicyMasterId { get; set; }

    public int? LeaveId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }
}
