using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class Odrequest00
{
    public int OdRequestId { get; set; }

    public int? CompanyId { get; set; }

    public string? RequestId { get; set; }

    public int? EmployeeId { get; set; }

    public string? PersonsMet { get; set; }

    public string? Organization { get; set; }

    public string? Purpose { get; set; }

    public string? ContactDetails { get; set; }

    public string? Comments { get; set; }

    public DateTime? AppliedOn { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public int? OdType { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? PlaceVisited { get; set; }

    public int? ProxyEmployeeId { get; set; }

    public string? EntryFrom { get; set; }

    public string? FlowStatus { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? DayCategoryId { get; set; }

    public decimal? NoDays { get; set; }

    public int? FirstHalf { get; set; }

    public int? SecondHalf { get; set; }

    public string? ContactAddress { get; set; }

    public string? ContactNumber { get; set; }

    public string? UpdatedFrom { get; set; }

    public string? CancelRemarks { get; set; }

    public int? ForOdcancelId { get; set; }

    public string? CancelStatus { get; set; }

    public string? CancelFlowStatus { get; set; }

    public int? NoOfApprovers { get; set; }

    public int? ReqBasedOnHours { get; set; }

    public double? StartTime { get; set; }

    public double? EndTime { get; set; }

    public string? TotalTime { get; set; }
}
