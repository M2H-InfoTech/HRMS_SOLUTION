using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class ClaimsApplication00
{
    public int ClaimsAppId { get; set; }

    public string? RequestId { get; set; }

    public int? EmployeeId { get; set; }

    public int? ProxyId { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? FlowStatus { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? CategoryId { get; set; }

    public double? TotalAmount { get; set; }

    public int? Travelid { get; set; }

    public int? PayrollType { get; set; }

    public DateTime? Claimreleasedate { get; set; }

    public string? Releasestatus { get; set; }

    public bool? IsSubmit { get; set; }

    public int? IsSpecialWorkFlow { get; set; }

    public int? Checker { get; set; }

    public int? FinalApprover { get; set; }

    public string? EntryFrom { get; set; }

    public int? ClaimNotify { get; set; }

    public int? RejectClaimId { get; set; }
}
