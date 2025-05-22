using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class ManualOverTime00
{
    public int Id { get; set; }

    public string? ManualOtsbatch { get; set; }

    public int? EmployeeId { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? FlowStatus { get; set; }

    public int? TransactionId { get; set; }

    public bool? IsUpload { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? PayrollPeriod { get; set; }

    public int? SubPayPeriod { get; set; }

    public int? EntryWise { get; set; }

    public int? Year { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public string? SubmitRemarks { get; set; }

    public int? SubmitBy { get; set; }

    public DateTime? SubmitDate { get; set; }
}
