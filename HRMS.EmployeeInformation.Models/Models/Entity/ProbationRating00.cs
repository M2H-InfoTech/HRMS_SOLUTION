namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class ProbationRating00
{
    public int ProbRateId { get; set; }

    public int? EmpId { get; set; }

    public int? TotalStar { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? Remark { get; set; }

    public string? RequestCode { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? FlowStatus { get; set; }

    public string? RequesterEmpId { get; set; }

    public string? TrainingRequiredIds { get; set; }

    public int? CurrentRecord { get; set; }

    public DateTime? FinalApprovalDate { get; set; }

    public DateTime? NextReviewDate { get; set; }

    public string? FinalReviewStatus { get; set; }

    public int? ReviewCountEval { get; set; }

    public int? ManualApprover { get; set; }

    public DateTime? ManualApproveDate { get; set; }
}
