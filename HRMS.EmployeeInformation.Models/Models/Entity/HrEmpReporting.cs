using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class HrEmpReporting
{
    public int? InstId { get; set; }

    public int ReportId { get; set; }

    public int EmpId { get; set; }

    public int? ReprotToWhome { get; set; }

    public DateTime? EffectDate { get; set; }

    public string? Active { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? OldReportingPerson { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
