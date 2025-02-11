namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class TransferDetails00
{
    public int TransferId { get; set; }

    public string? TransferSequence { get; set; }

    public int? ActionId { get; set; }

    public int? EmpId { get; set; }

    public int? BranchId { get; set; }

    public int? DepartmentId { get; set; }

    public int? BandId { get; set; }

    public int? GradeId { get; set; }

    public int? DesignationId { get; set; }

    public int? ProxyId { get; set; }

    public int? FirstEntity { get; set; }

    public string? EmpEntity { get; set; }

    public int? LastEntity { get; set; }

    public int? OldLastEntity { get; set; }

    public DateTime? ToDate { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? FlowStatus { get; set; }

    public string? EntryFrom { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? IsDirect { get; set; }

    public int? IsFutureDate { get; set; }

    public int? IsUpload { get; set; }

    public int? TransferBatchId { get; set; }

    public int? CompanyAccomodation { get; set; }

    public int? RelocationAllowance { get; set; }

    public DateTime? RelievingDate { get; set; }

    public int? Hraeligible { get; set; }

    public int? RelocationLeave { get; set; }

    public double? LeaveCount { get; set; }

    public int? CompanyTransferStatus { get; set; }

    public string? Remarks { get; set; }

    public string? TransferRemarks { get; set; }

    public int? RevokeStatus { get; set; }

    public int? RevokedBy { get; set; }

    public DateTime? RevokedDate { get; set; }

    public DateTime? HistoryFromDate { get; set; }
}
