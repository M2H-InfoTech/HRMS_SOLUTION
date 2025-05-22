using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class LeaveEnhancement00
{
    public int LeaveEnId { get; set; }

    public string SequenceId { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public string Status { get; set; } = null!;

    public string? FlowStatus { get; set; }

    public DateTime? FinalApprovalDate { get; set; }
}
