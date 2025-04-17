using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models.Models.Entity;

public partial class WeekEndMaster
{
    public int WeekEndMasterId { get; set; }

    public string? Name { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }
}
