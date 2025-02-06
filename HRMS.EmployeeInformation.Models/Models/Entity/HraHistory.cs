using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class HraHistory
{
    public int Id { get; set; }

    public int? EmployeeId { get; set; }

    public bool? IsHra { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public string? Remarks { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? Entryby { get; set; }

    public int? Initial { get; set; }
}
