using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class SuspensionNew00
{
    public long SuspensionId { get; set; }

    public string? RequestCode { get; set; }

    public int? BatchType { get; set; }

    public string? Remark { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? FlowStatus { get; set; }

    public bool? IsDirect { get; set; }

    public int? RefEmpId { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsUpload { get; set; }
}
