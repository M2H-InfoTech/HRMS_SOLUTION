using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class HrmsEmpdocumentsApproved00
{
    public int DocApprovedId { get; set; }

    public int? DetailId { get; set; }

    public int? DocId { get; set; }

    public int? EmpId { get; set; }

    public string? RequestId { get; set; }

    public string? TransactionType { get; set; }

    public string? Status { get; set; }

    public int? ProxyId { get; set; }

    public string? FlowStatus { get; set; }

    public DateTime? DateFrom { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public DateTime? FinalApprovalDate { get; set; }
}
