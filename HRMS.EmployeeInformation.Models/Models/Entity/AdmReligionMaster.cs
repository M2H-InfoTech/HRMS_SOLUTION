using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models;

public partial class AdmReligionMaster
{
    public int InstId { get; set; }

    public int ReligionId { get; set; }

    public string ReligionName { get; set; } = null!;

    public string? ReligionLocName { get; set; }

    public int EntryBy { get; set; }

    public DateTime EntryDate { get; set; }

    public int ModiBy { get; set; }

    public DateTime ModiDate { get; set; }
}
