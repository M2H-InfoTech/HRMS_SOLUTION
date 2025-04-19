using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Resource;

public partial class Geolocation00
{
    public int GeoBatchId { get; set; }

    public string? GeoBatchDescription { get; set; }

    public int? Status { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }
}
