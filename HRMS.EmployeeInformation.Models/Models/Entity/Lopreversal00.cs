using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class Lopreversal00
{
    public long LopreversalId { get; set; }

    public string? RequestCode { get; set; }

    public int? PayrollPeriodId { get; set; }

    public int? PayrollPeriodSubId { get; set; }

    public int? BatchId { get; set; }

    public string? Status { get; set; }

    public string? FlowStatus { get; set; }

    public int? EntryBy { get; set; }

    public int? RefEmpId { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsBulk { get; set; }

    public bool? IsDirectPosted { get; set; }

    public bool? IsOld { get; set; }

    public DateTime? FinalApprovalDate { get; set; }
}
