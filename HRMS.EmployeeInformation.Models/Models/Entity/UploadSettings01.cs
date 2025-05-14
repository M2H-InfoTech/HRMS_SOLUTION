using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class UploadSettings01
{
    public int SettingsTypeId { get; set; }

    public int? SettingsId { get; set; }

    public string? ExcellColumn { get; set; }

    public string? TableColumn { get; set; }
}
