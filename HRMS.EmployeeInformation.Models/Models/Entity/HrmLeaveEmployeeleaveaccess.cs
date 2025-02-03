using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class HrmLeaveEmployeeleaveaccess
{
    public long IdEmployeeLeaveAccess { get; set; }

    public int? EmployeeId { get; set; }

    public int? LeaveMaster { get; set; }

    public int? BasicSettingsId { get; set; }

    public int? IsCompanyLevel { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? Status { get; set; }

    public int? AssignPeriod { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ValidTo { get; set; }
}
