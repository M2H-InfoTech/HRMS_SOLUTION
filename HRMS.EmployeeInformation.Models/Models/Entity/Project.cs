using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models;

public partial class Project
{
    public int Id { get; set; }

    public int? EmployeeId { get; set; }

    public int? ProjectNameid { get; set; }

    public int? ProjectDescriptionid { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Status { get; set; }
}
