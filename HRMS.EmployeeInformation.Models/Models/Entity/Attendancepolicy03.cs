using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class Attendancepolicy03
{
    public int Attendancepolicy03id { get; set; }

    public int? AttendancePolicyId { get; set; }

    public int? ShortageId { get; set; }

    public double? PercentageFrom { get; set; }

    public double? PercentageTo { get; set; }
}
