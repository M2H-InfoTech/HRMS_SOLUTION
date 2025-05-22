using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class RequestPolicyInstanceLimit
{
    public int RequestPolicyInstanceLimitId { get; set; }

    public int? RequestPolicyMasterId { get; set; }

    public int? InstId { get; set; }

    public int? TypeId { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? MonthlyLimit { get; set; }

    public int? MaximumLimit { get; set; }

    public double? MaximamHours { get; set; }

    public double? MinimumHours { get; set; }

    public int? IscompoNeeded { get; set; }

    public int? Predateddayslimit { get; set; }

    public int? ForwardNext { get; set; }

    public int? IsShiftTime { get; set; }

    public int? IsBlockRequest { get; set; }

    public double? MonthlyHours { get; set; }

    public int? IsAllowPredatedApplication { get; set; }

    public int? PolicyType { get; set; }

    public int? PolicyTypeProxy { get; set; }

    public int? IsAllowPredatedApplicationProxy { get; set; }

    public int? PredateddayslimitProxy { get; set; }

    public int? IsAllowFutureDate { get; set; }

    public int? IsAllowFutureDateProxy { get; set; }

    public int? Minimumdayslimit { get; set; }

    public int? MinimumdayslimitProxy { get; set; }

    public int? MonthlyLimitSelf { get; set; }

    public int? MonthlyLimitProxy { get; set; }

    public int? EnableReason { get; set; }

    public int? Isholiday { get; set; }

    public int? DisableTime { get; set; }

    public int? BlockMultipleRequest { get; set; }

    public int? HideMnthlylimit { get; set; }

    public int? EnableFullDay { get; set; }

    public int? Disabledefaulttime { get; set; }

    public int? Approvremark { get; set; }

    public int? Rejectremark { get; set; }

    public string? RestrictedRequest { get; set; }

    public string? Halfdayreqcombination { get; set; }

    public int? EnableIdle { get; set; }

    public int? ConsiderWorkingDays { get; set; }

    public int? EnablePriorRequestSelf { get; set; }

    public int? PriorRequestDaysSelf { get; set; }

    public int? EnablePriorRequestProxy { get; set; }

    public double? PriorRequestDaysProxy { get; set; }

    public int? BlockAfterMonthlyLimit { get; set; }

    public int? BlockIfNoPunch { get; set; }

    public int? MultipleRequestOnaDay { get; set; }

    public int? SeperateLateinEarlyOutLimit { get; set; }

    public int? MonthlyLimitLatein { get; set; }

    public int? MonthlyLimitEarlyOut { get; set; }

    public int? BlockDaysTypeSelf { get; set; }

    public double? RequestBlockDaysSelf { get; set; }

    public int? BlockDaysTypeProxy { get; set; }

    public double? RequestBlockDaysProxy { get; set; }

    public double? InstanceLimit { get; set; }

    public int? AllowExtraLeaveProxy { get; set; }

    public DateTime? RequestBlockDaysSelfFrom { get; set; }

    public DateTime? RequestBlockDaysSelfEnd { get; set; }

    public DateTime? RequestBlockDaysSelfFromProxy { get; set; }

    public DateTime? RequestBlockDaysSelfEndProxy { get; set; }

    public double? Regularizationlimit { get; set; }

    public double? RegularizationLimitOut { get; set; }

    public int? DisableMonthlyLimitInProxy { get; set; }

    public int? ShowReqBasedonTime { get; set; }
}
