namespace ATTENDANCE.DTO.Request
{
    public class SpecialOvertimeDto
    {
        public int OverTimeTypeId { get; set; }
        public decimal? Maximum { get; set; }
        public decimal? Minimum { get; set; }
        public string? WeekDay { get; set; }
        public decimal? StartTime { get; set; }
        public decimal? EndTime { get; set; }
        public int? PolicyDayType { get; set; }
    }
}
