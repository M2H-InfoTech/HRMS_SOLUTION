using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models;

public partial class AssignedLetterType
{
    public long LetterReqId { get; set; }

    public string? RequestCode { get; set; }

    public int? LetterTypeId { get; set; }

    public string? Remark { get; set; }

    public string? ValidFrom { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? FlowStatus { get; set; }

    public bool? IsLetterAttached { get; set; }

    public string? UploadFileName { get; set; }

    public byte[]? FileImage { get; set; }

    public string? FileType { get; set; }

    public bool? Active { get; set; }

    public int? EmpId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? TemplateStyle { get; set; }

    public string? ApproverRemark { get; set; }

    public string? FinalApproverRemark { get; set; }

    public int? LetterSubType { get; set; }

    public int? IsLiability { get; set; }

    public int? LiabilityReqId { get; set; }

    public int? IsLiabilitySubmitted { get; set; }

    public byte[]? FileData { get; set; }

    public string? LiabilityLetter { get; set; }

    public int? IsProxy { get; set; }

    public int? Language { get; set; }

    public int? Uploaded { get; set; }

    public DateTime? ReleaseDate { get; set; }

    public int? IsDirectPosting { get; set; }

    public DateTime? IssueDate { get; set; }

    public DateTime? FinalApprovalDate { get; set; }

    public int? FinalApproverId { get; set; }
}
