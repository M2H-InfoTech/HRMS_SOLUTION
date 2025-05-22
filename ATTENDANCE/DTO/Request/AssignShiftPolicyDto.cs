namespace ATTENDANCE.DTO.Request
{
    public class AssignShiftPolicyDto
    {
        public int AttendanceAccessID { get; set; }
        public string ShiftIDs { get; set; }
        public string EmployeeIDs { get; set; }
        public DateTime ValidDatefrom { get; set; }
        public int levelId { get; set; }
        public int entryBy { get; set; }
        public DateTime validDateTo { get; set; }
    }
}
