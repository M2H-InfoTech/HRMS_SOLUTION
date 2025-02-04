using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class ParamWorkFlow02
{
    public long ValueId { get; set; }

    public int? LinkEmpId { get; set; }

    public int? TransactionId { get; set; }

    public int? WorkFlowId { get; set; }

    public int? LinkLevel { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
