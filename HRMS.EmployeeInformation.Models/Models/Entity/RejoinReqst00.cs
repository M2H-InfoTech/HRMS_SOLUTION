using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class RejoinReqst00
{
    public int RejoinId { get; set; }

    public string? RequestSequenceId { get; set; }

    public int? Employeeid { get; set; }

    public int? LeaveApplicationId { get; set; }

    public DateTime? ReturnDate { get; set; }

    public string? ReturnReason { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? FlowStatus { get; set; }

    public int? ProxyEmpId { get; set; }

    public int? Entryby { get; set; }

    public DateTime? Entrydate { get; set; }

    public string? EntyFrom { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? EmpAddress { get; set; }

    public string? EmpPhoneNmbr { get; set; }

    public int? LateRejoinDays { get; set; }

    public bool? PassportHandover { get; set; }

    public bool? PassportReceived { get; set; }

    public int? DdlpassportReceived { get; set; }

    public bool? IsDirect { get; set; }

    public DateTime? FinalApprovalDate { get; set; }
}
