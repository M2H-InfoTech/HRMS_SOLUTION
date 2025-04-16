using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class HrmsEmpstatutory00
{
    public int SatId { get; set; }

    public int? EmpId { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? Status { get; set; }
}
