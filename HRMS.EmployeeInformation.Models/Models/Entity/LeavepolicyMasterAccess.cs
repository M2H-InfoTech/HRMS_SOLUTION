using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class LeavepolicyMasterAccess
{
    public long LeaveAccessId { get; set; }

    public int? EmployeeId { get; set; }

    public int? PolicyId { get; set; }

    public int? IsCompanyLevel { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? Fromdate { get; set; }

    public DateTime? Validto { get; set; }
}
