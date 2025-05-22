using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class MissingInOut00
{
    public int MissingInOutId { get; set; }

    public int? CompanyId { get; set; }

    public string? RequestSequenceId { get; set; }

    public int? EmployeeId { get; set; }

    public DateTime? AppliedOn { get; set; }

    public DateTime? RequestDate { get; set; }

    public string? ApprovalStatus { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? EntryFrom { get; set; }

    public string? FlowStatus { get; set; }

    public int? ProxyEmployeeId { get; set; }

    public int? ShiftChecked { get; set; }

    public string? CommonReason { get; set; }

    public string? EmpAddress { get; set; }

    public string? EmpPhoneNmbr { get; set; }

    public string? UpdatedFrom { get; set; }

    public int? Reasontypeid { get; set; }

    public int? Daytype { get; set; }

    public int? Halfdaytype { get; set; }

    public string? CancelRemarks { get; set; }

    public int? IsManualPunch { get; set; }

    public int? IsIdle { get; set; }

    public int? NoOfApprovers { get; set; }
}
