namespace LEAVE.Models;

public partial class LeavePolicyWeekendInclude
{
    public int WeekendIncludeId { get; set; }

    public int? LeavePolicyInstanceLimitId { get; set; }

    public int? Weekendstatus { get; set; }

    public decimal? Fromdays { get; set; }

    public decimal? Todays { get; set; }

    public int? Leavetype { get; set; }

    public int? Createdby { get; set; }

    public DateTime? CreatedDate { get; set; }

    public decimal? LeaveDays { get; set; }

    public int? OffdaysIncExc { get; set; }
}
