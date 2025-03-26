using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class AdmRoleMaster
{
    public int InstId { get; set; }

    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public string Active { get; set; } = null!;

    public string RoleCode { get; set; } = null!;

    public string Type { get; set; } = null!;

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UserTypeId { get; set; }

    public int? TransferHravisible { get; set; }
}
