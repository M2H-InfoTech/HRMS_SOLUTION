using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class LateInEarlyOut01
{
    public int LateEarly01Id { get; set; }

    public int? LateEarlyId { get; set; }

    public int? Type { get; set; }

    public string? Reason { get; set; }

    public decimal? LateHours { get; set; }
}
