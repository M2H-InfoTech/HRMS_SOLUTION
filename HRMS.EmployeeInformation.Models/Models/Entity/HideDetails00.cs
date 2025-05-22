using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class HideDetails00
{
    public int HideDetailsId { get; set; }

    public int? RequestPolicyInstanceLimitId { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? Values { get; set; }
}
