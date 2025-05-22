using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class LateInEarlyOutCancel00
{
    public int LateInCancelId { get; set; }

    public string? BatchCode { get; set; }

    public int? LateInReqId { get; set; }

    public int? EmpId { get; set; }

    public int? ProxyId { get; set; }

    public DateTime? DateAppliedFor { get; set; }

    public double? Time { get; set; }

    public string? CanCelReason { get; set; }

    public string? ReqType { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Status { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? FlowStatus { get; set; }
}
