using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class HrmValueType
{
    public int Id { get; set; }

    public string? Type { get; set; }

    public int? Value { get; set; }

    public string? Code { get; set; }

    public string? Description { get; set; }

    public int? ReqId { get; set; }
}
