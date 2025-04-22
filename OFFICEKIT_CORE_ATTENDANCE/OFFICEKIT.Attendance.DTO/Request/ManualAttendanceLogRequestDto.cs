namespace OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO.Request
{
    public class ManualAttendanceLogRequestDto
    {
        public int EmployeeId { get; set; }
        public DateTime LogDate { get; set; } // Date with time
        public string Direction { get; set; }
    }
}
