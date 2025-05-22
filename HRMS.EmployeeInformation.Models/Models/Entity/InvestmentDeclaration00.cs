using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class InvestmentDeclaration00
{
    public int Declaration00Id { get; set; }

    public string? SequenceCode { get; set; }

    public int? EmployeeId { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? FlowStatus { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? TaxRegimeType { get; set; }

    public int? FinancialYear { get; set; }

    public double? AnnualIncome { get; set; }

    public double? TotalEarning { get; set; }

    public double? TotalDeduction { get; set; }

    public string? FinalSubmissionStatus { get; set; }

    public string? InitiatedBy { get; set; }

    public int? ResubmitStatus { get; set; }

    public int? ApprovalUpdateStatus { get; set; }
}
