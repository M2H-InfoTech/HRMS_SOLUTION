using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class MasterBranchDetail
{
    public int InstId { get; set; }

    public int BranchId { get; set; }

    public string? BranchCode { get; set; }

    public int? SubBranchId { get; set; }

    public string? BranchName { get; set; }

    public string? Location { get; set; }

    public string? Address { get; set; }

    public DateTime? DateOfBranchOpen { get; set; }

    public int? Currency { get; set; }

    public string? Latitude { get; set; }

    public string? Longitude { get; set; }

    public string? Circumference { get; set; }

    public string? Email { get; set; }

    public string? ExtensionCode { get; set; }

    public string? Phone { get; set; }

    public string? Logo { get; set; }

    public int? TimeOffSetId { get; set; }
}
