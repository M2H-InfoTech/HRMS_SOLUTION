using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class Resignation
{
    public long ResignationId { get; set; }

    public string? ResignationRequestId { get; set; }

    public int? EmpId { get; set; }

    public DateTime? ResignationDate { get; set; }

    public DateTime? RequestDate { get; set; }

    public string? RelievingType { get; set; }

    public string? Reason { get; set; }

    public string? Remarks { get; set; }

    public int? EntryBy { get; set; }

    public int? ProxyId { get; set; }

    public string? FlowStatus { get; set; }

    public string? EntryFrom { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? HandOverStatus { get; set; }

    public string? ExitClearenceStatus { get; set; }

    public string? RejoinStatus { get; set; }

    public string? OtherReason { get; set; }

    public DateTime? RelievingDate { get; set; }

    public string? FinalSettleStatus { get; set; }

    public string? ResignationType { get; set; }

    public string? PayrollStatus { get; set; }

    public DateTime? RejoinDate { get; set; }

    public string? RejoinRemarks { get; set; }

    public DateTime? OnNoticeEndDate { get; set; }

    public DateTime? ActualRelievingDate { get; set; }

    public string? RejoinApprovalStatus { get; set; }

    public DateTime? RejoinRequestDate { get; set; }

    public int? RejoinRequestBy { get; set; }

    public DateTime? RejoinApprovalDate { get; set; }

    public string? RejoinFlowStatus { get; set; }

    public string? RejoinRequestId { get; set; }

    public int? Finalsettlementstatus { get; set; }

    public int? CurrentRequest { get; set; }

    public int? IsDirect { get; set; }

    public int? Upload { get; set; }

    public int? PayType { get; set; }

    public string? DescriptionHeight { get; set; }

    public int? Overridestatus { get; set; }

    public int? Esobentryby { get; set; }

    public DateTime? EsobentryDate { get; set; }

    public string? Esobremark { get; set; }

    public string? IsEmployeeLeftOrganisation { get; set; }

    public DateTime? FinalApprovalDate { get; set; }
}
