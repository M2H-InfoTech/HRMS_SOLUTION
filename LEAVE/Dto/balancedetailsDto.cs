namespace LEAVE.Dto
{
    public class balancedetailsDto
    {
        public int? EmpId { get; set; }
        public string? EmpCode { get; set; }
        public string? EmpName { get; set; }
        public string? Name { get; set; }
        public int? LeaveMasterId { get; set; }
        public string? LeaveCode { get; set; }
        public string? Description { get; set; }
        public decimal? LeaveCredited { get; set; }
        public decimal? Accrued { get; set; }
        public decimal? Leavebalance { get; set; }
        public string? LeaveTaken { get; set; }
        public string? Used { get; set; }
        public string? Granted { get; set; }
        public string? Colour { get; set; }
        public double? MonthlyLimit { get; set; }
        public string? lapsedleaves { get; set; }
    }
}
