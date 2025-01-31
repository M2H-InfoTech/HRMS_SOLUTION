using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class TrainingMaster
{
    public long TrMasterId { get; set; }

    public bool? IsInside { get; set; }

    public string TrCode { get; set; } = null!;

    public string TrName { get; set; } = null!;

    public int? Capacity { get; set; }

    public string? TargetPeople { get; set; }

    public string? TrainingLocation { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public string TrainerName { get; set; } = null!;

    public int TrainingCost { get; set; }

    public string Description { get; set; } = null!;

    public string? FileUrl { get; set; }

    public int EntryBy { get; set; }

    public DateTime EntryDate { get; set; }

    public string Active { get; set; } = null!;

    public bool? IsSurvey { get; set; }

    public string? Survey { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
