using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class AutoCalAttendance00
{
    public long AutoCalAttendanceId { get; set; }

    public DateOnly? FromDate { get; set; }

    public DateOnly? ToDate { get; set; }

    public string? EmployeeId { get; set; }

    public string? Status { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? RequestFrom { get; set; }

    public int? RequestFromId { get; set; }

    public string? RequestId { get; set; }
}
