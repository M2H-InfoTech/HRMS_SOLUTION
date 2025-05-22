using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class SalaryAdvanceDetail
{
    public int SalaryAdvanceId { get; set; }

    public string? Batchcode { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? EmpId { get; set; }

    public int? ProxyEmpId { get; set; }

    public int? Amount { get; set; }

    public int? EmiAmount { get; set; }

    public int? RecoveryMode { get; set; }

    public int? PayMode { get; set; }

    public int? RemainingBalance { get; set; }

    public int? LastDeductAmount { get; set; }

    public string? PayMonth { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? FlowStatus { get; set; }

    public string? Status { get; set; }

    public int? ProcesspayRollId { get; set; }

    public int? ProcessPayRoll01Id { get; set; }

    public int? PayRollPeriodSubId { get; set; }

    public string? PayrollStatus { get; set; }

    public string? PayRollPeriodListId { get; set; }

    public int? IntermediateBit { get; set; }

    public int? PayperiodIdSalaryAdvance { get; set; }

    public string? PayRollPeriodYearSalary { get; set; }
}
