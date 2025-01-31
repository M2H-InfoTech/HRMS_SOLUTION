using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class EmployeesAssetsAssign
{
    public int AssignId { get; set; }

    public int? EmpId { get; set; }

    public string? AssetGroup { get; set; }

    public string? Asset { get; set; }

    public string? AssetNo { get; set; }

    public DateTime? ReceivedDate { get; set; }

    public string? Status { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? Remarks { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public DateTime? IssueDate { get; set; }

    public string? AssetValue { get; set; }

    public bool? IsRequested { get; set; }

    public int? AssetRequestId { get; set; }

    public bool? IsActive { get; set; }

    public string? AssetModel { get; set; }

    public DateTime? InWarranty { get; set; }

    public DateTime? OutOfWarranty { get; set; }

    public string? Monitor { get; set; }
}
