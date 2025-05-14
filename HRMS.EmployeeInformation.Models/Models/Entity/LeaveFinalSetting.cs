using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models;

public partial class LeaveFinalSetting
{
    public long? Id { get; set; }

    public int? EmployeeId { get; set; }

    public int? LeaveMaster { get; set; }

    public int? SettingsId { get; set; }
}
