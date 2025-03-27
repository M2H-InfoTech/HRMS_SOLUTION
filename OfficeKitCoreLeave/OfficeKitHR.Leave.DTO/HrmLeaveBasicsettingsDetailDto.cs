namespace OFFICEKITCORELEAVE.OfficeKitHR.Leave.DTO
{
    public class HrmLeaveBasicsettingsDetailDto
    {
        public int masterId { get; set; }
        public int headproraid { get; set; }
        public string regtype { get; set; }
        public string entitlement {  get; set; }
        public int SettingsDetailsId { get; set; }

        public int? SettingsId { get; set; }

        public int? Lopcheck { get; set; }

        public int? Gender { get; set; }

        public int? MaritalStatus { get; set; }

        public int? Carryforward { get; set; }

        public decimal? RolloverCount { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? OndemandLeaveGrand { get; set; }

        public int? EligibilityPeriod { get; set; }

        public int? PredatedApplication { get; set; }

        public int? Maternity { get; set; }

        public int? Compensatory { get; set; }

        public decimal? MinServiceDays { get; set; }

        public decimal? CsectionMaxLeave { get; set; }

        public int? EligibleCount { get; set; }

        public int? LeaveInclude { get; set; }

        public decimal? CompCaryfrwrd { get; set; }

        public int? Carryforwardtype { get; set; }

        public int? Defaultreturndate { get; set; }

        public int? Attachment { get; set; }

        public int? Salaryadvance { get; set; }

        public int? Roledeligation { get; set; }

        public int? LeaveType { get; set; }

        public int? Weeklyleaveday { get; set; }

        public int? Ishalfday { get; set; }

        public int? Returnrequest { get; set; }

        public double? LeavedaysSalaryadvance { get; set; }

        public double? SalaryadvanceApplybeforedays { get; set; }

        public int? Attachmentmandatory { get; set; }

        public int? Applywithoutbalance { get; set; }

        public int? Casualholiday { get; set; }

        public int? PassageeligibilityEnable { get; set; }

        public double? Passageeligibilitydays { get; set; }

        public int? PassportRequest { get; set; }

        public int? EnableLeaveGrander { get; set; }

        public int? Autocarryforward { get; set; }

        public int? Yearlylimit { get; set; }

        public double? Yearlylimitcount { get; set; }

        public int? CarryforwardbasedonCategory { get; set; }

        public int? YearlylimitbasedonCategory { get; set; }

        public int? MaxinstanceLimitbasedonCategory { get; set; }

        public int? Blockpreviouslap { get; set; }

        public int? ApplicableOnnotice { get; set; }

        public int? LeaveEncashment { get; set; }

        public int? Disableyearlylimit { get; set; }

        public int? Allowleavecancel { get; set; }

        public int? Passportrequireddays { get; set; }

        public int? LeaveAccrual { get; set; }

        public int? SalaryholduptoRejoin { get; set; }

        public int? Leavebalanceroundoption { get; set; }

        public int? ShowcurrentmonthWeekoff { get; set; }

        public int? Invisible { get; set; }

        public int? ApplyForFuture { get; set; }

        public int? LeaveEncashmentMnthly { get; set; }

        public int? LeaveReductionForLateIn { get; set; }

        //HrmLeaveBasicsettingsDetailsHistoryDto

        public long SettingsHistoryId { get; set; }

        public int? EmployeeId { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? Ipaddress { get; set; }

        //HrmLeaveEntitlementHeadDto
        public int LeaveEntitlementId { get; set; }

        public int? EmployeeType { get; set; }

        public decimal? AllemployeeLeaveCount { get; set; }

        public int? DateofJoiningCheck { get; set; }

        public int? JoinedDate { get; set; }

        public decimal? LeaveCount { get; set; }


        public int? LeaveGrantType { get; set; }

        public int? Experiance { get; set; }

        public int? Monthwise { get; set; }

        public int? NewjoinGranttype { get; set; }

        public int? NewjoinLeavecount { get; set; }

        public int? NewjoinMonthwise { get; set; }

        public decimal? Laps { get; set; }

        public DateTime? StartDate { get; set; }

        public int? Eligibility { get; set; }

        public int? EligibilityGrant { get; set; }


        public int? Cfbasedon { get; set; }

        public decimal? Rollovercount { get; set; }

        public int? CarryforwardNj { get; set; }

        public int? CfbasedonNj { get; set; }

        public decimal? RollovercountNj { get; set; }

        public double? Firstmonthleavecount { get; set; }

        public int? Credetedon { get; set; }

        public double? Yearcount { get; set; }

        public double? JoinmonthdayaftrNyear { get; set; }

        public double? JoinmonthleaveaftrNyear { get; set; }

        public int? Beginningcarryfrwrd { get; set; }

        public int? Vacationaccrualtype { get; set; }

        public int? Previousexperiance { get; set; }

        public int? GrantfullleaveforAll { get; set; }

        public int? Settingspaymode { get; set; }

        public double? PartialpaymentBalancedays { get; set; }

        public double? PartialpaymentNextcount { get; set; }

        public double? LeaveHours { get; set; }

        public int? LeaveCriteria { get; set; }

        public int? CalculateOnFirst { get; set; }

        public int? CalculateOnSecond { get; set; }

        public double? LeaveHoursNewjoin { get; set; }

        public int? LeaveCriteriaNewjoin { get; set; }

        public int? CalculateOnFirstNewjoin { get; set; }

        public int? CalculateOnSecondNewjoin { get; set; }

        public double? ExpactLeaveCount { get; set; }

        public int? LeavefromProbationDt { get; set; }

        public int? NyearBasedOnJoinDate { get; set; }

        public int? ConsiderProbationDate { get; set; }

        public int? FullleaveProRata { get; set; }

        public double? FullleaveProRataLeaveCount { get; set; }

        public decimal? ExtraLeaveCountProxy { get; set; }

        public int? IsShowPartialPaymentDays { get; set; }

        //HrmLeaveEntitlementRegDto
        public int LeaveentitlementregId { get; set; }


        public int? LeaveCondition { get; set; }

        public int? Year { get; set; }

        public int? Month { get; set; }

        public decimal? Count { get; set; }

        public int? Newjoin { get; set; }

        //HrmLeaveMasterandsettingsLinkDto
        public int IdMasterandSettingsLink { get; set; }

        public int? LeaveMasterId { get; set; }

        //HrmLeavePartialPaymentDto
        public int PartialpaymentId { get; set; }

        public int? ExperiancetabId { get; set; }

        public decimal? Daysfrom { get; set; }

        public decimal? Daysto { get; set; }

        public decimal? PayPercentage { get; set; }

        public int? NewjnStatus { get; set; }

        public int? Createdby { get; set; }

        public int? Ondemandpartial { get; set; }

        public int? Initialcount { get; set; }

        //HrmLeaveServicedbasedleaveDto

        public int IdServiceLeave { get; set; }

        public double? FromYear { get; set; }

        public double? ToYear { get; set; }

        public int? ExperiancebasedGrant { get; set; }

        public decimal? Experiancebasedrollover { get; set; }

        public int? Checkcase { get; set; }

        public int? ExperiancebasedVacation { get; set; }


    }
}
