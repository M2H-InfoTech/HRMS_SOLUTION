namespace OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO.Response
{
    public class AttLogListResponseDto
    {
        public long AttLogId { get; set; }
        public string? EmpCode { get; set; }
        public DateTime? LogDate { get; set; }  // Actual log date
        public DateTime? EntryDate { get; set; } // Updated or Downloaded date
        public string? DummyDate { get; set; } // yyyyMMdd formatted date
        public string? EntryBy { get; set; }  // Either "Device" or "Manual"
        public string? Direction { get; set; }  // IN / OUT
        public string? PunchTime { get; set; } // HH:mm format
    }
}
