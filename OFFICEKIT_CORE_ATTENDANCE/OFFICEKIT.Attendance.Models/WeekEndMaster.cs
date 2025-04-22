using System;
using System.Collections.Generic;

namespace OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Models;

public partial class WeekEndMaster
{
    public int WeekEndMasterId { get; set; }

    public string? Name { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }
}
