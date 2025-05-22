using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class AdmPmsrating02
{
    public long RatingId { get; set; }

    public int? ReviewId { get; set; }

    public int? GoalAssignId { get; set; }

    public int? CasGoalAssignId { get; set; }

    public string? PersonType { get; set; }

    public int? EvaluationId { get; set; }

    public int? ReviewerId { get; set; }

    public int? RatingTypeId { get; set; }

    public int? KpiRemarkId { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? ReviewRemark { get; set; }

    public string? Remarksdate { get; set; }

    public int? BadgeId { get; set; }

    public int? EmpId { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? RevRequestId { get; set; }

    public string? ReviewStatus { get; set; }

    public string? FlowStatus { get; set; }

    public string? Achievement { get; set; }

    public bool? IsReviewedByEmp { get; set; }

    public bool? IsReviewedByApprover { get; set; }

    public string? EmpReviewRemark { get; set; }

    public bool? IsReviewedBySkipLevel { get; set; }

    public int? FinancailId { get; set; }

    public int? ReviewdllId { get; set; }

    public string? RecomReason { get; set; }

    public string? RecomIds { get; set; }

    public bool? IsRejectBySkipLevel { get; set; }

    public string? RejectReason { get; set; }

    public bool? IsRejectByManager { get; set; }

    public string? RejectReasonManager { get; set; }

    public bool? IsGoalrejectBySkip { get; set; }

    public string? GoalrejectReasonBySkip { get; set; }

    public int? TrRating { get; set; }

    public string? TalentReviewRemark { get; set; }

    public bool? IsTalentReviewApprove { get; set; }

    public bool? IsTalentReviewReject { get; set; }

    public string? IsTalentReviewRejectReason { get; set; }

    public int? TalentReviewEmpId { get; set; }

    public DateTime? TalentReviewDate { get; set; }

    public int? TrApproverEmpId { get; set; }

    public int? IsAcceptedByEmployee { get; set; }

    public int? IsRejectedByEmployee { get; set; }

    public string? RevertRejectReason { get; set; }

    public int? IsRejectRevert { get; set; }

    public byte[]? FileImage { get; set; }

    public string? FileName { get; set; }

    public string? FileType { get; set; }

    public string? RejectedByEmployeeReason { get; set; }

    public int? ReviewAchievement { get; set; }
}
