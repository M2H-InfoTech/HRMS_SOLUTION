using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class ProcessPayRoll00
{
    public long ProcessPayRollId { get; set; }

    public string? BatchCode { get; set; }

    public int? PayRollPeriodId { get; set; }

    public int? PayRollPeriodSubId { get; set; }

    public int? BatchId { get; set; }

    public string? BatchDescription { get; set; }

    public int? EmployeeCount { get; set; }

    public double? TotalEarnings { get; set; }

    public double? TotalDeduction { get; set; }

    public double? TotalNetSalary { get; set; }

    public string? Status { get; set; }

    public string? RejectReason { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? FlowStatus { get; set; }

    public string? PayRollType { get; set; }

    public int? ProcessType { get; set; }

    public double? TotalTakeHomeEarnings { get; set; }

    public double? TotalTakeHomeDeductions { get; set; }

    public double? TotalTakeHomePay { get; set; }

    public double? FinalsettlementEarnings { get; set; }

    public double? FinalsettlementDeduction { get; set; }

    public double? Finalsettlementnetamount { get; set; }

    public double? FinalsettlementgrossEarnings { get; set; }

    public double? FinalsettlementgrossDeduction { get; set; }

    public double? Finalsettlementgrossamount { get; set; }

    public int? LeaveProcessId { get; set; }

    public double? UpdatedEarningsAmount { get; set; }

    public double? UpdatedDeductionAmount { get; set; }

    public double? UpdatedTotalAmount { get; set; }

    public double? UpdatedEarningsHometake { get; set; }

    public double? UpdatedDeductionHometake { get; set; }

    public double? UpdatedTotalHometake { get; set; }

    public double? TotalAmountforWorkingdayEarn00 { get; set; }

    public double? TotalAmountforWorkingdayDed00 { get; set; }

    public double? TotalLopamount { get; set; }

    public double? Percentage { get; set; }

    public long? OldProcesspayrollId { get; set; }

    public int? LeavesalaryPayrollperiodsubid { get; set; }

    public string? Remark { get; set; }

    public DateTime? FinalApprovalDate { get; set; }

    public int? PayrollApprovalType { get; set; }
}
