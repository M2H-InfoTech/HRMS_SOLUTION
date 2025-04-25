namespace EMPLOYEE_INFORMATION.Models.Entity
{
    public class HrmLeaveMaster
    {
        public int LeaveMasterId { get; set; }

        public string? LeaveCode { get; set; }

        public string? Description { get; set; }

        public int? PayType { get; set; }

        public int? LeaveUnit { get; set; }

        public int? Active { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? Colour { get; set; }

        public int? DefaultUnpaid { get; set; }
    }
}
