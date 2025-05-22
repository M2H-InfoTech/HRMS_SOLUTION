using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class AssignGoals00
{
    public int GoalassignId { get; set; }

    public int? KpiId { get; set; }

    public int? GoalId { get; set; }

    public double? Weightage { get; set; }

    public int? Goalcascade { get; set; }

    public int? Expectation { get; set; }

    public int? Autoaccept { get; set; }

    public string? Period { get; set; }

    public long? EvaluationTypeId { get; set; }

    public int? EmployeeId { get; set; }

    public string? Target { get; set; }

    public int? WorkedUom { get; set; }

    public DateOnly? Startdate { get; set; }

    public DateOnly? Enddate { get; set; }

    public string? BalanceGoal { get; set; }

    public long? AssignId { get; set; }

    public bool? IsFileUpload { get; set; }

    public string? FileName { get; set; }

    public byte[]? FileImage { get; set; }

    public string? FileType { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? Entrydate { get; set; }

    public int? Modifiedby { get; set; }

    public DateTime? Modifieddate { get; set; }

    public string? AcceptReject { get; set; }

    public int? AssignerId { get; set; }

    public int? CascadeGoalAssignId { get; set; }

    public string? CascadeTaskDesc { get; set; }

    public string? RejectRemark { get; set; }

    public DateTime? AcceptRejectDatetime { get; set; }

    public bool? Badge { get; set; }

    public string? BadgeRemark { get; set; }

    public string? TaskStatus { get; set; }

    public string? ReporteeType { get; set; }

    public int? GoalLevel { get; set; }

    public string? GoalGroup { get; set; }

    public int? LastValue { get; set; }

    public int? MainGoalId { get; set; }

    public int? GoalTypeId { get; set; }

    public string? GoalActiveStatus { get; set; }

    public int? DurationInDays { get; set; }

    public string? GoalRequestId { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? FlowStatus { get; set; }

    public string? RequesterRemark { get; set; }

    public string? NewGoalCreatedStatus { get; set; }

    public string? NewActionRemarks { get; set; }

    public string? NewActionStatus { get; set; }

    public int? FinancailId { get; set; }

    public int? PmsfinanId { get; set; }

    public bool? IsUpload { get; set; }

    public int? IsSelf { get; set; }

    public int? Category { get; set; }

    public string? Description { get; set; }

    public DateTime? ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public int? IsSubmit { get; set; }

    public int? YomType { get; set; }

    public int? Masterid { get; set; }

    public int? YomBit { get; set; }
}
