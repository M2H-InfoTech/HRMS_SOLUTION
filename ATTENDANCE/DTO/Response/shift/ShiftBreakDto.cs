namespace ATTENDANCE.DTO.Response.shift
{
    public class ShiftBreakDto
    {
        public decimal? StartTime { get; set; }
        public decimal? EndTime { get; set; }
        public int Shift02Id { get; set; }
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
