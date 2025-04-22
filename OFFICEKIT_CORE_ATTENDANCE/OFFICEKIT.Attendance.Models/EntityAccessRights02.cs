using System;
using System.Collections.Generic;

namespace OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Models;

public partial class EntityAccessRights02
{
    public long SubTrxId { get; set; }

    public int? RoleId { get; set; }

    public int? LinkLevel { get; set; }

    public string? SubCategoryList { get; set; }

    public string? LinkId { get; set; }

    public int? Hierarchy { get; set; }
}
