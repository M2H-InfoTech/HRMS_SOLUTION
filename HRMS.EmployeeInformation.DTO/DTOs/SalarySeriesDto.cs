namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class SalarySeriesDto
    {
        public int EmpId { get; set; }
        public int PayRequestId { get; set; }
        public int PayRequest01Id { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Branch { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal TotalPay { get; set; }
        public string EffectiveDate { get; set; }
        public string CurrencyCode { get; set; }
        public bool IsArrears { get; set; }
        public Dictionary<string, decimal> PayComponent { get; set; }
        public string PaycodeBatch { get; set; }
        public string PayPeriod { get; set; }
        public string? Remarks { get; set; }
    }
    public class PayComponentPivotDto
    {
        public int PayRequestId01 { get; set; }
        public Dictionary<string, decimal> PayCodeAmounts { get; set; } = new Dictionary<string, decimal>();
    }


}
