using System;
using System.Collections.Generic;

namespace OFFICEKITCORELEAVE.OfficeKitHR.Leave.MODELS;

public partial class HrmLeaveEntitlementReg
{
    public int LeaveentitlementregId { get; set; }

    public int? LeaveEntitlementId { get; set; }

    public int? LeaveCondition { get; set; }

    public int? Year { get; set; }

    public int? Month { get; set; }

    public decimal? Count { get; set; }

    public int? Newjoin { get; set; }
}
