using System;
using System.Collections.Generic;

namespace OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Models;

public partial class HrEmployeeUserRelation
{
    public int InstId { get; set; }

    public int EmpUsrRelatnId { get; set; }

    public int UserId { get; set; }

    public int EmpId { get; set; }

    public int EntryBy { get; set; }

    public DateTime EntryDt { get; set; }
}
