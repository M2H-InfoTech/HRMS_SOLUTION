using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class HrmsCommonField01
{
    public long CommonFieldId { get; set; }

    public int? ComMasId { get; set; }

    public int? ComFieldId { get; set; }

    public string? CommonVal { get; set; }
}
