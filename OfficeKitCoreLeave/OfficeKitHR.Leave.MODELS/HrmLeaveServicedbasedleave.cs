using System;
using System.Collections.Generic;

namespace OFFICEKITCORELEAVE.OfficeKitHR.Leave.MODELS;

public partial class HrmLeaveServicedbasedleave
{
    public int IdServiceLeave { get; set; }

    public int? LeaveEntitlementId { get; set; }

    public double? FromYear { get; set; }

    public double? ToYear { get; set; }

    public decimal? LeaveCount { get; set; }

    public int? ExperiancebasedGrant { get; set; }

    public decimal? Experiancebasedrollover { get; set; }

    public int? Checkcase { get; set; }

    public int? ExperiancebasedVacation { get; set; }
}
