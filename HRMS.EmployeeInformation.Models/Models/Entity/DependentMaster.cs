using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models;

public partial class DependentMaster
{
    public int DependentId { get; set; }

    public string? Code { get; set; }

    public string? Description { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? Self { get; set; }
}
