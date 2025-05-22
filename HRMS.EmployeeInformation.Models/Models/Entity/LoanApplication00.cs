using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class LoanApplication00
{
    public long AssignLoanId { get; set; }

    public string? LoanRequestId { get; set; }

    public int? CreatedEmpId { get; set; }

    public int? LoanTypeId { get; set; }

    public double? LoanAmt { get; set; }

    public double? LoanTenureYear { get; set; }

    public double? LoanTenureMonth { get; set; }

    public double? Emiamt { get; set; }

    public double? CalcIntAmt { get; set; }

    public double? CalProcFee { get; set; }

    public double? TotalLoanAmt { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? FlowStatus { get; set; }

    public bool? Active { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? Payrollprocessingtype { get; set; }

    public DateTime? Sanctiondate { get; set; }

    public DateTime? Expectedsanctiondate { get; set; }

    public DateTime? Releasedate { get; set; }

    public int? Recoverypayroll { get; set; }

    public string? Releasestatus { get; set; }

    public int? ProcesspayRollId { get; set; }

    public int? ProcessPayRoll01Id { get; set; }

    public int? PayRollPeriodSubId { get; set; }

    public int? LoanschemeId { get; set; }

    public double? TotalTenureMonths { get; set; }

    public int? IsUpload { get; set; }

    public int? ReleasedpayPending { get; set; }

    public int? LoanSettlementType { get; set; }

    public double? ProcessingFee { get; set; }

    public double? SminAmt { get; set; }

    public double? SmaxAmt { get; set; }

    public double? SinterestRate { get; set; }

    public int? SminYear { get; set; }

    public int? SmaxYear { get; set; }

    public int? SminMonth { get; set; }

    public int? SmaxMonth { get; set; }

    public int? StotalMinMonths { get; set; }

    public int? StotalMaxMonths { get; set; }

    public int? SamountonSalary { get; set; }

    public bool? SisInterest { get; set; }

    public string? SloanSchemeName { get; set; }

    public string? Reason { get; set; }
}
