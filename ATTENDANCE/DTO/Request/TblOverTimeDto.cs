namespace ATTENDANCE.DTO.Request
{
    public class TblOverTimeDto
    {
        public int AttendancePolicyId { get; set; }             // maps to @Error_Message
        public int? OverTimeTypeId { get; set; }                // maps to OverTimeID
        public decimal? Maximum { get; set; }                   // maps to Maximum
        public decimal? Minimum { get; set; }                   // maps to Minimum
        public string? WeekDay { get; set; }
    }
}
