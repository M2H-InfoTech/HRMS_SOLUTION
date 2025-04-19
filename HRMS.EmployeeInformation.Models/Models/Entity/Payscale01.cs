using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class Payscale01
{
    public long PayScale01Id { get; set; }

    public long? PayRequestId01 { get; set; }

    public long? PayRequestId { get; set; }

    public int? PayType { get; set; }

    public int? PayComponentId { get; set; }

    public double? Amount { get; set; }
}
