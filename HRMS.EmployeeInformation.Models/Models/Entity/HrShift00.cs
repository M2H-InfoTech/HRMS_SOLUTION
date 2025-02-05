using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class HrShift00
{
    public int ShiftId { get; set; }

    public int? CompanyId { get; set; }

    public string? ShiftCode { get; set; }

    public string? ShiftName { get; set; }

    public string? ShiftType { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? EndwithNextDay { get; set; }

    public string? IsUpload { get; set; }

    public double? ToleranceForward { get; set; }

    public double? ToleranceBackward { get; set; }

    public int? LateInMinutes { get; set; }

    public int? EarlyOutMinutes { get; set; }

    public int? CheckDirection { get; set; }
}
