using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class LeaveBalanceAmedment00
{
    public long LeaveBalanceAmndId { get; set; }

    public string? BatchCode { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Status { get; set; }

    public string? FlowStatus { get; set; }

    public int? LeaveMasterId { get; set; }

    public string? Remarks { get; set; }

    public DateTime? FinalApprovalDate { get; set; }
}
