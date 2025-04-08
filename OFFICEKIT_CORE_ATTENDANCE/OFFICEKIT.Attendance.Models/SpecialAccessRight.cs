using System;
using System.Collections.Generic;

namespace OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Models;

public partial class SpecialAccessRight
{
    public long SpecialId { get; set; }

    public int? RoleId { get; set; }

    public int? LinkLevel { get; set; }
}
