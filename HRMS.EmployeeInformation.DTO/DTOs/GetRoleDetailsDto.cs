namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class RoleDetailsDto
    {
        public string? Mode { get; set; }
        public int SecondEntityId { get; set; }
        public List<int>? EntityList { get; set; }
        public int FirstEntityId { get; set; }
        public int LinkableCategoryID { get; set; }
        public int EmpId { get; set; }
        public string? Role { get; set; }
        public string? Code { get; set; }
        public List<int>? AddList { get; set; }
        public List<int>? EditList { get; set; }
        public List<int>? RoleIdList { get; set; }
        public List<int>? EntityList1 { get; set; }
        public List<int>? CommonList { get; set; }
        public string? Level { get; set; }
        public string? LinkID { get; set; }
        public int PayRollPeriodID { get; set; }
        public bool Active { get; set; }
        public string? Prefix { get; set; }
        public int CreatedBy { get; set; }
        public int InstId { get; set; }
        public string? TransactionType { get; set; }
        public int OldRole { get; set; }
    }
}
