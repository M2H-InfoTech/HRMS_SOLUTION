namespace ATTENDANCE.DTO.Response
{
    public class AttendancePolicyAccessDto
    {
        public int AttendanceAccessId { get; set; }
        public int EmpId { get; set; }
        public string EmployeeName { get; set; }
        public int AttendancePolicyId { get; set; }
        public string PolicyName { get; set; }
        public int? EmployeeId { get; set; }
        public string Designation { get; set; }
        public string EmployeeCode { get; set; }
        public string Datefrom { get; set; }
        public string ValidDteFrm { get; set; }
        public string DateTo { get; set; }
        public string ValidDteTo { get; set; }

        public string ValidDatefrom { get; set; }
        public string ValidDateTo { get; set; }
    }

}
