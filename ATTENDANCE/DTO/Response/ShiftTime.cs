namespace ATTENDANCE.DTO.Response
{
    public class ShiftTime
    {
        public int ShiftStartType { get; set; }
        public decimal ShiftStartTime { get; set; }
        public int ShiftEndType { get; set; }
        public decimal ShiftEndTime { get; set; }
        public decimal TotalWorkHours { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public decimal MinimumWorkHours { get; set; }
        public float FirstHalf { get; set; }
        public float SecondHalf { get; set; }
    }
}