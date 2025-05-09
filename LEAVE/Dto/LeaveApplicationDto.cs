namespace LEAVE.Dto
{
    public class LeaveApplicationDto
    {
        public string CaseCode { get; set; }
        public string LeaveCode { get; set; }
        public string Description { get; set; }
        public int LeaveApplicationId { get; set; }
        public DateTime ?LeaveFrom { get; set; }
        public DateTime? LeaveTo { get; set; }
        public decimal NoOfLeaveDays { get; set; }
        public string Reason { get; set; }
        public int? TimeMode { get; set; }
        public string Daytype { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int? IDProxyLeave { get; set; }
        public string ApprovalStatus { get; set; }
        public string Name { get; set; }
        public int EmployeeId { get; set; }
        public string ApprovalStatusDesc { get; set; }
        public string RequestId { get; set; }
        public string FileName { get; set; } = "";
        public decimal CancelledDays { get; set; }
        public int LeaveCancelId { get; set; }
    }
}
