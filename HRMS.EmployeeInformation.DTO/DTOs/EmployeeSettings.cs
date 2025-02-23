

namespace MPLOYEE_INFORMATION.DTO.DTOs
{
    public class EmployeeSettings
    {
        public string? ActiveStatus { get; set; }

        public string? OnNotice { get; set; }

        public string? ValueType { get; set; }

        public string? ParameterCode { get; set; }

        public string? ReasonType { get; set; }

        public string? Monthly { get; set; }
        public string? DailyWage { get; set; }

        public string? UserSettings { get; set; }

        public string[]? ActiveStatusCodes { get; set; }

        public string[]? Extended { get; set; }

        public string? Sep { get; set; }

        public string? EmpStatus { get; set; }

        public string? Statuses { get; set; }
        public string? EmployeeReportingType { get; set; }

        public string? companyParameterCodes { get; set; }
        public string? companyParameterCodesType { get; set; }

        public string? CompanyParameterEmpInfoFormat { get; set; }
        public string? CompanyParameterType { get; set; }

        public string? DateFormat { get; set; }
        public string? EmployeeStatus { get; set; }

        public int? LinkLevel { get; set; } = 15;
        public string? NotAvailable { get; set; }

        public string? Hobbies { get; set; }
        public string? DataInsertSuccessStatus { get; set; }
        public string? DataInsertFailedStatus { get; set; }
        public string? PayrollPeriodScheme { get; set; }
        public string? PayrollPeriodType { get; set; }
        public string? PayrollBatchScheme { get; set; }
    }

}

