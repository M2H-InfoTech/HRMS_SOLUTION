namespace ATTENDANCE.DTO.Request
{
    public class ViewEmployeeShiftPolicyDto
    {
        public int entryBy { get; set; }
        public int roleId { get; set; }
        public string? EmployeeIDs { get; set; }
        
        public int? AttendanceAccessID { get; set; }
    }
}
