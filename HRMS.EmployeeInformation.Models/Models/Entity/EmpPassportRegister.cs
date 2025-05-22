using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class EmpPassportRegister
{
    public int Id { get; set; }

    public int? EmployeeId { get; set; }

    public string? RequestId { get; set; }

    public string? PassportNumber { get; set; }

    public string? Name { get; set; }

    public string? IssuedCountry { get; set; }

    public string? IssuedPlace { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public string? Remark { get; set; }

    public int? ProxyEmployeeId { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? FlowStatus { get; set; }

    public string? RequestType { get; set; }

    public int? Status { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? Reason { get; set; }

    public DateTime? ReturnDate { get; set; }

    public int? DirectStatus { get; set; }

    public string? EntryFrom { get; set; }

    public int? CashBalanceVal { get; set; }

    public string? CashBalanceRemark { get; set; }

    public int? HrPro { get; set; }

    public int? HoldingEmployee { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public string? TentativeRemarks { get; set; }

    public DateTime? FinalApprovalDate { get; set; }
}
