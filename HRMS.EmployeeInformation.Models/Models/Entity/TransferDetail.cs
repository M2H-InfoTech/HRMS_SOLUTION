namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class TransferDetail
{
    public int TransferBatchId { get; set; }

    public string? TransferSequence { get; set; }

    public int? ProxyId { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? FlowStatus { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? IsDirect { get; set; }

    public int? IsUpload { get; set; }

    public DateTime? FinalApprovalDate { get; set; }

    public int? TransferHistoryUpload { get; set; }
}
