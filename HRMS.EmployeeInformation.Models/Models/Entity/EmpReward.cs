using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class EmpReward
{
    public int RewardId { get; set; }

    public int? EmpId { get; set; }

    public int? AchievementId { get; set; }

    public int? RewardType { get; set; }

    public string? Reason { get; set; }

    public DateTime? RewardDate { get; set; }

    public decimal? Amount { get; set; }

    public int? Year { get; set; }

    public string? Status { get; set; }
}
