using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class TravelApplication
{
    public int TravelAppId { get; set; }

    public string? RequestId { get; set; }

    public int? EmployeeId { get; set; }

    public int? TicketType { get; set; }

    public int? PassengerNationality { get; set; }

    public long? MobileNumberIi { get; set; }

    public string? Email { get; set; }

    public int? TravelType { get; set; }

    public int? SourceId { get; set; }

    public int? DestinationId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int? TotalDays { get; set; }

    public int? TravelModeId { get; set; }

    public int? SpanofJourneyId { get; set; }

    public int? AccomponiedBy { get; set; }

    public int? ApprovalRecommended { get; set; }

    public string? Accomodation { get; set; }

    public string? AccomodationDetails { get; set; }

    public string? ReturnTicket { get; set; }

    public string? Purpose { get; set; }

    public string? TravelAdvance { get; set; }

    public int? AdvanceDetails { get; set; }

    public int? VisaId { get; set; }

    public int? PassportId { get; set; }

    public string? OtherDetails { get; set; }

    public int? InstId { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? IdproxyTravel { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? SequenceId { get; set; }

    public string? EntryFrom { get; set; }

    public string? FlowStatus { get; set; }

    public string? DeskStatus { get; set; }

    public DateTime? EffectiveFromDate { get; set; }

    public DateTime? EffectiveToDate { get; set; }

    public string? DeskRemark { get; set; }

    public int? EffectiveTotalDays { get; set; }

    public int? PurposeId { get; set; }

    public string? CancelStatus { get; set; }

    public string? CancelFlowStatus { get; set; }

    public int? RtSpanofJourneyId { get; set; }

    public int? RtTravelModeId { get; set; }

    public string? CostCenterNumber { get; set; }

    public int? CostCenterId1 { get; set; }

    public int? CostCenterId2 { get; set; }

    public int? AdvanceCurrency { get; set; }

    public string? CompoOffTypes { get; set; }

    public int? PassageLeaveId { get; set; }

    public int? PassageCashBalance { get; set; }

    public string? CashBalanceRemark { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? FlightTime { get; set; }

    public string? FlightTimeRet { get; set; }

    public DateTime? IssuedDate { get; set; }

    public int? PaymentType { get; set; }

    public int? Invoice { get; set; }

    public string? Cancelled { get; set; }

    public int? IsExtended { get; set; }

    public int? OldTravelAppId { get; set; }

    public string? PreferedLocation { get; set; }

    public int? IsSpecialWorkFlow { get; set; }

    public int? AirTicketAccrualId { get; set; }

    public int? ClosedStatus { get; set; }
}
