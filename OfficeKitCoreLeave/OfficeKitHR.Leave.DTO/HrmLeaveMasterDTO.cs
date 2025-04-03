namespace OFFICEKITCORELEAVE.OfficeKit.Leave.DTO
{
    public class HrmLeaveMasterDTO
    {
        public int LeaveMasterId { get; set; }

        public string? LeaveCode { get; set; }

        public string? Description { get; set; }

        public int? PayType { get; set; }

        public int? LeaveUnit { get; set; }

        public int? Active { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

        public string? Colour { get; set; }

        public int? DefaultUnpaid { get; set; }
    }
    public class HrmLeaveMasterSearchDto
    {
        public int LeaveMasterId { get; set; }
        public int? RoleId { get; set; }
        public int? employeeId { get; set; }
    }
    public class HrmLeaveMasterViewDto
    {
        public string Username { get; set; }
        public int LeavemasterId { get; set; }
        public string LeaveCode { get; set; }
        public string? Description { get; set; }
        public int? PayType { get; set; }
        public int? LeaveUnit { get; set; }
        public int? Active { get; set; }
        public int? CreatedDate { get; set; }
    }
}
