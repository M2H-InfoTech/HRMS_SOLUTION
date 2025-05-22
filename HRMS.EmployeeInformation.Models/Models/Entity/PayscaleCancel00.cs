using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class PayscaleCancel00
{
    public long PayCancelId { get; set; }

    public int? BatchId { get; set; }

    public string? PayCancelCode { get; set; }

    public string? Remarks { get; set; }

    public string? ApptovalStatus { get; set; }

    public string? RejectStatus { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? FlowStatus { get; set; }

    public string? RejectReason { get; set; }
}
