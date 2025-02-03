using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class ShiftMasterAccess
{
    public long ShiftAccessId { get; set; }

    public int? EmployeeId { get; set; }

    public int? ShiftId { get; set; }

    public int? IsCompanyLevel { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Active { get; set; }

    public DateTime? ValidDatefrom { get; set; }

    public DateTime? ValidDateTo { get; set; }

    public int? WeekEndMasterId { get; set; }

    public int? ShiftApprovalId { get; set; }

    public string? ApprovalStatus { get; set; }

    public int? ProjectId { get; set; }
}
