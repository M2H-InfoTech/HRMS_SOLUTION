using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class GrievanceRegistration
{
    public long GrievRegId { get; set; }

    public string? RequestCode { get; set; }

    public int? GrievCatId { get; set; }

    public int? GrievSubCatId { get; set; }

    public string? GrievDescription { get; set; }

    public int? AttachmentCount { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? FlowStatus { get; set; }

    public bool? Active { get; set; }

    public int? EmployeeId { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? DaysCount { get; set; }

    public int? HourCount { get; set; }

    public int? MinutesCount { get; set; }

    public string? PriorityType { get; set; }

    public bool? IsReOpen { get; set; }

    public bool? IsExceeding { get; set; }

    public string? Type { get; set; }

    public int? IsEscalate { get; set; }

    public DateTime? ReOpenDate { get; set; }

    public DateTime? EscalateDate { get; set; }

    public DateTime? ResolveDate { get; set; }

    public DateTime? CloseDate { get; set; }

    public string? EntryForm { get; set; }

    public string? DiscriptionHeight { get; set; }

    public int? FinalApproverId { get; set; }

    public int? WistleBlowerSettings { get; set; }
}
