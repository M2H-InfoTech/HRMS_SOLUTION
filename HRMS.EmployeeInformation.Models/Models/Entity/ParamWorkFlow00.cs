using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class ParamWorkFlow00
{
    public long ValueId { get; set; }

    public int? TransactionId { get; set; }

    public int? WorkFlowId { get; set; }

    public string? EntityLevel { get; set; }

    public int? LinkLevel { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? SecondLevelWorkflowId { get; set; }

    public string? AdditionalRoleNotif { get; set; }

    public int? RoleNotification { get; set; }
}
