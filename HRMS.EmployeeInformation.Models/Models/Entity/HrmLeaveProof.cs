using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models;

public partial class HrmLeaveProof
{
    public int ProofId { get; set; }

    public string? Proofdescription { get; set; }

    public int? InstantlimitId { get; set; }

    public int? Policyid { get; set; }
}
