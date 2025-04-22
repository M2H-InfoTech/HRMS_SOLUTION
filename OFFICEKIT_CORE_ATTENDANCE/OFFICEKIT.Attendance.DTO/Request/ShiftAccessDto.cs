namespace OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO.Request
{
    public class ShiftAccessDto
    {
        public int ShiftAccessId { get; set; }
        public int EmpId { get; set; }
        public string EmployeeName { get; set; }
        public int ShiftId { get; set; }
        public string ShiftName { get; set; }
        public string ShiftCodeName { get; set; }
        public string WeekName { get; set; }
        public int EmployeeId { get; set; }
        public int? WeekEndMasterID { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public DateTime ShiftStartEndTime { get; set; }
        public string ValidDatefrom { get; set; }
        public string ValidDateTo { get; set; }
        public string Branch { get; set; }
        public string Designation { get; set; }
        public string ProjectName { get; set; }
        
    }

}
