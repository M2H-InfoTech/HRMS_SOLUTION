using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class PayscaleRequest02
{
    public long PayRequestId02 { get; set; }

    public long? PayRequestId01 { get; set; }

    public long? PayRequestId { get; set; }

    public int? PayType { get; set; }

    public int? PayComponentId { get; set; }

    public double? Amount { get; set; }
}
