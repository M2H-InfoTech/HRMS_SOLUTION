using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class RoleDelegation00
{
    public int RoleDelegationId { get; set; }

    public string? CompanyId { get; set; }

    public string? SequenceId { get; set; }

    public int? EmpId { get; set; }

    public int? AssignedEmpId { get; set; }

    public string? Status { get; set; }

    public string? ApprovalStatus { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public int? NumOfDays { get; set; }

    public string? Revoke { get; set; }

    public DateTime? RevokeDate { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? EntryBy { get; set; }

    public string? Remarks { get; set; }

    public int? ProxyEmpId { get; set; }

    public int? Acceptstatus { get; set; }

    public string? Transactionids { get; set; }

    public long? LeaveapplicationId { get; set; }
}
