using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class EntityAccessRights02
{
    public long SubTrxId { get; set; }

    public int? RoleId { get; set; }

    public int? LinkLevel { get; set; }

    public string? SubCategoryList { get; set; }

    public string? LinkId { get; set; }

    public int? Hierarchy { get; set; }
}
