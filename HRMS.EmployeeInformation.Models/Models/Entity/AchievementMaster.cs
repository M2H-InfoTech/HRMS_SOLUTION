using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class AchievementMaster
{
    public int AchievementId { get; set; }

    public string? Code { get; set; }

    public string? Description { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
