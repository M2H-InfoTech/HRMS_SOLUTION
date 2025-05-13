namespace ATTENDANCE.DTO.Response.shift
{
    public class ShiftSeasonBreakDto
    {
        public int ShiftSeason02Id { get; set; }
        public int? ShiftId { get; set; }
        public int? BreakStartTypeID { get; set; }
        public string BreakStartType { get; set; }
        public string BreakStartTime { get; set; }
        public int? BreakEndTypeID { get; set; }
        public string BreakEndTime { get; set; }
        public string BreakEndType { get; set; }
        public decimal? TotalBreakHours { get; set; }
        public string EffectiveFrom { get; set; }
        public string IsPaid { get; set; }
    }

}
