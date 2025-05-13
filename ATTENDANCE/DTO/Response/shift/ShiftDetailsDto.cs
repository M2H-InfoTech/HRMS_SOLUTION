namespace ATTENDANCE.DTO.Response.shift
{
    public class ShiftDetailDto
    {
        public int Shift01Id { get; set; }
        public int? ShiftId { get; set; }
        public int? ShiftStartTypeID { get; set; }
        public string ShiftStartType { get; set; }
        public double? FirstHalf { get; set; }
        public double? SecondHalf { get; set; }
        public int? ShiftEndTypeID { get; set; }
        public string ShiftEndType { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public decimal? TotalHours { get; set; }
        public string EffectiveFrom { get; set; }
        public decimal? MinimumWorkHours { get; set; }
    }
}
