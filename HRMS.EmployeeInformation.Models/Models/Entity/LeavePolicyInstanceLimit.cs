using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;
public partial class LeavePolicyInstanceLimit
{
    public int LeavePolicyInstanceLimitId { get; set; }

    public int? LeavePolicyMasterId { get; set; }

    public int? InstId { get; set; }

    public int? LeaveId { get; set; }

    public double? MaximamLimit { get; set; }

    public double? MinimumLimit { get; set; }

    public bool? IsHolidayIncluded { get; set; }

    public bool? IsWeekendIncluded { get; set; }

    public decimal? NoOfDayIncludeHoliday { get; set; }

    public decimal? NoOfDayIncludeWeekEnd { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public decimal? Daysbtwnleaves { get; set; }

    public decimal? Salaryadvancedays { get; set; }

    public decimal? Roledeligationdays { get; set; }

    public decimal? Attachmentdays { get; set; }

    public decimal? ProbationMl { get; set; }

    public decimal? NewjoinMl { get; set; }

    public decimal? OtherMl { get; set; }

    public int? Halfday { get; set; }

    public int? PredatedApplication { get; set; }

    public decimal? Daysbtwndifferentleave { get; set; }

    public decimal? Daysleaveclubbing { get; set; }

    public decimal? Predateddayslimit { get; set; }

    public int? Returndate { get; set; }

    public int? Autotravelapprove { get; set; }

    public int? Leaveinclude { get; set; }

    public int? Contactdetails { get; set; }

    public int? Leavereason { get; set; }

    public int? Approvremark { get; set; }

    public int? Nobalance { get; set; }

    public int? Applyafterallleave { get; set; }

    public string? Applyafterleaveids { get; set; }

    public int? Showinapplicationonly { get; set; }

    public int? Rejectremark { get; set; }

    public int? Predatedapplicationproxy { get; set; }

    public double? Predateddayslimitproxy { get; set; }

    public int? PredatedapplicationAttendance { get; set; }

    public double? PredatedapplicationAttendanceDays { get; set; }

    public int? FutureleaveApplication { get; set; }

    public double? FutureleaveApplicationDays { get; set; }
}
