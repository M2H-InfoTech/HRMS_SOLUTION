using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class ParameterControlType
{
    public int Id { get; set; }

    public int? ParamControlId { get; set; }

    public string? ParamControlDesc { get; set; }

    public int? IsMultiple { get; set; }
}
