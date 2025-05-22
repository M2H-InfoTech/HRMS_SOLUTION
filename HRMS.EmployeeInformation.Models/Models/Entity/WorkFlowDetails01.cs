using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class WorkFlowDetails01
{
    public int WorkFlow01Id { get; set; }

    public int? WorkFlowId { get; set; }

    public int? Rule { get; set; }

    public int? FinalRule { get; set; }

    public int? ForwardNext { get; set; }

    public int? Rules { get; set; }

    public int? RuleOrder { get; set; }

    public int? NoOfApprovers { get; set; }

    public int? SentNotifToPrevApprovers { get; set; }

    public int? SkipAppNotDefinedEmployee { get; set; }

    public int? ParemeterId { get; set; }
}
