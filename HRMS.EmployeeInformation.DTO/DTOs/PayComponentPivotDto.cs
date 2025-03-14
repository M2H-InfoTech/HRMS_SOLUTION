namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class PayComponentPivotDto
    {
        public int PayRequestId01 { get; set; }
        public Dictionary<string, decimal> PayCodeAmounts { get; set; } = new Dictionary<string, decimal>();
    }
}
