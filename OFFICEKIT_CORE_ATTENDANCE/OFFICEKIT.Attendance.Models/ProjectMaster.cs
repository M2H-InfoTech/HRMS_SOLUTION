using System;
using System.Collections.Generic;

namespace OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Models;

public partial class ProjectMaster
{
    public int Id { get; set; }

    public string? ProjectCode { get; set; }

    public string? ProjectName { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? EntryBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Status { get; set; }
}
