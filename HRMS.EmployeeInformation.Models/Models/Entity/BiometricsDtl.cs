using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class BiometricsDtl
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public int EmployeeId { get; set; }

    public int DeviceId { get; set; }

    public string? UserId { get; set; }

    public int EntryBy { get; set; }

    public DateTime EntryDt { get; set; }
}
