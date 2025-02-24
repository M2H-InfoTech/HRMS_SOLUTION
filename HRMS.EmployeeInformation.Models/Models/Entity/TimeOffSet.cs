using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class TimeOffSet
{
    public int TimeOffSetId { get; set; }

    public string? Description { get; set; }

    public string? Offset { get; set; }

    public string? AddSign { get; set; }

    public string? OffsetValue { get; set; }
}
