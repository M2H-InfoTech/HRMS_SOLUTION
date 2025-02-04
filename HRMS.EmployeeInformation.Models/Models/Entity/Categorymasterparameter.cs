using System;
using System.Collections.Generic;

namespace  HRMS.EmployeeInformation.Models;

public partial class Categorymasterparameter
{
    public int ParameterId { get; set; }

    public int? InstId { get; set; }

    public int? EntityId { get; set; }

    public string? ParamDescription { get; set; }

    public string? DataType { get; set; }

    public bool? IsRole { get; set; }

    public bool? IsWorkFlow { get; set; }

    public int? RoleValue { get; set; }

    public bool? LinkToEmp { get; set; }

    public bool? LinkToEntity { get; set; }

    public int? Reporting { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ShowRoleInEmployeeTab { get; set; }
}
