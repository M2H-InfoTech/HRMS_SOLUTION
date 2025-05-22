using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class SuspensionTb
{
    public int SuspensionId { get; set; }

    public string? Employee { get; set; }

    public DateOnly? FromDate { get; set; }

    public DateOnly? ToDate { get; set; }

    public string? Remark { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? SuspensionCode { get; set; }

    public int? UpdatedBy { get; set; }

    public DateOnly? UpdatedDate { get; set; }

    public string? EntryByName { get; set; }

    public string? Status { get; set; }

    public string? ApprovalStatus { get; set; }
}
