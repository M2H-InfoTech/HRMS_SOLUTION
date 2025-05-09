using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models;

public partial class LeaveApplication00
{
    public int LeaveApplicationId { get; set; }

    public int? InstId { get; set; }

    public int? EmployeeId { get; set; }

    public int? LeaveId { get; set; }

    public DateTime? LeaveFrom { get; set; }

    public DateTime? LeaveTo { get; set; }

    public string? Reason { get; set; }

    public int? TimeMode { get; set; }

    public decimal? NoOfLeaveDays { get; set; }

    public DateTime? ReturnDate { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? IdproxyLeave { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? SequenceId { get; set; }

    public string? RequestId { get; set; }

    public int? RoleDelegation { get; set; }

    public string? RoleDelegationName { get; set; }

    public string? EntryFrom { get; set; }

    public string? FlowStatus { get; set; }

    public string? Cancelflowstatus { get; set; }

    public string? Cancelstatus { get; set; }

    public int? Firsthalf { get; set; }

    public int? Lasthalf { get; set; }

    public string? Contactaddress { get; set; }

    public string? Contactnumber { get; set; }

    public bool SalaryAdvance { get; set; }

    public int? LastupdatedBy { get; set; }

    public DateTime? LastupdatedDate { get; set; }

    public string? UpdatedFrom { get; set; }

    public DateTime? Selectedfromdate { get; set; }

    public DateTime? Selectedtodate { get; set; }

    public int? IsNoticePeriod { get; set; }

    public int? NoticePeriodValuation { get; set; }

    public string? RejoinStatus { get; set; }

    public int? Passportrequest { get; set; }

    public string? Transactiontype { get; set; }

    public int? Duallaps { get; set; }

    public int? Direct { get; set; }

    public string? CancelRemarks { get; set; }

    public bool? EnableRejoin { get; set; }

    public string? SecondApprovalStatus { get; set; }

    public int? ExtendedRejoin { get; set; }
}
