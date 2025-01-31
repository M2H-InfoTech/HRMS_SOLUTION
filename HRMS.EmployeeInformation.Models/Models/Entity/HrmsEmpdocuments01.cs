using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class HrmsEmpdocuments01
{
    public int DocFieldId { get; set; }

    public int? DetailId { get; set; }

    public int? DocFields { get; set; }

    public string? DocValues { get; set; }

    public int? DocNewId { get; set; }
}
