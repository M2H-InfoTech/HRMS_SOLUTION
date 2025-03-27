using System;
using System.Collections.Generic;

namespace OFFICEKITCORELEAVE.OfficeKitHR.Leave.MODELS;

public partial class HrmLeaveBasicsettingsDetailsHistory
{
    public long SettingsHistoryId { get; set; }

    public int? SettingsId { get; set; }

    public int? EmployeeId { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? Ipaddress { get; set; }
}
