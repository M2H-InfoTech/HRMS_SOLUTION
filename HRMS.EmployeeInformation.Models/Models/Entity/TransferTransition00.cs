namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class TransferTransition00
{
    public int TransferTransId { get; set; }

    public int? TransferBatchId { get; set; }

    public int? TransferId { get; set; }

    public int? EntityOrder { get; set; }

    public int? ActionId { get; set; }

    public int? EmployeeId { get; set; }

    public int? OldEntityId { get; set; }

    public string? OldEntityDescription { get; set; }

    public int? NewEntityId { get; set; }

    public string? NewEntityDescription { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public string? BatchApprovalStatus { get; set; }

    public string? EmpApprovalStatus { get; set; }

    public int? PreviousEmpId { get; set; }

    public int? CurrentEmpId { get; set; }

    public int? RevokeStatus { get; set; }

    public int? RevokedBy { get; set; }

    public DateTime? RevokedDate { get; set; }
}
