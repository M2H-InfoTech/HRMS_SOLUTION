using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class EmployeeCurrentStatus
{
    public int StatusId { get; set; }

    public string? StatusDesc { get; set; }

    public int? Status { get; set; }

    public string? Active { get; set; }

    public int? SortOrder { get; set; }
}
