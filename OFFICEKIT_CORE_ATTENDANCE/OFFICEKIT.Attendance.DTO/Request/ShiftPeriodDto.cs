namespace OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO.Request
{
    public class ShiftPeriodDto
    {
        public string ShiftCode { get; set; }
        public int ShiftId { get; set; }
        public string ShiftName { get; set; }
        public string ShiftType { get; set; }
        public decimal StartTime { get; set; }
        public decimal EndTime { get; set; }
        public int PeriodNum { get; set; }
    }

}
