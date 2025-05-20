namespace LEAVE.Dto
{
    public class LeaveEntitlementDto
    {
        public int MasterId { get; set; }
        public int? BasicSettingsId { get; set; }
        public int? CreatedBy { get; set; }
        public int? HeadProrataId { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public decimal? LeaveCount { get; set; }
        public string? RegType { get; set; }
        public int? GrantType { get; set; }
        public int? Rollover { get; set; }
        public int? CheckCase { get; set; }
        public int? VacationType { get; set; }
        public int? EmpType { get; set; }
        public decimal? AllEmployeeLeave { get; set; }
        public int? NewJoin { get; set; }
        public int? JoinDate { get; set; }
        public decimal? NewJoinLeaveCount { get; set; }
        public int? Experience { get; set; }
        public int? Monthwise { get; set; }
        public int? NJNGrantType { get; set; }
        public int? NJNLeaveCount { get; set; }
        public int? NJNMonthwise { get; set; }
        public decimal? Laps { get; set; }
        public DateTime? GrantingStartDate { get; set; }
        public int? Eligibility { get; set; }
        public int? EligibilityGrant { get; set; }
        public int? CarryForward { get; set; }
        public int? CarryForwardType { get; set; }
        public int? CarryForwardNJ { get; set; }
        public int? CFBasedOnNJ { get; set; }
        public int? RolloverCountNJ { get; set; }
        public double? FirstMonthLeaveCount { get; set; }
        public int? CreditedOn { get; set; }
        public int? ProbationML { get; set; }
        public int? NewJoinML { get; set; }
        public int? OtherML { get; set; }
        public int? BeginningCarryForward { get; set; }
        public int? PreviousExperience { get; set; }
        public int? GrantFullLeave { get; set; }
        public int? FullLeaveProRata { get; set; }
        public int? SettingsPayMode { get; set; }
        public double? PartialPaymentBalance { get; set; }
        public double? PartialPaymentNextCount { get; set; }
        public double? LeaveHours { get; set; }
        public int? LeaveCriteria { get; set; }
        public int? CalculateOnFirst { get; set; }
        public int? CalculateOnSecond { get; set; }
        public double? LeaveHoursNj { get; set; }
        public int? LeaveCriteriaNj { get; set; }
        public int? CalculateOnFirstNj { get; set; }
        public int? CalculateOnSecondNj { get; set; }
        public int? NYearAfterJoinDate { get; set; }
        public int? ConsiderProbationDate { get; set; }
        public int? ShowInLeaveBalance { get; set; }
        public decimal? ExtraLeaveCountProxy { get; set; }
    }
}
