using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class HrmsDocTypeMaster
{
    public long DocTypeId { get; set; }

    public string? DocType { get; set; }

    public string? Code { get; set; }
}
