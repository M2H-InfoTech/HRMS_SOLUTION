using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models;

public partial class TransactionMaster
{
    public int TransactionId { get; set; }

    public int? InstId { get; set; }

    public string? TransactionType { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? OrderId { get; set; }

    public int? SequenceNeed { get; set; }

    public int? WorkFlowNeed { get; set; }

    public int? TrxParam { get; set; }

    public int? IsRequest { get; set; }

    public int? Transactionbit { get; set; }

    public bool? WorkflowMonitorbit { get; set; }
}
