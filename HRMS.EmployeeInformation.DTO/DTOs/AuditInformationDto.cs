namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class AuditInformationDto
    {
        public int? InfoID { get; set; }
        public int? Info01ID { get; set; }
        public int? EmpID { get; set; }
        public string? EmpCode { get; set; }
        public string? Name { get; set; }
        public string? InformationType { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public string? UpdatedBy { get; set; }
        public string? EffectiveFrom { get; set; }
        public string? EffectiveTime { get; set; }

    }
}
