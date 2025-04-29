using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class HrmLeavePartialPayment
{
    public int PartialpaymentId { get; set; }

    public int? SettingsDetailsId { get; set; }

    public int? ExperiancetabId { get; set; }

    public decimal? Daysfrom { get; set; }

    public decimal? Daysto { get; set; }

    public decimal? PayPercentage { get; set; }

    public int? NewjnStatus { get; set; }

    public int? Createdby { get; set; }

    public int? Ondemandpartial { get; set; }

    public int? Initialcount { get; set; }
}
