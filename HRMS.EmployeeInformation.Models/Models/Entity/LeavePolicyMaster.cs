using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class LeavePolicyMaster
{
    public int LeavePolicyMasterId { get; set; }

    public int? InstId { get; set; }

    public string? PolicyName { get; set; }

    public int? Blockmultiunapprovedleave { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? Entrydate { get; set; }

    public int? EmpId { get; set; }
}
