using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class ParamRole00
{
    public long ValueId { get; set; }

    public int? ParameterId { get; set; }

    public int? EmpId { get; set; }

    public string? EntityLevel { get; set; }

    public int? LinkLevel { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
