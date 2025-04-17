using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models.Models.Entity;

public partial class SpecialcomponentsBatchSlab
{
    public int SpecialcomponentsBatchSlab1 { get; set; }

    public int? BatchId { get; set; }

    public string? BatchSlabDescripttion { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? Active { get; set; }
}
