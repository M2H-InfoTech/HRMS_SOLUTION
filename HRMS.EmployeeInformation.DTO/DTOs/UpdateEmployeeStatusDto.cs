namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class UpdateEmployeeStatusDto
    {
        public string EmpIDs { get; set; } = string.Empty;
        public int FirstEntityID { get; set; } = 0;
        public int EntryBy { get; set; } = 0;
        public string Status { get; set; } = "0";
        public string IsLeaveMod { get; set; } = string.Empty;
        public string IsAttendMod { get; set; } = string.Empty;
        public string IsHolydayMod { get; set; } = string.Empty;
        public DateTime ValidFrom { get; set; }
    }
}
