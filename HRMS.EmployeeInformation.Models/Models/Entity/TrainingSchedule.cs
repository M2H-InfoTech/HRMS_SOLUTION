using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class TrainingSchedule
{
    public int TrSchd { get; set; }

    public int TrMasterId { get; set; }

    public int? EmpId { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? SelectStatus { get; set; }

    public DateTime? AttDate { get; set; }

    public string? Status { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
