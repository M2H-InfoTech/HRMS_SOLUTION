namespace LEAVE.Dto
{
    public class LeaveDetailModelDto
    {
        public string? UserName { get; set; }
        public int LeaveMasterId { get; set; }
        public string? LeaveCode { get; set; }
        public string? Description { get; set; }
        public int? PayType { get; set; }
        public int? LeaveUnit { get; set; }
        public int? Active { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
