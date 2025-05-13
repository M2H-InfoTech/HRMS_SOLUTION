namespace ATTENDANCE.DTO.Response
{
    public class BreakTimeDto
    {
        public int BreakStartType { get; set; }
        public decimal BreakStartTime { get; set; }
        public int BreakEndType { get; set; }
        public decimal BreakEndTime { get; set; }
        public decimal TotalBreakHours { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public string IsPaid { get; set; }  // Consider changing to bool if possible
        public decimal ShiftStartTime { get; set; }
        public decimal ShiftEndTime { get; set; }
    }

}
