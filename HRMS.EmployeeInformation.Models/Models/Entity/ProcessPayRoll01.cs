using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class ProcessPayRoll01
{
    public long ProcessPayRoll01Id { get; set; }

    public long? ProcessPayRollId { get; set; }

    public int? PayRollPeriodId { get; set; }

    public int? PayRollPeriodSubId { get; set; }

    public int? BatchId { get; set; }

    public double? TotalEarnings { get; set; }

    public double? TotalDeduction { get; set; }

    public double? TotalNetSalary { get; set; }

    public double? FixedSalary { get; set; }

    public double? Lop { get; set; }

    public double? Lopamount { get; set; }

    public int? EmployeeId { get; set; }

    public int? CurrencyId { get; set; }

    public string? Status { get; set; }

    public string? RejectReason { get; set; }

    public int? RejectBy { get; set; }

    public DateTime? RejectDate { get; set; }

    public double? TotalTakeHomeEarnings { get; set; }

    public double? TotalTakeHomeDeductions { get; set; }

    public double? TotalTakeHomePay { get; set; }

    public double? ProcessDays { get; set; }

    public double? PrevLop { get; set; }

    public double? PrevLopAmount { get; set; }

    public double? CurrentLop { get; set; }

    public double? CurrentLopAmount { get; set; }

    public bool? IsRevision { get; set; }

    public DateTime? RevisionDate { get; set; }

    public double? FinalsettlementEarnings { get; set; }

    public double? FinalsettlementDeduction { get; set; }

    public double? Finalsettlementnetamount { get; set; }

    public string? FinalsettlementRemark { get; set; }

    public double? FinalsettlementgrossEarnings { get; set; }

    public double? FinalsettlementgrossDeduction { get; set; }

    public double? Finalsettlementgrossamount { get; set; }

    public int? Leaveid { get; set; }

    public double? Daysinpayrollperiod { get; set; }

    public double? Salarypayabledays { get; set; }

    public double? Leavedays { get; set; }

    public DateTime? Leavefromdate { get; set; }

    public DateTime? Leavetodate { get; set; }

    public double? AmountwithoutRisk { get; set; }

    public double? TotalMasterAmountEarnings { get; set; }

    public double? TotalMasterAmountDed { get; set; }

    public double? TotalAmountforWorkingdayEarn { get; set; }

    public double? TotalAmountforWorkingdayDed { get; set; }

    public double? UpdatedEarningsAmount01 { get; set; }

    public double? UpdatedDeductionAmount01 { get; set; }

    public double? UpdatedTotalAmount01 { get; set; }

    public double? UpdatedEarningsHometake01 { get; set; }

    public double? UpdatedDeductionHometake01 { get; set; }

    public double? UpdatedTotalHometake01 { get; set; }

    public double? Updatedprevlpcount { get; set; }

    public double? Updatedcurrentlpcount { get; set; }

    public double? Updatedlopcount { get; set; }

    public double? UpdatedprevlpAmount { get; set; }

    public double? UpdatedcurrentlpAmount { get; set; }

    public double? UpdatedlopAmount { get; set; }

    public double? LeaveencashmentDays { get; set; }

    public double? Esibasic { get; set; }

    public int? Companyworkingdays { get; set; }

    public long? Old01id { get; set; }

    public long? OldProcesspayrollId { get; set; }

    public double? EmployeeNetsalary { get; set; }

    public int? FsSheetGenerated { get; set; }

    public int? BackdatedArrear { get; set; }

    public int? Newjoinee { get; set; }

    public int? MultiPayscale { get; set; }

    public int? LatestPayscaleid { get; set; }

    public double? Holidayweekend { get; set; }

    public double? HolidayWeekendOtdays { get; set; }

    public double? DailywageAmnt { get; set; }

    public double? Othrs { get; set; }

    public double? EncashedComboDays { get; set; }

    public double? WorkedHrs { get; set; }

    public double? ShortageHrs { get; set; }
}
