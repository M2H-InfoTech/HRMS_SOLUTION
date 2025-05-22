using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class AirTicketAllowanceWorkFlowstatus
{
    public int FlowId { get; set; }

    public int? RequestId { get; set; }

    public bool? ShowStatus { get; set; }

    public string? ApprovalStatus { get; set; }

    public int? Rule { get; set; }

    public bool? HierarchyType { get; set; }

    public int? Approver { get; set; }

    public string? ApprovalRemarks { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDt { get; set; }

    public string? Deligate { get; set; }

    public int? RuleOrder { get; set; }

    public string? EntryFrom { get; set; }

    public int? DelegateApprover { get; set; }

    public int? HideFlow { get; set; }
}
