using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class SpecialAccessRight
{
    public long SpecialId { get; set; }

    public int? RoleId { get; set; }

    public int? LinkLevel { get; set; }
}
