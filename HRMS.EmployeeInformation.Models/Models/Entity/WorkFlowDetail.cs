using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class WorkFlowDetail
{
    public int WorkFlowId { get; set; }

    public int? InstId { get; set; }

    public string? Description { get; set; }

    public bool? HierarchyType { get; set; }

    public string? FinalRule { get; set; }

    public string? FinalRuleName { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? TransactionDate { get; set; }

    public bool? IsActive { get; set; }

    public int? ForwardNext { get; set; }

    public int? ReqNotifForProxy { get; set; }

    public int? OldType { get; set; }

    public bool? WistleBlower { get; set; }
}
