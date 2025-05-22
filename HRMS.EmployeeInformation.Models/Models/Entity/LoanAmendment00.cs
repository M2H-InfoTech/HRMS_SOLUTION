using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class LoanAmendment00
{
    public int LoanAmendId { get; set; }

    public string? SequenceId { get; set; }

    public int? AssignedLoanId { get; set; }

    public int? LoanChild01Id { get; set; }

    public int? RequestEmpId { get; set; }

    public string? AmendType { get; set; }

    public string? ApprovalStatus { get; set; }

    public DateTime? ApprovalDate { get; set; }

    public string? FlowStatus { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
