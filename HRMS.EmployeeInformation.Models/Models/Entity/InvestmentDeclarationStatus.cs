using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class InvestmentDeclarationStatus
{
    public int RequestId { get; set; }

    public int? DeclarationId { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? FlowStatus { get; set; }

    public string? RequestStatus { get; set; }

    public int? IsFinalSubmission { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }
}
