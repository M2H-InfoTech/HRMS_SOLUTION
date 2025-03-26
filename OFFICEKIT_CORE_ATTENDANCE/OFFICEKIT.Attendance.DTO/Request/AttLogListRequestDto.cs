namespace OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO.Request
{
    public class AttLogListRequestDto
    {
        public int EmpId { get; set; }  // Employee ID to filter logs
        public DateTime? FromDate { get; set; }  // Start date (expected in "yyyy-MM-dd" format)
        public DateTime? ToDate { get; set; }  // End date (expected in "yyyy-MM-dd" format)
    }
}
