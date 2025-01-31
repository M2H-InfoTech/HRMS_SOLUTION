using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class HrEmpStatusSetting
{
    public int Id { get; set; }

    public int StatusId { get; set; }

    public string? StatusDesc { get; set; }

    public string? Status { get; set; }

    public string? Active { get; set; }

    public int? Entryby { get; set; }

    public DateTime? EntryDate { get; set; }

    public bool? IsResignation { get; set; }
}
